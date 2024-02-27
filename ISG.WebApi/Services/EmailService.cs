using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Threading.Tasks;

namespace ISG.WebApi.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var client = new SendGridClient(ConfigurationManager.AppSettings["emailApiKey"]);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("taiseer@bitoftech.net", "Taiseer Joudeh"));
            msg.SetSubject(message.Subject);
            msg.AddContent(MimeType.Text, message.Body);
            //msg.AddContent(MimeType.Html, message.Body);
            msg.AddTo(message.Destination);
            //msg.SetTemplateId(Guid.NewGuid().ToString());
            //msg.AddSubstitution("-name-", "İSGSPLUS");
            //msg.AddSubstitution("-city-", "Adana");
            await client.SendEmailAsync(msg);
        }
    }
}