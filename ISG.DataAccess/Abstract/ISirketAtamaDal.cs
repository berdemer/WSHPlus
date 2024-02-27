using ISG.Entities.Concrete;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
    public interface ISirketAtamaDal : IEntityRepository<SirketAtama>
    {
        Task<bool> AtamaKontrol(string user_Id,int Sirket_Id);
    }
}
