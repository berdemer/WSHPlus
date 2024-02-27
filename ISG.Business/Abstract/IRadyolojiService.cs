using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IRadyolojiService
	{

		Task<Radyoloji> GetAsync(int id);

		Task<ICollection<Radyoloji>> GetAllAsync();

		Task<Radyoloji> FindAsync(Radyoloji radyoloji);

		Task<ICollection<Radyoloji>> FindAllAsync(Radyoloji radyoloji);

		Task<Radyoloji> AddAsync(Radyoloji radyoloji);

		Task<IEnumerable<Radyoloji>> AddAllAsync(IEnumerable<Radyoloji> radyolojiList);

		Task<Radyoloji> UpdateAsync(Radyoloji radyoloji, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
