using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIkazService
	{

		Task<Ikaz> GetAsync(int id);

		Task<ICollection<Ikaz>> GetAllAsync();

		Task<Ikaz> FindAsync(Ikaz ikaz);

		Task<ICollection<Ikaz>> FindAllAsync(Ikaz ikaz);

		Task<Ikaz> AddAsync(Ikaz ikaz);

		Task<IEnumerable<Ikaz>> AddAllAsync(IEnumerable<Ikaz> ikazList);

		Task<Ikaz> UpdateAsync(Ikaz ikaz, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

        Task<ICollection<Ikaz>> FindTumuAsync(Ikaz ikaz);

    }
}
