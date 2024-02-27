using ISG.Entities.Concrete;
using System;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IAsiDal:IEntityRepository<Asi>
	{
		Task<Asi> MukerrerAsiKontrolu(DateTime Yapilma_Tarihi, int Personel_Id);
	}
}
