using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfCalismaAnaliziDal : EfEntityRepositoryBase<CalismaAnalizi, ISGContext>, ICalismaAnaliziDal
    {
        public async Task<object> MeslekListesi()//Select distinct using linq
        {
            using (var context = new ISGContext())
            {
                return await context.CalismaAnalizleri.GroupBy(test => test.MeslekAdi).Select(grp => grp.FirstOrDefault()).Select(x => new { MeslekAdi = x.MeslekAdi, Meslek_Kodu = x.Meslek_Kodu }).ToListAsync();
            }
        }

        public async Task<CalismaAnalizi> MeslekAra(string meslekKodu)//Select distinct using linq
        {
                using (var context = new ISGContext())
                {
                    return await context.Set<CalismaAnalizi>().Where(p => p.Meslek_Kodu == meslekKodu).FirstOrDefaultAsync();
                }                                       
        }

        public async Task<CalismaAnalizi> BolumAraAsync(int Bolum_Id, int Sirket_Id)
        {
            try
            {
                using (var context = new ISGContext())
                {
                    return await context.Set<CalismaAnalizi>().Where(p => p.Bolum_Id == Bolum_Id && p.Sirket_Id == Sirket_Id).FirstOrDefaultAsync(); ;
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
