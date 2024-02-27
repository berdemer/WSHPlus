using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ISG.Entities.ComplexType;
using ISG.WebApi.Infrastructure;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ISG.WebApi.Controllers
{
	//[Authorize]
	[RoutePrefix("api/azurDepo")]
	
	public class UploadAzureController : BaseApiController
	{
		private IImageUploadService _imageUploadService;

		public UploadAzureController(IImageUploadService imageUploadService)
		{
			_imageUploadService = imageUploadService;
		}


		[HttpDelete]
		[Route("{genericName}/{mime?}")]
		public async Task<IHttpActionResult> Delete(string genericName,string mime = "x")
		{
			try 
			{
				imageUploadFilter asd = new imageUploadFilter() { GenericName=mime!="x"?genericName+"."+mime: genericName };
				imageUpload imu=await _imageUploadService.FindAsync(asd);
				int iReturnValue = await _imageUploadService.DeleteAsync(imu.id);
				if (iReturnValue>0)
				{
					var context = new StorageContext();
					var blobContainer = context.BlobClient.GetContainerReference(imu.Folder);
					blobContainer.CreateIfNotExists();

					var blob = blobContainer.GetBlockBlobReference(asd.GenericName);

					var blobExists = await blob.ExistsAsync();
					if (!blobExists)
					{
						 return Content(HttpStatusCode.NotFound, await Task.Run(() => genericName+" Azur deposunda silinecek dosya bulunamadı."));
					}
					await  blob.DeleteIfExistsAsync();
					return Ok(iReturnValue);
				}
				else
				{
					return Ok(0);
				}
			}
			catch (Exception)
			{

				return Content(HttpStatusCode.NotFound, await Task.Run(() => "Sistemde silinecek dosya bulunamadı."));
			}
		}


		[HttpGet]
		[Route("{IdGuid}/{Folder}/{idx?}")]
		public async Task<List<FileUploadResult>> GetAll(string IdGuid, string Folder,int idx=0)
		{
			imageUploadFilter asd = new imageUploadFilter();
			var FUR = new List<FileUploadResult>();
			asd.IdGuid = IdGuid;
			asd.Folder = Folder;
			asd.Protokol = idx;
			IEnumerable<imageUpload> qw =idx>0? await _imageUploadService.FindAllFileAsync(asd):
				await _imageUploadService.FindAllAsync(asd);
			qw.ToList().ForEach(x => FUR.Add(new FileUploadResult()
			{
				FileLength = x.FileLenght,
				FileName = x.FileName,
				GenericName = x.GenericName,
				LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + x.Folder + "/" + x.GenericName
			}));
			return FUR;
		}


		[HttpPost]
		[Route("{IdGuid}/{Folder}/{idx?}")]
		public async Task<IHttpActionResult> Post(string IdGuid, string Folder,int idx=0)
		{
			var context = new StorageContext();
			var blobContainer = context.BlobClient.GetContainerReference(Folder);


			if (!await blobContainer.ExistsAsync())
			{
				await blobContainer.CreateAsync(BlobContainerPublicAccessType.Container, null, null);
			}

			var provider = new AzureStorageMultipartFormDataStreamProvider(blobContainer);
			try
			{
				await Request.Content.ReadAsMultipartAsync(provider);
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

			var fileInfo = provider.FileData.Select(i =>
			{
				var uploadFiles = new imageUpload();
				var info = new FileInfo(i.LocalFileName);
				uploadFiles.IdGuid = IdGuid;
				uploadFiles.Protokol = idx;
				uploadFiles.Folder = Folder;
				uploadFiles.FileLenght =0;
				var sdff = i.Headers.ContentDisposition.FileName;
				uploadFiles.FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1);
				//                       son karakter silindi                     başındaki karakter silindi.
				uploadFiles.GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1];
				//split array neslesine dönüşüp son elemanı aldım.
				uploadFiles.MimeType = i.Headers.ContentType.MediaType;
				uploadFiles.UserId = User.Identity.GetUserId();
				var result = _imageUploadService.AddAsync(uploadFiles);
				uploadFiles = null;
				return new FileUploadResult()
				{
					FileLength = 0,
					FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1),
					LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + Folder + "/" + info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1],
					GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1]
				};
			});
			return Ok(await Task.Run(() => fileInfo));

		}

 
		//http://localhost:1943/api/Img/down/cc81e63a-d184-4f61-a066-4e09ac78a3d4
		[AllowAnonymous]
		[HttpGet]
		[Route("down/{genericName}")]
		public async Task<HttpResponseMessage> Download(string genericName)
		{		
			HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				imageUploadFilter asd = new imageUploadFilter() { GenericName = genericName };
				imageUpload imu = await _imageUploadService.FindAsync(asd);

				var context = new StorageContext();
				var blobContainer = context.BlobClient.GetContainerReference(imu.Folder);
				blobContainer.CreateIfNotExists();

				var blob = blobContainer.GetBlockBlobReference(genericName);

				var blobExists = await blob.ExistsAsync();
				if (!blobExists)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Dosya Bulunamadı!");
				}

				HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
				Stream blobStream = await blob.OpenReadAsync();

				result.Content = new StreamContent(blobStream);
				result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
				result.Content.Headers.ContentDisposition.FileName = imu.FileName;
				result.Content.Headers.ContentType = new MediaTypeHeaderValue(imu.MimeType);
				result.Content.Headers.ContentLength = blobStream.Length;
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " İndirelemedi!"));
			}
			return result;
		}

	}
}

public class AzureStorageMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
{
	private readonly CloudBlobContainer _blobContainer;

	public AzureStorageMultipartFormDataStreamProvider(CloudBlobContainer blobContainer) : base("azure")
	{
		_blobContainer = blobContainer;
	}

	public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
	{
		if (parent == null) throw new ArgumentNullException(nameof(parent));
		if (headers == null) throw new ArgumentNullException(nameof(headers));

		string fileName;
		if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))//dosya adı boş değilse 
		{
			var ext = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//uzantısnı almak istiyoruz
			var isImg = headers.ContentType.MediaType.ToString().StartsWith("image");
			ext = !string.IsNullOrEmpty(ext) && isImg ? ext : string.IsNullOrEmpty(ext) ? ".png" : "";
			fileName = Guid.NewGuid() + ext;
		}
		else
		{
			fileName = Guid.NewGuid() + ".data";
		}

		CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);

		if (headers.ContentType != null)
		{
			// Set appropriate content type for your uploaded file
			blob.Properties.ContentType = headers.ContentType.MediaType;
		   
		}

		this.FileData.Add(new MultipartFileData(headers, blob.Name));

		return blob.OpenWrite();

	}
}

public class StorageContext
{
	private CloudStorageAccount _storageAccount;

	public StorageContext()
	{
		var accountName = ConfigurationManager.AppSettings["storage:account:name"];
		var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
		 _storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
	}

	public CloudBlobClient BlobClient
	{
		get { return _storageAccount.CreateCloudBlobClient(); }
	}

	public Microsoft.WindowsAzure.Storage.Table.CloudTableClient TableClient
	{
		get { return _storageAccount.CreateCloudTableClient(); }
	}
}