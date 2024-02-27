using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIlacStokGirisiService
    {

		Task<IlacStokGirisi> GetAsync(int id);

		Task<ICollection<IlacStokGirisi>> GetAllAsync();

		Task<IlacStokGirisi> FindAsync(IlacStokGirisi ilacStokGirisi);

		Task<ICollection<IlacStokGirisi>> FindAllAsync(IlacStokGirisi ilacStokGirisi);

		Task<IlacStokGirisi> AddAsync(IlacStokGirisi ilacStokGirisi);

		Task<IEnumerable<IlacStokGirisi>> AddAllAsync(IEnumerable<IlacStokGirisi> adresList);

		Task<IlacStokGirisi> UpdateAsync(IlacStokGirisi ilacStokGirisi, Guid key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

        Task<ICollection<IlacStokGirisiView>> IlacStokGirisiView(int SB_Id, bool st);
    }
}
