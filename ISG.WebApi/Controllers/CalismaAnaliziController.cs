using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/calismaanalizi")]
    public class CalismaAnaliziController : ApiController
    {
        private ICalismaAnaliziService _calismaAnaliziService { get; set; }

        public CalismaAnaliziController(ICalismaAnaliziService calismaAnaliziService)
        {
            _calismaAnaliziService = calismaAnaliziService;
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<CalismaAnalizi> GetById(int id)
        {
            return await _calismaAnaliziService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<CalismaAnalizi>> Get()
        {
            return await _calismaAnaliziService.GetAllAsync();
        }

        [Route("bolumAra/{blmId}/{stiId}")]
        [HttpGet]
        public async Task<CalismaAnalizi> bolumAra(int blmId, int stiId)
        {
            return await _calismaAnaliziService.BolumAraAsync(blmId, stiId);
        }
        /// <summary>
        /// http://localhost:1943/api/calismaanalizi/meslekAra/9213.05/
        /// Decimal string sorunu nedeni ile
        /// convertor , atýyor. 
        /// link "/" bitmeli.
        /// </summary>
        /// <param name="Meslek_Kodu"></param>
        /// <returns></returns>
        [Route("meslekAra/{Meslek_Kodu:decimal}")]
        [HttpGet]
        public async Task<CalismaAnalizi> meslekAra(decimal Meslek_Kodu)
        {
            string mskKd = (Meslek_Kodu).ToString("0.00").Replace(',', '.');
            
            return await _calismaAnaliziService.MeslekAraAsync(mskKd);
        }

        [HttpPut]
        public async Task<CalismaAnalizi> Put(int key, [FromBody] CalismaAnalizi analiz)
        {
            CalismaAnalizi cv = await _calismaAnaliziService.GetAsync(key);
            cv.Bolum_Id = analiz.Bolum_Id;
            cv.CAJson = analiz.CAJson;
            cv.MeslekAdi = analiz.MeslekAdi;
            cv.Meslek_Kodu = analiz.Meslek_Kodu;
            cv.Tarih = GuncelTarih().Date;
            cv.UserId = User.Identity.GetUserId();
            CalismaAnalizi se = new CalismaAnalizi();
            try
            {
                se = await _calismaAnaliziService.UpdateAsync(cv, key);
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
        public async Task<CalismaAnalizi> CalismaAnalizi(CalismaAnalizi analiz)
        {
            analiz.UserId = User.Identity.GetUserId();
            analiz.Tarih = GuncelTarih().Date;
            return await _calismaAnaliziService.AddAsync(analiz);
        }

        [HttpDelete]
        public async Task<int> Delete(int id)
        {
            return await _calismaAnaliziService.DeleteAsync(id);
        }


        [Route("MeslekListesi")]
        [HttpGet]
        public async Task<IHttpActionResult> MeslekListesi()
        {
            return Ok(await _calismaAnaliziService.MeslekListesi());
        }

    }
}
