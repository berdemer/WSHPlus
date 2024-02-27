using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Gorme : IEntity
    {
        public int Gorme_Id { get; set; }
        public string GozKapagi { get; set; }//�i�lik,�i�me,k�zarkl�k,normal
        public string GozKuresi { get; set; }//��kmesi , ��kmas�, Normal
        public string GozKaymasi { get; set; }//i�e d�a yok
        public string GozdeKizariklik { get; set; }//var yok
        public string PupillaninDurumu { get; set; }//kucuk,buyuk,esit,birbirinden farkl�
        public string IsikRefleksi { get; set; }//var yok
        public string GormeAlani { get; set; }//normal %10,20,50,fazla
        public string GozTonusu { get; set; }//Yumu�ak  H�POTON,Normal	NORMOTON,Sert		H�PERTON
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
 10/10		Tam g�rme
5/10			S�r�c�ler i�in kabul edilebilir
1/10			Legal k�rl�k
PS			Parmak sayma
EH			El hareketleri
P+P +		Persepsiyon ve projeksiyon
P +			I��k alg�lama
Absolu		I��k alg�lama yok

    http://www.antahed.org.tr/ALGOR/Goz%20Muayene.ppt

     */
