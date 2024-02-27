using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/allerji")]
	public class AllerjiController : ApiController
	{

		private IAllerjiService _allerjiService { get; set; }

		public AllerjiController(IAllerjiService allerjiService)
		{
			_allerjiService = allerjiService;
		}

		[HttpGet]
		public async Task<Allerji> GetById(int id)
		{
			return await _allerjiService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Allerji>> Get()
		{
			return await _allerjiService.GetAllAsync();
		}

		[HttpPut]
		public async Task<Allerji> Put(int key, [FromBody]Allerji allerji)
		{
			allerji.UserId = User.Identity.GetUserId();
			return await _allerjiService.UpdateAsync(allerji,key);
		}

		[HttpPost]
		public async Task<Allerji> Post(Allerji allerji)
		{
			allerji.UserId = User.Identity.GetUserId();
			return await _allerjiService.AddAsync(allerji);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _allerjiService.DeleteAsync(id);
		}

	}
}
