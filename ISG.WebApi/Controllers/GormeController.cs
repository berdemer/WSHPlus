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
	[RoutePrefix("api/gorme")]
	public class GormeController : ApiController
	{

		private IGormeService _gormeService { get; set; }
		private IRevirIslemService _revirIslemService { get; set; }
		public GormeController(IGormeService gormeService,
			IRevirIslemService revirIslemService
			)
		{
			_gormeService = gormeService;
			_revirIslemService = revirIslemService;
		}

		[HttpGet]
		public async Task<Gorme> GetById(int id)
		{
			return await _gormeService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Gorme>> Get()
		{
			return await _gormeService.GetAllAsync();
		}
		private DateTime GuncelTarih()
		{
			TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
			return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
		}
		[HttpPut]
		public async Task<IHttpActionResult> Put(int key, [FromBody]Gorme gorme)
		{
			TimeSpan zaman = new TimeSpan();
			zaman = gorme.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
			gorme.Tarih = GuncelTarih();
			gorme.UserId = User.Identity.GetUserId();
			return Ok(await _gormeService.UpdateAsync(gorme,key));
		}

		[Route("{SB_Id}/{prt}")]
		[HttpPost]
		public async Task<Gorme> Post(Gorme gorme, int SB_Id, bool prt)
		{
			RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =gorme.MuayeneTuru , IslemDetayi ="Görme Muayenesi" , UserId = User.Identity.GetUserId() }, prt);
			gorme.RevirIslem_Id = rv.RevirIslem_Id;
			gorme.Protokol = rv.Protokol;
			gorme.Tarih = GuncelTarih();
			gorme.UserId = User.Identity.GetUserId();
			return await _gormeService.AddAsync(gorme);
		}

		[HttpDelete]
		public async Task<IHttpActionResult> Delete(int id)
		{
			Gorme gorme = await _gormeService.GetAsync(id);
			TimeSpan zaman = new TimeSpan();
			zaman = gorme.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
            await _gormeService.DeleteAsync(id);
            return Ok(await _revirIslemService.DeleteAsync((int)gorme.RevirIslem_Id));
		}


	}
}
