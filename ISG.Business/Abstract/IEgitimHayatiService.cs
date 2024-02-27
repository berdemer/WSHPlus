using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IEgitimHayatiService
	{

		Task<EgitimHayati> GetAsync(int id);

		Task<ICollection<EgitimHayati>> GetAllAsync();

		Task<EgitimHayati> FindAsync(EgitimHayati egitimHayati);

		Task<ICollection<EgitimHayati>> FindAllAsync(EgitimHayati egitimHayati);

		Task<EgitimHayati> AddAsync(EgitimHayati egitimHayati);

		Task<IEnumerable<EgitimHayati>> AddAllAsync(IEnumerable<EgitimHayati> egitimHayatiList);

		Task<EgitimHayati> UpdateAsync(EgitimHayati egitimHayati, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
