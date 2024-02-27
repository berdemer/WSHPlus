using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Gorme : IEntity
    {
        public int Gorme_Id { get; set; }
        public string GozKapagi { get; set; }//Þiþlik,þiþme,kýzarklýk,normal
        public string GozKuresi { get; set; }//Çökmesi , Çýkmasý, Normal
        public string GozKaymasi { get; set; }//içe dþa yok
        public string GozdeKizariklik { get; set; }//var yok
        public string PupillaninDurumu { get; set; }//kucuk,buyuk,esit,birbirinden farklý
        public string IsikRefleksi { get; set; }//var yok
        public string GormeAlani { get; set; }//normal %10,20,50,fazla
        public string GozTonusu { get; set; }//Yumuþak  HÝPOTON,Normal	NORMOTON,Sert		HÝPERTON
        public string Fundoskopi { get; set; }
        public string GormeKeskinligi { get; set; }//10/10,
        public string DerinlikDuyusu { get; set; }//var yok
        public string RenkKorlugu { get; set; }//var yok
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
 * GormeKeskinligi
 * 
 10/10		Tam görme
5/10			Sürücüler için kabul edilebilir
1/10			Legal körlük
PS			Parmak sayma
EH			El hareketleri
P+P +		Persepsiyon ve projeksiyon
P +			Iþýk algýlama
Absolu		Iþýk algýlama yok

    http://www.antahed.org.tr/ALGOR/Goz%20Muayene.ppt

     */
