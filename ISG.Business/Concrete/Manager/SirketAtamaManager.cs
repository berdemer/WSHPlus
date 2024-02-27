using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class SirketAtamaManager : ISirketAtamaService
    {
        public ISirketAtamaDal _sirketAtamaDal { get; set; }
        public SirketAtamaManager(ISirketAtamaDal sirketAtamaDal)
        {
            _sirketAtamaDal = sirketAtamaDal;
        }
        public async Task<SirketAtama> GetAsync(int id)
        {
            return await _sirketAtamaDal.GetAsync(id);
        }

        public async Task<ICollection<SirketAtama>> GetAllAsync()
        {
            return await _sirketAtamaDal.GetAllAsync();
        }

        public async Task<SirketAtama> FindAsync(SirketAtama sirketAtama)
        {
            return await _sirketAtamaDal.FindAsync(p => p.SirketAtama_id == sirketAtama.SirketAtama_id);
        }

        public async Task<ICollection<SirketAtama>> FindAllAsync(int sirketId)
        {
            return await _sirketAtamaDal.FindAllAsync(p => p.Sirket_id == sirketId);
        }

        public async Task<SirketAtama> AddAsync(SirketAtama sirketAtama)
        {
            return await _sirketAtamaDal.AddAsync(sirketAtama);
        }

        public async Task<IEnumerable<SirketAtama>> AddAllAsync(IEnumerable<SirketAtama> sirketAtamaList)
        {
            return await _sirketAtamaDal.AddAllAsync(sirketAtamaList);
        }

        public async Task<SirketAtama> UpdateAsync(SirketAtama sirketAtama, int key)
        {
            return await _sirketAtamaDal.UpdateAsync(sirketAtama, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _sirketAtamaDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _sirketAtamaDal.CountAsync();
        }

        public async Task<ICollection<SirketAtama>> UserSirketAtamasiAsync(string UserId, int sirket_Id)
        {
            return await _sirketAtamaDal.FindAllAsync(p => p.Sirket_id == sirket_Id && p.uzmanPersonelId==UserId);
        }


        public async Task<bool> AtamaKontrol(string user_Id, int Sirket_Id)
        {
            return await _sirketAtamaDal.AtamaKontrol(user_Id, Sirket_Id);
        }


        public async Task<ICollection<SirketAtama>> FindAllOrgAsync(string user)
        {
            return await _sirketAtamaDal.FindAllAsync(p => p.uzmanPersonelId == user);
        }

    }
}
