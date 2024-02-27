using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Aliskanlik : IEntity
    {
        public int Aliskanlik_Id { get; set; }
        public string Madde { get; set; }
        public DateTime BaslamaTarihi { get; set; }
        public Nullable<DateTime> BitisTarihi { get; set; }
        public string SiklikDurumu { get; set; }
        public string Aciklama { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
