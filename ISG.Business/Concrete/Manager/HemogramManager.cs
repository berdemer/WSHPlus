using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class HemogramManager : IHemogramService
	{
		private IHemogramDal _hemogramDal;
	 
		public HemogramManager(IHemogramDal hemogramDal)
		{
			_hemogramDal = hemogramDal;
		}

		public async Task<Hemogram> GetAsync(int id)
		{
			return await _hemogramDal.GetAsync(id);
		}

		public async Task<ICollection<Hemogram>> GetAllAsync()
		{
			return await _hemogramDal.GetAllAsync();
		}

		public async Task<Hemogram> FindAsync(Hemogram hemogram)
		{
			return await _hemogramDal.FindAsync(p => p.Hemogram_Id == hemogram.Hemogram_Id);
		}

		public async Task<ICollection<Hemogram>> FindAllAsync(Hemogram hemogram)
		{
			return await _hemogramDal.FindAllAsync(p => p.Personel_Id==hemogram.Personel_Id);
		}

		public async Task<Hemogram> AddAsync(Hemogram hemogram)
		{
			return await _hemogramDal.AddAsync(hemogram);
		}

		public async Task<IEnumerable<Hemogram>> AddAllAsync(IEnumerable<Hemogram> hemogramList)
		{
			return await _hemogramDal.AddAllAsync(hemogramList);
		}

		public async Task<Hemogram> UpdateAsync(Hemogram hemogram, int key)
		{
			return await _hemogramDal.UpdateAsync(hemogram, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _hemogramDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _hemogramDal.CountAsync();
		}
	}
}
