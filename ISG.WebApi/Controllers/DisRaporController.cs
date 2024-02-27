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
	[RoutePrefix("api/disRapor")]
	public class DisRaporController : ApiController
	{

		private IDisRaporService _disRaporService { get; set; }

		public DisRaporController(IDisRaporService disRaporService)
		{
			_disRaporService = disRaporService;
		}

		[HttpGet]
		public async Task<DisRapor> GetById(int id)
		{
			return await _disRaporService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<DisRapor>> Get()
		{
			return await _disRaporService.GetAllAsync();
		}

		[HttpPut]
		public async Task<DisRapor> Put(int key, [FromBody]DisRapor disRapor)
		{
            disRapor.User_Id = User.Identity.GetUserId();
            return await _disRaporService.UpdateAsync(disRapor,key);
		}

		[HttpPost]
		public async Task<DisRapor> Post(DisRapor disRapor)
		{
            disRapor.User_Id = User.Identity.GetUserId();
            return await _disRaporService.AddAsync(disRapor);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _disRaporService.DeleteAsync(id);
		}

	}
}
