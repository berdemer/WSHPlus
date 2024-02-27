using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using NinjaNye.SearchExtensions;
using System.Globalization;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfTanimDal : EfEntityRepositoryBase<Tanim, ISGContext>, ITanimDal
    {
        public async Task<ICollection<Tanim>> SikayetAra(string value)
        {
            using (var context = new ISGContext())
            {
                if (value.Length > 1)
                {
                    return await context.Tanimlari.Where(x => x.tanimAdi == "Şikayet Tablosu" && x.ifade.Contains(value)).OrderBy(x => x.ifade).ToListAsync();
                };
                return null;
            }
        }

        public async Task<ICollection<Tanim>> BulguAra(string value)
        {
            using (var context = new ISGContext())
            {
                if (value.Length > 1)
                {
                    return await context.Tanimlari.Where(x => x.tanimAdi == "Fiziksel Bulgu Tablosu" && x.ifade.Contains(value)).OrderBy(x => x.ifade).ToListAsync();
                };
                return null;
            }
        }
    }
}
