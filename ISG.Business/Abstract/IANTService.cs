using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IANTService
	{

		Task<ANT> GetAsync(int id);

		Task<ICollection<ANT>> GetAllAsync();

		Task<ANT> FindAsync(ANT aNT);

		Task<ICollection<ANT>> FindAllAsync(ANT aNT);

		Task<ANT> AddAsync(ANT aNT);

		Task<IEnumerable<ANT>> AddAllAsync(IEnumerable<ANT> aNTList);

		Task<ANT> UpdateAsync(ANT aNT, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
