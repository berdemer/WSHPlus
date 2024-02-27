using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Calisma_Gecmisi : IEntity
    {
        public int Calisma_Gecmisi_Id { get; set; }
        public string Calistigi_Yer_Adi { get; set; }
        public Nullable<System.DateTime> Ise_Baslama_Tarihi { get; set; }
        public Nullable<System.DateTime> Isden_Cikis_Tarihi { get; set; }
        public string Gorevi { get; set; }
        public string Unvani { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }

    }
}
