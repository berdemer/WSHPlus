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
	[RoutePrefix("api/soyGecmisi")]
	public class SoyGecmisiController : ApiController
	{

		private ISoyGecmisiService _soyGecmisiService { get; set; }

		public SoyGecmisiController(ISoyGecmisiService soyGecmisiService)
		{
			_soyGecmisiService = soyGecmisiService;
		}

		[HttpGet]
		public async Task<SoyGecmisi> GetById(int id)
		{
			return await _soyGecmisiService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<SoyGecmisi>> Get()
		{
			return await _soyGecmisiService.GetAllAsync();
		}
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPut]
		public async Task<SoyGecmisi> Put(int key, [FromBody]SoyGecmisi soyGecmisi)
		{
			soyGecmisi.UserId = User.Identity.GetUserId();
			soyGecmisi.Tarihi = GuncelTarih();
			return await _soyGecmisiService.UpdateAsync(soyGecmisi,key);
		}

		[HttpPost]
		public async Task<SoyGecmisi> Post(SoyGecmisi soyGecmisi)
		{
			soyGecmisi.UserId = User.Identity.GetUserId();
			soyGecmisi.Tarihi = GuncelTarih();
			return await _soyGecmisiService.AddAsync(soyGecmisi);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _soyGecmisiService.DeleteAsync(id);
		}

	}
}
