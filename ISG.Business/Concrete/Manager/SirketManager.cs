using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class SirketManager : ISirketService
    {
        private ISirketDal _sirketDal;
     
        public SirketManager(ISirketDal sirketDal)
        {
            _sirketDal = sirketDal;
        }

        public async Task<Sirket> GetAsync(int id)
        {
            return await _sirketDal.GetAsync(id);
        }

        public async Task<ICollection<Sirket>> GetAllAsync()
        {
            return await _sirketDal.GetAllAsync();
        }

        public async Task<Sirket> FindAsync(Sirket sirket)
        {
            return await _sirketDal.FindAsync(p => p.id == sirket.id);
        }

        public async Task<ICollection<Sirket>> FindAllAsync(bool status)
        {
            return await _sirketDal.FindAllAsync(p => p.status==status);
        }
        //startwitht başta arama EndsWith sondan arama yada "where SqlMethods.Like(u.Username,"%"+keyword+"%")"
        //return await _sirketDal.FindAllAsync(p => p.sirketAdi.StartsWith(sirketFilter.name));
        public async Task<Sirket> AddAsync(Sirket sirket)
        {
            return await _sirketDal.AddAsync(sirket);
        }

        public async Task<IEnumerable<Sirket>> AddAllAsync(IEnumerable<Sirket> sirketList)
        {
            return await _sirketDal.AddAllAsync(sirketList);
        }

        public async Task<Sirket> UpdateAsync(Sirket sirket, int key)
        {
            return await _sirketDal.UpdateAsync(sirket, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _sirketDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _sirketDal.CountAsync();
        }


        public List<Sirket> altTabloList(int p)
        {
            return _sirketDal.GetList(px => px.idRef == p).ToList();
        }

        public async Task<ICollection<sirketW>> OrganizationUserList(IEnumerable<int> Userlist, bool Durumu)
        {
            return await _sirketDal.OrganizationUserList(Userlist, Durumu);
        }
    }
}
