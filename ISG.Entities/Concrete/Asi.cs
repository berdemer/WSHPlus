using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Asi : IEntity
    {
        public int Asi_Id { get; set; }
        public string Asi_Tanimi { get; set; }
        public Nullable<System.DateTime> Yapilma_Tarihi { get; set; }
        public string Dozu { get; set; }
        public Nullable<int> Guncelleme_Suresi_Ay { get; set; }
        public DateTime? Muhtamel_Tarih { get; set; }
        public string Aciklama { get; set; }
        public bool StokHarcamasi { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
