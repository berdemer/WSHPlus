using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class EgitimHayatiManager : IEgitimHayatiService
	{
		private IEgitimHayatiDal _egitimHayatiDal;
	 
		public EgitimHayatiManager(IEgitimHayatiDal egitimHayatiDal)
		{
			_egitimHayatiDal = egitimHayatiDal;
		}

		public async Task<EgitimHayati> GetAsync(int id)
		{
			return await _egitimHayatiDal.GetAsync(id);
		}

		public async Task<ICollection<EgitimHayati>> GetAllAsync()
		{
			return await _egitimHayatiDal.GetAllAsync();
		}

		public async Task<EgitimHayati> FindAsync(EgitimHayati egitimHayati)
		{
			return await _egitimHayatiDal.FindAsync(p => p.EgitimHayati_Id == egitimHayati.EgitimHayati_Id);
		}

		public async Task<ICollection<EgitimHayati>> FindAllAsync(EgitimHayati egitimHayati)
		{
			return await _egitimHayatiDal.FindAllAsync(p => p.Personel_Id==egitimHayati.EgitimHayati_Id);
		}

		public async Task<EgitimHayati> AddAsync(EgitimHayati egitimHayati)
		{
			return await _egitimHayatiDal.AddAsync(egitimHayati);
		}

		public async Task<IEnumerable<EgitimHayati>> AddAllAsync(IEnumerable<EgitimHayati> egitimHayatiList)
		{
			return await _egitimHayatiDal.AddAllAsync(egitimHayatiList);
		}

		public async Task<EgitimHayati> UpdateAsync(EgitimHayati egitimHayati, int key)
		{
			return await _egitimHayatiDal.UpdateAsync(egitimHayati, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _egitimHayatiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _egitimHayatiDal.CountAsync();
		}
	}
}
