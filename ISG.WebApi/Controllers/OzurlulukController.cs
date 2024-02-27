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
	[RoutePrefix("api/ozurluluk")]
	public class OzurlulukController : ApiController
	{

		private IOzurlulukService _ozurlulukService { get; set; }

		public OzurlulukController(IOzurlulukService ozurlulukService)
		{
			_ozurlulukService = ozurlulukService;
		}

		[HttpGet]
		public async Task<Ozurluluk> GetById(int id)
		{
			return await _ozurlulukService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Ozurluluk>> Get()
		{
			return await _ozurlulukService.GetAllAsync();
		}

		[HttpPut]
		public async Task<Ozurluluk> Put(int key, [FromBody]Ozurluluk ozurluluk)
		{
            ozurluluk.UserId = User.Identity.GetUserId();
            return await _ozurlulukService.UpdateAsync(ozurluluk,key);
		}

		[HttpPost]
		public async Task<Ozurluluk> Post(Ozurluluk ozurluluk)
		{
            ozurluluk.UserId = User.Identity.GetUserId();
            return await _ozurlulukService.AddAsync(ozurluluk);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _ozurlulukService.DeleteAsync(id);
		}

	}
}
