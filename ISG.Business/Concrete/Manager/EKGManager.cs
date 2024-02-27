using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class EKGManager : IEKGService
	{
		private IEKGDal _ekgDal;
	 
		public EKGManager(IEKGDal ekgDal)
		{
			_ekgDal = ekgDal;
		}

		public async Task<EKG> GetAsync(int id)
		{
			return await _ekgDal.GetAsync(id);
		}

		public async Task<ICollection<EKG>> GetAllAsync()
		{
			return await _ekgDal.GetAllAsync();
		}

		public async Task<EKG> FindAsync(EKG ekg)
		{
			return await _ekgDal.FindAsync(p => p.EKG_Id == ekg.EKG_Id);
		}

		public async Task<ICollection<EKG>> FindAllAsync(EKG ekg)
		{
			return await _ekgDal.FindAllAsync(p => p.Personel_Id==ekg.Personel_Id);
		}

		public async Task<EKG> AddAsync(EKG ekg)
		{
			return await _ekgDal.AddAsync(ekg);
		}

		public async Task<IEnumerable<EKG>> AddAllAsync(IEnumerable<EKG> ekgList)
		{
			return await _ekgDal.AddAllAsync(ekgList);
		}

		public async Task<EKG> UpdateAsync(EKG ekg, int key)
		{
			return await _ekgDal.UpdateAsync(ekg, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ekgDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ekgDal.CountAsync();
		}
	}
}
