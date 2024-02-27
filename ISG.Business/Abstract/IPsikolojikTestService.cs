using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPsikolojikTestService
	{

		Task<PsikolojikTest> GetAsync(int id);

		Task<ICollection<PsikolojikTest>> GetAllAsync();

		Task<PsikolojikTest> FindAsync(PsikolojikTest psikolojikTest);

		Task<ICollection<PsikolojikTest>> FindAllAsync(PsikolojikTest psikolojikTest);

		Task<PsikolojikTest> AddAsync(PsikolojikTest psikolojikTest);

		Task<IEnumerable<PsikolojikTest>> AddAllAsync(IEnumerable<PsikolojikTest> psikolojikTestList);

		Task<PsikolojikTest> UpdateAsync(PsikolojikTest psikolojikTest, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
	}
}
