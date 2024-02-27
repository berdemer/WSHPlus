using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IAsiService
	{

		Task<Asi> GetAsync(int id);

		Task<ICollection<Asi>> GetAllAsync();

		Task<Asi> FindAsync(Asi asi);

		Task<ICollection<Asi>> FindAllAsync(Asi asi);

		Task<Asi> AddAsync(Asi asi);

		Task<IEnumerable<Asi>> AddAllAsync(IEnumerable<Asi> asiList);

		Task<Asi> UpdateAsync(Asi asi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

		Task<Asi> MukerrerAsiKontrolu(DateTime Yapilma_Tarihi, int Personel_Id);
	}
}
