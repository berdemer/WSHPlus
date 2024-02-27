using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISG.Entities.ComplexType;
using System;

namespace ISG.Business.Concrete.Manager
{
	public class RevirIslemManager : IRevirIslemService
	{
		private IRevirIslemDal _revirIslemDal;
	 
		public RevirIslemManager(IRevirIslemDal revirIslemDal)
		{
			_revirIslemDal = revirIslemDal;
		}

		public async Task<RevirIslem> GetAsync(int id)
		{
			return await _revirIslemDal.GetAsync(id);
		}

		public async Task<ICollection<RevirIslem>> GetAllAsync()
		{
			return await _revirIslemDal.GetAllAsync();
		}

		public async Task<RevirIslem> FindAsync(RevirIslem revirIslem)
		{
			return await _revirIslemDal.FindAsync(p => p.RevirIslem_Id == revirIslem.RevirIslem_Id);
		}

		public async Task<ICollection<RevirIslem>> FindAllAsync(RevirIslem revirIslem)
		{
			return await _revirIslemDal.FindAllAsync(p => p.RevirIslem_Id==revirIslem.RevirIslem_Id);
		}

		public async Task<RevirIslem> AddAsync(RevirIslem revirIslem)
		{
			return await _revirIslemDal.AddAsync(revirIslem);
		}

		public async Task<IEnumerable<RevirIslem>> AddAllAsync(IEnumerable<RevirIslem> revirIslemList)
		{
			return await _revirIslemDal.AddAllAsync(revirIslemList);
		}

		public async Task<RevirIslem> UpdateAsync(RevirIslem revirIslem, int key)
		{
			return await _revirIslemDal.UpdateAsync(revirIslem, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _revirIslemDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _revirIslemDal.CountAsync();
		}

        public async Task<RevirDonusu> RevirIslem(RevirIslem revir, bool prt)
        {
            return await _revirIslemDal.RevirIslem(revir, prt);
        }

        public async Task<object> RevirAnaliz(int year, int saglikBirimi_Id)
        {
            return await _revirIslemDal.RevirAnaliz(year, saglikBirimi_Id);
        }
    }
}
