using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISG.Entities.ComplexType
{
    public class PersonelView
    {
        public int Personel_Id { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string TcNo { get; set; }
        public string sirketAdi { get; set; }
        public string bolumAdi { get; set; }
        public string KadroDurumu { get; set; }
        public string SicilNo { get; set; }
        public string KanGrubu { get; set; }
        public string SgkNo { get; set; }
        public string Photo { get; set; }
        public string Mail { get; set; }
        public string Telefon { get; set; }
        public string Gorevi { get; set; }
        public string MedeniHali { get; set; }
        public string EgitimSeviyesi { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public DateTime? IlkIseBaslamaTarihi { get; set; }
        public Guid PerGuid { get; set; }
        public string CalismaSekli { get; set; }
    }
}
