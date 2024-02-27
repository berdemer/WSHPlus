using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfAliskanlikDal : EfEntityRepositoryBase<Aliskanlik, ISGContext>, IAliskanlikDal
	{

	}
}
