using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Radyoloji : IEntity
    {
        public int Radyoloji_Id { get; set; }
        public string RadyolojikTip { get; set; }
        public string RadyolojikIslem { get; set; }
        public string RadyolojikSonuc { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public string MuayeneTuru { get; set; }
        public int RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public int Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }

    }
}
