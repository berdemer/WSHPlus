using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class OdioManager : IOdioService
	{
		private IOdioDal _odioDal;
	 
		public OdioManager(IOdioDal odioDal)
		{
			_odioDal = odioDal;
		}

		public async Task<Odio> GetAsync(int id)
		{
			return await _odioDal.GetAsync(id);
		}

		public async Task<ICollection<Odio>> GetAllAsync()
		{
			return await _odioDal.GetAllAsync();
		}

		public async Task<Odio> FindAsync(Odio odio)
		{
			return await _odioDal.FindAsync(p => p.Odio_Id == odio.Odio_Id);
		}

		public async Task<ICollection<Odio>> FindAllAsync(Odio odio)
		{
			return await _odioDal.FindAllAsync(p => p.Personel_Id==odio.Personel_Id);
		}

		public async Task<Odio> AddAsync(Odio odio)
		{
			return await _odioDal.AddAsync(odio);
		}

		public async Task<IEnumerable<Odio>> AddAllAsync(IEnumerable<Odio> odioList)
		{
			return await _odioDal.AddAllAsync(odioList);
		}

		public async Task<Odio> UpdateAsync(Odio odio, int key)
		{
			return await _odioDal.UpdateAsync(odio, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _odioDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _odioDal.CountAsync();
		}
	}
}
