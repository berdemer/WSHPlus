using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Kkd : IEntity
    {
        public int Kkd_Id { get; set; }
        public string Kkd_Tanimi { get; set; }
        public Nullable<System.DateTime> Alinma_Tarihi { get; set; }
        public string Maruziyet { get; set; }
        public Nullable<int> Guncelleme_Suresi_Ay { get; set; }
        public string Aciklama { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
