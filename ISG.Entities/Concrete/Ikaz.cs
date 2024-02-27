using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public class Ikaz : IEntity
    {
        public int Ikaz_Id { get; set; }
        public string IkazText { get; set; }
        public string SonucIkazText { get; set; }
        public string MuayeneTuru { get; set; }
        public DateTime? SonTarih { get; set; }
        public DateTime? Tarih { get; set; }
        public int Personel_Id { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
