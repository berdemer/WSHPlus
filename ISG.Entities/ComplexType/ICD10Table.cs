using ISG.Entities.Concrete;
using System.Collections.Generic;

namespace ISG.Entities.ComplexType
{
    public class ICD10Table
    {
        public int TotalItems { get; set; }
        public int DisplayStart { get; set; }
        public int DisplayLength { get; set; }
        public IEnumerable<icd> ICDView { get; set; }

    }
}