using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ISG.Business.Concrete.Manager
{
    public class BolumRiskiManager : IBolumRiskiService
    {
        private IBolumRiskiDal _bolumRiskiDal;

        public BolumRiskiManager(IBolumRiskiDal bolumRiskiDal)
        {
            _bolumRiskiDal = bolumRiskiDal;
        }

        public async Task<BolumRiski> GetAsync(int id)
        {
            return await _bolumRiskiDal.GetAsync(id);
        }

        public async Task<ICollection<BolumRiski>> GetAllAsync()
        {
            return await _bolumRiskiDal.GetAllAsync();
        }

        public async Task<BolumRiski> FindAsync(BolumRiski bolumRiski)
        {
            return await _bolumRiskiDal.FindAsync(p => p.Bolum_Id == bolumRiski.Bolum_Id && p.Sirket_Id == bolumRiski.Sirket_Id);
        }

        public async Task<ICollection<BolumRiski>> FindAllAsync(BolumRiski bolumRiski)
        {
            return await _bolumRiskiDal.FindAllAsync(p => p.Bolum_Id == bolumRiski.Bolum_Id && p.Sirket_Id == bolumRiski.Sirket_Id);
        }

        public async Task<BolumRiski> AddAsync(BolumRiski bolumRiski)
        {
            return await _bolumRiskiDal.AddAsync(bolumRiski);
        }

        public async Task<IEnumerable<BolumRiski>> AddAllAsync(IEnumerable<BolumRiski> bolumRiskiList)
        {
            return await _bolumRiskiDal.AddAllAsync(bolumRiskiList);
        }

        public async Task<BolumRiski> UpdateAsync(BolumRiski bolumRiski, int key)
        {
            return await _bolumRiskiDal.UpdateAsync(bolumRiski, key);
        }

        public async Task<int> DeleteAsync(int key)
        {
            return await _bolumRiskiDal.DeleteAsync(key);
        }

        public async Task<int> CountAsync()
        {
            return await _bolumRiskiDal.CountAsync();
        }

        public async Task<object> Muayene_Durum_Listesi(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            return await _bolumRiskiDal.Muayene_Durum_Listesi(Sirket_Id, muayeneDurumu, muayeneSonucu,year);
        }

        public async Task<object> BolumlerinIsKazaSayilari(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year)
        {
            return await _bolumRiskiDal.BolumlerinIsKazaSayilari(Sirket_Id, muayeneDurumu, muayeneSonucu, year);
        }

        public async Task<object> PeriyodikMuayeneTakibiGelenler(int Sirket_Id, int Yil, int ay, int sure)
        {
            return await _bolumRiskiDal.PeriyodikMuayeneTakibiGelenler(Sirket_Id,Yil,ay,sure);
        }

        public async Task<object> AsiTakibiGelenler(int Sirket_Id, int Yil, int ay)
        {
            return await _bolumRiskiDal.AsiTakibiGelenler(Sirket_Id, Yil, ay);
        }

        public async Task<object> Gunluk_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih)
        {
            return await _bolumRiskiDal.Gunluk_Revir_Islemleri(SaglikBirimi_Id, tarih);
        }

        public async Task<object> Aylik_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih)
        {
            return await _bolumRiskiDal.Aylik_Revir_Islemleri(SaglikBirimi_Id, tarih);
        }

        public async Task<object> EngelliTakibi(int Sirket_Id)
        {
            return await _bolumRiskiDal.EngelliTakibi(Sirket_Id);
        }

        public async Task<object> KronikHastaTakibi(int Sirket_Id)
        {
            return await _bolumRiskiDal.KronikHastaTakibi(Sirket_Id);
        }

        public async Task<object> AllerjiHastaTakibi(int Sirket_Id)
        {
            return await _bolumRiskiDal.AllerjiHastaTakibi(Sirket_Id);
        }

        public async Task<object> AliskanlikHastaTakibi(int Sirket_Id)
        {
            return await _bolumRiskiDal.AliskanlikHastaTakibi(Sirket_Id);
        }

        public async Task<object> DisRaporAnalizi(int Sirket_Id, int Yil, int ay, string muayeneTuru)
        {
            return await _bolumRiskiDal.DisRaporAnalizi(Sirket_Id,Yil,ay, muayeneTuru);
        }
    }
}
