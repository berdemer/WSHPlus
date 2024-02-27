using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IAllerjiService
	{

		Task<Allerji> GetAsync(int id);

		Task<ICollection<Allerji>> GetAllAsync();

		Task<Allerji> FindAsync(Allerji allerji);

		Task<ICollection<Allerji>> FindAllAsync(Allerji allerji);

		Task<Allerji> AddAsync(Allerji allerji);

		Task<IEnumerable<Allerji>> AddAllAsync(IEnumerable<Allerji> allerjiList);

		Task<Allerji> UpdateAsync(Allerji allerji, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
