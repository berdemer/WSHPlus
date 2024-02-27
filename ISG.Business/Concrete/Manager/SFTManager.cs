using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class SFTManager : ISFTService
	{
		private ISFTDal _sFTDal;
	 
		public SFTManager(ISFTDal sFTDal)
		{
			_sFTDal = sFTDal;
		}

		public async Task<SFT> GetAsync(int id)
		{
			return await _sFTDal.GetAsync(id);
		}

		public async Task<ICollection<SFT>> GetAllAsync()
		{
			return await _sFTDal.GetAllAsync();
		}

		public async Task<SFT> FindAsync(SFT sFT)
		{
			return await _sFTDal.FindAsync(p => p.Sft_Id == sFT.Sft_Id);
		}

		public async Task<ICollection<SFT>> FindAllAsync(SFT sFT)
		{
            return await _sFTDal.FindAllAsync(p => p.Personel_Id == sFT.Personel_Id);
		}

		public async Task<SFT> AddAsync(SFT sFT)
		{
			return await _sFTDal.AddAsync(sFT);
		}

		public async Task<IEnumerable<SFT>> AddAllAsync(IEnumerable<SFT> sFTList)
		{
			return await _sFTDal.AddAllAsync(sFTList);
		}

		public async Task<SFT> UpdateAsync(SFT sFT, int key)
		{
			return await _sFTDal.UpdateAsync(sFT, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _sFTDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _sFTDal.CountAsync();
		}
	}
}
