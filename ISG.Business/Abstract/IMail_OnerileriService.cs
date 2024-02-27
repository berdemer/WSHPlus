using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IMail_OnerileriService
	{

		Task<Mail_Onerileri> GetAsync(int id);

		Task<ICollection<Mail_Onerileri>> GetAllAsync();

		Task<Mail_Onerileri> FindAsync(Mail_Onerileri mail_Onerileri);

		Task<ICollection<Mail_Onerileri>> FindAllAsync(Mail_Onerileri mail_Onerileri);

		Task<Mail_Onerileri> AddAsync(Mail_Onerileri mail_Onerileri);

		Task<IEnumerable<Mail_Onerileri>> AddAllAsync(IEnumerable<Mail_Onerileri> mail_OnerileriList);

		Task<Mail_Onerileri> UpdateAsync(Mail_Onerileri mail_Onerileri, int key);

		Task<int> DeleteAsync(int key);

		Task<int> CountAsync();
        Task<ICollection<Mail_Onerileri>> FindAllStiAsync(int id);
        Task<ICollection<Mail_Onerileri>> FindAllBlmAsync(int stiid, int id);
        Task<IList<MailListesi>> MailOnerileri(int Sti_Id, int Blm_Id, string gonderimSekli);
    }
}
