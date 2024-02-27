using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class CalismaAnaliziManager : ICalismaAnaliziService
	{
		private ICalismaAnaliziDal _calismaAnaliziDal;
	 
		public CalismaAnaliziManager(ICalismaAnaliziDal calismaAnaliziDal)
		{
			_calismaAnaliziDal = calismaAnaliziDal;
		}

		public async Task<CalismaAnalizi> GetAsync(int id)
		{
			return await _calismaAnaliziDal.GetAsync(id);
		}

		public async Task<ICollection<CalismaAnalizi>> GetAllAsync()
		{
			return await _calismaAnaliziDal.GetAllAsync();
		}

		public async Task<CalismaAnalizi> FindAsync(CalismaAnalizi calismaAnalizi)
		{
			return await _calismaAnaliziDal.FindAsync(p => p.Bolum_Id== calismaAnalizi.Bolum_Id&& p.Sirket_Id == calismaAnalizi.Sirket_Id);
		}

		public async Task<ICollection<CalismaAnalizi>> FindAllAsync(CalismaAnalizi calismaAnalizi)
		{
			return await _calismaAnaliziDal.FindAllAsync(p => p.Meslek_Kodu == calismaAnalizi.Meslek_Kodu);
		}

		public async Task<CalismaAnalizi> AddAsync(CalismaAnalizi calismaAnalizi)
		{
			return await _calismaAnaliziDal.AddAsync(calismaAnalizi);
		}

		public async Task<IEnumerable<CalismaAnalizi>> AddAllAsync(IEnumerable<CalismaAnalizi> calismaAnaliziList)
		{
			return await _calismaAnaliziDal.AddAllAsync(calismaAnaliziList);
		}

		public async Task<CalismaAnalizi> UpdateAsync(CalismaAnalizi calismaAnalizi, int key)
		{
			return await _calismaAnaliziDal.UpdateAsync(calismaAnalizi, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _calismaAnaliziDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _calismaAnaliziDal.CountAsync();
		}

        public async Task<CalismaAnalizi> BolumAraAsync(int Bolum_Id,int Sirket_Id)
        {
		   return await _calismaAnaliziDal.BolumAraAsync(Bolum_Id, Sirket_Id);
		}

        public async Task<CalismaAnalizi> MeslekAraAsync(string Meslek_Kodu)
        {
			return await _calismaAnaliziDal.MeslekAra(Meslek_Kodu);

		}
		public async Task<object> MeslekListesi()
        {
			return await _calismaAnaliziDal.MeslekListesi();

		}
    }
}
