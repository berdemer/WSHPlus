using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ICalisma_GecmisiService
	{

		Task<Calisma_Gecmisi> GetAsync(int id);

		Task<ICollection<Calisma_Gecmisi>> GetAllAsync();

		Task<Calisma_Gecmisi> FindAsync(Calisma_Gecmisi calisma_Gecmisi);

		Task<ICollection<Calisma_Gecmisi>> FindAllAsync(Calisma_Gecmisi calisma_Gecmisi);

		Task<Calisma_Gecmisi> AddAsync(Calisma_Gecmisi calisma_Gecmisi);

		Task<IEnumerable<Calisma_Gecmisi>> AddAllAsync(IEnumerable<Calisma_Gecmisi> calisma_GecmisiList);

		Task<Calisma_Gecmisi> UpdateAsync(Calisma_Gecmisi calisma_Gecmisi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
