using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface ITanimDal : IEntityRepository<Tanim>
    {
        Task<ICollection<Tanim>> SikayetAra(string value);
        Task<ICollection<Tanim>> BulguAra(string value);
    }
}
