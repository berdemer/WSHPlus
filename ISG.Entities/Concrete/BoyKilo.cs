using ISG.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace ISG.Entities.Concrete
{
	public partial class BoyKilo : IEntity
	{
		public int BoyKilo_Id { get; set; }
		public int Boy { get; set; }
		public int Kilo { get; set; }
		public int? Bel { get; set; }
		public int? Kalca { get; set; }
		public float? BKI { get; set; }
		public float? BKO { get; set; }
		public string Sonuc { get; set; }
		public string MuayeneTuru { get; set; }
		public int? RevirIslem_Id { get; set; }
		public int Personel_Id { get; set; }
		public int? Protokol { get; set; }
		public DateTime? Tarih { get; set; }
		public string UserId { get; set; }
		public virtual RevirIslem RevirIslem { get; set; }
		public virtual Personel Personel { get; set; }

	}
}

/*
BKÝ= (Kilo / Boy x Boy) x  10000

BKO= Bel Çevresi (cm) / Kalça Çevresi (cm)

19'dan küçük: Çok zayýf m²

20-25 arasýnda : Normal kilolu

25-30 arasýnda: Hafif þiþman

30 ve üzerinde: Þiþman (Obez)

40 ve üzerinde: Aþýrý Þiþman (Morbid obez)

Bel Kalça Oraný  Kadýnda 0.8 den küçük, erkekte 0.9 dan küçük olmalýdýr.

	 */
