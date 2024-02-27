using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface ITehlikeliIslerDal : IEntityRepository<TehlikeliIsler>
	{
		Task<ICollection<TehlikeliIsler>> TehlikeliIslerAra(string value);
		Task<ICollection<TehlikeliIsler>> GrupList(IEnumerable<string> grupList);
        Task<ICollection<string>> gruplardanAra(string value);


    }
}
