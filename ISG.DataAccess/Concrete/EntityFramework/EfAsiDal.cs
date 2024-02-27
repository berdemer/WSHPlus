using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfAsiDal : EfEntityRepositoryBase<Asi, ISGContext>, IAsiDal
	{
		public async Task<Asi> MukerrerAsiKontrolu(DateTime Yapilma_Tarihi, int Personel_Id)
		{
			using (var context = new ISGContext())
			{
				return await context.Set<Asi>().Where(p => p.Yapilma_Tarihi == Yapilma_Tarihi.Date && p.Personel_Id == Personel_Id).FirstOrDefaultAsync();
			}
		}
	}
}
