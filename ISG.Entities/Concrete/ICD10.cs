using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public class ICD10 : IEntity
    {
        public int ICD10_Id { get; set; }

        public int IdRef { get; set; }

        public string ICD10Code { get; set; }

        public string ICDTanimi { get; set; }

        public bool AramaOnceligi { get; set; }
    }
}
