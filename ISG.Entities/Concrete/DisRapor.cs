using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class DisRapor : IEntity
    {
        public int DisRapor_Id { get; set; }
        public string MuayeneTuru { get; set; }
        public string Tani { get; set; }
        public Nullable<System.DateTime> BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public Nullable<int> SureGun { get; set; }
        public string DoktorAdi { get; set; }
        public string RaporuVerenSaglikBirimi { get; set; }
        public string User_Id { get; set; }
        public Nullable<int> Personel_Id { get; set; }
        public Nullable<int> Revir_Id { get; set; }
        public Nullable<int> Sirket_Id { get; set; }
        public Nullable<int> Bolum_Id { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
