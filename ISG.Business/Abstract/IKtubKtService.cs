using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IKtubKtService
	{

		Task<KtubKt> GetAsync(int id);

		Task<ICollection<KtubKt>> GetAllAsync();

		Task<KtubKt> FindAsync(KtubKt ktubKt);

		Task<KtubKt> AddAsync(KtubKt ktubKt);

		Task<IEnumerable<KtubKt>> AddAllAsync(IEnumerable<KtubKt> ktubKtList);

		Task<KtubKt> UpdateAsync(KtubKt ktubKt, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

        Task<int> FullDeleteAsync();
    }
}
