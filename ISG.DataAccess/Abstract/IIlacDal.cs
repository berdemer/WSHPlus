using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface IIlacDal : IEntityRepository<Ilac>
    {
        Task<ICollection<SBView>> SaglikBirimiUserId(string user_Id);
        Task<ICollection<Ilac>> IlacAdiAra(string value);
        Task<KtubKt> KtubKtAra(string Search);
    }
}
