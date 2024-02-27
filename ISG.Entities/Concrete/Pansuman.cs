using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Pansuman : IEntity
    {
        public int Pansuman_Id { get; set; }
        public Nullable<bool> IsKazasi { get; set; }
        public string YaraCesidi { get; set; }
        public string YaraYeri { get; set; }
        public string PansumaninAmaci { get; set; }
        public string PansumanTuru { get; set; }
        public string SuturBicimi { get; set; }
        public string Sonuc { get; set; }      
        public int RevirIslem_Id { get; set; }
        public string MuayeneTuru { get; set; }
        public int Personel_Id { get; set; }
        public int? Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
/*
 Pansumanın amacı,
 Yarayı dış etkenlerden ve enfeksiyonlardan korumak,
 Yarada bulunan akıntıyı emmek ve uzaklaştırmak,
 Kanamayı durdurmak,
 Yaraya ilaç uygulamak,
 Yara ve çevresindeki dokuyu desteklemek,
 Ağrıyı azaltmak ve ısı kaybını önlemek,
 Nemli ortam sağlamak, ödemi önlemek,
 İyileşme sürecini hızlandırmaktır.

Yara çeşitlerini 6 grupta inceliyoruz. 1)Sıyrık yarası 2) Kesik yarası 3) Ezik yarası 4) Delici yara 5) Parçalı yara 6) Enfekte yara 
Yara Yeri  
 Kontüzyon
 Laserasyon
 Avülsiyon
 Amputasyon
 Batma
 Isırıklar

     */
