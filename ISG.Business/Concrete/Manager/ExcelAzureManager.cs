using ISG.Business.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Dynamic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace ISG.Business.Concrete.Manager
{
    public class ExcelAzureManager : IExcelAzureService
    {
        /// <summary>
        /// Azure stroge daki excel tablosundaki belirli sayfann tablosunu çeker
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public async Task<List<ExpandoObject>> AzurDeposundakiExceldenVeriAl(string accountName, string accountKey, string folderName, string fileName, int sheetIndex)
        {
            try
            {
                CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                CloudBlobClient context = storageAccount.CreateCloudBlobClient();
                var blobContainer = context.GetContainerReference(folderName.ToLower());
                blobContainer.CreateIfNotExists();
                var blob = blobContainer.GetBlockBlobReference(fileName);
                var blobExists = await blob.ExistsAsync();
                if (!blobExists)
                {
                    throw new Exception("Dosya Bulunamadý1");
                }
                Stream blobStream = await blob.OpenReadAsync();//stream haline getir
                List<ExpandoObject> expandoList = new List<ExpandoObject>();
                DataTable table = excelReaderFactory(blob.Properties.ContentType, blobStream).Tables[sheetIndex];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dynamic data = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
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
                return expandoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dosya Bulunamadý2 " + ex.InnerException.Message.ToString());
            }

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
                    case 'þ':
                    case 'Þ':
                        data = data.Replace(c, 's');
                        break;
                    case 'ç':
                    case 'Ç':
                        data = data.Replace(c, 'c');
                        break;
                    case 'ý':
                    case 'Ý':
                        data = data.Replace(c, 'i');
                        break;
                    case 'ð':
                    case 'Ð':
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

    }
}
