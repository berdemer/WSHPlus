using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IKkdService
	{

		Task<Kkd> GetAsync(int id);

		Task<ICollection<Kkd>> GetAllAsync();

		Task<Kkd> FindAsync(Kkd kkd);

		Task<ICollection<Kkd>> FindAllAsync(Kkd kkd);

		Task<Kkd> AddAsync(Kkd kkd);

		Task<IEnumerable<Kkd>> AddAllAsync(IEnumerable<Kkd> kkdList);

		Task<Kkd> UpdateAsync(Kkd kkd, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
