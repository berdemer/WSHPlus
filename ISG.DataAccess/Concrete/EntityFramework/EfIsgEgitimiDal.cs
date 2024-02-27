using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System;
using ISG.Entities.ComplexType;
using System.Globalization;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfIsgEgitimiDal : EfEntityRepositoryBase<IsgEgitimi, ISGContext>, IIsgEgitimiDal
    {
        public async Task<bool> SilAsync(int x)
        {
            using (var context = new ISGContext())
            {
                context.IsgEgitimleri.RemoveRange(await context.IsgEgitimleri.Where(a => a.IsgTopluEgitimi_Id == x).ToListAsync());
                _ = await context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<object> EgitimAlanPersAsyc(int Sirket_Id, int Year)
        {
                if (Sirket_Id == 0) return null;else
                using (var context = new ISGContext())
                {
                    var query = await (context.IsgEgitimleri.Where(v => v.VerildigiTarih.Value.Year == Year)
                      .GroupBy(c => new { c.Personel_Id, c.DersTipi })
                      .Select(g => new
                      {
                          g.Key.Personel_Id,
                          Teknik = g.Where(x => x.Tanimi == "Teknik").Sum(x => x.Egitim_Suresi),
                          Genel = g.Where(x => x.Tanimi == "Genel").Sum(x => x.Egitim_Suresi),
                          Saglik = g.Where(x => x.Tanimi == "Saglik").Sum(x => x.Egitim_Suresi)
                      }).ToListAsync());

                    var sirketDetayi = await context.SirketDetaylari.Where(x => x.SirketDetayi_Id == Sirket_Id).Select(x => new { tehlike= x.TehlikeGrubu,x.SirketYetkilisi}).FirstAsync();
                    int genel_tanimlanmis_sure = 0;
                    string tehlikeGrubu = "Tehlike durumu sisteme yüklenmemiþtir.";
                    switch (sirketDetayi.tehlike)
                    {
                        case 1:
                            genel_tanimlanmis_sure = 8*60;
                            tehlikeGrubu="Az Tehlikeli(8 Saat)";
                            break;
                        case 2:
                            genel_tanimlanmis_sure = 12*60;
                            tehlikeGrubu = "Tehlikeli(12 saat)";
                            break;
                        case 3:
                            genel_tanimlanmis_sure = 16*60;
                            tehlikeGrubu = "Çok Tehlikeli(16 saat)";
                            break;
                    };

                    var ekBilgiBenzersizleri=await context.IsgEgitimleri.Where(v => v.VerildigiTarih.Value.Year == Year).Select(x => new { x.Personel_Id, x.VerildigiTarih, x.Tanimi }).Distinct().ToListAsync();

                    IList<personelCardView> sd = await (from p in context.Personeller
                                                        join S in context.Sirketler on p.Sirket_Id equals S.id
                                                        join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
                                                        where p.Sirket_Id == Sirket_Id && p.Status == true
                                                        select new personelCardView()
                                                        {
                                                            Personel_Id = p.Personel_Id,
                                                            AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
                                                            TcNo = p.TcNo,
                                                            KanGrubu = p.KanGrubu.Trim(),
                                                            SicilNo = p.SicilNo.Trim(),
                                                            SgkNo = p.SgkNo.Trim(),
                                                            Photo = p.Photo,
                                                            Mail = p.Mail.Trim(),
                                                            Telefon = p.Telefon.Trim(),
                                                            sirketAdi = S.sirketAdi.Trim(),
                                                            KadroDurumu = p.KadroDurumu.Trim(),
                                                            Gorevi = p.Gorevi.Trim(),
                                                            PerGuid = p.PerGuid,
                                                            bolumAdi = SB.bolumAdi.Trim()
                                                        }).ToListAsync();
                    List<object> vb = new List<object>();
                    foreach (personelCardView i in sd)
                    {
                        var bilgiler = query.Where(x => x.Personel_Id == i.Personel_Id).ToList();
                        int teknik = 0; int Genel = 0; int Saglik = 0;
                        foreach (var z in bilgiler)
                        {
                            if (z != null && z.Teknik > 0)
                            {
                                teknik = (int)z.Teknik;
                            }
                            if (z.Genel != null && z.Genel > 0)
                            {
                                Genel = (int)z.Genel;
                            }
                            if (z.Saglik != null && z.Saglik > 0)
                            {
                                Saglik = (int)z.Saglik;
                            }
                        }
                        var ekbilgiler = ekBilgiBenzersizleri.Where(x => x.Personel_Id == i.Personel_Id).ToList();
                        List<string> sdbn = ekbilgiler.Where(x => x.Tanimi.Trim() == "Saglik").Select(x =>x.VerildigiTarih.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)).ToList();
                        List<string> gdbn = ekbilgiler.Where(x => x.Tanimi.Trim() == "Genel").Select(x => x.VerildigiTarih.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)).ToList();
                        List<string> tdbn = ekbilgiler.Where(x => x.Tanimi.Trim() == "Teknik").Select(x => x.VerildigiTarih.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)).ToList();
                        var detayliEgitim = await (context.IsgEgitimleri.Where(v => v.Personel_Id == i.Personel_Id&& v.VerildigiTarih.Value.Year == Year).ToListAsync());
                        vb.Add(new
                        {
                            E_Id = i.Personel_Id,
                            i.AdiSoyadi,
                            i.TcNo,
                            i.Mail,
                            i.Telefon,
                            SirketAdi = i.sirketAdi,
                            i.SicilNo,
                            BolumAdi = i.bolumAdi,
                            i.Gorevi,
                            Teknik_Suresi = teknik.ToString()+" dk",
                            Teknik_Sayisi = tdbn.Count,
                            Teknik_Tarihleri = string.Join(", ", tdbn),
                            Genel_Suresi = Genel.ToString() + " dk",
                            Genel_Sayisi = gdbn.Count,
                            Genel_Tarihleri = string.Join(", ", gdbn),
                            Saglik_Suresi = Saglik.ToString() + " dk",
                            Saglik_Sayisi = sdbn.Count,
                            Saglik_Tarihleri = string.Join(", ", sdbn),
                            ISG_Toplam_Suresi = teknik + Genel + Saglik,
                            ISG_Toplam_Suresi_Saat = TimeSpan.FromMinutes(teknik + Genel + Saglik),
                            ISG_Toplam_KatilimSayisi = tdbn.Count + gdbn.Count + sdbn.Count,
                            BasarimYuzdesi= "% " + (genel_tanimlanmis_sure > 0 ? Math.Ceiling((((double)(teknik + Genel + Saglik) / (double)genel_tanimlanmis_sure) * 100)).ToString() : "Toplam süre 0?"),
                            tehlikeGrubu,
                            sirketYetkilisi = sirketDetayi.SirketYetkilisi!=null ?sirketDetayi.SirketYetkilisi.Trim():"",
                            detayliEgitim
                        });
                    }
                    return vb;
                }
        }
    }
}
