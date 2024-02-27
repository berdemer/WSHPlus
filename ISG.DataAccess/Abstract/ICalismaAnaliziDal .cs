using ISG.Entities.Concrete;
using System.Threading.Tasks;

namespace ISG.DataAccess.Abstract
{
	public interface ICalismaAnaliziDal : IEntityRepository<CalismaAnalizi>
	{
		Task<object> MeslekListesi();
		Task<CalismaAnalizi> MeslekAra(string meslekKodu);

		Task<CalismaAnalizi> BolumAraAsync(int Bolum_Id, int Sirket_Id);
	}
}
