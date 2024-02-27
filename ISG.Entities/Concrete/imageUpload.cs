using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.Concrete
{
    public class imageUpload:IEntity
    {
        public long id { get; set; }
        public string Folder { get; set; }
        public string IdGuid { get; set; }
        public string FileName { get; set; }
        public string  GenericName { get; set; }
        public int FileLenght { get; set; }
        public string MimeType { get; set; }
        public int Protokol { get; set; }
        public string UserId { get; set; }
    }
}
