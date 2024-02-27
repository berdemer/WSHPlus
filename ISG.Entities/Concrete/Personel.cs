using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Personel : IEntity
    {
        
        public Personel()
        {
            Adresler = new List<Adres>();
            Aliskanliklar = new List<Aliskanlik>();
            Allerjiler = new List<Allerji>();
            Asilar = new List<Asi>();
            Calisma_Durumu = new List<Calisma_Durumu>();
            Calisma_Gecmisi = new List<Calisma_Gecmisi>();
            EgitimHayatlari = new List<EgitimHayati>();
            IsgEgitimleri = new List<IsgEgitimi>();
            KkdLeri = new List<Kkd>();
            Ozurlulukler = new List<Ozurluluk>();
            SoyGecmisleri = new List<SoyGecmisi>();
            DisRaporlari = new List<DisRapor>();
            IcRaporlari = new List<IcRapor>();
            KronikHastaliklar = new List<KronikHastalik>();
            SFTleri = new List<SFT>();
            Odiolar = new List<Odio>();
            Laboratuarlari = new List<Laboratuar>();
            Hemogramlar = new List<Hemogram>();
            Radyolojileri = new List<Radyoloji>();
            ANTlari = new List<ANT>();
            BoyKilolari = new List<BoyKilo>();
            EKGleri = new List<EKG>();
            Gormeleri = new List<Gorme>();
            Pansumanlari = new List<Pansuman>();
            RevirTedavileri = new List<RevirTedavi>();
            PersonelMuayeneleri = new List<PersonelMuayene>();
            PeriyodikMuayeneleri = new List<PeriyodikMuayene>();
            IsKazalari = new List<IsKazasi>();
            PsikolojikTestler = new List<PsikolojikTest>();
            Ikazlar = new List<Ikaz>();
            //Gorevler = new List<Gorev>();
        }

        public int Personel_Id { get; set; }
        public Guid PerGuid { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string TcNo { get; set; }
        public int? Sirket_Id { get; set; }
        public int? Bolum_Id { get; set; }
        public string KadroDurumu { get; set; }
        public string SicilNo { get; set; }
        public string Gorevi { get; set; }
        public string KanGrubu { get; set; }
        public string SgkNo { get; set; }
        public string Photo { get; set; }
        public string Mail { get; set; }
        public string Telefon { get; set; }
        public bool Status { get; set; }
        public byte[] rowVersion { get; set; }
        public string UserId { get; set; }


        public virtual PersonelDetayi PersonelDetayi { get; set; }//one-to-one tekil **
        //listeleri baðlamak için eklendi.
        public virtual ICollection<Adres> Adresler { get; set; }
        public virtual ICollection<Aliskanlik> Aliskanliklar { get; set; }
        public virtual ICollection<Allerji> Allerjiler { get; set; }
        public virtual ICollection<Asi> Asilar { get; set; }
        public virtual ICollection<Calisma_Durumu> Calisma_Durumu { get; set; }
        public virtual ICollection<Calisma_Gecmisi> Calisma_Gecmisi { get; set; }
        public virtual ICollection<EgitimHayati> EgitimHayatlari { get; set; }
        public virtual ICollection<IsgEgitimi> IsgEgitimleri { get; set; }
        public virtual ICollection<Kkd> KkdLeri { get; set; }
        public virtual ICollection<Ozurluluk> Ozurlulukler { get; set; }
        public virtual ICollection<SoyGecmisi> SoyGecmisleri { get; set; }
        public virtual ICollection<DisRapor> DisRaporlari { get; set; }
        public virtual ICollection<IcRapor> IcRaporlari { get; set; }
        public virtual ICollection<KronikHastalik> KronikHastaliklar { get; set; }
        public virtual ICollection<SFT> SFTleri { get; set; }
        public virtual ICollection<Odio> Odiolar { get; set; }
        public virtual ICollection<Laboratuar> Laboratuarlari { get; set; }
        public virtual ICollection<Hemogram> Hemogramlar { get; set; }
        public virtual ICollection<Radyoloji> Radyolojileri { get; set; }
        public virtual ICollection<ANT> ANTlari { get; set; }
        public virtual ICollection<BoyKilo> BoyKilolari { get; set; }
        public virtual ICollection<EKG> EKGleri { get; set; }
        public virtual ICollection<Gorme> Gormeleri { get; set; }
        public virtual ICollection<Pansuman> Pansumanlari { get; set; }
        public virtual ICollection<RevirTedavi> RevirTedavileri { get; set; }
        public virtual ICollection<PersonelMuayene> PersonelMuayeneleri { get; set; }
        public virtual ICollection<PeriyodikMuayene> PeriyodikMuayeneleri { get; set; }
        public virtual ICollection<IsKazasi> IsKazalari { get; set; }
        public virtual ICollection<PsikolojikTest> PsikolojikTestler { get; set; }
        public virtual ICollection<Ikaz> Ikazlar { get; set; }
        //public virtual ICollection<Gorev> Gorevler { get; set; }
        //þirket ve bölüm adlarýný almak için eklendi.
        public virtual SirketBolumu SirketBolumleri { get; set; }
        public virtual Sirket Sirketler { get; set; }
        
    }
}
