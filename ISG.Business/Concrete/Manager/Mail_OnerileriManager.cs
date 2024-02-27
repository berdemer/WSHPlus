using ISG.Business.Abstract;
using ISG.DataAccess.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.Business.Concrete.Manager
{
	public class Mail_OnerileriManager : IMail_OnerileriService
	{
		private IMail_OnerileriDal _mail_OnerileriDal;
	 
		public Mail_OnerileriManager(IMail_OnerileriDal mail_OnerileriDal)
		{
			_mail_OnerileriDal = mail_OnerileriDal;
		}

		public async Task<Mail_Onerileri> GetAsync(int id)
		{
			return await _mail_OnerileriDal.GetAsync(id);
		}

		public async Task<ICollection<Mail_Onerileri>> GetAllAsync()
		{
			return await _mail_OnerileriDal.GetAllAsync();
		}

		public async Task<Mail_Onerileri> FindAsync(Mail_Onerileri mail_Onerileri)
		{
			return await _mail_OnerileriDal.FindAsync(p => p.Mail_Onerileri_Id== mail_Onerileri.Mail_Onerileri_Id);
		}

		public async Task<ICollection<Mail_Onerileri>> FindAllAsync(Mail_Onerileri mail_Onerileri)
		{
			return await _mail_OnerileriDal.FindAllAsync(p => p.OneriTanimi==mail_Onerileri.OneriTanimi&&p.Sirket_Id==mail_Onerileri.Sirket_Id);
		}

        public async Task<ICollection<Mail_Onerileri>> FindAllStiAsync(int id)
        {
            return await _mail_OnerileriDal.FindAllAsync(p =>p.Sirket_Id == id);
        }

        public async Task<ICollection<Mail_Onerileri>> FindAllBlmAsync(int stiid,int id)
        {
            return await _mail_OnerileriDal.FindAllAsync(p => (p.Sirket_Id== stiid && p.TumSirketteOneriListesinde==true)|| p.Bolum_Id == id);
        }

        public async Task<Mail_Onerileri> AddAsync(Mail_Onerileri mail_Onerileri)
		{
			return await _mail_OnerileriDal.AddAsync(mail_Onerileri);
		}

		public async Task<IEnumerable<Mail_Onerileri>> AddAllAsync(IEnumerable<Mail_Onerileri> mail_OnerileriList)
		{
			return await _mail_OnerileriDal.AddAllAsync(mail_OnerileriList);
		}

		public async Task<Mail_Onerileri> UpdateAsync(Mail_Onerileri mail_Onerileri, int key)
		{
			return await _mail_OnerileriDal.UpdateAsync(mail_Onerileri, key);
		}

		public async Task<int> DeleteAsync(int key)
		{
			return await _mail_OnerileriDal.DeleteAsync(key);
		}

		public async Task<int> CountAsync()
		{
			return await _mail_OnerileriDal.CountAsync();
		}

        public async  Task<IList<MailListesi>> MailOnerileri(int Sti_Id, int Blm_Id, string gonderimSekli)
        {
            return await _mail_OnerileriDal.MailOnerileri(Sti_Id, Blm_Id, gonderimSekli);
        }
    }
}
