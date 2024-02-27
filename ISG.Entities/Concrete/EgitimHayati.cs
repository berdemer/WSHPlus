using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class EgitimHayati : IEntity
    {
        public int EgitimHayati_Id { get; set; }
        public string Egitim_seviyesi { get; set; }
        public string Okul_Adi { get; set; }
        public DateTime? Baslama_Tarihi { get; set; }
        public DateTime? Bitis_Tarihi { get; set; }
        public string Meslek_Tanimi { get; set; }    
        public string UserId { get; set; }
        public int Personel_Id { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
