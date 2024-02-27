using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class KkdManager : IKkdService
	{
		private IKkdDal _kkdDal;
	 
		public KkdManager(IKkdDal kkdDal)
		{
			_kkdDal = kkdDal;
		}

		public async Task<Kkd> GetAsync(int id)
		{
			return await _kkdDal.GetAsync(id);
		}

		public async Task<ICollection<Kkd>> GetAllAsync()
		{
			return await _kkdDal.GetAllAsync();
		}

		public async Task<Kkd> FindAsync(Kkd kkd)
		{
			return await _kkdDal.FindAsync(p => p.Kkd_Id == kkd.Kkd_Id);
		}

		public async Task<ICollection<Kkd>> FindAllAsync(Kkd kkd)
		{
			return await _kkdDal.FindAllAsync(p => p.Personel_Id==kkd.Personel_Id);
		}

		public async Task<Kkd> AddAsync(Kkd kkd)
		{
			return await _kkdDal.AddAsync(kkd);
		}

		public async Task<IEnumerable<Kkd>> AddAllAsync(IEnumerable<Kkd> kkdList)
		{
			return await _kkdDal.AddAllAsync(kkdList);
		}

		public async Task<Kkd> UpdateAsync(Kkd kkd, int key)
		{
			return await _kkdDal.UpdateAsync(kkd, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _kkdDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _kkdDal.CountAsync();
		}
	}
}
