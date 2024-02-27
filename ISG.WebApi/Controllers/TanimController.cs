using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Linq;
using System.Globalization;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/tanim")]
    public class TanimController : BaseApiController
    {

        private ITanimService _tanimService { get; set; }

        public TanimController(ITanimService tanimService)
        {
            _tanimService = tanimService;
        }
        [Route("SikayetAra/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> Sikayet(string value)
        {
            ICollection<Tanim> sikayetler = await _tanimService.SikayetAra(value);
            foreach (var item in sikayetler)
            {
                item.ifade = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.ifade.ToLower(new CultureInfo("tr-TR", false)));

            }
            return Ok(sikayetler);
        }

        [Route("BulguAra/{value}")]
        [HttpGet]
        public async Task<IHttpActionResult> Bulgu(string value)
        {
            ICollection<Tanim> bulgular = await _tanimService.BulguAra(value);
            foreach (var item in bulgular)
            {
                item.ifade = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.ifade.ToLower(new CultureInfo("tr-TR", false)));

            }
            return Ok(bulgular);
        }

        [HttpGet]
        public async Task<IEnumerable<Tanim>> Get()
        {
            IEnumerable<Tanim> liste = await _tanimService.GetAllAsync();
            var sd = liste.Select(x => x.tanimAdi).Distinct();
            return liste;
        }

        [Route("Gfx")]
        [HttpGet]
        public async Task<IHttpActionResult> Gfx()
        {
            IEnumerable<Tanim> liste = await _tanimService.GetAllAsync();
            IEnumerable<string> sd = liste.Select(x => x.tanimAdi).Distinct();
            return Ok(new ert() { uniqListe = sd, anaListe = liste });
        }

        //http://localhost:1943/api/tanim?tanimAd=Sevk Bölüm Tablosu
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Tanim>> tanimList(string tanimAd)
        {
            Tanim sd = new Tanim() { tanimAdi = tanimAd };
            return await _tanimService.FindAllAsync(sd);
        }

        [Route("ilceler")]
        [HttpGet]
        public async Task<IEnumerable<Tanim>> ilceList(string tanimAd)
        {
            Tanim sd = new Tanim() { ifadeBagimliligi = tanimAd };
            return await _tanimService.llceAllAsync(sd);
        }

        [Route("Update/{key}")]
        [HttpPost]
        public async Task<Tanim> Update(int key, [FromBody]Tanim tanim)
        {
            return await _tanimService.UpdateAsync(tanim, key);
        }

        [HttpPost]
        public async Task<Tanim> Post(Tanim tanim)
        {
            return await _tanimService.AddAsync(tanim);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _tanimService.DeleteAsync(id);
        }

        public class ert
        {
            public IEnumerable<Tanim> anaListe { get; set; }
            public IEnumerable<string> uniqListe { get; set; }
        }



    }
}

