using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IkazManager : IIkazService
	{
		private IIkazDal _ikazDal;
	 
		public IkazManager(IIkazDal ikazDal)
		{
			_ikazDal = ikazDal;
		}

		public async Task<Ikaz> GetAsync(int id)
		{
			return await _ikazDal.GetAsync(id);
		}

		public async Task<ICollection<Ikaz>> GetAllAsync()
		{
			return await _ikazDal.GetAllAsync();
		}

		public async Task<Ikaz> FindAsync(Ikaz ikaz)//ikaz uyarý olanlarý göster.
		{
			return await _ikazDal.FindAsync(p => p.Personel_Id == ikaz.Personel_Id && p.Status==ikaz.Status);
		}

		public async Task<ICollection<Ikaz>> FindAllAsync(Ikaz ikaz)
		{
			return await _ikazDal.FindAllAsync(p => p.Personel_Id==ikaz.Personel_Id && p.Status == ikaz.Status && p.SonTarih<ikaz.SonTarih);
		}

        public async Task<ICollection<Ikaz>> FindTumuAsync(Ikaz ikaz)
        {
            return await _ikazDal.FindAllAsync(p => p.Personel_Id == ikaz.Personel_Id);
        }

        public async Task<Ikaz> AddAsync(Ikaz ikaz)
		{
			return await _ikazDal.AddAsync(ikaz);
		}

		public async Task<IEnumerable<Ikaz>> AddAllAsync(IEnumerable<Ikaz> ikazList)
		{
			return await _ikazDal.AddAllAsync(ikazList);
		}

		public async Task<Ikaz> UpdateAsync(Ikaz ikaz, int key)
		{
			return await _ikazDal.UpdateAsync(ikaz, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ikazDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ikazDal.CountAsync();
		}
    }
}
