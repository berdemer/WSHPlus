using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using ISG.WebApi.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/pm")]
    public class PmController : BaseApiController
    {
        private IPersonelService _personelService { get; set; }
        private IPersonelDetayiService _personelDetayiService { get; set; }
        private IAdresService _adresService { get; set; }
        private IImageUploadService _imageUploadService { get; set; }
        private ISirketService _sirketService { get; set; }
        private ISirketBolumuService _sirketBolumuService { get; set; }
        private IANTService _aNTService { get; set; }
        private IBoyKiloService _boyKiloService { get; set; }
        private ILaboratuarService _laboratuarService { get; set; }
        private IHemogramService _hemogramService { get; set; }
        private IRadyolojiService _radyolojiService { get; set; }
        private IEKGService _eKGService { get; set; }
        private IPersonelMuayeneService _personelMuayeneService { get; set; }
        private IICDSablonuService _iCDSablonuService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private IIcRaporService _icRaporService { get; set; }
        private IRevirTedaviService _revirTedaviService { get; set; }
        private IDisRaporService _disRaporService { get; set; }
        private IIlacSarfCikisiService _ilacSarfCikisiService { get; set; }
        private IMailService _mailService { get;  set; }
        private ISaglikBirimiService _saglikBirimiService { get; set; }
        private IIkazService _ikazService { get; set; }

        private ICalisma_DurumuService _calisma_DurumuService { get; set; }

        public PmController(
            IPersonelService personelService,
            IPersonelDetayiService personelDetayiService,
            IAdresService adresService,
            ICalisma_DurumuService calisma_DurumuService,
            IImageUploadService imageUploadService,
            ISirketBolumuService sirketBolumuService,
            ISirketService sirketService,
            IANTService aNTService,
            IBoyKiloService boyKiloService,
            ILaboratuarService laboratuarService,
            IHemogramService hemogramService,
            IRadyolojiService radyolojiService,
            IEKGService eKGService,
            IPersonelMuayeneService personelMuayeneService,
            IICDSablonuService iCDSablonuService,
            IRevirIslemService revirIslemService,
            IRevirTedaviService revirTedaviService,
            IIcRaporService icRaporService,
            IIlacSarfCikisiService ilacSarfCikisiService,
            IDisRaporService disRaporService,
            IMailService  mailService,
            ISaglikBirimiService saglikBirimiService,
            IIkazService ikazService
            )
        {
            _personelService = personelService;
            _personelDetayiService = personelDetayiService;
            _adresService = adresService;
            _imageUploadService = imageUploadService;
            _imageUploadService = imageUploadService;
            _sirketService = sirketService;
            _sirketBolumuService = sirketBolumuService;
            _aNTService = aNTService;
            _boyKiloService = boyKiloService;
            _laboratuarService = laboratuarService;
            _hemogramService = hemogramService;
            _radyolojiService = radyolojiService;
            _eKGService = eKGService;
            _personelMuayeneService = personelMuayeneService;
            _iCDSablonuService = iCDSablonuService;
            _revirIslemService = revirIslemService;
            _revirTedaviService = revirTedaviService;
            _icRaporService = icRaporService;
            _ilacSarfCikisiService = ilacSarfCikisiService;
            _disRaporService = disRaporService;
            _mailService = mailService;
            _saglikBirimiService = saglikBirimiService;
            _ikazService = ikazService;
            _calisma_DurumuService = calisma_DurumuService;
        }
        #region Personel Muayene Arama
        //http://localhost:1943/api/pm/cc694a55-4c01-e611-ac6e-e0b9a5d34c70
        [Route("{GuidId}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid GuidId)
        {
            Personel data = await _personelService.GuidKontrol(GuidId);
            data.PersonelDetayi = await _personelDetayiService.FindAsync(new PersonelDetayi() { PersonelDetay_Id = data.Personel_Id });
            data.Adresler = await _adresService.FindAllAsync(new Adres() { Adres_Id = data.Personel_Id });
            data.Ikazlar = await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = data.Personel_Id, Status = true, SonTarih = DateTime.Now });
            data.Calisma_Durumu = await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = data.Personel_Id });
            string calismadurumu = data.Calisma_Durumu.Select(x => x.Calisma_Duzeni).LastOrDefault();

            //Kişinin çalıştığı bölüm ve şirket adı
            SirketBolumu sb = await _sirketBolumuService.FindAsync(new SirketBolumu() { id = Convert.ToInt32(data.Bolum_Id) });
            Sirket st = await _sirketService.FindAsync(new Sirket() { id = Convert.ToInt32(data.Sirket_Id) });
            //Resim için yazldı
            IEnumerable<imageUpload> imgList = await _imageUploadService.FindAllAsync(new imageUploadFilter() { IdGuid = GuidId.ToString(), Folder = "Personel" });
            var img = new List<FileUploadResult>();
            imgList.ToList().ForEach(x => img.Add(new FileUploadResult()
            {//birden fazla img 
                FileLength = x.FileLenght,
                FileName = x.FileName,
                GenericName = x.GenericName,
                LocalFilePath = "uploads/"
            }));
            //data.ANTlari = await _aNTService.FindAllAsync(new ANT() { Personel_Id = data.Personel_Id });
            //data.BoyKilolari = await _boyKiloService.FindAllAsync(new BoyKilo() { Personel_Id = data.Personel_Id });
            //data.Laboratuarlari = await _laboratuarService.FindAllAsync(new Laboratuar() { Personel_Id = data.Personel_Id });
            //data.Hemogramlar = await _hemogramService.FindAllAsync(new Hemogram() { Personel_Id = data.Personel_Id });
            //data.Radyolojileri = await _radyolojiService.FindAllAsync(new Radyoloji() { Personel_Id = data.Personel_Id });
            //data.EKGleri = await _eKGService.FindAllAsync(new EKG() { Personel_Id = data.Personel_Id });
            ICollection<PersonelMuayene> PersonelMuayeneleri = await _personelMuayeneService.FindAllAsync(new PersonelMuayene() { Personel_Id = data.Personel_Id });
            //JArray a = (JArray)JToken.FromObject(PersonelMuayeneleri);
            JArray pm = new JArray(/*http://www.newtonsoft.com/json/help/html/CreateJsonDeclaratively.htm*/
                 from p in PersonelMuayeneleri
                 select new JObject(
                     new JProperty("PersonelMuayene_Id", p.PersonelMuayene_Id),
                     new JProperty("PMJson", JObject.Parse(p.PMJson.ToString())),
                     new JProperty("MuayeneTuru", p.MuayeneTuru),
                     new JProperty("RevirIslem_Id", p.RevirIslem_Id),
                     new JProperty("Personel_Id", p.Personel_Id),
                     new JProperty("Protokol", p.Protokol),
                     new JProperty("Tarih", p.Tarih),
                     new JProperty("UserId", p.UserId)
                     )
                );
            JObject o = (JObject)JToken.FromObject(new { data = data, img = img, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi,sirketId=st.id, bolumId = sb.id, vardiyasi = calismadurumu });
            JObject Jdata = (JObject)o["data"];
            Jdata["PersonelMuayeneleri"] = pm; /*http://www.newtonsoft.com/json/help/html/ModifyJson.htm*/
            return Ok(await Task.Run(() => o));
        }

        #endregion

        #region ICD Şablon Tanımları
        /// <bas>
        /// şablon kaydı
        /// </bas>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [Route("st")]
        [HttpPost]
        public async Task<IHttpActionResult> STPost(JObject jsonData)
        {
            ICDSablonu icdSablonu = await _iCDSablonuService.Kontrol((jsonData["ICDkod"].Value<string>()).Substring(0, 3), User.Identity.GetUserId());
            if (icdSablonu == null)
            {
                return Ok(await Task.Run(() =>
                           _iCDSablonuService.AddAsync(new ICDSablonu() { ICDSablonuJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented), Status = true, Tarih =GuncelTarih(), UserId = User.Identity.GetUserId(), ICDkod = (jsonData["ICDkod"].Value<string>()).Substring(0, 3) })));
            }
            else
            {
                return Ok(await Task.Run(() =>
                            _iCDSablonuService.UpdateAsync(new ICDSablonu() { ICDSablonuJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented), Status = true, Tarih = GuncelTarih(), UserId = User.Identity.GetUserId(), ICDkod = (jsonData["ICDkod"].Value<string>()).Substring(0, 3), ICDSablonu_Id = icdSablonu.ICDSablonu_Id }, icdSablonu.ICDSablonu_Id)));
            };
        }

        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }

        /// <summary>
        /// tanı girilince kullanıcının ve genel şablon idlerini verir.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [Route("st")]
        [HttpGet]
        public async Task<IHttpActionResult> STGet(string val)
        {
            ICollection<ICDSablonu> icdSablonuListesi = await _iCDSablonuService.FindAllAsync(new ICDSablonu() { ICDkod = val.Substring(0, 3) });
            ICDSablonu kullanici = icdSablonuListesi.Where(x => x.UserId == User.Identity.GetUserId()).ToList().LastOrDefault();
            ICDSablonu genel = icdSablonuListesi.Where(x => x.UserId != User.Identity.GetUserId()).ToList().LastOrDefault();
            return Ok(await Task.Run(() => new { kullanici = kullanici == null ? 0 : kullanici.ICDSablonu_Id, genel = genel == null ? 0 : genel.ICDSablonu_Id }));
        }
        /// <summary>
        /// id göre şablonu json olarak döndürür.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [Route("std/{val}")]
        [HttpGet]
        public async Task<IHttpActionResult> STDGet(int val)
        {
            ICDSablonu deger = await _iCDSablonuService.GetAsync(val);
            return Ok(await Task.Run(() => JObject.Parse(deger.ICDSablonuJson.ToString())));
        }


        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            return Ok(await _iCDSablonuService.DeleteAsync(id));
        }
        #endregion

        #region Personel Muayene Kayıt Güncelleme ve Silme

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personelMuayene"></param>
        /// <param name="SB_Id"></param>
        /// <param name="personelGuid"></param>
        /// <returns></returns>
        [Route("{SB_Id}/{personelGuid}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(PersonelMuayene personelMuayene, int SB_Id, Guid personelGuid)
        {
            try
            {
                Personel data = await _personelService.GuidKontrol(personelGuid);
                RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = "PersonelMuayene", IslemDetayi = personelMuayene.MuayeneTuru, UserId = User.Identity.GetUserId() }, true);
                personelMuayene.RevirIslem_Id = rv.RevirIslem_Id;
                personelMuayene.Protokol = rv.Protokol;
                personelMuayene.Tarih = GuncelTarih();
                personelMuayene.UserId = User.Identity.GetUserId();
                personelMuayene.Personel_Id = data.Personel_Id;
                personelMuayene.MuayeneTuru = personelMuayene.MuayeneTuru;
                dynamic aIslemi = JObject.Parse(personelMuayene.PMJson.ToString());//json string dinamik+jobject yapısına atanıyor. aislem burada temel jsondatamız.
                /*jsondata işlemlere tabi tutulup Ekg is eklenmesi gibi sonra kayda alınıyor.
               karşılığında nesle varsa  aIslemi["EKG"]!=null sorgulaması yapabiliyorsun
               aksi halde array varsa (JArray)aIslemi["Lab"]).Count() değeri array çevirip count sayı 
               sorulaması yapabilirsin.
                 */
                if (IsPropertyExist(aIslemi, "EKG"))
                {
                    EKG ekg = new EKG()
                    {
                        MuayeneTuru = personelMuayene.MuayeneTuru,
                        Sonuc = aIslemi.EKG.Sonuc,
                        Personel_Id = data.Personel_Id,
                        Protokol = personelMuayene.Protokol,
                        RevirIslem_Id = personelMuayene.RevirIslem_Id,
                        Tarih = GuncelTarih(),
                        UserId = User.Identity.GetUserId()
                    };
                    ekg = await _eKGService.AddAsync(ekg);
                    aIslemi["EKG"].Add("EKG_Id", ekg.EKG_Id);
                }

                if (IsPropertyExist(aIslemi, "Lab"))
                    if (((JArray)aIslemi["Lab"]).Count() > 0)
                    {
                        Laboratuar lab = new Laboratuar()
                        {
                            MuayeneTuru = personelMuayene.MuayeneTuru,
                            Grubu = "BİYOKİMYA",
                            Sonuc = aIslemi["Lab"].ToString(),
                            Personel_Id = data.Personel_Id,
                            Protokol = (int)personelMuayene.Protokol,
                            RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                            Tarih = GuncelTarih(),
                            UserId = User.Identity.GetUserId()
                        };
                        lab = await _laboratuarService.AddAsync(lab);
                        aIslemi.Add("Laboratuar_Id", lab.Laboratuar_Id);
                    }

                if (IsPropertyExist(aIslemi, "ANT"))
                {
                    ANT ant = new ANT()
                    {
                        TASagKolDiastol = aIslemi.ANT.TASagKolDiastol,
                        TASolKolSistol = aIslemi.ANT.TASolKolSistol,
                        TASagKolSistol = aIslemi.ANT.TASagKolSistol,
                        TASolKolDiastol = aIslemi.ANT.TASolKolDiastol,
                        Ates = aIslemi.ANT.Ates,
                        Nabiz = IsPropertyExist(aIslemi.ANT, "Nabiz") ? Cast(aIslemi.ANT.Nabiz, typeof(int)) : 0,
                        NabizRitmi = aIslemi.ANT.NabizRitmi,
                        Personel_Id = data.Personel_Id,
                        Protokol = (int)personelMuayene.Protokol,
                        RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                        Tarih = GuncelTarih(),
                        UserId = User.Identity.GetUserId(),
                        MuayeneTuru = personelMuayene.MuayeneTuru
                    };
                    ant = await _aNTService.AddAsync(ant);
                    aIslemi["ANT"].Add("ANT_Id", ant.ANT_Id);
                }

                if (((JArray)aIslemi["Radyoloji"]).Count() > 0)
                {
                    JArray rad = new JArray();// jarray list oluşturuldu.
                    foreach (dynamic i in (JArray)aIslemi["Radyoloji"])
                    {
                        if (i.RadyolojikSonuc != null && i.RadyolojikIslem != null)
                        {

                            Radyoloji r = await _radyolojiService.AddAsync(new Radyoloji()
                            {
                                RadyolojikSonuc = i.RadyolojikSonuc,
                                RadyolojikIslem = i.RadyolojikIslem.grafiler,
                                Personel_Id = data.Personel_Id,
                                Protokol = (int)personelMuayene.Protokol,
                                RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                                Tarih = GuncelTarih(),
                                UserId = User.Identity.GetUserId(),
                                MuayeneTuru = "Normal Muayene İşlemleri",
                                IslemTarihi = GuncelTarih()
                            });
                            dynamic s = new JObject();// s dinamik jobject oluşturuldu.
                            s.RadyolojikSonuc = i.RadyolojikSonuc;//s yapısı {RadyolojikSonuc=null,RadyolojikIslem=null,Radyoloji_Id=null} olacak
                            dynamic z = new JObject();//RadyolojikIslem e object ataması yapılacaksa ona yeni bir object tanımlaması yapılır.
                            z.grafiler = i.RadyolojikIslem.grafiler;// veya Jarray da atanabilir.,
                            z.tipi = i.RadyolojikIslem.tipi;
                            s.RadyolojikIslem = z;//tanımlandıktan sonra radyojik işlem atama yapılabilir. 
                            s.Radyoloji_Id = r.Radyoloji_Id;
                            rad.Add(s);
                        }
                    }
                    aIslemi["Radyoloji"] = rad;
                }

                if (IsPropertyExist(aIslemi, "Tedaviler"))
                    //if (((JArray)aIslemi["Tedaviler"]).Count() > 0)
                    {
                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }

                        RevirTedavi rvTed = await _revirTedaviService.AddAsync(new RevirTedavi()
                        {
                            Sikayeti = string.Join(",", ((JArray)aIslemi["Sikayetler"])),
                            Tani = taniListesi[0].ToString(),
                            Personel_Id = data.Personel_Id,
                            Protokol = (int)personelMuayene.Protokol,
                            RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                            Tarih = GuncelTarih(),
                            UserId = User.Identity.GetUserId(),
                            MuayeneTuru = "Normal Muayene İşlemleri",
                        });
                        aIslemi.Add("RevirTedavi_Id", rvTed.RevirTedavi_Id);
                        JArray yeniTedaviList = new JArray();// jarray list oluşturuldu.
                        foreach (dynamic i in (JArray)aIslemi["Tedaviler"])
                        {
                            IlacSarfCikisi sd = new IlacSarfCikisi();
                            sd.IlacSarfCikisi_Id = Guid.NewGuid();
                            sd.IlacAdi = i.IlacAdi;
                            sd.RevirTedavi_Id = rvTed.RevirTedavi_Id;
                            sd.SaglikBirimi_Id = i.SaglikBirimi_Id;
                            sd.SarfMiktari = i.SarfMiktari;
                            sd.Status = true;
                            sd.Tarih = GuncelTarih();
                            sd.UserId = User.Identity.GetUserId();
                            sd.StokId = i.StokId;
                            await _ilacSarfCikisiService.AddAsync(sd);
                            i.IlacSarfCikisi_Id = sd.IlacSarfCikisi_Id;
                            yeniTedaviList.Add(i);
                        }
                        aIslemi["Tedaviler"] = yeniTedaviList;
                    }

                if (IsPropertyExist(aIslemi, "Istirahat"))
                {
                    List<string> taniListesi = new List<string>();
                    foreach (JObject i in (JArray)aIslemi["Tanilar"])
                    {
                        taniListesi.Add(i["ad"].Value<string>());
                    }
                    var dr = this.AppUserManager.Users.ToList().Where(s => s.Id == User.Identity.GetUserId()).Select(u => new drListesi() { adi = u.FirstName, soyadi = u.LastName }).FirstOrDefault();
                    IcRapor icrapor = new IcRapor()
                    {
                        BaslangicTarihi = IsPropertyExist(aIslemi.Istirahat, "BaslamaTarihi") ? CastDate(aIslemi.Istirahat.BaslamaTarihi) : default(DateTime?),
                        BitisTarihi = IsPropertyExist(aIslemi.Istirahat, "BitisTarihi") ? CastDate(aIslemi.Istirahat.BitisTarihi) : default(DateTime?),
                        SureGun = IsPropertyExist(aIslemi.Istirahat, "GunSayisi") ? Cast(aIslemi.Istirahat.GunSayisi, typeof(int)) : 0,
                        Tarih = GuncelTarih(),
                        MuayeneTuru = personelMuayene.MuayeneTuru,
                        Tani = string.Join(",", taniListesi.ToArray()),
                        RaporTuru = aIslemi.Istirahat.Nedeni,
                        Personel_Id = data.Personel_Id,
                        UserId = User.Identity.GetUserId(),
                        Doktor_Adi = dr.adi + " " + dr.soyadi,
                        Revir_Id = SB_Id,
                        Bolum_Id = data.Bolum_Id,
                        Sirket_Id = data.Sirket_Id,
                        Protokol = (int)personelMuayene.Protokol,
                        RevirIslem_Id = (int)personelMuayene.RevirIslem_Id
                    };
                    icrapor = await _icRaporService.AddAsync(icrapor);
                    aIslemi["Istirahat"].Add("IcRapor_Id", icrapor.IcRapor_Id);
                }

                if (IsPropertyExist(aIslemi, "iseDonus"))
                {
                    List<string> taniListesi = new List<string>();
                    foreach (JObject i in (JArray)aIslemi["Tanilar"])
                    {
                        taniListesi.Add(i["ad"].Value<string>());
                    }
                   
                    DisRapor disrapor = new DisRapor()
                    {
                        BaslangicTarihi = IsPropertyExist(aIslemi.iseDonus, "BaslamaTarihi") ? CastDate(aIslemi.iseDonus.BaslamaTarihi) : default(DateTime?),
                        BitisTarihi = IsPropertyExist(aIslemi.iseDonus, "BitisTarihi") ? CastDate(aIslemi.iseDonus.BitisTarihi) : default(DateTime?),
                        SureGun = IsPropertyExist(aIslemi.iseDonus, "gunSayisi") ? Cast(aIslemi.iseDonus.gunSayisi, typeof(int)) : 0,
                        MuayeneTuru = "İşe Dönüş",
                        Tani = string.Join(",", taniListesi.ToArray()),
                        DoktorAdi=aIslemi.iseDonus.verenHekim,
                        RaporuVerenSaglikBirimi=aIslemi.iseDonus.saglikBirimiAdi,
                        Personel_Id = data.Personel_Id,
                        User_Id = User.Identity.GetUserId(),
                        Revir_Id = SB_Id,
                        Bolum_Id = data.Bolum_Id,
                        Sirket_Id = data.Sirket_Id,
                    };
                    disrapor = await _disRaporService.AddAsync(disrapor);
                    aIslemi["iseDonus"].Add("DisRapor_Id", disrapor.DisRapor_Id);
                }
                /*
                 aIslemi buraya kadar Idleri eklendi. String dönüşümü yaplıp kayıda alınıyor.
                 */
                personelMuayene.PMJson = aIslemi.ToString(Formatting.None);
                PersonelMuayene pm = await _personelMuayeneService.AddAsync(personelMuayene);

                return Ok(await Task.Run(() => new JObject(
                         new JProperty("PersonelMuayene_Id", pm.PersonelMuayene_Id),
                         new JProperty("PMJson", aIslemi),
                         new JProperty("MuayeneTuru", pm.MuayeneTuru),
                         new JProperty("RevirIslem_Id", pm.RevirIslem_Id),
                         new JProperty("Personel_Id", data.Personel_Id),
                         new JProperty("Protokol", pm.Protokol),
                         new JProperty("Tarih", pm.Tarih),
                         new JProperty("UserId", pm.UserId)
                    )));
            }
            catch (DbEntityValidationException ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message));
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message));
            }
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(PersonelMuayene personelMuayene)
        {
            try
            {
                Personel data = await _personelService.GetAsync(personelMuayene.Personel_Id);
                PersonelMuayene pms = await _personelMuayeneService.GetAsync(personelMuayene.PersonelMuayene_Id);
                RevirIslem rv = await _revirIslemService.GetAsync((int)personelMuayene.RevirIslem_Id);
                rv.IslemDetayi = personelMuayene.MuayeneTuru;
                TimeSpan zaman = new TimeSpan();              
                zaman = pms.Tarih.Value - GuncelTarih();
                if (Math.Abs(zaman.Days) > 30) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "7 günlük zaman dilimi geçilmiştir.Güncelleme için sistem yöneticisi ile bağlantıya geçin."));
                dynamic aIslemi = JObject.Parse(personelMuayene.PMJson.ToString());
                dynamic bIslemi = JObject.Parse(pms.PMJson.ToString());
                if (IsPropertyExist(aIslemi, "EKG"))
                {
                    if (IsPropertyExist(aIslemi["EKG"], "EKG_Id"))
                    {
                        EKG ekg = await _eKGService.GetAsync((int)aIslemi["EKG"].EKG_Id);
                        ekg.MuayeneTuru = personelMuayene.MuayeneTuru;
                        ekg.Sonuc = aIslemi.EKG.Sonuc;
                        await _eKGService.UpdateAsync(ekg, (int)aIslemi["EKG"]["EKG_Id"]);
                    }
                    else
                    {
                        EKG ekg = new EKG()
                        {
                            MuayeneTuru = personelMuayene.MuayeneTuru,
                            Sonuc = aIslemi.EKG.Sonuc,
                            Personel_Id = data.Personel_Id,
                            Protokol = personelMuayene.Protokol,
                            RevirIslem_Id = personelMuayene.RevirIslem_Id,
                            Tarih = GuncelTarih(),
                            UserId = User.Identity.GetUserId()
                        };
                        ekg = await _eKGService.AddAsync(ekg);
                        aIslemi["EKG"].Add("EKG_Id", ekg.EKG_Id);
                    };
                };
                if (IsPropertyExist(aIslemi, "Lab"))
                if (((JArray)aIslemi["Lab"]).Count() > 0)
                    {
                        if (IsPropertyExist(aIslemi, "Laboratuar_Id"))
                        {
                            Laboratuar lab = await _laboratuarService.GetAsync((int)aIslemi["Laboratuar_Id"]);
                            lab.Sonuc = aIslemi["Lab"].ToString();
                            await _laboratuarService.UpdateAsync(lab, (int)aIslemi["Laboratuar_Id"]);
                        }
                        else
                        {
                            Laboratuar lab = new Laboratuar()
                            {
                                MuayeneTuru = personelMuayene.MuayeneTuru,
                                Grubu = "BİYOKİMYA",
                                Sonuc = aIslemi["Lab"].ToString(),
                                Personel_Id = data.Personel_Id,
                                Protokol = (int)personelMuayene.Protokol,
                                RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                                Tarih = GuncelTarih(),
                                UserId = User.Identity.GetUserId()
                            };
                            lab = await _laboratuarService.AddAsync(lab);
                            aIslemi.Add("Laboratuar_Id", lab.Laboratuar_Id);
                        };
                    }
                if (IsPropertyExist(aIslemi, "ANT"))
                {
                    if (IsPropertyExist(aIslemi["ANT"], "ANT_Id"))
                    {
                        ANT ant = await _aNTService.GetAsync((int)aIslemi["ANT"]["ANT_Id"]);
                        ant.TASagKolDiastol = aIslemi.ANT.TASagKolDiastol;
                        ant.TASolKolSistol = aIslemi.ANT.TASolKolSistol;
                        ant.TASagKolSistol = aIslemi.ANT.TASagKolSistol;
                        ant.TASolKolDiastol = aIslemi.ANT.TASolKolDiastol;
                        ant.Ates = aIslemi.ANT.Ates;
                        ant.Nabiz = IsPropertyExist(aIslemi.ANT, "Nabiz") ? Cast(aIslemi.ANT.Nabiz, typeof(int)) : 0;
                        ant.NabizRitmi = aIslemi.ANT.NabizRitm;
                        await _aNTService.UpdateAsync(ant, (int)aIslemi["ANT"]["ANT_Id"]);
                    }
                    else
                    {
                        ANT ant = new ANT()
                        {
                            TASagKolDiastol = aIslemi.ANT.TASagKolDiastol,
                            TASolKolSistol = aIslemi.ANT.TASolKolSistol,
                            TASagKolSistol = aIslemi.ANT.TASagKolSistol,
                            TASolKolDiastol = aIslemi.ANT.TASolKolDiastol,
                            Ates = aIslemi.ANT.Ates,
                            Nabiz = IsPropertyExist(aIslemi.ANT, "Nabiz") ? Cast(aIslemi.ANT.Nabiz, typeof(int)) : 0,
                            NabizRitmi = aIslemi.ANT.NabizRitmi,
                            Personel_Id = data.Personel_Id,
                            Protokol = (int)personelMuayene.Protokol,
                            RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                            Tarih = GuncelTarih(),
                            UserId = User.Identity.GetUserId(),
                            MuayeneTuru = personelMuayene.MuayeneTuru
                        };
                        ant = await _aNTService.AddAsync(ant);
                        aIslemi["ANT"].Add("ANT_Id", ant.ANT_Id);
                    };
                }
                if (((JArray)aIslemi["Radyoloji"]).Count() > 0)
                {
                    JArray rad = new JArray();// jarray list oluşturuldu.
                    foreach (dynamic i in (JArray)aIslemi["Radyoloji"])
                    {
                        if (i["RadyolojikSonuc"] != null && i["RadyolojikIslem"] != null)
                        {
                            Radyoloji r = new Radyoloji();
                            if (i.Radyoloji_Id != null)
                            {
                                r = await _radyolojiService.GetAsync((int)i.Radyoloji_Id);
                                r.RadyolojikSonuc = i.RadyolojikSonuc;
                                r.RadyolojikIslem = i.RadyolojikIslem.grafiler;
                                await _radyolojiService.UpdateAsync(r, (int)i.Radyoloji_Id);
                            }
                            else
                            {
                                r = await _radyolojiService.AddAsync(new Radyoloji()
                                {
                                    RadyolojikSonuc = i.RadyolojikSonuc,
                                    RadyolojikIslem = i.RadyolojikIslem.grafiler,
                                    Personel_Id = pms.Personel_Id,
                                    Protokol = (int)pms.Protokol,
                                    RevirIslem_Id = (int)pms.RevirIslem_Id,
                                    Tarih = GuncelTarih(),
                                    UserId = User.Identity.GetUserId(),
                                    MuayeneTuru = "Normal Muayene İşlemleri",
                                    IslemTarihi = GuncelTarih()
                                });
                            }
                            dynamic s = new JObject();// s dinamik jobject oluşturuldu.
                            s.RadyolojikSonuc = i.RadyolojikSonuc;//s yapısı {RadyolojikSonuc=null,RadyolojikIslem=null,Radyoloji_Id=null} olacak
                            dynamic z = new JObject();//RadyolojikIslem e object ataması yapılacaksa ona yeni bir object tanımlaması yapılır.
                            z.grafiler = i.RadyolojikIslem.grafiler;// veya Jarray da atanabilir.,
                            z.tipi = i.RadyolojikIslem.tipi;
                            s.RadyolojikIslem = z;//tanımlandıktan sonra radyojik işlem atama yapılabilir. 
                            s.Radyoloji_Id = r.Radyoloji_Id;
                            rad.Add(s);
                        }
                    }
                    aIslemi["Radyoloji"] = rad;
                }
                if (((JArray)aIslemi["Tedaviler"]).Count() > 0)
                    {
                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }
                        if (!IsPropertyExist(aIslemi, "RevirTedavi_Id"))
                        {
                            RevirTedavi rvTed = await _revirTedaviService.AddAsync(new RevirTedavi()
                            {
                                Sikayeti = string.Join(",", ((JArray)aIslemi["Sikayetler"])),
                                Tani = taniListesi[0].ToString(),
                                Personel_Id = data.Personel_Id,
                                Protokol = (int)personelMuayene.Protokol,
                                RevirIslem_Id = (int)personelMuayene.RevirIslem_Id,
                                Tarih = GuncelTarih(),
                                UserId = User.Identity.GetUserId(),
                                MuayeneTuru = "Normal Muayene İşlemleri",
                            });
                            aIslemi.Add("RevirTedavi_Id", rvTed.RevirTedavi_Id);
                            JArray yeniTedaviList = new JArray();// jarray list oluşturuldu.
                            foreach (dynamic i in (JArray)aIslemi["Tedaviler"])
                            {
                                IlacSarfCikisi sd = new IlacSarfCikisi();
                                sd.IlacSarfCikisi_Id = Guid.NewGuid();
                                sd.IlacAdi = i.IlacAdi;
                                sd.RevirTedavi_Id = rvTed.RevirTedavi_Id;
                                sd.SaglikBirimi_Id = i.SaglikBirimi_Id;
                                sd.SarfMiktari = i.SarfMiktari;
                                sd.Status = true;
                                sd.Tarih = GuncelTarih();
                                sd.UserId = User.Identity.GetUserId();
                                sd.StokId = i.StokId;
                                await _ilacSarfCikisiService.AddAsync(sd);
                                i.IlacSarfCikisi_Id = sd.IlacSarfCikisi_Id;
                                yeniTedaviList.Add(i);
                            }
                            aIslemi["Tedaviler"] = yeniTedaviList;
                        }
                        else
                        {
                            RevirTedavi rt = await _revirTedaviService.GetAsync((int)aIslemi["RevirTedavi_Id"]);
                            rt.Sikayeti = string.Join(",", ((JArray)aIslemi["Sikayetler"]));
                            rt.Tani = taniListesi[0].ToString();
                            await _revirTedaviService.UpdateAsync(rt, (int)aIslemi["RevirTedavi_Id"]);
                            JArray yeniTedaviList = new JArray();// jarray list oluşturuldu.
                            foreach (dynamic i in (JArray)aIslemi["Tedaviler"])
                            {
                                if (i.IlacSarfCikisi_Id == null)
                                {
                                IlacSarfCikisi sd = new IlacSarfCikisi()
                                {
                                    IlacSarfCikisi_Id = Guid.NewGuid(),
                                    IlacAdi = i.IlacAdi,
                                    RevirTedavi_Id = rt.RevirTedavi_Id,
                                    SaglikBirimi_Id = i.SaglikBirimi_Id,
                                    SarfMiktari = i.SarfMiktari,
                                    Status = true,
                                    Tarih = GuncelTarih(),
                                    UserId = User.Identity.GetUserId(),
                                    StokId = i.StokId
                                };
                                   await _ilacSarfCikisiService.AddAsync(sd);
                                    i.IlacSarfCikisi_Id = sd.IlacSarfCikisi_Id;
                                    yeniTedaviList.Add(i);
                                }
                                else
                                {
                                    IlacSarfCikisi isc = await _ilacSarfCikisiService.GetAsync((Guid)i["IlacSarfCikisi_Id"]);
                                    isc.SarfMiktari = (int)i["SarfMiktari"];
                                    await _ilacSarfCikisiService.UpdateAsync(isc, (Guid)i["IlacSarfCikisi_Id"]);
                                    yeniTedaviList.Add(i);
                                };
                            }
                            dynamic aIslemix = JObject.Parse(pms.PMJson.ToString());

                            foreach (dynamic i in (JArray)aIslemix["Tedaviler"])
                            {
                                JObject jo = ((JArray)aIslemi["Tedaviler"]).Children<JObject>().FirstOrDefault(o => o["StokId"] == i["StokId"]);
                                if (jo == null)
                                {
                                    await _ilacSarfCikisiService.DeleteAsync((Guid)i["IlacSarfCikisi_Id"]);
                                }
                            }
                            aIslemi["Tedaviler"] = yeniTedaviList;
                        };
                    }
                if (IsPropertyExist(aIslemi, "Istirahat"))
                {
                    IcRapor icrapor = await _icRaporService.FindProtokolAsync((int)pms.Protokol);
                    if (icrapor != null)
                    {

                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }
                        var dr = this.AppUserManager.Users.ToList().Where(s => s.Id == User.Identity.GetUserId()).Select(u => new drListesi() { adi = u.FirstName, soyadi = u.LastName }).FirstOrDefault();
                        icrapor.BaslangicTarihi = IsPropertyExist(aIslemi.Istirahat, "BaslamaTarihi") ? CastDate(aIslemi.Istirahat.BaslamaTarihi) : default(DateTime?);
                        icrapor.BitisTarihi = IsPropertyExist(aIslemi.Istirahat, "BitisTarihi") ? CastDate(aIslemi.Istirahat.BitisTarihi) : default(DateTime?);
                        icrapor.SureGun = IsPropertyExist(aIslemi.Istirahat, "GunSayisi") ? Cast(aIslemi.Istirahat.GunSayisi, typeof(int)) : 0;
                        icrapor.Tarih = GuncelTarih();
                        icrapor.MuayeneTuru = "Normal Muayene İşlemleri";
                        icrapor.Tani = string.Join(",", taniListesi.ToArray());
                        icrapor.RaporTuru = aIslemi.Istirahat.Nedeni;
                        icrapor.UserId = User.Identity.GetUserId();
                        icrapor.Doktor_Adi = dr.adi + " " + dr.soyadi;
                        await _icRaporService.UpdateAsync(icrapor, (int)aIslemi["Istirahat"]["IcRapor_Id"]);
                    }
                    else
                    {
                        icrapor = new IcRapor();
                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }
                        var dr = this.AppUserManager.Users.ToList().Where(s => s.Id == User.Identity.GetUserId()).Select(u => new drListesi() { adi = u.FirstName, soyadi = u.LastName }).FirstOrDefault();
                        icrapor.BaslangicTarihi = IsPropertyExist(aIslemi.Istirahat, "BaslamaTarihi") ? CastDate(aIslemi.Istirahat.BaslamaTarihi) : default(DateTime?);
                        icrapor.BitisTarihi = IsPropertyExist(aIslemi.Istirahat, "BitisTarihi") ? CastDate(aIslemi.Istirahat.BitisTarihi) : default(DateTime?);
                        icrapor.SureGun = IsPropertyExist(aIslemi.Istirahat, "GunSayisi") ? Cast(aIslemi.Istirahat.GunSayisi, typeof(int)) : 0;
                        icrapor.Tarih = GuncelTarih();
                        icrapor.MuayeneTuru = "Normal Muayene İşlemleri";
                        icrapor.Tani = string.Join(",", taniListesi.ToArray());
                        icrapor.RaporTuru = aIslemi.Istirahat.Nedeni;
                        icrapor.Personel_Id = pms.Personel_Id;
                        icrapor.UserId = User.Identity.GetUserId();
                        icrapor.Doktor_Adi = dr.adi + " " + dr.soyadi;
                        icrapor.Revir_Id = rv.SaglikBirimi_Id;
                        icrapor.Bolum_Id = data.Bolum_Id;
                        icrapor.Sirket_Id = data.Sirket_Id;
                        icrapor.Protokol = (int)personelMuayene.Protokol;
                        icrapor.RevirIslem_Id = (int)personelMuayene.RevirIslem_Id;
                        icrapor = await _icRaporService.AddAsync(icrapor);
                        aIslemi["Istirahat"].Add("IcRapor_Id", icrapor.IcRapor_Id);
                    }
                }
                else
                {
                    IcRapor rapor = await _icRaporService.FindProtokolAsync((int)pms.Protokol);
                    if (rapor != null) await _icRaporService.DeleteAsync(rapor.IcRapor_Id);
                }

                if (IsPropertyExist(aIslemi, "iseDonus"))
                {

                    DisRapor disRapor = aIslemi["iseDonus"]["DisRapor_Id"] == null ? null : await _disRaporService.GetAsync((int)aIslemi["iseDonus"]["DisRapor_Id"]);
                    if (disRapor != null)
                    {

                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }
                        disRapor.BaslangicTarihi = IsPropertyExist(aIslemi.iseDonus, "BaslamaTarihi") ? CastDate(aIslemi.iseDonus.BaslamaTarihi) : default(DateTime?);
                        disRapor.BitisTarihi = IsPropertyExist(aIslemi.iseDonus, "BitisTarihi") ? CastDate(aIslemi.iseDonus.BitisTarihi) : default(DateTime?);
                        disRapor.SureGun = IsPropertyExist(aIslemi.iseDonus, "gunSayisi") ? Cast(aIslemi.iseDonus.gunSayisi, typeof(int)) : 0;
                        disRapor.MuayeneTuru = "İşe Dönüş";
                        disRapor.Tani = string.Join(",", taniListesi.ToArray());
                        disRapor.Personel_Id = pms.Personel_Id;
                        disRapor.DoktorAdi = aIslemi.iseDonus.verenHekim;
                        disRapor.RaporuVerenSaglikBirimi = aIslemi.iseDonus.saglikBirimiAdi;
                        await _disRaporService.UpdateAsync(disRapor, (int)aIslemi["iseDonus"]["DisRapor_Id"]);
                    }
                    else
                    {
                        disRapor = new DisRapor();
                        List<string> taniListesi = new List<string>();
                        foreach (JObject i in (JArray)aIslemi["Tanilar"])
                        {
                            taniListesi.Add(i["ad"].Value<string>());
                        }
                        disRapor.BaslangicTarihi = IsPropertyExist(aIslemi.iseDonus, "BaslamaTarihi") ? CastDate(aIslemi.iseDonus.BaslamaTarihi) : default(DateTime?);
                        disRapor.BitisTarihi = IsPropertyExist(aIslemi.iseDonus, "BitisTarihi") ? CastDate(aIslemi.iseDonus.BitisTarihi) : default(DateTime?);
                        disRapor.SureGun = IsPropertyExist(aIslemi.iseDonus, "gunSayisi") ? Cast(aIslemi.iseDonus.gunSayisi, typeof(int)) : 0;
                        disRapor.MuayeneTuru = "İşe Dönüş";
                        disRapor.Tani = string.Join(",", taniListesi.ToArray());
                        disRapor.DoktorAdi = aIslemi.iseDonus.verenHekim;
                        disRapor.RaporuVerenSaglikBirimi = aIslemi.iseDonus.saglikBirimiAdi;
                        disRapor.Revir_Id = rv.SaglikBirimi_Id;
                        disRapor.Bolum_Id = data.Bolum_Id;
                        disRapor.Sirket_Id = data.Sirket_Id;
                        disRapor.Personel_Id = pms.Personel_Id;
                        disRapor = await _disRaporService.AddAsync(disRapor);
                        aIslemi["iseDonus"].Add("DisRapor_Id", disRapor.DisRapor_Id);
                    }
                }
                else
                {
                    DisRapor ds = new DisRapor();
                    ds = !IsPropertyExist(aIslemi, "iseDonus") ? null : await _disRaporService.GetAsync((int)bIslemi["iseDonus"]["DisRapor_Id"]);
                    if (ds != null) await _disRaporService.DeleteAsync(ds.DisRapor_Id);
                }

                pms.PMJson = aIslemi.ToString(Formatting.None);
                pms.UserId = User.Identity.GetUserId();
                pms.MuayeneTuru = personelMuayene.MuayeneTuru;
                pms.Tarih = GuncelTarih();
                await _revirIslemService.UpdateAsync(rv, rv.RevirIslem_Id);
                return Ok(await _personelMuayeneService.UpdateAsync(pms, personelMuayene.PersonelMuayene_Id));
            }
            catch (DbEntityValidationException ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message));
            }
            catch (Exception ex)
            {
                //throw;
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.InnerException.InnerException.Message));
            }
        }
        [Route("Sil/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> Sil(int id)
        {
            PersonelMuayene pms = await _personelMuayeneService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = pms.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 120) return Content(HttpStatusCode.NotFound, await Task.Run(() => "Silinme İşlemi Kapanmıştır."));
            return Ok(await _personelMuayeneService.DeleteAsync((int)id));
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
}
