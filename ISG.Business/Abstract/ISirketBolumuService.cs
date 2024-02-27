using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISirketBolumuService
    {
        void Add(SirketBolumu sirketBolumu);
        List<SirketBolumu> altTabloList(int p);

        Task<SirketBolumu> GetAsync(int id);

        Task<ICollection<SirketBolumu>> GetAllAsync();

        Task<SirketBolumu> FindAsync(SirketBolumu sirketBolumu);

        Task<ICollection<SirketBolumu>> FindAllAsync(bool status, int sirketId);

        Task<SirketBolumu> AddAsync(SirketBolumu sirketBolumu);

        Task<IEnumerable<SirketBolumu>> AddAllAsync(IEnumerable<SirketBolumu> sirketBolumuList);

        Task<SirketBolumu> UpdateAsync(SirketBolumu sirketBolumu, int key);
        Task<int> DeleteAsync(int key);
        Task<int> CountAsync();
    }
}
