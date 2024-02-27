using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using ISG.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/kronikHastalik")]
    public class KronikHastalikController : ApiController
    {
        private IKronikHastalikService _kronikHastalikService { get; set; }

        public KronikHastalikController(IKronikHastalikService kronikHastalikService)
        {
            _kronikHastalikService = kronikHastalikService;
        }

        [HttpGet]
        public async Task<KronikHastalik> GetById(int id)
        {
            return await _kronikHastalikService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<KronikHastalik>> Get()
        {
            return await _kronikHastalikService.GetAllAsync();
        }

        [HttpPut]
        public async Task<KronikHastalik> Put(Guid key, [FromBody]KronikHastalik kronikHastalik)
        {
            kronikHastalik.UserId = User.Identity.GetUserId();
            return await _kronikHastalikService.UpdateAsync(kronikHastalik, key);
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPost]
        public async Task<KronikHastalik> Post(KronikHastalik kronikHastalik)
        {
            kronikHastalik.UserId = User.Identity.GetUserId();
            kronikHastalik.Tarih = GuncelTarih();
            kronikHastalik.KronikHastalik_Id = GuidGenerator.GenerateTimeBasedGuid(GuncelTarih());//zaman tabanlı guid
            return await _kronikHastalikService.AddAsync(kronikHastalik);
        }

        [HttpDelete]
        public async Task<int> Delete(Guid id)
        {
            return await _kronikHastalikService.DeleteAsync(id);
        }

    }
}
