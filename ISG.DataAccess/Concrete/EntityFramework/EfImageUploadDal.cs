using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfImageUploadDal : EfEntityRepositoryBase<imageUpload, ISGContext>, IimageUploadDal
    {
    }
}
