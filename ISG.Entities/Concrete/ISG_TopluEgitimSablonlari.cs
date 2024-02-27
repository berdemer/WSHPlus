using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public class ISG_TopluEgitimSablonlari : IEntity
    {
        public int ISG_TopluEgitimSablonlariId { get; set; }
        public string Egitim_Turu { get; set; }
        public string EgitimJson { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }

    }
}
