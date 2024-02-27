using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public class Adres : IEntity
    {
        public int Adres_Id { get; set; }
        public string Adres_Turu { get; set; }
        public string GenelAdresBilgisi { get; set; }
        public string EkAdresBilgisi { get; set; }
        public string MapLokasyonu { get; set; }
        public int Personel_Id { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
