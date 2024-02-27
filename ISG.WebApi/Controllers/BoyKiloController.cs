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
	[RoutePrefix("api/boyKilo")]
	public class BoyKiloController : ApiController
	{

		private IBoyKiloService _boyKiloService { get; set; }
		private IRevirIslemService _revirIslemService { get; set; }
		public BoyKiloController(IBoyKiloService boyKiloService,
			IRevirIslemService revirIslemService
			)
		{
			_boyKiloService = boyKiloService;
			_revirIslemService = revirIslemService;
		}
		private DateTime GuncelTarih()
		{
			TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
			return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
		}

		[HttpGet]
		public async Task<BoyKilo> GetById(int id)
		{
			return await _boyKiloService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<BoyKilo>> Get()
		{
			return await _boyKiloService.GetAllAsync();
		}

		[HttpPut]
		public async Task<IHttpActionResult> Put(int key, [FromBody]BoyKilo boyKilo)
		{
			TimeSpan zaman = new TimeSpan();
			zaman = boyKilo.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
			boyKilo.Tarih = GuncelTarih();
			boyKilo.UserId = User.Identity.GetUserId();
			return Ok(await _boyKiloService.UpdateAsync(boyKilo,key));
		}

		[Route("{SB_Id}/{prt}")]
		[HttpPost]
		public async Task<BoyKilo> Post(BoyKilo boyKilo, int SB_Id, bool prt)
		{
			RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru = boyKilo.MuayeneTuru, IslemDetayi = "BoyKilo", UserId = User.Identity.GetUserId() }, prt);
			boyKilo.RevirIslem_Id = rv.RevirIslem_Id;
			boyKilo.Protokol = rv.Protokol;
			boyKilo.Tarih = GuncelTarih();
			boyKilo.UserId = User.Identity.GetUserId();
			return await _boyKiloService.AddAsync(boyKilo);
		}

		[HttpDelete]
		public async Task<IHttpActionResult> Delete(int id)
		{
			BoyKilo boyKilo = await _boyKiloService.GetAsync(id);
			TimeSpan zaman = new TimeSpan();
			zaman = boyKilo.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
			await _boyKiloService.DeleteAsync(id);
			return Ok(await _revirIslemService.DeleteAsync((int)boyKilo.RevirIslem_Id));
		}

	}
}
