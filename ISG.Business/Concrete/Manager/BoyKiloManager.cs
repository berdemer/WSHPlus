using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class BoyKiloManager : IBoyKiloService
	{
		private IBoyKiloDal _boyKiloDal;
	 
		public BoyKiloManager(IBoyKiloDal boyKiloDal)
		{
			_boyKiloDal = boyKiloDal;
		}

		public async Task<BoyKilo> GetAsync(int id)
		{
			return await _boyKiloDal.GetAsync(id);
		}

		public async Task<ICollection<BoyKilo>> GetAllAsync()
		{
			return await _boyKiloDal.GetAllAsync();
		}

		public async Task<BoyKilo> FindAsync(BoyKilo boyKilo)
		{
			return await _boyKiloDal.FindAsync(p => p.BoyKilo_Id == boyKilo.BoyKilo_Id);
		}

		public async Task<ICollection<BoyKilo>> FindAllAsync(BoyKilo boyKilo)
		{
			return await _boyKiloDal.FindAllAsync(p => p.Personel_Id==boyKilo.Personel_Id);
		}

		public async Task<BoyKilo> AddAsync(BoyKilo boyKilo)
		{
			return await _boyKiloDal.AddAsync(boyKilo);
		}

		public async Task<IEnumerable<BoyKilo>> AddAllAsync(IEnumerable<BoyKilo> boyKiloList)
		{
			return await _boyKiloDal.AddAllAsync(boyKiloList);
		}

		public async Task<BoyKilo> UpdateAsync(BoyKilo boyKilo, int key)
		{
			return await _boyKiloDal.UpdateAsync(boyKilo, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _boyKiloDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _boyKiloDal.CountAsync();
		}
	}
}
