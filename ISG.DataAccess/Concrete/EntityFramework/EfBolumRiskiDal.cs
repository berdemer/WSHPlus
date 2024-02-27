using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfBolumRiskiDal : EfEntityRepositoryBase<BolumRiski, ISGContext>, IBolumRiskiDal
    {
        public async Task<Object> Muayene_Durum_Listesi(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            using (var context = new ISGContext())
            {

                var list = await (from i in context.PersonelMuayeneleri
                                  join r in context.Personeller on i.Personel_Id equals r.Personel_Id
                                  join s in context.Sirketler on r.Sirket_Id equals s.id
                                  join b in context.SirketBolumleri on r.Bolum_Id equals b.id
                                  where r.Sirket_Id == Sirket_Id && i.PMJson.Contains(muayeneDurumu) && i.PMJson.Contains(muayeneSonucu) && i.Tarih.Value.Year == year
                                  select new
                                  {
                                      PersonelMuayene_Id = i.PersonelMuayene_Id,
                                      Personel_Id = r.Personel_Id,
                                      SirketAdi = s.sirketAdi,
                                      BolumAdi = b.bolumAdi,
                                      SirketId = r.Sirket_Id,
                                      BolumId = r.Bolum_Id,
                                      PersonelAdi = r.Adi,
                                      PersonelSoyadi = r.Soyadi,
                                      Tarih = i.Tarih,
                                      TxtData = i.PMJson,
                                      TcNo = r.TcNo,
                                      SicilNo = r.SicilNo
                                  }
                  ).ToListAsync();

                return list.Select(x => new
                {
                    PersonelMuayene_Id = x.PersonelMuayene_Id,
                    Personel_Id = x.Personel_Id,
                    SirketAdi = x.SirketAdi,
                    BolumAdi = x.BolumAdi,
                    SirketId = x.SirketId,
                    BolumId = x.BolumId,
                    PersonelAdi = x.PersonelAdi,
                    PersonelSoyadi = x.PersonelSoyadi,
                    Tarih = x.Tarih,
                    TxtData = JObject.Parse(x.TxtData),
                    TcNo = x.TcNo,
                    SicilNo = x.SicilNo
                })
                .ToList();
            }
        }

        public async Task<Object> BolumlerinIsKazaSayilari(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            using (var context = new ISGContext())
            {
                return await (from i in context.PersonelMuayeneleri
                              join r in context.Personeller on i.Personel_Id equals r.Personel_Id
                              join s in context.Sirketler on r.Sirket_Id equals s.id
                              join b in context.SirketBolumleri on r.Bolum_Id equals b.id
                              where r.Sirket_Id == Sirket_Id && i.PMJson.Contains(muayeneDurumu) && i.PMJson.Contains(muayeneSonucu) && i.Tarih.Value.Year == year
                              group i by b.bolumAdi
                               into o
                              select new
                              {
                                  bolumAdi = o.Key,
                                  Ocak = o.Where(t => t.Tarih.Value.Month == 1).Count(),
                                  Subat = o.Where(t => t.Tarih.Value.Month == 2).Count(),
                                  Mart = o.Where(t => t.Tarih.Value.Month == 3).Count(),
                                  Nisan = o.Where(t => t.Tarih.Value.Month == 4).Count(),
                                  Mayis = o.Where(t => t.Tarih.Value.Month == 5).Count(),
                                  Haziran = o.Where(t => t.Tarih.Value.Month == 6).Count(),
                                  Temmuz = o.Where(t => t.Tarih.Value.Month == 7).Count(),
                                  Agustos = o.Where(t => t.Tarih.Value.Month == 8).Count(),
                                  Eylul = o.Where(t => t.Tarih.Value.Month == 9).Count(),
                                  Ekim = o.Where(t => t.Tarih.Value.Month == 10).Count(),
                                  Kasim = o.Where(t => t.Tarih.Value.Month == 11).Count(),
                                  Aralik = o.Where(t => t.Tarih.Value.Month == 12).Count()
                              }).ToListAsync();
            }
        }

        //public async Task<object> PeriyodikMuayeneTakibiGelenler2(int Sirket_Id, int Yil, int ay, int sure)
        //{
        //    string guncelTarih = "01.01." + (Yil - sure).ToString();
        //    DateTime dateFrom = DateTime.Parse(guncelTarih);
        //    //convert iþlemleri linq sorgusu dýþýna almak gerekiyor.
        //    //https://www.gencayyildiz.com/blog/linq-to-entities-does-not-recognize-the-method-int32-toint32system-object-method-and-this-method-cannot-be-translated-unto-a-store-expression-hatasi-ve-cozumu/
        //    using (var context = new ISGContext())
        //    {
        //        return await (from n in context.Personeller
        //                      join s in context.PeriyodikMuayeneleri on n.Personel_Id equals s.Personel_Id
        //                      join d in context.Sirketler on n.Sirket_Id equals d.id
        //                      join b in context.SirketBolumleri on n.Bolum_Id equals b.id
        //                      where n.Sirket_Id == Sirket_Id && n.Status == true && s.Tarih <= dateFrom
        //                      select
        //                      new { id=s.Personel_Id, TcNo=n.TcNo, Adi=n.Adi.Trim(), Soyadi=n.Soyadi.Trim(), SirketAdi=d.sirketAdi.Trim(),BolumAdi=b.bolumAdi.Trim(), Tarih=s.Tarih })
        //                    .GroupBy(l => l.id)//personel id göre grupla
        //                    .Select(g => g.OrderByDescending(c => c.Tarih).FirstOrDefault())//son tarihi baþa gelecek þekilde sýrala ilkini al
        //                    .ToListAsync();
        //    }
        //}


        public async Task<object> PeriyodikMuayeneTakibiGelenler(int Sirket_Id, int Yil, int ay, int sure)
        {
            string guncelTarih = "01.01." + (Yil).ToString();
            string guncelTarih2 = "01.01." + (Yil + sure).ToString();
            DateTime baslangicTarih = DateTime.Parse(guncelTarih);
            DateTime bitisTarih = DateTime.Parse(guncelTarih2);
            DateTime bosTarih = DateTime.Parse("01.01.1900");
            //convert iþlemleri linq sorgusu dýþýna almak gerekiyor.
            ICollection<person> yapilanlar = null;//verilen tarih aralýðýnda muayenesi yapýlanlar
            ICollection<person> tumu = null;//O tarihte iþte çalýþanlar
            List<person> sonuc = new List<person>();
            //https://www.gencayyildiz.com/blog/linq-to-entities-does-not-recognize-the-method-int32-toint32system-object-method-and-this-method-cannot-be-translated-unto-a-store-expression-hatasi-ve-cozumu/
            using (var context = new ISGContext())
            {
                yapilanlar = await (from n in context.Personeller
                                    join s in context.PeriyodikMuayeneleri on n.Personel_Id equals s.Personel_Id
                                    join d in context.Sirketler on n.Sirket_Id equals d.id
                                    join b in context.SirketBolumleri on n.Bolum_Id equals b.id
                                    where n.Sirket_Id == Sirket_Id && n.Status == true && s.Tarih > baslangicTarih && s.Tarih < bitisTarih
                                    select
                              new person { id = s.Personel_Id, TcNo = n.TcNo, Adi = n.Adi.Trim(), Soyadi = n.Soyadi.Trim(), SirketAdi = d.sirketAdi.Trim(), BolumAdi = b.bolumAdi.Trim(), Tarih = s.Tarih ?? DateTime.Now, Mail = n.Mail, Telefon = n.Telefon })
                            .ToListAsync();


                tumu = await (from n in context.Personeller
                              join d in context.Sirketler on n.Sirket_Id equals d.id
                              join b in context.SirketBolumleri on n.Bolum_Id equals b.id
                              join v in context.PersonelDetaylari on n.Personel_Id equals v.PersonelDetay_Id
                              where n.Sirket_Id == Sirket_Id && n.Status == true && v.IlkIseBaslamaTarihi < bitisTarih
                              select
                              new person
                              {
                                  id = n.Personel_Id,
                                  TcNo = n.TcNo,
                                  Adi = n.Adi.Trim(),
                                  Soyadi = n.Soyadi.Trim(),
                                  SirketAdi = d.sirketAdi.Trim(),
                                  BolumAdi = b.bolumAdi.Trim(),
                                  Tarih = bosTarih,
                                  Mail = n.Mail.Trim(),
                                  Telefon = n.Telefon.Trim(),
                              })
            .ToListAsync();


                foreach (person i in tumu)
                {
                    if (!yapilanlar.Where(x => x.id == i.id).Any())
                    {
                        i.Tarih = await context.PeriyodikMuayeneleri.Where(x => x.Personel_Id == i.id).Select(x => x.Tarih).FirstOrDefaultAsync() ?? DateTime.Parse("01.01.1900");
                        i.Vardiyasi= (await (context.Calisma_Durumu.Where(x => x.Personel_Id == i.id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault()??"Verisi Yok!";
                        sonuc.Add(i);
                    }
                }

            }
            return sonuc.GroupBy(test => test.id).Select(grp => grp.First());
        }


        public async Task<object> AsiTakibiGelenler(int Sirket_Id, int Yil, int ay)
        {
            DateTime dateFrom = DateTime.Parse("01." + ay.ToString() + "." + Yil.ToString());
            using (var context = new ISGContext())
            {
                return await ((from n in context.Asilar
                               join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                               join d in context.Sirketler on s.Sirket_Id equals d.id
                               join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                               where s.Sirket_Id == Sirket_Id && s.Status == true && n.Muhtamel_Tarih <= dateFrom && n.Guncelleme_Suresi_Ay > 0
                               //muhtamel tarih tarih 
                               select new
                               {
                                   id = n.Personel_Id,
                                   Adi = s.Adi.Trim(),
                                   Soyadi = s.Soyadi.Trim(),
                                   TcNo = s.TcNo.Trim(),
                                   SirketAdi = d.sirketAdi.Trim(),
                                   BolumAdi = b.bolumAdi.Trim(),
                                   Asi_Tanimi = n.Asi_Tanimi.Trim(),
                                   Yapilma_Tarihi = n.Yapilma_Tarihi,
                                   Muhtamel_Tarih = n.Muhtamel_Tarih,
                                   Mail = s.Mail.Trim(),
                                   Telefon = s.Telefon.Trim(),
                               })
                              .GroupBy(l => new { l.id, l.Asi_Tanimi }).Select(g => g.OrderByDescending(c => c.Muhtamel_Tarih).FirstOrDefault()))//son tarihi baþa gelecek þekilde sýrala ilkini al
                              .ToListAsync();//personel id ve aþýlara göre gruplama yapýlmasý isteniyor.
            }

        }


        public async Task<object> DisRaporAnalizi(int Sirket_Id, int Yil, int ay, string muayeneTuru)
        {
            IList<Raporlulart> RaporluListesi = null;
            using (var context = new ISGContext())
            {
                RaporluListesi = await (from n in context.DýsRaporlari
                                        join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                                        join d in context.Sirketler on s.Sirket_Id equals d.id
                                        join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                                        where s.Sirket_Id == Sirket_Id && s.Status == true && n.BaslangicTarihi.Value.Month == ay && n.BaslangicTarihi.Value.Year == Yil && n.MuayeneTuru.ToLower().Contains(muayeneTuru.ToLower())
                                        select
                                        new Raporlulart
                                        {
                                            id = n.DisRapor_Id,
                                            Adi = s.Adi.Trim(),
                                            Soyadi = s.Soyadi.Trim(),
                                            TcNo = s.TcNo.Trim(),
                                            Guid_Id = s.PerGuid,
                                            SirketAdi = d.sirketAdi.Trim(),
                                            BolumAdi = b.bolumAdi.Trim(),
                                            MuayeneTuru = n.MuayeneTuru.Trim(),
                                            Tani = n.Tani.Trim(),
                                            BaslangicTarihi = n.BaslangicTarihi,
                                            BitisTarihi = n.BitisTarihi,
                                            SureGun = n.SureGun,
                                            DoktorAdi = n.DoktorAdi.Trim(),
                                            RaporuVerenSaglikBirimi = n.RaporuVerenSaglikBirimi.Trim(),
                                            Mail = s.Mail.Trim(),
                                            Telefon = s.Telefon.Trim(),
                                        })
                               .ToListAsync();//personel id ve aþýlara göre gruplama yapýlmasý isteniyor.

            }
            var gunToplami = RaporluListesi.Sum(x => x.SureGun);
            var AyPersonelSayisi = RaporluListesi.GroupBy(x => x.TcNo).Count();
            return new { RaporluListesi = RaporluListesi, gunToplami = gunToplami, AyPersonelSayisi = AyPersonelSayisi };
        }

        public async Task<object> EngelliTakibi(int Sirket_Id)
        {
            using (var context = new ISGContext())
            {
                return await ((from n in context.Ozurlulukler
                               join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                               join d in context.Sirketler on s.Sirket_Id equals d.id
                               join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                               where s.Sirket_Id == Sirket_Id && s.Status == true
                               select new
                               {
                                   Adi = s.Adi.Trim(),
                                   Soyadi = s.Soyadi.Trim(),
                                   TcNo = s.TcNo.Trim(),
                                   SirketAdi = d.sirketAdi.Trim(),
                                   BolumAdi = b.bolumAdi.Trim(),
                                   HastalikTanimi = n.HastalikTanimi,
                                   Oran = n.Oran,
                                   Derecesi = n.Derecesi,
                                   Mail = s.Mail.Trim(),
                                   Telefon = s.Telefon.Trim(),
                               })
                              .ToListAsync());
            }
        }


        public async Task<object> KronikHastaTakibi(int Sirket_Id)
        {
            using (var context = new ISGContext())
            {
                return await ((from n in context.KronikHastaliklar
                               join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                               join d in context.Sirketler on s.Sirket_Id equals d.id
                               join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                               where s.Sirket_Id == Sirket_Id && s.Status == true
                               select new
                               {
                                   Adi = s.Adi.Trim(),
                                   Soyadi = s.Soyadi.Trim(),
                                   TcNo = s.TcNo.Trim(),
                                   SirketAdi = d.sirketAdi.Trim(),
                                   BolumAdi = b.bolumAdi.Trim(),
                                   HastalikAdi = n.HastalikAdi,
                                   KullandigiIlaclar = n.KullandigiIlaclar,
                                   HastalikOzurDurumu = n.HastalikOzurDurumu,
                                   AmeliyatVarmi = n.AmeliyatVarmi,
                                   Mail = s.Mail.Trim(),
                                   Telefon = s.Telefon.Trim(),
                               })
                              .ToListAsync());
            }
        }

        public async Task<object> AllerjiHastaTakibi(int Sirket_Id)
        {
            using (var context = new ISGContext())
            {
                return await ((from n in context.Allerjiler
                               join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                               join d in context.Sirketler on s.Sirket_Id equals d.id
                               join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                               where s.Sirket_Id == Sirket_Id && s.Status == true
                               select new
                               {
                                   Adi = s.Adi.Trim(),
                                   Soyadi = s.Soyadi.Trim(),
                                   TcNo = s.TcNo.Trim(),
                                   SirketAdi = d.sirketAdi.Trim(),
                                   BolumAdi = b.bolumAdi.Trim(),
                                   Oykusu = n.AllerjiOykusu,
                                   Cesidi = n.AllerjiCesiti,
                                   Etkeni = n.AllerjiEtkeni,
                                   Mail = s.Mail.Trim(),
                                   Telefon = s.Telefon.Trim()
                               })
                              .ToListAsync());
            }
        }

        public async Task<object> AliskanlikHastaTakibi(int Sirket_Id)
        {
            using (var context = new ISGContext())
            {
                return await ((from n in context.Aliskanliklar
                               join s in context.Personeller on n.Personel_Id equals s.Personel_Id
                               join d in context.Sirketler on s.Sirket_Id equals d.id
                               join b in context.SirketBolumleri on s.Bolum_Id equals b.id
                               where s.Sirket_Id == Sirket_Id && s.Status == true
                               select new
                               {
                                   Adi = s.Adi.Trim(),
                                   Soyadi = s.Soyadi.Trim(),
                                   TcNo = s.TcNo.Trim(),
                                   SirketAdi = d.sirketAdi.Trim(),
                                   BolumAdi = b.bolumAdi.Trim(),
                                   Madde = n.Madde,
                                   BaslamaTarihi = n.BaslamaTarihi,
                                   BitisTarihi = n.BitisTarihi,
                                   siklik = n.SiklikDurumu
                               })
                              .ToListAsync());
            }
        }

        public async Task<Object> Gunluk_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih)
        {
            var bitistarih = tarih.AddDays(1);
            using (var context = new ISGContext())
            {
                var xc1 = await (from i in (from cust in context.PeriyodikMuayeneleri
                                            select new
                                            {
                                                MuayeneTuru = cust.MuayeneTuru,
                                                personelId = cust.Personel_Id,
                                                RevirIslem_Id = cust.RevirIslem_Id,
                                                Tarih = cust.Tarih
                                            }
              )
             .Concat//Union All fonksiyonu
                 (from emp in context.PersonelMuayeneleri
                  select new
                  {
                      MuayeneTuru = emp.MuayeneTuru,
                      personelId = emp.Personel_Id,
                      RevirIslem_Id = emp.RevirIslem_Id,
                      Tarih = emp.Tarih
                  }
                 )
                                 join r in context.Personeller on i.personelId equals r.Personel_Id
                                 join s in context.Sirketler on r.Sirket_Id equals s.id
                                 join l in context.SirketBolumleri on r.Bolum_Id equals l.id
                                 join b in context.RevirIslemleri on i.RevirIslem_Id equals b.RevirIslem_Id
                                 join c in context.SaglikBirimleri on b.SaglikBirimi_Id equals c.SaglikBirimi_Id
                                 where c.SaglikBirimi_Id == SaglikBirimi_Id && (i.Tarih >= tarih && i.Tarih <= bitistarih)
                                 select new
                                 {
                                     PersonelMuayene_Id = b.RevirIslem_Id,
                                     Personel_Id = r.Personel_Id,
                                     SirketAdi = s.sirketAdi,
                                     BolumAdi = l.bolumAdi,
                                     PersonelAdi = r.Adi,
                                     PersonelSoyadi = r.Soyadi,
                                     Tarih = i.Tarih,
                                     TcNo = r.TcNo,
                                     SicilNo = r.SicilNo,
                                     Muayene_Turu = i.MuayeneTuru,
                                     IslemDetayi = b.IslemDetayi,
                                     IslemTuru = b.IslemTuru,
                                     SirketId = r.Sirket_Id,
                                     BolumId = r.Bolum_Id,
                                 }).ToListAsync();
                return xc1.Select(x => new
                {
                    PersonelMuayene_Id = x.PersonelMuayene_Id,
                    Personel_Id = x.Personel_Id,
                    SirketAdi = x.SirketAdi,
                    BolumAdi = x.BolumAdi,
                    SirketId = x.SirketId,
                    BolumId = x.BolumId,
                    PersonelAdi = x.PersonelAdi,
                    PersonelSoyadi = x.PersonelSoyadi,
                    Tarih = x.Tarih,
                    TcNo = x.TcNo,
                    SicilNo = x.SicilNo,
                    Muayene_Turu = x.Muayene_Turu
                })
                .ToList();
            }
        }

        public async Task<Object> Aylik_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih)
        {
            DateTime baslangictarih = new DateTime(tarih.Year, tarih.Month, 1);
            DateTime bitistarih = baslangictarih.AddMonths(1);
            using (var context = new ISGContext())
            {

                var xc1 = await (from i in (from cust in context.PeriyodikMuayeneleri
                                            select new
                                            {
                                                MuayeneTuru = cust.MuayeneTuru,
                                                personelId = cust.Personel_Id,
                                                RevirIslem_Id = cust.RevirIslem_Id,
                                                Tarih = cust.Tarih
                                            }
          )
         .Concat//Union All fonksiyonu
             (from emp in context.PersonelMuayeneleri
              select new
              {
                  MuayeneTuru = emp.MuayeneTuru,
                  personelId = emp.Personel_Id,
                  RevirIslem_Id = emp.RevirIslem_Id,
                  Tarih = emp.Tarih
              }
             )
                                 join r in context.Personeller on i.personelId equals r.Personel_Id
                                 join s in context.Sirketler on r.Sirket_Id equals s.id
                                 join l in context.SirketBolumleri on r.Bolum_Id equals l.id
                                 join b in context.RevirIslemleri on i.RevirIslem_Id equals b.RevirIslem_Id
                                 join c in context.SaglikBirimleri on b.SaglikBirimi_Id equals c.SaglikBirimi_Id
                                 where c.SaglikBirimi_Id == SaglikBirimi_Id && (i.Tarih >= baslangictarih && i.Tarih < bitistarih)
                                 select new
                                 {
                                     PersonelMuayene_Id = b.RevirIslem_Id,
                                     Personel_Id = r.Personel_Id,
                                     SirketAdi = s.sirketAdi,
                                     BolumAdi = l.bolumAdi,
                                     PersonelAdi = r.Adi,
                                     PersonelSoyadi = r.Soyadi,
                                     Tarih = i.Tarih,
                                     TcNo = r.TcNo,
                                     SicilNo = r.SicilNo,
                                     Muayene_Turu = i.MuayeneTuru,
                                     IslemDetayi = b.IslemDetayi,
                                     IslemTuru = b.IslemTuru,
                                     SirketId = r.Sirket_Id,
                                     BolumId = r.Bolum_Id,
                                 }).ToListAsync();


                return xc1.Select(x => new
                {
                    PersonelMuayene_Id = x.PersonelMuayene_Id,
                    Personel_Id = x.Personel_Id,
                    SirketAdi = x.SirketAdi,
                    BolumAdi = x.BolumAdi,
                    SirketId = x.SirketId,
                    BolumId = x.BolumId,
                    PersonelAdi = x.PersonelAdi,
                    PersonelSoyadi = x.PersonelSoyadi,
                    Tarih = x.Tarih,
                    TcNo = x.TcNo,
                    SicilNo = x.SicilNo,
                    Muayene_Turu = x.Muayene_Turu
                })
                .ToList();
            }
        }

        private class Raporlulart
        {
            public int id { get; set; }
            public string TcNo { get; set; }
            public string Adi { get; set; }
            public string Soyadi { get; set; }
            public string SirketAdi { get; set; }
            public string BolumAdi { get; set; }
            public int DisRapor_Id { get; set; }
            public string MuayeneTuru { get; set; }
            public string Tani { get; set; }
            public Nullable<System.DateTime> BaslangicTarihi { get; set; }
            public Nullable<System.DateTime> BitisTarihi { get; set; }
            public Nullable<int> SureGun { get; set; }
            public string DoktorAdi { get; set; }
            public string RaporuVerenSaglikBirimi { get; set; }
            public Guid Guid_Id { get; internal set; }
            public string Mail { get; internal set; }
            public string Telefon { get; internal set; }
        }
    }

    internal class person
    {
        public int id { get; set; }
        public string TcNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string SirketAdi { get; set; }
        public string BolumAdi { get; set; }
        public DateTime Tarih { get; set; }
        public string Mail { get; set; }
        public string Telefon { get; set; }
        public string Vardiyasi { get; set; }

    }
}
//regex ile analiz yapabilme özellði gösterir gün sayýlarýný alýr

//var pattern = @"\""GunSayisi""\:\d+";

//return list.Select(x => new
//{
//    Asy = Regex.Match(x.TxtData, pattern),
//    PersonelMuayene_Id = x.PersonelMuayene_Id,
//    Personel_Id = x.Personel_Id,
//    SirketAdi = x.SirketAdi,
//    BolumAdi = x.BolumAdi,
//    SirketId = x.SirketId,
//    BolumId = x.BolumId,
//    PersonelAdi = x.PersonelAdi,
//    PersonelSoyadi = x.PersonelSoyadi,
//    Tarih = x.Tarih,
//    TxtData = x.TxtData
//})
//.Where(x => x.Asy.Success)
//.Select(x => new
//{
//    PersonelMuayene_Id = x.PersonelMuayene_Id,
//    Personel_Id = x.Personel_Id,
//    SirketAdi = x.SirketAdi,
//    BolumAdi = x.BolumAdi,
//    SirketId = x.SirketId,
//    BolumId = x.BolumId,
//    PersonelAdi = x.PersonelAdi,
//    PersonelSoyadi = x.PersonelSoyadi,
//    Tarih = x.Tarih,
//    TxtData =JObject.Parse(x.TxtData),
//    IstirahatGunSayisi = x.Asy.Value.Split(':')[1]
//   // deger = x.Asy.Value.Split(':')[0],
//})
//.ToList();