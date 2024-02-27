using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public partial class Ilac : IEntity
    {
        public string IlacBarkodu { get; set; }
        public string  IlacAdi { get; set; }
        public string  AtcKodu { get; set; }
        public string  AtcAdi { get; set; }
        public string FirmaAdi { get; set; }
        public string  ReceteTuru { get; set; }
        public decimal Fiyat { get; set; }
        public int Aski { get; set; }
        public bool SystemStatus { get; set; }
        public bool Status { get; set; }
    }
}
