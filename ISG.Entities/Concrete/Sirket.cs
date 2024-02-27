using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public class Sirket : IEntity
    {
        public Sirket()
        {
            this.SirketBolumleri = new List<SirketBolumu>();
            this.SirketAtamalari = new List<SirketAtama>();
            this.Mail_Onerileri = new List<Mail_Onerileri>();
            this.IsgTopluEgitimleri = new List<IsgTopluEgitimi>();
            this.SirketUploadlari = new List<SirketUpload>();
        }
        public int id { get; set; }

        public int idRef { get; set; }

        public string sirketAdi { get; set; }

        public bool status { get; set; }

        public virtual ICollection<SirketBolumu> SirketBolumleri { get; set; }//bir şirkete birden fazla bölüm yapılabilir(list***)
        public virtual ICollection<SirketAtama> SirketAtamalari { get; set; }//bir şirkete birden fazla atama yapılabilir
        public virtual ICollection<Mail_Onerileri> Mail_Onerileri { get; set; }
        public virtual SirketDetayi SirketDetayi{ get; set; }//one-to-one tekil **
        public virtual ICollection<Personel> Personeller { get; set; }
        public virtual ICollection<IsgTopluEgitimi> IsgTopluEgitimleri { get; set; }
        public virtual ICollection<SirketUpload> SirketUploadlari { get; set; }
    }
}
