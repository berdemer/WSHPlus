using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfDisRaporDal : EfEntityRepositoryBase<DisRapor, ISGContext>, IDisRaporDal
    {
        public async Task<DisRapor> MukerrerDisRaporKontrol(DateTime baslangic, int Personel_Id)
        {
            using (var context = new ISGContext())
            {
                return await context.Set<DisRapor>().Where(p => p.BaslangicTarihi == baslangic.Date & p.Personel_Id == Personel_Id).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> MukerrerDisRaporTakibi(DateTime baslangic, DateTime bitis, int Personel_Id)
        {
            IList<DisRapor> RaporlununListesi = null;
            using (var context = new ISGContext())
            {
                RaporlununListesi = await context.Set<DisRapor>().Where(p => p.Personel_Id == Personel_Id).ToListAsync();
            }
            TimeRange timeRange1 = new TimeRange(baslangic, bitis);
            bool akim = false;
            foreach (DisRapor x in RaporlununListesi)
            {
                TimeRange timeRange2 = new TimeRange((DateTime)x.BaslangicTarihi, (DateTime)x.BitisTarihi);
                switch (timeRange1.GetRelation(timeRange2))
                {
                    case PeriodRelation.After:
                        akim = false;
                        break;
                    case PeriodRelation.Before:
                        akim = false;
                        break;
                    case PeriodRelation.StartTouching:
                        akim = false;
                        break;
                    case PeriodRelation.EndTouching:
                        akim = false;
                        break;
                    default:
                        akim = true;
                        break;
                }
                if (akim == true) return true;
            }
            return false;
        }
    }
}
//(StartA > StartB? Start A: StartB) <= (EndA<EndB? EndA: EndB)
//https://stackoverflow.com/questions/325933/determine-whether-two-date-ranges-overlap?page=1&tab=votes#tab-top
/*
 SELECT 
    CONCAT(TRIM(Personel.Adi), ' ', TrIM(Personel.Soyadi)) AS 'Örnek Adı', 
    Personel.Gorevi AS 'Görevi',  Personel.TcNo AS 'TcNo',Sirketler.sirketAdi, SirketBolumleri.BolumAdi,
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 1 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Ocak',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 1 THEN 1 END) AS 'Ocak-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 2 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Şubat',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 2 THEN 1  END) AS 'Şubat-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 3 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Mart',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 3 THEN 1 END) AS 'Mart-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 4 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Nisan',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 4 THEN 1 END) AS 'Nisan-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 5 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Mayıs',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 5 THEN 1 END) AS 'Mayıs-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 6 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Haziran',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 6 THEN 1 END) AS 'Haziran-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 7 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Temmuz',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 7 THEN 1 END) AS 'Temmuz-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 8 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Ağustos',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 8 THEN 1 END) AS 'Ağustos-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 9 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Eylül',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 9 THEN 1 END) AS 'Eylül-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 10 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Ekim',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 10 THEN 1 END) AS 'Ekim-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 11 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Kasım',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 11 THEN 1 END) AS 'Kasım-Adet',
    SUM(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 12 THEN DısRaporlari.SureGun ELSE 0 END) AS 'Aralık',
	COUNT(CASE WHEN MONTH(DısRaporlari.BaslangicTarihi) = 12 THEN 1 END) AS 'Aralık-Adet',
    SUM(DısRaporlari.SureGun) AS 'Gün Toplam',
	COUNT(DısRaporlari.BaslangicTarihi) AS 'Adet Toplam'
FROM Personel
INNER JOIN DısRaporlari ON Personel.Personel_Id = DısRaporlari.Personel_Id
INNER JOIN Sirketler ON Personel.Sirket_Id = Sirketler.id
INNER JOIN SirketBolumleri ON Personel.Bolum_Id = SirketBolumleri.id
WHERE YEAR(DısRaporlari.BaslangicTarihi) = 2020 AND Personel.Durumu = 1
GROUP BY Personel.Adi, Personel.Soyadi, Personel.Gorevi,  Personel.TcNo,Sirketler.sirketAdi, SirketBolumleri.BolumAdi




var query = from p in context.Personel
            join d in context.DısRaporlari on p.Personel_Id equals d.Personel_Id
            join s in context.Sirketler on p.Sirket_Id equals s.id
            join b in context.SirketBolumleri on p.Bolum_Id equals b.id
            where d.BaslangicTarihi.Year == year
            group d by new
            {
                p.Adi,
                p.Soyadi,
                p.Gorevi,
                p.TcNo,
                s.sirketAdi,
                b.BolumAdi
            } into g
            select new
            {
                Örnek_Adı = g.Key.Adi.Trim() + " " + g.Key.Soyadi.Trim(),
                Görevi = g.Key.Gorevi,
                TcNo = g.Key.TcNo,
                sirketAdi = g.Key.sirketAdi,
                BolumAdi = g.Key.BolumAdi,
                Ocak = g.Sum(d => d.BaslangicTarihi.Month == 1 ? d.SureGun : 0),
                "Ocak-Adet" = g.Count(d => d.BaslangicTarihi.Month == 1),
                Subat = g.Sum(d => d.BaslangicTarihi.Month == 2 ? d.SureGun : 0),
                "Subat-Adet" = g.Count(d => d.BaslangicTarihi.Month == 2),
                Mart = g.Sum(d => d.BaslangicTarihi.Month == 3 ? d.SureGun : 0),
                "Mart-Adet" = g.Count(d => d.BaslangicTarihi.Month == 3),
                Nisan = g.Sum(d => d.BaslangicTarihi.Month == 4 ? d.SureGun : 0),
                "Nisan-Adet" = g.Count(d => d.BaslangicTarihi.Month == 4),
                Mayis = g.Sum(d => d.BaslangicTarihi.Month == 5 ? d.SureGun : 0),
                "Mayis-Adet" = g.Count(d => d.BaslangicTarihi.Month == 5),
                Haziran = g.Sum(d => d.BaslangicTarihi.Month == 6 ? d.SureGun : 0),
                "Haziran-Adet" = g.Count(d => d.BaslangicTarihi.Month == 6),
                Temmuz = g.Sum(d => d.BaslangicTarihi.Month == 7 ? d.SureGun : 0),
                "Temmuz-Adet" = g.Count(d => d.BaslangicTarihi.Month == 7),
                Agustos = g.Sum(d => d.BaslangicTarihi.Month == 8 ? d.SureGun : 0),
                "Agustos-Adet"= g.Count(d => d.BaslangicTarihi.Month == 8),
                Eylul = g.Sum(d => d.BaslangicTarihi.Month == 9 ? d.SureGun : 0),
                "Eylul-Adet" = g.Count(d => d.BaslangicTarihi.Month == 9),
                Ekim = g.Sum(d => d.BaslangicTarihi.Month == 10 ? d.SureGun : 0),
                "Ekim-Adet" = g.Count(d => d.BaslangicTarihi.Month == 10),
                Kasim = g.Sum(d => d.BaslangicTarihi.Month == 11 ? d.SureGun : 0),
                "Kasim-Adet" = g.Count(d => d.BaslangicTarihi.Month == 11),
                Aralik = g.Sum(d => d.BaslangicTarihi.Month == 12 ? d.SureGun : 0),
                "Aralik-Adet" = g.Count(d => d.BaslangicTarihi.Month == 12),
                 ToplamGun=g.Sum(d.SureGun),
                 ToplamGun=g.Count(d.BaslangicTarihi.Month)
                };
 
 
 
 
 
 */