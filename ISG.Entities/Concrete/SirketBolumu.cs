using ISG.Entities.Abstract;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class SirketBolumu : IEntity
    {
        public SirketBolumu()
        {
            this.Mail_Onerileri = new List<Mail_Onerileri>();
        }


        public int id { get; set; }
        public int idRef { get; set; }
        public string bolumAdi { get; set; }

        public int? Sirket_id { get; set; }

        public bool status { get; set; }
       
        public virtual Sirket Sirket { get; set; }
        public virtual ICollection<Personel> Personeller { get; set; }
        public virtual ICollection<Mail_Onerileri> Mail_Onerileri { get; set; }
    }
}
