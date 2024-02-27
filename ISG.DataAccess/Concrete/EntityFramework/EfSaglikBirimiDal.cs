using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfSaglikBirimiDal : EfEntityRepositoryBase<SaglikBirimi, ISGContext>, ISaglikBirimiDal
    {
        public async Task<SaglikBirimi> ProtokolAl(int id)
        {
            using (var context = new ISGContext())
            {
                SaglikBirimi protokol = await context.Set<SaglikBirimi>().FindAsync(id);           
                if (protokol != null) {
                    SaglikBirimi updatedData = protokol;
                    if (protokol.Year == DateTime.Now.Year)
                    {
                        updatedData.Protokol = protokol.Protokol == 0 ? 1 : protokol.Protokol + 1;
                        updatedData.Year = DateTime.Now.Year;
                    }
                    else
                    {
                        updatedData.Year = DateTime.Now.Year;
                        updatedData.Protokol = 1;
                    }
                    context.Entry(protokol).CurrentValues.SetValues(updatedData);
                    await context.SaveChangesAsync();
                }
                return protokol;
            }
        }
    }
}
