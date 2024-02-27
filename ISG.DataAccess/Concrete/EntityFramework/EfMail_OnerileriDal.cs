using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NinjaNye.SearchExtensions;
using System.Text.RegularExpressions;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfMail_OnerileriDal : EfEntityRepositoryBase<Mail_Onerileri, ISGContext>, IMail_OnerileriDal
	{
        //private readonly object p;

        private IList<MailListesi> ustAmirMailleri(int Sti_Id, int Blm_Id)//üst amirinde mail adresi yoksa kademeli olarak bir üstündeki mail adreslerini alacak
        {
            using (var context = new ISGContext())
            {
                SirketBolumu referansBlm =  context.Set<SirketBolumu>().Where(p => p.id == Blm_Id).FirstOrDefault();
                IList<MailListesi> referansMailListesi =  (from p in context.Personeller
                                                                where (p.Bolum_Id == referansBlm.idRef && p.Sirket_Id == Sti_Id && (p.Mail != null && p.Mail.Length > 0 && p.Status==true))
                                                                select new MailListesi()
                                                                {
                                                                    AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
                                                                    GonderimSekli = "Bilgi",
                                                                    MailAdresi = p.Mail.Trim(),
                                                                    OneriTanimi = ""
                                                                }).ToList();

                if (referansBlm.idRef == 0) {
                    return referansMailListesi;
                }
                if (referansMailListesi.Count > 0)
                {
                    return referansMailListesi;
                }
                else 
                {
                    return ustAmirMailleri(Sti_Id, referansBlm.idRef);
                }
                
            }
        }



        public async Task<IList<MailListesi>> MailOnerileri(int Sti_Id,int Blm_Id,string gonderimSekli)
        {
                using (var context = new ISGContext())
                {
                    //kiþinin üst amir referansýna göre //recursive metod dön
                    //SirketBolumu referansBlm = await context.Set<SirketBolumu>().Where(p => p.id == Blm_Id).FirstOrDefaultAsync();
                    //IList<MailListesi> referansMailListesi = await (from p in context.Personeller
                    //                                             where ( p.Bolum_Id== referansBlm.idRef && p.Sirket_Id==Sti_Id&& (p.Mail!=null && p.Mail.Length>0))
                    //                                                select new MailListesi()
                    //                                                {
                    //                                                   AdiSoyadi=p.Adi.Trim()+" "+p.Soyadi.Trim(),
                    //                                                   GonderimSekli = "Bilgi",
                    //                                                   MailAdresi=p.Mail.Trim(),
                    //                                                   OneriTanimi=""
                    //                                                }).ToListAsync();


                    IList<MailListesi> referansMailListesi = ustAmirMailleri(Sti_Id, Blm_Id);

                    //mail listesi referansýna göre
                    IList <MailListesi> MailListesi = await context.Set<Mail_Onerileri>()
                        .Where(p => p.Sirket_Id == Sti_Id && p.TumSirketteOneriListesinde == true //o þirkette tüm mail tanýmlamalarý
                        || p.Sirket_Id == Sti_Id && p.OneriTanimi == gonderimSekli && p.TumSirketteOneriListesinde == false////o þirketin mail öneri iþ kazalarý vs gönder
                        || p.Bolum_Id == Blm_Id  && p.TumSirketteOneriListesinde == false)//mail adresi o bölüme tanýmlanmýþsa
                        .Select(p=>new MailListesi {
                         AdiSoyadi= p.MailAdiVeSoyadi==null?"":p.MailAdiVeSoyadi.Trim(),
                         MailAdresi=p.MailAdresi.Trim(),
                         GonderimSekli=p.gonderimSekli.Trim(),
                         OneriTanimi=p.OneriTanimi.Trim()
                        }).ToListAsync();
                    return (referansMailListesi.Union(MailListesi)).OrderBy(p => p.AdiSoyadi).Distinct().ToList();
                }

        }
    }
}
