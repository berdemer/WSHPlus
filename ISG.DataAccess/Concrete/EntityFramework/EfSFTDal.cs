using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfSFTDal : EfEntityRepositoryBase<SFT, ISGContext>, ISFTDal
	{

	}
}
