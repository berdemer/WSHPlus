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
	[RoutePrefix("api/mail_Onerileri")]
	public class Mail_OnerileriController : ApiController
	{
		private IPersonelService _personelService{ get; set; }

		private IMail_OnerileriService _mail_OnerileriService { get; set; }

		public Mail_OnerileriController(IPersonelService personelService, IMail_OnerileriService mail_OnerileriService)
		{
			_mail_OnerileriService = mail_OnerileriService;
			_personelService = personelService;
		}

        [Route("bul/{stiid}/{blmid}")]
        [HttpGet]
		public async Task<IEnumerable<Mail_Onerileri>> bul(int stiid,int blmid)
		{
			return await _mail_OnerileriService.FindAllBlmAsync(stiid, blmid);
		}

		[Route("oneri/{oneri}/{sirketId}")]
		[HttpGet]
		public async Task<IEnumerable<Mail_Onerileri>> oneri(string oneri,int sirketId)
		{
			return await _mail_OnerileriService.FindAllAsync(new Mail_Onerileri() { OneriTanimi=oneri,Sirket_Id=sirketId});
		}

        [Route("onerim/{oneri}/{sirketId}/{bolumId}")]
        [HttpGet]
        public async Task<IEnumerable<MailListesi>> onerim(string oneri, int sirketId,int bolumId)
        {
            return await _mail_OnerileriService.MailOnerileri(sirketId, bolumId, oneri);
        }

         [HttpPut]
		public async Task<Mail_Onerileri> Put(int key, [FromBody]Mail_Onerileri mail_Onerileri)
		{
			Mail_Onerileri cv = await _mail_OnerileriService.GetAsync(key);
			cv.UserId = User.Identity.GetUserId();
			cv.MailAdiVeSoyadi = mail_Onerileri.MailAdiVeSoyadi;
			cv.MailAdresi = mail_Onerileri.MailAdresi;
			cv.OneriTanimi = mail_Onerileri.OneriTanimi;
			cv.gonderimSekli = mail_Onerileri.gonderimSekli;
			Mail_Onerileri se = new Mail_Onerileri();
			try
			{
				se= await _mail_OnerileriService.UpdateAsync(cv, key);
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
		public async Task<Mail_Onerileri> Post(Mail_Onerileri mail_Onerileri)
		{
			mail_Onerileri.UserId= User.Identity.GetUserId();
			return await _mail_OnerileriService.AddAsync(mail_Onerileri);
		}

		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _mail_OnerileriService.DeleteAsync(id);
		}

	}
}
