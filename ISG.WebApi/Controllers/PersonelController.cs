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
    [RoutePrefix("api/personel")]

    public class PersonelController : ApiController
    {

        private IPersonelService _personelService { get; set; }
        private ISirketService _sirketService { get; set; }
        private ISirketBolumuService _sirketBolumuService { get; set; }
        private IPersonelDetayiService _personelDetayiService { get; set; }
        private IAdresService _adresService { get; set; }
        private ICalisma_DurumuService _calisma_DurumuService { get; set; }
        private IOzurlulukService _ozurlulukService { get; set; }
        private ISirketAtamaService _sirketAtamaService { get; set; }
        private ICalisma_GecmisiService _calisma_GecmisiService { get; set; }
        private IEgitimHayatiService _egitimHayatiService { get; set; }
        private IDisRaporService _disRaporService { get; set; }
        private IIcRaporService _icRaporService { get; set; }
        private IIsgEgitimiService _isgEgitimiService { get; set; }
        private IImageUploadService _imageUploadService { get; set; }
        private IKkdService _kkdService { get; set; }
        private IAsiService _asiService { get; set; }
        private IIkazService _ikazService { get; set; }

        public PersonelController(IPersonelService personelService,
            ISirketBolumuService sirketBolumuService,
            ISirketService sirketService,
            IPersonelDetayiService personelDetayiService,
            IAdresService adresService,
            ICalisma_DurumuService calisma_DurumuService,
            IOzurlulukService ozurlulukService,
            ISirketAtamaService sirketAtamaService,
            ICalisma_GecmisiService calisma_GecmisiService,
            IEgitimHayatiService egitimHayatiService,
            IDisRaporService disRaporService,
            IIcRaporService icRaporService,
            IIsgEgitimiService isgEgitimiService,
            IImageUploadService imageUploadService,
            IKkdService kkdService,
            IAsiService asiService,
            IIkazService ikazService
            )
        {
            _sirketService = sirketService;
            _personelService = personelService;
            _sirketBolumuService = sirketBolumuService;
            _personelDetayiService = personelDetayiService;
            _adresService = adresService;
            _calisma_DurumuService = calisma_DurumuService;
            _ozurlulukService = ozurlulukService;
            _sirketAtamaService = sirketAtamaService;
            _calisma_GecmisiService = calisma_GecmisiService;
            _egitimHayatiService = egitimHayatiService;
            _disRaporService = disRaporService;
            _icRaporService = icRaporService;
            _isgEgitimiService = isgEgitimiService;
            _imageUploadService = imageUploadService;
            _kkdService = kkdService;
            _asiService = asiService;
            _ikazService = ikazService;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<Personel> GetById(int id)
        {
            return await _personelService.GetAsync(id);
        }


        [HttpGet]
        public async Task<IEnumerable<Personel>> Get()
        {
            return await _personelService.GetAllAsync();
        }

        [Route("GuidId/{GuidId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GuidId(Guid GuidId)
        {
            Personel data = await _personelService.GuidKontrol(GuidId);
            data.PersonelDetayi = await _personelDetayiService.FindAsync(new PersonelDetayi() { PersonelDetay_Id = data.Personel_Id });
            data.Adresler = await _adresService.FindAllAsync(new Adres() { Adres_Id = data.Personel_Id });
            data.Ozurlulukler = await _ozurlulukService.FindAllAsync(new Ozurluluk() { Ozurluluk_Id = data.Personel_Id });
            data.Calisma_Durumu = await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = data.Personel_Id });
            data.Calisma_Gecmisi = await _calisma_GecmisiService.FindAllAsync(new Calisma_Gecmisi() { Calisma_Gecmisi_Id = data.Personel_Id });
            data.EgitimHayatlari = await _egitimHayatiService.FindAllAsync(new EgitimHayati() { EgitimHayati_Id = data.Personel_Id });
            data.DisRaporlari = await _disRaporService.FindAllAsync(new DisRapor() { DisRapor_Id = data.Personel_Id });
            data.IcRaporlari = await _icRaporService.FindAllAsync(new IcRapor() { IcRapor_Id = data.Personel_Id });
            data.IsgEgitimleri = await _isgEgitimiService.FindAllAsync(new IsgEgitimi() { Egitim_Id = data.Personel_Id });
            data.KkdLeri = await _kkdService.FindAllAsync(new Kkd() { Personel_Id = data.Personel_Id });
            data.Ikazlar = await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = data.Personel_Id, Status = true, SonTarih = DateTime.Now });
            string calismadurumu = data.Calisma_Durumu.Select(x => x.Calisma_Duzeni).LastOrDefault();
            SirketBolumu sb = await _sirketBolumuService.FindAsync(new SirketBolumu() { id = Convert.ToInt32(data.Bolum_Id) });
            Sirket st = await _sirketService.FindAsync(new Sirket() { id = Convert.ToInt32(data.Sirket_Id) });
            return Ok(await Task.Run(() => new { data = data, bolumAdi = sb.bolumAdi, sirketAdi = st.sirketAdi, vardiyasi = calismadurumu }));
        }

        [Route("Personel/{key}")]
        [HttpPut]
        public async Task<Personel> Put(int key, [FromBody] Personel personel)
        {
            return await _personelService.UpdateAsync(personel, key);
        }
        [ProcessRequest(true)]
        //kullanıcıyı pasifize etmek için yazılmıştır.
        [Route("Aktif/{key}")]
        [HttpPost]
        public async Task<bool> ActiveStatus(Guid key)
        {
            Personel data = await _personelService.GuidKontrol(key);
            data.Status = data.Status != true;
            await _personelService.UpdateAsync(data, data.Personel_Id);
            return data.Status;
        }

        [Route("TcKont/{key}")]
        [HttpGet]
        public async Task<IHttpActionResult> TcDurumu(string key)
        {
            Personel data = await _personelService.FindAsync(new Personel() { TcNo = key });
            if (!TcDogrulaV2(key)) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Hatalı bir Tc Kimlik No Giriyorsunuz."));
            if (data == null) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ""));
            return Ok(new
            {
                gui = data.PerGuid,
                durum = data.Status,
                tc = data.TcNo,
                adi = data.Adi,
                soyadi = data.Soyadi
            });
        }


        [ProcessRequest(true)]//Duplucate engelle
        [HttpPost]
        public async Task<Personel> Post(Personel personel)
        {
            Personel se = new Personel();
            try
            {
                se = await _personelService.AddAsync(personel);
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
            return se;
        }

        private async Task<IList<sirketX>> UserOrganizationTable(bool status)
        {
            string UserId = User.Identity.GetUserId();
            ICollection<SirketAtama> sirAtama = await _sirketAtamaService.FindAllOrgAsync(UserId);
            IEnumerable<int> Sirketlerimiz = sirAtama.OrderBy(x => x.Sirket_id).Select(x => x.Sirket_id);
            ICollection<Sirket> tablo = await _sirketService.FindAllAsync(status);
            //Asıl tablo çekildi
            IList<sirketX> tablox = new List<sirketX>();//sanal tablo oluşturuldu.
            foreach (var sirket in tablo)//yetkilerne göre yeni tablo oluşturuldu.
            {
                if (Sirketlerimiz.Where(c => c == sirket.id).Any()) //yetkisi olan siketlerler tablosuna bulguları yerleştirildi.
                {
                    tablox.Add(new sirketX() { id = sirket.id, idRef = sirket.idRef, sirketAdi = sirket.sirketAdi, status = sirket.status, bulgu = true });
                }
                else
                {
                    tablox.Add(new sirketX() { id = sirket.id, idRef = sirket.idRef, sirketAdi = sirket.sirketAdi, status = sirket.status, bulgu = false });
                }
            };

            foreach (var sirket in tablox)/*tablo döngüye alınıp yeniden refid yapılanması sağlandı.*/
            {
                if (sirket.bulgu == true && sirket.idRef > 0)
                {
                    int v = RecursiveID(sirket.idRef, tablox);
                    sirket.idRef = v;
                }
            };
            return tablox.Where(z => z.bulgu == true).ToList();
        }

        private int RecursiveID(int idRef, IList<sirketX> tablox)
        {
            sirketX sd = tablox.Where(p => p.id == idRef).SingleOrDefault();
            if (sd == null)
            {
                return 0;//üzerinde başka bir deer yoksa 0 root olmas gerekir aksi taktirde tree oluşmaz.
            };
            if (sd.bulgu == false)//yoksa ref id bir üst değere atlanır.
            {
                return RecursiveID(sd.idRef, tablox);
            };
            return sd.id;
        }

        [Route("Sirketler/{status}")]
        [HttpGet]
        public async Task<IEnumerable<BasicTreeview>> Sirketler(bool status)
        {
            IList<sirketX> tablox = await UserOrganizationTable(status);

            IEnumerable<BasicTreeview> nodes = tablox.RecursiveJoin(element => element.id,
               element => element.idRef,
               (sirketX element, int index, int depth, IEnumerable<BasicTreeview> children) =>
               {
                   return new BasicTreeview()
                   {
                       name = element.sirketAdi,
                       id = element.id,
                       children = children
                   };
               });
            return nodes;
        }

        //http://localhost:1943/api/personel/bolumleri/3/true
        [Route("SirketBolumleri/{sirketId}/{status}")]
        [HttpGet]
        public async Task<IEnumerable<BasicTreeview>> SirketBolumleri(bool status, int sirketId)
        {
            ICollection<SirketBolumu> tablo = await _sirketBolumuService.FindAllAsync(status, sirketId);
            //recursive metod uygulama yeri
            IEnumerable<BasicTreeview> nodes = tablo.RecursiveJoin(element => element.id,
               element => element.idRef,
               (SirketBolumu element, int index, int depth, IEnumerable<BasicTreeview> children) =>
               {
                   return new BasicTreeview()
                   {
                       name = element.bolumAdi,
                       id = element.id,
                       children = children
                   };
               });
            return nodes;
        }

        //http://localhost:1943/api/personel/SpecialSirktPersList?Sirkt=2&Genel=true
        [Route("SpecialSirktPersList/{Sirkt}/{Genel}/{Status}")]
        [HttpGet]
        public async Task<IHttpActionResult> SpecialSirktPersList(int Sirkt, bool Genel, bool Status)
        {
            //kullanıcı atama listesinde yetkisi var mı?
            if (await _sirketAtamaService.AtamaKontrol(User.Identity.GetUserId(), Sirkt))
            {
                if (Genel)
                {//tüm bagımlı sirket bölümleriyle gösterilir.
                    ICollection<tasima> tasiyici = new List<tasima>() { };
                    IList<sirketX> tablox = await UserOrganizationTable(true);
                    ICollection<tasima> BagimliSirketListesi = await YineliyiciSorgula(Sirkt, tasiyici, tablox);
                    IEnumerable<int> Sirketlerimiz = BagimliSirketListesi.Select(x => x.sorgusu);
                    IEnumerable<int> Bolumlerimiz = new List<int>() { };
                    return Ok(await _personelService.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Status));
                }
                else
                {
                    IList<int> Sirketlerimiz = new List<int>
                    {
                        Sirkt
                    };
                    IList<int> Bolumlerimiz = new List<int>() { };
                    return Ok(await _personelService.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Status));
                }
            }
            else
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Şirket personellerini görmeye yetkili değilsiniz."));
            }
        }

        private async Task<ICollection<tasima>> YineliyiciSorgula(int id, ICollection<tasima> tasiyici, IList<sirketX> tablo1)//recursive function
        {
            //şirketler tablosunu yükle
            if ((from c in tasiyici where c.sorgusu == id select c).Count() > 0)//id değerinin alt sorguları varsa
            {
                tasiyici.Where(x => x.sorgusu == id).First().sorgusuyapıldı = true;//id degerini true yap
            }
            else//yoksa true yap tasiyiciya ekle
            {
                tasiyici.Add(new tasima() { sorgusu = id, sorgusuyapıldı = true });
            }

            var toplayici = (from s in tablo1 where s.idRef == id select s.id);//refid göre bagılı satırları bul

            if (toplayici.Count() > 0)// bu satırın alt bagımlılıkları varsa tasıyıcıya ekle altbagımlık sorguları
                foreach (int df in toplayici)//yapılmadığı için false ver
                {
                    tasiyici.Add(new tasima() { sorgusu = df, sorgusuyapıldı = false });
                };

            foreach (tasima q in tasiyici)//tasiyicları kontrol et alt sorguları yapılmadıysa kontrolünü yap 
            {
                if (q.sorgusuyapıldı == false) return await Task.Run(() => YineliyiciSorgula(q.sorgusu, tasiyici, tablo1));
            }// sorgusu yapılmayanlar tekrar aynı fonksiyonla sorgula...Yukarıda kontrol işlemi sonra ekleme...

            return await Task.Run(() => tasiyici);
        }

        //http://localhost:1943/api/personel/SpecialBolumPersList?Sirkt=2&Genel=true
        [Route("SpecialBolumPersList/{Bolum}/{SirktId}/{Genel}/{Status}")]
        [HttpGet]
        public async Task<IHttpActionResult> SpecialBolumPersList(int Bolum, int SirktId, bool Genel, bool Status)
        {
            if (Genel)
            {//tüm bagımlı sirket bölümleriyle gösterilir.
                ICollection<tasima> tasiyici = new List<tasima>() { };
                ICollection<tasima> BagimliBolumListesi = await YineliyiciBolumSorgula(Bolum, SirktId, tasiyici);
                IEnumerable<int> Bolumlerimiz = BagimliBolumListesi.Select(x => x.sorgusu);
                IEnumerable<int> Sirketlerimiz = new List<int>() { };
                return Ok(await _personelService.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Status));
            }
            else
            {
                IList<int> Sirketlerimiz = new List<int>() { };
                IList<int> Bolumlerimiz = new List<int>() { };
                Bolumlerimiz.Add(Bolum);

                return Ok(await _personelService.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Status));
            }
        }

        private async Task<ICollection<tasima>> YineliyiciBolumSorgula(int id, int SirktId, ICollection<tasima> tasiyici)//recursive function
        {
            ICollection<SirketBolumu> tablo = await _sirketBolumuService.FindAllAsync(true, SirktId);/* burası parametreden verilebilir.*/
            //şirketler tablosunu yükle
            if ((from c in tasiyici where c.sorgusu == id select c).Count() > 0)//id değerinin alt sorguları varsa
            {
                tasiyici.Where(x => x.sorgusu == id).First().sorgusuyapıldı = true;//id degerini true yap
            }
            else//yoksa true yap tasiyiciya ekle
            {
                tasiyici.Add(new tasima() { sorgusu = id, sorgusuyapıldı = true });
            }

            var toplayici = (from s in tablo where s.idRef == id select s.id);//refid göre bagılı satırları bul

            if (toplayici.Count() > 0)// bu satırın alt bagımlılıkları varsa tasıyıcıya ekle altbagımlık sorguları
                foreach (int df in toplayici)//yapılmadığı için false ver
                {
                    tasima sd2 = new tasima
                    {
                        sorgusu = df,
                        sorgusuyapıldı = false
                    };
                    tasiyici.Add(sd2);
                };

            foreach (tasima q in tasiyici)//tasiyicları kontrol et alt sorguları yapılmadıysa kontrolünü yap 
            {
                if (q.sorgusuyapıldı == false) return await Task.Run(() => YineliyiciBolumSorgula(q.sorgusu, SirktId, tasiyici));
            }// sorgusu yapılmayanlar tekrar aynı fonksiyonla sorgula...Yukarıda kontrol işlemi sonra ekleme...

            return await Task.Run(() => tasiyici);
        }

        [Route("AllPersList/{Genel}")]
        [HttpGet]
        public async Task<IHttpActionResult> AllPersList(bool Genel)
        {
            //ICollection<SirketAtama> sirAtama = await _sirketAtamaService.FindAllOrgAsync(User.Identity.GetUserId());
            //IEnumerable<int> Sirketlerimiz = sirAtama.OrderBy(x => x.Sirket_id).Select(x => x.Sirket_id);
            //IEnumerable<int> Bolumlerimiz = new List<int>() { };
            //return Ok(await _personelService.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Genel));
            return Ok(await _personelService.FindAllAsync(new Personel() { Status=Genel}));
        }

        #region Personel Kayıt ve Güncelleme

        [Route("insertPersonelView")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertPersonelView(JObject jPersonel)
        {
            dynamic json = jPersonel;
            Personel se = new Personel();
            PersonelDetayi pD = new PersonelDetayi();
            try
            {
                se = new Personel()
                {
                    Adi = Cast(json.Adi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)),
                    Mail = json.Mail ?? json.Mail,
                    UserId = User.Identity.GetUserId(),
                    Telefon = json.Telefon ?? json.Telefon,
                    Soyadi = Cast(json.Soyadi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)),
                    Bolum_Id = IsPropertyExist(json, "Bolum_Id") ? Cast(json.Bolum_Id, typeof(int)) : 0,
                    Sirket_Id = IsPropertyExist(json, "Sirket_Id") ? Cast(json.Sirket_Id, typeof(int)) : 0,
                    TcNo = json.TcNo,
                    Gorevi = json.Gorevi,
                    Status = true,
                    SicilNo = json.SicilNo ?? json.SicilNo,
                    KadroDurumu = json.KadroDurumu,
                    KanGrubu = json.KanGrubu,
                    SgkNo = json.SgkNo ?? json.SgkNo
                };
                Personel pers = await _personelService.FindAsync(se);
                if (pers != null) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Benzer bir Tc No kayıtlı bir personel bulunmaktadır."));
                if (!TcDogrulaV2(se.TcNo)) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Hatalı bir Tc Kimlik No Giriyorsunuz."));
                pD = (await _personelDetayiService.AddAsync(new PersonelDetayi()
                {
                    Personel = se,
                    AskerlikDurumu = json.AskerlikDurumu ?? null,
                    Cinsiyet = json.Cinsiyet,
                    CocukSayisi = IsPropertyExist(json, "CocukSayisi") ? Cast(json.CocukSayisi, typeof(int)) : 0,
                    DogumTarihi = IsPropertyExist(json, "DogumTarihi") ? CastDate(json.DogumTarihi) : default(DateTime?),
                    DogumYeri = json.DogumYeri ?? null,
                    EgitimSeviyesi = json.EgitimSeviyesi ?? null,
                    IlkIseBaslamaTarihi = IsPropertyExist(json, "IlkIseBaslamaTarihi") ? CastDate(json.IlkIseBaslamaTarihi) : default(DateTime?),
                    KardesSayisi = json.KardesSayisi == null ? 0 : Cast(json.KardesSayisi, typeof(int)),
                    Kardes_Sag_Bilgisi = json.Kardes_Sag_Bilgisi ?? null,
                    MedeniHali = json.MedeniHali ?? null,
                    Uyruk = json.Uyruk ?? null,
                    anne_adi = json.anne_adi ?? null,
                    anne_sag = json.anne_sag ?? null,
                    anne_sag_bilgisi = json.anne_sag_bilgisi ?? null,
                    baba_adi = json.baba_adi ?? null,
                    baba_sag = json.baba_sag ?? null,
                    UserId = User.Identity.GetUserId(),
                    baba_sag_bilgisi = json.baba_sag_bilgisi ?? json.baba_sag_bilgisi,
                }));
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        var propName = err.PropertyName;
                        var errMess = err.ErrorMessage;
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Validasyon uygun olmayan giriş hatası " + errMess));
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " Boşluklara dikkat edin!"));
            }
            return Ok(await Task.Run(() => new { Personel_Id = pD.Personel.Personel_Id, PerGuid = pD.Personel.PerGuid }));
        }

        [Route("updatePersonelView")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePersonelView(JObject jPersonel)
        {
            dynamic json = jPersonel;
            Personel se = await _personelService.GuidKontrol(new Guid(Cast(json.PerGuid, typeof(string))));
            try
            {
                se.Adi = Cast(json.Adi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false));
                se.Mail = json.Mail ?? json.Mail;
                se.Telefon = json.Telefon ?? json.Telefon;
                se.Soyadi = Cast(json.Soyadi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false));
                se.Bolum_Id = IsPropertyExist(json, "Bolum_Id") ? Cast(json.Bolum_Id, typeof(int)) : 0;
                se.Sirket_Id = IsPropertyExist(json, "Sirket_Id") ? Cast(json.Sirket_Id, typeof(int)) : 0;
                se.TcNo = json.TcNo;
                se.Gorevi = json.Gorevi;
                se.SicilNo = json.SicilNo ?? json.SicilNo;
                se.KadroDurumu = json.KadroDurumu;
                se.KanGrubu = json.KanGrubu;
                se.SgkNo = json.SgkNo ?? json.SgkNo;

                if (string.IsNullOrEmpty(se.TcNo)) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Tc Kimlik Numarası Boş olarak geliyor. :( Tc Kimilk kutusunun boşukların alın."));
                if (!TcDogrulaV2(se.TcNo.Trim())) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Hatalı bir Tc Kimlik No Giriyorsunuz."));

                PersonelDetayi rw = await _personelDetayiService.FindAsync(new PersonelDetayi() { PersonelDetay_Id = se.Personel_Id });

                rw.AskerlikDurumu = json.AskerlikDurumu ?? json.AskerlikDurumu;
                rw.Cinsiyet = json.Cinsiyet;
                rw.CocukSayisi = IsPropertyExist(json, "CocukSayisi") ? Cast(json.CocukSayisi, typeof(int)) : 0;
                rw.DogumTarihi = IsPropertyExist(json, "DogumTarihi") ? CastDate(json.DogumTarihi) : default(DateTime?);
                rw.DogumYeri = json.DogumYeri ?? null;
                rw.EgitimSeviyesi = json.EgitimSeviyesi ?? null;
                rw.IlkIseBaslamaTarihi = IsPropertyExist(json, "IlkIseBaslamaTarihi") ? CastDate(json.IlkIseBaslamaTarihi) : default(DateTime?);
                rw.KardesSayisi = json.KardesSayisi == null ? 0 : Cast(json.KardesSayisi, typeof(int));
                rw.Kardes_Sag_Bilgisi = json.Kardes_Sag_Bilgisi ?? null;
                rw.MedeniHali = json.MedeniHali ?? null;
                rw.Uyruk = json.Uyruk ?? null;
                rw.anne_adi = json.anne_adi ?? null;
                rw.anne_sag = json.anne_sag ?? null;
                rw.anne_sag_bilgisi = json.anne_sag_bilgisi ?? null;
                rw.baba_adi = json.baba_adi ?? null;
                rw.baba_sag = json.baba_sag ?? null;
                rw.UserId = User.Identity.GetUserId();
                rw.baba_sag_bilgisi = json.baba_sag_bilgisi ?? null;

                Personel pers = await _personelService.UpdateAsync(se, se.Personel_Id);
                PersonelDetayi pD = await _personelDetayiService.UpdateAsync(rw, se.Personel_Id);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        var propName = err.PropertyName;
                        var errMess = err.ErrorMessage;
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Validasyon uygun olmayan giriş hatası " + errMess));
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => ex.Message.ToString() + " Boşluklara dikkat edin!" +
               " Tabs Tuşu ile tekrar tüm girişleri geziniz!!!"));
            }
            return Ok(await Task.Run(() => new { Personel_Id = se.Personel_Id, PerGuid = se.PerGuid }));
        }

        public static bool TcDogrulaV2(string tcKimlikNo)
        {
            bool returnvalue = false;
            if (tcKimlikNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(tcKimlikNo);

                // bolu yuz islemi int tanimlanmis degiskende son 2 haneyi silmek icin kullanılır.

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                /*
                Q1 TC nosunun 10. hanesi
                Q2 TC nosunun 11. hanesi
                BTCNO son 2 hanesi olmayan tckimlikNo
                */

                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return returnvalue;
        }

        //http://localhost:1943/api/personel?id=gfshgfhs16161
        [HttpDelete]
        public async Task<int> Delete(Guid id)
        {
            try
            {
                Personel data = await _personelService.GuidKontrol(id);
                return await _personelService.DeleteAsync(data.Personel_Id);
            }
            catch (Exception)
            { throw; }
        }
        #endregion

        #region cardView toolunda kullanılanlar

        //http://localhost:1943/api/personel/PrCdVw/true/Ere/0/10
        [Route("PrCdVw/{Durumu}/{Search}/{DisplayStart}/{DisplayLength}")]
        [HttpGet]
        public async Task<IHttpActionResult> PersonelCardView(bool Durumu, string Search, int DisplayStart, int DisplayLength)
        {
            string UserId = User.Identity.GetUserId();
            ICollection<SirketAtama> sirAtama = await _sirketAtamaService.FindAllOrgAsync(UserId);
            IEnumerable<int> Sirketlerimiz = sirAtama.OrderBy(x => x.Sirket_id).Select(x => x.Sirket_id);
            return Ok(await _personelService.PersonelCardView(Sirketlerimiz, Durumu, Search, DisplayStart, DisplayLength));
        }

        [Route("PrEgList/{Search}/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> PersonelEgitimListesi(string Search, int Sirket_Id)
        {
            return Ok(await _personelService.EgitimPersonelleriListesi(Sirket_Id, true, Search));
        }

        [Route("PrEgTmList/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> PersonelEgitimTumListesi(int Sirket_Id)
        {
            return Ok(await _personelService.EgitimPersonelleriTumListesi(Sirket_Id, true));
        }

        //http://localhost:1943/api/personel/SPCardList/2/True/True/0/4
        [Route("SPCardList/{Sirkt}/{Genel}/{Durum}/{DisplayStart}/{DisplayLength}")]
        [HttpGet]
        public async Task<IHttpActionResult> SPCardList(int Sirkt, bool Genel, bool Durum, int DisplayStart, int DisplayLength)
        {
            //kullanıcı atama listesinde yetkisi var mı?
            if (await _sirketAtamaService.AtamaKontrol(User.Identity.GetUserId(), Sirkt))
            {
                if (Genel)
                {//tüm bagımlı sirket bölümleriyle gösterilir.                
                    IList<sirketX> tablox = await UserOrganizationTable(true);
                    ICollection<tasima> BagimliSirketListesi = await YineliyiciSorgula(Sirkt, new List<tasima>() { }, tablox);
                    IEnumerable<int> Sirketlerimiz = BagimliSirketListesi.Select(x => x.sorgusu);
                    IEnumerable<int> Bolumlerimiz = new List<int>() { };
                    return Ok(await _personelService.PersonelDeparmentCardView(Sirketlerimiz, Bolumlerimiz, Durum, DisplayStart, DisplayLength));
                }
                else
                {
                    IList<int> Sirketlerimiz = new List<int>
                    {
                        Sirkt
                    };
                    IList<int> Bolumlerimiz = new List<int>() { };
                    return Ok(await _personelService.PersonelDeparmentCardView(Sirketlerimiz, Bolumlerimiz, Durum, DisplayStart, DisplayLength));
                }
            }
            else
            {
                return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Şirket personellerini görmeye yetkili değilsiniz."));
            }
        }
        /// <summary>
        /// Bölüm Personel Listesi kart kistesi olarak dönüş sağlıyor.
        /// </summary>
        /// <param name="Bolum"></param>
        /// <param name="SirktId"></param>
        /// <param name="Genel"></param>
        /// <param name="Durum"></param>
        /// <param name="DisplayStart"></param>
        /// <param name="DisplayLength"></param>
        /// <returns></returns>
        //http://localhost:1943/api/personel/BPCardList/4/2/true/true/1/5
        [Route("BPCardList/{Bolum}/{SirktId}/{Genel}/{Durum}/{DisplayStart}/{DisplayLength}")]
        [HttpGet]
        public async Task<IHttpActionResult> BPCardList(int Bolum, int SirktId, bool Genel, bool Durum, int DisplayStart, int DisplayLength)
        {
            if (Genel)
            {//tüm bagımlı sirket bölümleriyle gösterilir.
                ICollection<tasima> BagimliBolumListesi = await YineliyiciBolumSorgula(Bolum, SirktId, new List<tasima>() { });
                IEnumerable<int> Bolumlerimiz = BagimliBolumListesi.Select(x => x.sorgusu);
                IEnumerable<int> Sirketlerimiz = new List<int>() { };
                return Ok(await _personelService.PersonelDeparmentCardView(Sirketlerimiz, Bolumlerimiz, Durum, DisplayStart, DisplayLength));
            }
            else
            {
                IList<int> Sirketlerimiz = new List<int>() { };
                IList<int> Bolumlerimiz = new List<int>() { };
                Bolumlerimiz.Add(Bolum);

                return Ok(await _personelService.PersonelDeparmentCardView(Sirketlerimiz, Bolumlerimiz, Durum, DisplayStart, DisplayLength));
            }
        }

        #endregion

        #region Excel kayıt işlemleri

        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("~/uploads/excelTemp");


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

        [Route("Alanlar")]
        [HttpGet]
        public async Task<IHttpActionResult> Alanlar()
        {
            //dynamic datt = new ExpandoObject();
            //datt.Adi = "";
            //datt.Soyadi = "";
            //datt.AdiSoyadi = "";
            //return Ok(await Task.Run(()=>datt));
            //dinamik tip tanımlaması yapıldı.
            return Ok(await Task.Run(
                () => new
                {// İsimsiz Tip (Anonymous Type) gereksiz class yazmaktan kurtuluyoruz.
                    TcNo = "",
                    Adi = "",
                    Soyadi = "",
                    AdiSoyadi = "",
                    KanGrubu = "",
                    SgkNo = "",
                    Mail = "",
                    Telefon = "",
                    IlkIseBaslamaTarihi = "",
                    DogumTarihi = "",
                    DogumYeri = "",
                    Cinsiyet = "",
                    Uyruk = "",
                    EgitimSeviyesi = "",
                    AskerlikDurumu = "",
                    MedeniHali = "",
                    GenelAdresBilgisi = "",
                    CocukSayisi = "",
                    anne_adi = "",
                    anne_sag_bilgisi = "",
                    baba_adi = "",
                    baba_sag_bilgisi = "",
                    KardesSayisi = "",
                    Kardes_Sag_Bilgisi = "",
                    Ise_Baslama_Tarihi = "",
                    Is_Bitis_Tarihi = "",
                    Is_Calisma_Duzeni = "",
                    Is_SicilNo = "",
                    Is_KadroDurumu = "",
                    Is_Gorevi = "",
                    OzurlulukHastalikTanimi = "",
                    OzurlulukOran = "",
                    OzurlulukAciklama = "",
                    DurumuAktifmi = "",
                    HepatitAsiTarihi = "",
                    HepatitAsiRapelTarihi = "",
                    TetanozAsiTarihi = "",
                    TetanozAsiRapelTarihi = "",
                    GripAsiTarihi = "",
                    GripAsiRapelTarihi = "",
                    DisRapor_BaslangicTarihi = "",
                    DisRapor_BitisTarihi = "",
                    DisRapor_Tani = "",
                    DisRapor_DoktorAdi = "",
                    DisRapor_RaporuVerenSaglikBirimi = "",
                    SinovacAsiTarihi = "",
                    BiontecAsiTarihi = "",
                    TurkovacAsiTarihi = "",
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

        public static string TelDuzelt(string tel)
        {
            string ds = Regex.Replace(tel, @"[^\d]", String.Empty);

            return ds.Length > 10 ? ds.Substring(ds.Length - 10) : ds;
        }

        public static string kangrubuDüzelt(string kan)
        {
            string kanGrubu = "";
            try
            {
                Match eslesme7 = Regex.Match(kan, @"^AB\b", RegexOptions.IgnoreCase);
                if (eslesme7.Success)
                {
                    Match eslesme8 = Regex.Match(kan, @"[+]", RegexOptions.IgnoreCase);
                    if (eslesme8.Success) { kanGrubu = "AB Rh(+)"; } else { kanGrubu = "AB Rh(-)"; };
                }
                Match eslesme = Regex.Match(kan, @"^0\b", RegexOptions.IgnoreCase);
                if (eslesme.Success)
                {
                    Match eslesme1 = Regex.Match(kan, @"[+]", RegexOptions.IgnoreCase);
                    if (eslesme1.Success) { kanGrubu = "0 Rh(+)"; } else { kanGrubu = "0 Rh(-)"; };
                }
                Match eslesme3 = Regex.Match(kan, @"^A\b", RegexOptions.IgnoreCase);
                if (eslesme3.Success)
                {
                    Match eslesme4 = Regex.Match(kan, @"[+]", RegexOptions.IgnoreCase);
                    if (eslesme4.Success) { kanGrubu = "A Rh(+)"; } else { kanGrubu = "A Rh(-)"; };
                }

                Match eslesme5 = Regex.Match(kan, @"^B\b", RegexOptions.IgnoreCase);
                if (eslesme5.Success)
                {
                    Match eslesme6 = Regex.Match(kan, @"[+]", RegexOptions.IgnoreCase);
                    if (eslesme6.Success) { kanGrubu = "B Rh(+)"; } else { kanGrubu = "B Rh(-)"; };
                }
                return kanGrubu;
            }
            catch (Exception)
            {

                return kanGrubu;
            }


        }

        [Route("InsertPrsList")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertPrsList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            var identity = User.Identity.GetUserId();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);
                    if (!String.IsNullOrEmpty(Cast(jsItem.AdiSoyadi, typeof(string))))
                    {
                        string filtre = @"\s+";
                        Regex nesne = new Regex(filtre);
                        string[] parcalar = nesne.Split(Cast(jsItem.AdiSoyadi, typeof(string)));
                        jsItem.Soyadi = parcalar[parcalar.Length - 1].Trim();
                        string firstItem = "";
                        for (int i = 0; i < parcalar.Length - 1; i++)
                        {
                            firstItem = firstItem + " " + parcalar[i];
                        }
                        jsItem.Adi = firstItem.Trim();
                    };
                    if (String.IsNullOrEmpty(Cast(jsItem.Adi, typeof(string))) ||
                       String.IsNullOrEmpty(Cast(jsItem.Soyadi, typeof(string))) ||
                       String.IsNullOrEmpty(Cast(jsItem.TcNo, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "TcNo ve Adı Soyadı olmadan kayıt yapamazsınız."));
                    };


                    Personel personel = new Personel()
                    {
                        Adi = Cast(jsItem.Adi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)),
                        Soyadi = Cast(jsItem.Soyadi, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)),
                        TcNo = Cast(jsItem.TcNo, typeof(string)),
                        KanGrubu = kangrubuDüzelt(Cast(jsItem.KanGrubu, typeof(string))),
                        SgkNo = Cast(jsItem.SgkNo, typeof(string)),
                        Mail = Cast(jsItem.Mail, typeof(string)),
                        Telefon = IsPropertyExist(jsItem, "Telefon") ? TelDuzelt(Cast(jsItem.Telefon, typeof(string))) : null,
                        Sirket_Id = IsPropertyExist(jsItem, "Sirket_Id") ? Cast(jsItem.Sirket_Id, typeof(int)) : 0,
                        Bolum_Id = IsPropertyExist(jsItem, "Bolum_id") ? Cast(jsItem.Bolum_id, typeof(int)) : 0,
                        KadroDurumu = Cast(jsItem.Is_KadroDurumu, typeof(string)),
                        Gorevi = Cast(jsItem.Is_Gorevi, typeof(string)),
                        SicilNo = Cast(jsItem.Is_SicilNo, typeof(string)),
                        Status = true,
                        UserId = User.Identity.GetUserId()
                    };
                    bool yCinsiyet = false;
                    string eCinsiyet = Cast(jsItem.Cinsiyet, typeof(string));
                    if (eCinsiyet != null)
                        yCinsiyet = eCinsiyet[0] == 'E' || eCinsiyet[0] == 'e' || eCinsiyet.ToLower() == "bay";

                    if (jsItem.EgitimSeviyesi != null)
                    {
                        string s = Cast(jsItem.EgitimSeviyesi, typeof(string));
                        switch (s.Substring(0, 4).ToLower(new CultureInfo("tr-TR", false)))
                        {
                            case "lise":
                                jsItem.EgitimSeviyesi = "LISE VE DENGİ O";
                                break;
                            case "lıse":
                                jsItem.EgitimSeviyesi = "LISE VE DENGİ O";
                                break;
                            case "üniv":
                                jsItem.EgitimSeviyesi = "UNIVERSİTE";
                                break;
                            case "univ":
                                jsItem.EgitimSeviyesi = "UNIVERSİTE";
                                break;
                            case "ilko":
                                jsItem.EgitimSeviyesi = "ILKOKUL";
                                break;
                            case "orta":
                                jsItem.EgitimSeviyesi = "ORTAOKUL";
                                break;
                            case "önli":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "ön l":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "yüks":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "mast":
                                jsItem.EgitimSeviyesi = "MASTER";
                                break;
                            case "doct":
                                jsItem.EgitimSeviyesi = "DOKTORA";
                                break;
                            default:
                                jsItem.EgitimSeviyesi = "YOK";
                                break;
                        }

                    }
                    PersonelDetayi personelDetayi = new PersonelDetayi()
                    {
                        DogumTarihi = IsPropertyExist(jsItem, "DogumTarihi") ? CastDate(jsItem.DogumTarihi) : default(DateTime?),
                        DogumYeri = jsItem.DogumYeri,
                        Cinsiyet = yCinsiyet,
                        Uyruk = jsItem.Uyruk,
                        EgitimSeviyesi = jsItem.EgitimSeviyesi,
                        AskerlikDurumu = jsItem.AskerlikDurumu,
                        IlkIseBaslamaTarihi = IsPropertyExist(jsItem, "IlkIseBaslamaTarihi") ? CastDate(jsItem.IlkIseBaslamaTarihi) : DateTime.Now,
                        MedeniHali = jsItem.MedeniHali,
                        CocukSayisi = IsPropertyExist(jsItem, "CocukSayisi") ? Cast(jsItem.CocukSayisi, typeof(int)) : 0,
                        anne_adi = jsItem.anne_adi,
                        baba_adi = jsItem.baba_adi,
                        anne_sag_bilgisi = jsItem.anne_sag_bilgisi,
                        baba_sag_bilgisi = jsItem.baba_sag_bilgisi,
                        KardesSayisi = jsItem.KardesSayisi == null ? 0 : Cast(jsItem.KardesSayisi, typeof(int)),
                        Kardes_Sag_Bilgisi = jsItem.Kardes_Sag_Bilgisi,
                        UserId = User.Identity.GetUserId()
                    };

                    Adres adres = new Adres()
                    {
                        Adres_Turu = "Ev Adresi",
                        GenelAdresBilgisi = jsItem.GenelAdresBilgisi,
                        Status = true,
                        UserId = User.Identity.GetUserId()
                    };

                    Calisma_Durumu calismaDurumu = new Calisma_Durumu()
                    {
                        Sirket = jsItem.Sirket,
                        Sirket_Id = IsPropertyExist(jsItem, "Sirket_Id") ? Cast(jsItem.Sirket_Id, typeof(int)) : 0,
                        Bolum = jsItem.Bolum,
                        Bolum_Id = IsPropertyExist(jsItem, "Bolum_id") ? Cast(jsItem.Bolum_id, typeof(int)) : 0,
                        Baslama_Tarihi = IsPropertyExist(jsItem, "Ise_Baslama_Tarihi") ? CastDate(jsItem.Ise_Baslama_Tarihi) : null,
                        Bitis_Tarihi = IsPropertyExist(jsItem, "Is_Bitis_Tarihi") ? CastDate(jsItem.Is_Bitis_Tarihi) : null,
                        Calisma_Duzeni = jsItem.Is_Calisma_Duzeni,
                        KadroDurumu = jsItem.Is_KadroDurumu,
                        Gorevi = jsItem.Is_Gorevi,
                        SicilNo = jsItem.Is_SicilNo,
                        Status = true,
                        UserId = User.Identity.GetUserId()
                    };

                    Ozurluluk ozurluluk = new Ozurluluk()
                    {
                        HastalikTanimi = jsItem.OzurlulukHastalikTanimi,
                        Oran = IsPropertyExist(jsItem, "OzurlulukOran") ? Cast(jsItem.OzurlulukOran, typeof(int)) : 0,
                        Aciklama = jsItem.OzurlulukAciklama,
                        UserId = User.Identity.GetUserId()
                    };
                    if ((!personel.TcNo.Equals(null)) && (TcDogrulaV2(personel.TcNo)))
                    {
                        Personel pers = await _personelService.FindAsync(personel);
                        if (pers == null)
                        {
                            Personel persInsert = await _personelService.AddAsync(personel);
                            if (persInsert != null)
                            {
                                personelDetayi.PersonelDetay_Id = persInsert.Personel_Id;
                                await _personelDetayiService.AddAsync(personelDetayi);
                                adres.Personel_Id = persInsert.Personel_Id;
                                if (!String.IsNullOrEmpty(adres.GenelAdresBilgisi))
                                    await _adresService.AddAsync(adres);
                                calismaDurumu.Personel_Id = persInsert.Personel_Id;
                                await _calisma_DurumuService.AddAsync(calismaDurumu);
                                ozurluluk.Personel_Id = persInsert.Personel_Id;
                                if (!String.IsNullOrEmpty(ozurluluk.HastalikTanimi))
                                    await _ozurlulukService.AddAsync(ozurluluk);
                            }

                            if (IsPropertyExist(jsItem, "HepatitAsiTarihi"))
                            {
                                Asi asi = new Asi()
                                {
                                    Personel_Id = persInsert.Personel_Id,
                                    Asi_Tanimi = "Hepatit B",
                                    Guncelleme_Suresi_Ay = 0,
                                    Dozu = "1",
                                    Yapilma_Tarihi = CastDate(jsItem.HepatitAsiTarihi),
                                    Muhtamel_Tarih = IsPropertyExist(jsItem, "HepatitAsiRapelTarihi") ? CastDate(jsItem.HepatitAsiRapelTarihi) : default(DateTime?),
                                    UserId = User.Identity.GetUserId()
                                };
                                await _asiService.AddAsync(asi);
                            }

                            if (IsPropertyExist(jsItem, "TetanozAsiTarihi"))
                            {
                                Asi asi = new Asi()
                                {
                                    Personel_Id = persInsert.Personel_Id,
                                    Asi_Tanimi = "Tetanoz, difteri (Td)",
                                    Guncelleme_Suresi_Ay = 0,
                                    Dozu = "1",
                                    Yapilma_Tarihi = CastDate(jsItem.TetanozAsiTarihi),
                                    Muhtamel_Tarih = IsPropertyExist(jsItem, "TetanozAsiRapelTarihi") ? CastDate(jsItem.TetanozAsiRapelTarihi) : default(DateTime?),
                                    UserId = User.Identity.GetUserId()
                                };
                                await _asiService.AddAsync(asi);
                            }

                            if (IsPropertyExist(jsItem, "GripAsiTarihi"))
                            {
                                Asi asi = new Asi()
                                {
                                    Personel_Id = persInsert.Personel_Id,
                                    Asi_Tanimi = "İnfluenza(Grip)",
                                    Guncelleme_Suresi_Ay = 0,
                                    Dozu = "1",
                                    Yapilma_Tarihi = CastDate(jsItem.GripAsiTarihi),
                                    Muhtamel_Tarih = IsPropertyExist(jsItem, "GripAsiRapelTarihi") ? CastDate(jsItem.GripAsiRapelTarihi) : default(DateTime?),
                                    UserId = User.Identity.GetUserId()
                                };
                                await _asiService.AddAsync(asi);
                            }

                            dictionary.Add("Adı Soyadı", persInsert.Adi + " " + persInsert.Soyadi);
                            dictionary.Add("TcNo", persInsert.TcNo);
                            dictionary.Add("Şirket", jsItem.Sirket);
                            dictionary.Add("Bölüm", jsItem.Bolum);
                            dictionary.Add("durum", true);
                            dictionary.Add("Kayıt Durumu", "Başarılı bir kayıt yapıldı..");
                        }
                        else
                        {
                            dictionary.Add("Adı Soyadı", personel.Adi + " " + personel.Soyadi);
                            dictionary.Add("TcNo", jsItem.TcNo);
                            dictionary.Add("Şirket", jsItem.Sirket);
                            dictionary.Add("Bölüm", jsItem.Bolum);
                            dictionary.Add("durum", true);
                            dictionary.Add("Kayıt Durumu", "Bu Kişinin Kaydı Yapılmış.Tekrar kayıt yapılamaz.");
                        };

                    }
                    else
                    {
                        dictionary.Add("Adı Soyadı", personel.Adi + " " + personel.Soyadi);
                        dictionary.Add("TcNo", jsItem.TcNo);
                        dictionary.Add("Şirket", jsItem.Sirket);
                        dictionary.Add("Bölüm", jsItem.Bolum);
                        dictionary.Add("durum", true);
                        dictionary.Add("Kayıt Durumu", "Tc Kimlik No Hatalı veya Yok.Kontrol edin.");
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
                            dictionary.Add("Adı Soyadı", jsItem.Adi + " " + jsItem.Soyadi);
                            dictionary.Add("TcNo", jsItem.TcNo);
                            dictionary.Add("Şirket", jsItem.Sirket);
                            dictionary.Add("Bölüm", jsItem.Bolum);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("Adı Soyadı", jsItem.Adi + " " + jsItem.Soyadi);
                    dictionary.Add("TcNo", jsItem.TcNo);
                    dictionary.Add("Şirket", jsItem.Sirket);
                    dictionary.Add("Bölüm", jsItem.Bolum);
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

        [Route("ExtractPrsList")]
        [HttpPost]
        public async Task<IHttpActionResult> ExtractPersList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                dictionary.Add("Donus_Id", jsItem.id);
                Personel personel = new Personel()
                {
                    TcNo = Cast(jsItem.TcNo, typeof(string))
                };
                Personel pers = await _personelService.TcNoKontrol(personel.TcNo);
                if (pers != null)
                    dictionary.Add("durum", true);
                else dictionary.Add("durum", false);
                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }


        [Route("UpdatePrsList")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePrsList(JArray paramList)
        {
            List<ExpandoObject> expandoList = new List<ExpandoObject>();
            var identity = User.Identity.GetUserId();
            bool kisiVarmi = false;
            foreach (dynamic jsItem in paramList)
            {
                dynamic data = new ExpandoObject();
                IDictionary<string, object> dictionary = (IDictionary<string, object>)data;
                try
                {
                    dictionary.Add("Donus_Id", jsItem.id);
                    if (!String.IsNullOrEmpty(Cast(jsItem.AdiSoyadi, typeof(string))))
                    {
                        string filtre = @"\s+";
                        Regex nesne = new Regex(filtre);
                        string[] parcalar = nesne.Split(Cast(jsItem.AdiSoyadi, typeof(string)));
                        jsItem.Soyadi = parcalar[parcalar.Length - 1].Trim();
                        string firstItem = "";
                        for (int i = 0; i < parcalar.Length - 1; i++)
                        {
                            firstItem = firstItem + " " + parcalar[i];
                        }
                        jsItem.Adi = firstItem.Trim();
                    };

                    bool yCinsiyet = false;
                    string eCinsiyet = Cast(jsItem.Cinsiyet, typeof(string));
                    if (eCinsiyet != null)
                        yCinsiyet = eCinsiyet[0] == 'E' || eCinsiyet[0] == 'e' || eCinsiyet.ToLower() == "bay";

                    if (String.IsNullOrEmpty(Cast(jsItem.TcNo, typeof(string))))
                    {
                        return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "TcNo olmadan güncelleme yapamazsınız."));
                    };

                    Personel persUpdate = await _personelService.FindAsync(new Personel()
                    {
                        TcNo = Cast(jsItem.TcNo, typeof(string)).ToUpper(new CultureInfo("tr-TR", false)).Trim()
                    });

                    if (persUpdate != null)
                    {
                        kisiVarmi = true;

                        if (Cast(jsItem.Sirket_Id, typeof(int)) > 0 && Cast(jsItem.Bolum_id, typeof(int)) > 0)
                        {
                            persUpdate.Sirket_Id = IsPropertyExist(jsItem, "Sirket_Id") ? Cast(jsItem.Sirket_Id, typeof(int)) : 0;
                            persUpdate.Bolum_Id = IsPropertyExist(jsItem, "Bolum_id") ? Cast(jsItem.Bolum_id, typeof(int)) : 0;
                            Calisma_Durumu calismaDurumu = new Calisma_Durumu
                            {
                                Sirket = jsItem.Sirket,
                                Sirket_Id = IsPropertyExist(jsItem, "Sirket_Id") ? Cast(jsItem.Sirket_Id, typeof(int)) : 0,
                                Bolum = jsItem.Bolum,
                                Bolum_Id = IsPropertyExist(jsItem, "Bolum_id") ? Cast(jsItem.Bolum_id, typeof(int)) : 0,
                                Baslama_Tarihi = IsPropertyExist(jsItem, "Ise_Baslama_Tarihi") ? CastDate(jsItem.Ise_Baslama_Tarihi) : GuncelTarih(),
                                Bitis_Tarihi = IsPropertyExist(jsItem, "Is_Bitis_Tarihi") ? CastDate(jsItem.Is_Bitis_Tarihi) : default(DateTime?),
                                Calisma_Duzeni = IsPropertyExist(jsItem, "Is_Calisma_Duzeni") ? Cast(jsItem.Is_Calisma_Duzeni, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Görevli",
                                KadroDurumu = IsPropertyExist(jsItem, "Is_KadroDurumu") ? Cast(jsItem.Is_KadroDurumu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.KadroDurumu?.TrimEnd(),
                                Gorevi = IsPropertyExist(jsItem, "Is_Gorevi") ? Cast(jsItem.Is_Gorevi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.Gorevi?.TrimEnd(),
                                SicilNo = IsPropertyExist(jsItem, "Is_SicilNo") ? Cast(jsItem.Is_SicilNo, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.SicilNo ?? null,
                                Status = true,
                                UserId = User.Identity.GetUserId(),
                                Personel_Id = persUpdate.Personel_Id
                            };
                            await _calisma_DurumuService.AddAsync(calismaDurumu);
                        };
                        //durum bilgisi yeniden güncellenecekse...
                        if ((IsPropertyExist(jsItem, "Is_Bitis_Tarihi") ||
                            IsPropertyExist(jsItem, "Is_Calisma_Duzeni") ||
                            IsPropertyExist(jsItem, "Is_KadroDurumu") ||
                            IsPropertyExist(jsItem, "Is_Gorevi") ||
                            IsPropertyExist(jsItem, "Is_SicilNo")
                            ) && Cast(jsItem.Sirket_Id, typeof(int)) < 1)
                        {
                            Calisma_Durumu calismaDurumu = (await _calisma_DurumuService.FindAllAsync(new Calisma_Durumu() { Calisma_Durumu_Id = persUpdate.Personel_Id })).ToList().LastOrDefault();
                            calismaDurumu.Bitis_Tarihi = IsPropertyExist(jsItem, "Is_Bitis_Tarihi") ? CastDate(jsItem.Is_Bitis_Tarihi) : calismaDurumu.Bitis_Tarihi;
                            calismaDurumu.Calisma_Duzeni = IsPropertyExist(jsItem, "Is_Calisma_Duzeni") ? Cast(jsItem.Is_Calisma_Duzeni, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : calismaDurumu.Calisma_Duzeni;
                            calismaDurumu.KadroDurumu = IsPropertyExist(jsItem, "Is_KadroDurumu") ? Cast(jsItem.Is_KadroDurumu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.KadroDurumu != null ? persUpdate.KadroDurumu.TrimEnd() : calismaDurumu.KadroDurumu;
                            calismaDurumu.Gorevi = IsPropertyExist(jsItem, "Is_Gorevi") ? Cast(jsItem.Is_Gorevi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.Gorevi != null ? persUpdate.Gorevi.TrimEnd() : calismaDurumu.Gorevi;
                            calismaDurumu.SicilNo = IsPropertyExist(jsItem, "Is_SicilNo") ? Cast(jsItem.Is_SicilNo, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.SicilNo ?? calismaDurumu.SicilNo;
                            await _calisma_DurumuService.UpdateAsync(calismaDurumu, calismaDurumu.Calisma_Durumu_Id);
                            //}
                        };

                        persUpdate.Adi = IsPropertyExist(jsItem, "Adi") ? Cast(jsItem.Adi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.Adi;
                        persUpdate.Soyadi = IsPropertyExist(jsItem, "Soyadi") ? Cast(jsItem.Soyadi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.Soyadi;
                        persUpdate.TcNo = IsPropertyExist(jsItem, "TcNo") ? Cast(jsItem.TcNo, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.TcNo;
                        persUpdate.KanGrubu = IsPropertyExist(jsItem, "KanGrubu") ? Cast(jsItem.KanGrubu, typeof(string)) : persUpdate.KanGrubu;
                        persUpdate.SgkNo = IsPropertyExist(jsItem, "SgkNo") ? Cast(jsItem.SgkNo, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.SgkNo;
                        persUpdate.Mail = IsPropertyExist(jsItem, "Mail") ? Cast(jsItem.Mail, typeof(string)).Trim() : persUpdate.Mail;
                        persUpdate.Telefon = IsPropertyExist(jsItem, "Telefon") ? TelDuzelt(Cast(jsItem.Telefon, typeof(string))) : persUpdate.Telefon;
                        persUpdate.KadroDurumu = IsPropertyExist(jsItem, "Is_KadroDurumu") ? Cast(jsItem.Is_KadroDurumu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.KadroDurumu;
                        persUpdate.Gorevi = IsPropertyExist(jsItem, "Is_Gorevi") ? Cast(jsItem.Is_Gorevi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.Gorevi;
                        persUpdate.SicilNo = IsPropertyExist(jsItem, "Is_SicilNo") ? Cast(jsItem.Is_SicilNo, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persUpdate.SicilNo;
                        persUpdate.Status = IsPropertyExist(jsItem, "DurumuAktifmi") ? (Cast(jsItem.DurumuAktifmi, typeof(string)).Trim() == "Aktif" || Cast(jsItem.DurumuAktifmi, typeof(string)).Trim() == "Evet" || Cast(jsItem.DurumuAktifmi, typeof(string)).Trim() == "1" ? persUpdate.Status = true : persUpdate.Status = false) : persUpdate.Status;
                        await _personelService.UpdateAsync(persUpdate, persUpdate.Personel_Id);
                    }
                    else throw new ArgumentException("Sistemde böyle kayıtlı bir kişi bulunmamaktadır.Güncelleme yapamayız.!");

                    PersonelDetayi persDetayUpdate = await _personelDetayiService.FindAsync(new PersonelDetayi()
                    {
                        PersonelDetay_Id = persUpdate.Personel_Id
                    });
                    if (jsItem.EgitimSeviyesi != null)
                    {
                        string s = Cast(jsItem.EgitimSeviyesi, typeof(string));
                        switch (s.Substring(0, 4).ToLower(new CultureInfo("tr-TR", false)))
                        {
                            case "lise":
                                jsItem.EgitimSeviyesi = "LISE VE DENGİ O";
                                break;
                            case "üniv":
                                jsItem.EgitimSeviyesi = "UNIVERSİTE";
                                break;
                            case "ilko":
                                jsItem.EgitimSeviyesi = "ILKOKUL";
                                break;
                            case "orta":
                                jsItem.EgitimSeviyesi = "ORTAOKUL";
                                break;
                            case "önli":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "ön l":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "yüks":
                                jsItem.EgitimSeviyesi = "YUKSEK OKUL";
                                break;
                            case "mast":
                                jsItem.EgitimSeviyesi = "MASTER";
                                break;
                            case "doct":
                                jsItem.EgitimSeviyesi = "DOKTORA";
                                break;
                            default:
                                jsItem.EgitimSeviyesi = "YOK";
                                break;
                        }

                    }

                    if (persDetayUpdate != null)
                    {
                        persDetayUpdate.DogumYeri = IsPropertyExist(jsItem, "DogumYeri") ? Cast(jsItem.DogumYeri, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.DogumYeri;
                        persDetayUpdate.DogumTarihi = IsPropertyExist(jsItem, "DogumTarihi") ? CastDate(jsItem.DogumTarihi) : persDetayUpdate.DogumTarihi;
                        persDetayUpdate.IlkIseBaslamaTarihi = IsPropertyExist(jsItem, "IlkIseBaslamaTarihi") ? CastDate(jsItem.IlkIseBaslamaTarihi) : persDetayUpdate.IlkIseBaslamaTarihi;
                        persDetayUpdate.CocukSayisi = IsPropertyExist(jsItem, "CocukSayisi") ? Cast(jsItem.CocukSayisi, typeof(int)) : persDetayUpdate.CocukSayisi;
                        persDetayUpdate.KardesSayisi = IsPropertyExist(jsItem, "KardesSayisi") ? Cast(jsItem.KardesSayisi, typeof(int)) : persDetayUpdate.KardesSayisi;
                        persDetayUpdate.Cinsiyet = IsPropertyExist(jsItem, "Cinsiyet") ? yCinsiyet : persDetayUpdate.Cinsiyet;
                        persDetayUpdate.Uyruk = IsPropertyExist(jsItem, "Uyruk") ? Cast(jsItem.Uyruk, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.Uyruk;
                        persDetayUpdate.EgitimSeviyesi = IsPropertyExist(jsItem, "EgitimSeviyesi") ? Cast(jsItem.EgitimSeviyesi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.EgitimSeviyesi;
                        persDetayUpdate.AskerlikDurumu = IsPropertyExist(jsItem, "AskerlikDurumu") ? Cast(jsItem.AskerlikDurumu, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.AskerlikDurumu;
                        persDetayUpdate.MedeniHali = IsPropertyExist(jsItem, "MedeniHali") ? Cast(jsItem.MedeniHali, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.MedeniHali;
                        persDetayUpdate.anne_adi = IsPropertyExist(jsItem, "anne_adi") ? Cast(jsItem.anne_adi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.anne_adi;
                        persDetayUpdate.baba_adi = IsPropertyExist(jsItem, "baba_adi") ? Cast(jsItem.baba_adi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.baba_adi;
                        persDetayUpdate.anne_sag_bilgisi = IsPropertyExist(jsItem, "anne_sag_bilgisi") ? Cast(jsItem.anne_sag_bilgisi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.anne_sag_bilgisi;
                        persDetayUpdate.baba_sag_bilgisi = IsPropertyExist(jsItem, "baba_sag_bilgisi") ? Cast(jsItem.baba_sag_bilgisi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : persDetayUpdate.baba_sag_bilgisi;
                        await _personelDetayiService.UpdateAsync(persDetayUpdate, persUpdate.Personel_Id);
                    };

                    Adres adres = new Adres()
                    {
                        Personel_Id = persUpdate.Personel_Id,
                        Adres_Turu = "Ev Adresi",
                        GenelAdresBilgisi = jsItem.GenelAdresBilgisi,
                        Status = true,
                        UserId = User.Identity.GetUserId()
                    };
                    if (!String.IsNullOrEmpty(adres.GenelAdresBilgisi))
                        await _adresService.AddAsync(adres);

                    Ozurluluk ozurluluk = new Ozurluluk()
                    {

                        Personel_Id = persUpdate.Personel_Id,
                        HastalikTanimi = jsItem.OzurlulukHastalikTanimi,
                        Oran = IsPropertyExist(jsItem, "OzurlulukOran") ? Cast(jsItem.OzurlulukOran, typeof(int)) : 0,
                        Aciklama = jsItem.OzurlulukAciklama,
                        UserId = User.Identity.GetUserId()
                    };
                    if (!String.IsNullOrEmpty(ozurluluk.HastalikTanimi))
                        await _ozurlulukService.AddAsync(ozurluluk);

                    if (IsPropertyExist(jsItem, "HepatitAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "Hepatit B",
                            Dozu = "1",
                            Guncelleme_Suresi_Ay = 0,
                            Yapilma_Tarihi = CastDate(jsItem.HepatitAsiTarihi),
                            Muhtamel_Tarih = IsPropertyExist(jsItem, "HepatitAsiRapelTarihi") ? CastDate(jsItem.HepatitAsiRapelTarihi) : default(DateTime?),
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.Çakışma olabilir.");
                    }

                    if (IsPropertyExist(jsItem, "TetanozAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "Tetanoz, difteri (Td)",
                            Guncelleme_Suresi_Ay = 0,
                            Dozu = "1",
                            Yapilma_Tarihi = CastDate(jsItem.TetanozAsiTarihi),
                            Muhtamel_Tarih = IsPropertyExist(jsItem, "TetanozAsiRapelTarihi") ? CastDate(jsItem.TetanozAsiRapelTarihi) : default(DateTime?),
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.");
                    }

                    if (IsPropertyExist(jsItem, "GripAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "İnfluenza(Grip)",
                            Guncelleme_Suresi_Ay = 0,
                            Dozu = "1",
                            Yapilma_Tarihi = CastDate(jsItem.GripAsiTarihi),
                            Muhtamel_Tarih = IsPropertyExist(jsItem, "GripAsiRapelTarihi") ? CastDate(jsItem.GripAsiRapelTarihi) : default(DateTime?),
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.");
                    }

                    if (IsPropertyExist(jsItem, "SinovacAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "Sinovac",
                            Guncelleme_Suresi_Ay = 0,
                            Dozu = "1",
                            Yapilma_Tarihi = CastDate(jsItem.SinovacAsiTarihi),
                            Muhtamel_Tarih = default,
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.");
                    }

                    if (IsPropertyExist(jsItem, "BiontecAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "Biontec",
                            Guncelleme_Suresi_Ay = 0,
                            Dozu = "1",
                            Yapilma_Tarihi = CastDate(jsItem.BiontecAsiTarihi),
                            Muhtamel_Tarih = default,
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.");
                    }

                    if (IsPropertyExist(jsItem, "TurkovacAsiTarihi"))
                    {
                        Asi asi = new Asi()
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            Asi_Tanimi = "Turkovac",
                            Guncelleme_Suresi_Ay = 0,
                            Dozu = "1",
                            Yapilma_Tarihi = CastDate(jsItem.TurkovacAsiTarihi),
                            Muhtamel_Tarih = default,
                            UserId = User.Identity.GetUserId()
                        };
                        Asi asm = await _asiService.MukerrerAsiKontrolu((DateTime)asi.Yapilma_Tarihi, persUpdate.Personel_Id);
                        if (asm == null)
                            await _asiService.AddAsync(asi);
                        else
                            throw new ArgumentException("Mükerrer AŞI kaydı var.Aşı tarihini kontrol ediniz.");
                    }

                    if (IsPropertyExist(jsItem, "DisRapor_BaslangicTarihi") && IsPropertyExist(jsItem, "DisRapor_BitisTarihi"))
                    {
                        DisRapor disRapor = new DisRapor
                        {
                            Personel_Id = persUpdate.Personel_Id,
                            MuayeneTuru = "Dışarıdan İstirahat",
                            Tani = IsPropertyExist(jsItem, "DisRapor_Tani") ? Cast(jsItem.DisRapor_Tani, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Tanı Eklenmedi",
                            DoktorAdi = IsPropertyExist(jsItem, "DisRapor_DoktorAdi") ? Cast(jsItem.DisRapor_DoktorAdi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Doktor Adı Yok",
                            RaporuVerenSaglikBirimi = IsPropertyExist(jsItem, "DisRapor_RaporuVerenSaglikBirimi") ? Cast(jsItem.DisRapor_RaporuVerenSaglikBirimi, typeof(string)).Trim().ToUpper(new CultureInfo("tr-TR", false)) : "Hastane tanımlanmadı",
                            BaslangicTarihi = (CastDate(jsItem.DisRapor_BaslangicTarihi))?.Date,
                            BitisTarihi = (CastDate(jsItem.DisRapor_BitisTarihi))?.Date,
                            Bolum_Id = persUpdate.Bolum_Id,
                            Sirket_Id = persUpdate.Sirket_Id,
                            User_Id = User.Identity.GetUserId(),
                            Revir_Id = 1
                        };
                        TimeSpan df = (TimeSpan)(disRapor.BitisTarihi - disRapor.BaslangicTarihi);
                        disRapor.SureGun = Convert.ToInt32(df.TotalDays);
                        if (await _disRaporService.MukerrerDisRaporTakibi((DateTime)disRapor.BaslangicTarihi, (DateTime)disRapor.BitisTarihi, persUpdate.Personel_Id))
                            throw new ArgumentException("Mükerrer rapor kaydı var.Rapor başlama tarihini kontrol ediniz.");
                        else await _disRaporService.AddAsync(disRapor);
                    }
                    dictionary.Add("Adı Soyadı", persUpdate.Adi + " " + persUpdate.Soyadi);
                    dictionary.Add("TcNo", persUpdate.TcNo);
                    dictionary.Add("Şirket", "");
                    dictionary.Add("Bölüm", "");
                    dictionary.Add("durum", true);
                    dictionary.Add("Kayıt Durumu", "Başarılı bir güncelleme yapıldı..");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errs in ex.EntityValidationErrors)
                    {
                        foreach (var err in errs.ValidationErrors)
                        {
                            var propName = err.PropertyName;
                            var errMess = err.ErrorMessage;
                            dictionary.Add("Adı Soyadı", jsItem.Adi + " " + jsItem.Soyadi);
                            dictionary.Add("TcNo", jsItem.TcNo);
                            dictionary.Add("Şirket", jsItem.Sirket);
                            dictionary.Add("Bölüm", jsItem.Bolum);
                            dictionary.Add("durum", false);
                            dictionary.Add("Kayıt Durumu", "Validation Hatası" + propName + ": " + errMess + " Sistem değerleri ile verilen alanlar uyuşmuyor. Bu alanı kaldırın.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    dictionary.Add("Adı Soyadı", kisiVarmi == true ? jsItem.Adi + " " + jsItem.Soyadi : "Sistemde TcNo Kaydı yok!");
                    dictionary.Add("TcNo", jsItem.TcNo);
                    dictionary.Add("Şirket", jsItem.Sirket);
                    dictionary.Add("Bölüm", jsItem.Bolum);
                    dictionary.Add("durum", false);
                    dictionary.Add("Kayıt Durumu", "Sistem Hatası: " + ex.Message.ToString());
                }

                expandoList.Add(data);
                data = null;
            }
            return Ok(await Task.Run(() => expandoList));
        }

        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        #endregion
    }
}

