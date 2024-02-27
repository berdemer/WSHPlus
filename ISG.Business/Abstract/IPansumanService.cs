using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPansumanService
	{

		Task<Pansuman> GetAsync(int id);

		Task<ICollection<Pansuman>> GetAllAsync();

		Task<Pansuman> FindAsync(Pansuman pansuman);

		Task<ICollection<Pansuman>> FindAllAsync(Pansuman pansuman);

		Task<Pansuman> AddAsync(Pansuman pansuman);

		Task<IEnumerable<Pansuman>> AddAllAsync(IEnumerable<Pansuman> pansumanList);

		Task<Pansuman> UpdateAsync(Pansuman pansuman, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
