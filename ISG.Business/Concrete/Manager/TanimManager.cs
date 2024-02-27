using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ISG.Business.Concrete.Manager
{
    public class TanimManager : ITanimService
    {
        private ITanimDal _tanimDal;

        public TanimManager(ITanimDal tanimDal)
        {
            _tanimDal = tanimDal;
        }

        public async Task<Tanim> GetAsync(int id)
        {
            return await _tanimDal.GetAsync(id);
        }

        public async Task<ICollection<Tanim>> GetAllAsync()
        {
            return await _tanimDal.GetAllAsync();
        }

        public async Task<Tanim> FindAsync(Tanim tanim)
        {
            return await _tanimDal.FindAsync(p => p.tanim_Id == tanim.tanim_Id);
        }

        public async Task<ICollection<Tanim>> FindAllAsync(Tanim tanim)
        {
            return await _tanimDal.FindAllAsync(p => p.tanimAdi==tanim.tanimAdi);
        }

        public async Task<ICollection<Tanim>> llceAllAsync(Tanim tanim)
        {
            return await _tanimDal.FindAllAsync(p => p.ifadeBagimliligi==tanim.ifadeBagimliligi);
        }

        public async Task<Tanim> AddAsync(Tanim tanim)
        {
            return await _tanimDal.AddAsync(tanim);
        }

        public async Task<IEnumerable<Tanim>> AddAllAsync(IEnumerable<Tanim> tanimList)
        {
            return await _tanimDal.AddAllAsync(tanimList);
        }

        public async Task<Tanim> UpdateAsync(Tanim tanim, int key)
        {
            return await _tanimDal.UpdateAsync(tanim, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _tanimDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _tanimDal.CountAsync();
        }

        public async Task<ICollection<Tanim>> SikayetAra(string value)
        {
            return await _tanimDal.SikayetAra(value);
        }

        public async Task<ICollection<Tanim>> BulguAra(string value)
        {
            return await _tanimDal.BulguAra(value);
        }
    }
}
