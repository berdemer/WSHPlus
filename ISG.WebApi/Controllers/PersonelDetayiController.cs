using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/personelDetayi")]
	public class PersonelDetayiController : ApiController
	{

		private IPersonelDetayiService _personelDetayiService { get; set; }

		public PersonelDetayiController(IPersonelDetayiService personelDetayiService)
		{
			_personelDetayiService = personelDetayiService;
		}

		[Route("PersonelDetayi/{id}")]
		[HttpGet]
		public async Task<PersonelDetayi> GetById(int id)
		{
			return await _personelDetayiService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<PersonelDetayi>> Get()
		{
			return await _personelDetayiService.GetAllAsync();
		}

		[Route("PersonelDetayi/{key}")]
		[HttpPut]
		public async Task<PersonelDetayi> Put(int key, [FromBody]PersonelDetayi personelDetayi)
		{
			return await _personelDetayiService.UpdateAsync(personelDetayi,key);
		}

		[HttpPost]
		public async Task<PersonelDetayi> Post(PersonelDetayi personelDetayi)
		{
			return await _personelDetayiService.AddAsync(personelDetayi);
		}

		[Route("PersonelDetayi/{id}")]
		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _personelDetayiService.DeleteAsync(id);
		}

	}
}
