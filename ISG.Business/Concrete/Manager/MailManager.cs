using ISG.Business.Abstract;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System;
using ISG.Entities.ComplexType;
using System.Net.Mime;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;

namespace ISG.Business.Concrete.Manager
{
    public class MailManager : IMailService
    {
        public async Task<bool> SendEmailAsync(M_Client cl, M_Message ml)
        {
            if (cl.mailSekli.Trim() == "Exchange".Trim())
            {
                try
                {
                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1)
                    {
                        // Set user login credentials  
                        Credentials = new WebCredentials(cl.name.Trim(), cl.Password.Trim())
                    };
                    string serviceUrl = cl.Host.Trim();//"https://outlook.office365.com/ews/exchange.asmx";
                    service.Url = new Uri(serviceUrl);//https://subdomainfinder.c99.nl/scans/2020-11-18/outlook.com   farklý sub domainler altýnda çalýþýr.

                    EmailMessage message = new EmailMessage(service);
                    message.From = new EmailAddress(ml.From.DisplayName, ml.From.Address); 
                    if (ml.To.Count > 0)
                    {
                        foreach (MailAddress item in ml.To)
                        {
                            message.ToRecipients.Add(item.DisplayName, item.Address);
                        };
                    }

                    if (ml.CC.Count > 0)
                    {
                        foreach (MailAddress item in ml.CC)
                        {
                            message.CcRecipients.Add(item.DisplayName, item.Address);
                        };
                    }
                    if (ml.Bcc.Count > 0)
                    {
                        foreach (MailAddress item in ml.Bcc)
                        {
                            message.BccRecipients.Add(item.DisplayName, item.Address);
                        };
                    }
                    message.Subject = ml.Subject;
                    message.Body = new MessageBody(BodyType.HTML, ml.Body.Trim());
                    message.SendAndSaveCopy();


                    return true;
                }
                catch (AutodiscoverRemoteException ex)
                {
                    return false;
                }
                catch (ServiceRequestException ex)
                {
                    return false;
                }
                catch (WebException ex)
                {
                    return false; ;
                }
            }
            else
            {
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Port = cl.Port;
                        client.Host = cl.Host.Trim();
                        client.EnableSsl = cl.EnableSsl;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = cl.UseDefaultCredentials;
                        
                        client.Timeout = (60 * 5 * 1000);
                       // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                        client.Credentials = new NetworkCredential(cl.name.Trim(), cl.Password.Trim());

                        //client.SendCompleted += new SendCompletedEventHandler(MailDeliveryComplete);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        MailMessage message = new MailMessage();
                        message.From = ml.From;
                        //message.Sender = new MailAddress("");
                        if (ml.To.Count > 0)
                        {
                            foreach (MailAddress item in ml.To)
                            {
                                message.To.Add(item);
                            };
                        }

                        if (ml.CC.Count > 0)
                        {
                            foreach (MailAddress item in ml.CC)
                            {
                                message.CC.Add(item);
                            };
                        }
                        if (ml.Bcc.Count > 0)
                        {
                            foreach (MailAddress item in ml.Bcc)
                            {
                                message.Bcc.Add(item);
                            };
                        }
                        message.Subject = ml.Subject;
                        message.IsBodyHtml = true;
                        //if (ml.AlternateViews != null)
                        //{//html mail sayfasna resim gömmek için yazldý.
                        //    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(ml.Body, new System.Net.Mime.ContentType("text/html"));
                        //    foreach (embededList item in ml.AlternateViews)
                        //    {
                        //        LinkedResource inLine = new LinkedResource(item.path);
                        //        inLine.ContentId = item.cidName;
                        //        htmlView.LinkedResources.Add(inLine);
                        //    }
                        //    message.AlternateViews.Add(htmlView);
                        //    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("ISG Bilgilendirme Formu", new System.Net.Mime.ContentType("text/plain")));
                        //}
                        //else
                        //{//spam engelemek için yazldý.
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(ml.Body.Trim(), new ContentType(MediaTypeNames.Text.Html));
                        htmlView.ContentType.CharSet = Encoding.UTF8.WebName;//türkçe karakter sorunu için eklendi.

                        message.AlternateViews.Add(htmlView);
                        //message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("ISG Bilgilendirme Formu", new System.Net.Mime.ContentType("text/plain")));
                        //}

                        message.Priority = MailPriority.Normal;
                        message.BodyEncoding = Encoding.UTF8;
                        message.SubjectEncoding = Encoding.UTF8;
                        if (ml.Attachment != null)
                        {
                            foreach (System.Net.Mail.Attachment attach in ml.Attachment)
                            {
                                message.Attachments.Add(attach);
                            }
                        }
                        await client.SendMailAsync(message);
                        message.Dispose();
                        return true;
                    }
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    //for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    //{
                    //    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    //    if (status == SmtpStatusCode.MailboxBusy ||
                    //        status == SmtpStatusCode.MailboxUnavailable)
                    //    {
                    //        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                    //        System.Threading.Thread.Sleep(5000);

                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Failed to deliver message to {0}",
                    //            ex.InnerExceptions[i].FailedRecipient);
                    //    }
                    //}
                    return false;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }


        private static void MailDeliveryComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //handle error
            }
            else if (e.Cancelled)
            {
                //handle cancelled
            }
            else
            {
                //handle sent email
                MailMessage message = (MailMessage)e.UserState;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            var redirectionUri = new Uri(redirectionUrl);
            var result = redirectionUri.Scheme == "https";
            return result;
        }

    }
}
//var service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
////service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
//service.Credentials =
//    new WebCredentials("bulent_erdem", "zaq1xsw2", "bossa");
//service.TraceEnabled = true;
//service.TraceFlags = TraceFlags.All;
//service.AutodiscoverUrl("berdem@bossa.com.tr",
//    RedirectionUrlValidationCallback);
//EmailMessage email = new EmailMessage(service);
//email.ToRecipients.Add("drbulenterdem@hotmail.com");
//email.CcRecipients.Add(new EmailAddress("bulent", "bulenterdem@gmail.com"));
//email.Subject = ml.Subject;
//email.Body = new MessageBody(BodyType.HTML, ml.Body);
//email.Send();