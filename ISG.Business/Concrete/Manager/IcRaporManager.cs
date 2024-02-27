using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class IcRaporManager : IIcRaporService
	{
		private IIcRaporDal _icRaporDal;
	 
		public IcRaporManager(IIcRaporDal icRaporDal)
		{
			_icRaporDal = icRaporDal;
		}

		public async Task<IcRapor> GetAsync(int id)
		{
			return await _icRaporDal.GetAsync(id);
		}

		public async Task<ICollection<IcRapor>> GetAllAsync()
		{
			return await _icRaporDal.GetAllAsync();
		}

		public async Task<IcRapor> FindAsync(IcRapor icRapor)
		{
			return await _icRaporDal.FindAsync(p => p.IcRapor_Id == icRapor.IcRapor_Id);
		}

		public async Task<ICollection<IcRapor>> FindAllAsync(IcRapor icRapor)
		{
			return await _icRaporDal.FindAllAsync(p => p.Personel_Id==icRapor.IcRapor_Id);
		}

		public async Task<IcRapor> AddAsync(IcRapor icRapor)
		{
			return await _icRaporDal.AddAsync(icRapor);
		}

		public async Task<IEnumerable<IcRapor>> AddAllAsync(IEnumerable<IcRapor> icRaporList)
		{
			return await _icRaporDal.AddAllAsync(icRaporList);
		}

		public async Task<IcRapor> UpdateAsync(IcRapor icRapor, int key)
		{
			return await _icRaporDal.UpdateAsync(icRapor, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _icRaporDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _icRaporDal.CountAsync();
		}

        public async Task<IcRapor> FindProtokolAsync(int protokol)
        {
            return await _icRaporDal.FindAsync(p => p.Protokol == protokol);
        }
    }
}
