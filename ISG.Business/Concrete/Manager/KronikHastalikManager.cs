using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class KronikHastalikManager : IKronikHastalikService
	{
		private IKronikHastalikDal _kronikHastalikDal;
	 
		public KronikHastalikManager(IKronikHastalikDal kronikHastalikDal)
		{
			_kronikHastalikDal = kronikHastalikDal;
		}

		public async Task<KronikHastalik> GetAsync(int id)
		{
			return await _kronikHastalikDal.GetAsync(id);
		}

		public async Task<ICollection<KronikHastalik>> GetAllAsync()
		{
			return await _kronikHastalikDal.GetAllAsync();
		}

		public async Task<KronikHastalik> FindAsync(KronikHastalik kronikHastalik)
		{
			return await _kronikHastalikDal.FindAsync(p => p.KronikHastalik_Id== kronikHastalik.KronikHastalik_Id);
		}

		public async Task<ICollection<KronikHastalik>> FindAllAsync(KronikHastalik kronikHastalik)
		{
			return await _kronikHastalikDal.FindAllAsync(p => p.Personel_Id==kronikHastalik.Personel_Id);
		}

		public async Task<KronikHastalik> AddAsync(KronikHastalik kronikHastalik)
		{
			return await _kronikHastalikDal.AddAsync(kronikHastalik);
		}

		public async Task<IEnumerable<KronikHastalik>> AddAllAsync(IEnumerable<KronikHastalik> kronikHastalikList)
		{
			return await _kronikHastalikDal.AddAllAsync(kronikHastalikList);
		}

		public async Task<KronikHastalik> UpdateAsync(KronikHastalik kronikHastalik, Guid key)
		{
			return await _kronikHastalikDal.UpdateAsync(kronikHastalik, key);
		}

		public async Task<int> DeleteAsync(Guid key)
		{
			return await _kronikHastalikDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _kronikHastalikDal.CountAsync();
		}
	}
}
