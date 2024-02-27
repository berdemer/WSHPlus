using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/isgTopluEgitimi")]
    public class IsgTopluEgitimiController : ApiController
    {
        private ApplicationUserManager _AppUserManager = null;
        private IIsgEgitimiService _isgEgitimiService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private IIsgTopluEgitimiService _isgTopluEgitimiService { get; set; }
        private IISG_TopluEgitimSablonlariService _isg_TopluEgitimSablonlariService { get; set; }
        private IPersonelService _personelService { get; set; }
        public IsgTopluEgitimiController(IIsgEgitimiService isgEgitimiService,
            IRevirIslemService revirIslemService,
            IIsgTopluEgitimiService isgTopluEgitimiService,
            IISG_TopluEgitimSablonlariService isg_TopluEgitimSablonlariService,
            IPersonelService personelService
            )
        {
            _isgEgitimiService = isgEgitimiService;
            _revirIslemService = revirIslemService;
            _isgTopluEgitimiService = isgTopluEgitimiService;
            _isg_TopluEgitimSablonlariService = isg_TopluEgitimSablonlariService;
            _personelService = personelService;
        }
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        #region Eğitim Alan Personel listesi
        [Route("EgtmAlList/{Sirket_Id}/{year}")]
        [HttpGet]
        public async Task<IHttpActionResult> EgitimAlanlarListesi(int Sirket_Id,int year)
        {
            try
            {
                 return Ok(await _isgEgitimiService.EgitimAlanPersAsyc(Sirket_Id,year));
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.InnerException.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(ex.Message)),
                    ReasonPhrase = ex.Source

                };
                throw new HttpResponseException(resp);
            }
        }
        #endregion
        #region Toplu Eğitim Kayıtları kontrollerinin yapıldığı bölüm

        [Route("{key}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, JObject jIsgEgitimi)
        {
            IsgTopluEgitimi cv = await _isgTopluEgitimiService.GetAsync(key);
            dynamic json = jIsgEgitimi;
            if (json.egitimYapisi.belgeTipi == null) throw new ArgumentException("Belge Tipini Giriniz!");
            if (json.egitimYapisi.egitimtur == null) throw new ArgumentException("Eğitim Türünü Giriniz!");
            if (json.egitimYapisi.isgProfTckNo == null || json.dersler.IsgProfesyoneliAdi == null) throw new ArgumentException("ISG Profesyonelini Giriniz!");
            if (json.egitimYapisi.egitimYer == null) throw new ArgumentException("Eğitim Yerini Giriniz!");
            string[] vb = Convert.ToString(json.egitimYapisi.egitimTarihi).Split('-');
            DateTime egitimTarihi = new DateTime(int.Parse(vb[2]), int.Parse(vb[1]), int.Parse(vb[0]));           
            cv.IsgTopluEgitimiJson = Convert.ToString(json);
            cv.belgeTipi = json.egitimYapisi.belgeTipi ?? Cast(json.egitimYapisi.belgeTipi, typeof(int));
            cv.calisanObjects = Convert.ToString(json.egitimYapisi.calisanObjects);
            cv.egiticiTckNo = Convert.ToString(json.egitimYapisi.egiticiTckNo);
            cv.egitimObjects = Convert.ToString(json.egitimYapisi.egitimObjects);
            cv.egitimTarihi = egitimTarihi;
            cv.egitimtur = json.egitimYapisi.egitimtur ?? Cast(json.egitimYapisi.egitimtur, typeof(int));
            cv.egitimYer = json.egitimYapisi.egitimYer ?? Cast(json.egitimYapisi.egitimYer, typeof(int));
            cv.isgProfTckNo = json.egitimYapisi.isgProfTckNo ?? Cast(json.egitimYapisi.isgProfTckNo, typeof(long));
            cv.status = json.dersler.status ?? Cast(json.dersler.status, typeof(int));
            cv.nitelikDurumu = Convert.ToString(json.dersler.nitelikDurumu);
            IsgTopluEgitimi se = new IsgTopluEgitimi();
            try
            {
                //[{'durum':'Kaydet','id':1},{'durum':'Planla','id':2},{'durum':'Bakanlığa Gönder','id':3},]
                if (cv.status == 2 && await _isgEgitimiService.SilAsync(key)) {
                    se = await _isgTopluEgitimiService.UpdateAsync(cv, key);
                }else
                if (cv.status == 1)
                {
                    if (await _isgEgitimiService.SilAsync(key))//Tüm isgeğitim tablosundaki veriler silindi.
                    {
                        string[] calisanlar = cv.calisanObjects.TrimStart('[').TrimEnd(']').Split(',');
                        foreach (var item in calisanlar)
                        {
                            Personel p = await _personelService.TcNoKontrol(item.Trim());
                            foreach (dynamic egitim in json.dersler.KaydaGidecekDersler)
                            {
                                IsgEgitimi egt = new IsgEgitimi()
                                {
                                    Personel_Id = p.Personel_Id,
                                    Tarih = GuncelTarih(),
                                    UserId = User.Identity.GetUserId(),
                                    Egitim_Turu = Convert.ToString(egitim.konu),
                                    Egitim_Suresi = Cast(egitim.egitimSuresi, typeof(int)),
                                    VerildigiTarih = egitimTarihi,
                                    Tanimi = Convert.ToString(egitim.tipi),
                                    DersTipi = TipiniVer(Convert.ToString(egitim.tipi)),
                                    IsgEgitimiVerenPersonel = Convert.ToString(json.dersler.IsgProfesyoneliAdi),
                                    IsgTopluEgitimi_Id = key
                                };
                                await _isgEgitimiService.AddAsync(egt);
                            }
                        }
                    };
                    se = await _isgTopluEgitimiService.UpdateAsync(cv, key);
                }
                else
                {
                    throw new ArgumentException("Eğitim Güncellenmeye Kapanmıştır!");
                };
               
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.InnerException.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(ex.Message)),
                    ReasonPhrase = ex.Source

                };
                throw new HttpResponseException(resp);
            }
            return Ok(se);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(JObject jIsgEgitimi)
        {
            try
            {
                dynamic json = jIsgEgitimi;
                if (json.egitimYapisi.belgeTipi == null) throw new ArgumentException("Belge Tipini Giriniz!");
                if (json.egitimYapisi.egitimtur == null) throw new ArgumentException("Eğitim Türünü Giriniz!");
                if (json.egitimYapisi.isgProfTckNo == null || json.dersler.IsgProfesyoneliAdi == null) 
                    throw new ArgumentException("ISG Profesyonelini Giriniz!");
                if (json.egitimYapisi.egitimYer == null) throw new ArgumentException("Eğitim Yerini Giriniz!");
                DateTime guncelZaman = GuncelTarih();
                string[] cv = Convert.ToString(json.egitimYapisi.egitimTarihi).Split('-');
                DateTime egitimTarihi = new DateTime(int.Parse(cv[2]), int.Parse(cv[1]), int.Parse(cv[0]));
                IsgTopluEgitimi isgEgitimiResult = new IsgTopluEgitimi
                {
                    IsgTopluEgitimiJson = Convert.ToString(json),
                    belgeTipi = json.egitimYapisi.belgeTipi ?? Cast(json.egitimYapisi.belgeTipi, typeof(int)),
                    calisanObjects = Convert.ToString(json.egitimYapisi.calisanObjects),
                    egiticiTckNo = Convert.ToString(json.egitimYapisi.egiticiTckNo),
                    egitimObjects = Convert.ToString(json.egitimYapisi.egitimObjects),
                    egitimTarihi = egitimTarihi,
                    egitimtur = json.egitimYapisi.egitimtur ?? Cast(json.egitimYapisi.egitimtur, typeof(int)),
                    firmaKodu = Convert.ToString(json.egitimYapisi.firmaKodu),
                    egitimYer = json.egitimYapisi.egitimYer ?? Cast(json.egitimYapisi.egitimYer, typeof(int)),
                    imzalıDeger = Cast(json.egitimYapisi.imzalıDeger, typeof(string)),
                    isgProfTckNo = json.egitimYapisi.isgProfTckNo ?? Cast(json.egitimYapisi.isgProfTckNo, typeof(long)),
                    sgkTescilNo = Cast(json.egitimYapisi.sgkTescilNo, typeof(string)),
                    Sirket_id = json.Sirket_id ?? Cast(json.Sirket_id, typeof(int)),
                    sorguNo = Cast(json.egitimYapisi.sorguNo, typeof(string)),
                    status = json.dersler.status ?? Cast(json.dersler.status, typeof(int)),
                    Tarih = guncelZaman,
                    UserId = User.Identity.GetUserId(),
                    nitelikDurumu = Convert.ToString(json.dersler.nitelikDurumu)
                };
                //Karşılaştırma
                string[] calisanlar = isgEgitimiResult.calisanObjects.TrimStart('[').TrimEnd(']').Split(',');
                List<string> calisanlarG = new List<string>();
                foreach (var i in calisanlar)
                {
                    calisanlarG.Add(i.Trim());
                }
                ICollection<IsgTopluEgitimi> vb = await _isgTopluEgitimiService.KarsilastirAsync(isgEgitimiResult);

                foreach (IsgTopluEgitimi egitim in vb)
                {
                    string[] kayitliCalisanlar = egitim.calisanObjects.TrimStart('[').TrimEnd(']').Split(',');
                    List<string> kayitliCalisanlarG = new List<string>();
                    foreach (var i in kayitliCalisanlar)
                    {
                        kayitliCalisanlarG.Add(i.Trim());
                    }
                    var result = kayitliCalisanlarG.Intersect(calisanlarG).ToList();
                    if (result.Count > 0)
                    {
                        throw new ArgumentException($"Aynı günde {string.Join(",", result)} " +
                            $"TcNo lu personelin eğitim çakışması var .Kayıt İptal Edildi.");
                    }
                }
                ////////////////
                var isgEgitimiResult2 = await _isgTopluEgitimiService.AddAsync(isgEgitimiResult);
                //[{'durum':'Kaydet','id':1},{'durum':'Planla','id':2},{'durum':'Bakanlığa Gönder','id':3},]
                if (isgEgitimiResult.status == 1 || isgEgitimiResult.status == 3)
                {
                    foreach (var item in calisanlar)
                    {
                        Personel p = await _personelService.TcNoKontrol(item.Trim());
                        foreach (dynamic egitim in json.dersler.KaydaGidecekDersler)
                        {
                            IsgEgitimi egt = new IsgEgitimi()
                            {
                                Personel_Id = p.Personel_Id,
                                Tarih = guncelZaman,
                                UserId = User.Identity.GetUserId(),
                                Egitim_Turu = Convert.ToString(egitim.konu),
                                Egitim_Suresi = Cast(egitim.egitimSuresi, typeof(int))??0,
                                VerildigiTarih = egitimTarihi,
                                Tanimi = Convert.ToString(egitim.tipi),
                                DersTipi = TipiniVer(Convert.ToString(egitim.tipi)),
                                IsgEgitimiVerenPersonel = Convert.ToString(json.dersler.IsgProfesyoneliAdi),
                                IsgTopluEgitimi_Id = isgEgitimiResult2.IsgTopluEgitimi_Id
                            };
                            //mükerrer kayıt olabilir ama farklı kişiler aynı eğitimi vermek isteyebilir.
                            await _isgEgitimiService.AddAsync(egt);
                        }
                    }
                }

                return Ok(isgEgitimiResult2);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.InnerException.Message);
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(ex.Message)),
                    ReasonPhrase = ex.Source

                };
                throw new HttpResponseException(resp);
            }
        }
       
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            dynamic data = new ExpandoObject();
            IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
            IsgTopluEgitimi e = new IsgTopluEgitimi
            {
                IsgTopluEgitimi_Id = id
            };
            IsgTopluEgitimi eg = await _isgTopluEgitimiService.FindAsync(e);

            if (eg != null)
            {
            dictionary.Add("Id", eg.IsgTopluEgitimi_Id);
            dictionary.Add("JData", JsonConvert.DeserializeObject(eg.IsgTopluEgitimiJson));
            } else return Content(HttpStatusCode.NotFound, "Verilen id göre data bulunamadı!."); ;
            return Ok(await Task.Run(() => dictionary));
        }

        [Route("TopluEgitimListesi/{year}/{Sirket_Id}")]
        [HttpGet]
        public async  Task<IHttpActionResult> TopluEgitimListesi(int Sirket_id, int year)
        {
            IList<ApplicationUser> isgUsers = this.AppUserManager.Users.ToList();
            ICollection<IsgTopluEgitimi> egitimler= await _isgTopluEgitimiService.TopluEgitimListesiAsync(Sirket_id, year);
            
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            string kayit;
            foreach (IsgTopluEgitimi i in egitimler)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                ApplicationUser user = isgUsers.Where(x => x.TcNo == i.egiticiTckNo).ToList().FirstOrDefault();
                dictionary.Add("Id", i.IsgTopluEgitimi_Id);
                dictionary.Add("AdiSoyadi", user.FirstName+" "+ user.LastName);
                dictionary.Add("Tarih", i.egitimTarihi);
                dictionary.Add("BelgeTipi", i.belgeTipi == 1 ? "İşyeri hekimi" : "İş güvenliği uzmanı") ;
                dictionary.Add("Meslek",user.Meslek);
                switch (i.status)
                {
                    case 1:
                        kayit = "Personelere Kayıtlı.";
                        break;
                    case 2:
                        kayit = "Planlama Yapıldı.";
                        break;
                    default:
                        kayit = "Bakanlık Girdisinde.";
                        break;
                }
                dictionary.Add("KayitDurumu", kayit);
                expandoList.Add(data);
                data = null;
            }

            return Ok(await Task.Run(() => expandoList));
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            //await _revirIslemService.DeleteAsync((int)aNT.RevirIslem_Id);
            return await _isgTopluEgitimiService.DeleteAsync(id);
        }

        #endregion
        #region Eğitici personellerin listesini veren kontrol
        [Route("PrEgList/{Search}/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> PersonelEgitimListesi(string Search, int Sirket_Id)
        {         
            return Ok(await _personelService.EgitimPersonelleriListesi(Sirket_Id, true, Search));
        }
        #endregion        
        #region Şablon kontrolleri
        [Route("sablon")]
        [HttpGet]
        public async Task<IEnumerable<ISG_TopluEgitimSablonlari>> GetAll()
        {
            return await _isg_TopluEgitimSablonlariService.GetAllAsync();
        }
        [Route("sablon")]
        [HttpPost]
        public async Task<ISG_TopluEgitimSablonlari> Post(ISG_TopluEgitimSablonlari data)
        {
            data.UserId = User.Identity.GetUserId();
            data.Status = true;
            return await _isg_TopluEgitimSablonlariService.AddAsync(data);
        }

        [Route("sablon/{iid}")]
        [HttpDelete]
        public async Task<int> Deleten(int iid)
        {
            return await _isg_TopluEgitimSablonlariService.DeleteAsync(iid);
        }
        #endregion
        #region Yardımcı prosedürler fonksiyonlar
        private int TipiniVer(string df)
        {
            switch (df.Substring(0, 1).ToLower(new CultureInfo("tr-TR", false)))
            {
                case "g":
                    return 1;
                case "s":
                    return 2;
                case "t":
                    return 3;
                default:
                    return 0;
            }
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
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
        #endregion

    }
}
