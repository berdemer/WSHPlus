using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISirketUploadService
    {
        Task<SirketUpload> GetAsync(int id);

        Task<ICollection<SirketUpload>> GetAllAsync();

        Task<SirketUpload> FindAsync(imageUploadFilter sirketUploadFilter);

        Task<ICollection<SirketUpload>> FindAllAsync(imageUploadFilter sirketUploaddFilter);

        Task<ICollection<SirketUpload>> FindAllFileAsync(imageUploadFilter sirketUploadFilter);

        Task<SirketUpload> AddAsync(SirketUpload sirketUpload);

        Task<IEnumerable<SirketUpload>> AddAllAsync(IEnumerable<SirketUpload> SirketUploadList);

        Task<SirketUpload> UpdateAsync(SirketUpload sirketUpload, int key);
        Task<int> DeleteAsync(long key);
        Task<int> CountAsync();
    }
}
