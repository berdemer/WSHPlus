using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfHemogramDal : EfEntityRepositoryBase<Hemogram, ISGContext>, IHemogramDal
	{

	}
}
