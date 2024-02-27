using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using System;

namespace ISG.Business.Concrete.Manager
{
	public class IlacManager : IIlacService
	{
		private IIlacDal _ilacDal;
	 
		public IlacManager(IIlacDal ilacDal)
		{
			_ilacDal = ilacDal;
		}

		public async Task<Ilac> GetAsync(int id)
		{
			return await _ilacDal.GetAsync(id);
		}

		public async Task<ICollection<Ilac>> GetAllAsync()
		{
			return await _ilacDal.GetAllAsync();
		}

		public async Task<Ilac> FindAsync(Ilac ilac)
		{
			return await _ilacDal.FindAsync(p => p.IlacBarkodu== ilac.IlacBarkodu);
		}

		public async Task<ICollection<Ilac>> FindAllAsync(Ilac ilac)
		{
			return await _ilacDal.FindAllAsync(p => p.IlacAdi==ilac.IlacAdi);
		}

		public async Task<Ilac> AddAsync(Ilac ilac)
		{
			return await _ilacDal.AddAsync(ilac);
		}

		public async Task<IEnumerable<Ilac>> AddAllAsync(IEnumerable<Ilac> ilacList)
		{
			return await _ilacDal.AddAllAsync(ilacList);
		}

		public async Task<Ilac> UpdateStringAsync(Ilac ilac, string key)
		{
			return await _ilacDal.UpdateStringAsync(ilac, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ilacDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ilacDal.CountAsync();
		}

        public async Task<ICollection<SBView>> SaglikBirimiUserId(string user_Id)
        {
            return await _ilacDal.SaglikBirimiUserId(user_Id);
        }

        public async Task<ICollection<Ilac>> IlacAdiAra(string value)
        {
            return await _ilacDal.IlacAdiAra(value);
        }

        public async Task<int> FullDeleteAsync()
        {
            return await _ilacDal.FullDeleteAsync();
        }

        public async Task<KtubKt> KtubKtAra(string Search)
        {
            return await _ilacDal.KtubKtAra(Search);
        }
    }
}
