using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IsgTopluEgitimiManager : IIsgTopluEgitimiService
	{
		private IIsgTopluEgitimiDal _isgTopluEgitimiDal;
	 
		public IsgTopluEgitimiManager(IIsgTopluEgitimiDal isgTopluEgitimiDal)
		{
			_isgTopluEgitimiDal = isgTopluEgitimiDal;
		}

		public async Task<IsgTopluEgitimi> GetAsync(int id)
		{
			return await _isgTopluEgitimiDal.GetAsync(id);
		}

		public async Task<ICollection<IsgTopluEgitimi>> GetAllAsync()
		{
			return await _isgTopluEgitimiDal.GetAllAsync();
		}

		public async Task<IsgTopluEgitimi> FindAsync(IsgTopluEgitimi isgTopluEgitimi)
		{
			return await _isgTopluEgitimiDal.FindAsync(p => p.IsgTopluEgitimi_Id== isgTopluEgitimi.IsgTopluEgitimi_Id);
		}

		public async Task<ICollection<IsgTopluEgitimi>> FindAllAsync(IsgTopluEgitimi isgTopluEgitimi)
		{
			return await _isgTopluEgitimiDal.FindAllAsync(p => p.Sirket_id==isgTopluEgitimi.Sirket_id);
		}

		public async Task<ICollection<IsgTopluEgitimi>> TopluEgitimListesiAsync(int Sirket_id,int year)
		{
			return await _isgTopluEgitimiDal.FindAllAsync(p => p.Sirket_id == Sirket_id && p.egitimTarihi.Value.Year== year);
		}

		public async Task<IsgTopluEgitimi> AddAsync(IsgTopluEgitimi isgTopluEgitimi)
		{
			return await _isgTopluEgitimiDal.AddAsync(isgTopluEgitimi);
		}

		public async Task<IEnumerable<IsgTopluEgitimi>> AddAllAsync(IEnumerable<IsgTopluEgitimi> isgTopluEgitimiList)
		{
			return await _isgTopluEgitimiDal.AddAllAsync(isgTopluEgitimiList);
		}

		public async Task<IsgTopluEgitimi> UpdateAsync(IsgTopluEgitimi isgTopluEgitimi, int key)
		{
			return await _isgTopluEgitimiDal.UpdateAsync(isgTopluEgitimi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _isgTopluEgitimiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _isgTopluEgitimiDal.CountAsync();
		}

        public async Task<ICollection<IsgTopluEgitimi>> KarsilastirAsync(IsgTopluEgitimi c)
        {
			return await _isgTopluEgitimiDal.FindAllAsync(p => p.isgProfTckNo==c.isgProfTckNo&&p.egitimTarihi==c.egitimTarihi&&p.Sirket_id==c.Sirket_id);
		}
    }
}
