using ExcelDataReader;
using ISG.Business.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace ISG.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/ilac")]
    public class IlacController : ApiController
    {
        private IIlacService _ilacService { get; set; }
        private IIlacStokGirisiService _ilacStokGirisiService { get; set; }
        private IIlacStokService _ilacStokService { get; set; }
        private IIlacSarfCikisiService _ilacSarfCikisiService { get; set; }
        private IKtubKtService _ktubKtService { get; set; }


        public IlacController(
            IIlacService ilacService,
            IIlacStokGirisiService ilacStokGirisiService,
            IIlacStokService ilacStokService,
            IIlacSarfCikisiService ilacSarfCikisiService,
            IKtubKtService ktubKtService
            )
        {
            _ilacService = ilacService;
            _ilacStokGirisiService = ilacStokGirisiService;
            _ilacStokService = ilacStokService;
            _ilacSarfCikisiService = ilacSarfCikisiService;
            _ktubKtService = ktubKtService;
        }

        #region Excel kayıt işlemleri
        /// <summary>
        /// Excel Kayıt işlemeleri için yazıldı.Selçuk Eca deposunda gelen
        /// exce tablosunu kaydetmek için yazıldı.
        /// </summary>
        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");

        /// <summary>
        /// Upload Dosyasını alır.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Excel dosyasının sayfa,dosyaadı,başlık için okur ve aktarır.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="sheet"></param>
        /// <param name="HDR"></param>
        /// <returns></returns>
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
            sbConnection.DataSource = ServerUploadFolder + "\\" + fileNames;
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

        /// <summary>
        /// Türkçe karakterleri ingilizce karaktere dönüştürür.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Dönüşüme karşılık gelecek alanaları verir.
        /// </summary>
        /// <returns></returns>
        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            return Ok(await Task.Run(
                () => new
                {// İsimsiz Tip (Anonymous Type) gereksiz class yazmaktan kurtuluyoruz.
                    IlacBarkodu = "",
                    IlacAdi = "",
                    AtcKodu = "",
                    AtcAdi = "",
                    FirmaAdi = "",
                    ReceteTuru = "",
                    Fiyat = "",
                    Aski = "",
                    GeriOdeme = "",
                    Durumu = ""
                }
            )
            );
        }
        /// <summary>
        /// dinamik tarih cast için yazıldı
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        [Route("InsertIlacList")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertIlacList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);
                    if (String.IsNullOrEmpty(Cast(jsItem.IlacBarkodu, typeof(string))) ||
                       String.IsNullOrEmpty(Cast(jsItem.IlacAdi, typeof(string))))
                    {
                        // return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Barkodu , İlaç Adı ve Etken Madde olmadan kayıt yapamazsınız."));
                        dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                        dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                        dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                        dictionary.Add("Atc Adı", jsItem.AtcAdi);
                        dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                        dictionary.Add("durum", false);
                        dictionary.Add("Kayıt Durumu", "Barkodu, İlaç Adı olmadan kayıt yapamazsınız.");

                    }
                    else
                    {
                        int AskiyaAlinanIlac = IsPropertyExist(jsItem, "Aski") ? Int32.Parse(Cast(jsItem.Aski, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false))) : 1;
                        Ilac ilac = new Ilac()
                        {
                            IlacBarkodu = Cast(jsItem.IlacBarkodu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)),
                            IlacAdi = Cast(jsItem.IlacAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)),
                            AtcKodu = IsPropertyExist(jsItem, "AtcKodu") ? Cast(jsItem.AtcKodu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "",
                            AtcAdi = IsPropertyExist(jsItem, "AtcAdi") ? Cast(jsItem.AtcAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "",
                            FirmaAdi = IsPropertyExist(jsItem, "FirmaAdi") ? Cast(jsItem.FirmaAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Firma Adı Girilmedi",
                            ReceteTuru = IsPropertyExist(jsItem, "ReceteTuru") ? Cast(jsItem.ReceteTuru, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Normal",
                            SystemStatus = IsPropertyExist(jsItem, "GeriOdeme") ? Int32.Parse(Cast(jsItem.GeriOdeme, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false))) == 1 ? true : false : false,
                            Status = AskiyaAlinanIlac == 0 ? true : false,
                            Aski = AskiyaAlinanIlac,
                            Fiyat = IsPropertyExist(jsItem, "Fiyat") ? decimal.Parse((Cast(jsItem.Fiyat, typeof(string)).Replace(",", ".")), CultureInfo.InvariantCulture) : 0
                        };

                        ilac.SystemStatus = IsPropertyExist(jsItem, "Durumu") ? (Cast(jsItem.Durumu, typeof(string)).Trim() == "Aktif" ? ilac.SystemStatus = true : ilac.SystemStatus = false) : ilac.SystemStatus;


                        if (!ilac.IlacBarkodu.Equals(null))
                        {
                            Ilac ilacVarmi = await _ilacService.FindAsync(ilac);
                            if (ilacVarmi == null)
                            {
                                Ilac ilacInsert = await _ilacService.AddAsync(ilac);

                                dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                                dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                                dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                                dictionary.Add("Atc Adı", jsItem.AtcAdi);
                                dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                                dictionary.Add("durum", true);
                                dictionary.Add("Kayıt Durumu", "Başarılı bir kayıt yapıldı...");
                            }
                            else
                            {
                                dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                                dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                                dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                                dictionary.Add("Atc Adı", jsItem.AtcAdi);
                                dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                                dictionary.Add("durum", false);
                                dictionary.Add("Kayıt Durumu", "Bu İlaçın Kaydı Yapılmış.Tekrar kayıt yapılamaz.");
                            };

                        }
                        else
                        {
                            dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                            dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                            dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                            dictionary.Add("Atc Adı", jsItem.AtcAdi);
                            dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "İlaçın Barkodu Olmadan Kayıt Yapılamaz");
                        }
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
                            dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                            dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                            dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                            dictionary.Add("Atc Adı", jsItem.AtcAdi);
                            dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                            dictionary.Add("durum", false);
                            dictionary.Add("KayÄ±t Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                    dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                    dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                    dictionary.Add("Atc Adı", jsItem.AtcAdi);
                    dictionary.Add("Firma Adı", jsItem.FirmaAdi);
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

        [Route("ExtractIlacList")]
        [HttpPost]
        public async Task<IHttpActionResult> ExtractIlacList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                dictionary.Add("Donus_Id", jsItem.id);
                Ilac ilac = new Ilac()
                {
                    IlacBarkodu = Cast(jsItem.IlacBarkodu, typeof(string)).Trim()
                };
                Ilac ilacVarmi = await _ilacService.FindAsync(ilac);
                if (ilacVarmi != null)
                    dictionary.Add("durum", true);
                else dictionary.Add("durum", false);
                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }

        [Route("IlacFiyatList")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateIlacList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);
                    if (String.IsNullOrEmpty(Cast(jsItem.IlacBarkodu, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Barkodu girilmeden güncelleme yapamazsınız."));
                    };

                    Ilac ilac = new Ilac()
                    {
                        IlacBarkodu = Cast(jsItem.IlacBarkodu, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)).Trim()
                    };

                    if (!ilac.IlacBarkodu.Equals(null))
                    {
                        Ilac ilacVarmi = await _ilacService.FindAsync(ilac);

                        if (ilacVarmi != null)
                        {
                            ilacVarmi.Fiyat = IsPropertyExist(jsItem, "Fiyat") ? decimal.Parse((Cast(jsItem.Fiyat, typeof(string)).Replace(",", ".")), CultureInfo.InvariantCulture) : ilacVarmi.Fiyat;
                            ilacVarmi.IlacAdi = IsPropertyExist(jsItem, "IlacAdi") ? Cast(jsItem.IlacAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : ilacVarmi.IlacAdi;
                            ilacVarmi.AtcAdi = IsPropertyExist(jsItem, "AtcAdi") ? Cast(jsItem.AtcAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : ilacVarmi.AtcAdi;
                            ilacVarmi.AtcKodu = IsPropertyExist(jsItem, "AtcKodu") ? Cast(jsItem.AtcKodu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : ilacVarmi.AtcKodu;
                            ilacVarmi.FirmaAdi = IsPropertyExist(jsItem, "FirmaAdi") ? Cast(jsItem.FirmaAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : ilacVarmi.FirmaAdi;
                            ilacVarmi.Aski = IsPropertyExist(jsItem, "Aski") ? Cast(jsItem.Aski, typeof(Int32)) : ilacVarmi.Aski;
                            ilacVarmi.Status = IsPropertyExist(jsItem, "Aski") ? Cast(jsItem.Aski, typeof(Int32)) == 0 ? true : false : ilacVarmi.Status;
                            ilacVarmi.SystemStatus = IsPropertyExist(jsItem, "GeriOdeme") ? Cast(jsItem.GeriOdeme, typeof(Int32)) == 1 ? true : false : ilacVarmi.SystemStatus;
                            ilacVarmi.SystemStatus = IsPropertyExist(jsItem, "Durumu") ? (Cast(jsItem.Durumu, typeof(string)).Trim() == "Aktif" ? ilacVarmi.SystemStatus = true : ilacVarmi.SystemStatus = false) : ilacVarmi.SystemStatus;
                            ilacVarmi.ReceteTuru = IsPropertyExist(jsItem, "ReceteTuru") ? Cast(jsItem.ReceteTuru, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : ilacVarmi.ReceteTuru;

                            Ilac ilacUpdate = await _ilacService.UpdateStringAsync(ilacVarmi, Cast(jsItem.IlacBarkodu, typeof(string)).Trim());

                            dictionary.Add("İlaç Barkodu", ilacUpdate.IlacBarkodu);
                            dictionary.Add("Fiyat", ilacUpdate.Fiyat);
                            dictionary.Add("İlaç Adı", ilacUpdate.IlacAdi);
                            dictionary.Add("Atc Kodu", ilacUpdate.AtcKodu);
                            dictionary.Add("Atc Adı", ilacUpdate.AtcAdi);
                            dictionary.Add("Firma Adı", ilacUpdate.FirmaAdi);
                            dictionary.Add("durum", true);
                            dictionary.Add("Kayıt Durumu", "Başarılı bir kayıt yapıldı...");
                        }
                        else
                        {
                            dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                            dictionary.Add("Fiyat", jsItem.Fiyat);
                            dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                            dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                            dictionary.Add("Atc Adı", jsItem.AtcAdi);
                            dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Bu İlaçın Kaydı Yapılmamış.Veri sisteminde yok.Güncellenmesi yapılamadı.");
                        };

                    }
                    else
                    {
                        dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                        dictionary.Add("Fiyat", jsItem.Fiyat);
                        dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                        dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                        dictionary.Add("Atc Adı", jsItem.AtcAdi);
                        dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                        dictionary.Add("durum", false);
                        dictionary.Add("Kayıt Durumu", "İlaçın Barkodu Olmadan Kayıt Yapılamaz.");
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
                            dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                            dictionary.Add("Fiyat", jsItem.Fiyat);
                            dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                            dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                            dictionary.Add("Atc Adı", jsItem.AtcAdi);
                            dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("İlaç Barkodu", jsItem.IlacBarkodu);
                    dictionary.Add("Fiyat", jsItem.Fiyat);
                    dictionary.Add("İlaç Adı", jsItem.IlacAdi);
                    dictionary.Add("Atc Kodu", jsItem.AtcAdi);
                    dictionary.Add("Atc Adı", jsItem.AtcAdi);
                    dictionary.Add("Firma Adı", jsItem.FirmaAdi);
                    dictionary.Add("durum", false);
                    dictionary.Add("Kayıt Durumu", "Sistem Hatası: " + ex.Message.ToString());
                }

                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }

        [Route("IlaclariSil")]
        [HttpGet]
        public async Task<IHttpActionResult> ilaclarisil()
        {
            return Ok(await _ilacService.FullDeleteAsync());
        }


       // private static readonly object HttpUtility;

        [Route("kubKtlistesi")]
        [HttpGet]
        public async Task<IHttpActionResult> KubKtlistesi()
        {
            try
            {
                await _ktubKtService.FullDeleteAsync();
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://www.titck.gov.tr/kubkt") as HttpWebRequest;
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse(); ;
                string cookieHeader = Response.Headers["Set-Cookie"];
                IList<string> cook = cookieHeader.Split(';');
                string XSRF = "";
                string kurumwebsitesi_session = "";
                string token = "";
                foreach (string item in cook)
                {
                    if ((item.Trim().Length) > 25)
                    {
                        if (item.Trim().Substring(0, 10) == "XSRF-TOKEN")
                        {

                            XSRF = item.Trim();
                        };
                        if (item.Trim().Substring(0, 29) == "path=/,kurumwebsitesi_session")
                        {
                            kurumwebsitesi_session = (item.Trim().Split(','))[1];
                        };
                    }
                }
                var resp = new StreamReader(Response.GetResponseStream()).ReadToEnd();//
                Match m = Regex.Match(resp, "_token:(.*?),", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                token = m.Groups[1].Value.ToString();
                /// sonra web sitesinin istediği gibi veri gönderilir.
                /// 
                var qs = "";///sorgulama
                var parsed = System.Web.HttpUtility.ParseQueryString(qs);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.titck.gov.tr/getkubktviewdatatable");
                request.Method = "POST";
                request.ContentType = " application/x-www-form-urlencoded; charset=UTF-8";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers["Cookie"] = XSRF + "; " + kurumwebsitesi_session;
                string postData = "draw=1&columns%5B0%5D%5Bdata%5D=name&columns%5B0%5D%5Bname%5D=&columns%5B0%5D%5Bsearchable%5D=true&columns%5B0%5D%5Borderable%5D=true&columns%5B0%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B0%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B1%5D%5Bdata%5D=element&columns%5B1%5D%5Bname%5D=&columns%5B1%5D%5Bsearchable%5D=true&columns%5B1%5D%5Borderable%5D=true&columns%5B1%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B1%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B2%5D%5Bdata%5D=firmName&columns%5B2%5D%5Bname%5D=&columns%5B2%5D%5Bsearchable%5D=true&columns%5B2%5D%5Borderable%5D=true&columns%5B2%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B2%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B3%5D%5Bdata%5D=confirmationDateKub&columns%5B3%5D%5Bname%5D=&columns%5B3%5D%5Bsearchable%5D=true&columns%5B3%5D%5Borderable%5D=true&columns%5B3%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B3%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B4%5D%5Bdata%5D=confirmationDateKt&columns%5B4%5D%5Bname%5D=&columns%5B4%5D%5Bsearchable%5D=true&columns%5B4%5D%5Borderable%5D=true&columns%5B4%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B4%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B5%5D%5Bdata%5D=documentPathKub&columns%5B5%5D%5Bname%5D=&columns%5B5%5D%5Bsearchable%5D=true&columns%5B5%5D%5Borderable%5D=true&columns%5B5%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B5%5D%5Bsearch%5D%5Bregex%5D=false&columns%5B6%5D%5Bdata%5D=documentPathKt&columns%5B6%5D%5Bname%5D=&columns%5B6%5D%5Bsearchable%5D=true&columns%5B6%5D%5Borderable%5D=true&columns%5B6%5D%5Bsearch%5D%5Bvalue%5D=&columns%5B6%5D%5Bsearch%5D%5Bregex%5D=false&order%5B0%5D%5Bcolumn%5D=0&order%5B0%5D%5Bdir%5D=asc&start=0&length=13243&search%5Bvalue%5D=" + parsed + "&search%5Bregex%5D=false&_token=" + token.Substring(2, token.Length - 3);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                var result = reader.ReadToEnd();
                dynamic json = JsonConvert.DeserializeObject(result);
                stream.Dispose();
                reader.Dispose();
                foreach (dynamic item in json.data)
                {
                    KtubKt ktub = new KtubKt
                    {
                        Name = item.name,
                        FirmName = item.firmName,
                        ConfirmationDateKt = item.confirmationDateKt,
                        ConfirmationDateKub = item.confirmationDateKub,
                        DocumentPathKt = item.documentPathKt,
                        DocumentPathKub = item.documentPathKub,
                        Element = item.element
                    };
                    await _ktubKtService.AddAsync(ktub);
                }

                return Ok(json.data);
            }
            catch (Exception)
            {
           
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Kullanıcıya göre sağlık birimlerine stok tanımlanması

        [Route("SBList")]
        [HttpGet]
        public async Task<IHttpActionResult> SaglikBirimiList()
        {
            return Ok(await _ilacService.SaglikBirimiUserId(User.Identity.GetUserId()));
        }

        //sağlık birimine göre tanımlanmış stok listesi için yazıldı
        [Route("SBStokList/{SaglikBirimi_Id}/{status}")]
        [HttpGet]
        public async Task<IHttpActionResult> SaglikBirimineGoreStokListesi(int SaglikBirimi_Id, bool status)
        {
            //return Ok(await _ilacStokService.FindAllAsync(new IlacStok() { SaglikBirimi_Id = SaglikBirimi_Id, Status = status }));
            return Ok(await _ilacStokService.IlacStokHesaplari(SaglikBirimi_Id, status));
        }
        #endregion

        #region İlaç adı ara==> 2-3 StartWith/4-5 Contains/6 Soundex metoduna göre arama

        [Route("Ara/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> ilacAra(string value)
        {
            return Ok(await _ilacService.IlacAdiAra(karakterCevir(value.ToUpper())));
        }

        public static string karakterCevir(string kelime)
        {
            string mesaj = kelime;
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş' };
            char[] newValue = new char[] { 'o', 'O', 'u', 'U', 'c', 'C', 'I', 'i', 'G', 'g', 'S', 's' };
            for (int sayac = 0; sayac < oldValue.Length; sayac++)
            {
                mesaj = mesaj.Replace(oldValue[sayac], newValue[sayac]);
            }
            return mesaj;
        }


        [Route("KubKtAra")]
        [HttpPost]
        public async Task<IHttpActionResult> KtubKtAra(dynamic value)
        {
            return Ok(await _ilacService.KtubKtAra(value.IlacAdi.ToString()));
        }

        #endregion

        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }

        #region stok tanımlama kaydı/güncelleme/silme
        /// <summary>
        /// ilaç stok ana kaydını yapılır.
        /// </summary>
        /// <param name="ilacStok"></param>
        /// <returns></returns>
        [Route("AddIlacStok")]
        [HttpPost]
        public async Task<IlacStok> Post(IlacStok ilacStok)
        {
            ilacStok.Status = true;
            //ilacStok.StokId = Guid.NewGuid();
            //string IP = HttpContext.Current.Request.Params["HTTP_CLIENT_IP"] ?? HttpContext.Current.Request.UserHostAddress;
            //ilacStok.StokId = GuidGenerator.GenerateTimeBasedGuid(GuncelTarih(), IPAddress.Parse(IP));//zaman tabanlı guid
            ilacStok.StokId = GuidGenerator.GenerateTimeBasedGuid(GuncelTarih());
            return await _ilacStokService.AddAsync(ilacStok);
        }

        /// <summary>
        /// İlaç Stok Güncellemesi ve Silme şlemi yapılır.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ilacStok"></param>
        /// <returns></returns>
        [Route("UpdateIlacStok")]
        [HttpPut]
        public async Task<IlacStok> Put(Guid key, IlacStok ilacStok)
        {
            return await _ilacStokService.UpdateAsync(ilacStok, key);
        }


        #endregion

        #region Stok Girişi Kayıt/Güncelleme/Silme/Tablo Görüntüsü
        /// <summary>
        /// sağlık birimi id göre tablo görüntüsü
        /// </summary>
        /// <param name="SB_id"></param>
        /// <returns></returns>
        [Route("StokGirisi/{SB_id}/{st}")]
        [HttpGet]
        public async Task<IHttpActionResult> StokGirisi(int SB_id, bool st)
        {
            return Ok(await _ilacStokGirisiService.IlacStokGirisiView(SB_id, st));
        }

        [Route("StokGirisi")]
        [HttpPost]
        public async Task<IlacStokGirisi> StokGirisiAdd(IlacStokGirisi ilacStokGirisi)
        {
            try
            {
                ilacStokGirisi.Status = true;
                ilacStokGirisi.UserId = User.Identity.GetUserId();
                ilacStokGirisi.Id = GuidGenerator.GenerateTimeBasedGuid(GuncelTarih());//zaman tabanlı guid
                return await _ilacStokGirisiService.AddAsync(ilacStokGirisi);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        var propName = err.PropertyName;
                        var errMess = err.ErrorMessage;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return ilacStokGirisi;
        }


        [Route("StokGirisiGuncelle")]
        [HttpPut]
        public async Task<IlacStokGirisi> StokGirisiUpdate(IlacStokGirisi ilacStokGirisi, Guid key)
        {
            ilacStokGirisi.UserId = User.Identity.GetUserId();
            return await _ilacStokGirisiService.UpdateAsync(ilacStokGirisi, key);
        }

        [Route("StokGirisiSil")]
        [HttpPost]
        public async Task<IlacStokGirisi> StokGirisiDelete(IlacStokGirisi ilacStokGirisi, Guid key)
        {
            bool ws = ilacStokGirisi.Status == true ? false : true;
            ilacStokGirisi.Status = ws;
            ilacStokGirisi.UserId = User.Identity.GetUserId();
            return await _ilacStokGirisiService.UpdateAsync(ilacStokGirisi, key);
        }

        #endregion

        #region İlac Sarfı 

        #endregion

    }
}
