using ExcelDataReader;
using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Row = DocumentFormat.OpenXml.Spreadsheet.Row;

namespace ISG.WebApi.Controllers
{
  //  [Authorize]
    [RoutePrefix("api/laboratuar")]
    public class LaboratuarController : ApiController
    {
        private ILaboratuarService _laboratuarService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }

        private IPersonelService _personelService { get; set; }
        private IExcelAzureService _excelAzureService { get; set; }

        public LaboratuarController(ILaboratuarService laboratuarService,
            IRevirIslemService revirIslemService,
            IPersonelService personelService,
            IExcelAzureService excelAzureService
            )
        {
            _laboratuarService = laboratuarService;
            _revirIslemService = revirIslemService;
            _personelService = personelService;
            _excelAzureService = excelAzureService;
        }


        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/excel");

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

        #region lab kayýt güncelleme
        [HttpGet]
        public async Task<IHttpActionResult> Tanimlari()
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            sbConnection.DataSource = ServerUploadFolder + "\\labTestleri.xlsx";
            String strExtendedProperties = String.Empty;
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = "Excel 12.0 Xml;HDR=YES;";
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                await conn.OpenAsync();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = conn;
                Cmd.CommandText = "Select * from [lab$]";
                var Reader = await Cmd.ExecuteReaderAsync();
                while (Reader.Read())
                {
                    dynamic data = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
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
            }
            return Ok(expandoList);
        }


        [Route("azurtanimi")]
        [HttpGet]
        public async Task<IHttpActionResult> AzureTanimlari()
        {
            return Ok(await _excelAzureService.AzurDeposundakiExceldenVeriAl(ConfigurationManager.AppSettings["storage:account:name"],
                ConfigurationManager.AppSettings["storage:account:key"], "excel", "labtestleri.xlsx", 0));

        }

        public async Task<List<ExpandoObject>> Tanimlari2()
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            sbConnection.DataSource = ServerUploadFolder + "\\labTestleri.xlsx";
            String strExtendedProperties = String.Empty;
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = "Excel 12.0 Xml;HDR=YES;";
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                await conn.OpenAsync();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = conn;
                Cmd.CommandText = "Select * from [lab$]";
                var Reader = await Cmd.ExecuteReaderAsync();
                while (Reader.Read())
                {
                    dynamic data = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
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
            }
            return expandoList;
        }

        [Route("tanimAra/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> TanimAra(string value)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            sbConnection.DataSource = ServerUploadFolder + "\\labTestleri2.xls";
            String strExtendedProperties = String.Empty;
            sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
            strExtendedProperties = "Excel 12.0 Xml;HDR=YES;";
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<string> listSheet = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                await conn.OpenAsync();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = conn;
                Cmd.CommandText = "Select * from [lab$]";
                var Reader = await Cmd.ExecuteReaderAsync();
                while (Reader.Read())
                {
                    dynamic data = new ExpandoObject();
                    IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
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
            }
            //var smd = expandoList.Cast<dynamic>().Where().Select(x => x.tetkik).ToList();
            var smdx = expandoList.Cast<ExpandoObject>().SelectMany(x => x, (obj, field) => new { obj, field })
           .Where(x => x.field.Value != null && x.field.Value.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
           .Select(x => x.field.Value);
            return Ok(smdx);
        }


        //[Route("tanimAra/{value}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> TanimAra(string value)
        //{
        //    var filePath = ServerUploadFolder + "\\labTestleri2.xlsx";
        //    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

        //    // Excel dosyasýný yükleyin ve okuyun.
        //    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
        //    {
        //        WorkbookPart workbookPart = doc.WorkbookPart;
        //        Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
        //        WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
        //        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        //        SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;

        //        foreach (Row r in sheetData.Elements<Row>())
        //        {
        //            Dictionary<string, string> rowValues = new Dictionary<string, string>();
        //            foreach (Cell c in r.Elements<Cell>())
        //            {
        //                string value1 = string.Empty;
        //                if (c.DataType != null && c.DataType == CellValues.SharedString)
        //                {
        //                    value1 = stringTablePart.SharedStringTable.ChildElements[int.Parse(c.CellValue.InnerText)].InnerText;
        //                }
        //                else
        //                {
        //                    value1 = c.CellValue.InnerText;
        //                }
        //                rowValues.Add(c.CellReference, value1);
        //            }
        //            rows.Add(rowValues);
        //        }
        //    }

        //    // Ýstenen deðere göre sonuçlarý filtreleyin.
        //    var filteredResults = rows.SelectMany(row => row)
        //                              .Where(pair => pair.Value.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
        //                              .Select(pair => pair.Value)
        //                              .ToList();

        //    return Ok(filteredResults);
        //}


        [Route("tanimAraAzure/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> TanimAraAzure(string value)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            expandoList = await _excelAzureService.AzurDeposundakiExceldenVeriAl(ConfigurationManager.AppSettings["storage:account:name"],
                ConfigurationManager.AppSettings["storage:account:key"], "excel", "labtestleri.xlsx", 0);
            var smdx = expandoList.Cast<ExpandoObject>().SelectMany(x => x, (obj, field) => new { obj, field })
           .Where(x => x.field.Value != null && x.field.Value.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
           .Select(x => x.field.Value);
            return Ok(smdx);
        }

        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]Laboratuar laboratuar)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = laboratuar.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
            laboratuar.Tarih = GuncelTarih();
            laboratuar.UserId = User.Identity.GetUserId();
            return Ok(await _laboratuarService.UpdateAsync(laboratuar, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<Laboratuar> Post(Laboratuar laboratuar, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = laboratuar.MuayeneTuru, IslemDetayi = "Biyokimya", UserId = User.Identity.GetUserId() }, prt);
            laboratuar.RevirIslem_Id = rv.RevirIslem_Id;
            laboratuar.Protokol = rv.Protokol;
            laboratuar.Tarih = GuncelTarih();
            laboratuar.UserId = User.Identity.GetUserId();
            return await _laboratuarService.AddAsync(laboratuar);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Laboratuar laboratuar = await _laboratuarService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = laboratuar.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _laboratuarService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)laboratuar.RevirIslem_Id));
        }

        #endregion
        #region Labaratuvar Toplu Excel Kayýt

        private static readonly string ServerUploadFolder2 = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder2);
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
            sbConnection.DataSource = ServerUploadFolder2 + "\\" + info.Name;
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
                        if (Regex.IsMatch(drSheet["TABLE_NAME"].ToString(), @"\B$"))//\W$ W bütün karakterler  $ ise esas karakter B sýnýrý olmayan tüm karakterler
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
            FileStream stream = File.Open(ServerUploadFolder2 + "\\" + info.Name, FileMode.Open, FileAccess.Read);
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
            sbConnection.DataSource = ServerUploadFolder2 + "\\" + fileNames;
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
            FileStream stream = File.Open(ServerUploadFolder2 + "\\" + fileNames, FileMode.Open, FileAccess.Read);
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

        private class Item
        {
            public string TcNo { get; set; }
        }

        [Authorize(Roles = "ISG_Admin , Admin")]
        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            JObject jo = JObject.FromObject(new Item { TcNo = "" });
            List<ExpandoObject> sd =await Tanimlari2();
            foreach (dynamic itemx in sd)
            {
                jo.Add(itemx.tetkik, "");
            }
            return Ok(await Task.Run(
                () => jo
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
        [Authorize(Roles = "ISG_Admin , Admin")]
        [Route("InsertBiyokimyaList")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertBiyokimyaList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                StringBuilder sonuc =new StringBuilder();
                foreach (var item in (JObject)jsItem)
                {
                    string key = item.Key;
                    string val = item.Value.ToString();
                    if (key != "TcNo"&& key != "id")
                    {
                        sonuc.Append(key + ":"+val+", ");
                        sonuc.Append(",");
                    }
                }
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);

                    if (String.IsNullOrEmpty(Cast(jsItem.TcNo, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tc No olmadan kayýt yapamazsýnýz."));
                    };
                    Personel se = new Personel()
                    {
                        TcNo = jsItem.TcNo,
                    };
                    Personel pers = await _personelService.FindAsync(se);//tc Kaydýný sorgula
                    if (pers != null)
                    {
                        Laboratuar laboratuar = new Laboratuar()
                        {
                            Sonuc = sonuc.ToString().Substring(0, sonuc.ToString().Length - 1),
                            Grubu= "BÝYOKÝMYA",
                            Tarih = GuncelTarih(),
                            Personel_Id = pers.Personel_Id,
                            MuayeneTuru = "Normal Muayene Ýþlemleri",
                            UserId = User.Identity.GetUserId(),
                        };

                        RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = 1, IslemTuru = "Revir Ýþlemleri", IslemDetayi = "Biyokimya", UserId = User.Identity.GetUserId() }, false);
                        laboratuar.RevirIslem_Id = rv.RevirIslem_Id;
                        Laboratuar LaboratuarInsert = await _laboratuarService.AddAsync(laboratuar);
                        dictionary.Add("id", (LaboratuarInsert.Laboratuar_Id).ToString());
                        dictionary.Add("TcNo", pers.TcNo);
                        dictionary.Add("Sonuc", sonuc.ToString().Substring(0,sonuc.ToString().Length-1));
                        dictionary.Add("tarih", LaboratuarInsert.Tarih);
                        dictionary.Add("durum", true);
                        dictionary.Add("Kayýt Durumu", "Baþarýlý bir kayýt yapýldý...");
                    }
                    else
                    {

                        dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
                        dictionary.Add("tarih", "Kayýt Yapýlmadý!");
                        dictionary.Add("durum", true);
                        dictionary.Add("Kayýt Durumu", "TcNo Kaydý Yok.");
                    };
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errs in ex.EntityValidationErrors)
                    {
                        foreach (var err in errs.ValidationErrors)
                        {
                            var propName = err.PropertyName;
                            var errMess = err.ErrorMessage;
                            dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
                            dictionary.Add("tarih", "Kayýt Yapýlmadý!");
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayýt Durumu", "Validation Hatasý" + propName + ": " + errMess + " Sistem deðerleri ile verilen alanlar uyuþmuyor. Bu alaný kaldýrýn.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
                    dictionary.Add("tarih", "Kayýt Yapýlmadý!");
                    dictionary.Add("durum", false);
                    dictionary.Add("Kayýt Durumu", "Sistem Hatasý: " + ex.Message.ToString());
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
        #endregion


    }
}
