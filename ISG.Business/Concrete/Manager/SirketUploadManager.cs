using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
    public class SirketUploadManager : ISirketUploadService
    {
        private ISirketUploaddDal _SirketUploadDal;

        public SirketUploadManager(ISirketUploaddDal SirketUploadDal)
        {
            _SirketUploadDal = SirketUploadDal;
        }

        public async Task<SirketUpload> GetAsync(int id)
        {
            return await _SirketUploadDal.GetAsync(id);
        }

        public async Task<ICollection<SirketUpload>> GetAllAsync()
        {
            return await _SirketUploadDal.GetAllAsync();
        }

        public async Task<SirketUpload> FindAsync(imageUploadFilter ImageUploadFilter)
        {
            return await _SirketUploadDal.FindAsync(p => p.GenericName == ImageUploadFilter.GenericName);
        }

        public async Task<SirketUpload> AddAsync(SirketUpload SirketUpload)
        {
            return await _SirketUploadDal.AddAsync(SirketUpload);
        }

        public async Task<IEnumerable<SirketUpload>> AddAllAsync(IEnumerable<SirketUpload> ImageUploadList)
        {
            return await _SirketUploadDal.AddAllAsync(ImageUploadList);
        }

        public async Task<SirketUpload> UpdateAsync(SirketUpload SirketUpload, int key)
        {
            return await _SirketUploadDal.UpdateAsync(SirketUpload, key);
        }

        public async Task<int> DeleteAsync(long key)
        {
            return await _SirketUploadDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _SirketUploadDal.CountAsync();
        }

        public async Task<ICollection<SirketUpload>> FindAllAsync(imageUploadFilter sirketUploaddFilter)
        {
            return await _SirketUploadDal.FindAllAsync(x => x.DosyaTipi == sirketUploaddFilter.DosyaTipi && x.Sirket_Id == sirketUploaddFilter.Sirket_Id);
        }
        public async Task<ICollection<SirketUpload>> FindAllFileAsync(imageUploadFilter ImageUploadFilter)
        {
            return await _SirketUploadDal.FindAllAsync(p => p.Sirket_Id == ImageUploadFilter.Sirket_Id && p.DosyaTipi == ImageUploadFilter.DosyaTipi && p.DosyaTipiID == ImageUploadFilter.DosyaTipiID);
        }


    }
}
