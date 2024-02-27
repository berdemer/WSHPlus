using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public partial class SirketAtama : IEntity
    {
        public int SirketAtama_id { get; set; }

        public string  uzmanPersonelId { get; set; }

        public int Sirket_id { get; set; }

        public virtual Sirket Sirket { get; set; }
    }
}
