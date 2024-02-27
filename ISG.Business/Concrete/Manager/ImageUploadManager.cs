using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class ImageUploadManager:IImageUploadService
    {
        private IimageUploadDal _imageUploadDal;

        public ImageUploadManager(IimageUploadDal imageUploadDal)
        {
            _imageUploadDal = imageUploadDal;
        }
        public async Task<imageUpload> GetAsync(int id)
        {
            return await _imageUploadDal.GetAsync(id);
        }

        public async Task<ICollection<imageUpload>> GetAllAsync()
        {
            return await _imageUploadDal.GetAllAsync();
        }

        public async Task<imageUpload> FindAsync(imageUploadFilter ImageUploadFilter)
        {
            return await _imageUploadDal.FindAsync(p => p.GenericName == ImageUploadFilter.GenericName);
        }

        public async Task<ICollection<imageUpload>> FindAllAsync(imageUploadFilter ImageUploadFilter)
        {
            return await _imageUploadDal.FindAllAsync(p => p.IdGuid == ImageUploadFilter.IdGuid&&p.Folder==ImageUploadFilter.Folder);
        }

        public async Task<ICollection<imageUpload>> FindAllFileAsync(imageUploadFilter ImageUploadFilter)
        {
            return await _imageUploadDal.FindAllAsync(p => p.IdGuid == ImageUploadFilter.IdGuid && p.Folder == ImageUploadFilter.Folder && p.Protokol == ImageUploadFilter.Protokol);
        }

        public async Task<imageUpload> AddAsync(imageUpload imageUpload)
        {
            return await _imageUploadDal.AddAsync(imageUpload);
        }

        public async Task<IEnumerable<imageUpload>> AddAllAsync(IEnumerable<imageUpload> ImageUploadList)
        {
            return await _imageUploadDal.AddAllAsync(ImageUploadList);
        }

        public async Task<imageUpload> UpdateAsync(imageUpload imageUpload, int key)
        {
            return await _imageUploadDal.UpdateAsync(imageUpload, key);
        }

        public async Task<int> DeleteAsync(long key)
        {
            return await _imageUploadDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _imageUploadDal.CountAsync();
        }

       
    }
}
