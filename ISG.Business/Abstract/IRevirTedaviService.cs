using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IRevirTedaviService
	{

		Task<RevirTedavi> GetAsync(int id);

		Task<ICollection<RevirTedavi>> GetAllAsync();

		Task<RevirTedavi> FindAsync(RevirTedavi revirTedavi);

		Task<ICollection<RevirTedavi>> FindAllAsync(RevirTedavi revirTedavi);

		Task<RevirTedavi> AddAsync(RevirTedavi revirTedavi);

		Task<IEnumerable<RevirTedavi>> AddAllAsync(IEnumerable<RevirTedavi> revirTedaviList);

		Task<RevirTedavi> UpdateAsync(RevirTedavi revirTedavi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
