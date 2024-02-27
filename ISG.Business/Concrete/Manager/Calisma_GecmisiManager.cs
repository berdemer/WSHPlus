using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class Calisma_GecmisiManager : ICalisma_GecmisiService
	{
		private ICalisma_GecmisiDal _calisma_GecmisiDal;
	 
		public Calisma_GecmisiManager(ICalisma_GecmisiDal calisma_GecmisiDal)
		{
			_calisma_GecmisiDal = calisma_GecmisiDal;
		}

		public async Task<Calisma_Gecmisi> GetAsync(int id)
		{
			return await _calisma_GecmisiDal.GetAsync(id);
		}

		public async Task<ICollection<Calisma_Gecmisi>> GetAllAsync()
		{
			return await _calisma_GecmisiDal.GetAllAsync();
		}

		public async Task<Calisma_Gecmisi> FindAsync(Calisma_Gecmisi calisma_Gecmisi)
		{
			return await _calisma_GecmisiDal.FindAsync(p => p.Calisma_Gecmisi_Id == calisma_Gecmisi.Calisma_Gecmisi_Id);
		}

		public async Task<ICollection<Calisma_Gecmisi>> FindAllAsync(Calisma_Gecmisi calisma_Gecmisi)
		{
			return await _calisma_GecmisiDal.FindAllAsync(p => p.Personel_Id==calisma_Gecmisi.Calisma_Gecmisi_Id);
		}

		public async Task<Calisma_Gecmisi> AddAsync(Calisma_Gecmisi calisma_Gecmisi)
		{
			return await _calisma_GecmisiDal.AddAsync(calisma_Gecmisi);
		}

		public async Task<IEnumerable<Calisma_Gecmisi>> AddAllAsync(IEnumerable<Calisma_Gecmisi> calisma_GecmisiList)
		{
			return await _calisma_GecmisiDal.AddAllAsync(calisma_GecmisiList);
		}

		public async Task<Calisma_Gecmisi> UpdateAsync(Calisma_Gecmisi calisma_Gecmisi, int key)
		{
			return await _calisma_GecmisiDal.UpdateAsync(calisma_Gecmisi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _calisma_GecmisiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _calisma_GecmisiDal.CountAsync();
		}
	}
}
