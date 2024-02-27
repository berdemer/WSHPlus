using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class RevirTedavi : IEntity
    {
        public RevirTedavi()
        {
            IlacSarfCikislari = new List<IlacSarfCikisi>();
        }       
        public int RevirTedavi_Id { get; set; }
        public string Sikayeti { get; set; }
        public string Tani { get; set; }
        public string HastaninIlaclari { get; set; }
        public string Sonuc { get; set; }
        public string MuayeneTuru { get; set; }
        public int Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public int? RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }
        public virtual ICollection<IlacSarfCikisi> IlacSarfCikislari { get; set; }
    }
}