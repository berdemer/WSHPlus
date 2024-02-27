using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IAliskanlikService
	{

		Task<Aliskanlik> GetAsync(int id);

		Task<ICollection<Aliskanlik>> GetAllAsync();

		Task<Aliskanlik> FindAsync(Aliskanlik aliskanlik);

		Task<ICollection<Aliskanlik>> FindAllAsync(Aliskanlik aliskanlik);

		Task<Aliskanlik> AddAsync(Aliskanlik aliskanlik);

		Task<IEnumerable<Aliskanlik>> AddAllAsync(IEnumerable<Aliskanlik> aliskanlikList);

		Task<Aliskanlik> UpdateAsync(Aliskanlik aliskanlik, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
