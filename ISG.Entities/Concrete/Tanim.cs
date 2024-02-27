using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public class Tanim : IEntity
    {
        public int tanim_Id { get; set; }
        public string  tanimAdi { get; set; }
        public string  tanimKisaltmasi { get; set; }
        public string ifade { get; set; }
        public string ifadeBagimliligi { get; set; }
        public string  aciklama { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
