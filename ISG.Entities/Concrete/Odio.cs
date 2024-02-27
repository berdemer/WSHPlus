using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Odio : IEntity
    {
        public int Odio_Id { get; set; }
        public int? Sag250 { get; set; }
        public int? Sag500 { get; set; }
        public int? Sag1000 { get; set; }
        public int? Sag2000 { get; set; }
        public int? Sag3000 { get; set; }
        public int? Sag4000 { get; set; }
        public int? Sag5000 { get; set; }
        public int? Sag6000 { get; set; }
        public int? Sag7000 { get; set; }
        public int? Sag8000 { get; set; }
        public int? Sol250 { get; set; }
        public int? Sol500 { get; set; }
        public int? Sol1000 { get; set; }
        public int? Sol2000 { get; set; }
        public int? Sol3000 { get; set; }
        public int? Sol4000 { get; set; }
        public int? Sol5000 { get; set; }
        public int? Sol6000 { get; set; }
        public int? Sol7000 { get; set; }
        public int? Sol8000 { get; set; }
        public int? SagOrtalama { get; set; }
        public int? SolOrtalama { get; set; }
        public string Sonuc { get; set; }
        public string MuayeneTuru { get; set; }
        public int RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public int Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }

    }
}
