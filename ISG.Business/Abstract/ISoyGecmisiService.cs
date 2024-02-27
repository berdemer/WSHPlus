using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ISoyGecmisiService
	{

		Task<SoyGecmisi> GetAsync(int id);

		Task<ICollection<SoyGecmisi>> GetAllAsync();

		Task<SoyGecmisi> FindAsync(SoyGecmisi soyGecmisi);

		Task<ICollection<SoyGecmisi>> FindAllAsync(SoyGecmisi soyGecmisi);

		Task<SoyGecmisi> AddAsync(SoyGecmisi soyGecmisi);

		Task<IEnumerable<SoyGecmisi>> AddAllAsync(IEnumerable<SoyGecmisi> soyGecmisiList);

		Task<SoyGecmisi> UpdateAsync(SoyGecmisi soyGecmisi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
