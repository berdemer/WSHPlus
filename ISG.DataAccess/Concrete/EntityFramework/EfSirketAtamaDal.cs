using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfSirketAtamaDal : EfEntityRepositoryBase<SirketAtama, ISGContext>, ISirketAtamaDal
    {
        public async Task<bool> AtamaKontrol(string user_Id, int Sirket_Id)
        {
            using (var context = new ISGContext())
            {
                return await context.Set<SirketAtama>().Where(p => p.Sirket_id == Sirket_Id && p.uzmanPersonelId== user_Id).AnyAsync();
            }
        }
    }
}
