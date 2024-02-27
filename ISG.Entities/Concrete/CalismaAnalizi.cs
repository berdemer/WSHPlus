using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class CalismaAnalizi: IEntity
    {
        public int CalismaAnalizi_Id { get; set; }
        public string CAJson { get; set; }
        public int Sirket_Id { get; set; }
        public int Bolum_Id { get; set; }
        public string MeslekAdi { get; set; }
        public string Meslek_Kodu { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
    }
}
