using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ICalisma_DurumuService
	{

		Task<Calisma_Durumu> GetAsync(int id);

		Task<ICollection<Calisma_Durumu>> GetAllAsync();

		Task<Calisma_Durumu> FindAsync(Calisma_Durumu calisma_Durumu);

		Task<ICollection<Calisma_Durumu>> FindAllAsync(Calisma_Durumu calisma_Durumu);

		Task<Calisma_Durumu> AddAsync(Calisma_Durumu calisma_Durumu);

		Task<IEnumerable<Calisma_Durumu>> AddAllAsync(IEnumerable<Calisma_Durumu> calisma_DurumuList);

		Task<Calisma_Durumu> UpdateAsync(Calisma_Durumu calisma_Durumu, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
