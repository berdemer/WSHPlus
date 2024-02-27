using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class IcRapor : IEntity
    {
        public int IcRapor_Id { get; set; }
        public string MuayeneTuru { get; set; }
        public string Tani { get; set; }
        public string RaporTuru { get; set; }
        public Nullable<System.DateTime> BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public Nullable<int> SureGun { get; set; }
        public string Doktor_Adi { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string UserId { get; set; }
        public int Personel_Id { get; set; }
        public Nullable<int> Revir_Id { get; set; }
        public Nullable<int> Sirket_Id { get; set; }
        public Nullable<int> Bolum_Id { get; set; }
        public int? RevirIslem_Id { get; set; }
        public int? Protokol { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
