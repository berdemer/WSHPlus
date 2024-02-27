using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public class SaglikBirimi:IEntity
    {
        public SaglikBirimi()
        {
            IlacSarfCikislari = new List<IlacSarfCikisi>();
        }
        public int SaglikBirimi_Id { get; set; }
        public string Adi { get; set; }
        public int StiId { get; set; }
        public int Protokol { get; set; }
        public int Year { get; set; }
        public bool Status { get; set; }
        public int MailPort { get; set; }
        public string MailHost { get; set; }
        public string MailUserName { get; set; }
        public string MailPassword { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string mailfromAddress { get; set; }
        public string domain { get; set; }
        public string mailSekli { get; set; }
        public virtual ICollection<IlacSarfCikisi> IlacSarfCikislari { get; set; }
    }
}
