using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIlacService
	{

		Task<Ilac> GetAsync(int id);

		Task<ICollection<Ilac>> GetAllAsync();

		Task<Ilac> FindAsync(Ilac ilac);

		Task<ICollection<Ilac>> FindAllAsync(Ilac ilac);

		Task<Ilac> AddAsync(Ilac ilac);

		Task<IEnumerable<Ilac>> AddAllAsync(IEnumerable<Ilac> adresList);

        Task<Ilac> UpdateStringAsync(Ilac ilac, string key);

        Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
        Task<ICollection<SBView>> SaglikBirimiUserId(string user_Id);

        Task<ICollection<Ilac>> IlacAdiAra(string value);

        Task<int> FullDeleteAsync();

        Task<KtubKt> KtubKtAra(string Search);
    }
}
