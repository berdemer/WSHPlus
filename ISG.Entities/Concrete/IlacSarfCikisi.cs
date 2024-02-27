using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class IlacSarfCikisi : IEntity
    {
        public Guid IlacSarfCikisi_Id { get; set; }
        public string IlacAdi { get; set; }
        public int SarfMiktari { get; set; }
        public DateTime? Tarih { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public int RevirTedavi_Id { get; set; }
        public Guid StokId { get; set; }
        public int SaglikBirimi_Id { get; set; }
        public virtual IlacStok IlacStoklari { get; set; }
        public virtual  RevirTedavi RevirTedavileri { get; set; }
        public virtual SaglikBirimi SaglikBirimleri { get; set; }
    }
}
