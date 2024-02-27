using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IBolumRiskiService
	{

		Task<BolumRiski> GetAsync(int id);

		Task<ICollection<BolumRiski>> GetAllAsync();

		Task<BolumRiski> FindAsync(BolumRiski bolumRiski);

		Task<ICollection<BolumRiski>> FindAllAsync(BolumRiski bolumRiski);

		Task<BolumRiski> AddAsync(BolumRiski bolumRiski);

		Task<IEnumerable<BolumRiski>> AddAllAsync(IEnumerable<BolumRiski> bolumRiskiList);

		Task<BolumRiski> UpdateAsync(BolumRiski bolumRiski, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
        Task<Object> Muayene_Durum_Listesi(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year);
        Task<Object> BolumlerinIsKazaSayilari(int Sirket_Id, string muayeneDurumu, string muayeneSonucu, int year);
        Task<Object> PeriyodikMuayeneTakibiGelenler(int Sirket_Id, int Yil, int ay, int sure);
        Task<Object> AsiTakibiGelenler(int Sirket_Id, int Yil, int ay);
        Task<Object> Gunluk_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih);
        Task<Object> Aylik_Revir_Islemleri(int SaglikBirimi_Id, DateTime tarih);
        Task<object> EngelliTakibi(int Sirket_Id);
        Task<object> KronikHastaTakibi(int Sirket_Id);
        Task<object> AllerjiHastaTakibi(int Sirket_Id);
        Task<object> AliskanlikHastaTakibi(int Sirket_Id);
        Task<object> DisRaporAnalizi(int Sirket_Id, int Yil, int ay, string muayeneTuru);

    }
}
