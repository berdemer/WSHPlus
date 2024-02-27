using ISG.Entities.ComplexType;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IMailService
	{
        Task<bool> SendEmailAsync(M_Client cl, M_Message ml);

	}
}
