using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class PeriyodikMuayene : IEntity
    {
        public int PeriyodikMuayene_Id { get; set; }
        public string PMJson { get; set; }
        public string MuayeneTuru { get; set; }
        public int? RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public int? Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }

    }
}
