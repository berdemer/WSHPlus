using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface IIlacStokGirisiDal : IEntityRepository<IlacStokGirisi>
    {
        Task<ICollection<IlacStokGirisiView>> IlacStokGirisiView(int SB_Id, bool st);
    }
}
