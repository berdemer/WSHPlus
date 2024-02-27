using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NinjaNye.SearchExtensions;
using System;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfICD10Dal : EfEntityRepositoryBase<ICD10, ISGContext>, IICD10Dal
    {
        public async Task<ICollection<ICD10>> KontrolluIcd10Kaydi(IEnumerable<ICD10> icd10)
        {
            using (var context = new ISGContext())
            {
                foreach (var icd in icd10)
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    string sd = (icd.ICD10Code).Substring(0, 3);
                    icd.IdRef = await context.Set<ICD10>().Where(x => x.ICD10Code == sd).Select(x => x.ICD10_Id).FirstOrDefaultAsync();
                    context.Set<ICD10>().Add(icd);
                    await context.SaveChangesAsync();
                }
                return await context.Set<ICD10>().ToListAsync();
            }
        }

        public async Task<ICD10Table> ICDSearch(string Search, int DisplayStart, int DisplayLength)
        {
            string[] ouy = Search.ToLower().Split(' ');
            string search = Search;
            using (var context = new ISGContext())
            {
                //IList<ICD10> sd = await context.Set<ICD10>().ToListAsync();
                IList<icd> sd = await (from p in context.ICD10
                                                    select new icd()
                                                    {   ICD10_Id=p.ICD10_Id,
                                                        ICD10Code=p.ICD10Code,
                                                        ICDTanimi=p.ICDTanimi
                                                    }).ToListAsync();

                var sdx = sd.AsQueryable().Search(
                x => !string.IsNullOrEmpty(x.ICD10Code) ? x.ICD10Code.ToLower() : "",
                x => !string.IsNullOrEmpty(x.ICDTanimi) ? x.ICDTanimi.ToLower() : ""
                 ).ContainingAll(ouy);
                int count = sdx.Count();
                IList<icd> xcv = sdx.Skip(DisplayStart).Take(DisplayLength).ToList();
                return new ICD10Table() { DisplayLength = DisplayLength, DisplayStart = DisplayStart, TotalItems = count, ICDView = xcv };
            }
        }

        public async Task<ICollection<ICD10>> HastalikAdiAra(string value)
        {
            using (var context = new ISGContext())
            {
                //string[] ouy = value.ToLower().Split(' ');
                //if (value.Length > 1 && value.Length < 4)
                //{
                //    return await context.ICD10.Where(x => x.ICDTanimi.StartsWith(value)).ToListAsync();
                //}//direkt sqlden sonuç alýr çok hýzlýdýr.

                //if (value.Length > 1 && value.Length <4)
                //{
                return await context.ICD10.Where(x => new[] { x.ICDTanimi, x.ICD10Code }.Any(s => s.Contains(value))).ToListAsync();

                //}//direkt sqlden sonuç alýr çok hýzlýdýr.
                //if (value.Length > 3)
                //{
                //    IList<ICD10> sd = await context.Set<ICD10>().ToListAsync();
                //    var sdx = sd.Search(x => !string.IsNullOrEmpty(x.ICDTanimi) ? x.ICDTanimi.ToLower() : "").Soundex(ouy);
                //    return sdx.ToList();
                //}//Soundex algoritmasý uygulanýr.Hata paylarýný düzeltir.
                //return null;

            }
        }
    }
}
