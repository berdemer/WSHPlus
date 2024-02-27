using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIsKazasiService
	{

		Task<IsKazasi> GetAsync(int id);

		Task<ICollection<IsKazasi>> GetAllAsync();

		Task<IsKazasi> FindAsync(IsKazasi isKazasi);

		Task<ICollection<IsKazasi>> FindAllAsync(IsKazasi isKazasi);

		Task<IsKazasi> AddAsync(IsKazasi isKazasi);

		Task<IEnumerable<IsKazasi>> AddAllAsync(IEnumerable<IsKazasi> isKazasiList);

		Task<IsKazasi> UpdateAsync(IsKazasi isKazasi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
