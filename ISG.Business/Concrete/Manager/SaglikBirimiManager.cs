using ISG.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISG.Entities.Concrete;
using ISG.DataAccess.Abstract;

namespace ISG.Business.Concrete.Manager
{
    public class SaglikBirimiManager : ISaglikBirimiService
    {
        private ISaglikBirimiDal _saglikBirimiDal;

        public SaglikBirimiManager(ISaglikBirimiDal saglikBirimiDal)
        {
            _saglikBirimiDal = saglikBirimiDal;
        }

        public async Task<IEnumerable<SaglikBirimi>> AddAllAsync(IEnumerable<SaglikBirimi> saglikBirimiList)
        {
            return await _saglikBirimiDal.AddAllAsync(saglikBirimiList);
        }

        public async Task<SaglikBirimi> AddAsync(SaglikBirimi saglikBirimi)
        {
            return await _saglikBirimiDal.AddAsync(saglikBirimi);
        }

        public async Task<int> CountAsync()
        {
            return await _saglikBirimiDal.CountAsync();
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _saglikBirimiDal.DeleteAsync(key);
        }

        public async Task<ICollection<SaglikBirimi>> FindAllAsync(SaglikBirimi saglikBirimi)
        {
            return await _saglikBirimiDal.FindAllAsync(p=>p.StiId==saglikBirimi.StiId&&p.Status==saglikBirimi.Status);
        }

        public async Task<SaglikBirimi> FindAsync(SaglikBirimi saglikBirimi)
        {
            return await _saglikBirimiDal.FindAsync(p => p.Protokol == saglikBirimi.Protokol);
        }

        public async Task<ICollection<SaglikBirimi>> GetAllAsync()
        {
            return await _saglikBirimiDal.GetAllAsync();
        }

        public async Task<SaglikBirimi> GetAsync(int id)
        {
            return await _saglikBirimiDal.GetAsync(id);
        }

        public async Task<SaglikBirimi> ProtokolAl(int id)
        {
            return await _saglikBirimiDal.ProtokolAl(id);
        }

        public async Task<SaglikBirimi> UpdateAsync(SaglikBirimi saglikBirimi, int key)
        {
            return await _saglikBirimiDal.UpdateAsync(saglikBirimi, key);
        }
    }
}
