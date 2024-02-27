using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public class ICDSablonu : IEntity
    {
        public int ICDSablonu_Id { get; set; }

        public string ICDkod { get; set; }
        public string ICDSablonuJson { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public DateTime? Tarih { get; set; }
    }
}
