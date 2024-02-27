using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ITanimService
    {

        Task<Tanim> GetAsync(int id);

        Task<ICollection<Tanim>> GetAllAsync();

        Task<Tanim> FindAsync(Tanim tanim);

        Task<ICollection<Tanim>> FindAllAsync(Tanim tanim);

        Task<Tanim> AddAsync(Tanim tanim);

        Task<IEnumerable<Tanim>> AddAllAsync(IEnumerable<Tanim> tanimList);

        Task<Tanim> UpdateAsync(Tanim tanim, int key);

        Task<int> DeleteAsync(int key);

        Task<int> CountAsync();
        Task<ICollection<Tanim>> SikayetAra(string value);
        Task<ICollection<Tanim>> BulguAra(string value);
        Task<ICollection<Tanim>> llceAllAsync(Tanim tanim);
    }
}