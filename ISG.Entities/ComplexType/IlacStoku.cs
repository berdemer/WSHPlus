using System;

namespace ISG.Entities.ComplexType
{
    public class IlacStoku
    {
        public string IlacAdi { get; set; }
        public int KritikStokMiktari { get; set; }
        public int SaglikBirimi_Id { get; set; }
        public bool Status { get; set; }
        public Guid StokId { get; set; }
        public string StokMiktarBirimi { get; set; }
        public int StokMiktari { get; set; }
        public string StokTuru { get; set; }
    }
}