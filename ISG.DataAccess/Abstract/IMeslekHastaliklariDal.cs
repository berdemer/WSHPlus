using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface IMeslekHastaliklariDal : IEntityRepository<MeslekHastaliklari>
	{
		Task<ICollection<MeslekHastaliklari>> MeslekHastalikAra(string value);
        Task<ICollection<MeslekHastaliklari>> GrupList(IEnumerable<string> grupList);

    }
}
