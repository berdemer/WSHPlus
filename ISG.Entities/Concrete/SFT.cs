using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class SFT : IEntity
    {
        public int Sft_Id { get; set; }
        public int? FVC { get; set; }
        public int? FEV1 { get; set; }
        public int? Fev1Fvc { get; set; }
        public int? VC { get; set; }
        public int? RV { get; set; }
        public int? TLC { get; set; }
        public int? Fev2575 { get; set; }
        public int? FEV50 { get; set; }
        public int? MVV { get; set; }
        public int? PEF { get; set; }
        public string Sonuc { get; set; }
        /// <summary>
        /// iþlemler bölümü genel hususlar
        /// </summary>
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
