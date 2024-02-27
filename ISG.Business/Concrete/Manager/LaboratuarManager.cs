using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class LaboratuarManager : ILaboratuarService
	{
		private ILaboratuarDal _laboratuarDal;
	 
		public LaboratuarManager(ILaboratuarDal laboratuarDal)
		{
			_laboratuarDal = laboratuarDal;
		}

		public async Task<Laboratuar> GetAsync(int id)
		{
			return await _laboratuarDal.GetAsync(id);
		}

		public async Task<ICollection<Laboratuar>> GetAllAsync()
		{
			return await _laboratuarDal.GetAllAsync();
		}

		public async Task<Laboratuar> FindAsync(Laboratuar laboratuar)
		{
			return await _laboratuarDal.FindAsync(p => p.Laboratuar_Id == laboratuar.Laboratuar_Id);
		}

		public async Task<ICollection<Laboratuar>> FindAllAsync(Laboratuar laboratuar)
		{
			return await _laboratuarDal.FindAllAsync(p => p.Personel_Id==laboratuar.Personel_Id);
		}

		public async Task<Laboratuar> AddAsync(Laboratuar laboratuar)
		{
			return await _laboratuarDal.AddAsync(laboratuar);
		}

		public async Task<IEnumerable<Laboratuar>> AddAllAsync(IEnumerable<Laboratuar> laboratuarList)
		{
			return await _laboratuarDal.AddAllAsync(laboratuarList);
		}

		public async Task<Laboratuar> UpdateAsync(Laboratuar laboratuar, int key)
		{
			return await _laboratuarDal.UpdateAsync(laboratuar, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _laboratuarDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _laboratuarDal.CountAsync();
		}
	}
}
