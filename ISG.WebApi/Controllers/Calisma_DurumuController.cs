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
	[RoutePrefix("api/calisma_Durumu")]
	public class Calisma_DurumuController : ApiController
	{

		private ICalisma_DurumuService _calisma_DurumuService { get; set; }

		public Calisma_DurumuController(ICalisma_DurumuService calisma_DurumuService)
		{
			_calisma_DurumuService = calisma_DurumuService;
		}

		[HttpGet]
		public async Task<Calisma_Durumu> GetById(int id)
		{
			return await _calisma_DurumuService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Calisma_Durumu>> Get()
		{
			return await _calisma_DurumuService.GetAllAsync();
		}

		[HttpPut]
		public async Task<Calisma_Durumu> Put(int key, [FromBody]Calisma_Durumu calisma_Durumu)
		{
            calisma_Durumu.UserId = User.Identity.GetUserId();
            return await _calisma_DurumuService.UpdateAsync(calisma_Durumu,key);
		}

		[HttpPost]
		public async Task<Calisma_Durumu> Post(Calisma_Durumu calisma_Durumu)
		{
            Calisma_Durumu cd = new Calisma_Durumu();
            try
            {
                calisma_Durumu.UserId= User.Identity.GetUserId();
                cd = await _calisma_DurumuService.AddAsync(calisma_Durumu);
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
            return cd;
        }

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _calisma_DurumuService.DeleteAsync(id);
		}

	}
}
