using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class ANTManager : IANTService
	{
		private IANTDal _aNTDal;
	 
		public ANTManager(IANTDal aNTDal)
		{
			_aNTDal = aNTDal;
		}

		public async Task<ANT> GetAsync(int id)
		{
			return await _aNTDal.GetAsync(id);
		}

		public async Task<ICollection<ANT>> GetAllAsync()
		{
			return await _aNTDal.GetAllAsync();
		}

		public async Task<ANT> FindAsync(ANT aNT)
		{
			return await _aNTDal.FindAsync(p => p.ANT_Id == aNT.ANT_Id);
		}

		public async Task<ICollection<ANT>> FindAllAsync(ANT aNT)
		{
			return await _aNTDal.FindAllAsync(p => p.Personel_Id==aNT.Personel_Id);
		}

		public async Task<ANT> AddAsync(ANT aNT)
		{
			return await _aNTDal.AddAsync(aNT);
		}

		public async Task<IEnumerable<ANT>> AddAllAsync(IEnumerable<ANT> aNTList)
		{
			return await _aNTDal.AddAllAsync(aNTList);
		}

		public async Task<ANT> UpdateAsync(ANT aNT, int key)
		{
			return await _aNTDal.UpdateAsync(aNT, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _aNTDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _aNTDal.CountAsync();
		}
	}
}
