using ISG.Business.Abstract;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/ikaz")]
    public class IkazController : ApiController
    {
        private IPersonelService _personelService { get; set; }

        private IIkazService _ikazService { get; set; }

        public IkazController(IPersonelService personelService, IIkazService ikazService)
        {
            _ikazService = ikazService;
            _personelService = personelService;
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
      //  [Route("ikaz/{id}")]
        [HttpGet]
        public async Task<IEnumerable<Ikaz>> GetById(int id)
        {
            return  await _ikazService.FindAllAsync(new Ikaz() { Personel_Id = id, Status = true, SonTarih = DateTime.Now });
        }

        [Route("tumu/{id}")]
        [HttpGet]
        public async Task<IEnumerable<Ikaz>> Tumu(int id)
        {
            return await _ikazService.FindTumuAsync(new Ikaz() { Personel_Id = id });
        }

        [HttpPut]
        public async Task<Ikaz> Put(int key, [FromBody]Ikaz ikaz)
        {
            Ikaz cv = await _ikazService.GetAsync(key);
            cv.IkazText = ikaz.IkazText;
            cv.SonucIkazText = ikaz.SonucIkazText;
            cv.MuayeneTuru = ikaz.MuayeneTuru;
            cv.SonTarih = ikaz.SonTarih;
            cv.Status = ikaz.Status;
            cv.UserId = User.Identity.GetUserId();
            Ikaz se = new Ikaz();
            try
            {
                se = await _ikazService.UpdateAsync(cv, key);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        var propName = err.PropertyName;
                        var errMess = err.ErrorMessage;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return se;
        }

        [HttpPost]
        public async Task<Ikaz> Post(Ikaz ikaz)
        {
            ikaz.UserId = User.Identity.GetUserId();
            return await _ikazService.AddAsync(ikaz);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _ikazService.DeleteAsync(id);
        }
    }
}
