using ISG.Entities.Abstract;
using System;

namespace ISG.Entities.Concrete
{
    public partial class SoyGecmisi : IEntity
    {
        public int SoyGecmisi_Id { get; set; }
        public string AkrabalikDurumi { get; set; }//kim anne baba vs
        public string HastalikAdi { get; set; }//Genel Hastal�k sorgusu//allerji ves
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
 Hangi Bilgileri Toplamal�y�m?

Kimlere soracaks�n�z?

1. Anne ve baba
2. Karde�ler
3. �ocuklar�n�z
4. B�y�kanne/baba
5. Amca, teyze, day� ve kuzenler

Neler Soracaks�n�z?

1. Hayatta olanlar ka� ya��nda?
2. Hastal�k ge�irdiler mi?
3. Hastal��a ka� ya��nda yakaland�lar?
4. �l�m nedenleri nedir?
5. Ka� ya��nda �ld�ler?
6. Gen� ya�ta, ani veya beklenmeyen �l�m var m�, nedeni biliniyor mu?

Hangi Hastal�klar Sorgulanmal�?

1. Alzheimer hastal���
2. Ast�m ve alerjiler
3. Do�um anomalileri
4. G�rme kayb�, k�rl�k
5. Kanserler (meme, yumurtal�k, kal�n ba��rsak, prostat gibi)
6. Gen� ya�ta i�itme kayb�
7. Geli�me, zek� gerili�i
8. �eker hastal���
9. Kalp hastal���
10. Y�ksek tansiyon
11. Y�ksek kolesterol
12. Ameliyatlar
13. Ruh hastal�klar� (Depresyon, �izofreni)
14. Obezite
15. Gebelikte sorunlar
16. Fel�
17. Madde ba��ml�l��� (Alkol, sigara)

     */
