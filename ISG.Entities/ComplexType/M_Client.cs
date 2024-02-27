using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Entities.ComplexType
{
    public class M_Client
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string name { get; set; }
        public string Password { get; set; }
        public string mailAdress{ get; set; }
        public string displayName { get; set; }
        public string domain { get; set; }       
        public string mailSekli { get; set; }  
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials{ get; set; }
        public string apiKey { get; set; }
    }
}