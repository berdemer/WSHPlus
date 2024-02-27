using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IRevirIslemService
	{

		Task<RevirIslem> GetAsync(int id);

		Task<ICollection<RevirIslem>> GetAllAsync();

		Task<RevirIslem> FindAsync(RevirIslem revirIslem);

		Task<ICollection<RevirIslem>> FindAllAsync(RevirIslem revirIslem);

		Task<RevirIslem> AddAsync(RevirIslem revirIslem);

		Task<IEnumerable<RevirIslem>> AddAllAsync(IEnumerable<RevirIslem> revirIslemList);

		Task<RevirIslem> UpdateAsync(RevirIslem revirIslem, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
		Task<RevirDonusu> RevirIslem(RevirIslem revir, bool prt);
        Task<Object> RevirAnaliz(int year, int saglikBirimi_Id);
    }
}
