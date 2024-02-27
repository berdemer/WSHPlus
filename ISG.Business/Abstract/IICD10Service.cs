using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IICD10Service
	{

		Task<ICD10> GetAsync(int id);

		Task<ICollection<ICD10>> GetAllAsync();

		Task<ICD10> FindAsync(ICD10 icd10);

		Task<ICollection<ICD10>> FindAllAsync(ICD10 icd10);

		Task<ICD10> AddAsync(ICD10 icd10);

		Task<IEnumerable<ICD10>> AddAllAsync(IEnumerable<ICD10> icd10List);

		Task<ICD10> UpdateAsync(ICD10 icd10, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

        Task<ICD10Table> ICDSearch(string Search, int DisplayStart, int DisplayLength);

        Task<ICollection<ICD10>> KontrolluIcd10Kaydi(IEnumerable<ICD10> icd10);

        Task<ICollection<ICD10>> HastalikAdiAra(string value);
    }
}
