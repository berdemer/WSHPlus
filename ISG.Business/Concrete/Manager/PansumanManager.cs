using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class PansumanManager : IPansumanService
	{
		private IPansumanDal _pansumanDal;
	 
		public PansumanManager(IPansumanDal pansumanDal)
		{
			_pansumanDal = pansumanDal;
		}

		public async Task<Pansuman> GetAsync(int id)
		{
			return await _pansumanDal.GetAsync(id);
		}

		public async Task<ICollection<Pansuman>> GetAllAsync()
		{
			return await _pansumanDal.GetAllAsync();
		}

		public async Task<Pansuman> FindAsync(Pansuman pansuman)
		{
			return await _pansumanDal.FindAsync(p => p.Pansuman_Id == pansuman.Pansuman_Id);
		}

		public async Task<ICollection<Pansuman>> FindAllAsync(Pansuman pansuman)
		{
			return await _pansumanDal.FindAllAsync(p => p.Personel_Id==pansuman.Personel_Id);
		}

		public async Task<Pansuman> AddAsync(Pansuman pansuman)
		{
			return await _pansumanDal.AddAsync(pansuman);
		}

		public async Task<IEnumerable<Pansuman>> AddAllAsync(IEnumerable<Pansuman> pansumanList)
		{
			return await _pansumanDal.AddAllAsync(pansumanList);
		}

		public async Task<Pansuman> UpdateAsync(Pansuman pansuman, int key)
		{
			return await _pansumanDal.UpdateAsync(pansuman, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _pansumanDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _pansumanDal.CountAsync();
		}
	}
}
