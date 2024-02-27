using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class ICDSablonuManager : IICDSablonuService
	{
		private IICDSablonuDal _iCDSablonuDal;
	 
		public ICDSablonuManager(IICDSablonuDal iCDSablonuDal)
		{
			_iCDSablonuDal = iCDSablonuDal;
		}

		public async Task<ICDSablonu> GetAsync(int id)
		{
			return await _iCDSablonuDal.GetAsync(id);
		}

		public async Task<ICollection<ICDSablonu>> GetAllAsync()
		{
			return await _iCDSablonuDal.GetAllAsync();
		}

		public async Task<ICDSablonu> FindAsync(ICDSablonu iCDSablonu)
		{
			return await _iCDSablonuDal.FindAsync(p => p.ICDSablonu_Id== iCDSablonu.ICDSablonu_Id);
		}

		public async Task<ICollection<ICDSablonu>> FindAllAsync(ICDSablonu iCDSablonu)
		{
			return await _iCDSablonuDal.FindAllAsync(p => p.ICDkod==iCDSablonu.ICDkod);
		}

		public async Task<ICDSablonu> AddAsync(ICDSablonu iCDSablonu)
		{
			return await _iCDSablonuDal.AddAsync(iCDSablonu);
		}

		public async Task<IEnumerable<ICDSablonu>> AddAllAsync(IEnumerable<ICDSablonu> iCDSablonuList)
		{
			return await _iCDSablonuDal.AddAllAsync(iCDSablonuList);
		}

		public async Task<ICDSablonu> UpdateAsync(ICDSablonu iCDSablonu, int key)
		{
			return await _iCDSablonuDal.UpdateAsync(iCDSablonu, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _iCDSablonuDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _iCDSablonuDal.CountAsync();
		}

		public async Task<ICDSablonu> Kontrol(string ICDkod, string UserId)
		{
			return await _iCDSablonuDal.FindAsync(p => p.ICDkod == ICDkod&&p.UserId==UserId);
		}
	}
}
