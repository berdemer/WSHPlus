using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class BolumRiski: IEntity
    {
        public int BolumRiski_Id { get; set; }
        public string PMJson { get; set; }
        public int Sirket_Id { get; set; }
        public int Bolum_Id { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
    }
}
