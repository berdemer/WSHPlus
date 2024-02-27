using System;

namespace ISG.Entities.Concrete
{
    public class IlacStokGirisiView
    {
        public Guid Id { get; set; }
        public Guid StokId { get; set; }
        public string IlacAdi { get; set; }
        public string StokEkBilgisi { get; set; }
        public int KutuIcindekiMiktar { get; set; }
        public int KutuMiktari { get; set; }
        public int ToplamMiktar { get; set; }
        public DateTime MiadTarihi { get; set; }
        public DateTime KritikMiadTarihi { get; set; }
        public int ArtanMiadTelefMiktari { get; set; }
        public string ArtanTelefNedeni { get; set; }
        public DateTime Tarih { get; set; }
        public decimal Maliyet { get; set; }
        public bool Status { get; set; }

        public bool MiadUyarisi { get; set; }

        public bool KritikMiadUyarisi { get; set; }
    }
}