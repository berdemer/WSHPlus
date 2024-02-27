using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ISG.Business.Concrete.Manager
{
	public class IlacStokGirisiManager : IIlacStokGirisiService
	{
		private IIlacStokGirisiDal _ilacStokGirisiDal;
	 
		public IlacStokGirisiManager(IIlacStokGirisiDal ilacStokGirisiDal)
		{
			_ilacStokGirisiDal = ilacStokGirisiDal;
		}

		public async Task<IlacStokGirisi> GetAsync(int id)
		{
			return await _ilacStokGirisiDal.GetAsync(id);
		}

		public async Task<ICollection<IlacStokGirisi>> GetAllAsync()
		{
			return await _ilacStokGirisiDal.GetAllAsync();
		}

		public async Task<IlacStokGirisi> FindAsync(IlacStokGirisi ilacStokGirisi)
		{
			return await _ilacStokGirisiDal.FindAsync(p => p.Id== ilacStokGirisi.Id);
		}

		public async Task<ICollection<IlacStokGirisi>> FindAllAsync(IlacStokGirisi ilacStokGirisi)
		{
			return await _ilacStokGirisiDal.FindAllAsync(p => p.StokId==ilacStokGirisi.StokId);
		}

		public async Task<IlacStokGirisi> AddAsync(IlacStokGirisi ilacStokGirisi)
		{
			return await _ilacStokGirisiDal.AddAsync(ilacStokGirisi);
		}

		public async Task<IEnumerable<IlacStokGirisi>> AddAllAsync(IEnumerable<IlacStokGirisi> ilacStokGirisiList)
		{
			return await _ilacStokGirisiDal.AddAllAsync(ilacStokGirisiList);
		}

		public async Task<IlacStokGirisi> UpdateAsync(IlacStokGirisi ilacStokGirisi, Guid key)
		{
			return await _ilacStokGirisiDal.UpdateAsync(ilacStokGirisi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ilacStokGirisiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ilacStokGirisiDal.CountAsync();
		}


        public async Task<ICollection<IlacStokGirisiView>> IlacStokGirisiView(int SB_Id, bool st)
        {
            return await _ilacStokGirisiDal.IlacStokGirisiView(SB_Id,st);
        }
    }
}
