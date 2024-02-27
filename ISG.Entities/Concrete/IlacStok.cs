using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class IlacStok: IEntity
    {
        public IlacStok(){
            IlacStokGirisleri = new List<IlacStokGirisi>();
            IlacSarfCikislari = new List<IlacSarfCikisi>();
        }

        public virtual ICollection<IlacStokGirisi> IlacStokGirisleri { get; set; }
        public virtual ICollection<IlacSarfCikisi> IlacSarfCikislari { get; set; }

        public Guid StokId { get; set; }
        public int SaglikBirimi_Id { get; set; }
        public string  IlacAdi { get; set; }
        public string StokTuru { get; set; }//Demirbaş,Sarf,ilaç,Diğer
        public int StokMiktari { get; set; }
        public string StokMiktarBirimi { get; set; }//kg,adet,gr,litre,cm,metre,
        public  int KritikStokMiktari { get; set;}
        public bool Status { get; set; }
    }
}
