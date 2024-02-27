using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class PersonelDetayiManager : IPersonelDetayiService
	{
		private IPersonelDetayiDal _personelDetayiDal;
	 
		public PersonelDetayiManager(IPersonelDetayiDal personelDetayiDal)
		{
			_personelDetayiDal = personelDetayiDal;
		}

		public async Task<PersonelDetayi> GetAsync(int id)
		{
			return await _personelDetayiDal.GetAsync(id);
		}

		public async Task<ICollection<PersonelDetayi>> GetAllAsync()
		{
			return await _personelDetayiDal.GetAllAsync();
		}

		public async Task<PersonelDetayi> FindAsync(PersonelDetayi personelDetayi)
		{
			return await _personelDetayiDal.FindAsync(p => p.PersonelDetay_Id== personelDetayi.PersonelDetay_Id);
		}

		public async Task<ICollection<PersonelDetayi>> FindAllAsync(PersonelDetayi personelDetayi)
		{
			return await _personelDetayiDal.FindAllAsync(p => p.PersonelDetay_Id == personelDetayi.PersonelDetay_Id);
		}

		public async Task<PersonelDetayi> AddAsync(PersonelDetayi personelDetayi)
		{
			return await _personelDetayiDal.AddAsync(personelDetayi);
		}

		public async Task<IEnumerable<PersonelDetayi>> AddAllAsync(IEnumerable<PersonelDetayi> personelDetayiList)
		{
			return await _personelDetayiDal.AddAllAsync(personelDetayiList);
		}

		public async Task<PersonelDetayi> UpdateAsync(PersonelDetayi personelDetayi, int key)
		{
			return await _personelDetayiDal.UpdateAsync(personelDetayi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _personelDetayiDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _personelDetayiDal.CountAsync();
		}
	}
}
