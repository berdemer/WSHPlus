using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/isgEgitimi")]
    public class IsgEgitimiController : ApiController
    {

        private IIsgEgitimiService _isgEgitimiService { get; set; }
        private IRevirIslemService _revirIslemService { get; set; }
        public IsgEgitimiController(IIsgEgitimiService isgEgitimiService,
            IRevirIslemService revirIslemService)
        {
            _isgEgitimiService = isgEgitimiService;
            _revirIslemService = revirIslemService;
        }

        [HttpGet]
        public async Task<IsgEgitimi> GetById(int id)
        {
            return await _isgEgitimiService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<IsgEgitimi>> Get()
        {
            return await _isgEgitimiService.GetAllAsync();
        }

        [HttpPut]
        public async Task<IsgEgitimi> Put(int key, [FromBody] JObject jIsgEgitimi)
        {
            dynamic json = jIsgEgitimi;
            IsgEgitimi isgEgitimi = await _isgEgitimiService.FindAsync(new IsgEgitimi() { Egitim_Id = key });
            isgEgitimi.Egitim_Turu = Convert.ToString(json.Egitim_Turu);
            isgEgitimi.Tanimi = Convert.ToString(json.Tanimi);
            isgEgitimi.Egitim_Suresi = IsPropertyExist(json, "Egitim_Suresi") ? Cast(json.Egitim_Suresi, typeof(int)) : 0;
            isgEgitimi.VerildigiTarih = IsPropertyExist(json, "VerildigiTarih") ? CastDate(json.VerildigiTarih) : default(DateTime?);
            isgEgitimi.IsgEgitimiVerenPersonel = Convert.ToString(json.IsgEgitimiVerenPersonel);
			isgEgitimi.UserId = User.Identity.GetUserId();
            return await _isgEgitimiService.UpdateAsync(isgEgitimi, key);
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [HttpPost]
        public async Task<IsgEgitimi> Post(JObject jIsgEgitimi)
        {
            dynamic json = jIsgEgitimi;
            IsgEgitimi isgEgitimiResult = new IsgEgitimi();
            IsgEgitimi isgEgitimi = new IsgEgitimi()
            {
                Egitim_Turu = Convert.ToString(json.Egitim_Turu),
                Tanimi = Convert.ToString(json.Tanimi),
                Egitim_Suresi = IsPropertyExist(json, "Egitim_Suresi") ? Cast(json.Egitim_Suresi, typeof(int)) : 0,
                VerildigiTarih = IsPropertyExist(json, "VerildigiTarih") ? CastDate(json.VerildigiTarih) : default(DateTime?),
                IsgEgitimiVerenPersonel = Convert.ToString(json.IsgEgitimiVerenPersonel),
                Personel_Id = IsPropertyExist(json, "Personel_Id") ? Cast(json.Personel_Id, typeof(int)) : 0,
            };
            isgEgitimi.UserId = User.Identity.GetUserId();
            isgEgitimi.Tarih = GuncelTarih();
            try
            {
                isgEgitimiResult = await _isgEgitimiService.AddAsync(isgEgitimi);
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
            return isgEgitimiResult;

        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            //await _revirIslemService.DeleteAsync((int)aNT.RevirIslem_Id);
            return await _isgEgitimiService.DeleteAsync(id);
        }

        public static dynamic Cast(dynamic obj, Type castTo)
        {
            try
            {
                return Convert.ChangeType(obj, castTo);
            }
            catch (FormatException)
            {
                return castTo == typeof(int) ? 0 : 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsPropertyExist(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }
        }

        public static Object CastDate(dynamic obj)
        {
            try
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(Cast(obj, typeof(string)), CultureInfo.GetCultureInfo("tr-TR"), DateTimeStyles.None, out dt))
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
