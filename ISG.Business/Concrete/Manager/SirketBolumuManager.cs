using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class SirketBolumuManager: ISirketBolumuService
    {
        private ISirketBolumuDal _sirketBolumuDal;
     
        public SirketBolumuManager(ISirketBolumuDal sirketBolumuDal)
        {
            _sirketBolumuDal = sirketBolumuDal;
        }

        public async Task<SirketBolumu> GetAsync(int id)
        {
            return await _sirketBolumuDal.GetAsync(id);
        }

        public async Task<ICollection<SirketBolumu>> GetAllAsync()
        {
            return await _sirketBolumuDal.GetAllAsync();
        }

        public async Task<SirketBolumu> FindAsync(SirketBolumu sirketBolumu)
        {
            return await _sirketBolumuDal.FindAsync(p => p.id == sirketBolumu.id);
        }

        public async Task<ICollection<SirketBolumu>> FindAllAsync(bool status,int sirketId)
        {
            return await _sirketBolumuDal.FindAllAsync(p => p.status==status && p.Sirket_id==sirketId);
        }
        //startwitht başta arama EndsWith sondan arama yada "where SqlMethods.Like(u.Username,"%"+keyword+"%")"
        //return await _SirketBolumuDal.FindAllAsync(p => p.SirketBolumuAdi.StartsWith(SirketBolumuFilter.name));
        public async Task<SirketBolumu> AddAsync(SirketBolumu sirketBolumu)
        {
            return await _sirketBolumuDal.AddAsync(sirketBolumu);
        }

        public async Task<IEnumerable<SirketBolumu>> AddAllAsync(IEnumerable<SirketBolumu> sirketBolumuList)
        {
            return await _sirketBolumuDal.AddAllAsync(sirketBolumuList);
        }

        public async Task<SirketBolumu> UpdateAsync(SirketBolumu sirketBolumu, int key)
        {
            return await _sirketBolumuDal.UpdateAsync(sirketBolumu, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _sirketBolumuDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _sirketBolumuDal.CountAsync();
        }


        public List<SirketBolumu> altTabloList(int p)
        {
            return _sirketBolumuDal.GetList(px => px.idRef == p).ToList();
        }

        public void Add(SirketBolumu sirketBolumu)
        {
            _sirketBolumuDal.Add(sirketBolumu);
        }
    }
}
