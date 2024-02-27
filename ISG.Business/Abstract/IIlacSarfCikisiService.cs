using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIlacSarfCikisiService
	{

		Task<IlacSarfCikisi> GetAsync(Guid id);

		Task<ICollection<IlacSarfCikisi>> GetAllAsync();

		Task<IlacSarfCikisi> FindAsync(IlacSarfCikisi ilacSarfCikisi);

		Task<ICollection<IlacSarfCikisi>> FindAllAsync(IlacSarfCikisi ilacSarfCikisi);

		Task<IlacSarfCikisi> AddAsync(IlacSarfCikisi ilacSarfCikisi);

		Task<IEnumerable<IlacSarfCikisi>> AddAllAsync(IEnumerable<IlacSarfCikisi> ilacSarfCikisiList);

		Task<IlacSarfCikisi> UpdateAsync(IlacSarfCikisi ilacSarfCikisi, Guid key);

		Task<int> DeleteAsync(Guid key);

		Task<int> CountAsync();
	}
}
