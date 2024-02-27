using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface IIlacStokDal:IEntityRepository<IlacStok>
    {
       Task<ICollection<IlacStoku>> IlacStokHesaplari(int saglikBirimi_Id, bool status);
    }
}
