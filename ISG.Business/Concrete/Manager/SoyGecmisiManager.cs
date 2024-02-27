using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class SoyGecmisiManager : ISoyGecmisiService
	{
		private ISoyGecmisiDal _soyGecmisiDal;
	 
		public SoyGecmisiManager(ISoyGecmisiDal soyGecmisiDal)
		{
			_soyGecmisiDal = soyGecmisiDal;
		}

		public async Task<SoyGecmisi> GetAsync(int id)
		{
			return await _soyGecmisiDal.GetAsync(id);
		}

		public async Task<ICollection<SoyGecmisi>> GetAllAsync()
		{
			return await _soyGecmisiDal.GetAllAsync();
		}

		public async Task<SoyGecmisi> FindAsync(SoyGecmisi soyGecmisi)
		{
			return await _soyGecmisiDal.FindAsync(p => p.SoyGecmisi_Id == soyGecmisi.SoyGecmisi_Id);
		}

		public async Task<ICollection<SoyGecmisi>> FindAllAsync(SoyGecmisi soyGecmisi)
		{
			return await _soyGecmisiDal.FindAllAsync(p => p.Personel_Id==soyGecmisi.Personel_Id);
		}

		public async Task<SoyGecmisi> AddAsync(SoyGecmisi soyGecmisi)
		{
			return await _soyGecmisiDal.AddAsync(soyGecmisi);
		}

		public async Task<IEnumerable<SoyGecmisi>> AddAllAsync(IEnumerable<SoyGecmisi> soyGecmisiList)
		{
			return await _soyGecmisiDal.AddAllAsync(soyGecmisiList);
		}

		public async Task<SoyGecmisi> UpdateAsync(SoyGecmisi soyGecmisi, int key)
		{
			return await _soyGecmisiDal.UpdateAsync(soyGecmisi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _soyGecmisiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _soyGecmisiDal.CountAsync();
		}
	}
}
