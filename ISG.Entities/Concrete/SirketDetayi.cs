using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public partial class SirketDetayi:IEntity//birebir one-to-one
    {
        public int SirketDetayi_Id { get; set; }
        public string SGKSicilNo { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Faks { get; set; }
        public string Mail { get; set; }
        public string ilMedullaKodu { get; set; }
        public string SirketYetkilisi { get; set; }
        public string SirketYetkilisiTcNo { get; set; }
        public string WebUrl { get; set; }
        public string WebUrlApi { get; set; }
        public string Nacekodu { get; set; }
        public int TehlikeGrubu { get; set; }
        public virtual Sirket Sirket { get; set; }
    }
}
