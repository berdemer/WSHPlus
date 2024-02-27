using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ISG.Business.Concrete.Manager
{
	public class PersonelManager : IPersonelService
	{
		private IPersonelDal _personelDal;
	 
		public PersonelManager(IPersonelDal personelDal)
		{
			_personelDal = personelDal;
		}

		public async Task<Personel> GetAsync(int id)
		{
			return await _personelDal.GetAsync(id);
		}

		public async Task<ICollection<Personel>> GetAllAsync()
		{
			return await _personelDal.GetAllAsync();
		}

		public async Task<Personel> FindAsync(Personel personel)
		{
			return await _personelDal.FindAsync(p => p.TcNo== personel.TcNo);
		}


        public async Task<Personel> SicilNoKontrol(string sicil)
        {
            return await _personelDal.SicilNoKontrol(sicil);
        }


        public async Task<ICollection<Personel>> FindAllAsync(Personel personel)
		{
			return await _personelDal.FindAllAsync(p => p.Status==personel.Status);
		}

		public async Task<Personel> AddAsync(Personel personel)
		{
			return await _personelDal.AddAsync(personel);
		}

		public async Task<IEnumerable<Personel>> AddAllAsync(IEnumerable<Personel> personelList)
		{
			return await _personelDal.AddAllAsync(personelList);
		}

		public async Task<Personel> UpdateAsync(Personel personel, int key)
		{
			return await _personelDal.UpdateAsync(personel, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _personelDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _personelDal.CountAsync();
		}
		public async Task<Personel> TcNoKontrol(string tcNo)
		{
			return await _personelDal.TcNoKontrol(tcNo);
		}


		public async Task<ICollection<Personel>> SirketPersonelleri(int sirket_id)
		{
			return await _personelDal.FindAllAsync(p => p.Sirket_Id == sirket_id && p.Status==true);
		}

		public async Task<ICollection<Personel>> BolumPersonelleri(int bolum_id)
		{
			return await _personelDal.FindAllAsync(p => p.Bolum_Id == bolum_id);
		}


		public async Task<ICollection<PersonelView>> PersonelDeparmentView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durum)
		{
			return await _personelDal.PersonelDeparmentView(Sirketlerimiz, Bolumlerimiz, Durum);
		}

        public async Task<Personel> GuidKontrol(Guid GuidId)
        {
            return await _personelDal.GuidKontrol(GuidId);
        }

        public async Task<ICollection<PersonelView>> PersonelAllDeparmentView(bool Durum)
        {
            return await _personelDal.PersonelAllDeparmentView(Durum);
        }

        public async Task<personelCard> PersonelCardView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength)
        {
            return await _personelDal.PersonelCardView(Sirketlerimiz, Durumu, Search, DisplayStart, DisplayLength);
        }

        public async Task<personelCard> PersonelListOrderSearchView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength, int sortColumnIndex, string SortDir)
        {
            return await _personelDal.PersonelListOrderSearchView(Sirketlerimiz,Durumu,Search,DisplayStart,DisplayLength,sortColumnIndex,SortDir);
        }

        public async Task<personelCard> PersonelDeparmentCardView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durumu, int DisplayStart, int DisplayLength)
        {
            return await _personelDal.PersonelDeparmentCardView(Sirketlerimiz,Bolumlerimiz,Durumu,DisplayStart,DisplayLength);
        }

        public async Task<object> EgitimPersonelleriListesi(int Sirket_Id, bool Durumu, string Search)
        {
			return await _personelDal.EgitimPersonelleriListesi(Sirket_Id, Durumu, Search);
		}

        public async Task<object> EgitimPersonelleriTumListesi(int Sirket_Id, bool Durumu)
        {
			return await _personelDal.EgitimPersonelleriTumListesi(Sirket_Id, Durumu);
		}
    }
}
