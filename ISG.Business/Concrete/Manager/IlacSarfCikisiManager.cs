using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IlacSarfCikisiManager : IIlacSarfCikisiService
	{
		private IIlacSarfCikisiDal _ilacSarfCikisiDal;
	 
		public IlacSarfCikisiManager(IIlacSarfCikisiDal ilacSarfCikisiDal)
		{
			_ilacSarfCikisiDal = ilacSarfCikisiDal;
		}

		public async Task<IlacSarfCikisi> GetAsync(Guid id)
		{
			return await _ilacSarfCikisiDal.GetAsync(id);
		}

		public async Task<ICollection<IlacSarfCikisi>> GetAllAsync()
		{
			return await _ilacSarfCikisiDal.GetAllAsync();
		}

		public async Task<IlacSarfCikisi> FindAsync(IlacSarfCikisi ilacSarfCikisi)
		{
			return await _ilacSarfCikisiDal.FindAsync(p => p.IlacSarfCikisi_Id== ilacSarfCikisi.IlacSarfCikisi_Id);
		}

		public async Task<ICollection<IlacSarfCikisi>> FindAllAsync(IlacSarfCikisi ilacSarfCikisi)
		{
			return await _ilacSarfCikisiDal.FindAllAsync(p => p.RevirTedavi_Id==ilacSarfCikisi.RevirTedavi_Id);
		}

		public async Task<IlacSarfCikisi> AddAsync(IlacSarfCikisi ilacSarfCikisi)
		{
			return await _ilacSarfCikisiDal.AddAsync(ilacSarfCikisi);
		}

		public async Task<IEnumerable<IlacSarfCikisi>> AddAllAsync(IEnumerable<IlacSarfCikisi> ilacSarfCikisiList)
		{
			return await _ilacSarfCikisiDal.AddAllAsync(ilacSarfCikisiList);
		}

		public async Task<IlacSarfCikisi> UpdateAsync(IlacSarfCikisi ilacSarfCikisi, Guid key)
		{
			return await _ilacSarfCikisiDal.UpdateAsync(ilacSarfCikisi, key);
		}

		public async Task<int> DeleteAsync(Guid key)
		{
			return await _ilacSarfCikisiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ilacSarfCikisiDal.CountAsync();
		}
	}
}
