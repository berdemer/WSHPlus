using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IGormeService
	{

		Task<Gorme> GetAsync(int id);

		Task<ICollection<Gorme>> GetAllAsync();

		Task<Gorme> FindAsync(Gorme gorme);

		Task<ICollection<Gorme>> FindAllAsync(Gorme gorme);

		Task<Gorme> AddAsync(Gorme gorme);

		Task<IEnumerable<Gorme>> AddAllAsync(IEnumerable<Gorme> gormeList);

		Task<Gorme> UpdateAsync(Gorme gorme, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
