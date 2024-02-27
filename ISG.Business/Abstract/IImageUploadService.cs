using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface IImageUploadService
    {
        Task<imageUpload> GetAsync(int id);

        Task<ICollection<imageUpload>> GetAllAsync();

        Task<imageUpload> FindAsync(imageUploadFilter ImageUploadFilter);

        Task<ICollection<imageUpload>> FindAllAsync(imageUploadFilter ImageUploadFilter);

        Task<ICollection<imageUpload>> FindAllFileAsync(imageUploadFilter ImageUploadFilter);

        Task<imageUpload> AddAsync(imageUpload imageUpload);

        Task<IEnumerable<imageUpload>> AddAllAsync(IEnumerable<imageUpload> ImageUploadList);

        Task<imageUpload> UpdateAsync(imageUpload imageUpload, int key);
        Task<int> DeleteAsync(long key);
        Task<int> CountAsync();
    }
}
