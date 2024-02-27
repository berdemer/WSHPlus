using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfRevirIslemDal : EfEntityRepositoryBase<RevirIslem, ISGContext>, IRevirIslemDal
    {
        public async Task<RevirDonusu> RevirIslem(RevirIslem revir, bool prt)
        {
            using (var context = new ISGContext())
            {
                SaglikBirimi Sb = await context.Set<SaglikBirimi>().Where(p => p.SaglikBirimi_Id == revir.SaglikBirimi_Id).FirstOrDefaultAsync();
                Sirket Si = await context.Set<Sirket>().Where(p => p.id == Sb.StiId).FirstOrDefaultAsync();
                if (Sb.Year == DateTime.Now.Year)
                {
                    Sb.Protokol = prt ? Sb.Protokol + 1 : 0;
                }
                else
                {
                    Sb.Protokol = prt ? 1 : 0;
                    Sb.Year = DateTime.Now.Year;
                };

                if (prt == true)
                {
                    context.Entry(Sb).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                else context.Entry(Sb).State = EntityState.Unchanged;

                revir.Protokol = prt == false ? 0 : Sb.Protokol;
                revir.Tarih = DateTime.Now;
                revir.Status = false;
                context.Set<RevirIslem>().Add(revir);
                await context.SaveChangesAsync();
                return new RevirDonusu() { Protokol = revir.Protokol ?? default(int), RevirIslem_Id = revir.RevirIslem_Id, RevirAdi = Si.sirketAdi + " " + Sb.Adi };
            }
        }

        public async Task<Object> RevirAnaliz(int year, int saglikBirimi_Id)
        {
            using (var context = new ISGContext())
            {
                return await (from p in context.RevirIslemleri
                              where p.Tarih.Value.Year == year && p.SaglikBirimi_Id == saglikBirimi_Id
                              group p by p.IslemDetayi into g
                              select new
                              {
                                  IslemDetayi = g.Key,
                                  Ocak = g.Where(t => t.Tarih.Value.Month == 1).Count(),
                                  Subat = g.Where(t => t.Tarih.Value.Month == 2).Count(),
                                  Mart = g.Where(t => t.Tarih.Value.Month == 3).Count(),
                                  Nisan = g.Where(t => t.Tarih.Value.Month == 4).Count(),
                                  Mayis = g.Where(t => t.Tarih.Value.Month == 5).Count(),
                                  Haziran = g.Where(t => t.Tarih.Value.Month == 6).Count(),
                                  Temmuz = g.Where(t => t.Tarih.Value.Month == 7).Count(),
                                  Agustos = g.Where(t => t.Tarih.Value.Month == 8).Count(),
                                  Eylul = g.Where(t => t.Tarih.Value.Month == 9).Count(),
                                  Ekim = g.Where(t => t.Tarih.Value.Month == 10).Count(),
                                  Kasim = g.Where(t => t.Tarih.Value.Month == 11).Count(),
                                  Aralik = g.Where(t => t.Tarih.Value.Month == 12).Count()
                              }).ToListAsync();
            }
        }
    }
}
