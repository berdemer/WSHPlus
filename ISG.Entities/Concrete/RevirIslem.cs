using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class RevirIslem : IEntity
    {
        public RevirIslem()
        {
            ANTlari = new List<ANT>();
            SFTleri = new List<SFT>();      
            Odiolar = new List<Odio>();
            Hemogramlar = new List<Hemogram>();
            Laboratuarlari = new List<Laboratuar>();
            Radyolojileri = new List<Radyoloji>();
            BoyKilolari = new List<BoyKilo>();
            EKGleri = new List<EKG>();
            Gormeleri = new List<Gorme>();
            Pansumanlari = new List<Pansuman>();
            Tetkikleri = new List<Tetkik>();
            RevirTedavileri = new List<RevirTedavi>();
            PersonelMuayeneleri = new List<PersonelMuayene>();
            PeriyodikMuayeneleri = new List<PeriyodikMuayene>();
            IsKazalari = new List<IsKazasi>();
            PsikolojikTestler = new List<PsikolojikTest>();
            //Gorevler = new List<Gorev>();
        }
        public int RevirIslem_Id { get; set; }
        public int SaglikBirimi_Id { get; set; }
        public string IslemTuru { get; set; }//odio,sft,ekg,muayene
        public int? Protokol { get; set; }
        public string IslemDetayi { get; set; }//muayene,iþ kazasý ,revir ,vs
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<ANT> ANTlari { get; set; }
        public virtual ICollection<Odio> Odiolar { get; set; }
        public virtual ICollection<Hemogram> Hemogramlar { get; set; }
        public virtual ICollection<Pansuman> Pansumanlari { get; set; }
        public virtual ICollection<SFT> SFTleri { get; set; }
        public virtual ICollection<Tetkik> Tetkikleri { get; set; }
        public virtual ICollection<Laboratuar> Laboratuarlari { get; set; }
        public virtual ICollection<Radyoloji> Radyolojileri { get; set; }
        public virtual ICollection<BoyKilo> BoyKilolari { get; set; }
        public virtual ICollection<EKG> EKGleri { get; set; }
        public virtual ICollection<Gorme> Gormeleri { get; set; }
        public virtual ICollection<RevirTedavi> RevirTedavileri { get; set; }
        public virtual ICollection<PersonelMuayene> PersonelMuayeneleri { get; set; }
        public virtual ICollection<PeriyodikMuayene> PeriyodikMuayeneleri { get; set; }
        public virtual ICollection<IsKazasi> IsKazalari { get; set; }
        public virtual ICollection<PsikolojikTest> PsikolojikTestler { get; set; }
        //public virtual ICollection<Gorev> Gorevler { get; set; }
    }
   
}
