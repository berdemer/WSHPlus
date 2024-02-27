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
	[RoutePrefix("api/calisma_Gecmisi")]
	public class Calisma_GecmisiController : ApiController
	{

		private ICalisma_GecmisiService _calisma_GecmisiService { get; set; }

		public Calisma_GecmisiController(ICalisma_GecmisiService calisma_GecmisiService)
		{
			_calisma_GecmisiService = calisma_GecmisiService;
		}

		[HttpGet]
		public async Task<Calisma_Gecmisi> GetById(int id)
		{
			return await _calisma_GecmisiService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Calisma_Gecmisi>> Get()
		{
			return await _calisma_GecmisiService.GetAllAsync();
		}

		[HttpPut]
		public async Task<Calisma_Gecmisi> Put(int key, [FromBody]Calisma_Gecmisi calisma_Gecmisi)
		{
            calisma_Gecmisi.UserId = User.Identity.GetUserId();
            return await _calisma_GecmisiService.UpdateAsync(calisma_Gecmisi,key);
		}

		[HttpPost]
		public async Task<Calisma_Gecmisi> Post(Calisma_Gecmisi calisma_Gecmisi)
		{
            calisma_Gecmisi.UserId= User.Identity.GetUserId();
            return await _calisma_GecmisiService.AddAsync(calisma_Gecmisi);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _calisma_GecmisiService.DeleteAsync(id);
		}

	}
}
