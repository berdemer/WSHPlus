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
	[RoutePrefix("api/icRapor")]
	public class IcRaporController : ApiController
	{

		private IIcRaporService _icRaporService { get; set; }

		public IcRaporController(IIcRaporService icRaporService)
		{
			_icRaporService = icRaporService;
		}

		[HttpGet]
		public async Task<IcRapor> GetById(int id)
		{
			return await _icRaporService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<IcRapor>> Get()
		{
			return await _icRaporService.GetAllAsync();
		}

		[HttpPut]
		public async Task<IcRapor> Put(int key, [FromBody]IcRapor icRapor)
		{
            icRapor.UserId = User.Identity.GetUserId();
            return await _icRaporService.UpdateAsync(icRapor,key);
		}

		[HttpPost]
		public async Task<IcRapor> Post(IcRapor icRapor)
		{
            icRapor.UserId = User.Identity.GetUserId();
            return await _icRaporService.AddAsync(icRapor);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _icRaporService.DeleteAsync(id);
		}

	}
}
