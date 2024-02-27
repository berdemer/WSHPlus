using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISG.Entities.ComplexType;
using System;

namespace ISG.Business.Concrete.Manager
{
    public class ICD10Manager : IICD10Service
	{
		private IICD10Dal _icd10Dal;
	 
		public ICD10Manager(IICD10Dal icd10Dal)
		{
			_icd10Dal = icd10Dal;
		}

		public async Task<ICD10> GetAsync(int id)
		{
			return await _icd10Dal.GetAsync(id);
		}

		public async Task<ICollection<ICD10>> GetAllAsync()
		{
			return await _icd10Dal.GetAllAsync();
		}

		public async Task<ICD10> FindAsync(ICD10 icd10)
		{
			return await _icd10Dal.FindAsync(p => p.ICD10Code== icd10.ICD10Code);
		}

		public async Task<ICollection<ICD10>> FindAllAsync(ICD10 icd10)
		{
			return await _icd10Dal.FindAllAsync(p => p.ICD10_Id==icd10.ICD10_Id);
		}

		public async Task<ICD10> AddAsync(ICD10 icd10)
		{
			return await _icd10Dal.AddAsync(icd10);
		}

		public async Task<IEnumerable<ICD10>> AddAllAsync(IEnumerable<ICD10> icd10List)
		{
			return await _icd10Dal.AddAllAsync(icd10List);
		}

		public async Task<ICD10> UpdateAsync(ICD10 icd10, int key)
		{
			return await _icd10Dal.UpdateAsync(icd10, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _icd10Dal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _icd10Dal.CountAsync();
		}

        public async Task<ICollection<ICD10>> KontrolluIcd10Kaydi(IEnumerable<ICD10> icd10)
        {
            return await _icd10Dal.KontrolluIcd10Kaydi(icd10);
        }

        public async Task<ICD10Table> ICDSearch(string Search, int DisplayStart, int DisplayLength)
        {
            return await _icd10Dal.ICDSearch(Search, DisplayStart, DisplayLength);
        }

        public async Task<ICollection<ICD10>> HastalikAdiAra(string value)
        {
            return await _icd10Dal.HastalikAdiAra(value);
        }
    }
}
