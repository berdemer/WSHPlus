using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class DisRaporManager : IDisRaporService
	{
		private IDisRaporDal _disRaporDal;
	 
		public DisRaporManager(IDisRaporDal disRaporDal)
		{
			_disRaporDal = disRaporDal;
		}

		public async Task<DisRapor> GetAsync(int id)
		{
			return await _disRaporDal.GetAsync(id);
		}

		public async Task<ICollection<DisRapor>> GetAllAsync()
		{
			return await _disRaporDal.GetAllAsync();
		}

		public async Task<DisRapor> FindAsync(DisRapor disRapor)
		{
			return await _disRaporDal.FindAsync(p => p.DisRapor_Id == disRapor.DisRapor_Id);
		}

		public async Task<ICollection<DisRapor>> FindAllAsync(DisRapor disRapor)
		{
			return await _disRaporDal.FindAllAsync(p => p.Personel_Id==disRapor.DisRapor_Id);
		}

		public async Task<DisRapor> AddAsync(DisRapor disRapor)
		{
			return await _disRaporDal.AddAsync(disRapor);
		}

		public async Task<IEnumerable<DisRapor>> AddAllAsync(IEnumerable<DisRapor> disRaporList)
		{
			return await _disRaporDal.AddAllAsync(disRaporList);
		}

		public async Task<DisRapor> UpdateAsync(DisRapor disRapor, int key)
		{
			return await _disRaporDal.UpdateAsync(disRapor, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _disRaporDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _disRaporDal.CountAsync();
		}

        public async Task<DisRapor> MukerrerDisRaporKontrol(DateTime baslangic, int Personel_Id)
        {
			return await _disRaporDal.MukerrerDisRaporKontrol(baslangic, Personel_Id);
		}

        public async Task<bool> MukerrerDisRaporTakibi(DateTime baslangic, DateTime bitis, int Personel_Id)
        {
            return await _disRaporDal.MukerrerDisRaporTakibi(baslangic, bitis, Personel_Id);
        }
    }
}
