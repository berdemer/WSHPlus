using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPeriyodikMuayeneService
	{

		Task<PeriyodikMuayene> GetAsync(int id);

		Task<ICollection<PeriyodikMuayene>> GetAllAsync();

		Task<PeriyodikMuayene> FindAsync(PeriyodikMuayene periyodikMuayene);

		Task<ICollection<PeriyodikMuayene>> FindAllAsync(PeriyodikMuayene periyodikMuayene);

		Task<PeriyodikMuayene> AddAsync(PeriyodikMuayene periyodikMuayene);

		Task<IEnumerable<PeriyodikMuayene>> AddAllAsync(IEnumerable<PeriyodikMuayene> periyodikMuayeneList);

		Task<PeriyodikMuayene> UpdateAsync(PeriyodikMuayene periyodikMuayene, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
