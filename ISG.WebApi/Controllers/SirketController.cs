using ISG.Business.Abstract;
using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ISG.WebApi.Controllers
{
	[Authorize(Roles = "ISG_Admin , Admin")]
	[RoutePrefix("api/sirket")]
	public class SirketController : ApiController
	{
		private ISirketDetayiService _sirketDetayiService { get; set; }
		private ISirketBolumuService _sirketBolumuService { get; set; }
		private ISirketService _sirketService { get; set; }
		private ISirketAtamaService _sirketAtamaService { get; set; }

		public SirketController(ISirketService sirketService,
			ISirketBolumuService sirketBolumuService,
			ISirketAtamaService sirketAtamaService,
			ISirketDetayiService sirketDetayiService)
		{
			_sirketService = sirketService;
			_sirketBolumuService = sirketBolumuService;
			_sirketAtamaService = sirketAtamaService;
			_sirketDetayiService = sirketDetayiService;
		}

        [Route("OrgAtama")]
        [HttpGet]
        public async Task<IEnumerable<Treeview>> OrganizasyonelAtama()
        {
            string UserId = User.Identity.GetUserId();
            ICollection<SirketAtama> sirAtama = await _sirketAtamaService.FindAllOrgAsync(UserId);
            IEnumerable<int> Sirketlerimiz = sirAtama.OrderBy(x => x.Sirket_id).Select(x => x.Sirket_id);
            ICollection<Sirket> tablo = await _sirketService.FindAllAsync(true);
            //Asıl tablo çekildi
            IList<sirketX> tablox = new List<sirketX>();//sanal tablo oluşturuldu.
            foreach (var sirket in tablo)
            {
                if (Sirketlerimiz.Where(c => c == sirket.id).Any()) //yetkisi olan siketlerler tablosuna bulguları yerleştirildi.
                {
                    tablox.Add(new sirketX() { id = sirket.id, idRef = sirket.idRef, sirketAdi = sirket.sirketAdi, status = sirket.status, bulgu = true });
                }
                else
                {
                    tablox.Add(new sirketX() { id = sirket.id, idRef = sirket.idRef, sirketAdi = sirket.sirketAdi, status = sirket.status, bulgu = false });
                }
            };

            foreach (var sirket in tablox)/*tablo döngüye alınıp yeniden refid yapılanması sağlandı.*/
            {
                if (sirket.bulgu == true && sirket.idRef > 0)
                {
                    int v= RecursiveID(sirket.idRef, tablox);
                    sirket.idRef = v;
                }
            };
            //ICollection<sirketW> tablo = await _sirketService.OrganizationUserList(Sirketlerimiz,true);
            //recursive metod uygulama yeri
            IEnumerable <Treeview> nodes = tablox.Where(z => z.bulgu == true).ToList().RecursiveJoin(element => element.id,
               element => element.idRef,
               (sirketX element, int index, int depth, IEnumerable<Treeview> children) =>
               {
                   return new Treeview()
                   {
                       Text = element.sirketAdi,
                       tabloId = element.id,
                       Children = children,
                       Depth = element.idRef,
                       status = element.status
                   };
               });
            return nodes;
        }

        private int RecursiveID(int idRef, IList<sirketX> tablox)
        {
            sirketX sd = tablox.Where(p => p.id == idRef).SingleOrDefault();
            if (sd.bulgu == false)
            {
                return RecursiveID(sd.idRef, tablox);
            }
            return sd.id;
        }

        [HttpGet]
		public async Task<IEnumerable<Treeview>> GetAll(bool status)
		{
			ICollection<Sirket> tablo = await _sirketService.FindAllAsync(status);
			//recursive metod uygulama yeri
			IEnumerable<Treeview> nodes = tablo.RecursiveJoin(element => element.id,
			   element => element.idRef,
			   (Sirket element, int index, int depth, IEnumerable<Treeview> children) =>
			   {
				   return new Treeview()
				   {
					   Text = element.sirketAdi,
					   tabloId=element.id,
					   Children = children,
					   Depth = element.idRef,
					   status=element.status
				   };
			   });
			return nodes;
		}

		//http://localhost:1943/api/Sirket?key=3
		[HttpPut]
		public async Task<Sirket> Put(int key, [FromBody]Sirket sirket)
		{
			return await _sirketService.UpdateAsync(sirket,key);
		}
		//http://localhost:1943/api/Sirket
		[HttpPost]
		public async Task<Sirket> Post(Sirket sirket)
		{
			return await _sirketService.AddAsync(sirket);
		}

		//http://localhost:1943/api/Sirket/bolumu?status=true
		[Route("bolumu/{sirketId}/{status}")]
		[HttpGet]
		public async Task<IEnumerable<Treeview>> GetAll(bool status,int sirketId)
		{
			ICollection<SirketBolumu> tablo = await _sirketBolumuService.FindAllAsync(status,sirketId);
			//recursive metod uygulama yeri
			IEnumerable<Treeview> nodes = tablo.RecursiveJoin(element => element.id,
			   element => element.idRef,
			   (SirketBolumu element, int index, int depth, IEnumerable<Treeview> children) =>
			   {
				   return new Treeview()
				   {
					   Text = element.bolumAdi,
					   tabloId = element.id,
					   Children = children,
					   Depth = element.idRef,
					   status = element.status
				   };
			   });
			return nodes;
		}

		//http://localhost:1943/api/Sirket/bolumu?key=3
		[Route("bolumu")]
		[HttpPut]
		public async Task<SirketBolumu> Put(int key, [FromBody]SirketBolumu sirketBolumu)
		{
			return await _sirketBolumuService.UpdateAsync(sirketBolumu, key);
		}
		//http://localhost:1943/api/Sirket/bolumu
		[Route("bolumu")]
		[HttpPost]
		public async Task<SirketBolumu> Post(SirketBolumu sirketBolumu)
		{
			return await _sirketBolumuService.AddAsync(sirketBolumu);
		}

		//http://localhost:1943/api/Sirket/atama/2
		[Route("atama/{sirketId}")]
		[HttpGet]
		public async Task<IEnumerable<SirketAtama>> GetAll(int sirketId)
		{
			return await _sirketAtamaService.FindAllAsync(sirketId);
		}

		//http://localhost:1943/api/Sirket/atama
		[Route("atama")]
		[HttpPost]
		public async Task<SirketAtama> Post(SirketAtama sirketAtama)
		{
			return await _sirketAtamaService.AddAsync(sirketAtama);
		}

		//http://localhost:1943/api/Sirket/atama/id
		[Route("atama/{id}")]
		[HttpDelete]
		public async Task<int> Delete(int id)
		{
			return await _sirketAtamaService.DeleteAsync(id);
		}

		//http://localhost:1943/api/Sirket/detayi/id
		[Route("detayi/{id}")]
		[HttpGet]
		public async Task<SirketDetayi> GetById(int id)
		{
			return await _sirketDetayiService.GetAsync(id);
		}


		//http://localhost:1943/api/Sirket/detayi
		[Route("detayi")]
		[HttpPost]
		public async Task<SirketDetayi> Post(SirketDetayi sirketDetayi)
		{
			return await _sirketDetayiService.AddAsync(sirketDetayi);
		}

		//http://localhost:1943/api/Sirket/detayi/3
		[Route("detayi/{idx}")]
		[HttpDelete]
		public async Task<int> Remove(int idx)
		{
			return await _sirketDetayiService.DeleteAsync(idx);
		}


		//http://localhost:1943/api/Sirket/detayi/3
		[Route("detayi/{key}")]
		[HttpPut]
		public async Task<SirketDetayi> Put(int key, [FromBody]SirketDetayi sirketDetayi)
		{
			return await _sirketDetayiService.UpdateAsync(sirketDetayi, key);
		}


	}
}