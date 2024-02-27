using ISG.DataAccess.Concrete.EntityFramework.Mappings;
using ISG.Entities.Concrete;
using System.Data.Entity;

namespace ISG.DataAccess.Concrete.EntityFramework.Context
{
    public class ISGContext : DbContext
    {
        public ISGContext()
            : base("Name=DefaultConnectionAccess")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

        }
        /*Lazy Loading sadece Entity Framework'e ait olmayan programlama
          jargonunda genel bir ifadedir. Birbiriyle parent-child ilişkisi
          bulunan yapılarda parent nesnesi çağırıldığında ilgili child 
          verisinin de yüklenmesi anlamına gelir. */
        public DbSet<Meslek> Meslekler { get; set; }
        public DbSet<SaglikBirimi> SaglikBirimleri { get; set; }
        public DbSet<imageUpload> imageUploads { get; set; }
        public DbSet<Sirket> Sirketler { get; set; }
        public DbSet<SirketBolumu> SirketBolumleri { get; set; }
        public DbSet<SirketAtama> SirketAtamalari { get; set; }
        public DbSet<SirketDetayi> SirketDetaylari { get; set; }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Aliskanlik> Aliskanliklar { get; set; }
        public DbSet<Allerji> Allerjiler { get; set; }
        public DbSet<Asi> Asilar { get; set; }
        public DbSet<Calisma_Durumu> Calisma_Durumu { get; set; }
        public DbSet<Calisma_Gecmisi> Calisma_Gecmisi { get; set; }
        public DbSet<EgitimHayati> EgitimHayatlari { get; set; }
        public DbSet<IsgEgitimi> IsgEgitimleri { get; set; }
        public DbSet<Ozurluluk> Ozurlulukler { get; set; }
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<PersonelDetayi> PersonelDetaylari { get; set; }
        public DbSet<SoyGecmisi> SoyGecmisleri { get; set; }
        public DbSet<ANT> ANTlari { get; set; }
        public DbSet<DisRapor> DısRaporlari { get; set; }
        public DbSet<IcRapor> IcRaporlari { get; set; }
        public DbSet<Laboratuar> Laboratuarlari { get; set; }
        public DbSet<Odio> Odiolari { get; set; }
        public DbSet<Pansuman> Pansumanlari { get; set; }
        public DbSet<RevirIslem> RevirIslemleri { get; set; }
        public DbSet<SFT> SFTleri { get; set; }
        public DbSet<Tetkik> Tetkikleri { get; set; }
        public DbSet<Tanim> Tanimlari { get; set; }
        public DbSet<Ilac> Ilaclar { get; set; }
        public DbSet<IlacStok> IlacStoklari { get; set; }
        public DbSet<IlacStokGirisi> IlacStokGirisleri { get; set; }
        public DbSet<IlacSarfCikisi> IlacSarfCikislari { get; set; }
        public DbSet<ICD10> ICD10 { get; set; }
        public DbSet<KronikHastalik> KronikHastaliklar { get; set; }
        public DbSet<Hemogram> Hemogramlar { get; set; }
        public DbSet<Radyoloji> Radyolojileri { get; set; }
        public DbSet<ICDSablonu> ICDSablonlari { get; set; }
        public DbSet<BoyKilo> BoyKilolari { get; set; }
        public DbSet<EKG> EKGleri { get; set; }
        public DbSet<Gorme> Gormeleri { get; set; }
        public DbSet<RevirTedavi> RevirTedavileri { get; set; }
        public DbSet<PersonelMuayene> PersonelMuayeneleri { get; set; }
        public DbSet<PeriyodikMuayene> PeriyodikMuayeneleri { get; set; }
        public DbSet<IsKazasi> IsKazalari { get; set; }
        public DbSet<Kkd> Kkdleri { get; set; }
        public DbSet<Mail_Onerileri> Mail_Onerileri { get; set; }
        public DbSet<TehlikeliIsler> TehlikeliIsler { get; set; }
        public DbSet<MeslekHastaliklari> MeslekHastaliklari { get; set; }
        public DbSet<BolumRiski> BolumRiskleri { get; set; }
        public DbSet<PsikolojikTest> PsikolojikTestler { get; set; }
        public DbSet<KtubKt> KtubKtListesi { get; set; }
        public DbSet<Ikaz> Ikazlar { get; set; }
        public DbSet<CalismaAnalizi> CalismaAnalizleri { get; set; }
        public DbSet<IsgTopluEgitimi> IsgTopluEgitimleri { get; set; }
         public DbSet<SirketUpload> SirketUploadlari { get; set; }

        public DbSet<ISG_TopluEgitimSablonlari> ISG_TopluEgitimSablonlari { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AdresMap());
            modelBuilder.Configurations.Add(new AliskanlikMap());
            modelBuilder.Configurations.Add(new AllerjiMap());
            modelBuilder.Configurations.Add(new ANTMap());
            modelBuilder.Configurations.Add(new AsiMap());
            modelBuilder.Configurations.Add(new Calisma_DurumuMap());
            modelBuilder.Configurations.Add(new Calisma_GecmisiMap());
            modelBuilder.Configurations.Add(new DisRaporMap());
            modelBuilder.Configurations.Add(new IcRaporMap());
            modelBuilder.Configurations.Add(new LaboratuarMap());
            modelBuilder.Configurations.Add(new OdioMap());
            modelBuilder.Configurations.Add(new OzurlulukMap());
            modelBuilder.Configurations.Add(new PansumanMap());
            modelBuilder.Configurations.Add(new PersonelMap());
            modelBuilder.Configurations.Add(new PersonelDetayiMap());
            modelBuilder.Configurations.Add(new RevirIslemMap());
            modelBuilder.Configurations.Add(new SFTMap());
            modelBuilder.Configurations.Add(new SoyGecmisiMap());
            modelBuilder.Configurations.Add(new TetkikMap());
            modelBuilder.Configurations.Add(new EgitimHayatiMap());
            modelBuilder.Configurations.Add(new IsgEgitimiMap());
            modelBuilder.Configurations.Add(new TanimMap());
            modelBuilder.Configurations.Add(new MeslekMap());
            modelBuilder.Configurations.Add(new SaglikBirimiMap());
            modelBuilder.Configurations.Add(new ImageUploadMap());
            modelBuilder.Configurations.Add(new SirketMap());
            modelBuilder.Configurations.Add(new SirketBolumuMap());
            modelBuilder.Configurations.Add(new SirketAtamaMap());
            modelBuilder.Configurations.Add(new SirketDetayiMap());
            modelBuilder.Configurations.Add(new IlacMap());
            modelBuilder.Configurations.Add(new IlacStokMap());
            modelBuilder.Configurations.Add(new IlacStokGirisiMap());
            modelBuilder.Configurations.Add(new IlacSarfCikisiMap());
            modelBuilder.Configurations.Add(new ICD10Map());
            modelBuilder.Configurations.Add(new KronikHastalikMap());
            modelBuilder.Configurations.Add(new HemogramMap());
            modelBuilder.Configurations.Add(new RadyolojiMap());
            modelBuilder.Configurations.Add(new BoyKiloMap());
            modelBuilder.Configurations.Add(new EKGMap());
            modelBuilder.Configurations.Add(new GormeMap());
            modelBuilder.Configurations.Add(new RevirTedaviMap());
            modelBuilder.Configurations.Add(new PersonelMuayeneMap());
            modelBuilder.Configurations.Add(new PeriyodikMuayeneMap());
            modelBuilder.Configurations.Add(new ICDSablonuMap());
            modelBuilder.Configurations.Add(new IsKazasiMap());
            modelBuilder.Configurations.Add(new KkdMap());
            modelBuilder.Configurations.Add(new Mail_OnerileriMap());
            modelBuilder.Configurations.Add(new TehlikeliIslerMap());
            modelBuilder.Configurations.Add(new MeslekHastaliklariMap());
            modelBuilder.Configurations.Add(new BolumRiskiMap());
            modelBuilder.Configurations.Add(new PsikolojikTestMap());
            modelBuilder.Configurations.Add(new KtubKtMap());
            modelBuilder.Configurations.Add(new IkazMap());
            modelBuilder.Configurations.Add(new CalismaAnalizMap());
            modelBuilder.Configurations.Add(new IsgTopluEgitimiMap());
            modelBuilder.Configurations.Add(new ISG_TopluEgitimSablonlariMap());
            modelBuilder.Configurations.Add(new SirketUploadMap());
        }
    }
}
