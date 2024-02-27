using ISG.Business.Abstract;
using System.Threading.Tasks;
using ISG.Entities.ComplexType;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System;


namespace ISG.Business.Concrete.Manager
{
    public class AzureMailManager : IAzureMailService
    {
        public async Task<bool> SendEmailAsync(M_Client cl, M_Message ml)
        {
            //Environment.SetEnvironmentVariable("ISGSPLUS_MAIL_CONNECTION_KEY", "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");
            //var apiKey = Environment.GetEnvironmentVariable("ISGSPLUS_MAIL_CONNECTION_KEY");
            var client = new SendGridClient(cl.apiKey);
            var msg = new SendGridMessage();           
            msg.SetFrom(new EmailAddress(ml.From.Address, ml.From.DisplayName));
            msg.SetSubject(ml.Subject);
            //msg.AddContent(MimeType.Text, "ÝSG Bilgilendirme Formu!");
            msg.AddContent(MimeType.Html, ml.Body.Trim());
            if (ml.To.Count > 0)
            {
                foreach (MailAddress item in ml.To)
                {
                    msg.AddTo(new EmailAddress(item.Address, item.User));
                };
            }
            if (ml.CC.Count > 0)
            {
                foreach (MailAddress item in ml.CC)
                {
                    msg.AddCc(new EmailAddress(item.Address, item.User));
                };
            }
            if (ml.Bcc.Count > 0)
            {
                foreach (MailAddress item in ml.Bcc)
                {
                    msg.AddBcc(new EmailAddress(item.Address, item.User));
                };
            }
            if (ml.Attachment != null)
            {
                foreach (System.Net.Mail.Attachment attach in ml.Attachment)
                {
                    attach.ContentStream.Position = 0;
                    byte[] buffer = new byte[attach.ContentStream.Length];
                    for (int totalBytesCopied = 0; totalBytesCopied < attach.ContentStream.Length;)
                        totalBytesCopied += attach.ContentStream.Read(buffer, totalBytesCopied, Convert.ToInt32(attach.ContentStream.Length) - totalBytesCopied);
                    var file = Convert.ToBase64String(buffer);
                    msg.AddAttachment(attach.Name,file);
                }


                //Azurdan dosya çekilecek ise
                //var getBlob = blobContainer.GetBlobReferenceFromServer(fileUploadLink.Name);

                //if (getBlob != null)
                //{
                //    var memoryStream = new MemoryStream();

                //    getBlob.DownloadToStream(memoryStream);
                //    memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream back to beginning
                //    emailMessage.AddAttachment(memoryStream, fileUploadLink.Name);
                //}
            }
            //msg.SetTemplateId(Guid.NewGuid().ToString());
            //msg.AddSubstitution("-name-", "ÝSGSPLUS");
            //msg.AddSubstitution("-city-", "Adana");
            var response = await client.SendEmailAsync(msg);
            return true;
        }
    }
}
