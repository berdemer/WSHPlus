using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPersonelMuayeneService
	{

		Task<PersonelMuayene> GetAsync(int id);

		Task<ICollection<PersonelMuayene>> GetAllAsync();

		Task<PersonelMuayene> FindAsync(PersonelMuayene personelMuayene);

		Task<ICollection<PersonelMuayene>> FindAllAsync(PersonelMuayene personelMuayene);

		Task<PersonelMuayene> AddAsync(PersonelMuayene personelMuayene);

		Task<IEnumerable<PersonelMuayene>> AddAllAsync(IEnumerable<PersonelMuayene> personelMuayeneList);

		Task<PersonelMuayene> UpdateAsync(PersonelMuayene personelMuayene, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
    }
}
