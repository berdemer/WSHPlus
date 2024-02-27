using ExcelDataReader;
using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
	[RoutePrefix("api/radyoloji")]
	public class RadyolojiController : ApiController
	{

		private IRadyolojiService _radyolojiService { get; set; }
		private IRevirIslemService _revirIslemService { get; set; }
		private IPersonelService _personelService { get; set; }
		private IExcelAzureService _excelAzureService { get; set; }
		public RadyolojiController(IRadyolojiService radyolojiService,
			IPersonelService personelService,
			IRevirIslemService revirIslemService,
			IExcelAzureService excelAzureService
			)
		{
			_radyolojiService = radyolojiService;
			_revirIslemService = revirIslemService;
			_personelService = personelService;
			_excelAzureService = excelAzureService;
		}


        #region Radyoloji kayýt
        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/excel");
		[HttpGet]
		public async Task<IHttpActionResult> Tanimlari()
		{
			try
			{
                List<ExpandoObject> expandoList = new List<ExpandoObject>();
                OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
                sbConnection.DataSource = ServerUploadFolder2 + "\\radyoloji.xls";
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
                    Cmd.CommandText = "Select * from [sayfa1$]";
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
			catch (Exception ex)
			{

				throw;
			}
			
		}

		[Route("azurtanimi")]
		[HttpGet]
		public async Task<IHttpActionResult> TanimlariAzure()
		{
			return Ok(await _excelAzureService.AzurDeposundakiExceldenVeriAl(ConfigurationManager.AppSettings["storage:account:name"],
				ConfigurationManager.AppSettings["storage:account:key"], "excel", "radyoloji.xls", 0));

		}

		[HttpPut]
		public async Task<IHttpActionResult> Put(int key, [FromBody]Radyoloji radyoloji)
		{
			TimeSpan zaman = new TimeSpan();
			zaman = radyoloji.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
			radyoloji.Tarih = GuncelTarih();
			radyoloji.UserId = User.Identity.GetUserId();
			return Ok(await _radyolojiService.UpdateAsync(radyoloji,key));
		}

		[Route("{SB_Id}/{prt}")]
		[HttpPost]
		public async Task<Radyoloji> Post(Radyoloji radyoloji, int SB_Id, bool prt)
		{
			RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =radyoloji.MuayeneTuru , IslemDetayi ="Radyoloji" , UserId = User.Identity.GetUserId() }, prt);
			radyoloji.RevirIslem_Id = rv.RevirIslem_Id;
			radyoloji.Protokol = rv.Protokol;
			radyoloji.Tarih = GuncelTarih();
			radyoloji.UserId = User.Identity.GetUserId();
			return await _radyolojiService.AddAsync(radyoloji);
		}

		[HttpDelete]
		public async Task<IHttpActionResult> Delete(int id)
		{
			Radyoloji radyoloji = await _radyolojiService.GetAsync(id);
			TimeSpan zaman = new TimeSpan();
			zaman = radyoloji.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
			await _radyolojiService.DeleteAsync(id);
			return Ok(await _revirIslemService.DeleteAsync((int)radyoloji.RevirIslem_Id));
		}
		#endregion

		#region Radyoloji excelden toplu kayýt

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
					TcNo="",
					radyolojikIslem = "",
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
		private DateTime GuncelTarih()
		{
			TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
			return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
		}

		[Authorize(Roles = "ISG_Admin , Admin")]
		[Route("InsertRadyolojiList")]
		[HttpPost]
		public async Task<IHttpActionResult> InsertRadyolojiList(JArray paramList)
		{
			List<ExpandoObject> expandoList = new List<ExpandoObject>();
			foreach (dynamic jsItem in paramList)
			{
				dynamic data = new ExpandoObject();
				IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
				try
				{
					dictionary.Add("Donus_Id", jsItem.id);

					if (String.IsNullOrEmpty(Cast(jsItem.radyolojikIslem, typeof(string))) ||
					   String.IsNullOrEmpty(Cast(jsItem.Sonuc, typeof(string)))||String.IsNullOrEmpty(Cast(jsItem.TcNo, typeof(string))))
					{
						return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Ýþlem adý,sonuç ve Tc No olmadan kayýt yapamazsýnýz."));
					};
					Personel se = new Personel()
					{
						TcNo = jsItem.TcNo,
					};
					Personel pers = await _personelService.FindAsync(se);//tc Kaydýný sorgula
					if (pers != null) {
					 Radyoloji radyoloji = new Radyoloji()
					{
						RadyolojikIslem = Cast(jsItem.radyolojikIslem, typeof(string)),
						RadyolojikSonuc = Cast(jsItem.Sonuc, typeof(string)),
						IslemTarihi = GuncelTarih(),
						Personel_Id = pers.Personel_Id,
						MuayeneTuru = "Normal Muayene Ýþlemleri",
						Tarih=GuncelTarih(),
						UserId= User.Identity.GetUserId(),                      
					 };

					if (!radyoloji.RadyolojikSonuc.Equals(null))
					{
						RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = 1, IslemTuru = "Revir Ýþlemleri", IslemDetayi = "Radyoloji", UserId = User.Identity.GetUserId() },false );
						radyoloji.RevirIslem_Id = rv.RevirIslem_Id;
						Radyoloji RadyolojiInsert = await _radyolojiService.AddAsync(radyoloji);
						dictionary.Add("id", (RadyolojiInsert.Radyoloji_Id).ToString());
						dictionary.Add("TcNo", pers.TcNo);
						dictionary.Add("RadyolojikIslem", (RadyolojiInsert.RadyolojikIslem).ToString());
						dictionary.Add("RadyolojikSonuc", RadyolojiInsert.RadyolojikSonuc);
						dictionary.Add("tarih", RadyolojiInsert.Tarih);
						dictionary.Add("durum", true);
						dictionary.Add("Kayýt Durumu", "Baþarýlý bir kayýt yapýldý...");
					}
					} else {

						dictionary.Add("TcNo", Cast(jsItem.TcNo, typeof(string)));
						dictionary.Add("RadyolojikIslem", Cast(jsItem.radyolojikIslem, typeof(string)));
						dictionary.Add("RadyolojikSonuc", Cast(jsItem.Sonuc, typeof(string)));
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
							dictionary.Add("RadyolojikIslem", Cast(jsItem.radyolojikIslem, typeof(string)));
							dictionary.Add("RadyolojikSonuc", Cast(jsItem.Sonuc, typeof(string)));
							dictionary.Add("tarih", "Kayýt Yapýlmadý!");
							dictionary.Add("durum", false);
							dictionary.Add("Kayýt Durumu", "Validation Hatasý" + propName + ": " + errMess + " Sistem deðerleri ile verilen alanlar uyuþmuyor. Bu alaný kaldýrýn.");
						}
					}
				}
				catch (Exception ex)
				{
					dictionary.Add("TcNo",Cast(jsItem.TcNo, typeof(string)));
					dictionary.Add("RadyolojikIslem", Cast(jsItem.radyolojikIslem, typeof(string)));
					dictionary.Add("RadyolojikSonuc", Cast(jsItem.Sonuc, typeof(string)));
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

		//[Route("ExtractRadyolojiList")]
		//[HttpPost]
		//public async Task<IHttpActionResult> ExtractRadyolojiList(JArray paramList)
		//{
		//	List<ExpandoObject> expandoList = new List<ExpandoObject>();
		//	foreach (dynamic jsItem in paramList)
		//	{
		//		dynamic data = new ExpandoObject();
		//		IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
		//		dictionary.Add("Donus_Id", jsItem.id);
		//		Radyoloji Radyoloji = new Radyoloji()
		//		{
		//			RadyolojiCode = Cast(jsItem.RadyolojiCode, typeof(string))
		//		};
		//		Radyoloji RadyolojiVarmi = await _radyolojiService.FindAsync(Radyoloji);
		//		if (RadyolojiVarmi != null)
		//			dictionary.Add("durum", true);
		//		else dictionary.Add("durum", false);
		//		expandoList.Add(data);
		//		data = null;
		//	}
		//	return Ok(await Task.Run(() => expandoList));
		//}

		//[Route("RadyolojiUpdateList")]
		//[HttpPost]
		//public async Task<IHttpActionResult> UpdateRadyolojiList(JArray paramList)
		//{
		//	List<ExpandoObject> expandoList = new List<ExpandoObject>();
		//	foreach (dynamic jsItem in paramList)
		//	{
		//		dynamic data = new ExpandoObject();
		//		IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
		//		try
		//		{
		//			dictionary.Add("Donus_Id", jsItem.id);
		//			if (String.IsNullOrEmpty(Cast(jsItem.RadyolojiCode, typeof(string))) ||
		//			  String.IsNullOrEmpty(Cast(jsItem.ICDTanimi, typeof(string))))
		//			{
		//				return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tanýmý ve Kodu olmadan kayýt yapamazsýnýz."));
		//			};

		//			Radyoloji Radyoloji = new Radyoloji()
		//			{
		//				RadyolojiCode = Cast(jsItem.RadyolojiCode, typeof(string))
		//			};

		//			if (!Radyoloji.RadyolojiCode.Equals(null))
		//			{
		//				Radyoloji RadyolojiVarmi = await _radyolojiService.FindAsync(Radyoloji);
		//				if (RadyolojiVarmi != null)
		//				{
		//					RadyolojiVarmi.ICDTanimi = Cast(jsItem.ICDTanimi, typeof(string));

		//					Radyoloji RadyolojiUpdate = await _radyolojiService.UpdateAsync(RadyolojiVarmi, RadyolojiVarmi.Radyoloji_Id);

		//					dictionary.Add("Radyoloji Kodu", RadyolojiUpdate.RadyolojiCode);
		//					dictionary.Add("Radyoloji Tanýmý", RadyolojiUpdate.ICDTanimi);
		//					dictionary.Add("Arama Önceliði", RadyolojiUpdate.AramaOnceligi);
		//					dictionary.Add("durum", true);
		//					dictionary.Add("Kayýt Durumu", "Baþarýlý bir kayýt yapýldý...");
		//				}
		//				else
		//				{
		//					dictionary.Add("Radyoloji Kodu", jsItem.RadyolojiCode);
		//					dictionary.Add("Radyoloji Tanýmý", jsItem.ICDTanimi);
		//					dictionary.Add("Arama Önceliði", jsItem.AramaOnceligi);
		//					dictionary.Add("durum", false);
		//					dictionary.Add("Kayýt Durumu", "Bu Kodun Kaydý Yapýlmamýþ.Veri sisteminde yok.Taným güncellenmesi yapýlamadý.");
		//				};

		//			}
		//			else
		//			{
		//				dictionary.Add("Radyoloji Kodu", jsItem.RadyolojiCode);
		//				dictionary.Add("Radyoloji Tanýmý", jsItem.ICDTanimi);
		//				dictionary.Add("Arama Önceliði", jsItem.AramaOnceligi);
		//				dictionary.Add("durum", false);
		//				dictionary.Add("Kayýt Durumu", "Radyoloji Kodu Olmadan Kayýt Yapýlamaz.");
		//			}
		//		}
		//		catch (DbEntityValidationException ex)
		//		{
		//			foreach (var errs in ex.EntityValidationErrors)
		//			{
		//				foreach (var err in errs.ValidationErrors)
		//				{
		//					var propName = err.PropertyName;
		//					var errMess = err.ErrorMessage;
		//					dictionary.Add("Radyoloji Kodu", jsItem.RadyolojiCode);
		//					dictionary.Add("Radyoloji Tanýmý", jsItem.ICDTanimi);
		//					dictionary.Add("Arama Önceliði", jsItem.AramaOnceligi);
		//					dictionary.Add("durum", false);
		//					dictionary.Add("Kayýt Durumu", "Validation Hatasý" + propName + ": " + errMess + " Sistem deðerleri ile verilen alanlar uyuþmuyor. Bu alaný kaldýrýn.");
		//				}
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			dictionary.Add("Radyoloji Kodu", jsItem.RadyolojiCode);
		//			dictionary.Add("Radyoloji Tanýmý", jsItem.ICDTanimi);
		//			dictionary.Add("Arama Önceliði", jsItem.AramaOnceligi);
		//			dictionary.Add("durum", false);
		//			dictionary.Add("Kayýt Durumu", "Sistem Hatasý: " + ex.Message.ToString());
		//		}

		//		expandoList.Add(data);
		//		data = null;
		//	}
		//	return Ok(await Task.Run(() => expandoList));
		//}

		#endregion

	}
}
