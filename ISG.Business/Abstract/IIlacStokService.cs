using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIlacStokService
    {

		Task<IlacStok> GetAsync(int id);

		Task<ICollection<IlacStok>> GetAllAsync();

		Task<IlacStok> FindAsync(IlacStok ilacStok);

		Task<ICollection<IlacStok>> FindAllAsync(IlacStok ilacStok);

		Task<IlacStok> AddAsync(IlacStok ilacStok);

		Task<IEnumerable<IlacStok>> AddAllAsync(IEnumerable<IlacStok> ilacStokList);

		Task<IlacStok> UpdateAsync(IlacStok ilacStok, Guid key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

        Task<ICollection<IlacStoku>> IlacStokHesaplari(int saglikBirimi_Id, bool status);

    }
}
