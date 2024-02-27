using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public class SirketUpload:IEntity
    {
        public long SirketUpload_Id { get; set; }
        public int Sirket_Id { get; set; }// hangi şirkete ait
        public string FileName { get; set; }//orjinal adı
        public string  GenericName { get; set; }//saklamaAdı
        public int FileLenght { get; set; }//büyüklüğü
        public string MimeType { get; set; }//uzantı tipi
        public string DosyaTipi { get; set; }//icon eğitim vs
        public string DosyaTipiID { get; set; }//icon eğitim vs tanımlanmış gerekebilir ek ıd tanımlaması
        public string Konu { get; set; }// dosyaya ilişlendirilmiş açıklalam
        public string Hazırlayan { get; set; }// hazırlayan kişi
        public string UserId { get; set; }
        public virtual Sirket Sirket { get; set; }
    }
}
