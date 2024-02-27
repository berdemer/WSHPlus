using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IOzurlulukService
	{

		Task<Ozurluluk> GetAsync(int id);

		Task<ICollection<Ozurluluk>> GetAllAsync();

		Task<Ozurluluk> FindAsync(Ozurluluk ozurluluk);

		Task<ICollection<Ozurluluk>> FindAllAsync(Ozurluluk ozurluluk);

		Task<Ozurluluk> AddAsync(Ozurluluk ozurluluk);

		Task<IEnumerable<Ozurluluk>> AddAllAsync(IEnumerable<Ozurluluk> ozurlulukList);

		Task<Ozurluluk> UpdateAsync(Ozurluluk ozurluluk, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
