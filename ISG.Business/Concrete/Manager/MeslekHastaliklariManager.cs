using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class MeslekHastaliklariManager : IMeslekHastaliklariService
    {
        private IMeslekHastaliklariDal _meslekHastaliklariDal;

        public MeslekHastaliklariManager(IMeslekHastaliklariDal meslekHastaliklariDal)
        {
            _meslekHastaliklariDal = meslekHastaliklariDal;
        }

        public async Task<MeslekHastaliklari> GetAsync(int id)
        {
            return await _meslekHastaliklariDal.GetAsync(id);
        }

        public async Task<ICollection<MeslekHastaliklari>> GetAllAsync()
        {
            return await _meslekHastaliklariDal.GetAllAsync();
        }

        public async Task<MeslekHastaliklari> FindAsync(MeslekHastaliklari meslekHastaliklari)
        {
            return await _meslekHastaliklariDal.FindAsync(p => p.MeslekHastaliklari_Id == meslekHastaliklari.MeslekHastaliklari_Id);
        }

        public async Task<ICollection<MeslekHastaliklari>> FindAllAsync(MeslekHastaliklari meslekHastaliklari)
        {
            return await _meslekHastaliklariDal.FindAllAsync(p => p.grubu == meslekHastaliklari.grubu);
        }

        public async Task<MeslekHastaliklari> AddAsync(MeslekHastaliklari meslekHastaliklari)
        {
            return await _meslekHastaliklariDal.AddAsync(meslekHastaliklari);
        }

        public async Task<IEnumerable<MeslekHastaliklari>> AddAllAsync(IEnumerable<MeslekHastaliklari> meslekHastaliklariList)
        {
            return await _meslekHastaliklariDal.AddAllAsync(meslekHastaliklariList);
        }

        public async Task<MeslekHastaliklari> UpdateAsync(MeslekHastaliklari meslekHastaliklari, int key)
        {
            return await _meslekHastaliklariDal.UpdateAsync(meslekHastaliklari, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _meslekHastaliklariDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _meslekHastaliklariDal.CountAsync();
        }

        public async Task<ICollection<MeslekHastaliklari>> MeslekHastalikAra(string value)
        {
            return await _meslekHastaliklariDal.MeslekHastalikAra(value);
        }

        public async Task<ICollection<MeslekHastaliklari>> GrupList(IEnumerable<string> grupList)
        {
            return await _meslekHastaliklariDal.GrupList(grupList);
        }
    }
}
