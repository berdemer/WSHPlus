using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfTehlikeliIslerDal : EfEntityRepositoryBase<TehlikeliIsler, ISGContext>, ITehlikeliIslerDal
	{
		public async Task<ICollection<TehlikeliIsler>> TehlikeliIslerAra(string value)
		{
			using (var context = new ISGContext())
			{
				return await context.TehlikeliIsler.Where(x => new[] { x.meslek }.Any(s => s.Contains(value))).Distinct().ToListAsync();
			}
		}

        public async Task<ICollection<string>> gruplardanAra(string value)
        {
            using (var context = new ISGContext())
            {
                return await context.TehlikeliIsler.Where(x => new[] { x.meslek }.Any(s => s.Contains(value))).Select(x=>x.grubu).Distinct().ToListAsync();
            }
        }

        public async Task<ICollection<TehlikeliIsler>> GrupList(IEnumerable<string> grupList)
		{
			using (var context = new ISGContext())
			{
				var sd= await (from p in context.TehlikeliIsler
							  where grupList.Contains(p.grubu)
							  select new 
							  {
								  TehlikeliIsler_Id = p.TehlikeliIsler_Id,
								  grubu = p.grubu,
								  meslek = p.meslek,
								  sure = p.sure
							  }).Distinct().ToListAsync();

                return sd.Select(p => new TehlikeliIsler()
                {
                    TehlikeliIsler_Id = p.TehlikeliIsler_Id,
                    grubu = p.grubu,
                    meslek = p.meslek,
                    sure = p.sure
                }).ToList();
            }
		}
	}
}
