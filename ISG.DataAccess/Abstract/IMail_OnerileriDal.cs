using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IMail_OnerileriDal : IEntityRepository<Mail_Onerileri>
	{
        Task<IList<MailListesi>> MailOnerileri(int Sti_Id, int Blm_Id, string gonderimSekli);
    }
}
