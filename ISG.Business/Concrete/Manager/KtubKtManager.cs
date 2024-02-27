using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class KtubKtManager : IKtubKtService
	{
		private IKtubKtDal _ktubKtDal;
	 
		public KtubKtManager(IKtubKtDal ktubKtDal)
		{
			_ktubKtDal = ktubKtDal;
		}

		public async Task<KtubKt> GetAsync(int id)
		{
			return await _ktubKtDal.GetAsync(id);
		}

		public async Task<ICollection<KtubKt>> GetAllAsync()
		{
			return await _ktubKtDal.GetAllAsync();
		}

		public async Task<KtubKt> FindAsync(KtubKt ktubKt)
		{
			return await _ktubKtDal.FindAsync(p => p.KtubKt_Id== ktubKt.KtubKt_Id);
		}

		public async Task<KtubKt> AddAsync(KtubKt ktubKt)
		{
			return await _ktubKtDal.AddAsync(ktubKt);
		}

		public async Task<IEnumerable<KtubKt>> AddAllAsync(IEnumerable<KtubKt> ktubKtList)
		{
			return await _ktubKtDal.AddAllAsync(ktubKtList);
		}

		public async Task<KtubKt> UpdateAsync(KtubKt ktubKt, int key)
		{
			return await _ktubKtDal.UpdateAsync(ktubKt, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _ktubKtDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _ktubKtDal.CountAsync();
		}

        public async Task<int> FullDeleteAsync()
        {
            return await _ktubKtDal.FullDeleteAsync();
        }
    }
}
