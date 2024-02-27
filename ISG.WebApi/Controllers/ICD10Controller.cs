using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Linq;
using ISG.Entities.ComplexType;
using ExcelDataReader;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/icd")]
    public class ICD10Controller : ApiController
    {
        private IICD10Service _icd10Service { get; set; }

        public ICD10Controller(
            IICD10Service icd10Service
            )
        {
            _icd10Service = icd10Service;
        }

        #region Excel kayıt işlemleri

        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");


        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                string dosyaIsmi = streamProvider.FileData[0].Headers.ContentDisposition.FileName;
                FileInfo info = new FileInfo(streamProvider.FileData[0].LocalFileName);
                return Ok(ExcelSayfaOkumaExcelReader(info, dosyaIsmi));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> ExcelSayfaOkumaOleDb(FileInfo info)
        {
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
            return new { fileName = info.Name, SheetNames = listSheet };
        }

        public object ExcelSayfaOkumaExcelReader(FileInfo info, string filePathx)
        {
            List<string> visibleWorksheetNames = new List<string>();
            FileStream stream = File.Open(ServerUploadFolder + "\\" + info.Name, FileMode.Open, FileAccess.Read);
            string uzanti = filePathx.Substring(filePathx.IndexOf(".") + 1, 4).Trim().ToUpper();
            if (uzanti == "XLSX")
            {
                // Reading from a OpenXml Excel file(2007 format; *.xlsx)
                visibleWorksheetNames = ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet(new ExcelDataSetConfiguration()).Tables.OfType<DataTable>().Select(c => c.TableName).ToList();
            }
            else
            {
                //Reading from a binary Excel file ('97-2003 format; *.xls)
                visibleWorksheetNames = ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet(new ExcelDataSetConfiguration()).Tables.OfType<DataTable>().Select(c => c.TableName).ToList();
            }
            stream.Close();
            stream.Dispose();
            return new { fileName = info.Name, SheetNames = visibleWorksheetNames, DosyaUzantisi = uzanti };
        }

        [Route("ExcelDataAl/{fileNames}/{sheet}/{HDR}")]
        [HttpGet]
        public async Task<IHttpActionResult> ExcelDataAl(string fileNames, string sheet, bool HDR)
        {
            return Ok(await ExcelDataOkumaExcelReader(fileNames, sheet, HDR));
        }

        public async Task<List<ExpandoObject>> ExcelDataOkumaOleDb(string fileNames, string sheet, bool HDR)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            sbConnection.DataSource = ServerUploadFolder+ "\\" + fileNames;
            String strExtendedProperties = String.Empty;
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = HDR ? "Excel 12.0 Xml;HDR=YES;" : "Excel 12.0 Xml;HDR=NO;";
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                await conn.OpenAsync();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = conn;
                Cmd.CommandText = "Select * from [" + sheet + "]";
                var Reader = await Cmd.ExecuteReaderAsync();
                while (Reader.Read())
                {
                    dynamic data = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                    dictionary.Add("durum", false);
                    for (int i = 0; i < Reader.FieldCount; i++)
                    {
                        string sd = Reader.GetName(i) == null ? "BaslikYazilmamis" : changeTurtoEng(Regex.Replace(Reader.GetName(i).ToString(), @"\s+", "")).ToLower();
                        dictionary.Add(sd, Reader[i].ToString());
                    }
                    expandoList.Add(data);
                    data = null;
                }
                Reader.Close();
                conn.Close();
                //Reader.Dispose();
                //conn.Dispose();
            }
            return expandoList;
        }

        public async Task<List<ExpandoObject>> ExcelDataOkumaExcelReader(string fileNames, string sheet, bool HDR)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            FileStream stream = File.Open(ServerUploadFolder + "\\" + fileNames, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = a => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = HDR
                }
            };
            DataSet dataSet = excelDataReader.AsDataSet(conf);
            //DataTable dataTable = dataSet.Tables["Sheet1"];
            DataRowCollection row = dataSet.Tables[sheet].Rows;
            DataColumnCollection col = dataSet.Tables[sheet].Columns;
            //List<object> rowDataList = null;
            //List<object> allRowsList = new List<object>();
            foreach (DataRow r in row)
            {
                //rowDataList = item.ItemArray.ToList(); //list of each rows
                //allRowsList.Add(rowDataList); //adding the above list of each row to another list
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                for (int i = 0; i < col.Count; i++)
                {
                    dictionary.Add(col[i].ColumnName, r[i].ToString());
                }
                expandoList.Add(data);
                data = null;
            }
            stream.Close();
            stream.Dispose();
            return expandoList;
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

        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            return Ok(await Task.Run(
                () => new
                {
                   ICD10Code="",
                   ICDTanimi=""
                }
            )
            );
        }

        public static Object CastDate(dynamic obj)
        {
            try
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(Cast(obj, typeof(string)), CultureInfo.GetCultureInfo("tr-TR"), DateTimeStyles.None, out dt))
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
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
        [Route("InsertICD10List")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertICD10List(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);

                    if (String.IsNullOrEmpty(Cast(jsItem.ICD10Code, typeof(string))) ||
                       String.IsNullOrEmpty(Cast(jsItem.ICDTanimi, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tanımı ve Kodu olmadan kayıt yapamazsınız."));
                    };

                    ICD10 icd10 = new ICD10()
                    {
                        ICD10Code = Cast(jsItem.ICD10Code, typeof(string)),
                        ICDTanimi = Cast(jsItem.ICDTanimi, typeof(string)),
                        AramaOnceligi = false,
                    };

                    if (!icd10.ICD10Code.Equals(null))
                    {
                        ICD10 icd10Varmi = await _icd10Service.FindAsync(icd10);
                        if (icd10Varmi == null)
                        {
                            string sd = (icd10.ICD10Code).Substring(0, 3);
                            ICD10 icd10Varmix = await _icd10Service.FindAsync(new ICD10(){ ICD10Code=sd});
                            icd10.IdRef = icd10Varmix ==null ? 0 : icd10Varmix.ICD10_Id;
                            ICD10 icd10Insert = await _icd10Service.AddAsync(icd10);

                            dictionary.Add("id", (icd10Insert.ICD10_Id).ToString());
                            dictionary.Add("referans", (icd10Insert.IdRef).ToString());
                            dictionary.Add("ICD10 Kodu", icd10Insert.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", icd10Insert.ICDTanimi);
                            dictionary.Add("Arama Önceliği", icd10Insert.AramaOnceligi);
                            dictionary.Add("durum", true);
                            dictionary.Add("Kayıt Durumu", "Başarılı bir kayıt yapıldı...");
                        }
                        else
                        {
                            dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                            dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Bu Kodun Kaydı Yapılmış.Tekrar kayıt yapılamaz.");
                        };

                    }
                    else
                    {
                        dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                        dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                        dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                        dictionary.Add("durum", false);
                        dictionary.Add("Kayıt Durumu", "ICD10 Kodu Olmadan Kayıt Yapılamaz");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errs in ex.EntityValidationErrors)
                    {
                        foreach (var err in errs.ValidationErrors)
                        {
                            var propName = err.PropertyName;
                            var errMess = err.ErrorMessage;
                            dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                            dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                    dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                    dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                    dictionary.Add("durum", false);
                    dictionary.Add("Kayıt Durumu", "Sistem Hatası: " + ex.Message.ToString());
                }

                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
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

        [Route("ExtractICD10List")]
        [HttpPost]
        public async Task<IHttpActionResult> ExtractICD10List(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                dictionary.Add("Donus_Id", jsItem.id);
                ICD10 icd10 = new ICD10()
                {
                    ICD10Code = Cast(jsItem.ICD10Code, typeof(string))
                };
                ICD10 icd10Varmi = await _icd10Service.FindAsync(icd10);
                if (icd10Varmi != null)
                    dictionary.Add("durum", true);
                else dictionary.Add("durum", false);
                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }

        [Route("ICD10UpdateList")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateICD10List(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);
                    if (String.IsNullOrEmpty(Cast(jsItem.ICD10Code, typeof(string))) ||
                      String.IsNullOrEmpty(Cast(jsItem.ICDTanimi, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tanımı ve Kodu olmadan kayıt yapamazsınız."));
                    };

                    ICD10 icd10 = new ICD10()
                    {
                        ICD10Code = Cast(jsItem.ICD10Code, typeof(string))
                    };

                    if (!icd10.ICD10Code.Equals(null))
                    {
                        ICD10 icd10Varmi = await _icd10Service.FindAsync(icd10);
                        if (icd10Varmi != null)
                        {
                            icd10Varmi.ICDTanimi = Cast(jsItem.ICDTanimi, typeof(string));

                            ICD10 icd10Update = await _icd10Service.UpdateAsync(icd10Varmi, icd10Varmi.ICD10_Id);

                            dictionary.Add("ICD10 Kodu", icd10Update.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", icd10Update.ICDTanimi);
                            dictionary.Add("Arama Önceliği", icd10Update.AramaOnceligi);
                            dictionary.Add("durum", true);
                            dictionary.Add("Kayıt Durumu", "Başarılı bir kayıt yapıldı...");
                        }
                        else
                        {
                            dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                            dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Bu Kodun Kaydı Yapılmamış.Veri sisteminde yok.Tanım güncellenmesi yapılamadı.");
                        };

                    }
                    else
                    {
                        dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                        dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                        dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                        dictionary.Add("durum", false);
                        dictionary.Add("Kayıt Durumu", "ICD10 Kodu Olmadan Kayıt Yapılamaz.");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errs in ex.EntityValidationErrors)
                    {
                        foreach (var err in errs.ValidationErrors)
                        {
                            var propName = err.PropertyName;
                            var errMess = err.ErrorMessage;
                            dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                            dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                            dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("ICD10 Kodu", jsItem.ICD10Code);
                    dictionary.Add("ICD10 Tanımı", jsItem.ICDTanimi);
                    dictionary.Add("Arama Önceliği", jsItem.AramaOnceligi);
                    dictionary.Add("durum", false);
                    dictionary.Add("Kayıt Durumu", "Sistem Hatası: " + ex.Message.ToString());
                }

                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }


        #endregion

        #region  Hastalık Ara soundex algoritmasına göre

        [Route("Ara/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> HastalikAra(string value)
        {
            //ICollection<ICD10> icd = await _icd10Service.HastalikAdiAra(value);

            //return Ok(icd.Select(x => x.ICD10Code + " " + x.ICDTanimi));

            ICD10Table icd = await _icd10Service.ICDSearch(value, 0, 120);
            return Ok(icd.ICDView.Select(x => x.ICD10Code + " " + x.ICDTanimi));
        }
        #endregion

    }
}
