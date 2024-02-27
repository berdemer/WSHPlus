using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IISG_TopluEgitimSablonlariService
	{

		Task<ISG_TopluEgitimSablonlari> GetAsync(int id);

		Task<ICollection<ISG_TopluEgitimSablonlari>> GetAllAsync();

		Task<ISG_TopluEgitimSablonlari> FindAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari);

		Task<ICollection<ISG_TopluEgitimSablonlari>> FindAllAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari);

		Task<ISG_TopluEgitimSablonlari> AddAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari);

		Task<IEnumerable<ISG_TopluEgitimSablonlari>> AddAllAsync(IEnumerable<ISG_TopluEgitimSablonlari> isgTopluEgitimSablonlariList);

		Task<ISG_TopluEgitimSablonlari> UpdateAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
