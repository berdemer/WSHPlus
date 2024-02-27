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
    [RoutePrefix("api/asi")]
    public class AsiController : ApiController
    {

        private IAsiService _asiService { get; set; }

        public AsiController(IAsiService asiService)
        {
            _asiService = asiService;
        }

        [HttpGet]
        public async Task<Asi> GetById(int id)
        {
            return await _asiService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Asi>> Get()
        {
            return await _asiService.GetAllAsync();
        }

        [HttpPut]
        public async Task<Asi> Put(int key, [FromBody]Asi asi)
        {
            asi.UserId = User.Identity.GetUserId();
            return await _asiService.UpdateAsync(asi, key);
        }

        [HttpPost]
        public async Task<Asi> Post(Asi asi)
        {
            asi.UserId = User.Identity.GetUserId();
            return await _asiService.AddAsync(asi);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _asiService.DeleteAsync(id);
        }

    }
}
