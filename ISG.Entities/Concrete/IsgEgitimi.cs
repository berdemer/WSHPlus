using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class IsgEgitimi : IEntity
    {
        public DateTime? Tarih { get; set; }
        public long Egitim_Id { get; set; }
        public string Egitim_Turu { get; set; }
        public int DersTipi { get; set; }//genel 1 saglýk 2 teknik 3 diger 0
        public string Tanimi { get; set; }
        public int? Egitim_Suresi { get; set; }
        public DateTime? VerildigiTarih { get; set; }
        public string IsgEgitimiVerenPersonel { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public int IsgTopluEgitimi_Id { get; set; }
        public virtual Personel Personel { get; set; }
        public virtual IsgTopluEgitimi IsgTopluEgitimi { get; set; }
    }
}
