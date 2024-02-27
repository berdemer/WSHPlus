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
	[RoutePrefix("api/ekg")]
	public class EKGController : ApiController
	{

		private IEKGService _ekgService { get; set; }
		private IRevirIslemService _revirIslemService { get; set; }
		public EKGController(IEKGService ekgService,
			IRevirIslemService revirIslemService
			)
		{
			_ekgService = ekgService;
			_revirIslemService = revirIslemService;
		}

		[HttpGet]
		public async Task<EKG> GetById(int id)
		{
			return await _ekgService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<EKG>> Get()
		{
			return await _ekgService.GetAllAsync();
		}
		private DateTime GuncelTarih()
		{
			TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
			return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
		}
		[HttpPut]
		public async Task<IHttpActionResult> Put(int key, [FromBody]EKG ekg)
		{
			TimeSpan zaman = new TimeSpan();
			zaman = ekg.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 2) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Güncellenme Kapanmýþtýr."));
			ekg.Tarih = GuncelTarih();
			ekg.UserId = User.Identity.GetUserId();
			return Ok(await _ekgService.UpdateAsync(ekg,key));
		}

		[Route("{SB_Id}/{prt}")]
		[HttpPost]
		public async Task<EKG> Post(EKG ekg, int SB_Id, bool prt)
		{
			RevirDonusu rv = await _revirIslemService.RevirIslem(new RevirIslem() { SaglikBirimi_Id = SB_Id, IslemTuru =ekg.MuayeneTuru, IslemDetayi = "EKG" , UserId = User.Identity.GetUserId() }, prt);
			ekg.RevirIslem_Id = rv.RevirIslem_Id;
			ekg.Protokol = rv.Protokol;
			ekg.Tarih = GuncelTarih();
			ekg.UserId = User.Identity.GetUserId();
			return await _ekgService.AddAsync(ekg);
		}

		[HttpDelete]
		public async Task<IHttpActionResult> Delete(int id)
		{
			EKG ekg = await _ekgService.GetAsync(id);
			TimeSpan zaman = new TimeSpan();
			zaman = ekg.Tarih.Value - GuncelTarih();
			if (Math.Abs(zaman.Days) > 1) return Content(System.Net.HttpStatusCode.NotFound, await Task.Run(() => "Silinme Ýþlemi Kapanmýþtýr."));
			await _ekgService.DeleteAsync(id);
			return Ok(await _revirIslemService.DeleteAsync((int)ekg.RevirIslem_Id));
		}


	}
}
