using ExcelDataReader;
using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
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
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/odio")]
    public class OdioController : ApiController
    {

        private IOdioService _odioService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private IPersonelService _personelService { get; set; }
        public OdioController(IOdioService odioService,
            IRevirIslemService revirIslemService,
            IPersonelService personelService

            )
        {
            _odioService = odioService;
            _revirIslemService = revirIslemService;
            _personelService = personelService;
        }

        #region Odio Kay�t ��lemleri

        [HttpGet]
        public async Task<Odio> GetById(int id)
        {
            return await _odioService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Odio>> Get()
        {
            return await _odioService.GetAllAsync();
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]Odio odio)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = odio.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "G�ncellenme Kapanm��t�r."));
            odio.Tarih = GuncelTarih();
            odio.UserId = User.Identity.GetUserId();
            return Ok(await _odioService.UpdateAsync(odio, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<Odio> Post(Odio odio, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =  odio.MuayeneTuru, IslemDetayi ="Odio", UserId = User.Identity.GetUserId() }, prt);
            odio.RevirIslem_Id = rv.RevirIslem_Id;
            odio.Protokol = rv.Protokol;
            odio.Tarih = GuncelTarih();
            odio.UserId = User.Identity.GetUserId();
            return await _odioService.AddAsync(odio);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Odio odio = await _odioService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = odio.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme ��lemi Kapanm��t�r."));
            await _odioService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)odio.RevirIslem_Id));
        }

        #endregion

        #region Odio Toplu Excel Kay�t

        private static readonly string ServerUploadFolder2 = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");
        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/excel");

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
                        if (Regex.IsMatch(drSheet["TABLE_NAME"].ToString(), @"\B$"))//\W$ W b�t�n karakterler  $ ise esas karakter B s�n�r� olmayan t�m karakterler
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

        private string changeTurtoEng(string data)
        {
            foreach (char c in data)
            {
                switch (c)
                {
                    case '�':
                    case '�':
                        data = data.Replace(c, 's');
                        break;
                    case '�':
                    case '�':
                        data = data.Replace(c, 'c');
                        break;
                    case '�':
                    case '�':
                        data = data.Replace(c, 'i');
                        break;
                    case '�':
                    case '�':
                        data = data.Replace(c, 'g');
                        break;
                    case '�':
                    case '�':
                        data = data.Replace(c, 'u');
                        break;
                    case '�':
                    case '�':
                        data = data.Replace(c, 'o');
                        break;
                }
            }
            return data;
        }

        private class Item
        {
            public string TcNo  { get; set; }
        }

        [Authorize(Roles = "ISG_Admin , Admin")]
        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            //JObject jo= JObject.FromObject(new Item{TcNo="" });
            //jo.Add("Sag250", "");

            //return Ok(jo)

            return Ok(await Task.Run(
                () => new
                {
                    TcNo = "",
                    Sag250 = "",
                    Sag500 = "",
                    Sag1000 = "",
                    Sag2000 = "",
                    Sag3000 = "",
                    Sag4000 = "",
                    Sag5000 = "",
                    Sag6000 = "",
                    Sag7000 = "",
                    Sag8000 = "",
                    Sol250 = "",
                    Sol500 = "",
                    Sol1000 = "",
                    Sol2000 = "",
                    Sol3000 = "",
                    Sol4000 = "",
                    Sol5000 = "",
                    Sol6000 = "",
                    Sol7000 = "",
                    Sol8000 = "",
                    SagOrtalama = "",
                    SolOrtalama = "",
                    Sonuc = ""
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
        [Authorize(Roles = "ISG_Admin , Admin")]
        [Route("InsertOdioList")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertOdioList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                //foreach (var item in (JObject)jsItem)
                //{
                //    string key = item.Key;
                //    string val = item.Value.ToString();
                //}


                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);

                    if (String.IsNullOrEmpty(Cast(jsItem.TcNo, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tc No olmadan kay�t yapamazs�n�z."));
                    };
                    Personel se = new Personel()
                    {
                        TcNo = jsItem.TcNo,
                    };
                    Personel pers = await _personelService.FindAsync(se);//tc Kayd�n� sorgula
                    if (pers != null)
                    {
                        Odio odio = new Odio()
                        {   
                            Sag250 = IsPropertyExist(jsItem, "Sag250") ? Cast(jsItem.Sag250, typeof(int)) : 0,
                            Sag500 = IsPropertyExist(jsItem, "Sag500") ? Cast(jsItem.Sag500, typeof(int)) : 0,
                            Sag1000 = IsPropertyExist(jsItem, "Sag1000") ? Cast(jsItem.Sag1000, typeof(int)) : 0,
                            Sag2000 = IsPropertyExist(jsItem, "Sag2000") ? Cast(jsItem.Sag2000, typeof(int)) : 0,
                            Sag3000 = IsPropertyExist(jsItem, "Sag3000") ? Cast(jsItem.Sag3000, typeof(int)) : 0,
                            Sag4000 = IsPropertyExist(jsItem, "Sag4000") ? Cast(jsItem.Sag4000, typeof(int)) : 0,
                            Sag5000 = IsPropertyExist(jsItem, "Sag5000") ? Cast(jsItem.Sag5000, typeof(int)) : 0,
                            Sag6000 = IsPropertyExist(jsItem, "Sag6000") ? Cast(jsItem.Sag6000, typeof(int)) : 0,
                            Sag7000 = IsPropertyExist(jsItem, "Sag7000") ? Cast(jsItem.Sag7000, typeof(int)) : 0,
                            Sag8000 = IsPropertyExist(jsItem, "Sag8000") ? Cast(jsItem.Sag8000, typeof(int)) : 0,
                            Sol250 = IsPropertyExist(jsItem, "Sol250") ? Cast(jsItem.Sol250, typeof(int)) : 0,
                            Sol500 = IsPropertyExist(jsItem, "Sol500") ? Cast(jsItem.Sol500, typeof(int)) : 0,
                            Sol1000 = IsPropertyExist(jsItem, "Sol1000") ? Cast(jsItem.Sol1000, typeof(int)) : 0,
                            Sol2000 = IsPropertyExist(jsItem, "Sol2000") ? Cast(jsItem.Sol2000, typeof(int)) : 0,
                            Sol3000 = IsPropertyExist(jsItem, "Sol3000") ? Cast(jsItem.Sol3000, typeof(int)) : 0,
                            Sol4000 = IsPropertyExist(jsItem, "Sol4000") ? Cast(jsItem.Sol4000, typeof(int)) : 0,
                            Sol5000 = IsPropertyExist(jsItem, "Sol5000") ? Cast(jsItem.Sol5000, typeof(int)) : 0,
                            Sol6000 = IsPropertyExist(jsItem, "Sol6000") ? Cast(jsItem.Sol6000, typeof(int)) : 0,
                            Sol7000 = IsPropertyExist(jsItem, "Sol7000") ? Cast(jsItem.Sol7000, typeof(int)) : 0,
                            Sol8000 = IsPropertyExist(jsItem, "Sol8000") ? Cast(jsItem.Sol8000, typeof(int)) : 0,
                            SagOrtalama = IsPropertyExist(jsItem, "SagOrtalama") ? Cast(jsItem.SagOrtalama, typeof(int)) : 0,
                            SolOrtalama = IsPropertyExist(jsItem, "SolOrtalama") ? Cast(jsItem.SolOrtalama, typeof(int)) : 0,
                            Sonuc = Cast(jsItem.Sonuc, typeof(string)),
                            Tarih = GuncelTarih(),
                            Personel_Id = pers.Personel_Id,
                            MuayeneTuru = "Normal Muayene ��lemleri",
                            UserId = User.Identity.GetUserId(),
                        };

                        RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = 1, IslemTuru = "Revir ��lemleri", IslemDetayi = "Odio", UserId = User.Identity.GetUserId() }, false);
                        odio.RevirIslem_Id = rv.RevirIslem_Id;
                        Odio OdioInsert = await _odioService.AddAsync(odio);
                        dictionary.Add("id", (OdioInsert.Odio_Id).ToString());
                        dictionary.Add("TcNo", pers.TcNo);
                        dictionary.Add("tarih", OdioInsert.Tarih);
                        dictionary.Add("durum", true);
                        dictionary.Add("Kay�t Durumu", "Ba�ar�l� bir kay�t yap�ld�...");

                    }
                    else
                    {

                        dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
                        dictionary.Add("tarih", "Kay�t Yap�lmad�!");
                        dictionary.Add("durum", true);
                        dictionary.Add("Kay�t Durumu", "TcNo Kayd� Yok.");
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
                            dictionary.Add("tarih", "Kay�t Yap�lmad�!");
                            dictionary.Add("durum", false);
                            dictionary.Add("Kay�t Durumu", "Validation Hatas�" + propName + ": " + errMess + " Sistem de�erleri ile verilen alanlar uyu�muyor. Bu alan� kald�r�n.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
                    dictionary.Add("tarih", "Kay�t Yap�lmad�!");
                    dictionary.Add("durum", false);
                    dictionary.Add("Kay�t Durumu", "Sistem Hatas�: " + ex.Message.ToString());
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
