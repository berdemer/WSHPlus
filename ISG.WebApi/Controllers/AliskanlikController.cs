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
	[RoutePrefix("api/aliskanlik")]
	public class AliskanlikController : ApiController
	{

		private IAliskanlikService _aliskanlikService { get; set; }

		public AliskanlikController(IAliskanlikService aliskanlikService)
		{
			_aliskanlikService = aliskanlikService;
		}


		[HttpGet]
		public async Task<Aliskanlik> GetById(int id)
		{
			return await _aliskanlikService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Aliskanlik>> Get()
		{
			return await _aliskanlikService.GetAllAsync();
		}


		[HttpPut]
		public async Task<Aliskanlik> Put(int key, [FromBody]Aliskanlik aliskanlik)
		{
			aliskanlik.UserId = User.Identity.GetUserId();
			return await _aliskanlikService.UpdateAsync(aliskanlik,key);
		}

		[HttpPost]
		public async Task<Aliskanlik> Post(Aliskanlik aliskanlik)
		{
			aliskanlik.UserId = User.Identity.GetUserId();
			return await _aliskanlikService.AddAsync(aliskanlik);
		}


		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _aliskanlikService.DeleteAsync(id);
		}

	}
}
