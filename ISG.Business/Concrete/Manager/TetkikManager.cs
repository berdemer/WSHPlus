using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class TetkikManager : ITetkikService
	{
		private ITetkikDal _tetkikDal;
	 
		public TetkikManager(ITetkikDal tetkikDal)
		{
			_tetkikDal = tetkikDal;
		}

		public async Task<Tetkik> GetAsync(int id)
		{
			return await _tetkikDal.GetAsync(id);
		}

		public async Task<ICollection<Tetkik>> GetAllAsync()
		{
			return await _tetkikDal.GetAllAsync();
		}

		public async Task<Tetkik> FindAsync(Tetkik tetkik)
		{
			return await _tetkikDal.FindAsync(p => p.Tetkik_Id == tetkik.Tetkik_Id);
		}

		public async Task<ICollection<Tetkik>> FindAllAsync(Tetkik tetkik)
		{
			return await _tetkikDal.FindAllAsync(p => p.Tetkik_Id==tetkik.Tetkik_Id);
		}

		public async Task<Tetkik> AddAsync(Tetkik tetkik)
		{
			return await _tetkikDal.AddAsync(tetkik);
		}

		public async Task<IEnumerable<Tetkik>> AddAllAsync(IEnumerable<Tetkik> tetkikList)
		{
			return await _tetkikDal.AddAllAsync(tetkikList);
		}

		public async Task<Tetkik> UpdateAsync(Tetkik tetkik, int key)
		{
			return await _tetkikDal.UpdateAsync(tetkik, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _tetkikDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _tetkikDal.CountAsync();
		}
	}
}
