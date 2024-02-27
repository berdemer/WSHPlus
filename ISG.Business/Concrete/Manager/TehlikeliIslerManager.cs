using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class TehlikeliIslerManager : ITehlikeliIslerService
	{
		private ITehlikeliIslerDal _tehlikeliIslerDal;
	 
		public TehlikeliIslerManager(ITehlikeliIslerDal tehlikeliIslerDal)
		{
			_tehlikeliIslerDal = tehlikeliIslerDal;
		}

		public async Task<TehlikeliIsler> GetAsync(int id)
		{
			return await _tehlikeliIslerDal.GetAsync(id);
		}

		public async Task<ICollection<TehlikeliIsler>> GetAllAsync()
		{
			return await _tehlikeliIslerDal.GetAllAsync();
		}

		public async Task<TehlikeliIsler> FindAsync(TehlikeliIsler tehlikeliIsler)
		{
			return await _tehlikeliIslerDal.FindAsync(p => p.TehlikeliIsler_Id== tehlikeliIsler.TehlikeliIsler_Id);
		}

		public async Task<ICollection<TehlikeliIsler>> FindAllAsync(TehlikeliIsler tehlikeliIsler)
		{
			return await _tehlikeliIslerDal.FindAllAsync(p => p.grubu==tehlikeliIsler.grubu);
		}

		public async Task<TehlikeliIsler> AddAsync(TehlikeliIsler tehlikeliIsler)
		{
			return await _tehlikeliIslerDal.AddAsync(tehlikeliIsler);
		}

		public async Task<IEnumerable<TehlikeliIsler>> AddAllAsync(IEnumerable<TehlikeliIsler> tehlikeliIslerList)
		{
			return await _tehlikeliIslerDal.AddAllAsync(tehlikeliIslerList);
		}

		public async Task<TehlikeliIsler> UpdateAsync(TehlikeliIsler tehlikeliIsler, int key)
		{
			return await _tehlikeliIslerDal.UpdateAsync(tehlikeliIsler, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _tehlikeliIslerDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _tehlikeliIslerDal.CountAsync();
		}

        public async Task<ICollection<TehlikeliIsler>> GrupList(IEnumerable<string> grupList)
        {
            return await _tehlikeliIslerDal.GrupList(grupList);
        }

        public async Task<ICollection<TehlikeliIsler>> TehlikeliIslerAra(string value)
        {
            return await _tehlikeliIslerDal.TehlikeliIslerAra(value);
        }

        public async Task<ICollection<string>> gruplardanAra(string value)
        {
            return await _tehlikeliIslerDal.gruplardanAra(value);
        }

    }
}
