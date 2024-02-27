using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class PsikolojikTestManager : IPsikolojikTestService
	{
		private IPsikolojikTestDal _psikolojikTestDal;
	 
		public PsikolojikTestManager(IPsikolojikTestDal psikolojikTestDal)
		{
			_psikolojikTestDal = psikolojikTestDal;
		}

		public async Task<PsikolojikTest> GetAsync(int id)
		{
			return await _psikolojikTestDal.GetAsync(id);
		}

		public async Task<ICollection<PsikolojikTest>> GetAllAsync()
		{
			return await _psikolojikTestDal.GetAllAsync();
		}

		public async Task<PsikolojikTest> FindAsync(PsikolojikTest psikolojikTest)
		{
			return await _psikolojikTestDal.FindAsync(p => p.PsikolojikTest_Id == psikolojikTest.PsikolojikTest_Id);
		}

		public async Task<ICollection<PsikolojikTest>> FindAllAsync(PsikolojikTest psikolojikTest)
		{
			return await _psikolojikTestDal.FindAllAsync(p => p.Personel_Id==psikolojikTest.Personel_Id);
		}

		public async Task<PsikolojikTest> AddAsync(PsikolojikTest psikolojikTest)
		{
			return await _psikolojikTestDal.AddAsync(psikolojikTest);
		}

		public async Task<IEnumerable<PsikolojikTest>> AddAllAsync(IEnumerable<PsikolojikTest> psikolojikTestList)
		{
			return await _psikolojikTestDal.AddAllAsync(psikolojikTestList);
		}

		public async Task<PsikolojikTest> UpdateAsync(PsikolojikTest psikolojikTest, int key)
		{
			return await _psikolojikTestDal.UpdateAsync(psikolojikTest, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _psikolojikTestDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _psikolojikTestDal.CountAsync();
		}
	}
}
