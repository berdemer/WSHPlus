using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class ISG_TopluEgitimSablonlariManager : IISG_TopluEgitimSablonlariService
	{
		private IISG_TopluEgitimSablonlariDal _isgTopluEgitimSablonlariDal;
	 
		public ISG_TopluEgitimSablonlariManager(IISG_TopluEgitimSablonlariDal isgTopluEgitimSablonlariDal)
		{
			_isgTopluEgitimSablonlariDal = isgTopluEgitimSablonlariDal;
		}

		public async Task<ISG_TopluEgitimSablonlari> GetAsync(int id)
		{
			return await _isgTopluEgitimSablonlariDal.GetAsync(id);
		}

		public async Task<ICollection<ISG_TopluEgitimSablonlari>> GetAllAsync()
		{
			return await _isgTopluEgitimSablonlariDal.GetAllAsync();
		}

		public async Task<ISG_TopluEgitimSablonlari> FindAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari)
		{
			return await _isgTopluEgitimSablonlariDal.FindAsync(p => p.ISG_TopluEgitimSablonlariId==isgTopluEgitimSablonlari.ISG_TopluEgitimSablonlariId);
		}

		public async Task<ISG_TopluEgitimSablonlari> AddAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari)
		{
			return await _isgTopluEgitimSablonlariDal.AddAsync(isgTopluEgitimSablonlari);
		}

		public async Task<IEnumerable<ISG_TopluEgitimSablonlari>> AddAllAsync(IEnumerable<ISG_TopluEgitimSablonlari> isgTopluEgitimSablonlariList)
		{
			return await _isgTopluEgitimSablonlariDal.AddAllAsync(isgTopluEgitimSablonlariList);
		}

		public async Task<ISG_TopluEgitimSablonlari> UpdateAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari, int key)
		{
			return await _isgTopluEgitimSablonlariDal.UpdateAsync(isgTopluEgitimSablonlari, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _isgTopluEgitimSablonlariDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _isgTopluEgitimSablonlariDal.CountAsync();
		}

        public Task<ICollection<ISG_TopluEgitimSablonlari>> FindAllAsync(ISG_TopluEgitimSablonlari isgTopluEgitimSablonlari)
        {
            throw new System.NotImplementedException();
        }
    }
}
