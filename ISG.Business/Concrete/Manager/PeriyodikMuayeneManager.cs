using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class PeriyodikMuayeneManager : IPeriyodikMuayeneService
	{
		private IPeriyodikMuayeneDal _periyodikMuayeneDal;
	 
		public PeriyodikMuayeneManager(IPeriyodikMuayeneDal periyodikMuayeneDal)
		{
			_periyodikMuayeneDal = periyodikMuayeneDal;
		}

		public async Task<PeriyodikMuayene> GetAsync(int id)
		{
			return await _periyodikMuayeneDal.GetAsync(id);
		}

		public async Task<ICollection<PeriyodikMuayene>> GetAllAsync()
		{
			return await _periyodikMuayeneDal.GetAllAsync();
		}

		public async Task<PeriyodikMuayene> FindAsync(PeriyodikMuayene periyodikMuayene)
		{
			return await _periyodikMuayeneDal.FindAsync(p => p.PeriyodikMuayene_Id == periyodikMuayene.PeriyodikMuayene_Id);
		}

		public async Task<ICollection<PeriyodikMuayene>> FindAllAsync(PeriyodikMuayene periyodikMuayene)
		{
			return await _periyodikMuayeneDal.FindAllAsync(p => p.Personel_Id==periyodikMuayene.Personel_Id);
		}

		public async Task<PeriyodikMuayene> AddAsync(PeriyodikMuayene periyodikMuayene)
		{
			return await _periyodikMuayeneDal.AddAsync(periyodikMuayene);
		}

		public async Task<IEnumerable<PeriyodikMuayene>> AddAllAsync(IEnumerable<PeriyodikMuayene> periyodikMuayeneList)
		{
			return await _periyodikMuayeneDal.AddAllAsync(periyodikMuayeneList);
		}

		public async Task<PeriyodikMuayene> UpdateAsync(PeriyodikMuayene periyodikMuayene, int key)
		{
			return await _periyodikMuayeneDal.UpdateAsync(periyodikMuayene, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _periyodikMuayeneDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _periyodikMuayeneDal.CountAsync();
		}
	}
}
