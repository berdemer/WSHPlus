using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IKronikHastalikService
	{

		Task<KronikHastalik> GetAsync(int id);

		Task<ICollection<KronikHastalik>> GetAllAsync();

		Task<KronikHastalik> FindAsync(KronikHastalik kronikHastalik);

		Task<ICollection<KronikHastalik>> FindAllAsync(KronikHastalik kronikHastalik);

		Task<KronikHastalik> AddAsync(KronikHastalik kronikHastalik);

		Task<IEnumerable<KronikHastalik>> AddAllAsync(IEnumerable<KronikHastalik> kronikHastalikList);

		Task<KronikHastalik> UpdateAsync(KronikHastalik kronikHastalik, Guid key);

		Task<int> DeleteAsync(Guid key);

		Task<int> CountAsync();
	}
}
