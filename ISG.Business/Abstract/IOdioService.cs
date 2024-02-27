using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IOdioService
	{

		Task<Odio> GetAsync(int id);

		Task<ICollection<Odio>> GetAllAsync();

		Task<Odio> FindAsync(Odio odio);

		Task<ICollection<Odio>> FindAllAsync(Odio odio);

		Task<Odio> AddAsync(Odio odio);

		Task<IEnumerable<Odio>> AddAllAsync(IEnumerable<Odio> odioList);

		Task<Odio> UpdateAsync(Odio odio, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
