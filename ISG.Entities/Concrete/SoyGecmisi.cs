using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class SoyGecmisi : IEntity
    {
        public int SoyGecmisi_Id { get; set; }
        public string AkrabalikDurumi { get; set; }//kim anne baba vs
        public string HastalikAdi { get; set; }//Genel Hastalýk sorgusu//allerji ves
        public int AkrabaninYasi { get; set; }
        public int AkrabaninHastaOlduguYas { get; set; }
        public bool HastalikSuAnAktifmi { get; set; }
        public string ICD10 { get; set; }
        public string OlumNedeni { get; set; }
        public int OlumYasi { get; set; }
        public DateTime? Tarihi { get; set; }
        public string Aciklama { get; set; }
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
/*
 Hangi Bilgileri Toplamalýyým?

Kimlere soracaksýnýz?

1. Anne ve baba
2. Kardeþler
3. Çocuklarýnýz
4. Büyükanne/baba
5. Amca, teyze, dayý ve kuzenler

Neler Soracaksýnýz?

1. Hayatta olanlar kaç yaþýnda?
2. Hastalýk geçirdiler mi?
3. Hastalýða kaç yaþýnda yakalandýlar?
4. Ölüm nedenleri nedir?
5. Kaç yaþýnda öldüler?
6. Genç yaþta, ani veya beklenmeyen ölüm var mý, nedeni biliniyor mu?

Hangi Hastalýklar Sorgulanmalý?

1. Alzheimer hastalýðý
2. Astým ve alerjiler
3. Doðum anomalileri
4. Görme kaybý, körlük
5. Kanserler (meme, yumurtalýk, kalýn baðýrsak, prostat gibi)
6. Genç yaþta iþitme kaybý
7. Geliþme, zekâ geriliði
8. Þeker hastalýðý
9. Kalp hastalýðý
10. Yüksek tansiyon
11. Yüksek kolesterol
12. Ameliyatlar
13. Ruh hastalýklarý (Depresyon, Þizofreni)
14. Obezite
15. Gebelikte sorunlar
16. Felç
17. Madde baðýmlýlýðý (Alkol, sigara)

     */
