using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfSirketBolumuDal : EfEntityRepositoryBase<SirketBolumu, ISGContext>, ISirketBolumuDal
    {
    }
}
