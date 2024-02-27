using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
    public partial class Tetkik : IEntity
    {
        public int Tetkik_Id { get; set; }
        public string MuayeneTuru { get; set; }
        public string TetkikTanimi { get; set; }
        public string TetkikSonucu { get; set; }
        public string HekimAdi { get; set; }
        public string Sikayeti { get; set; }
        public int RevirIslem_Id { get; set; }
        public virtual RevirIslem RevirIslem { get; set; }
   
    }
}
