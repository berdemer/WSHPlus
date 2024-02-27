using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize]
	[RoutePrefix("api/mail")]
	public class MailController : BaseApiController
	{
		private IMailService _mailService { get; set; }
		private IAzureMailService _azureMailService { get; set; }
		private ISaglikBirimiService _saglikBirimiService { get; set; }
		public MailController(IMailService mailService, 
			IAzureMailService azureMailService,
			ISaglikBirimiService saglikBirimiService)
		{
			_mailService = mailService;
			_azureMailService = azureMailService;
			_saglikBirimiService = saglikBirimiService;
		}

		
		[HttpPost]
		public async Task<IHttpActionResult> Post(JObject jsonData)
		{
            try
            {
                dynamic Islem = jsonData;
                SaglikBirimi sb = await _saglikBirimiService.GetAsync((int)Islem.SagId);
                string fullPath = HttpContext.Current.Server.MapPath("~/uploads");
                M_Client mc = new M_Client()
                {
                    EnableSsl = sb.EnableSsl,
                    Host = sb.MailHost.Trim(),
                    name = sb.MailUserName,
                    Password = sb.MailPassword,
                    Port = sb.MailPort,
                    UseDefaultCredentials = sb.UseDefaultCredentials,
                    displayName = sb.Adi,
                    domain = sb.domain,
                    mailAdress = sb.mailfromAddress,
                    mailSekli = sb.mailSekli,
                    apiKey = ConfigurationManager.AppSettings["emailApiKey"]
                };
                List<MailAddress> to = new List<MailAddress>();
                if (((JArray)Islem["To"]).Count > 0)
                {
                    foreach (dynamic i in (JArray)Islem["To"])
                    {
                        to.Add(new MailAddress(((string)i.address).Trim(), ((string)i.displayName).Trim()));
                    }
                }
                List<MailAddress> cc = new List<MailAddress>();
                if (((JArray)Islem["CC"]).Count > 0)
                {
                    foreach (dynamic i in (JArray)Islem["CC"])
                    {
                        cc.Add(new MailAddress(((string)i.address).Trim(), ((string)i.displayName).Trim()));
                    }
                }
                List<MailAddress> bcc = new List<MailAddress>();
                if (((JArray)Islem["Bcc"]).Count > 0)
                {
                    foreach (dynamic i in (JArray)Islem["Bcc"])
                    {
                        bcc.Add(new MailAddress(((string)i.address).Trim(), ((string)i.displayName).Trim()));
                    }
                }
                List<embededList> AlternateViews = new List<embededList>();
                if (((JArray)Islem["Resimler"]).Count > 0)
                {
                    foreach (dynamic i in (JArray)Islem["Resimler"])
                    {
                        AlternateViews.Add(new embededList() { path = fullPath + "/" + (string)i.path, cidName = (string)i.cidName });
                    }
                }
                //ApplicationUser user = await AppUserManager.FindByIdAsync(User.Identity.GetUserId());
                MailAddress from = new MailAddress(mc.mailAdress.Trim(), mc.displayName.Trim());
                M_Message mm = new M_Message() { IsBodyHtml = true, Subject = Islem.Subject, Body = Islem.Body, To = to, From = from, Bcc = bcc, CC = cc };
                return Ok(ConfigurationManager.AppSettings["isAzureMailService"] == "true" ? await _azureMailService.SendEmailAsync(mc, mm) : await _mailService.SendEmailAsync(mc, mm));
            }
            catch (System.Exception)
            {

                throw;
            }


		}
	}
}
