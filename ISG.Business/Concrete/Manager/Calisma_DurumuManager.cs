using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class Calisma_DurumuManager : ICalisma_DurumuService
	{
		private ICalisma_DurumuDal _calisma_DurumuDal;
	 
		public Calisma_DurumuManager(ICalisma_DurumuDal calisma_DurumuDal)
		{
			_calisma_DurumuDal = calisma_DurumuDal;
		}

		public async Task<Calisma_Durumu> GetAsync(int id)
		{
			return await _calisma_DurumuDal.GetAsync(id);
		}

		public async Task<ICollection<Calisma_Durumu>> GetAllAsync()
		{
			return await _calisma_DurumuDal.GetAllAsync();
		}

		public async Task<Calisma_Durumu> FindAsync(Calisma_Durumu calisma_Durumu)
		{
			return await _calisma_DurumuDal.FindAsync(p => p.Calisma_Durumu_Id == calisma_Durumu.Calisma_Durumu_Id);
		}

		public async Task<ICollection<Calisma_Durumu>> FindAllAsync(Calisma_Durumu calisma_Durumu)
		{
			return await _calisma_DurumuDal.FindAllAsync(p => p.Personel_Id==calisma_Durumu.Calisma_Durumu_Id);
		}

		public async Task<Calisma_Durumu> AddAsync(Calisma_Durumu calisma_Durumu)
		{
			return await _calisma_DurumuDal.AddAsync(calisma_Durumu);
		}

		public async Task<IEnumerable<Calisma_Durumu>> AddAllAsync(IEnumerable<Calisma_Durumu> calisma_DurumuList)
		{
			return await _calisma_DurumuDal.AddAllAsync(calisma_DurumuList);
		}

		public async Task<Calisma_Durumu> UpdateAsync(Calisma_Durumu calisma_Durumu, int key)
		{
			return await _calisma_DurumuDal.UpdateAsync(calisma_Durumu, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _calisma_DurumuDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _calisma_DurumuDal.CountAsync();
		}
	}
}
