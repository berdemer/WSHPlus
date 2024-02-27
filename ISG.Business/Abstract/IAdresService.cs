using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IAdresService
	{

		Task<Adres> GetAsync(int id);

		Task<ICollection<Adres>> GetAllAsync();

		Task<Adres> FindAsync(Adres adres);

		Task<ICollection<Adres>> FindAllAsync(Adres adres);

		Task<Adres> AddAsync(Adres adres);

		Task<IEnumerable<Adres>> AddAllAsync(IEnumerable<Adres> adresList);

		Task<Adres> UpdateAsync(Adres adres, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
