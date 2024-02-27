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
    [RoutePrefix("api/revirTedavi")]
    public class RevirTedaviController : ApiController
    {

        private IRevirTedaviService _revirTedaviService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        private IIlacSarfCikisiService _ilacSarfCikisiService { get; set; }

        public RevirTedaviController(IRevirTedaviService revirTedaviService,
            IRevirIslemService revirIslemService,
            IIlacSarfCikisiService ilacSarfCikisiService
            )
        {
            _revirTedaviService = revirTedaviService;
            _revirIslemService = revirIslemService;
            _ilacSarfCikisiService = ilacSarfCikisiService;
        }

        [HttpGet]
        public async Task<RevirTedavi> GetById(int id)
        {
            return await _revirTedaviService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<RevirTedavi>> Get()
        {
            return await _revirTedaviService.GetAllAsync();
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(int key, [FromBody]RevirTedavi revirTedavi)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = revirTedavi.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Ýþlemi Kapanmýþtýr."));
            revirTedavi.UserId = User.Identity.GetUserId();
            return Ok(await _revirTedaviService.UpdateAsync(revirTedavi, key));
        }

        [Route("{SB_Id}/{prt}")]
        [HttpPost]
        public async Task<RevirTedavi> Post(RevirTedavi revirTedavi, int SB_Id, bool prt)
        {
            RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =revirTedavi.MuayeneTuru , IslemDetayi = "Revir Tedavi", UserId = User.Identity.GetUserId() }, prt);
            revirTedavi.RevirIslem_Id = rv.RevirIslem_Id;
            revirTedavi.Protokol = rv.Protokol;
            revirTedavi.Tarih = GuncelTarih();
            revirTedavi.UserId = User.Identity.GetUserId();
            IEnumerable<IlacSarfCikisi> sarflar = revirTedavi.IlacSarfCikislari;
            revirTedavi.IlacSarfCikislari = null;
            RevirTedavi sonuc = await _revirTedaviService.AddAsync(revirTedavi);
            foreach (IlacSarfCikisi ilac in sarflar)
            {
                IlacSarfCikisi sd = new IlacSarfCikisi();
                sd.IlacSarfCikisi_Id = Guid.NewGuid();
                sd.IlacAdi = ilac.IlacAdi;
                sd.RevirTedavi_Id = sonuc.RevirTedavi_Id;
                sd.SaglikBirimi_Id = ilac.SaglikBirimi_Id;
                sd.SarfMiktari = ilac.SarfMiktari;
                sd.Status = true;
                sd.Tarih = GuncelTarih();
                sd.UserId = User.Identity.GetUserId();
                sd.StokId = ilac.StokId;
                await _ilacSarfCikisiService.AddAsync(sd);
            }
            return sonuc;
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            RevirTedavi revirTedavi = await _revirTedaviService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = revirTedavi.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _revirTedaviService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)revirTedavi.RevirIslem_Id));
        }

        [Route("{sarf}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(Guid key, [FromBody]IlacSarfCikisi ilacSarfCikisi)
        {
            TimeSpan zaman = new TimeSpan();
            zaman = ilacSarfCikisi.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Ýþlemi Kapanmýþtýr."));
            ilacSarfCikisi.UserId = User.Identity.GetUserId();
            return Ok(await _ilacSarfCikisiService.UpdateAsync(ilacSarfCikisi, key));
        }
        [Route("{sarf}")]
        [HttpPost]
        public async Task<IlacSarfCikisi> Post(IlacSarfCikisi ilacSarfCikisi)
        {
            ilacSarfCikisi.UserId = User.Identity.GetUserId();
            ilacSarfCikisi.IlacSarfCikisi_Id = Guid.NewGuid();
            ilacSarfCikisi.Tarih = GuncelTarih();
            ilacSarfCikisi.Status = true;
            return await _ilacSarfCikisiService.AddAsync(ilacSarfCikisi);
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [Route("{sarf}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            IlacSarfCikisi ilacSarfCikisi = await _ilacSarfCikisiService.GetAsync(id);
            TimeSpan zaman = new TimeSpan();
            zaman = ilacSarfCikisi.Tarih.Value - GuncelTarih();
            if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            return Ok(await _ilacSarfCikisiService.DeleteAsync(id));
        }

    }
}