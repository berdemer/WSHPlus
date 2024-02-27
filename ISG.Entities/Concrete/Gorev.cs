using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Gorev : IEntity
    {
        public int Gorev_Id { get; set; }
        public string MailKonu { get; set; }//görevin konusu
        public int? TekrarliGun { get; set; }//Tarihden günü alıp tekrar altında ise Mailislem false ise maili atıp true haline getirecek
        public bool MailIslem { get; set; }
        public string MailAdresleri { get; set; }//atılacak mail adresleri
        public string MuayeneTuru { get; set; }
        public int? RevirIslem_Id { get; set; }
        public int Personel_Id { get; set; }
        public int? Protokol { get; set; }
        public DateTime? Tarih { get; set; }
        public string UserId { get; set; }
        public bool Durum { get; set; }//konu iptal termin tamamlandı
        //public virtual RevirIslem RevirIslem { get; set; }
        //public virtual Personel Personel { get; set; }

    }
}