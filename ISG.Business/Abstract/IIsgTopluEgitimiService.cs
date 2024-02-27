using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIsgTopluEgitimiService
	{

		Task<IsgTopluEgitimi> GetAsync(int id);

		Task<ICollection<IsgTopluEgitimi>> GetAllAsync();

		Task<IsgTopluEgitimi> FindAsync(IsgTopluEgitimi isgTopluEgitimi);

		Task<ICollection<IsgTopluEgitimi>> FindAllAsync(IsgTopluEgitimi isgTopluEgitimi);
		Task<ICollection<IsgTopluEgitimi>> KarsilastirAsync(IsgTopluEgitimi isgTopluEgitimi);
		Task<IsgTopluEgitimi> AddAsync(IsgTopluEgitimi isgTopluEgitimi);

		Task<IEnumerable<IsgTopluEgitimi>> AddAllAsync(IEnumerable<IsgTopluEgitimi> isgTopluEgitimiList);

		Task<IsgTopluEgitimi> UpdateAsync(IsgTopluEgitimi isgTopluEgitimi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

		Task<ICollection<IsgTopluEgitimi>> TopluEgitimListesiAsync(int Sirket_id, int year);

	}
}
