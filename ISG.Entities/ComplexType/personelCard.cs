using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.ComplexType
{
    public class personelCard
    {
        public int TotalItems { get; set; }
        public int DisplayStart { get; set; }
        public int DisplayLength { get; set; }
        public IEnumerable<personelCardView> PersonelCards{get;set;}

    }
}
