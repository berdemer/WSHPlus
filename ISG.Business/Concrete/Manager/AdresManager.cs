using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class AdresManager : IAdresService
	{
		private IAdresDal _adresDal;
	 
		public AdresManager(IAdresDal adresDal)
		{
			_adresDal = adresDal;
		}

		public async Task<Adres> GetAsync(int id)
		{
			return await _adresDal.GetAsync(id);
		}

		public async Task<ICollection<Adres>> GetAllAsync()
		{
			return await _adresDal.GetAllAsync();
		}

		public async Task<Adres> FindAsync(Adres adres)
		{
			return await _adresDal.FindAsync(p => p.Adres_Id== adres.Adres_Id);
		}

		public async Task<ICollection<Adres>> FindAllAsync(Adres adres)
		{
			return await _adresDal.FindAllAsync(p => p.Personel_Id==adres.Adres_Id);
		}

		public async Task<Adres> AddAsync(Adres adres)
		{
			return await _adresDal.AddAsync(adres);
		}

		public async Task<IEnumerable<Adres>> AddAllAsync(IEnumerable<Adres> adresList)
		{
			return await _adresDal.AddAllAsync(adresList);
		}

		public async Task<Adres> UpdateAsync(Adres adres, int key)
		{
			return await _adresDal.UpdateAsync(adres, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _adresDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _adresDal.CountAsync();
		}
	}
}
