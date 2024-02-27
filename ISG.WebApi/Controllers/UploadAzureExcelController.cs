using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Dynamic;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using ExcelDataReader;

namespace ISG.WebApi.Controllers
{

	[RoutePrefix("api/azurExcelDepo")]
	
	public class UploadAzureExcelController : BaseApiController
	{
		public UploadAzureExcelController()
		{
			
		}
		//private const string Container = "excelDepo";
		[Route("upload")]
		[HttpPost]
		public async Task<IHttpActionResult> UploadFile()
		{
			try
			{
				var accountName = ConfigurationManager.AppSettings["storage:account:name"];
				var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
				CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
				CloudBlobClient context = storageAccount.CreateCloudBlobClient();
				var blobContainer = context.GetContainerReference("1atemp");		  
				if (!await blobContainer.ExistsAsync())
				{
					await blobContainer.CreateAsync(BlobContainerPublicAccessType.Container, null, null);
				}
				var provider = new AzureStorageMultipartFormDataStreamProvider(blobContainer);
				await Request.Content.ReadAsMultipartAsync(provider);
				FileInfo info = new FileInfo(provider.FileData[0].LocalFileName);
				var dosyaIsmi = provider.FileData[0].Headers.ContentDisposition.FileName;
				var mimeName = provider.FileData[0].Headers.ContentType.MediaType.ToString();
				var blob = blobContainer.GetBlockBlobReference(provider.FileData[0].LocalFileName);//yüklenen blob dosyasını çağır
				Stream blobStream = await blob.OpenReadAsync();
				List<sheet> listSheet = new List<sheet>();
				int i = 0;		
				foreach (DataTable table in excelReaderFactory(mimeName, blobStream).Tables)
				{
					listSheet.Add(new sheet() {id=i,sheetName=table.TableName});
					i++;
				}                                 
				return Ok(new { fileName = info.Name, SheetNames = listSheet,mime= mimeName });
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}

		}

		/// <summary>
		/// Excel dosyasının sayfa,dosyaadı,başlık için okur ve aktarır.
		/// </summary>
		/// <param name="fileNames"></param>
		/// <param name="sheet"></param>
		/// <param name="HDR"></param>
		/// <returns></returns>
		[Route("ExcelDataAl/{fileNames}/{sheetIndex}")]
		[HttpGet]
		public async Task<IHttpActionResult> ExcelDataAl(string fileNames, int sheetIndex)
		{
			var context = new StorageContext();
			var blobContainer = context.BlobClient.GetContainerReference("1atemp");
			blobContainer.CreateIfNotExists();
			var blob = blobContainer.GetBlockBlobReference(fileNames);
			var blobExists = await blob.ExistsAsync();
			if (!blobExists)
			{
				return Content(HttpStatusCode.NotFound, await Task.Run(() => "Dosya Bulunamadı"));
			}
			Stream blobStream = await blob.OpenReadAsync();//stream haline getir
			List<ExpandoObject> expandoList = new List<ExpandoObject>();
			DataTable table = excelReaderFactory(blob.Properties.ContentType, blobStream).Tables[sheetIndex];
			for (int i = 0; i < table.Rows.Count; i++)
			{
				dynamic data = new ExpandoObject();
				IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
				dictionary.Add("durum", false);
				for (int j = 0; j < table.Columns.Count; j++)
				{
					if (table.Rows.Count - 1 > i)
					{
						string sd = table.Rows[0].ItemArray[j].ToString() == "" ? j.ToString() : changeTurtoEng(Regex.Replace(table.Rows[0].ItemArray[j].ToString(), @"\s+", "")).ToLower().Trim();
						dictionary.Add(sd, table.Rows[i + 1].ItemArray[j].ToString().Trim());
					}
				}
				expandoList.Add(data);
				
				data = null;
				
			}

			return Ok(expandoList);
		}

		private DataSet excelReaderFactory(string mimeName, Stream blobStream)
		{
			if (mimeName == "application/vnd.ms-excel")
			{
				IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(blobStream);
				return excelReader.AsDataSet();
			}
			else
			{
				IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(blobStream);
				return excelReader.AsDataSet();
			};
			
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

		private class sheet
		{
			public int id { get; set; }
			public string sheetName { get; set; }
		}
	}
}
