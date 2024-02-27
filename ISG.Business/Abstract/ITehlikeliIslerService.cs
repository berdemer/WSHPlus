using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ITehlikeliIslerService
	{

		Task<TehlikeliIsler> GetAsync(int id);

		Task<ICollection<TehlikeliIsler>> GetAllAsync();

		Task<TehlikeliIsler> FindAsync(TehlikeliIsler tehlikeliIsler);

		Task<ICollection<TehlikeliIsler>> FindAllAsync(TehlikeliIsler tehlikeliIsler);

		Task<TehlikeliIsler> AddAsync(TehlikeliIsler tehlikeliIsler);

		Task<IEnumerable<TehlikeliIsler>> AddAllAsync(IEnumerable<TehlikeliIsler> tehlikeliIslerList);

		Task<TehlikeliIsler> UpdateAsync(TehlikeliIsler tehlikeliIsler, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

		Task<ICollection<TehlikeliIsler>> GrupList(IEnumerable<string> grupList);
		Task<ICollection<TehlikeliIsler>> TehlikeliIslerAra(string value);
        Task<ICollection<string>> gruplardanAra(string value);

    }
}
