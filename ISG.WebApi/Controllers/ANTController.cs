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
    [RoutePrefix("api/ant")]
    public class ANTController : ApiController
    {

        private IANTService _aNTService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        public ANTController(IANTService aNTService,
            IRevirIslemService revirIslemService
            )
        {
            _aNTService = aNTService;
            _revirIslemService = revirIslemService;
        }

        [HttpGet]
        public async Task<ANT> GetById(int id)
        {
            return await _aNTService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<ANT>> Get()
        {
            return await _aNTService.GetAllAsync();
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]ANT aNT)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = aNT.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Ýþlemi Kapanmýþtýr."));
            //aNT.Tarih = GuncelTarih();
            aNT.UserId = User.Identity.GetUserId();
            return Ok(await _aNTService.UpdateAsync(aNT, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<ANT> Post(ANT aNT, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =aNT.MuayeneTuru, IslemDetayi ="ANT" , UserId = User.Identity.GetUserId() }, prt);
            aNT.RevirIslem_Id = rv.RevirIslem_Id;
            aNT.Protokol = rv.Protokol;
            aNT.Tarih = GuncelTarih();
            aNT.UserId = User.Identity.GetUserId();
            return await _aNTService.AddAsync(aNT);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            ANT aNT = await _aNTService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = aNT.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _aNTService.DeleteAsync(id);
            return Ok( await _revirIslemService.DeleteAsync((int)aNT.RevirIslem_Id));
        }

    }
}