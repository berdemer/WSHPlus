using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfSirketDal : EfEntityRepositoryBase<Sirket, ISGContext>, ISirketDal
    {
        public async Task<ICollection<sirketW>> OrganizationUserList(IEnumerable<int> Userlist, bool Durumu)
        {
            using (var context = new ISGContext())
            {
                return await (from p in context.Sirketler
                              where Userlist.Contains(p.id)  && p.status == Durumu
                              select new sirketW()
                              {
                                  id=p.id,
                                  idRef=p.idRef,
                                  sirketAdi=p.sirketAdi,
                                  status=p.status
                                  
                              }).ToListAsync();
            }
        }
    }
}
/*yetki listesindeki dataları almak için yazıldı*/