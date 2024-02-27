using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISaglikBirimiService
    {
        Task<SaglikBirimi> GetAsync(int id);

        Task<ICollection<SaglikBirimi>> GetAllAsync();

        Task<SaglikBirimi> FindAsync(SaglikBirimi saglikBirimi);

        Task<ICollection<SaglikBirimi>> FindAllAsync(SaglikBirimi saglikBirimi);

        Task<SaglikBirimi> AddAsync(SaglikBirimi saglikBirimi);

        Task<IEnumerable<SaglikBirimi>> AddAllAsync(IEnumerable<SaglikBirimi> saglikBirimiList);

        Task<SaglikBirimi> UpdateAsync(SaglikBirimi saglikBirimi, int key);

        Task<int> DeleteAsync(int key);

        Task<int> CountAsync();

        Task<SaglikBirimi> ProtokolAl(int id);
    }
}
