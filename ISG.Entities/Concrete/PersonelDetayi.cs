using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class PersonelDetayi : IEntity
    {
        public int PersonelDetay_Id { get; set; }
        public Nullable<System.DateTime> DogumTarihi { get; set; }
        public string DogumYeri { get; set; }
        public bool Cinsiyet { get; set; }
        public string Uyruk { get; set; }
        public string EgitimSeviyesi { get; set; }
        public string AskerlikDurumu { get; set; }
        public Nullable<System.DateTime> IlkIseBaslamaTarihi { get; set; }
        public string MedeniHali { get; set; }
        public Nullable<int> CocukSayisi { get; set; }
        public string anne_adi { get; set; }
        public string baba_adi { get; set; }
        public Nullable<bool> anne_sag { get; set; }
        public string anne_sag_bilgisi { get; set; }
        public Nullable<bool> baba_sag { get; set; }
        public string baba_sag_bilgisi { get; set; }
        public Nullable<int> KardesSayisi { get; set; }
        public string Kardes_Sag_Bilgisi { get; set; }
        public byte[] rowVersion { get; set; }
        public string UserId { get; set; }

        public virtual Personel Personel{ get; set; }//one-to-one tekil **
    }
}
