using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPersonelDetayiService
	{

		Task<PersonelDetayi> GetAsync(int id);

		Task<ICollection<PersonelDetayi>> GetAllAsync();

		Task<PersonelDetayi> FindAsync(PersonelDetayi personelDetayi);

		Task<ICollection<PersonelDetayi>> FindAllAsync(PersonelDetayi personelDetayi);

		Task<PersonelDetayi> AddAsync(PersonelDetayi personelDetayi);

		Task<IEnumerable<PersonelDetayi>> AddAllAsync(IEnumerable<PersonelDetayi> personelDetayiList);

		Task<PersonelDetayi> UpdateAsync(PersonelDetayi personelDetayi, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
