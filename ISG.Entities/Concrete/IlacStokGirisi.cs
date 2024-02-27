using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class IlacStokGirisi : IEntity
    {
        public Guid Id { get; set; }
        public Guid StokId { get; set; }
        public string StokEkBilgisi { get; set; }
        public int SaglikBirimi_Id { get; set; }
        public int KutuIcindekiMiktar { get; set; }
        public int KutuMiktari { get; set; }
        public int ToplamMiktar { get; set; }
        public DateTime? MiadTarihi { get; set; }
        public DateTime? KritikMiadTarihi { get; set; }
        public int ArtanMiadTelefMiktari { get; set; }
        public string ArtanTelefNedeni { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }

        public decimal Maliyet { get; set; }

        public virtual IlacStok IlacStoklari { get; set; }
    }
}
