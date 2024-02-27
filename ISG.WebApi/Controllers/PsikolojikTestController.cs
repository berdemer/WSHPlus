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
    [RoutePrefix("api/psikolojiktest")]
    public class PsikolojikTestController : ApiController
    {

        private IPsikolojikTestService _psikolojikTestService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        public PsikolojikTestController(IPsikolojikTestService psikolojikTestService,
            IRevirIslemService revirIslemService
            )
        {
            _psikolojikTestService = psikolojikTestService;
            _revirIslemService = revirIslemService;
        }

        [HttpGet]
        public async Task<PsikolojikTest> GetById(int id)
        {
            return await _psikolojikTestService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<PsikolojikTest>> Get()
        {
            return await _psikolojikTestService.GetAllAsync();
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [Route("Putin/{key}")]
        [HttpPost]
        public async Task<IHttpActionResult> Putin(int key, PsikolojikTest psikolojikTest)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = psikolojikTest.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Ýþlemi Kapanmýþtýr."));
            //psikolojikTest.Tarih = GuncelTarih();
            psikolojikTest.UserId = User.Identity.GetUserId();
            return Ok(await _psikolojikTestService.UpdateAsync(psikolojikTest, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<PsikolojikTest> Post(PsikolojikTest psikolojikTest, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =psikolojikTest.MuayeneTuru, IslemDetayi ="PsikolojikTest" , UserId = User.Identity.GetUserId() }, prt);
            psikolojikTest.RevirIslem_Id = rv.RevirIslem_Id;
            psikolojikTest.Protokol = rv.Protokol;
            psikolojikTest.Tarih = GuncelTarih();
            psikolojikTest.UserId = User.Identity.GetUserId();
            return await _psikolojikTestService.AddAsync(psikolojikTest);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            PsikolojikTest psikolojikTest = await _psikolojikTestService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = psikolojikTest.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _psikolojikTestService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)psikolojikTest.RevirIslem_Id));
        }

    }
}