using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/tetkik")]
	public class TetkikController : ApiController
	{

		private ITetkikService _tetkikService { get; set; }

		public TetkikController(ITetkikService tetkikService)
		{
			_tetkikService = tetkikService;
		}

		[Route("Tetkik/{id}")]
		[HttpGet]
		public async Task<Tetkik> GetById(int id)
		{
			return await _tetkikService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Tetkik>> Get()
		{
			return await _tetkikService.GetAllAsync();
		}

		[Route("Tetkik/{key}")]
		[HttpPut]
		public async Task<Tetkik> Put(int key, [FromBody]Tetkik tetkik)
		{
			return await _tetkikService.UpdateAsync(tetkik,key);
		}

		[HttpPost]
		public async Task<Tetkik> Post(Tetkik tetkik)
		{
			return await _tetkikService.AddAsync(tetkik);
		}

		[Route("Tetkik/{id}")]
		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _tetkikService.DeleteAsync(id);
		}

	}
}
