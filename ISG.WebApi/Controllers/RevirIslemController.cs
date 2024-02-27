using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/revirIslem")]
	public class RevirIslemController : ApiController
	{

		private IRevirIslemService _revirIslemService { get; set; }

		public RevirIslemController(IRevirIslemService revirIslemService)
		{
			_revirIslemService = revirIslemService;
		}

		[Route("RevirIslem/{id}")]
		[HttpGet]
		public async Task<RevirIslem> GetById(int id)
		{
			return await _revirIslemService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<RevirIslem>> Get()
		{
			return await _revirIslemService.GetAllAsync();
		}

		[Route("RevirIslem/{key}")]
		[HttpPut]
		public async Task<RevirIslem> Put(int key, [FromBody]RevirIslem revirIslem)
		{
			return await _revirIslemService.UpdateAsync(revirIslem,key);
		}

		[HttpPost]
		public async Task<RevirIslem> Post(RevirIslem revirIslem)
		{
			return await _revirIslemService.AddAsync(revirIslem);
		}

		[Route("RevirIslem/{id}")]
		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _revirIslemService.DeleteAsync(id);
		}

		[Route("RevirAnaliz/{year}/{saglikBirimi_Id}")]
		[HttpGet]
		public async Task<IHttpActionResult> RevirAnaliz(int year,int saglikBirimi_Id)
		{
			return Ok (await _revirIslemService.RevirAnaliz(year,saglikBirimi_Id));
		}
	}
}
