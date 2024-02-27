using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IlacStokManager : IIlacStokService
	{
		private IIlacStokDal _ilacStokDal;
	 
		public IlacStokManager(IIlacStokDal ilacStokDal)
		{
			_ilacStokDal = ilacStokDal;
		}

		public async Task<IlacStok> GetAsync(int id)
		{
			return await _ilacStokDal.GetAsync(id);
		}

		public async Task<ICollection<IlacStok>> GetAllAsync()
		{
			return await _ilacStokDal.GetAllAsync();
		}

		public async Task<IlacStok> FindAsync(IlacStok ilacStok)
		{
			return await _ilacStokDal.FindAsync(p => p.StokId== ilacStok.StokId);
		}

		public async Task<ICollection<IlacStok>> FindAllAsync(IlacStok ilacStok)
		{
			return await _ilacStokDal.FindAllAsync(p => p.SaglikBirimi_Id==ilacStok.SaglikBirimi_Id&&p.Status==ilacStok.Status);
		}

		public async Task<IlacStok> AddAsync(IlacStok ilacStok)
		{
			return await _ilacStokDal.AddAsync(ilacStok);
		}

		public async Task<IEnumerable<IlacStok>> AddAllAsync(IEnumerable<IlacStok> ilacStokList)
		{
			return await _ilacStokDal.AddAllAsync(ilacStokList);
		}

		public async Task<IlacStok> UpdateAsync(IlacStok ilacStok, Guid key)
		{
			return await _ilacStokDal.UpdateAsync(ilacStok, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ilacStokDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ilacStokDal.CountAsync();
		}

        public async Task<ICollection<IlacStoku>> IlacStokHesaplari(int saglikBirimi_Id, bool status)
        {
            return await _ilacStokDal.IlacStokHesaplari(saglikBirimi_Id, status);
        }
    }
}
