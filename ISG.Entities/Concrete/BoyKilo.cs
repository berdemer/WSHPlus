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
BK�= (Kilo / Boy x Boy) x  10000

BKO= Bel �evresi (cm) / Kal�a �evresi (cm)

19'dan k���k: �ok zay�f m�

20-25 aras�nda : Normal kilolu

25-30 aras�nda: Hafif �i�man

30 ve �zerinde: �i�man (Obez)

40 ve �zerinde: A��r� �i�man (Morbid obez)

Bel Kal�a Oran�  Kad�nda 0.8 den k���k, erkekte 0.9 dan k���k olmal�d�r.

	 */
