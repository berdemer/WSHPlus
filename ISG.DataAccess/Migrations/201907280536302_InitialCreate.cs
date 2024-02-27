namespace ISG.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresler",
                c => new
                    {
                        Adres_Id = c.Int(nullable: false, identity: true),
                        Adres_Turu = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        GenelAdresBilgisi = c.String(),
                        EkAdresBilgisi = c.String(),
                        MapLokasyonu = c.String(maxLength: 100, fixedLength: true),
                        Personel_Id = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Adres_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Personel",
                c => new
                    {
                        Personel_Id = c.Int(nullable: false, identity: true),
                        PerGuid_Id = c.Guid(nullable: false, identity: true),
                        Adi = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Soyadi = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        TcNo = c.String(nullable: false, maxLength: 11, fixedLength: true),
                        Sirket_Id = c.Int(),
                        Bolum_Id = c.Int(),
                        KadroDurumu = c.String(maxLength: 50, fixedLength: true),
                        SicilNo = c.String(maxLength: 20, fixedLength: true),
                        Gorevi = c.String(maxLength: 55, fixedLength: true),
                        KanGrubu = c.String(maxLength: 20, fixedLength: true),
                        SgkNo = c.String(maxLength: 30, fixedLength: true),
                        Photo = c.String(maxLength: 100, fixedLength: true),
                        Mail = c.String(maxLength: 55, fixedLength: true),
                        Telefon = c.String(maxLength: 30, fixedLength: true),
                        Durumu = c.Boolean(nullable: false),
                        rowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Personel_Id)
                .ForeignKey("dbo.SirketBolumleri", t => t.Bolum_Id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_Id)
                .Index(t => t.Sirket_Id)
                .Index(t => t.Bolum_Id);
            
            CreateTable(
                "dbo.Aliskanliklar",
                c => new
                    {
                        Aliskanlik_Id = c.Int(nullable: false, identity: true),
                        Madde = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        BaslamaTarihi = c.DateTime(nullable: false),
                        BitisTarihi = c.DateTime(),
                        SiklikDurumu = c.String(maxLength: 50, fixedLength: true),
                        Aciklama = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Aliskanlik_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Allerjiler",
                c => new
                    {
                        Allerji_Id = c.Int(nullable: false, identity: true),
                        Oykusu = c.String(maxLength: 350, fixedLength: true),
                        Cesiti = c.String(),
                        Etken = c.String(),
                        SureYil = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Allerji_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.ANTlari",
                c => new
                    {
                        ANT_Id = c.Int(nullable: false, identity: true),
                        TASagKolSistol = c.String(maxLength: 50),
                        TASagKolDiastol = c.String(maxLength: 50),
                        TASolKolSistol = c.String(maxLength: 50),
                        TASolKolDiastol = c.String(maxLength: 50),
                        Nabiz = c.Int(),
                        Ates = c.String(maxLength: 10, fixedLength: true),
                        NabizRitmi = c.String(maxLength: 75),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(),
                        RevirIslem_Id = c.Int(),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.ANT_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.RevirIslemleri",
                c => new
                    {
                        RevirIslem_Id = c.Int(nullable: false, identity: true),
                        SaglikBirimi_Id = c.Int(nullable: false),
                        IslemTuru = c.String(maxLength: 150),
                        Protokol = c.Int(),
                        IslemDetayi = c.String(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RevirIslem_Id);
            
            CreateTable(
                "dbo.BoyKilolari",
                c => new
                    {
                        BoyKilo_Id = c.Int(nullable: false, identity: true),
                        Boy = c.Int(nullable: false),
                        Kilo = c.Int(nullable: false),
                        Bel = c.Int(),
                        Kalca = c.Int(),
                        BKI = c.Single(),
                        BKO = c.Single(),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.BoyKilo_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.EKGleri",
                c => new
                    {
                        EKG_Id = c.Int(nullable: false, identity: true),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.EKG_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Gormeleri",
                c => new
                    {
                        Gorme_Id = c.Int(nullable: false, identity: true),
                        GozKapagi = c.String(maxLength: 30, fixedLength: true),
                        GozKuresi = c.String(maxLength: 30, fixedLength: true),
                        GozKaymasi = c.String(maxLength: 30, fixedLength: true),
                        GozdeKizariklik = c.String(maxLength: 30, fixedLength: true),
                        PupillaninDurumu = c.String(maxLength: 30, fixedLength: true),
                        IsikRefleksi = c.String(maxLength: 30, fixedLength: true),
                        GormeAlani = c.String(maxLength: 30, fixedLength: true),
                        GozTonusu = c.String(maxLength: 30, fixedLength: true),
                        Fundoskopi = c.String(maxLength: 128, fixedLength: true),
                        GormeKeskinligi = c.String(maxLength: 30, fixedLength: true),
                        DerinlikDuyusu = c.String(maxLength: 30, fixedLength: true),
                        RenkKorlugu = c.String(maxLength: 20, fixedLength: true),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Gorme_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Hemogramlar",
                c => new
                    {
                        Hemogram_Id = c.Int(nullable: false, identity: true),
                        Eritrosit = c.String(maxLength: 10, fixedLength: true),
                        Hematokrit = c.String(maxLength: 10, fixedLength: true),
                        Hemoglobin = c.String(maxLength: 10, fixedLength: true),
                        MCV = c.String(maxLength: 10, fixedLength: true),
                        MCH = c.String(maxLength: 10, fixedLength: true),
                        MCHC = c.String(maxLength: 10, fixedLength: true),
                        RDW = c.String(maxLength: 10, fixedLength: true),
                        Lokosit = c.String(maxLength: 10, fixedLength: true),
                        Lenfosit_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Monosit_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Granülosit_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Notrofil_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Eoznofil_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Bazofil_Yuzde = c.String(maxLength: 10, fixedLength: true),
                        Trombosit = c.String(maxLength: 10, fixedLength: true),
                        MeanPlateletVolume = c.String(maxLength: 10, fixedLength: true),
                        Platekrit = c.String(maxLength: 10, fixedLength: true),
                        PDW = c.String(maxLength: 10, fixedLength: true),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Hemogram_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.IsKazalari",
                c => new
                    {
                        IsKazasi_Id = c.Int(nullable: false, identity: true),
                        PM = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.IsKazasi_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Laboratuarlari",
                c => new
                    {
                        Laboratuar_Id = c.Int(nullable: false, identity: true),
                        Grubu = c.String(nullable: false, maxLength: 150),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Laboratuar_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Odiolar",
                c => new
                    {
                        Odio_Id = c.Int(nullable: false, identity: true),
                        Sag250 = c.Int(),
                        Sag500 = c.Int(),
                        Sag1000 = c.Int(),
                        Sag2000 = c.Int(),
                        Sag3000 = c.Int(),
                        Sag4000 = c.Int(),
                        Sag5000 = c.Int(),
                        Sag6000 = c.Int(),
                        Sag7000 = c.Int(),
                        Sag8000 = c.Int(),
                        Sol250 = c.Int(),
                        Sol500 = c.Int(),
                        Sol1000 = c.Int(),
                        Sol2000 = c.Int(),
                        Sol3000 = c.Int(),
                        Sol4000 = c.Int(),
                        Sol5000 = c.Int(),
                        Sol6000 = c.Int(),
                        Sol7000 = c.Int(),
                        Sol8000 = c.Int(),
                        SagOrtalama = c.Int(),
                        SolOrtalama = c.Int(),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Odio_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Pansumanlari",
                c => new
                    {
                        Pansuman_Id = c.Int(nullable: false, identity: true),
                        IsKazasi = c.Boolean(),
                        YaraCesidi = c.String(maxLength: 30, fixedLength: true),
                        YaraYeri = c.String(),
                        PansumaninAmaci = c.String(maxLength: 100, fixedLength: true),
                        PansumanTuru = c.String(maxLength: 30, fixedLength: true),
                        SuturBicimi = c.String(maxLength: 30, fixedLength: true),
                        Sonuc = c.String(),
                        RevirIslem_Id = c.Int(nullable: false),
                        MuayeneTuru = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Pansuman_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.PeriyodikMuayeneleri",
                c => new
                    {
                        PeriyodikMuayene_Id = c.Int(nullable: false, identity: true),
                        PM = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.PeriyodikMuayene_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.PersonelMuayeneleri",
                c => new
                    {
                        PersonelMuayene_Id = c.Int(nullable: false, identity: true),
                        PM = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.PersonelMuayene_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.PsikolojikTestleri",
                c => new
                    {
                        PsikolojikTest_Id = c.Int(nullable: false, identity: true),
                        TestAdi = c.String(),
                        TestJson = c.String(),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(),
                        RevirIslem_Id = c.Int(),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.PsikolojikTest_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Radyolojileri",
                c => new
                    {
                        Radyoloji_Id = c.Int(nullable: false, identity: true),
                        RadyolojikTip = c.String(maxLength: 100, fixedLength: true),
                        RadyolojikIslem = c.String(),
                        Sonuc = c.String(),
                        IslemTarihi = c.DateTime(),
                        MuayeneTuru = c.String(maxLength: 100),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Radyoloji_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.RevirTedavileri",
                c => new
                    {
                        RevirTedavi_Id = c.Int(nullable: false, identity: true),
                        Sikayeti = c.String(),
                        Tani = c.String(),
                        HastaninIlaclari = c.String(),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        RevirIslem_Id = c.Int(),
                        Personel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RevirTedavi_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.IlacSarfCikislari",
                c => new
                    {
                        IlacSarfCikisi_Id = c.Guid(nullable: false),
                        IlacAdi = c.String(),
                        SarfMiktari = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        RevirTedavi_Id = c.Int(nullable: false),
                        StokId = c.Guid(nullable: false),
                        SaglikBirimi_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IlacSarfCikisi_Id)
                .ForeignKey("dbo.IlacStoklari", t => t.StokId, cascadeDelete: true)
                .ForeignKey("dbo.RevirTedavileri", t => t.RevirTedavi_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaglikBirimleri", t => t.SaglikBirimi_Id, cascadeDelete: true)
                .Index(t => t.RevirTedavi_Id)
                .Index(t => t.StokId)
                .Index(t => t.SaglikBirimi_Id);
            
            CreateTable(
                "dbo.IlacStoklari",
                c => new
                    {
                        StokId = c.Guid(nullable: false),
                        SaglikBirimi_Id = c.Int(nullable: false),
                        IlacAdi = c.String(nullable: false, maxLength: 250, fixedLength: true),
                        StokTuru = c.String(nullable: false),
                        StokMiktari = c.Int(nullable: false),
                        StokMiktarBirimi = c.String(nullable: false, maxLength: 15, fixedLength: true),
                        KritikStokMiktari = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StokId);
            
            CreateTable(
                "dbo.IlacStokGirisleri",
                c => new
                    {
                        IlacStokGirisiId = c.Guid(nullable: false),
                        StokId = c.Guid(nullable: false),
                        StokEkBilgisi = c.String(),
                        SaglikBirimi_Id = c.Int(nullable: false),
                        KutuIcindekiMiktar = c.Int(nullable: false),
                        KutuMiktari = c.Int(nullable: false),
                        ToplamMiktar = c.Int(nullable: false),
                        MiadTarihi = c.DateTime(precision: 7, storeType: "datetime2"),
                        KritikMiadTarihi = c.DateTime(precision: 7, storeType: "datetime2"),
                        ArtanMiadTelefMiktari = c.Int(nullable: false),
                        Nedeni = c.String(maxLength: 20),
                        Status = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        Tarih = c.DateTime(),
                        Maliyet = c.Decimal(nullable: false, precision: 10, scale: 2),
                    })
                .PrimaryKey(t => t.IlacStokGirisiId)
                .ForeignKey("dbo.IlacStoklari", t => t.StokId, cascadeDelete: true)
                .Index(t => t.StokId);
            
            CreateTable(
                "dbo.SaglikBirimleri",
                c => new
                    {
                        SaglikBirimi_Id = c.Int(nullable: false, identity: true),
                        Adi = c.String(nullable: false, maxLength: 150),
                        StiId = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Yil = c.Int(nullable: false),
                        Durumu = c.Boolean(nullable: false),
                        MailPort = c.Int(nullable: false),
                        MailHost = c.String(),
                        MailUserName = c.String(),
                        MailPassword = c.String(),
                        EnableSsl = c.Boolean(nullable: false),
                        UseDefaultCredentials = c.Boolean(nullable: false),
                        mailfromAddress = c.String(),
                        Domain = c.String(),
                        mailSekli = c.String(),
                    })
                .PrimaryKey(t => t.SaglikBirimi_Id);
            
            CreateTable(
                "dbo.SFTleri",
                c => new
                    {
                        Sft_Id = c.Int(nullable: false, identity: true),
                        FVC = c.Int(),
                        FEV1 = c.Int(),
                        Fev1Fvc = c.Int(),
                        VC = c.Int(),
                        RV = c.Int(),
                        TLC = c.Int(),
                        Fev2575 = c.Int(),
                        FEV50 = c.Int(),
                        MVV = c.Int(),
                        PEF = c.Int(),
                        Sonuc = c.String(),
                        MuayeneTuru = c.String(maxLength: 50),
                        RevirIslem_Id = c.Int(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        Protokol = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Sft_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Tetkikleri",
                c => new
                    {
                        Tetkik_Id = c.Int(nullable: false, identity: true),
                        MuayeneTuru = c.String(maxLength: 50),
                        TetkikTanimi = c.String(maxLength: 50),
                        TetkikSonucu = c.String(maxLength: 50),
                        HekimAdi = c.String(maxLength: 50),
                        Sikayeti = c.String(),
                        RevirIslem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Tetkik_Id)
                .ForeignKey("dbo.RevirIslemleri", t => t.RevirIslem_Id, cascadeDelete: true)
                .Index(t => t.RevirIslem_Id);
            
            CreateTable(
                "dbo.Asilar",
                c => new
                    {
                        Asi_Id = c.Int(nullable: false, identity: true),
                        Asi_Tanimi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        Yapilma_Tarihi = c.DateTime(),
                        Dozu = c.String(maxLength: 30, fixedLength: true),
                        Guncelleme_Suresi_Ay = c.Int(),
                        Muhtamel_Tarih = c.DateTime(),
                        Aciklama = c.String(maxLength: 200, fixedLength: true),
                        StokHarcamasi = c.Boolean(nullable: false),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Asi_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Calisma_Durumu",
                c => new
                    {
                        Calisma_Durumu_Id = c.Int(nullable: false, identity: true),
                        Sirket = c.String(maxLength: 150, fixedLength: true),
                        Sirket_Id = c.Int(),
                        Bolum = c.String(maxLength: 150, fixedLength: true),
                        Bolum_Id = c.Int(),
                        Baslama_Tarihi = c.DateTime(),
                        Bitis_Tarihi = c.DateTime(),
                        Status = c.Boolean(),
                        Aciklama = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        Calisma_Duzeni = c.String(maxLength: 35, fixedLength: true),
                        KadroDurumu = c.String(maxLength: 35, fixedLength: true),
                        Gorevi = c.String(maxLength: 55, fixedLength: true),
                        SicilNo = c.String(maxLength: 35, fixedLength: true),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Calisma_Durumu_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Calisma_Gecmisi",
                c => new
                    {
                        Calisma_Gecmisi_Id = c.Int(nullable: false, identity: true),
                        Calistigi_Yer_Adi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        Ise_Baslama_Tarihi = c.DateTime(),
                        Isden_Cikis_Tarihi = c.DateTime(),
                        Gorevi = c.String(nullable: false, maxLength: 100, fixedLength: true),
                        Unvani = c.String(nullable: false, maxLength: 100, fixedLength: true),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Calisma_Gecmisi_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.DısRaporlari",
                c => new
                    {
                        DisRapor_Id = c.Int(nullable: false, identity: true),
                        MuayeneTuru = c.String(maxLength: 50, fixedLength: true),
                        Tani = c.String(),
                        BaslangicTarihi = c.DateTime(),
                        BitisTarihi = c.DateTime(),
                        SureGun = c.Int(),
                        DoktorAdi = c.String(maxLength: 100, fixedLength: true),
                        RaporuVerenSaglikBirimi = c.String(maxLength: 100, fixedLength: true),
                        User_Id = c.String(maxLength: 40, fixedLength: true),
                        Personel_Id = c.Int(),
                        Revir_Id = c.Int(),
                        Sirket_Id = c.Int(),
                        Bolum_Id = c.Int(),
                    })
                .PrimaryKey(t => t.DisRapor_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.EgitimHayatlari",
                c => new
                    {
                        EgitimHayati_Id = c.Int(nullable: false, identity: true),
                        Egitim_seviyesi = c.String(maxLength: 20, fixedLength: true),
                        Okul_Adi = c.String(maxLength: 50, fixedLength: true),
                        Baslama_Tarihi = c.DateTime(),
                        Bitis_Tarihi = c.DateTime(),
                        Meslek_Tanimi = c.String(maxLength: 50, fixedLength: true),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        Personel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EgitimHayati_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.IcRaporlari",
                c => new
                    {
                        IcRapor_Id = c.Int(nullable: false, identity: true),
                        MuayeneTuru = c.String(maxLength: 50),
                        Tani = c.String(),
                        RaporTuru = c.String(maxLength: 20, fixedLength: true),
                        BaslangicTarihi = c.DateTime(),
                        BitisTarihi = c.DateTime(),
                        SureGun = c.Int(),
                        Doktor_Adi = c.String(maxLength: 100, fixedLength: true),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        Personel_Id = c.Int(nullable: false),
                        Revir_Id = c.Int(),
                        Sirket_Id = c.Int(),
                        Bolum_Id = c.Int(),
                        RevirIslem_Id = c.Int(),
                        Protokol = c.Int(),
                    })
                .PrimaryKey(t => t.IcRapor_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.IsgEgitimleri",
                c => new
                    {
                        Egitim_Id = c.Int(nullable: false, identity: true),
                        Tarih = c.DateTime(),
                        Egitim_Turu = c.String(nullable: false, maxLength: 75, fixedLength: true),
                        Tanimi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        Egitim_Suresi = c.Int(),
                        Guncelleme_Suresi_Ay = c.Int(),
                        VerildigiTarih = c.DateTime(),
                        GuncellenecekTarih = c.DateTime(),
                        Tamamlandi = c.Boolean(),
                        Aciklama = c.String(),
                        IsgEgitimiVerenPersonel = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Egitim_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Kkdleri",
                c => new
                    {
                        Kkd_Id = c.Int(nullable: false, identity: true),
                        Kkd_Tanimi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        Alinma_Tarihi = c.DateTime(),
                        Maruziyet = c.String(),
                        Guncelleme_Suresi_Ay = c.Int(),
                        Aciklama = c.String(maxLength: 200, fixedLength: true),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Kkd_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.KronikHastaliklar",
                c => new
                    {
                        KronikHastalik_Id = c.Guid(nullable: false, identity: true),
                        Personel_Id = c.Int(nullable: false),
                        HastalikAdi = c.String(),
                        KullandigiIlaclar = c.String(),
                        HastalikYilSuresi = c.Int(nullable: false),
                        AmeliyatVarmi = c.Boolean(nullable: false),
                        IsKazasi = c.Boolean(nullable: false),
                        HastalikOzurDurumu = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                        Tarih = c.DateTime(),
                    })
                .PrimaryKey(t => t.KronikHastalik_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.Ozurlulukler",
                c => new
                    {
                        Ozurluluk_Id = c.Int(nullable: false, identity: true),
                        HastalikTanimi = c.String(),
                        Oran = c.Int(nullable: false),
                        Derecesi = c.String(),
                        HastahaneAdi = c.String(),
                        Aciklama = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Ozurluluk_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.PersonelDetayi",
                c => new
                    {
                        PersonelDetay_Id = c.Int(nullable: false),
                        DogumTarihi = c.DateTime(),
                        DogumYeri = c.String(maxLength: 50, fixedLength: true),
                        Cinsiyet = c.Boolean(nullable: false),
                        Uyruk = c.String(maxLength: 10, fixedLength: true),
                        EgitimSeviyesi = c.String(maxLength: 50, fixedLength: true),
                        AskerlikDurumu = c.String(maxLength: 10, fixedLength: true),
                        IlkIseBaslamaTarihi = c.DateTime(),
                        MedeniHali = c.String(maxLength: 10, fixedLength: true),
                        CocukSayisi = c.Int(),
                        anne_adi = c.String(maxLength: 40, fixedLength: true),
                        baba_adi = c.String(maxLength: 40, fixedLength: true),
                        anne_sag = c.Boolean(),
                        anne_sag_bilgisi = c.String(),
                        baba_sag = c.Boolean(),
                        baba_sag_bilgisi = c.String(),
                        KardesSayisi = c.Int(),
                        Kardes_Sag_Bilgisi = c.String(),
                        rowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.PersonelDetay_Id)
                .ForeignKey("dbo.Personel", t => t.PersonelDetay_Id, cascadeDelete: true)
                .Index(t => t.PersonelDetay_Id);
            
            CreateTable(
                "dbo.SirketBolumleri",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        referans = c.Int(nullable: false),
                        BolumAdi = c.String(nullable: false),
                        Sirket_id = c.Int(nullable: false),
                        durumu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_id, cascadeDelete: true)
                .Index(t => t.Sirket_id);
            
            CreateTable(
                "dbo.Mail_Onerileri",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sirket_Id = c.Int(nullable: false),
                        Bolum_Id = c.Int(nullable: false),
                        Adresi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        AdiVeSoyadi = c.String(),
                        OneriTanimi = c.String(nullable: false, maxLength: 150, fixedLength: true),
                        TumSirketteOneriListesinde = c.Boolean(nullable: false),
                        gonderimSekli = c.String(maxLength: 5, fixedLength: true),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_Id, cascadeDelete: true)
                .ForeignKey("dbo.SirketBolumleri", t => t.Bolum_Id)
                .Index(t => t.Sirket_Id)
                .Index(t => t.Bolum_Id);
            
            CreateTable(
                "dbo.Sirketler",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        referans = c.Int(nullable: false),
                        sirketAdi = c.String(),
                        durumu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SirketAtamalari",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        uzmanPersonelId = c.String(nullable: false, maxLength: 50),
                        Sirket_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Sirketler", t => t.Sirket_id, cascadeDelete: true)
                .Index(t => t.Sirket_id);
            
            CreateTable(
                "dbo.SirketDetaylari",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SGKSicilNo = c.String(maxLength: 50),
                        Adres = c.String(),
                        Telefon = c.String(maxLength: 50),
                        Faks = c.String(maxLength: 50),
                        Mail = c.String(maxLength: 50),
                        ilMedullaKodu = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sirketler", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SoyGecmisleri",
                c => new
                    {
                        SoyGecmisi_Id = c.Int(nullable: false, identity: true),
                        AkrabalikDurumi = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        HastalikAdi = c.String(nullable: false, maxLength: 70, fixedLength: true),
                        AkrabaninYasi = c.Int(nullable: false),
                        AkrabaninHastaOlduguYas = c.Int(nullable: false),
                        HastalikSuAnAktifmi = c.Boolean(nullable: false),
                        ICD10 = c.String(),
                        OlumNedeni = c.String(),
                        OlumYasi = c.Int(nullable: false),
                        Tarihi = c.DateTime(),
                        Aciklama = c.String(),
                        Personel_Id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.SoyGecmisi_Id)
                .ForeignKey("dbo.Personel", t => t.Personel_Id, cascadeDelete: true)
                .Index(t => t.Personel_Id);
            
            CreateTable(
                "dbo.BolumRiskleri",
                c => new
                    {
                        BolumRiski_Id = c.Int(nullable: false, identity: true),
                        BR = c.String(),
                        Sirket_Id = c.Int(nullable: false),
                        Bolum_Id = c.Int(nullable: false),
                        Tarih = c.DateTime(),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.BolumRiski_Id);
            
            CreateTable(
                "dbo.ICD10",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        referans = c.Int(nullable: false),
                        ICD10Code = c.String(),
                        Tanimi = c.String(),
                        oncelik = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ICDSablonlari",
                c => new
                    {
                        ICDSablonu_Id = c.Int(nullable: false, identity: true),
                        ICDkod = c.String(nullable: false, maxLength: 5),
                        ICDSablonuJson = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        UserId = c.String(),
                        Tarih = c.DateTime(),
                    })
                .PrimaryKey(t => t.ICDSablonu_Id);
            
            CreateTable(
                "dbo.Ilaclar",
                c => new
                    {
                        IlacBarkodu = c.String(nullable: false, maxLength: 20, fixedLength: true),
                        IlacAdi = c.String(),
                        AtcKodu = c.String(maxLength: 10, fixedLength: true),
                        AtcAdi = c.String(),
                        FirmaAdi = c.String(maxLength: 150, fixedLength: true),
                        ReceteTuru = c.String(maxLength: 30, fixedLength: true),
                        Fiyat = c.Decimal(nullable: false, storeType: "money"),
                        Aski = c.Int(nullable: false),
                        SystemStatus = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IlacBarkodu)
                .Index(t => t.IlacBarkodu, unique: true, name: "UK_Unique_Ilac_Barkodu");
            
            CreateTable(
                "dbo.ImageUploads",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        Folder = c.String(nullable: false, maxLength: 50),
                        IdGuid = c.String(nullable: false, maxLength: 50),
                        FileName = c.String(),
                        GenericName = c.String(nullable: false),
                        FileLenght = c.Int(nullable: false),
                        MimeType = c.String(nullable: false, maxLength: 100),
                        Protokol = c.Int(nullable: false),
                        UserId = c.String(maxLength: 40, fixedLength: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.KtubKtListesi",
                c => new
                    {
                        KtubKt_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Element = c.String(),
                        FirmName = c.String(),
                        ConfirmationDateKub = c.String(),
                        ConfirmationDateKt = c.String(),
                        DocumentPathKub = c.String(),
                        DocumentPathKt = c.String(),
                    })
                .PrimaryKey(t => t.KtubKt_Id);
            
            CreateTable(
                "dbo.MeslekHastaliklari",
                c => new
                    {
                        MeslekHastaliklari_Id = c.Int(nullable: false, identity: true),
                        meslekHastalik = c.String(),
                        grubu = c.String(),
                        sure = c.String(maxLength: 20, fixedLength: true),
                    })
                .PrimaryKey(t => t.MeslekHastaliklari_Id);
            
            CreateTable(
                "dbo.Meslekler",
                c => new
                    {
                        Meslek_id = c.Int(nullable: false, identity: true),
                        MeslekAdi = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Meslek_id);
            
            CreateTable(
                "dbo.Tanimlari",
                c => new
                    {
                        tanim_Id = c.Int(nullable: false, identity: true),
                        tanimAdi = c.String(maxLength: 50),
                        tanimKisaltmasi = c.String(maxLength: 10, fixedLength: true),
                        ifade = c.String(maxLength: 128, fixedLength: true),
                        ifadeBagimliligi = c.String(maxLength: 70, fixedLength: true),
                        aciklama = c.String(maxLength: 128, fixedLength: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.tanim_Id);
            
            CreateTable(
                "dbo.TehlikeliIsler",
                c => new
                    {
                        TehlikeliIsler_Id = c.Int(nullable: false, identity: true),
                        meslek = c.String(),
                        grubu = c.String(),
                        sure = c.String(maxLength: 20, fixedLength: true),
                    })
                .PrimaryKey(t => t.TehlikeliIsler_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adresler", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.SoyGecmisleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Personel", "Sirket_Id", "dbo.Sirketler");
            DropForeignKey("dbo.Personel", "Bolum_Id", "dbo.SirketBolumleri");
            DropForeignKey("dbo.SirketBolumleri", "Sirket_id", "dbo.Sirketler");
            DropForeignKey("dbo.Mail_Onerileri", "Bolum_Id", "dbo.SirketBolumleri");
            DropForeignKey("dbo.Mail_Onerileri", "Sirket_Id", "dbo.Sirketler");
            DropForeignKey("dbo.SirketDetaylari", "Id", "dbo.Sirketler");
            DropForeignKey("dbo.SirketAtamalari", "Sirket_id", "dbo.Sirketler");
            DropForeignKey("dbo.PersonelDetayi", "PersonelDetay_Id", "dbo.Personel");
            DropForeignKey("dbo.Ozurlulukler", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.KronikHastaliklar", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Kkdleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.IsgEgitimleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.IcRaporlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.EgitimHayatlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.DısRaporlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Calisma_Gecmisi", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Calisma_Durumu", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Asilar", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.ANTlari", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Tetkikleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.SFTleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.SFTleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.RevirTedavileri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.RevirTedavileri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.IlacSarfCikislari", "SaglikBirimi_Id", "dbo.SaglikBirimleri");
            DropForeignKey("dbo.IlacSarfCikislari", "RevirTedavi_Id", "dbo.RevirTedavileri");
            DropForeignKey("dbo.IlacSarfCikislari", "StokId", "dbo.IlacStoklari");
            DropForeignKey("dbo.IlacStokGirisleri", "StokId", "dbo.IlacStoklari");
            DropForeignKey("dbo.Radyolojileri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Radyolojileri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.PsikolojikTestleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.PsikolojikTestleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.PersonelMuayeneleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.PersonelMuayeneleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.PeriyodikMuayeneleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.PeriyodikMuayeneleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Pansumanlari", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Pansumanlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Odiolar", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Odiolar", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Laboratuarlari", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Laboratuarlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.IsKazalari", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.IsKazalari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Hemogramlar", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Hemogramlar", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Gormeleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.Gormeleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.EKGleri", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.EKGleri", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.BoyKilolari", "RevirIslem_Id", "dbo.RevirIslemleri");
            DropForeignKey("dbo.BoyKilolari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.ANTlari", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Allerjiler", "Personel_Id", "dbo.Personel");
            DropForeignKey("dbo.Aliskanliklar", "Personel_Id", "dbo.Personel");
            DropIndex("dbo.Ilaclar", "UK_Unique_Ilac_Barkodu");
            DropIndex("dbo.SoyGecmisleri", new[] { "Personel_Id" });
            DropIndex("dbo.SirketDetaylari", new[] { "Id" });
            DropIndex("dbo.SirketAtamalari", new[] { "Sirket_id" });
            DropIndex("dbo.Mail_Onerileri", new[] { "Bolum_Id" });
            DropIndex("dbo.Mail_Onerileri", new[] { "Sirket_Id" });
            DropIndex("dbo.SirketBolumleri", new[] { "Sirket_id" });
            DropIndex("dbo.PersonelDetayi", new[] { "PersonelDetay_Id" });
            DropIndex("dbo.Ozurlulukler", new[] { "Personel_Id" });
            DropIndex("dbo.KronikHastaliklar", new[] { "Personel_Id" });
            DropIndex("dbo.Kkdleri", new[] { "Personel_Id" });
            DropIndex("dbo.IsgEgitimleri", new[] { "Personel_Id" });
            DropIndex("dbo.IcRaporlari", new[] { "Personel_Id" });
            DropIndex("dbo.EgitimHayatlari", new[] { "Personel_Id" });
            DropIndex("dbo.DısRaporlari", new[] { "Personel_Id" });
            DropIndex("dbo.Calisma_Gecmisi", new[] { "Personel_Id" });
            DropIndex("dbo.Calisma_Durumu", new[] { "Personel_Id" });
            DropIndex("dbo.Asilar", new[] { "Personel_Id" });
            DropIndex("dbo.Tetkikleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.SFTleri", new[] { "Personel_Id" });
            DropIndex("dbo.SFTleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.IlacStokGirisleri", new[] { "StokId" });
            DropIndex("dbo.IlacSarfCikislari", new[] { "SaglikBirimi_Id" });
            DropIndex("dbo.IlacSarfCikislari", new[] { "StokId" });
            DropIndex("dbo.IlacSarfCikislari", new[] { "RevirTedavi_Id" });
            DropIndex("dbo.RevirTedavileri", new[] { "Personel_Id" });
            DropIndex("dbo.RevirTedavileri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Radyolojileri", new[] { "Personel_Id" });
            DropIndex("dbo.Radyolojileri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.PsikolojikTestleri", new[] { "Personel_Id" });
            DropIndex("dbo.PsikolojikTestleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.PersonelMuayeneleri", new[] { "Personel_Id" });
            DropIndex("dbo.PersonelMuayeneleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.PeriyodikMuayeneleri", new[] { "Personel_Id" });
            DropIndex("dbo.PeriyodikMuayeneleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Pansumanlari", new[] { "Personel_Id" });
            DropIndex("dbo.Pansumanlari", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Odiolar", new[] { "Personel_Id" });
            DropIndex("dbo.Odiolar", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Laboratuarlari", new[] { "Personel_Id" });
            DropIndex("dbo.Laboratuarlari", new[] { "RevirIslem_Id" });
            DropIndex("dbo.IsKazalari", new[] { "Personel_Id" });
            DropIndex("dbo.IsKazalari", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Hemogramlar", new[] { "Personel_Id" });
            DropIndex("dbo.Hemogramlar", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Gormeleri", new[] { "Personel_Id" });
            DropIndex("dbo.Gormeleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.EKGleri", new[] { "Personel_Id" });
            DropIndex("dbo.EKGleri", new[] { "RevirIslem_Id" });
            DropIndex("dbo.BoyKilolari", new[] { "Personel_Id" });
            DropIndex("dbo.BoyKilolari", new[] { "RevirIslem_Id" });
            DropIndex("dbo.ANTlari", new[] { "Personel_Id" });
            DropIndex("dbo.ANTlari", new[] { "RevirIslem_Id" });
            DropIndex("dbo.Allerjiler", new[] { "Personel_Id" });
            DropIndex("dbo.Aliskanliklar", new[] { "Personel_Id" });
            DropIndex("dbo.Personel", new[] { "Bolum_Id" });
            DropIndex("dbo.Personel", new[] { "Sirket_Id" });
            DropIndex("dbo.Adresler", new[] { "Personel_Id" });
            DropTable("dbo.TehlikeliIsler");
            DropTable("dbo.Tanimlari");
            DropTable("dbo.Meslekler");
            DropTable("dbo.MeslekHastaliklari");
            DropTable("dbo.KtubKtListesi");
            DropTable("dbo.ImageUploads");
            DropTable("dbo.Ilaclar");
            DropTable("dbo.ICDSablonlari");
            DropTable("dbo.ICD10");
            DropTable("dbo.BolumRiskleri");
            DropTable("dbo.SoyGecmisleri");
            DropTable("dbo.SirketDetaylari");
            DropTable("dbo.SirketAtamalari");
            DropTable("dbo.Sirketler");
            DropTable("dbo.Mail_Onerileri");
            DropTable("dbo.SirketBolumleri");
            DropTable("dbo.PersonelDetayi");
            DropTable("dbo.Ozurlulukler");
            DropTable("dbo.KronikHastaliklar");
            DropTable("dbo.Kkdleri");
            DropTable("dbo.IsgEgitimleri");
            DropTable("dbo.IcRaporlari");
            DropTable("dbo.EgitimHayatlari");
            DropTable("dbo.DısRaporlari");
            DropTable("dbo.Calisma_Gecmisi");
            DropTable("dbo.Calisma_Durumu");
            DropTable("dbo.Asilar");
            DropTable("dbo.Tetkikleri");
            DropTable("dbo.SFTleri");
            DropTable("dbo.SaglikBirimleri");
            DropTable("dbo.IlacStokGirisleri");
            DropTable("dbo.IlacStoklari");
            DropTable("dbo.IlacSarfCikislari");
            DropTable("dbo.RevirTedavileri");
            DropTable("dbo.Radyolojileri");
            DropTable("dbo.PsikolojikTestleri");
            DropTable("dbo.PersonelMuayeneleri");
            DropTable("dbo.PeriyodikMuayeneleri");
            DropTable("dbo.Pansumanlari");
            DropTable("dbo.Odiolar");
            DropTable("dbo.Laboratuarlari");
            DropTable("dbo.IsKazalari");
            DropTable("dbo.Hemogramlar");
            DropTable("dbo.Gormeleri");
            DropTable("dbo.EKGleri");
            DropTable("dbo.BoyKilolari");
            DropTable("dbo.RevirIslemleri");
            DropTable("dbo.ANTlari");
            DropTable("dbo.Allerjiler");
            DropTable("dbo.Aliskanliklar");
            DropTable("dbo.Personel");
            DropTable("dbo.Adresler");
        }
    }
}
