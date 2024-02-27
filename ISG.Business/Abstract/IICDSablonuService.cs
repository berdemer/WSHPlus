using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IICDSablonuService
	{

		Task<ICDSablonu> GetAsync(int id);

		Task<ICollection<ICDSablonu>> GetAllAsync();

		Task<ICDSablonu> FindAsync(ICDSablonu iCDSablonu);

		Task<ICollection<ICDSablonu>> FindAllAsync(ICDSablonu iCDSablonu);

		Task<ICDSablonu> AddAsync(ICDSablonu iCDSablonu);

		Task<IEnumerable<ICDSablonu>> AddAllAsync(IEnumerable<ICDSablonu> iCDSablonuList);

		Task<ICDSablonu> UpdateAsync(ICDSablonu iCDSablonu, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<ICDSablonu> Kontrol(string ICDkod, string UserId);

	}
}
