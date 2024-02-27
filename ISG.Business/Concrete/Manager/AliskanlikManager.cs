using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class AliskanlikManager : IAliskanlikService
	{
		private IAliskanlikDal _aliskanlikDal;
	 
		public AliskanlikManager(IAliskanlikDal aliskanlikDal)
		{
			_aliskanlikDal = aliskanlikDal;
		}

		public async Task<Aliskanlik> GetAsync(int id)
		{
			return await _aliskanlikDal.GetAsync(id);
		}

		public async Task<ICollection<Aliskanlik>> GetAllAsync()
		{
			return await _aliskanlikDal.GetAllAsync();
		}

		public async Task<Aliskanlik> FindAsync(Aliskanlik aliskanlik)
		{
			return await _aliskanlikDal.FindAsync(p => p.Aliskanlik_Id == aliskanlik.Aliskanlik_Id);
		}

		public async Task<ICollection<Aliskanlik>> FindAllAsync(Aliskanlik aliskanlik)
		{
			return await _aliskanlikDal.FindAllAsync(p => p.Personel_Id==aliskanlik.Personel_Id);
		}

		public async Task<Aliskanlik> AddAsync(Aliskanlik aliskanlik)
		{
			return await _aliskanlikDal.AddAsync(aliskanlik);
		}

		public async Task<IEnumerable<Aliskanlik>> AddAllAsync(IEnumerable<Aliskanlik> aliskanlikList)
		{
			return await _aliskanlikDal.AddAllAsync(aliskanlikList);
		}

		public async Task<Aliskanlik> UpdateAsync(Aliskanlik aliskanlik, int key)
		{
			return await _aliskanlikDal.UpdateAsync(aliskanlik, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _aliskanlikDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _aliskanlikDal.CountAsync();
		}
	}
}
