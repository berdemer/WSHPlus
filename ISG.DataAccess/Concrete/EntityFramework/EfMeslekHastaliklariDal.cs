using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using NinjaNye.SearchExtensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfMeslekHastaliklariDal : EfEntityRepositoryBase<MeslekHastaliklari, ISGContext>, IMeslekHastaliklariDal
	{
		public async Task<ICollection<MeslekHastaliklari>> MeslekHastalikAra(string value)
		{
			using (var context = new ISGContext())
			{
				return await context.MeslekHastaliklari.Where(x => new[] { x.meslekHastalik }.Any(s => s.Contains(value))).Distinct().ToListAsync();
			}
		}

		public async Task<ICollection<MeslekHastaliklari>> GrupList(IEnumerable<string> grupList)
		{
			using (var context = new ISGContext())
			{
                var sd = await (from p in context.MeslekHastaliklari
							  where grupList.Contains(p.grubu) 
							  select new
							  {
                                  p.MeslekHastaliklari_Id,
                                  p.grubu,
                                  p.meslekHastalik,
                                  p.sure
							  }).Distinct().ToListAsync();

                return sd.Select(p => new MeslekHastaliklari()
                {
                    MeslekHastaliklari_Id = p.MeslekHastaliklari_Id,
                    grubu = p.grubu,
                    meslekHastalik = p.meslekHastalik,
                    sure = p.sure
                }).ToList();

            }
		}


	}
}
