using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IMeslekHastaliklariService
	{

		Task<MeslekHastaliklari> GetAsync(int id);

		Task<ICollection<MeslekHastaliklari>> GetAllAsync();

		Task<MeslekHastaliklari> FindAsync(MeslekHastaliklari meslekHastaliklari);

		Task<ICollection<MeslekHastaliklari>> FindAllAsync(MeslekHastaliklari meslekHastaliklari);

		Task<MeslekHastaliklari> AddAsync(MeslekHastaliklari meslekHastaliklari);

		Task<IEnumerable<MeslekHastaliklari>> AddAllAsync(IEnumerable<MeslekHastaliklari> meslekHastaliklariList);

		Task<MeslekHastaliklari> UpdateAsync(MeslekHastaliklari meslekHastaliklari, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<ICollection<MeslekHastaliklari>> MeslekHastalikAra(string value);

		Task<ICollection<MeslekHastaliklari>> GrupList(IEnumerable<string> grupList);
  
	}
}
