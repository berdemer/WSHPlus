using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class AsiManager : IAsiService
	{
		private IAsiDal _asiDal;
	 
		public AsiManager(IAsiDal asiDal)
		{
			_asiDal = asiDal;
		}

		public async Task<Asi> GetAsync(int id)
		{
			return await _asiDal.GetAsync(id);
		}

		public async Task<ICollection<Asi>> GetAllAsync()
		{
			return await _asiDal.GetAllAsync();
		}

		public async Task<Asi> FindAsync(Asi asi)
		{
			return await _asiDal.FindAsync(p => p.Asi_Id == asi.Asi_Id);
		}

		public async Task<ICollection<Asi>> FindAllAsync(Asi asi)
		{
			return await _asiDal.FindAllAsync(p => p.Personel_Id==asi.Personel_Id);
		}

		public async Task<Asi> AddAsync(Asi asi)
		{
			return await _asiDal.AddAsync(asi);
		}

		public async Task<IEnumerable<Asi>> AddAllAsync(IEnumerable<Asi> asiList)
		{
			return await _asiDal.AddAllAsync(asiList);
		}

		public async Task<Asi> UpdateAsync(Asi asi, int key)
		{
			return await _asiDal.UpdateAsync(asi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _asiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _asiDal.CountAsync();
		}

        public async Task<Asi> MukerrerAsiKontrolu(DateTime Yapilma_Tarihi, int Personel_Id)
        {
			return await _asiDal.MukerrerAsiKontrolu(Yapilma_Tarihi, Personel_Id);
        }
    }
}
