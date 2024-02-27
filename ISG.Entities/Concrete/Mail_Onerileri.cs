using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public class Mail_Onerileri : IEntity
    {
        public int Mail_Onerileri_Id { get; set; }

        public int Sirket_Id { get; set; }
        public int Bolum_Id { get; set; }
        public string MailAdresi { get; set; }
        public string MailAdiVeSoyadi { get; set; }
        public string OneriTanimi { get; set; }
        public bool TumSirketteOneriListesinde { get; set; }
        public string gonderimSekli{ get; set; }
        public string UserId { get; set; }
        public virtual Sirket Sirket { get; set; }
        public virtual SirketBolumu SirketBolumu { get; set; }
    }
}
