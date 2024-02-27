using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IPersonelService
	{

		Task<Personel> GetAsync(int id);

		Task<ICollection<Personel>> GetAllAsync();

		Task<Personel> FindAsync(Personel personel);

        Task<Personel> SicilNoKontrol(string sicil);

        Task<ICollection<Personel>> FindAllAsync(Personel personel);

		Task<Personel> AddAsync(Personel personel);

		Task<IEnumerable<Personel>> AddAllAsync(IEnumerable<Personel> personelList);

		Task<Personel> UpdateAsync(Personel personel, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();

		Task<Personel> TcNoKontrol(string tcNo);

        Task<Personel> GuidKontrol(Guid GuidId);

        Task<ICollection<Personel>> SirketPersonelleri(int sirket_id);

		Task<ICollection<Personel>> BolumPersonelleri(int bolum_id);

		Task<ICollection<PersonelView>> PersonelDeparmentView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durum);

        Task<ICollection<PersonelView>> PersonelAllDeparmentView(bool Durum);

        Task<personelCard> PersonelCardView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength);

        Task<personelCard> PersonelDeparmentCardView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durumu, int DisplayStart, int DisplayLength);

        Task<personelCard> PersonelListOrderSearchView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength, int sortColumnIndex, string SortDir);

		Task<Object> EgitimPersonelleriListesi(int Sirket_Id, bool Durumu, string Search);

		Task<Object> EgitimPersonelleriTumListesi(int Sirket_Id, bool Durumu);
	}
}
