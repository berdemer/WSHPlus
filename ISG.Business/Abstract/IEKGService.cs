using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IEKGService
	{

		Task<EKG> GetAsync(int id);

		Task<ICollection<EKG>> GetAllAsync();

		Task<EKG> FindAsync(EKG ekg);

		Task<ICollection<EKG>> FindAllAsync(EKG ekg);

		Task<EKG> AddAsync(EKG ekg);

		Task<IEnumerable<EKG>> AddAllAsync(IEnumerable<EKG> ekgList);

		Task<EKG> UpdateAsync(EKG ekg, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
