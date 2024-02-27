using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IsKazasiManager : IIsKazasiService
	{
		private IIsKazasiDal _isKazasiDal;
	 
		public IsKazasiManager(IIsKazasiDal isKazasiDal)
		{
			_isKazasiDal = isKazasiDal;
		}

		public async Task<IsKazasi> GetAsync(int id)
		{
			return await _isKazasiDal.GetAsync(id);
		}

		public async Task<ICollection<IsKazasi>> GetAllAsync()
		{
			return await _isKazasiDal.GetAllAsync();
		}

		public async Task<IsKazasi> FindAsync(IsKazasi isKazasi)
		{
			return await _isKazasiDal.FindAsync(p => p.IsKazasi_Id == isKazasi.IsKazasi_Id);
		}

		public async Task<ICollection<IsKazasi>> FindAllAsync(IsKazasi isKazasi)
		{
			return await _isKazasiDal.FindAllAsync(p => p.Personel_Id==isKazasi.Personel_Id);
		}

		public async Task<IsKazasi> AddAsync(IsKazasi isKazasi)
		{
			return await _isKazasiDal.AddAsync(isKazasi);
		}

		public async Task<IEnumerable<IsKazasi>> AddAllAsync(IEnumerable<IsKazasi> isKazasiList)
		{
			return await _isKazasiDal.AddAllAsync(isKazasiList);
		}

		public async Task<IsKazasi> UpdateAsync(IsKazasi isKazasi, int key)
		{
			return await _isKazasiDal.UpdateAsync(isKazasi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _isKazasiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _isKazasiDal.CountAsync();
		}
	}
}
