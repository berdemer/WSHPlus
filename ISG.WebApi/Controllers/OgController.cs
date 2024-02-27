using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
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
    [RoutePrefix("api/og")]
    public class OgController : ApiController
    {
        private IPersonelService _personelService { get; set; }
        private IPersonelDetayiService _personelDetayiService { get; set; }
        private IAdresService _adresService { get; set; }
        private IOzurlulukService _ozurlulukService { get; set; }
        private IAllerjiService _allerjiService{ get; set; }
        private IImageUploadService _imageUploadService { get; set; }
        private IIkazService _ikazService { get; set; }
        private ISoyGecmisiService _soyGecmisiService { get; set; }
        private IAsiService _asiService { get; set; }
        private IKronikHastalikService _kronikHastalikService { get; set; }
        private ISirketService _sirketService { get; set; }
        private ISirketBolumuService _sirketBolumuService { get; set; }
        private IAliskanlikService _aliskanlikService { get; set; }
        private ICalisma_DurumuService _calisma_DurumuService { get; set; }
        public OgController(IPersonelService personelService,
            IPersonelDetayiService personelDetayiService,
            IAdresService adresService,
            ICalisma_DurumuService calisma_DurumuService,
            IOzurlulukService ozurlulukService,
            IImageUploadService imageUploadService,
            IAllerjiService allerjiService,
            ISoyGecmisiService soyGecmisiService,
            IAsiService asiService,
            IKronikHastalikService kronikHastalikService,
            ISirketBolumuService sirketBolumuService,
            ISirketService sirketService,
            IAliskanlikService  aliskanlikService,
            IIkazService ikazService
            )
        {
            _personelService = personelService;
            _personelDetayiService = personelDetayiService;
            _adresService = adresService;
            _ozurlulukService = ozurlulukService;
            _imageUploadService = imageUploadService;
            _allerjiService = allerjiService;
            _soyGecmisiService = soyGecmisiService;
            _asiService = asiService;
            _kronikHastalikService = kronikHastalikService;
            _imageUploadService = imageUploadService;
            _sirketService = sirketService;
            _sirketBolumuService = sirketBolumuService;
            _aliskanlikService = aliskanlikService;
            _ikazService = ikazService;
            _calisma_DurumuService = calisma_DurumuService;
        }

        [Route("GuidId/{GuidId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GuidId(Guid GuidId)
        {
            Personel data = await _personelService.GuidKontrol(GuidId);
            data.PersonelDetayi = await _personelDetayiService.FindAsync(new PersonelDetayi() { PersonelDetay_Id = data.Personel_Id });
            data.Adresler = await _adresService.FindAllAsync(new Adres() { Adres_Id = data.Personel_Id });
            data.Ozurlulukler = await _ozurlulukService.FindAllAsync(new Ozurluluk() { Ozurluluk_Id = data.Personel_Id });
            data.Allerjiler = await _allerjiService.FindAllAsync(new Allerji() { Personel_Id= data.Personel_Id });
            data.SoyGecmisleri = await _soyGecmisiService.FindAllAsync(new SoyGecmisi() { Personel_Id = data.Personel_Id });
            data.Asilar = await _asiService.FindAllAsync(new Asi() { Personel_Id = data.Personel_Id });
            data.KronikHastaliklar = await _kronikHastalikService.FindAllAsync(new KronikHastalik() { Personel_Id = data.Personel_Id });
            data.Aliskanliklar = await _aliskanlikService.FindAllAsync(new Aliskanlik() { Personel_Id = data.Personel_Id });
            data.Ikazlar = await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = data.Personel_Id, Status = true, SonTarih = DateTime.Now });
            data.Calisma_Durumu = await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = data.Personel_Id });
            string calismadurumu = data.Calisma_Durumu.Select(x => x.Calisma_Duzeni).LastOrDefault();
            //Kişinin çalıştığı bölüm ve şirket adı
            SirketBolumu sb = await _sirketBolumuService.FindAsync(new SirketBolumu() { id = Convert.ToInt32(data.Bolum_Id) });
            Sirket st = await _sirketService.FindAsync(new Sirket() { id = Convert.ToInt32(data.Sirket_Id) });
            //Resim için yazldı
            IEnumerable<imageUpload> imgList =await _imageUploadService.FindAllAsync(new imageUploadFilter() { IdGuid = GuidId.ToString(),Folder = "Personel"});
            var img = new List<FileUploadResult>();
            imgList.ToList().ForEach(x => img.Add(new FileUploadResult(){//birden fazla img 
                FileLength = x.FileLenght,
                FileName = x.FileName,
                GenericName = x.GenericName,
                LocalFilePath = "uploads/"
            }));
            return Ok(await Task.Run(() => new { data = data,img = img, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi,vardiya= calismadurumu }));
        }

    }
}
