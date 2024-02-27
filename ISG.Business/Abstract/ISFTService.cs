using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ISFTService
	{

		Task<SFT> GetAsync(int id);

		Task<ICollection<SFT>> GetAllAsync();

		Task<SFT> FindAsync(SFT sFT);

		Task<ICollection<SFT>> FindAllAsync(SFT sFT);

		Task<SFT> AddAsync(SFT sFT);

		Task<IEnumerable<SFT>> AddAllAsync(IEnumerable<SFT> sFTList);

		Task<SFT> UpdateAsync(SFT sFT, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
