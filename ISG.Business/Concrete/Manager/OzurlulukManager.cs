using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class OzurlulukManager : IOzurlulukService
	{
		private IOzurlulukDal _ozurlulukDal;
	 
		public OzurlulukManager(IOzurlulukDal ozurlulukDal)
		{
			_ozurlulukDal = ozurlulukDal;
		}

		public async Task<Ozurluluk> GetAsync(int id)
		{
			return await _ozurlulukDal.GetAsync(id);
		}

		public async Task<ICollection<Ozurluluk>> GetAllAsync()
		{
			return await _ozurlulukDal.GetAllAsync();
		}

		public async Task<Ozurluluk> FindAsync(Ozurluluk ozurluluk)
		{
			return await _ozurlulukDal.FindAsync(p => p.Ozurluluk_Id == ozurluluk.Ozurluluk_Id);
		}

		public async Task<ICollection<Ozurluluk>> FindAllAsync(Ozurluluk ozurluluk)
		{
			return await _ozurlulukDal.FindAllAsync(p => p.Personel_Id==ozurluluk.Ozurluluk_Id);
		}

		public async Task<Ozurluluk> AddAsync(Ozurluluk ozurluluk)
		{
			return await _ozurlulukDal.AddAsync(ozurluluk);
		}

		public async Task<IEnumerable<Ozurluluk>> AddAllAsync(IEnumerable<Ozurluluk> ozurlulukList)
		{
			return await _ozurlulukDal.AddAllAsync(ozurlulukList);
		}

		public async Task<Ozurluluk> UpdateAsync(Ozurluluk ozurluluk, int key)
		{
			return await _ozurlulukDal.UpdateAsync(ozurluluk, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ozurlulukDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ozurlulukDal.CountAsync();
		}
	}
}
