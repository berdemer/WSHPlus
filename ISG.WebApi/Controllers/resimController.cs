using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/resimler")]
	public class resimController : BaseApiController
	{
		private IImageUploadService _imageUploadService;
		private IPersonelService _personelService { get; set; }
		public resimController(IImageUploadService imageUploadService,
							   IPersonelService personelService
			)
		{
			_imageUploadService = imageUploadService;
			_personelService = personelService;
		}


		#region Resim için Excel Dosyasının Hazırlık Yeri
		private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");

		[Route("upload")]
		[HttpPost]
		public async Task<IHttpActionResult> UploadFile()
		{
			var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
			await Request.Content.ReadAsMultipartAsync(streamProvider);
			FileInfo info = new FileInfo(streamProvider.FileData[0].LocalFileName);
			OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
			sbConnection.DataSource = ServerUploadFolder + "\\" + info.Name;
			String strExtendedProperties = String.Empty;
			sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
			strExtendedProperties = "Excel 12.0 Xml;HDR=YES;";
			sbConnection.Add("Extended Properties", strExtendedProperties);
			List<string> listSheet = new List<string>();
			using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
			{
				await conn.OpenAsync();
				DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
				foreach (DataRow drSheet in dtSheet.Rows)
				{
					if (drSheet["TABLE_NAME"].ToString().Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
					{
						if (Regex.IsMatch(drSheet["TABLE_NAME"].ToString(), @"\B$"))//\W$ W bütün karakterler  $ ise esas karakter B sınırı olmayan tüm karakterler
							listSheet.Add(drSheet["TABLE_NAME"].ToString());
					}

				}
				conn.Close();
			}
			return Ok(new { fileName = info.Name, SheetNames = listSheet });
		}

		[Route("Alanlar")]
		[HttpGet]
		public async Task<IHttpActionResult> Alanlar()
		{
			return Ok(await Task.Run(
				() => new
				{
					ResimAdi = "",
					TCNo = "",
					SicilNo = ""
				}
			)
			);
		}

		private string changeTurtoEng(string data)
		{
			foreach (char c in data)
			{
				switch (c)
				{
					case 'ş':
					case 'Ş':
						data = data.Replace(c, 's');
						break;
					case 'ç':
					case 'Ç':
						data = data.Replace(c, 'c');
						break;
					case 'ı':
					case 'İ':
						data = data.Replace(c, 'i');
						break;
					case 'ğ':
					case 'Ğ':
						data = data.Replace(c, 'g');
						break;
					case 'ü':
					case 'Ü':
						data = data.Replace(c, 'u');
						break;
					case 'ö':
					case 'Ö':
						data = data.Replace(c, 'o');
						break;
				}
			}
			return data;
		}
		#endregion

		private object GetFormData<T>(MultipartFormDataStreamProvider result)
		{
			if (result.FormData.HasKeys())
			{
				var unescapedFormData = Uri.UnescapeDataString(result.FormData
					.GetValues(0).FirstOrDefault() ?? String.Empty);
				if (!String.IsNullOrEmpty(unescapedFormData))
					return JsonConvert.DeserializeObject<T>(unescapedFormData);
			}
			return null;
		}

		/// <summary>
		/// DosyaAdi true ise dosya adı ile işlem görür excel(paramList) dosyası ile işlem yapmaz
		/// false ise excel ile sorgulama yapar
		/// TcNo true ise sorguma yapmadan işleme alır false ise sicili sorgulatır.
		/// http://www.c-sharpcorner.com/article/post-json-data-and-files-in-same-request-with-angularjs-and/
		/// http://monox.mono-software.com/blog/post/Mono/233/Async-upload-using-angular-file-upload-directive-and-net-WebAPI-service/
		/// </summary>
		/// NOT:üst üste yaplan resim kayıtlarnda hata veriyor.**********
		/// <param name="paramList"></param>
		/// <param name="DosyaAdi"></param>
		/// <param name="TcNo"></param>
		/// <returns></returns>

		[Route("tp/{DosyaAdi}/{TcNo}")]
		[HttpPost]
		public async Task<IHttpActionResult> tp(bool DosyaAdi, bool TcNo)
		{

			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
			}

			string folderName = System.IO.Path.GetRandomFileName();
			string newPath = HttpContext.Current.Server.MapPath("~/uploads");
			string fullPath = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");//tüm dosyalarn yükleneceği yolu belirliyoruz.
			string pathString = Path.Combine(fullPath, folderName);//bu yola rand. klasör atyoruz
			Directory.CreateDirectory(pathString);
			var streamProvider = new CustomxMultipartFormDataStreamProvider(pathString);//yeni klasöre yüklüyoruz.
			try
			{//mime dosyalarını okuyoruz
				await Request.Content.ReadAsMultipartAsync(streamProvider);
			}
			catch (Exception)
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			var model = streamProvider.FormData["deger"];//formdata olarak ek bilgi alıyoruz.
			JObject json = JObject.Parse(model);//json dosyasını parse ediyoruz
			JArray paramList = (JArray)json.SelectToken("komponent");//komponent array alyoruz.

			IEnumerable<liste> list = ((JArray)paramList).Select(x => new liste
			{
				ResimAdi = (string)x["ResimAdi"],
				SicilNo = (string)x["SicilNo"],
				TCNo = (string)x["TCNo"]
			}).ToList();

			bool cb = false;
			IList<FileUploadResult> fileInfo = new List<FileUploadResult>();
			foreach (MultipartFileData i in streamProvider.FileData)
			{
				var info = new FileInfo(i.LocalFileName);
				var uploadFiles = new imageUpload();
				uploadFiles.Protokol = 0;
				uploadFiles.Folder = "personel";
				uploadFiles.FileLenght = Convert.ToInt32(info.Length);
				var sdff = i.Headers.ContentDisposition.FileName;
				uploadFiles.FileName = sdff.Substring(1, sdff.Length - 1).Substring(0, sdff.Substring(1, sdff.Length - 1).Length - 1);
				//                       son karakter silindi                     başındaki karakter silindi.
				uploadFiles.GenericName = info.FullName.Split('\\')[info.FullName.Split('\\').Length - 1];
				//split array neslesine dönüşüp son elemanı aldım.
				uploadFiles.MimeType = i.Headers.ContentType.MediaType;
				uploadFiles.UserId = User.Identity.GetUserId();
				string[] dosyasmi = uploadFiles.FileName.Split('.');

				if (DosyaAdi)
				{//direk dosyadan okuyacak
					if (TcNo)
					{
						Personel pers = await _personelService.TcNoKontrol(dosyasmi[0].ToString());
						if (pers != null)
						{
							cb = true;
							uploadFiles.IdGuid = pers.PerGuid.ToString();
							string yeniYol = Path.Combine(newPath, uploadFiles.GenericName);
							try
							{
                                UploadFileFtp(info.FullName, WebRequestMethods.Ftp.UploadFile, info.Name);
                                File.Copy(info.FullName, yeniYol, true);
                            }
							catch (IOException iox)
							{

								throw;
							}

						}
					}
					else
					{
						Personel persx = await _personelService.SicilNoKontrol(dosyasmi[0].ToString());
						if (persx != null)
						{
							cb = true;
							uploadFiles.IdGuid = persx.PerGuid.ToString();
							string yeniYol = Path.Combine(newPath, uploadFiles.GenericName);
                            UploadFileFtp(info.FullName, WebRequestMethods.Ftp.UploadFile, info.Name);
                        }
					};
				}
				else
				{//excelden okuyacak
					if (list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.TCNo).SingleOrDefault() != null)
					{//tc boş değilse
						string tc = list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.TCNo).SingleOrDefault();
						Personel persd = await _personelService.TcNoKontrol(tc);
						if (persd != null)
						{
							cb = true;
							uploadFiles.IdGuid = persd.PerGuid.ToString();
							string yeniYol = Path.Combine(newPath, uploadFiles.GenericName);
                            UploadFileFtp(info.FullName, WebRequestMethods.Ftp.UploadFile, info.Name);
                        }
					}
					else
					{
						string sicil = list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.SicilNo).SingleOrDefault();
						Personel persv = await _personelService.SicilNoKontrol(sicil);
						if (persv != null)
						{
							cb = true;
							uploadFiles.IdGuid = persv.PerGuid.ToString();
							string yeniYol = Path.Combine(newPath, uploadFiles.GenericName);
                            UploadFileFtp(info.FullName, WebRequestMethods.Ftp.UploadFile, info.Name);
                            File.Copy(info.FullName, yeniYol, true);
						}
					};
				};

				if (cb) { await _imageUploadService.AddAsync(uploadFiles); }
				string bilgi = cb == true ? "yapıldı" : "yapılmadı";

				fileInfo.Add(new FileUploadResult()
				{
					FileLength = uploadFiles.FileLenght,
					FileName = uploadFiles.FileName,
					LocalFilePath = "uploads/",
					GenericName = uploadFiles.GenericName,
					yukleme = bilgi
				});
				cb = false;
				uploadFiles = null;
			}
			return Ok(fileInfo);
		}

		[Route("tpazur")]
		[HttpPost]
		public async Task<IHttpActionResult> tpazur()
		{

			string folderName = System.IO.Path.GetRandomFileName().Replace(".", "").ToLower();
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var context = new StorageContext();
				var blobContainer = context.BlobClient.GetContainerReference(folderName);


				if (!await blobContainer.ExistsAsync())
				{
					await blobContainer.CreateAsync(BlobContainerPublicAccessType.Container, null, null);
				}

				var provider = new AzureStorageMultipartFormDataStreamProvider2(blobContainer);
				try
				{
					await Request.Content.ReadAsMultipartAsync(provider);
				}
				catch (Exception ex)
				{
					throw new ApplicationException(ex.Message);
				}

			return Ok(folderName);
		}

		[Route("tpazur/{KlasorAdi}/{DosyaAdi}/{TcNo}")]
		[HttpPost]
		public async Task<IHttpActionResult> tpazur(string KlasorAdi, bool DosyaAdi, bool TcNo, JArray paramList)
		{
			var context = new StorageContext();
			var blobContainer = context.BlobClient.GetContainerReference(KlasorAdi);
			if (!await blobContainer.ExistsAsync())
			{
				await blobContainer.CreateAsync(BlobContainerPublicAccessType.Container, null, null);
			}
			//destination Container işleme alnacak depo
			var destinationContainer = context.BlobClient.GetContainerReference("personel");
			if (!await destinationContainer.ExistsAsync())
			{
				await destinationContainer.CreateAsync(BlobContainerPublicAccessType.Container, null, null);
			}
			IEnumerable<liste> list = ((JArray)paramList).Select(itemx => new liste
			{
				ResimAdi = (string)itemx["ResimAdi"],
				SicilNo = (string)itemx["SicilNo"],
				TCNo = (string)itemx["TCNo"]
			}).ToList();
			bool cb = false;
			IList<FileUploadResult> fileInfo = new List<FileUploadResult>();
			foreach (var blobx in blobContainer.ListBlobs())
			{
				var blob = (CloudBlockBlob)blobx;
				await blob.FetchAttributesAsync();
				var uploadFiles = new imageUpload();
				uploadFiles.Protokol = 0;
				uploadFiles.Folder = "personel";
				uploadFiles.FileLenght = 0;
				uploadFiles.FileName = blob.Name;
				uploadFiles.GenericName = Guid.NewGuid().ToString() +'.'+ blob.Name.Split('.')[1].ToString();
				uploadFiles.MimeType = "";
				uploadFiles.UserId = User.Identity.GetUserId();
				string[] dosyasmi = uploadFiles.FileName.Split('.');
				if (DosyaAdi)
				{//direk dosyadan okuyacak
					if (TcNo)
					{
						Personel pers = await _personelService.TcNoKontrol(dosyasmi[0].ToString());
						if (pers != null)
						{
							cb = true;
							uploadFiles.IdGuid = pers.PerGuid.ToString();
							var sourceBlob = blobContainer.GetBlockBlobReference(uploadFiles.FileName);
							var destinationBlob = destinationContainer.GetBlockBlobReference(uploadFiles.GenericName);
							destinationBlob.StartCopy(sourceBlob);
							sourceBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots);
						}
					}
					else
					{
						Personel persx = await _personelService.SicilNoKontrol(dosyasmi[0].ToString());
						if (persx != null)
						{
							cb = true;
							uploadFiles.IdGuid = persx.PerGuid.ToString();
							var sourceBlob = blobContainer.GetBlockBlobReference(uploadFiles.FileName);
							var destinationBlob = destinationContainer.GetBlockBlobReference(uploadFiles.GenericName);
							destinationBlob.StartCopy(sourceBlob);
							sourceBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots);
						}
					};
				}
				else
				{//excelden okuyacak
					if (list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.TCNo).SingleOrDefault() != null)
					{//tc boş değilse
						string tc = list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.TCNo).SingleOrDefault();
						Personel persd = await _personelService.TcNoKontrol(tc);
						if (persd != null)
						{
							cb = true;
							uploadFiles.IdGuid = persd.PerGuid.ToString();
							var sourceBlob = blobContainer.GetBlockBlobReference(uploadFiles.FileName);
							var destinationBlob = destinationContainer.GetBlockBlobReference(uploadFiles.GenericName);
							destinationBlob.StartCopy(sourceBlob);
							sourceBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots);
						}
					}
					else
					{
						string sicil = list.Where(x => x.ResimAdi == dosyasmi[0].ToString()).Select(x => x.SicilNo).SingleOrDefault();
						Personel persv = await _personelService.SicilNoKontrol(sicil);
						if (persv != null)
						{
							cb = true;
							uploadFiles.IdGuid = persv.PerGuid.ToString();
							var sourceBlob = blobContainer.GetBlockBlobReference(uploadFiles.FileName);
							var destinationBlob = destinationContainer.GetBlockBlobReference(uploadFiles.GenericName);
							destinationBlob.StartCopy(sourceBlob);
							sourceBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots);
						}
					};
				};
				if (cb) { await _imageUploadService.AddAsync(uploadFiles); }
				string bilgi = cb == true ? "yapıldı" : "yapılmadı";

				fileInfo.Add(new FileUploadResult()
				{
					FileLength = 0,
					FileName = uploadFiles.FileName,
					LocalFilePath = ConfigurationManager.AppSettings["storageLink"] + "personel/" + uploadFiles.FileName,
					GenericName = uploadFiles.FileName,
					yukleme = bilgi
				});
				cb = false;
				uploadFiles = null;
			}
			await blobContainer.DeleteAsync();
			return Ok(fileInfo);
		}

        #region yardımcı prosedürler



        public static string UploadFileFtp(string localFile,string method, string name)
        {
            try
            {
                string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
                string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
                string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
                string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
                if (ftp_Work == "true")
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                        Uri uri = new Uri(ftp_Link +name);
                        client.UploadFile(uri, method, localFile);
                    }

                }
                return "Tamam";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public static string UploadFileStreamFtp(string localFile, Stream st)
        {// dosya izinleri aşılmazsa buradan stream gönderilir.
            try
            {
                string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
                string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
                string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
                string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];
                if (ftp_Work == "true")
                {
                    st.Seek(0, SeekOrigin.Begin);

                    WebRequest request =
                        WebRequest.Create(ftp_Link + localFile);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        st.CopyTo(ftpStream);
                    }

                }
                return "Tamam";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string DeleteFtpFile(string fileName)
        {
            string ftp_Link = ConfigurationManager.AppSettings["ftp_Link"];
            string ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string ftp_Work = ConfigurationManager.AppSettings["ftp_Work"];

            try
            {
                if (ftp_Work == "true")
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp_Link + fileName);
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        return response.StatusDescription;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool IsPropertyExist(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }
        }
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            try
            {
                return Convert.ChangeType(obj, castTo);
            }
            catch (FormatException)
            {
                return castTo == typeof(int) ? 0 : 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion




    }

    public class liste
	{
		public string TCNo { get; set; }
		public string SicilNo { get; set; }
		public string ResimAdi { get; set; }
	}
	public class UploadDataModel
	{
		public string testString1 { get; set; }
		public string testString2 { get; set; }
	}
}
public class CustomxMultipartFormDataStreamProvider : MultipartFormDataStreamProvider// upload ettiğimiz dosyaların adını burada değiştiriyoruz.
{
	public CustomxMultipartFormDataStreamProvider(string path) : base(path) { }

	public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
	{// dosya adını burada belirleyebiliyoruz
		string fileName;
		if (!string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))//dosya adı boş değilse 
		{
			var ext = Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));//uzantısnı almak istiyoruz
			fileName = Guid.NewGuid() + ext;
		}
		else
		{
			fileName = Guid.NewGuid() + ".data";
		}
		return fileName;
	}
}


public class AzureStorageMultipartFormDataStreamProvider2 : MultipartFormDataStreamProvider
{
	private readonly CloudBlobContainer _blobContainer;

	public AzureStorageMultipartFormDataStreamProvider2(CloudBlobContainer blobContainer) : base("azure")
	{
		_blobContainer = blobContainer;
	}

	public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
	{
		if (parent == null) throw new ArgumentNullException(nameof(parent));
		if (headers == null) throw new ArgumentNullException(nameof(headers));
		CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(headers.ContentDisposition.FileName.Substring(1, headers.ContentDisposition.FileName.Length - 1).Substring(0, headers.ContentDisposition.FileName.Substring(1, headers.ContentDisposition.FileName.Length - 1).Length - 1));
		if (headers.ContentType != null)
		{
			// Set appropriate content type for your uploaded file
			blob.Properties.ContentType = headers.ContentType.MediaType;

		}

		this.FileData.Add(new MultipartFileData(headers, blob.Name));

		return blob.OpenWrite();

	}
}
