using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.ComplexType
{
    public class BasicTreeview
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<BasicTreeview> children { get; set; }
    }
}
