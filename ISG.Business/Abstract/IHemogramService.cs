using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IHemogramService
	{

		Task<Hemogram> GetAsync(int id);

		Task<ICollection<Hemogram>> GetAllAsync();

		Task<Hemogram> FindAsync(Hemogram hemogram);

		Task<ICollection<Hemogram>> FindAllAsync(Hemogram hemogram);

		Task<Hemogram> AddAsync(Hemogram hemogram);

		Task<IEnumerable<Hemogram>> AddAllAsync(IEnumerable<Hemogram> hemogramList);

		Task<Hemogram> UpdateAsync(Hemogram hemogram, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
