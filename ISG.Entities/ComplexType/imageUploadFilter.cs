using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISG.Entities.ComplexType
{
    public class imageUploadFilter
    {
        public string Folder { get; set; }
        public string IdGuid { get; set; }
        public int Protokol { get; set; }
        public string  GenericName { get; set; }
        public string DosyaTipi { get; set; }//icon eğitim vs
        public string DosyaTipiID { get; set; }//icon eğitim vs tanımlanmış gerekebilir ek ıd tanımlaması
        public string Konu { get; set; }// dosyaya ilişlendirilmiş açıklalam
        public string Hazırlayan { get; set; }// hazırlayan kişi
        public int Sirket_Id { get; set; }
    }
}
