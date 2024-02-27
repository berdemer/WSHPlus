using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework.Context;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NinjaNye.SearchExtensions;
using System.Text.RegularExpressions;


namespace ISG.DataAccess.Concrete.EntityFramework
{
	public class EfPersonelDal : EfEntityRepositoryBase<Personel, ISGContext>, IPersonelDal
	{
		public async Task<Personel> TcNoKontrol(string tcNo)
		{
			using (var context = new ISGContext())
			{
				return await context.Set<Personel>().Where(p => p.TcNo == tcNo).FirstOrDefaultAsync();
			}
		}

		public async Task<Personel> SicilNoKontrol(string sicil)
		{
			using (var context = new ISGContext())
			{
				return await context.Set<Personel>().Where(p => p.SicilNo == sicil).FirstOrDefaultAsync();
			}
		}

		public async Task<Personel> GuidKontrol(Guid GuidId)
		{
			using (var context = new ISGContext())
			{
				return await context.Set<Personel>().Where(p => p.PerGuid == GuidId).FirstOrDefaultAsync();
			}
		}

		public async Task<ICollection<PersonelView>> PersonelDeparmentView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durumu)
		{
			using (var context = new ISGContext())
			{
				ICollection<PersonelView> personels= await (from p in context.Personeller
							  join c in context.PersonelDetaylari on p.Personel_Id equals c.PersonelDetay_Id
							  join S in context.Sirketler on p.Sirket_Id equals S.id
							  join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
							  where (Sirketlerimiz.Contains(S.id) || Bolumlerimiz.Contains(SB.id)) && p.Status == Durumu
							  select new PersonelView()
							  {
								  Personel_Id = p.Personel_Id,
								  Adi = p.Adi.Trim(),
								  Soyadi = p.Soyadi.Trim(),
								  TcNo = p.TcNo.Trim(),
								  KanGrubu = p.KanGrubu.Trim(),
								  SicilNo = p.SicilNo.Trim(),
								  SgkNo = p.SgkNo,
								  Photo = p.Photo,
								  Mail = p.Mail.Trim(),
								  Telefon = p.Telefon.Trim(),
								  sirketAdi = S.sirketAdi.Trim(),
								  bolumAdi = SB.bolumAdi.Trim(),
								  KadroDurumu = p.KadroDurumu.Trim(),
								  Gorevi = p.Gorevi.Trim(),
								  PerGuid = p.PerGuid,
								  DogumTarihi = c.DogumTarihi,
								  MedeniHali = c.MedeniHali.Trim(),
								  EgitimSeviyesi = c.EgitimSeviyesi.Trim(),
								  IlkIseBaslamaTarihi = c.IlkIseBaslamaTarihi
							  }).ToListAsync();
				foreach (PersonelView p in personels)
				{
					p.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == p.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
				}

				return personels;
			}
		}

		public async Task<ICollection<PersonelView>> PersonelAllDeparmentView(bool Durumu)
		{
			using (var context = new ISGContext())
			{
				ICollection<PersonelView> personels = await (from p in context.Personeller
							  join S in context.Sirketler on p.Sirket_Id equals S.id
							  join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
							  where p.Status == Durumu
							  select new PersonelView()
							  {
								  Personel_Id = p.Personel_Id,
								  Adi = p.Adi.Trim(),
								  Soyadi = p.Soyadi.Trim(),
								  TcNo = p.TcNo.Trim(),
								  KanGrubu = p.KanGrubu.Trim(),
								  SicilNo = p.SicilNo.Trim(),
								  SgkNo = p.SgkNo.Trim(),
								  Photo = p.Photo,
								  Mail = p.Mail.Trim(),
								  Telefon = p.Telefon.Trim(),
								  sirketAdi = S.sirketAdi.Trim(),
								  bolumAdi = SB.bolumAdi.Trim(),
								  KadroDurumu = p.KadroDurumu.Trim(),
								  Gorevi = p.Gorevi.Trim(),
								  PerGuid = p.PerGuid
							  }).ToListAsync();
                foreach (PersonelView p in personels)
                {
					p.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == p.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
				}

				return personels;
			}

		}

		public async Task<personelCard> PersonelCardView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength)
		{
			try
			{
				using (var context = new ISGContext())
				{
					IList<personelCardView> sd = await (from p in context.Personeller
														join S in context.Sirketler on p.Sirket_Id equals S.id
														join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
														where Sirketlerimiz.Contains(S.id) && p.Status == Durumu
														select new personelCardView()
														{
															Personel_Id = p.Personel_Id,
															AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
															TcNo = p.TcNo,
															KanGrubu = p.KanGrubu.Trim(),
															SicilNo = p.SicilNo.Trim(),
															SgkNo = p.SgkNo.Trim(),
															Photo = p.Photo,
															Mail = p.Mail.Trim(),
															Telefon = p.Telefon.Trim(),
															sirketAdi = S.sirketAdi.Trim(),
															bolumAdi = SB.bolumAdi.Trim(),
															KadroDurumu = p.KadroDurumu.Trim(),
															Gorevi = p.Gorevi.Trim(),
															PerGuid = p.PerGuid
														}).ToListAsync();
					/*// @bþlangýçlar arada boþluk olmadan arama yapmasý için
					List<string> ouyx = new List<string>();
					if (Regex.IsMatch(Search, @"^@"))
					{
					   ouyx.Add(Search.Substring(1, Search.Length-1));
					}
					else
					{
						foreach (var item in Search.ToLower().Split(' '))
						{
							ouyx.Add(item);
						}
					}
					string[] ouy = ouyx.ToArray();
					 */

					string[] ouy =Search.ToLower().Split(' ');
					var sdx = sd.AsQueryable().Search(
						x => x.PerGuid.ToString(),
						x => !string.IsNullOrEmpty(x.AdiSoyadi) ? x.AdiSoyadi.ToLower() : "",
						x => !string.IsNullOrEmpty(x.TcNo) ? x.TcNo.ToLower() : "",
						x => !string.IsNullOrEmpty(x.KanGrubu) ? x.KanGrubu.ToLower() : "",
						x => !string.IsNullOrEmpty(x.SicilNo) ? x.SicilNo.ToLower() : "",
						x => !string.IsNullOrEmpty(x.SgkNo) ? x.SgkNo.ToLower() : "",
						//x => !string.IsNullOrEmpty(x.Mail) ? x.Mail.ToLower() : "",
						x => !string.IsNullOrEmpty(x.Telefon) ? x.Telefon.ToLower() : "",
						x => !string.IsNullOrEmpty(x.sirketAdi) ? x.sirketAdi.ToLower() : "",
						x => !string.IsNullOrEmpty(x.bolumAdi) ? x.bolumAdi.ToLower() : "",
						x => !string.IsNullOrEmpty(x.KadroDurumu) ? x.KadroDurumu.ToLower() : "",
						x => !string.IsNullOrEmpty(x.Gorevi) ? x.Gorevi.ToLower() : ""
					).ContainingAll(ouy);

					//IList<personelCardView> sdx = sd.Where(c =>
					//                    !string.IsNullOrEmpty(c.AdiSoyadi) && c.AdiSoyadi.ToLower().Contains(Search.ToLower()) || SqlFunctions.SoundCode(c.AdiSoyadi) == SqlFunctions.SoundCode(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.TcNo) && c.TcNo.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.KanGrubu) && c.KanGrubu.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.SicilNo) && c.SicilNo.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.SgkNo) && c.SgkNo.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.Mail) && c.Mail.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.Telefon) && c.Telefon.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.sirketAdi) && c.sirketAdi.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.bolumAdi) && c.bolumAdi.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.KadroDurumu) && c.KadroDurumu.ToLower().Contains(Search.ToLower())
					//                    ||
					//                    !string.IsNullOrEmpty(c.Gorevi) && c.Gorevi.ToLower().Contains(Search.ToLower())
					//                   ).ToList();
					int count = sdx.Count();
					IList<personelCardView> xcv = sdx.Skip(DisplayStart).Take(DisplayLength).ToList();

					foreach (var per in xcv)
					{
						var Photo = await (context.imageUploads.Where(p => p.IdGuid == per.PerGuid.ToString() && p.Folder == "Personel").Select(p => p.GenericName).FirstOrDefaultAsync());
						per.Photo = Photo != null ? Photo.ToString() : "";
                        bool Ikaz = await (context.Ikazlar.Where(s => s.Personel_Id == per.Personel_Id && s.Status == true && s.SonTarih < DateTime.Now).Select(d => d.Status).FirstOrDefaultAsync());
                        per.Ikaz = Ikaz == true;
						per.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == per.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
					}
					return new personelCard() { DisplayLength = DisplayLength, DisplayStart = DisplayStart, TotalItems = count, PersonelCards = xcv };
				}
			}
			catch (Exception)
			{

				throw;
			}

		}


		public async Task<Object> EgitimPersonelleriListesi(int Sirket_Id, bool Durumu, string Search)
		{
			try
			{
				using (var context = new ISGContext())
				{
					IList<personelCardView> sd = await (from p in context.Personeller
														join S in context.Sirketler on p.Sirket_Id equals S.id
														join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
														where p.Sirket_Id==Sirket_Id && p.Status == Durumu
														select new personelCardView()
														{
															Personel_Id = p.Personel_Id,
															AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
															TcNo = p.TcNo,
															KanGrubu = p.KanGrubu.Trim(),
															SicilNo = p.SicilNo.Trim(),
															SgkNo = p.SgkNo.Trim(),
															Photo = p.Photo,
															Mail = p.Mail.Trim(),
															Telefon = p.Telefon.Trim(),
															sirketAdi = S.sirketAdi.Trim(),
															KadroDurumu = p.KadroDurumu.Trim(),
															Gorevi = p.Gorevi.Trim(),
															PerGuid = p.PerGuid,
															bolumAdi=SB.bolumAdi.Trim()
														}).OrderBy(p => p.AdiSoyadi).ToListAsync();
					string[] ouy =Search.ToLower().Split(' ');
					var sdx = sd.AsQueryable().Search(
						x => x.PerGuid.ToString(),
						x => !string.IsNullOrEmpty(x.AdiSoyadi) ? x.AdiSoyadi.ToLower() : "",
						x => !string.IsNullOrEmpty(x.TcNo) ? x.TcNo.ToLower() : "",
						x => !string.IsNullOrEmpty(x.SicilNo) ? x.SicilNo.ToLower() : ""
					).ContainingAll(ouy);
					foreach (var per in sdx)
					{
						var Photo = await (context.imageUploads.Where(p => p.IdGuid == per.PerGuid.ToString() && p.Folder == "Personel").Select(p => p.GenericName).FirstOrDefaultAsync());
						per.Photo = Photo != null ? Photo.ToString() : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
						per.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == per.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
					}
					return new personelCard() { TotalItems = 0, DisplayLength = 0, DisplayStart = 0, PersonelCards = sdx };
				}
			}
			catch (Exception)
			{ 
				throw;
			}
		}

		public async Task<Object> EgitimPersonelleriTumListesi(int Sirket_Id, bool Durumu)
		{
			try
			{
				using (var context = new ISGContext())
				{
					IList<personelCardView> sd = await (from p in context.Personeller
														join S in context.Sirketler on p.Sirket_Id equals S.id
														join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
														where p.Sirket_Id == Sirket_Id && p.Status == Durumu
														select new personelCardView()
														{
															Personel_Id = p.Personel_Id,
															AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
															TcNo = p.TcNo,
															KanGrubu = p.KanGrubu.Trim(),
															SicilNo = p.SicilNo.Trim(),
															SgkNo = p.SgkNo.Trim(),
															Photo = p.Photo,
															Mail = p.Mail.Trim(),
															Telefon = p.Telefon.Trim(),
															sirketAdi = S.sirketAdi.Trim(),
															KadroDurumu = p.KadroDurumu.Trim(),
															Gorevi = p.Gorevi.Trim(),
															PerGuid = p.PerGuid,
															bolumAdi = SB.bolumAdi.Trim()
														}).OrderBy(p => p.AdiSoyadi).ToListAsync(); 
					foreach (var per in sd)
					{
						var Photo = await (context.imageUploads.Where(p => p.IdGuid == per.PerGuid.ToString() && p.Folder == "Personel").Select(p => p.GenericName).FirstOrDefaultAsync());
						per.Photo = Photo != null ? Photo.ToString() : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
						per.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == per.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
					}
					return new personelCard() { TotalItems = 0, DisplayLength = 0, DisplayStart = 0, PersonelCards = sd };
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public int yas(DateTime dgunu)
        {
			DateTime simdi = DateTime.Today;
			int yas = simdi.Year - dgunu.Year;
			if (dgunu > simdi.AddYears(-yas))
				yas--;
			return yas;
		}

		public async Task<personelCard> PersonelDeparmentCardView(IEnumerable<int> Sirketlerimiz, IEnumerable<int> Bolumlerimiz, bool Durumu, int DisplayStart, int DisplayLength)
		{
			try
			{
				using (var context = new ISGContext())
				{
					ICollection<personelCardView> sd = await (from p in context.Personeller
															  join S in context.Sirketler on p.Sirket_Id equals S.id
															  join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
															  where (Sirketlerimiz.Contains(S.id) || Bolumlerimiz.Contains(SB.id)) && p.Status == Durumu
															  select new personelCardView()
															  {
																  Personel_Id = p.Personel_Id,
																  AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
																  TcNo = p.TcNo,
																  KanGrubu = p.KanGrubu.Trim(),
																  SicilNo = p.SicilNo.Trim(),
																  SgkNo = p.SgkNo.Trim(),
																  Photo = p.Photo,
																  Mail = p.Mail.Trim(),
																  Telefon = p.Telefon.Trim(),
																  sirketAdi = S.sirketAdi.Trim(),
																  bolumAdi = SB.bolumAdi.Trim(),
																  KadroDurumu = p.KadroDurumu.Trim(),
																  Gorevi = p.Gorevi.Trim(),
																  PerGuid = p.PerGuid
															  }).OrderBy(p => p.AdiSoyadi).ToListAsync();
					int count = sd.Count();
					ICollection<personelCardView> sdx = sd.Skip(DisplayStart).Take(DisplayLength).ToList();
					foreach (var per in sdx) 

					{
						var Photo = await (context.imageUploads.Where(p => p.IdGuid == per.PerGuid.ToString() && p.Folder == "Personel").Select(p => p.GenericName).FirstOrDefaultAsync());
						per.Photo = Photo != null ? Photo.ToString() : "";
                        bool Ikaz = await (context.Ikazlar.Where(s => s.Personel_Id == per.Personel_Id && s.Status == true&& s.SonTarih<DateTime.Now).Select(d => d.Status).FirstOrDefaultAsync());
                        per.Ikaz = Ikaz==true;
						per.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == per.Personel_Id).Select(x=>x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
					}
					return new personelCard() { TotalItems = count, DisplayLength = DisplayLength, DisplayStart = DisplayStart, PersonelCards = sdx };
				}
			}
			catch (Exception)
			{

				throw;
			}

		}

		public async Task<personelCard> PersonelListOrderSearchView(IEnumerable<int> Sirketlerimiz, bool Durumu, string Search, int DisplayStart, int DisplayLength, int sortColumnIndex, string SortDir)
		{
			try
			{
				using (var context = new ISGContext())
				{
					IList<personelCardView> sd = null;
					if (!string.IsNullOrEmpty(Search))
					{
						sd = await (from p in context.Personeller
									join S in context.Sirketler on p.Sirket_Id equals S.id
									join SB in context.SirketBolumleri on p.Bolum_Id equals SB.id
									where Sirketlerimiz.Contains(S.id) || p.Status == Durumu
									select new personelCardView()
									{
										Personel_Id = p.Personel_Id,
										AdiSoyadi = p.Adi.Trim() + " " + p.Soyadi.Trim(),
										TcNo = p.TcNo,
										KanGrubu = p.KanGrubu.Trim(),
										SicilNo = p.SicilNo.Trim(),
										SgkNo = p.SgkNo.Trim(),
										Photo = p.Photo,
										Mail = p.Mail.Trim(),
										Telefon = p.Telefon,
										sirketAdi = S.sirketAdi.Trim(),
										bolumAdi = SB.bolumAdi.Trim(),
										KadroDurumu = p.KadroDurumu.Trim(),
										Gorevi = p.Gorevi.Trim(),
										PerGuid = p.PerGuid
									}).ToListAsync();
						//aslnda tek sorgu gibi gönderelebilir.
						IEnumerable<personelCardView> sdx = sd.Where(c =>
											!string.IsNullOrEmpty(c.AdiSoyadi) && c.AdiSoyadi.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.TcNo) && c.TcNo.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.KanGrubu) && c.KanGrubu.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.SicilNo) && c.SicilNo.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.SgkNo) && c.SgkNo.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.Mail) && c.Mail.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.Telefon) && c.Telefon.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.sirketAdi) && c.sirketAdi.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.bolumAdi) && c.bolumAdi.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.KadroDurumu) && c.KadroDurumu.ToLower().Contains(Search.ToLower())
											||
											!string.IsNullOrEmpty(c.Gorevi) && c.Gorevi.ToLower().Contains(Search.ToLower())
										   ).ToList();
						int totalItems = sdx.Count();
						//delege oluþturuldu index kolununa göre index alnacak alan seçildi.
						Func<personelCardView, string> orderingFunction = (c => sortColumnIndex == 1 ? c.AdiSoyadi :
																	sortColumnIndex == 2 ? c.SicilNo :
																	sortColumnIndex == 3 ? c.sirketAdi :
																	sortColumnIndex == 4 ? c.bolumAdi :
																	sortColumnIndex == 5 ? c.Gorevi :
																	sortColumnIndex == 6 ? c.TcNo :
																	sortColumnIndex == 7 ? c.KadroDurumu :
																	sortColumnIndex == 8 ? c.Telefon :
																	sortColumnIndex == 9 ? c.KanGrubu : "");

						if (SortDir == "asc")//asc yönlendirmesi varsa
							sdx = sdx.OrderBy(orderingFunction);
						else
							sdx = sdx.OrderByDescending(orderingFunction);

						var sce = sdx.Skip(DisplayStart).Take(DisplayLength);
						// baþlama noktasý ve uzunluk 2 sayfa 10 satýr gibi
						foreach (var per in sce)
						{
							var Photo = await (context.imageUploads.Where(p => p.IdGuid == per.PerGuid.ToString() && p.Folder == "Personel").
								Select(p => p.GenericName).SingleOrDefaultAsync());
							per.Photo = Photo != null ? Photo.ToString() : "";
                            bool Ikaz = await (context.Ikazlar.Where(s => s.Personel_Id == per.Personel_Id && s.Status == true && s.SonTarih < DateTime.Now).Select(d => d.Status).FirstOrDefaultAsync());
                            per.Ikaz = Ikaz == true;
							per.CalismaSekli = (await (context.Calisma_Durumu.Where(x => x.Personel_Id == per.Personel_Id).Select(x => x.Calisma_Duzeni).ToListAsync())).ToList().LastOrDefault();
						}
						return new personelCard() { DisplayLength = DisplayLength, DisplayStart = DisplayStart, PersonelCards = sce, TotalItems = totalItems };
					}
					else return new personelCard() { };
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}



//  public async Task<List<string>> GetPersonelDetayiAsync()
//  {
//      List<string> result = new List<string>();

//      using (var context = new ISGContext())
//      {
//          result = await (from vb in context.Personeller where vb.Meslek_id == 1 select vb.AdiSoyadi).ToListAsync();
//          return result;
//      }
//  }

/*
 var blogs = await (from b in db.Blogs 
							orderby b.Name 
							select b).ToListAsync(); 
 
 * 
 http://weblogs.asp.net/scottgu/linq-to-sql-part-9-using-a-custom-linq-expression-with-the-lt-asp-linqdatasource-gt-control
 * https://codefirstfunctions.codeplex.com/
 * http://www.c-sharpcorner.com/UploadFile/ff2f08/code-first-stored-procedure-entity-framework-6-0/
 * 
 */


/*
 public IEnumerable<Store> ListStores(Expression<Func<Store, string>> sort, bool desc, int page, int pageSize, out int totalRecords)
{
	List<Store> stores = new List<Store>();
	using (var context = new TectonicEntities())
	{
		totalRecords = context.Stores.Count();
		int skipRows = (page - 1) * pageSize;
		if (desc)
			stores = context.Stores.OrderByDescending(sort).Skip(skipRows).Take(pageSize).ToList();
		else
			stores = context.Stores.OrderBy(sort).Skip(skipRows).Take(pageSize).ToList();
	}
	return stores;
}
Expression<Func<Store, string>> sort-----------
 */
