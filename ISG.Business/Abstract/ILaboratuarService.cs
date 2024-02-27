using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ILaboratuarService
	{

		Task<Laboratuar> GetAsync(int id);

		Task<ICollection<Laboratuar>> GetAllAsync();

		Task<Laboratuar> FindAsync(Laboratuar laboratuar);

		Task<ICollection<Laboratuar>> FindAllAsync(Laboratuar laboratuar);

		Task<Laboratuar> AddAsync(Laboratuar laboratuar);

		Task<IEnumerable<Laboratuar>> AddAllAsync(IEnumerable<Laboratuar> laboratuarList);

		Task<Laboratuar> UpdateAsync(Laboratuar laboratuar, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
