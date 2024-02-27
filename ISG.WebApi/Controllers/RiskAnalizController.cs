using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/analiz")]
    public class RiskAnalizController : ApiController
    {
        private IMeslekHastaliklariService _meslekHastaliklariService { get; set; }
        private ITehlikeliIslerService _tehlikeliIslerService { get; set; }
        private IBolumRiskiService _bolumRiskiService { get; set; }
        public RiskAnalizController(IMeslekHastaliklariService meslekHastaliklariService,
            ITehlikeliIslerService tehlikeliIslerService,
            IBolumRiskiService bolumRiskiService)
        {
            _tehlikeliIslerService = tehlikeliIslerService;
            _meslekHastaliklariService = meslekHastaliklariService;
            _bolumRiskiService = bolumRiskiService;
        }

        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }

        [Route("groupList")]
        [HttpGet]
        public async Task<IHttpActionResult> groupListesi()
        {
            IEnumerable<TehlikeliIsler> cv = await _tehlikeliIslerService.GetAllAsync();
            return Ok(cv.Select(x => x.grubu.Trim()).Distinct());
        }

        [Route("groupSelect/{grp}")]
        [HttpGet]
        public async Task<IHttpActionResult> groupSelect(string grp)
        {
            IEnumerable<MeslekHastaliklari> meslek = await _meslekHastaliklariService.FindAllAsync(new MeslekHastaliklari() { grubu = grp });
            IEnumerable<TehlikeliIsler> isc = await _tehlikeliIslerService.FindAllAsync(new TehlikeliIsler() { grubu = grp });
            return Ok(new { meslekHastaliklari = meslek.Select(x => new { x.meslekHastalik, x.sure }).Distinct(), isler = isc.Select(x => x.meslek).Distinct(), grup = grp });
        }

        [Route("groupSearch/{grp}")]
        [HttpGet]
        public async Task<IHttpActionResult> groupSearch(string grp)
        {
            ICollection<string> gruplistesi = await _tehlikeliIslerService.gruplardanAra(grp);
            ICollection<TehlikeliIsler> isc = await _tehlikeliIslerService.GrupList(gruplistesi);
            ICollection<MeslekHastaliklari> mHastaliklari = await _meslekHastaliklariService.GrupList(gruplistesi);
            return Ok(new { meslekHastaliklari = mHastaliklari.Select(x => new { x.meslekHastalik, x.sure }).Distinct(), isler = isc.Select(x => x.meslek.Trim()).Distinct(), gruplistesi = gruplistesi.Distinct() });
        }

        [Route("meslekHastaliklari/{val}")]
        [HttpGet]
        public async Task<IHttpActionResult> meslekHastaliklari(string val)
        {
            IEnumerable<MeslekHastaliklari> meslekHastaliklari = await _meslekHastaliklariService.MeslekHastalikAra(val);
            ICollection<TehlikeliIsler> isc = await _tehlikeliIslerService.GrupList(meslekHastaliklari.Select(x => x.grubu).Distinct());
            return Ok(new { meslekHastaliklari = meslekHastaliklari.Select(x => new { x.meslekHastalik, x.sure }).Distinct(), isler = isc.Select(x => x.meslek).Distinct(), gruplistesi = meslekHastaliklari.Select(x => new { x.grubu }).Distinct() });
        }

        [Route("isler/{val}")]
        [HttpGet]
        public async Task<IHttpActionResult> isler(string val)
        {
            IEnumerable<TehlikeliIsler> isler = await _tehlikeliIslerService.TehlikeliIslerAra(val);
            ICollection<MeslekHastaliklari> mHastaliklari = await _meslekHastaliklariService.GrupList(isler.Select(x => x.grubu).Distinct());
            return Ok(new { meslekHastaliklari = mHastaliklari.Select(x => new { x.meslekHastalik, x.sure }).Distinct(), isler = isler.Select(x => x.meslek).Distinct(), gruplistesi = isler.Select(x => new { x.grubu }).Distinct() });
        }

        [Route("bolumRiski")]
        [HttpPost]
        public async Task<IHttpActionResult> bolumRiski(BolumRiski blm)
        {
            BolumRiski blmRiski = await _bolumRiskiService.FindAsync(new BolumRiski() { Sirket_Id = blm.Sirket_Id, Bolum_Id = blm.Bolum_Id });

            if (blmRiski == null)
            {
                blm.Tarih = GuncelTarih();
                blm.UserId = User.Identity.GetUserId();
                blmRiski = await _bolumRiskiService.AddAsync(blm);
            }
            else
            {
                blmRiski.Tarih = GuncelTarih();
                blmRiski.UserId = User.Identity.GetUserId();
                blmRiski.PMJson = blm.PMJson;
                await _bolumRiskiService.UpdateAsync(blmRiski, blmRiski.BolumRiski_Id);
            }
            return Ok(new { blmRiski = JObject.Parse(blm.PMJson), blmRiskiId = blm.BolumRiski_Id });
        }

        [Route("bolumRisk/{sti}/{blm}")]
        [HttpGet]
        public async Task<IHttpActionResult> bolumRisk(int sti, int blm)
        {
            BolumRiski blmRiski = await _bolumRiskiService.FindAsync(new BolumRiski() { Sirket_Id = sti, Bolum_Id = blm });
            return Ok(new { blmRiski = blmRiski == null ? null : JObject.Parse(blmRiski.PMJson), blmRiskiId = blmRiski == null ? 0 : blmRiski.BolumRiski_Id });
        }

        [Route("MuDuList/{Sirket_Id}/{muayeneDurumu}/{muayeneSonucu}/{year}")]
        [HttpGet]
        public async Task<IHttpActionResult> Muayene_Durum_Listesi(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            muayeneSonucu = muayeneSonucu == "hepsi" ? "" : muayeneSonucu;
            return Ok(await _bolumRiskiService.Muayene_Durum_Listesi(Sirket_Id, muayeneDurumu, muayeneSonucu, year));
        }

        [Route("BoIsKSayilari/{Sirket_Id}/{muayeneDurumu}/{muayeneSonucu}/{year}")]
        [HttpGet]
        public async Task<IHttpActionResult> BolumlerinIsKazaSayilari(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            muayeneSonucu = muayeneSonucu == "hepsi" ? "" : muayeneSonucu;
            return Ok(await _bolumRiskiService.BolumlerinIsKazaSayilari(Sirket_Id, muayeneDurumu, muayeneSonucu, year));
        }

        [Route("PerMuaTakGelen/{Sirket_Id}/{Yil}/{ay}/{sure}")]
        [HttpGet]
        public async Task<IHttpActionResult> PeriyodikMuayeneTakibiGelenler(int Sirket_Id, int Yil, int ay, int sure)
        {
            return Ok(await _bolumRiskiService.PeriyodikMuayeneTakibiGelenler(Sirket_Id,Yil,ay,sure));
        }

        [Route("AsiTakGelen/{Sirket_Id}/{Yil}/{ay}")]
        [HttpGet]
        public async Task<IHttpActionResult> AsiTakibiGelenler(int Sirket_Id, int Yil, int ay)
        {
            return Ok(await _bolumRiskiService.AsiTakibiGelenler(Sirket_Id, Yil, ay));
        }

        [Route("DisRaporlularAnalizi/{muayeneTuru}/{Sirket_Id}/{Yil}/{ay}")]
        [HttpGet]
        public async Task<IHttpActionResult> DisRaporlularAnalizi(int Sirket_Id, int Yil, int ay,string muayeneTuru)
        {
            return Ok(await _bolumRiskiService.DisRaporAnalizi(Sirket_Id, Yil, ay, muayeneTuru));
        }


        [Route("RevirIslemleri/{Id}/{day:int}/{month:int}/{year:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Gunluk_Revir_Islemleri(int Id, int day,int month,int year)
        {
            DateTime selectedDate = new DateTime(year, month, day);
            return Ok(await _bolumRiskiService.Gunluk_Revir_Islemleri(Id, selectedDate));
        }
        [Route("AylikRevirIslemleri/{Id}/{day:int}/{month:int}/{year:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> Aylik_Revir_Islemleri(int Id, int day,int month,int year)
        {
            DateTime selectedDate = new DateTime(year, month, day);
            return Ok(await _bolumRiskiService.Aylik_Revir_Islemleri(Id, selectedDate));
        }

        [Route("EngelliTakibi/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> EngelliTakibi(int Sirket_Id)
        {
            return Ok(await _bolumRiskiService.EngelliTakibi(Sirket_Id));
        }

        [Route("KronikHastaTakibi/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> KronikHastaTakibi(int Sirket_Id)
        {
            return Ok(await _bolumRiskiService.KronikHastaTakibi(Sirket_Id));
        }

        [Route("AllerjiHastaTakibi/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> AllerjiHastaTakibi(int Sirket_Id)
        {
            return Ok(await _bolumRiskiService.AllerjiHastaTakibi(Sirket_Id));
        }

        [Route("AliskanlikHastaTakibi/{Sirket_Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> AliskanlikHastaTakibi(int Sirket_Id)
        {
            return Ok(await _bolumRiskiService.AliskanlikHastaTakibi(Sirket_Id));
        }
    }
}
