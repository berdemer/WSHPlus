using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISirketAtamaService
    {
        Task<SirketAtama> GetAsync(int id);

        Task<ICollection<SirketAtama>> GetAllAsync();

        Task<SirketAtama> FindAsync(SirketAtama sirketAtama);

        Task<ICollection<SirketAtama>> FindAllAsync(int sirketId);

        Task<ICollection<SirketAtama>> FindAllOrgAsync(string user);

        Task<SirketAtama> AddAsync(SirketAtama sirketAtama);

        Task<IEnumerable<SirketAtama>> AddAllAsync(IEnumerable<SirketAtama> sirketAtamaList);

        Task<SirketAtama> UpdateAsync(SirketAtama sirketAtama, int key);
        Task<int> DeleteAsync(int key);
        Task<int> CountAsync();

        Task<ICollection<SirketAtama>> UserSirketAtamasiAsync(string UserId, int sirket_Id);
        Task<bool> AtamaKontrol(string user_Id, int Sirket_Id);
    }
}
