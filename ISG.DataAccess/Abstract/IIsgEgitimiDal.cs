using ISG.Entities.Concrete;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IIsgEgitimiDal:IEntityRepository<IsgEgitimi>
	{
	    Task<bool> SilAsync(int x);
		Task<object> EgitimAlanPersAsyc(int Sirket_Id, int Year);
	}
}
