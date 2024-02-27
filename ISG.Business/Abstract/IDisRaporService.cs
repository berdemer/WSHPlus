using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IDisRaporService
	{

		Task<DisRapor> GetAsync(int id);

		Task<ICollection<DisRapor>> GetAllAsync();

		Task<DisRapor> FindAsync(DisRapor disRapor);

		Task<ICollection<DisRapor>> FindAllAsync(DisRapor disRapor);

		Task<DisRapor> AddAsync(DisRapor disRapor);

		Task<IEnumerable<DisRapor>> AddAllAsync(IEnumerable<DisRapor> disRaporList);

		Task<DisRapor> UpdateAsync(DisRapor disRapor, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

		Task<DisRapor> MukerrerDisRaporKontrol(DateTime baslangic, int Personel_Id);
		Task<bool> MukerrerDisRaporTakibi(DateTime baslangic, DateTime bitis, int Personel_Id);
	}
}
