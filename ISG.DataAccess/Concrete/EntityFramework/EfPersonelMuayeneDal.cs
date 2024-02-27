using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfPersonelMuayeneDal : EfEntityRepositoryBase<PersonelMuayene, ISGContext>, IPersonelMuayeneDal
    {
    
    }
}
