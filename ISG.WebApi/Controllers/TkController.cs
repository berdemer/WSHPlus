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
    [RoutePrefix("api/tk")]
    public class TkController : ApiController
    {
        private IPersonelService _personelService { get; set; }
        private IPersonelDetayiService _personelDetayiService { get; set; }
        private IAdresService _adresService { get; set; }
        private IImageUploadService _imageUploadService { get; set; }
        private ISirketService _sirketService { get; set; }
        private ISirketBolumuService _sirketBolumuService { get; set; }
        private ISFTService _sFTService { get; set; }
        private IOdioService _odioService { get; set; }
        private IANTService _aNTService { get; set; }
        private IBoyKiloService _boyKiloService { get;  set; }
        private ILaboratuarService _laboratuarService { get;  set; }
        private IHemogramService _hemogramService { get; set; }
        private IRadyolojiService _radyolojiService { get; set; }
        private IEKGService _eKGService { get;  set; }
        private IGormeService _gormeService { get;  set; }
        private IPansumanService _pansumanService { get; set; }
        private IRevirTedaviService _revirTedaviService { get; set; }
        private IIlacSarfCikisiService _ilacSarfCikisiService { get; set; }
        private IPsikolojikTestService _psikolojikTestService { get; set; }
        private IIkazService _ikazService { get; set; }
        private ICalisma_DurumuService _calisma_DurumuService { get; set; }
        public TkController(IPersonelService personelService,
            IPersonelDetayiService personelDetayiService,
            IAdresService adresService,
            ICalisma_DurumuService calisma_DurumuService,
            IImageUploadService imageUploadService,
            ISirketBolumuService sirketBolumuService,
            ISirketService sirketService,
            ISFTService sFTService ,
            IOdioService  odioService,
            IANTService aNTService,
            IBoyKiloService boyKiloService,
            ILaboratuarService laboratuarService,
            IHemogramService hemogramService,
            IRadyolojiService radyolojiService,
            IEKGService eKGService,
            IGormeService gormeService,
            IPansumanService pansumanService,
            IRevirTedaviService revirTedaviService,
            IPsikolojikTestService psikolojikTestService,
            IIlacSarfCikisiService ilacSarfCikisiService,
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
            _sFTService = sFTService;
            _odioService = odioService;
            _aNTService = aNTService;
            _boyKiloService = boyKiloService;
            _laboratuarService = laboratuarService;
            _hemogramService = hemogramService;
            _radyolojiService = radyolojiService;
            _eKGService = eKGService;
            _gormeService = gormeService;
            _pansumanService = pansumanService;
            _revirTedaviService = revirTedaviService;
            _psikolojikTestService = psikolojikTestService;
            _ilacSarfCikisiService = ilacSarfCikisiService;
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
            //tetkikler bölümü
            data.SFTleri = await _sFTService.FindAllAsync(new SFT() {Personel_Id = data.Personel_Id });
            data.Odiolar = await _odioService.FindAllAsync(new Odio() { Personel_Id = data.Personel_Id });
            data.ANTlari = await _aNTService.FindAllAsync(new ANT() { Personel_Id = data.Personel_Id });
            data.BoyKilolari = await _boyKiloService.FindAllAsync(new BoyKilo() { Personel_Id = data.Personel_Id });
            data.Laboratuarlari = await _laboratuarService.FindAllAsync(new Laboratuar() { Personel_Id = data.Personel_Id });
            data.Hemogramlar = await _hemogramService.FindAllAsync(new Hemogram() { Personel_Id = data.Personel_Id });
            data.Radyolojileri = await _radyolojiService.FindAllAsync(new Radyoloji() { Personel_Id = data.Personel_Id });
            data.EKGleri = await _eKGService.FindAllAsync(new EKG() { Personel_Id = data.Personel_Id });
            data.Gormeleri = await _gormeService.FindAllAsync(new Gorme() { Personel_Id = data.Personel_Id });
            data.Pansumanlari = await _pansumanService.FindAllAsync(new Pansuman() { Personel_Id = data.Personel_Id });
            data.RevirTedavileri = await _revirTedaviService.FindAllAsync(new RevirTedavi() { Personel_Id = data.Personel_Id });
            data.PsikolojikTestler = await _psikolojikTestService.FindAllAsync(new PsikolojikTest() { Personel_Id = data.Personel_Id });
            data.Ikazlar = await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = data.Personel_Id, Status = true,SonTarih=DateTime.Now });
            data.Calisma_Durumu = await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = data.Personel_Id });
            string calismadurumu = data.Calisma_Durumu.Select(x=>x.Calisma_Duzeni).LastOrDefault();
           
            foreach (var revirTedavi in data.RevirTedavileri)
            {
                IEnumerable<IlacSarfCikisi> ilacsarfi = await _ilacSarfCikisiService.FindAllAsync(new IlacSarfCikisi() { RevirTedavi_Id = revirTedavi.RevirTedavi_Id });
                revirTedavi.IlacSarfCikislari = ilacsarfi.ToList();
            }
            return Ok(await Task.Run(() => new { data = data, img = img, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi,vardiyasi= calismadurumu}));
        }
    }
}
