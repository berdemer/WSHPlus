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
    [RoutePrefix("api/Ik")]
    public class IsKazasiController : BaseApiController
    {

        private IPersonelService _personelService { get; set; }
        private IPersonelDetayiService _personelDetayiService { get; set; }
        private IAdresService _adresService { get; set; }
        private IImageUploadService _imageUploadService { get; set; }
        private ISirketService _sirketService { get; set; }
        private ISirketBolumuService _sirketBolumuService { get; set; }      
        private ICalisma_GecmisiService _calisma_GecmisiService { get; set; }     
        private IOzurlulukService _ozurlulukService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private ISirketDetayiService _sirketDetayiService { get; set; }
        private ICalisma_DurumuService _calisma_DurumuService { get; set; }
        private IIsKazasiService _isKazasiService { get;  set; }

        public IsKazasiController(
            IPersonelService personelService,
            IPersonelDetayiService personelDetayiService,
            IAdresService adresService,
            ICalisma_DurumuService calisma_DurumuService,
            IImageUploadService imageUploadService,
            ISirketBolumuService sirketBolumuService,
            ISirketService sirketService,
            ICalisma_GecmisiService calisma_GecmisiService,
            IRevirIslemService revirIslemService,
            ISirketDetayiService sirketDetayiService,
            IIsKazasiService  isKazasiService
            )
        {
            _personelService = personelService;
            _personelDetayiService = personelDetayiService;
            _adresService = adresService;
            _imageUploadService = imageUploadService;
            _imageUploadService = imageUploadService;
            _sirketService = sirketService;
            _sirketBolumuService = sirketBolumuService;
            _calisma_GecmisiService = calisma_GecmisiService;
            _revirIslemService = revirIslemService;
            _sirketDetayiService = sirketDetayiService;
            _calisma_DurumuService = calisma_DurumuService;
            _isKazasiService = isKazasiService;
        }

        #region Personel İş Kazası Arama
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
            var calismaBilgisi = data.Calisma_Durumu.Select(x => new { tarih = x.Baslama_Tarihi == null ? null : x.Baslama_Tarihi, sicilNo = x.SicilNo == null ? null : x.SicilNo.Trim(), gorevi = x.Gorevi == null ? null : x.Gorevi.Trim(), kadroDurumu = x.KadroDurumu == null ? null : x.KadroDurumu.Trim(), bolumu = x.Bolum == null ? null : x.Bolum.Trim() }).LastOrDefault();
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
            ICollection<IsKazasi> IsKazalari = await _isKazasiService.FindAllAsync(new IsKazasi() { Personel_Id = data.Personel_Id });

            JArray pm = new JArray(/*http://www.newtonsoft.com/json/help/html/CreateJsonDeclaratively.htm*/
                 from p in IsKazalari
                 select new JObject(
                     new JProperty("IsKazasi_Id", p.IsKazasi_Id),
                     new JProperty("PMJson", JObject.Parse(p.PMJson.ToString())),
                     new JProperty("MuayeneTuru", p.MuayeneTuru.Trim()),
                     new JProperty("RevirIslem_Id", p.RevirIslem_Id),
                     new JProperty("Personel_Id", p.Personel_Id),
                     new JProperty("Protokol", p.Protokol),
                     new JProperty("Tarih", p.Tarih),
                     new JProperty("UserId", p.UserId)
                     )
                );
            JObject o = (JObject)JToken.FromObject(new { img = img, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi, sirketDetayi = sd, calismaBilgisi = calismaBilgisi, adres = adres, data = data });
            JObject Jdata = (JObject)o["data"];
            Jdata["IsKazalari"] = pm; /*http://www.newtonsoft.com/json/help/html/ModifyJson.htm*/
            return Ok(await Task.Run(() => o));
        }

        #endregion

        #region İş kazası ekle veya güncelle
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [Route("{SB_Id}/{personelGuid}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(IsKazasi isKazasi, int SB_Id, Guid personelGuid)
        {
            try
            {
                Personel data = await _personelService.GuidKontrol(personelGuid);
                RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =  isKazasi.MuayeneTuru, IslemDetayi ="İş Kazası Bildirimleri", UserId = User.Identity.GetUserId() }, true);
                isKazasi.RevirIslem_Id = rv.RevirIslem_Id;
                isKazasi.Protokol = rv.Protokol;
                isKazasi.Tarih = GuncelTarih();
                isKazasi.UserId = User.Identity.GetUserId();
                isKazasi.Personel_Id = data.Personel_Id;

                dynamic aIslemi = JObject.Parse(isKazasi.PMJson.ToString());

               IsKazasi ik = await _isKazasiService.AddAsync(isKazasi);

                return Ok(await Task.Run(() => new JObject(
                         new JProperty("IsKazasi_Id", ik.IsKazasi_Id),
                         new JProperty("PMJson", aIslemi),
                         new JProperty("MuayeneTuru", ik.MuayeneTuru),
                         new JProperty("RevirIslem_Id", ik.RevirIslem_Id),
                         new JProperty("Personel_Id", data.Personel_Id),
                         new JProperty("Protokol", ik.Protokol),
                         new JProperty("Tarih", ik.Tarih),
                         new JProperty("UserId", ik.UserId)
                    )));
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message));
            }
        }

        [Route("update")]
        [HttpPost]
        public async Task<IHttpActionResult> Put(IsKazasi isKazasi)
        {
            try
            {
                IsKazasi ik = await _isKazasiService.GetAsync(isKazasi.IsKazasi_Id);
                ik.PMJson = isKazasi.PMJson;
                ik.MuayeneTuru = "İş Kazası";
                ik.UserId = User.Identity.GetUserId();
                TimeSpan zaman = new TimeSpan();
                zaman = ik.Tarih.Value - GuncelTarih();
                if (Math.Abs(zaman.Days) > 15) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "15 günlük zaman dilimi geçilmiştir.Güncelleme için sistem yöneticisi ile bağlantıya geçin."));

                return Ok(await _isKazasiService.UpdateAsync(ik, isKazasi.IsKazasi_Id));
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.InnerException.InnerException.Message));
            }
        }
        #endregion

    }

}