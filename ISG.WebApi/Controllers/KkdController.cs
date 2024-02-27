using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/kkd")]
    public class KkdController : ApiController
    {

        private IKkdService _kkdService { get; set; }

        public KkdController(IKkdService kkdService)
        {
            _kkdService = kkdService;
        }

        [HttpGet]
        public async Task<Kkd> GetById(int id)
        {
            return await _kkdService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Kkd>> Get()
        {
            return await _kkdService.GetAllAsync();
        }

        [HttpPut]
        public async Task<Kkd> Put(int key, [FromBody]Kkd kkd)
        {
            kkd.UserId = User.Identity.GetUserId();
            return await _kkdService.UpdateAsync(kkd, key);
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPost]
        public async Task<Kkd> Post(Kkd kkd)
        {
            kkd.UserId = User.Identity.GetUserId();
            kkd.Alinma_Tarihi= GuncelTarih();
            return await _kkdService.AddAsync(kkd);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _kkdService.DeleteAsync(id);
        }

    }
}
