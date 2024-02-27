using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IIcRaporService
	{

		Task<IcRapor> GetAsync(int id);

		Task<ICollection<IcRapor>> GetAllAsync();

		Task<IcRapor> FindAsync(IcRapor icRapor);

		Task<ICollection<IcRapor>> FindAllAsync(IcRapor icRapor);

		Task<IcRapor> AddAsync(IcRapor icRapor);

		Task<IEnumerable<IcRapor>> AddAllAsync(IEnumerable<IcRapor> icRaporList);

		Task<IcRapor> UpdateAsync(IcRapor icRapor, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<IcRapor> FindProtokolAsync(int protokol);

	}
}
