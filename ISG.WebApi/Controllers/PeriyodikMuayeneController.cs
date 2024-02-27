using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/prm")]
    public class PeriyodikMuayeneController : BaseApiController
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
        private IPeriyodikMuayeneService _periyodikMuayeneService { get; set; }
        private ICalisma_GecmisiService _calisma_GecmisiService { get; set; }
        private IAllerjiService _allerjiService { get; set; }
        private ISoyGecmisiService _soyGecmisiService { get; set; }
        private IAsiService _asiService { get; set; }
        private IKronikHastalikService _kronikHastalikService { get; set; }
        private IAliskanlikService _aliskanlikService { get; set; }
        private ISFTService _sFTService { get; set; }
        private IOdioService _odioService { get; set; }
        private IOzurlulukService _ozurlulukService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private ISirketDetayiService _sirketDetayiService { get; set; }
        private ICalisma_DurumuService _calisma_DurumuService { get; set; }
        private IMail_OnerileriService _mail_OnerileriService { get; set; }
        private IIkazService _ikazService { get; set; }
        public PeriyodikMuayeneController(
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
            IPeriyodikMuayeneService periyodikMuayeneService,
            ICalisma_GecmisiService calisma_GecmisiService,
            IAllerjiService allerjiService,
            ISoyGecmisiService soyGecmisiService,
            IAsiService asiService,
            IKronikHastalikService kronikHastalikService,
            IAliskanlikService aliskanlikService,
            ISFTService sFTService,
            IOdioService odioService,
            IOzurlulukService ozurlulukService,
            IRevirIslemService revirIslemService,
            ISirketDetayiService sirketDetayiService,
            IMail_OnerileriService mail_OnerileriService,
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
            _periyodikMuayeneService = periyodikMuayeneService;
            _calisma_GecmisiService = calisma_GecmisiService;
            _allerjiService = allerjiService;
            _soyGecmisiService = soyGecmisiService;
            _asiService = asiService;
            _kronikHastalikService = kronikHastalikService;
            _aliskanlikService = aliskanlikService;
            _sFTService = sFTService;
            _odioService = odioService;
            _ozurlulukService = ozurlulukService;
            _revirIslemService = revirIslemService;
            _sirketDetayiService = sirketDetayiService;
            _calisma_DurumuService = calisma_DurumuService;
            _mail_OnerileriService = mail_OnerileriService;
            _ikazService = ikazService;
        }

        #region Personel Periyodik Muayene Arama
        //http://localhost:1943/api/prm/cc694a55-4c01-e611-ac6e-e0b9a5d34c70
        [Route("{GuidId}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid GuidId)
        {
            Personel data = await _personelService.GuidKontrol(GuidId);
            data.PersonelDetayi = await _personelDetayiService.FindAsync(new PersonelDetayi() { PersonelDetay_Id = data.Personel_Id });
            data.Adresler = await _adresService.FindAllAsync(new Adres() { Adres_Id = data.Personel_Id });
            data.Calisma_Durumu = await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = data.Personel_Id });
            data.Calisma_Gecmisi = await _calisma_GecmisiService.FindAllAsync(new Calisma_Gecmisi() { Calisma_Gecmisi_Id = data.Personel_Id });
            data.Allerjiler = await _allerjiService.FindAllAsync(new Allerji() { Personel_Id = data.Personel_Id });
            data.SoyGecmisleri = await _soyGecmisiService.FindAllAsync(new SoyGecmisi() { Personel_Id = data.Personel_Id });
            data.Asilar = await _asiService.FindAllAsync(new Asi() { Personel_Id = data.Personel_Id });
            data.KronikHastaliklar = await _kronikHastalikService.FindAllAsync(new KronikHastalik() { Personel_Id = data.Personel_Id });
            data.Aliskanliklar = await _aliskanlikService.FindAllAsync(new Aliskanlik() { Personel_Id = data.Personel_Id });
            data.SFTleri = await _sFTService.FindAllAsync(new SFT() { Personel_Id = data.Personel_Id });
            data.Odiolar = await _odioService.FindAllAsync(new Odio() { Personel_Id = data.Personel_Id });
            data.ANTlari = await _aNTService.FindAllAsync(new ANT() { Personel_Id = data.Personel_Id });
            data.BoyKilolari = await _boyKiloService.FindAllAsync(new BoyKilo() { Personel_Id = data.Personel_Id });
            data.Laboratuarlari = await _laboratuarService.FindAllAsync(new Laboratuar() { Personel_Id = data.Personel_Id });
            data.Hemogramlar = await _hemogramService.FindAllAsync(new Hemogram() { Personel_Id = data.Personel_Id });
            data.Radyolojileri = await _radyolojiService.FindAllAsync(new Radyoloji() { Personel_Id = data.Personel_Id });
            data.Ozurlulukler = await _ozurlulukService.FindAllAsync(new Ozurluluk() { Ozurluluk_Id = data.Personel_Id });
            data.EKGleri = await _eKGService.FindAllAsync(new EKG() { Personel_Id = data.Personel_Id });
            data.Ikazlar = await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = data.Personel_Id, Status = true, SonTarih = DateTime.Now });
            var calismaBilgisi = data.Calisma_Durumu.Select(x => new { tarih = x.Baslama_Tarihi == null ? null : x.Baslama_Tarihi, sicilNo = x.SicilNo == null ? null : x.SicilNo.Trim(), gorevi = x.Gorevi == null ? null : x.Gorevi.Trim(), kadroDurumu = x.KadroDurumu == null ? null : x.KadroDurumu.Trim(), bolumu = x.Bolum == null ? null : x.Bolum.Trim(), vardiyasi = x.Calisma_Duzeni == null ? null : x.Calisma_Duzeni.Trim(), }).LastOrDefault();
            var adres = data.Adresler.Where(x => x.Adres_Turu.Trim() == "Ev Adresi").Select(x => x.GenelAdresBilgisi).LastOrDefault();
            //Kişinin çalıştığı bölüm ve şirket adıdata
            SirketBolumu sb = await _sirketBolumuService.FindAsync(new SirketBolumu() { id = Convert.ToInt32(data.Bolum_Id) });
            Sirket st = await _sirketService.FindAsync(new Sirket() { id = Convert.ToInt32(data.Sirket_Id) });
            SirketDetayi sd = await _sirketDetayiService.GetAsync(Convert.ToInt32(data.Sirket_Id));
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
            //bölümüne göre mail önerileri
            var MailListesi = await _mail_OnerileriService.MailOnerileri(st.id, sb.id, "Periyodik Muayene");

            ICollection<PeriyodikMuayene> PeriyodikMuayeneleri = await _periyodikMuayeneService.FindAllAsync(new PeriyodikMuayene() { Personel_Id = data.Personel_Id });
            //JArray a = (JArray)JToken.FromObject(PersonelMuayeneleri);

            JArray pm = new JArray(/*http://www.newtonsoft.com/json/help/html/CreateJsonDeclaratively.htm*/
                 from p in PeriyodikMuayeneleri
                 select new JObject(
                     new JProperty("PeriyodikMuayene_Id", p.PeriyodikMuayene_Id),
                     new JProperty("PMJson", JObject.Parse(p.PMJson.ToString())),
                     new JProperty("MuayeneTuru", p.MuayeneTuru.Trim()),
                     new JProperty("RevirIslem_Id", p.RevirIslem_Id),
                     new JProperty("Personel_Id", p.Personel_Id),
                     new JProperty("Protokol", p.Protokol),
                     new JProperty("Tarih", p.Tarih),
                     new JProperty("UserId", p.UserId)
                     )
                );


            JObject o = (JObject)JToken.FromObject(new {mailListeOnerileri= MailListesi, img = img, bolumId = sb.id, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi, sirketId = st.id, sirketDetayi = sd, calismaBilgisi = calismaBilgisi, adres = adres, data = data });
            JObject Jdata = (JObject)o["data"];
            Jdata["PeriyodikMuayeneleri"] = pm; /*http://www.newtonsoft.com/json/help/html/ModifyJson.htm*/
            return Ok(await Task.Run(() => o));
        }

        #endregion

        #region Personel periyodik muayene kayıt ve güncelleme
        [Route("Sil/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> Sil(int id)
        {
            PeriyodikMuayene pms = await _periyodikMuayeneService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = pms.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 120) return Content(HttpStatusCode.NotFound, await Task.Run(() => "Silinme İşlemi Kapanmıştır."));
            return Ok(await _periyodikMuayeneService.DeleteAsync((int)id));
        }

        [Route("{SB_Id}/{personelGuid}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(PeriyodikMuayene periyodikMuayene, int SB_Id, Guid personelGuid)
        {
            try
            {
                Personel data = await _personelService.GuidKontrol(personelGuid);
                RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = "PersonelMuayene", IslemDetayi = periyodikMuayene.MuayeneTuru, UserId = User.Identity.GetUserId() }, true);
                periyodikMuayene.RevirIslem_Id = rv.RevirIslem_Id;
                periyodikMuayene.Protokol = rv.Protokol;
                periyodikMuayene.Tarih = GuncelTarih();
                periyodikMuayene.UserId = User.Identity.GetUserId();
                periyodikMuayene.Personel_Id = data.Personel_Id;

                dynamic aIslemi = JObject.Parse(periyodikMuayene.PMJson.ToString());

                PeriyodikMuayene pm = await _periyodikMuayeneService.AddAsync(periyodikMuayene);

                return Ok(await Task.Run(() => new JObject(
                         new JProperty("PeriyodikMuayene_Id", pm.PeriyodikMuayene_Id),
                         new JProperty("PMJson", aIslemi),
                         new JProperty("MuayeneTuru", pm.MuayeneTuru),
                         new JProperty("RevirIslem_Id", pm.RevirIslem_Id),
                         new JProperty("Personel_Id", data.Personel_Id),
                         new JProperty("Protokol", pm.Protokol),
                         new JProperty("Tarih", pm.Tarih),
                         new JProperty("UserId", pm.UserId)
                    )));
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message));
            }
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [Route("update")]
        [HttpPost]
        public async Task<IHttpActionResult> Put(PeriyodikMuayene periyodikMuayene)
        {
            try
            {
                PeriyodikMuayene pms = await _periyodikMuayeneService.GetAsync(periyodikMuayene.PeriyodikMuayene_Id);
                pms.PMJson = periyodikMuayene.PMJson;
                pms.MuayeneTuru = periyodikMuayene.MuayeneTuru;
                pms.UserId = User.Identity.GetUserId();
                TimeSpan zaman = new TimeSpan();
                zaman = pms.Tarih.Value - GuncelTarih();
                if (Math.Abs(zaman.Days) > 300) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "15 günlük zaman dilimi geçilmiştir.Güncelleme için sistem yöneticisi ile bağlantıya geçin."));

                return Ok(await _periyodikMuayeneService.UpdateAsync(pms, periyodikMuayene.PeriyodikMuayene_Id));
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.InnerException.InnerException.Message));
            }
        }


      

    }
    #endregion
}