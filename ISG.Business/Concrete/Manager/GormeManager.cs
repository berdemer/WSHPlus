using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class GormeManager : IGormeService
	{
		private IGormeDal _gormeDal;
	 
		public GormeManager(IGormeDal gormeDal)
		{
			_gormeDal = gormeDal;
		}

		public async Task<Gorme> GetAsync(int id)
		{
			return await _gormeDal.GetAsync(id);
		}

		public async Task<ICollection<Gorme>> GetAllAsync()
		{
			return await _gormeDal.GetAllAsync();
		}

		public async Task<Gorme> FindAsync(Gorme gorme)
		{
			return await _gormeDal.FindAsync(p => p.Gorme_Id == gorme.Gorme_Id);
		}

		public async Task<ICollection<Gorme>> FindAllAsync(Gorme gorme)
		{
			return await _gormeDal.FindAllAsync(p => p.Personel_Id==gorme.Personel_Id);
		}

		public async Task<Gorme> AddAsync(Gorme gorme)
		{
			return await _gormeDal.AddAsync(gorme);
		}

		public async Task<IEnumerable<Gorme>> AddAllAsync(IEnumerable<Gorme> gormeList)
		{
			return await _gormeDal.AddAllAsync(gormeList);
		}

		public async Task<Gorme> UpdateAsync(Gorme gorme, int key)
		{
			return await _gormeDal.UpdateAsync(gorme, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _gormeDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _gormeDal.CountAsync();
		}
	}
}
