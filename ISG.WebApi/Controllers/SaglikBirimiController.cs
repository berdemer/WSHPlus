using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    public class SaglikBirimiController : ApiController
    {
        private ISaglikBirimiService _saglikBirimiService { get; set; }

        public SaglikBirimiController(ISaglikBirimiService saglikBirimiService)
        {
            _saglikBirimiService = saglikBirimiService;
        }

        /*[Route("SaglikBirimi/{id}")]*///şirket Id si ve aktif olan sağlık birimlerini verir.
        [HttpGet]
        public async Task<IEnumerable<SaglikBirimi>> GetById(int id)
        {
            SaglikBirimi sbirimi = new SaglikBirimi() {StiId=id,Status=true };
            return await _saglikBirimiService.FindAllAsync(sbirimi);
        }

        [HttpPut]
        public async Task<SaglikBirimi> Put(int key, [FromBody]SaglikBirimi saglikBirimi)
        {
            return await _saglikBirimiService.UpdateAsync(saglikBirimi, key);
        }

        [HttpPost]
        public async Task<SaglikBirimi> Post(SaglikBirimi saglikBirimi)
        {
            return await _saglikBirimiService.AddAsync(saglikBirimi);
        }


        [Route("{id}")]
        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _saglikBirimiService.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<SaglikBirimi>> Get()
        {
            return await _saglikBirimiService.GetAllAsync();
        }
    }
}
