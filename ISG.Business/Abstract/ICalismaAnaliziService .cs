using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface ICalismaAnaliziService
	{

		Task<CalismaAnalizi> GetAsync(int id);

		Task<ICollection<CalismaAnalizi>> GetAllAsync();

		Task<CalismaAnalizi> FindAsync(CalismaAnalizi calismaAnalizi);

		Task<ICollection<CalismaAnalizi>> FindAllAsync(CalismaAnalizi calismaAnalizi);

		Task<CalismaAnalizi> AddAsync(CalismaAnalizi calismaAnalizi);

		Task<IEnumerable<CalismaAnalizi>> AddAllAsync(IEnumerable<CalismaAnalizi> calismaAnaliziList);

		Task<CalismaAnalizi> UpdateAsync(CalismaAnalizi calismaAnalizi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<object> MeslekListesi();
		Task<CalismaAnalizi> BolumAraAsync(int blmId, int stiId);
		Task<CalismaAnalizi> MeslekAraAsync(string Meslek_Kodu);
	}
}
