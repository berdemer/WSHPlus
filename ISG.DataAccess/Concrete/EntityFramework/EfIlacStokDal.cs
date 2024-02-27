using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ISG.DataAccess.Concrete.EntityFramework
{
    public class EfIlacStokDal : EfEntityRepositoryBase<IlacStok, ISGContext>, IIlacStokDal
    {
        public async Task<ICollection<IlacStoku>> IlacStokHesaplari(int saglikBirimi_Id, bool status)
        {
            using (var context = new ISGContext())
            {
                return await (from i in context.IlacStoklari
                              join r in (from i in context.IlacStokGirisleri
                                         group i by new { i.StokId, i.SaglikBirimi_Id } into g
                                         select new
                                         {
                                             StokId = g.Key.StokId,
                                             Miktar = (int)g.Sum(x => x.ToplamMiktar) - (int)g.Sum(x => x.ArtanMiadTelefMiktari)
                                         }) on i.StokId equals r.StokId
                              into xc
                              from r in xc.DefaultIfEmpty()
                              join z in (from i in context.IlacSarfCikislari
                                         group i by new { i.StokId, i.SaglikBirimi_Id } into g
                                         select new
                                         {
                                             StokId = g.Key.StokId,
                                             Miktar = (int)g.Sum(x => x.SarfMiktari)
                                         }) on i.StokId equals z.StokId
                              into sr
                              from z in sr.DefaultIfEmpty()
                              where i.SaglikBirimi_Id == saglikBirimi_Id && i.Status == status
                              select new IlacStoku()
                              {
                                  StokId = i.StokId,
                                  SaglikBirimi_Id = i.SaglikBirimi_Id,
                                  IlacAdi = i.IlacAdi,
                                  StokTuru = i.StokTuru,
                                  StokMiktari = (r == null ? 0 : r.Miktar) - (z == null ? 0 : z.Miktar),
                                  KritikStokMiktari = i.KritikStokMiktari,
                                  Status = i.Status,
                                  StokMiktarBirimi = i.StokMiktarBirimi
                              }).ToListAsync();
            }
        }
    }
}
/*
  StokMiktari = (r == null ? 0 : r.Miktar) - (z == null ? 0 : z.Miktar),
     IlacStokGirisleri temsil eden r null olabilir. r.Miktar null olamaz. 
     ondan dolayý null exception hatasý alýnýr. ayrýca
     from r in xc.DefaultIfEmpty() burada empty olabileceini ifade ediyoruz.
     boþ bir ifadede r.miktar null olamaz.
     */
//       var giris =await (from i in context.IlacStokGirisleri
//             group i by new { i.StokId, i.SaglikBirimi_Id } into g
//             select new
//             {
//                 StokId = g.Key.StokId,
//                 ToplamMiktar= g.Sum(x => x.ToplamMiktar),
//                 ArtanMiadTelefMiktari= g.Sum(x => x.ArtanMiadTelefMiktari)
//             }).ToListAsync();

//        var cikis =await (from i in context.IlacSarfCikislari
//             group i by new { i.StokId, i.SaglikBirimi_Id } into g
//             select new
//             {
//                 StokId = g.Key.StokId,
//                 SarfMiktari=g.Sum(x=>x.SarfMiktari)
//             }).ToListAsync();

//var ilacstoku = await (from i in context.IlacStoklari
//                       select new IlacStoku
//                       {
//                           StokId = i.StokId,
//                           SaglikBirimi_Id = i.SaglikBirimi_Id,
//                           IlacAdi = i.IlacAdi,
//                           StokTuru = i.StokTuru,
//                           StokMiktari = i.StokMiktari,
//                           KritikStokMiktari = i.KritikStokMiktari,
//                           Status = i.Status,
//                           StokMiktarBirimi = i.StokMiktarBirimi
//                       }).ToListAsync();


//           var genel = (from i in ilacstoku.AsEnumerable()
//             join r in giris.AsEnumerable() on i.StokId equals r.StokId
//             into xc
//             from r in xc.DefaultIfEmpty()
//             join z in cikis.AsEnumerable() on i.StokId equals z.StokId
//             into sr
//             from z in sr.DefaultIfEmpty()
//             where i.SaglikBirimi_Id == saglikBirimi_Id && i.Status == status                                
//             select new 
//             {
//                 StokId = (System.Guid)(i.StokId==null?new Guid(): i.StokId),
//                 SaglikBirimi_Id = (System.Int32)(i==null?0: i.SaglikBirimi_Id),
//                 IlacAdi = (System.String)(i.IlacAdi==null?"": i.IlacAdi),
//                 StokTuru = (System.String)(i.StokTuru==null?"": i.StokTuru),
//                 StokMiktari = (System.Int32)(r== null ? 0 : r.ToplamMiktar) - ((z== null ? 0 : z.SarfMiktari) + (r == null ? 0 : r.ArtanMiadTelefMiktari)),
//                 KritikStokMiktari = (System.Int32)(i==null?0: i.KritikStokMiktari),
//                 Status = (System.Boolean)(i==null?false: i.Status),
//                 StokMiktarBirimi = (System.String)(i.StokMiktarBirimi==null?"": i.StokMiktarBirimi)
//             }).ToList();

//ICollection<IlacStoku> asd = new List<IlacStoku>();
//foreach (var item in genel)
//{
//    asd.Add(new IlacStoku() {
//        StokId= item.StokId,
//        SaglikBirimi_Id=item.SaglikBirimi_Id,
//        IlacAdi=item.IlacAdi,
//        StokTuru=item.StokTuru,
//        StokMiktari=item.StokMiktari,
//        KritikStokMiktari=item.KritikStokMiktari,
//        Status=item.Status,
//        StokMiktarBirimi=item.StokMiktarBirimi

//    });
//}
//return asd;