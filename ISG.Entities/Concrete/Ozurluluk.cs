using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Ozurluluk : IEntity
    {
        public int Ozurluluk_Id { get; set; }
        public string HastalikTanimi { get; set; }
        public int Oran { get; set; }
        public string Derecesi { get; set; }//3. derece %40-60 2 derece 60-80 3 derece 80-100
        public string HastahaneAdi { get; set; }
        public string Aciklama { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
