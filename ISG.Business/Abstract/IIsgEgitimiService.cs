using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIsgEgitimiService
	{

		Task<IsgEgitimi> GetAsync(int id);

		Task<ICollection<IsgEgitimi>> GetAllAsync();

		Task<IsgEgitimi> FindAsync(IsgEgitimi isgEgitimi);

		Task<ICollection<IsgEgitimi>> FindAllAsync(IsgEgitimi isgEgitimi);

		Task<IsgEgitimi> AddAsync(IsgEgitimi isgEgitimi);

		Task<IEnumerable<IsgEgitimi>> AddAllAsync(IEnumerable<IsgEgitimi> isgEgitimiList);

		Task<IsgEgitimi> UpdateAsync(IsgEgitimi isgEgitimi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<bool> SilAsync(int x);
		Task<object> EgitimAlanPersAsyc(int Sirket_Id, int Year);
	}
}
