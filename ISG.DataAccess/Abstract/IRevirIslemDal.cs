using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IRevirIslemDal:IEntityRepository<RevirIslem>
	{
        Task<RevirDonusu> RevirIslem(RevirIslem revir, bool prt);
        Task<Object> RevirAnaliz(int year, int saglikBirimi_Id);

    }
}
