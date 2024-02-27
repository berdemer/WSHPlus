using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISirketService
    {

        List<Sirket> altTabloList(int p);

        Task<Sirket> GetAsync(int id);

        Task<ICollection<Sirket>> GetAllAsync();

        Task<Sirket> FindAsync(Sirket sirket);

        Task<ICollection<Sirket>> FindAllAsync(bool status);

        Task<Sirket> AddAsync(Sirket sirket);

        Task<IEnumerable<Sirket>> AddAllAsync(IEnumerable<Sirket> sirketList);

        Task<Sirket> UpdateAsync(Sirket sirket, int key);

        Task<int> DeleteAsync(int key);

        Task<int> CountAsync();

        Task<ICollection<sirketW>> OrganizationUserList(IEnumerable<int> Userlist, bool Durumu);
    }
}
