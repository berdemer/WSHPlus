using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/pansuman")]
    public class PansumanController : ApiController
    {

        private IPansumanService _pansumanService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        public PansumanController(IPansumanService pansumanService,
            IRevirIslemService revirIslemService
            )
        {
            _pansumanService = pansumanService;
            _revirIslemService = revirIslemService;
        }

        [HttpGet]
        public async Task<Pansuman> GetById(int id)
        {
            return await _pansumanService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Pansuman>> Get()
        {
            return await _pansumanService.GetAllAsync();
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]Pansuman pansuman)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = pansuman.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Ýþlemi Kapanmýþtýr."));
            //pansuman.Tarih = GuncelTarih();
            pansuman.UserId = User.Identity.GetUserId();
            return Ok(await _pansumanService.UpdateAsync(pansuman, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<Pansuman> Post(Pansuman pansuman, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = pansuman.MuayeneTuru, IslemDetayi ="Pansuman" , UserId = User.Identity.GetUserId() }, prt);
            pansuman.RevirIslem_Id = rv.RevirIslem_Id;
            pansuman.Protokol = rv.Protokol;
            pansuman.Tarih = GuncelTarih();
            pansuman.UserId = User.Identity.GetUserId();
            return await _pansumanService.AddAsync(pansuman);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Pansuman pansuman = await _pansumanService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = pansuman.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _pansumanService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)pansuman.RevirIslem_Id));
        }

    }
}
