using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.ComplexType
{
    public class M_Message
    {
        public MailAddress From { get; set; }
        public IList<MailAddress> To { get; set; }
        public IList<MailAddress> CC { get; set; }
        public IList<MailAddress> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public IList<Attachment> Attachment { get; set; }
        public IList<embededList> AlternateViews { get; set; }
    }
}