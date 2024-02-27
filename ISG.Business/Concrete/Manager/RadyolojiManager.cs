using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class RadyolojiManager : IRadyolojiService
	{
		private IRadyolojiDal _radyolojiDal;
	 
		public RadyolojiManager(IRadyolojiDal radyolojiDal)
		{
			_radyolojiDal = radyolojiDal;
		}

		public async Task<Radyoloji> GetAsync(int id)
		{
			return await _radyolojiDal.GetAsync(id);
		}

		public async Task<ICollection<Radyoloji>> GetAllAsync()
		{
			return await _radyolojiDal.GetAllAsync();
		}

		public async Task<Radyoloji> FindAsync(Radyoloji radyoloji)
		{
			return await _radyolojiDal.FindAsync(p => p.Radyoloji_Id == radyoloji.Radyoloji_Id);
		}

		public async Task<ICollection<Radyoloji>> FindAllAsync(Radyoloji radyoloji)
		{
			return await _radyolojiDal.FindAllAsync(p => p.Personel_Id==radyoloji.Personel_Id);
		}

		public async Task<Radyoloji> AddAsync(Radyoloji radyoloji)
		{
			return await _radyolojiDal.AddAsync(radyoloji);
		}

		public async Task<IEnumerable<Radyoloji>> AddAllAsync(IEnumerable<Radyoloji> radyolojiList)
		{
			return await _radyolojiDal.AddAllAsync(radyolojiList);
		}

		public async Task<Radyoloji> UpdateAsync(Radyoloji radyoloji, int key)
		{
			return await _radyolojiDal.UpdateAsync(radyoloji, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _radyolojiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _radyolojiDal.CountAsync();
		}
	}
}
