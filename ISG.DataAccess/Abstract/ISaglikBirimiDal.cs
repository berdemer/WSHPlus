using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface ISaglikBirimiDal:IEntityRepository<SaglikBirimi>
    {
        Task<SaglikBirimi> ProtokolAl(int id);
    }
}
