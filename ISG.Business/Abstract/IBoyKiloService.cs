using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IBoyKiloService
	{

		Task<BoyKilo> GetAsync(int id);

		Task<ICollection<BoyKilo>> GetAllAsync();

		Task<BoyKilo> FindAsync(BoyKilo boyKilo);

		Task<ICollection<BoyKilo>> FindAllAsync(BoyKilo boyKilo);

		Task<BoyKilo> AddAsync(BoyKilo boyKilo);

		Task<IEnumerable<BoyKilo>> AddAllAsync(IEnumerable<BoyKilo> boyKiloList);

		Task<BoyKilo> UpdateAsync(BoyKilo boyKilo, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
