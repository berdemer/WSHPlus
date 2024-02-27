using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class RevirTedaviManager : IRevirTedaviService
	{
		private IRevirTedaviDal _revirTedaviDal;
	 
		public RevirTedaviManager(IRevirTedaviDal revirTedaviDal)
		{
			_revirTedaviDal = revirTedaviDal;
		}

		public async Task<RevirTedavi> GetAsync(int id)
		{
			return await _revirTedaviDal.GetAsync(id);
		}

		public async Task<ICollection<RevirTedavi>> GetAllAsync()
		{
			return await _revirTedaviDal.GetAllAsync();
		}

		public async Task<RevirTedavi> FindAsync(RevirTedavi revirTedavi)
		{
			return await _revirTedaviDal.FindAsync(p => p.RevirTedavi_Id == revirTedavi.RevirTedavi_Id);
		}

		public async Task<ICollection<RevirTedavi>> FindAllAsync(RevirTedavi revirTedavi)
		{
			return await _revirTedaviDal.FindAllAsync(p => p.Personel_Id==revirTedavi.Personel_Id);
		}

		public async Task<RevirTedavi> AddAsync(RevirTedavi revirTedavi)
		{
			return await _revirTedaviDal.AddAsync(revirTedavi);
		}

		public async Task<IEnumerable<RevirTedavi>> AddAllAsync(IEnumerable<RevirTedavi> revirTedaviList)
		{
			return await _revirTedaviDal.AddAllAsync(revirTedaviList);
		}

		public async Task<RevirTedavi> UpdateAsync(RevirTedavi revirTedavi, int key)
		{
			return await _revirTedaviDal.UpdateAsync(revirTedavi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _revirTedaviDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _revirTedaviDal.CountAsync();
		}
	}
}
