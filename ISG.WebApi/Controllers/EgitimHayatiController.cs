using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/egitimHayati")]
	public class EgitimHayatiController : ApiController
	{

		private IEgitimHayatiService _egitimHayatiService { get; set; }

		public EgitimHayatiController(IEgitimHayatiService egitimHayatiService)
		{
			_egitimHayatiService = egitimHayatiService;
		}

		[HttpGet]
		public async Task<EgitimHayati> GetById(int id)
		{
			return await _egitimHayatiService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<EgitimHayati>> Get()
		{
			return await _egitimHayatiService.GetAllAsync();
		}

		[HttpPut]
		public async Task<EgitimHayati> Put(int key, [FromBody]EgitimHayati egitimHayati)
		{
            egitimHayati.UserId = User.Identity.GetUserId();
            return await _egitimHayatiService.UpdateAsync(egitimHayati,key);
		}

		[HttpPost]
		public async Task<EgitimHayati> Post(EgitimHayati egitimHayati)
		{
            egitimHayati.UserId = User.Identity.GetUserId();
            return await _egitimHayatiService.AddAsync(egitimHayati);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _egitimHayatiService.DeleteAsync(id);
		}

	}
}
