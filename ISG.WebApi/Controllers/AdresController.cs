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
	[RoutePrefix("api/adres")]
	public class AdresController : ApiController
	{
		private IAdresService _adresService { get; set; }

		public AdresController( IAdresService adresService)
		{
			_adresService = adresService;
		}

		[Route("adres/{id}")]
		[HttpGet]
		public async Task<Adres> GetById(int id)
		{
			return await _adresService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<Adres>> Get()
		{
			return await _adresService.GetAllAsync();
		}

		[HttpPut]
		public async Task<Adres> Put(int key, [FromBody]Adres adres)
		{
			Adres cv = await _adresService.GetAsync(key);
			cv.GenelAdresBilgisi = adres.GenelAdresBilgisi;
			cv.EkAdresBilgisi = adres.EkAdresBilgisi;
			cv.MapLokasyonu = adres.MapLokasyonu;
			cv.Adres_Turu = adres.Adres_Turu;
			cv.UserId = User.Identity.GetUserId();
			Adres se = new Adres();
			try
			{
				se= await _adresService.UpdateAsync(cv, key);
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
		public async Task<Adres> Post(Adres adres)
		{
			adres.UserId= User.Identity.GetUserId();
			return await _adresService.AddAsync(adres);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _adresService.DeleteAsync(id);
		}

	}
}
