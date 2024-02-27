using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class IsgTopluEgitimi : IEntity
    {
       
        public IsgTopluEgitimi()
        {
            IsgEgitimleri = new List<IsgEgitimi>();

        }
        public int IsgTopluEgitimi_Id { get; set; }
        public string IsgTopluEgitimiJson { get; set; }
        public string nitelikDurumu { get; set; }//Planlama,temel kayıt,Onayda,onaylanmış
        public int belgeTipi { get; set; }
        public long isgProfTckNo { get; set; }
        public string egitimObjects { get; set; }
        public string calisanObjects { get; set; }
        public DateTime? egitimTarihi { get; set; }
        public int egitimYer { get; set; }
        public int egitimtur { get; set; }
        public string sgkTescilNo { get; set; }
        public string egiticiTckNo { get; set; }
        public string imzalıDeger { get; set; }
        public string firmaKodu { get; set; }
        public string sorguNo { get; set; }//servisteki numara
        public int status { get; set; }//1 planlama 2
        public int Sirket_id { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual Sirket Sirket { get; set; }
        public virtual ICollection<IsgEgitimi> IsgEgitimleri { get; set; }
    }
}
