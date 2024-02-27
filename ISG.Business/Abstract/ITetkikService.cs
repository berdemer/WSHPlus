using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ITetkikService
	{

		Task<Tetkik> GetAsync(int id);

		Task<ICollection<Tetkik>> GetAllAsync();

		Task<Tetkik> FindAsync(Tetkik tetkik);

		Task<ICollection<Tetkik>> FindAllAsync(Tetkik tetkik);

		Task<Tetkik> AddAsync(Tetkik tetkik);

		Task<IEnumerable<Tetkik>> AddAllAsync(IEnumerable<Tetkik> tetkikList);

		Task<Tetkik> UpdateAsync(Tetkik tetkik, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
