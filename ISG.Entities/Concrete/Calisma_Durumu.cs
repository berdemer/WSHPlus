using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Calisma_Durumu : IEntity
    {
        public int Calisma_Durumu_Id { get; set; }
        public string Sirket { get; set; }
        public Nullable<int> Sirket_Id { get; set; }
        public string Bolum { get; set; }
        public Nullable<int> Bolum_Id { get; set; }
        public Nullable<System.DateTime> Baslama_Tarihi { get; set; }
        public Nullable<System.DateTime> Bitis_Tarihi { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Aciklama { get; set; }
        public int Personel_Id { get; set; }
        public string Calisma_Duzeni { get; set; }
        public string KadroDurumu { get; set; }
        public string Gorevi { get; set; }
        public string SicilNo { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
