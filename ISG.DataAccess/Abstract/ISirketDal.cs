using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface ISirketDal:IEntityRepository<Sirket>
    {
        Task<ICollection<sirketW>> OrganizationUserList(IEnumerable<int> Userlist, bool Durumu);
    }
}
