using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class PersonelMuayeneManager : IPersonelMuayeneService
	{
		private IPersonelMuayeneDal _personelMuayeneDal;
	 
		public PersonelMuayeneManager(IPersonelMuayeneDal personelMuayeneDal)
		{
			_personelMuayeneDal = personelMuayeneDal;
		}

		public async Task<PersonelMuayene> GetAsync(int id)
		{
			return await _personelMuayeneDal.GetAsync(id);
		}

		public async Task<ICollection<PersonelMuayene>> GetAllAsync()
		{
			return await _personelMuayeneDal.GetAllAsync();
		}

		public async Task<PersonelMuayene> FindAsync(PersonelMuayene personelMuayene)
		{
			return await _personelMuayeneDal.FindAsync(p => p.PersonelMuayene_Id == personelMuayene.PersonelMuayene_Id);
		}

		public async Task<ICollection<PersonelMuayene>> FindAllAsync(PersonelMuayene personelMuayene)
		{
			return await _personelMuayeneDal.FindAllAsync(p => p.Personel_Id==personelMuayene.Personel_Id);
		}

		public async Task<PersonelMuayene> AddAsync(PersonelMuayene personelMuayene)
		{
			return await _personelMuayeneDal.AddAsync(personelMuayene);
		}

		public async Task<IEnumerable<PersonelMuayene>> AddAllAsync(IEnumerable<PersonelMuayene> personelMuayeneList)
		{
			return await _personelMuayeneDal.AddAllAsync(personelMuayeneList);
		}

		public async Task<PersonelMuayene> UpdateAsync(PersonelMuayene personelMuayene, int key)
		{
			return await _personelMuayeneDal.UpdateAsync(personelMuayene, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _personelMuayeneDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _personelMuayeneDal.CountAsync();
		}
    }
}
