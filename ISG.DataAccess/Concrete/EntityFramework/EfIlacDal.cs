using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ISG.DataAccess.Concrete.EntityFramework.ComplexType;
using NinjaNye.SearchExtensions;
using System;
using NinjaNye.SearchExtensions.Soundex;

namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfIlacDal : EfEntityRepositoryBase<Ilac, ISGContext>, IIlacDal
	{
		public async Task<ICollection<SBView>> SaglikBirimiUserId(string user_Id)
		{
			using (var context = new ISGContext())
			{
				ICollection<SBView> vb = await (from p in context.SirketAtamalari
												join s in context.Sirketler on p.Sirket_id equals s.id
												join b in context.SaglikBirimleri on p.Sirket_id equals b.StiId
												where p.uzmanPersonelId == user_Id 
												select new SBView()
												{
													SaglikBirimi_Id = b.SaglikBirimi_Id,
													SirketAdi = s.sirketAdi,
													SirketId=s.id,
													SaglikBirimiAdi = b.Adi
												}
					   ).ToListAsync();
				return vb;
			}
		 }

		public async Task<ICollection<Ilac>> IlacAdiAra(string value)
		{
			using (var context = new ISGContext())
			{
				if (value.Length > 1 && value.Length <4) {
					return await context.Ilaclar.Where(x => x.IlacAdi.StartsWith(value)||x.AtcAdi.StartsWith(value)).ToListAsync();
				}//direkt sqlden sonuç alýr çok hýzlýdýr.
				if (value.Length > 3)
				{
                    ICollection<Ilac> mn =  await context.Ilaclar.Where(x => x.IlacAdi.Contains(value)||x.AtcAdi.Contains(value)).ToListAsync();
                    if (mn.Count > 0)
                    {
                        return mn;
                    }else
                    {
                        IList<Ilac> sd = await context.Set<Ilac>().ToListAsync();
                        var sdx = sd.SoundexOf(x => !string.IsNullOrEmpty(x.IlacAdi) ? x.IlacAdi.ToLower() : "", x => !string.IsNullOrEmpty(x.AtcAdi) ? x.AtcAdi.ToLower() : "").Matching(value);
                        return sdx.ToList();
                    }


                }//direkt sqlden sonuç alýr çok hýzlýdýr.
				//if (value.Length > 5)
				//{
				//	IList<Ilac> sd = await context.Set<Ilac>().ToListAsync();
    //                var sdx = sd.SoundexOf(x => !string.IsNullOrEmpty(x.IlacAdi) ? x.IlacAdi.ToLower() : "", x => !string.IsNullOrEmpty(x.AtcAdi) ? x.AtcAdi.ToLower() : "").Matching(value);
				//	return sdx.ToList();
				//}//Soundex algoritmasý uygulanýr.Hata paylarýný düzeltir.
				return null;
			}
		}

        public async Task<KtubKt> KtubKtAra(string Search)
        {
            string[] kelimeDizisi = Search.ToUpper().Split(' ');
            using (var context = new ISGContext())
            {
                int s = kelimeDizisi.Length;
                for (int i = 0; i < kelimeDizisi.Length; i++)
                {
                    var search = kelimeDizisi.Take(s);//kelime katarýný almak için
                    string vs = string.Join(" ", search);
                    List<KtubKt> cv = await context.KtubKtListesi.Where(x => x.Name.Contains(vs)).ToListAsync();
                    if (cv.Count > 0)
                    {
                          return cv.FirstOrDefault();
                    }
                    else
                    {
                        s = s - 1;
                    }
                }               
            }
            return null;
        }

    }
}
//	return  await context.Ilaclar.Where(x=>x.IlacAdi.StartsWith(value)).ToListAsync();

////var sdx = sd.Search(x => !string.IsNullOrEmpty(x.IlacAdi) ? x.IlacAdi.ToLower() : "").Soundex(value);

////var sdx = sd.Search(x => !string.IsNullOrEmpty(x.IlacAdi) ? x.IlacAdi.ToLower() : "").Containing(value);
////return sdx.ToList();