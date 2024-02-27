using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface IPersonelDal:IEntityRepository<Personel>
	{
		Task<Personel> TcNoKontrol(string tcNo);
        Task<Personel> SicilNoKontrol(string sicil);
        Task<ICollection<PersonelView>> PersonelDeparmentView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durum);
        Task<ICollection<PersonelView>> PersonelAllDeparmentView( bool Durum);
        Task<Personel> GuidKontrol(Guid GuidId);
        Task<personelCard> PersonelCardView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength);
        Task<personelCard> PersonelDeparmentCardView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durumu, int DisplayStart, int DisplayLength);
        Task<personelCard> PersonelListOrderSearchView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength, int sortColumnIndex, string SortDir);
        Task<Object> EgitimPersonelleriListesi(int Sirket_Id, bool Durumu, string Search);
        Task<Object> EgitimPersonelleriTumListesi(int Sirket_Id, bool Durumu);
    }
}
