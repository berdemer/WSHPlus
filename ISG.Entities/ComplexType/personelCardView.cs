using System;

namespace ISG.Entities.ComplexType
{
    public class personelCardView
    {
        public int Personel_Id { get; set; }
        public string AdiSoyadi { get; set; }
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
        public string CalismaSekli { get; set; }
        public bool Ikaz { get; set; }
        public Guid PerGuid { get; set; }
    }
}
