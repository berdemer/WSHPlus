using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfIlacStokGirisiDal : EfEntityRepositoryBase<IlacStokGirisi, ISGContext>, IIlacStokGirisiDal
	{
		public async Task<ICollection<IlacStokGirisiView>> IlacStokGirisiView(int SB_Id,bool st)
		{
			using (var context = new ISGContext())
			{

				return await (from c in context.IlacStokGirisleri
							  join b in context.IlacStoklari on c.StokId equals b.StokId
							  where c.SaglikBirimi_Id == SB_Id && c.Status == st
							  orderby c.Tarih descending
							  select new IlacStokGirisiView()
							  {
								  Id = c.Id,
								  Tarih = c.Tarih ?? DateTime.MinValue,
								  ArtanMiadTelefMiktari = c.ArtanMiadTelefMiktari,
								  StokEkBilgisi=c.StokEkBilgisi,
								  IlacAdi = b.IlacAdi,
								  KritikMiadTarihi = c.KritikMiadTarihi ?? DateTime.MinValue,
								  KutuIcindekiMiktar = c.KutuIcindekiMiktar,
								  KutuMiktari = c.KutuMiktari,
								  MiadTarihi = c.MiadTarihi ?? DateTime.MinValue,
								  StokId = c.StokId,
								  ToplamMiktar = c.ToplamMiktar,
								  Maliyet = c.Maliyet,
								  ArtanTelefNedeni=c.ArtanTelefNedeni                                
							  }
					   ).ToListAsync();

			}
		}
	}
}
