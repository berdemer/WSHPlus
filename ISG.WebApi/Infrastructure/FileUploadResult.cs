using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISG.WebApi.Infrastructure
{
    public class FileUploadResult
    {
        public string LocalFilePath { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string  GenericName { get; set; }
        public string  yukleme { get; set; }
    }
}