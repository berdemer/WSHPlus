using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class ANT : IEntity
    {
        public int ANT_Id { get; set; }
        public string TASagKolSistol { get; set; }
        public string TASagKolDiastol { get; set; }
        public string TASolKolSistol { get; set; }
        public string TASolKolDiastol { get; set; }
        public int? Nabiz { get; set; }
        public string Ates { get; set; }
        public string NabizRitmi { get; set; }
        public string Sonuc { get; set; }
        public string MuayeneTuru { get; set; }
        public int? RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public int Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }
    }
}