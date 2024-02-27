using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISG.Entities.Concrete
{
    public class Meslek : IEntity
    {

        public int Meslek_id { get; set; }
        public string MeslekAdi { get; set; }
    }
}
