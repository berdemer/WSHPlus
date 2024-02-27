using ISG.Entities.Concrete;
using System;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IDisRaporDal:IEntityRepository<DisRapor>
	{
		Task<DisRapor> MukerrerDisRaporKontrol(DateTime baslangic, int Personel_Id);
		Task<bool> MukerrerDisRaporTakibi(DateTime baslangic, DateTime bitis, int Personel_Id);
	}
}
