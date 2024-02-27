using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IICD10Dal:IEntityRepository<ICD10>
	{
        Task<ICollection<ICD10>> KontrolluIcd10Kaydi(IEnumerable<ICD10> icd10);
        Task<ICD10Table> ICDSearch(string Search, int DisplayStart, int DisplayLength);
        Task<ICollection<ICD10>> HastalikAdiAra(string value);
    }
}
