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
    [RoutePrefix("api/hemogram")]
    public class HemogramController : ApiController
    {

        private IHemogramService _hemogramService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private IPersonelService _personelService { get; set; }
        public HemogramController(IHemogramService hemogramService,
            IRevirIslemService revirIslemService, IPersonelService personelService
            )
        {
            _hemogramService = hemogramService;
            _revirIslemService = revirIslemService;
            _personelService = personelService;
        }
        #region Hemogram kayt Güncelleme
        [HttpGet]
        public async Task<Hemogram> GetById(int id)
        {
            return await _hemogramService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Hemogram>> Get()
        {
            return await _hemogramService.GetAllAsync();
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]Hemogram hemogram)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = hemogram.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
            hemogram.Tarih = GuncelTarih();
            hemogram.UserId = User.Identity.GetUserId();
            return Ok(await _hemogramService.UpdateAsync(hemogram, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<Hemogram> Post(Hemogram hemogram, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = hemogram.MuayeneTuru, IslemDetayi ="Hemogram" , UserId = User.Identity.GetUserId() }, prt);
            hemogram.RevirIslem_Id = rv.RevirIslem_Id;
            hemogram.Protokol = rv.Protokol;
            hemogram.Tarih = GuncelTarih();
            hemogram.UserId = User.Identity.GetUserId();
            return await _hemogramService.AddAsync(hemogram);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Hemogram hemogram = await _hemogramService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = hemogram.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _hemogramService.DeleteAsync(id);
            return Ok( await _revirIslemService.DeleteAsync((int)hemogram.RevirIslem_Id));
        }
        #endregion


        #region Toplu Kayýt
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
        [Authorize(Roles = "ISG_Admin , Admin")]
        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            return Ok(await Task.Run(
                () => new
                {
                    TcNo  = "",
                    Sonuc  = "",
                    Eritrosit  = "",
                    Hematokrit  = "",
                    Hemoglobin  = "",
                    MCV  = "",
                    MCH  = "",
                    MCHC  = "",
                    RDW  = "",
                    Lokosit  = "",
                    Lenfosit_Yuzde  = "",
                    Monosit_Yuzde  = "",
                    Granülosit_Yuzde  = "",
                    Notrofil_Yuzde  = "",
                    Eoznofil_Yuzde  = "",
                    Bazofil_Yuzde  = "",
                    Trombosit  = "",
                    MeanPlateletVolume  = "",
                    Platekrit  = "",
                    PDW = ""
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
        [Route("InsertHemogramList")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertHemogramList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
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
                        Hemogram hemogram = new Hemogram()
                        {
                            Eritrosit = Cast(jsItem.Eritrosit, typeof(string)),
                            Hematokrit = Cast(jsItem.Hematokrit, typeof(string)),
                            Hemoglobin = Cast(jsItem.Hemoglobin, typeof(string)),
                            MCV = Cast(jsItem.MCV, typeof(string)),
                            MCH = Cast(jsItem.MCH, typeof(string)),
                            MCHC = Cast(jsItem.MCHC, typeof(string)),
                            RDW = Cast(jsItem.RDW, typeof(string)),
                            Lokosit = Cast(jsItem.Lokosit, typeof(string)),
                            Lenfosit_Yuzde = Cast(jsItem.Lenfosit_Yuzde, typeof(string)),
                            Monosit_Yuzde = Cast(jsItem.Monosit_Yuzde, typeof(string)),
                            Granülosit_Yuzde = Cast(jsItem.Granülosit_Yuzde, typeof(string)),
                            Notrofil_Yuzde = Cast(jsItem.Notrofil_Yuzde, typeof(string)),
                            Eoznofil_Yuzde = Cast(jsItem.Eoznofil_Yuzde, typeof(string)),
                            Bazofil_Yuzde = Cast(jsItem.Bazofil_Yuzde, typeof(string)),
                            Trombosit = Cast(jsItem.Trombosit, typeof(string)),
                            MeanPlateletVolume = Cast(jsItem.MeanPlateletVolume, typeof(string)),
                            Platekrit = Cast(jsItem.Platekrit, typeof(string)),
                            PDW = Cast(jsItem.PDW, typeof(string)),
                            Sonuc = Cast(jsItem.Sonuc, typeof(string)),
                            Tarih = GuncelTarih(),
                            Personel_Id = pers.Personel_Id,
                            MuayeneTuru = "Normal Muayene Ýþlemleri",
                            UserId = User.Identity.GetUserId(),
                        };

                        RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = 1, IslemTuru = "Revir Ýþlemleri", IslemDetayi = "Hemogram", UserId = User.Identity.GetUserId() }, false);
                        hemogram.RevirIslem_Id = rv.RevirIslem_Id;
                        Hemogram HemogramInsert = await _hemogramService.AddAsync(hemogram);
                        dictionary.Add("id", (HemogramInsert.Hemogram_Id).ToString());
                        dictionary.Add("TcNo", pers.TcNo);
                        dictionary.Add("tarih", HemogramInsert.Tarih);
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
