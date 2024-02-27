using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IsgEgitimiManager : IIsgEgitimiService
	{
		private IIsgEgitimiDal _isgEgitimiDal;
	 
		public IsgEgitimiManager(IIsgEgitimiDal isgEgitimiDal)
		{
			_isgEgitimiDal = isgEgitimiDal;
		}

		public async Task<IsgEgitimi> GetAsync(int id)
		{
			return await _isgEgitimiDal.GetAsync(id);
		}

		public async Task<ICollection<IsgEgitimi>> GetAllAsync()
		{
			return await _isgEgitimiDal.GetAllAsync();
		}

		public async Task<IsgEgitimi> FindAsync(IsgEgitimi isgEgitimi)
		{
			return await _isgEgitimiDal.FindAsync(p => p.Egitim_Id== isgEgitimi.Egitim_Id);
		}

		public async Task<ICollection<IsgEgitimi>> FindAllAsync(IsgEgitimi isgEgitimi)
		{
            return await _isgEgitimiDal.FindAllAsync(p => p.Personel_Id == isgEgitimi.Egitim_Id);
		}

		public async Task<IsgEgitimi> AddAsync(IsgEgitimi isgEgitimi)
		{
			return await _isgEgitimiDal.AddAsync(isgEgitimi);
		}

		public async Task<IEnumerable<IsgEgitimi>> AddAllAsync(IEnumerable<IsgEgitimi> isgEgitimiList)
		{
			return await _isgEgitimiDal.AddAllAsync(isgEgitimiList);
		}

		public async Task<IsgEgitimi> UpdateAsync(IsgEgitimi isgEgitimi, int key)
		{
			return await _isgEgitimiDal.UpdateAsync(isgEgitimi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _isgEgitimiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _isgEgitimiDal.CountAsync();
		}

        public async Task<bool> SilAsync(int x)
        {
			return await _isgEgitimiDal.SilAsync(x);

		}

		public async Task<object> EgitimAlanPersAsyc(int Sirket_Id, int Year)
        {
			return await _isgEgitimiDal.EgitimAlanPersAsyc(Sirket_Id, Year);
		}
    }
}
