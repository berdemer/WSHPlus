using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISG.Entities.ComplexType
{
    public class Treeview
    {
        public int Depth { get; set; }
        public int tabloId { get; set; }
        public string Text { get; set; }

        public bool status { get; set; }
        public IEnumerable<Treeview> Children { get; set; }

    }
}
