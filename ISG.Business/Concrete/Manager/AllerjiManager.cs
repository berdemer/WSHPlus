using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class AllerjiManager : IAllerjiService
	{
		private IAllerjiDal _allerjiDal;
	 
		public AllerjiManager(IAllerjiDal allerjiDal)
		{
			_allerjiDal = allerjiDal;
		}

		public async Task<Allerji> GetAsync(int id)
		{
			return await _allerjiDal.GetAsync(id);
		}

		public async Task<ICollection<Allerji>> GetAllAsync()
		{
			return await _allerjiDal.GetAllAsync();
		}

		public async Task<Allerji> FindAsync(Allerji allerji)
		{
			return await _allerjiDal.FindAsync(p => p.Personel_Id == allerji.Allerji_Id);
		}

		public async Task<ICollection<Allerji>> FindAllAsync(Allerji allerji)
		{
			return await _allerjiDal.FindAllAsync(p => p.Personel_Id==allerji.Personel_Id);
		}

		public async Task<Allerji> AddAsync(Allerji allerji)
		{
			return await _allerjiDal.AddAsync(allerji);
		}

		public async Task<IEnumerable<Allerji>> AddAllAsync(IEnumerable<Allerji> allerjiList)
		{
			return await _allerjiDal.AddAllAsync(allerjiList);
		}

		public async Task<Allerji> UpdateAsync(Allerji allerji, int key)
		{
			return await _allerjiDal.UpdateAsync(allerji, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _allerjiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _allerjiDal.CountAsync();
		}
	}
}
