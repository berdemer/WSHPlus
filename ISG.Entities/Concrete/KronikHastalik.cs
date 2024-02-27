using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class KronikHastalik : IEntity
    {
        public Guid KronikHastalik_Id { get; set; }

        public int Personel_Id { get; set; }

        public string HastalikAdi { get; set; }

        public string KullandigiIlaclar { get; set; }

        public int HastalikYilSuresi { get; set; }

        public bool AmeliyatVarmi { get; set; }

        public bool IsKazasi { get; set; }

        public bool HastalikOzurDurumu { get; set; }

        public string UserId { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Personel Personel { get; set; }
    }
}
