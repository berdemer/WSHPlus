using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Hemogram : IEntity
    {
        public int Hemogram_Id { get; set; }
        public string Eritrosit { get; set; }//rbc
        public string Hematokrit { get; set; }//hct
        public string Hemoglobin { get; set; }//hgb
        public string MCV { get; set; }//
        public string MCH { get; set; }
        public string MCHC { get; set; }
        public string RDW { get; set; }
        public string Lokosit { get; set; }//wbc
        public string Lenfosit_Yuzde { get; set; }//LYM
        public string Monosit_Yuzde { get; set; }//MID
        public string Granülosit_Yuzde { get; set; }//GRan
        public string Notrofil_Yuzde { get; set; }//NEU
        public string Eoznofil_Yuzde { get; set; }
        public string Bazofil_Yuzde { get; set; }
        public string Trombosit { get; set; }//PLT
        public string MeanPlateletVolume { get; set; }
        public string Platekrit { get; set; }
        public string PDW { get; set; }
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
/*
 Kan sayým cihazlarýnda genellikle aþaðýdaki parametreler saptanabilmektedir:
•	RBC (Red Blood Cell, kýrmýzý kan hücresi, eritrosit)
•	Hgb (Hemoglobin)
•	Hct (Hematokrit; PCV: Packed Cell Volume)
•	MCV (Mean Corpuscular Volume)
•	MCH (Mean Corpuscular Hemoglobin)
•	MCHC (Mean Corpuscular Hemoglobin Concentration)
•	RDW (Red cell Distribution Width, Eritrosit daðýlým geniþliði)

•	WBC (White Blood Cell, beyaz kan hücresi, lökosit)
•	Lym % ve # (Lenfosit % ve sayý)
•	Mono %  ve # (Monosit % ve sayý)
•	Gran % ve # (Granülosit % ve sayý)
(Ayrýca Neut, Eos, Bas % ve # : Nötrofil, eoznofil, bazofil % ve sayýsý)

•	Plt (Trombosit)
•	MPV (Mean Platelet Volume)
•	Pct (Platekrit)
•	PDW (Platelet Distribution Width, Trombosit daðýlým geniþliði)

     */
