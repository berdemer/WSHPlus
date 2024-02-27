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
    public class SirketDetayiManager:ISirketDetayiService
    {
        private ISirketDetayiDal _sirketDetayiDal;
        public SirketDetayiManager(ISirketDetayiDal sirketDetayiDal)
        {
            _sirketDetayiDal = sirketDetayiDal;
        }
        public async Task<SirketDetayi> GetAsync(int id)
        {
            return await _sirketDetayiDal.GetAsync(id);
        }

        public async Task<SirketDetayi> AddAsync(SirketDetayi sirketDetayi)
        {
            return await _sirketDetayiDal.AddAsync(sirketDetayi);
        }

        public async Task<SirketDetayi> UpdateAsync(SirketDetayi sirketDetayi, int key)
        {
            return await _sirketDetayiDal.UpdateAsync(sirketDetayi, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _sirketDetayiDal.DeleteAsync(key);
        }
    }
}
