using ISG.Business.Abstract;
using ISG.Business.Concrete.Manager;
using ISG.DataAccess.Abstract;
using ISG.DataAccess.Concrete.EntityFramework;
using Ninject;
using System;
using System.Reflection;

namespace ISG.WebApi.App_Start
{
    public static class NinjectConfig
    {
        public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterServices(kernel);
            return kernel;
        });

        private static void RegisterServices(KernelBase kernel)
        {
            kernel.Bind<IImageUploadService>().To<ImageUploadManager>().InSingletonScope();
            kernel.Bind<IimageUploadDal>().To<EfImageUploadDal>().InSingletonScope();
            kernel.Bind<ISirketService>().To<SirketManager>().InSingletonScope();
            kernel.Bind<ISirketDal>().To<EfSirketDal>().InSingletonScope();
            kernel.Bind<ISirketBolumuService>().To<SirketBolumuManager>().InSingletonScope();
            kernel.Bind<ISirketBolumuDal>().To<EfSirketBolumuDal>().InSingletonScope();
            kernel.Bind<ISirketAtamaService>().To<SirketAtamaManager>().InSingletonScope();
            kernel.Bind<ISirketAtamaDal>().To<EfSirketAtamaDal>().InSingletonScope();
            kernel.Bind<ISirketDetayiService>().To<SirketDetayiManager>().InSingletonScope();
            kernel.Bind<ISirketDetayiDal>().To<EfSirketDetayiDal>().InSingletonScope();

            kernel.Bind<IAdresService>().To<AdresManager>().InSingletonScope();
            kernel.Bind<IAdresDal>().To<EfAdresDal>().InSingletonScope();
            kernel.Bind<IIkazService>().To<IkazManager>().InSingletonScope();
            kernel.Bind<IIkazDal>().To<EfIkazDal>().InSingletonScope();
            kernel.Bind<IAliskanlikService>().To<AliskanlikManager>().InSingletonScope();
            kernel.Bind<IAliskanlikDal>().To<EfAliskanlikDal>().InSingletonScope();
            kernel.Bind<IAllerjiService>().To<AllerjiManager>().InSingletonScope();
            kernel.Bind<IAllerjiDal>().To<EfAllerjiDal>().InSingletonScope();
            kernel.Bind<IANTService>().To<ANTManager>().InSingletonScope();
            kernel.Bind<IANTDal>().To<EfANTDal>().InSingletonScope();
            kernel.Bind<IAsiService>().To<AsiManager>().InSingletonScope();
            kernel.Bind<IAsiDal>().To<EfAsiDal>().InSingletonScope();
            kernel.Bind<IKkdService>().To<KkdManager>().InSingletonScope();
            kernel.Bind<IKkdDal>().To<EfKkdDal>().InSingletonScope();
            kernel.Bind<ICalisma_DurumuService>().To<Calisma_DurumuManager>().InSingletonScope();
            kernel.Bind<ICalisma_DurumuDal>().To<EfCalisma_DurumuDal>().InSingletonScope();
            kernel.Bind<ICalisma_GecmisiService>().To<Calisma_GecmisiManager>().InSingletonScope();
            kernel.Bind<ICalisma_GecmisiDal>().To<EfCalisma_GecmisiDal>().InSingletonScope();
            kernel.Bind<IDisRaporService>().To<DisRaporManager>().InSingletonScope();
            kernel.Bind<IDisRaporDal>().To<EfDisRaporDal>().InSingletonScope();
            kernel.Bind<IEgitimHayatiService>().To<EgitimHayatiManager>().InSingletonScope();
            kernel.Bind<IEgitimHayatiDal>().To<EfEgitimHayatiDal>().InSingletonScope();
            kernel.Bind<IIcRaporService>().To<IcRaporManager>().InSingletonScope();
            kernel.Bind<IIcRaporDal>().To<EfIcRaporDal>().InSingletonScope();
            kernel.Bind<IIsgEgitimiService>().To<IsgEgitimiManager>().InSingletonScope();
            kernel.Bind<IIsgEgitimiDal>().To<EfIsgEgitimiDal>().InSingletonScope();
            kernel.Bind<ILaboratuarService>().To<LaboratuarManager>().InSingletonScope();
            kernel.Bind<ILaboratuarDal>().To<EfLaboratuarDal>().InSingletonScope();
            kernel.Bind<IOdioService>().To<OdioManager>().InSingletonScope();
            kernel.Bind<IOdioDal>().To<EfOdioDal>().InSingletonScope();
            kernel.Bind<IOzurlulukService>().To<OzurlulukManager>().InSingletonScope();
            kernel.Bind<IOzurlulukDal>().To<EfOzurlulukDal>().InSingletonScope();
            kernel.Bind<IPansumanService>().To<PansumanManager>().InSingletonScope();
            kernel.Bind<IPansumanDal>().To<EfPansumanDal>().InSingletonScope();
            kernel.Bind<IPersonelService>().To<PersonelManager>().InSingletonScope();
            kernel.Bind<IPersonelDal>().To<EfPersonelDal>().InSingletonScope();
            kernel.Bind<IPersonelDetayiService>().To<PersonelDetayiManager>().InSingletonScope();
            kernel.Bind<IPersonelDetayiDal>().To<EfPersonelDetayiDal>().InSingletonScope();
            kernel.Bind<IRevirIslemService>().To<RevirIslemManager>().InSingletonScope();
            kernel.Bind<IRevirIslemDal>().To<EfRevirIslemDal>().InSingletonScope();
            kernel.Bind<ISFTService>().To<SFTManager>().InSingletonScope();
            kernel.Bind<ISFTDal>().To<EfSFTDal>().InSingletonScope();
            kernel.Bind<ISoyGecmisiService>().To<SoyGecmisiManager>().InSingletonScope();
            kernel.Bind<ISoyGecmisiDal>().To<EfSoyGecmisiDal>().InSingletonScope();
            kernel.Bind<ITetkikService>().To<TetkikManager>().InSingletonScope();
            kernel.Bind<ITetkikDal>().To<EfTetkikDal>().InSingletonScope();
            kernel.Bind<ITanimService>().To<TanimManager>().InSingletonScope();
            kernel.Bind<ITanimDal>().To<EfTanimDal>().InSingletonScope();
            kernel.Bind<ISaglikBirimiService>().To<SaglikBirimiManager>().InSingletonScope();
            kernel.Bind<ISaglikBirimiDal>().To<EfSaglikBirimiDal>().InSingletonScope();

            kernel.Bind<IIlacService>().To<IlacManager>().InSingletonScope();
            kernel.Bind<IIlacDal>().To<EfIlacDal>().InSingletonScope();
            kernel.Bind<IIlacSarfCikisiService>().To<IlacSarfCikisiManager>().InSingletonScope();
            kernel.Bind<IIlacSarfCikisiDal>().To<EfIlacSarfCikisiDal>().InSingletonScope();
            kernel.Bind<IIlacStokService>().To<IlacStokManager>().InSingletonScope();
            kernel.Bind<IIlacStokDal>().To<EfIlacStokDal>().InSingletonScope();
            kernel.Bind<IIlacStokGirisiService>().To<IlacStokGirisiManager>().InSingletonScope();
            kernel.Bind<IIlacStokGirisiDal>().To<EfIlacStokGirisiDal>().InSingletonScope();
            kernel.Bind<IICD10Service>().To<ICD10Manager>().InSingletonScope();
            kernel.Bind<IICD10Dal>().To<EfICD10Dal>().InSingletonScope();
            kernel.Bind<IKronikHastalikService>().To<KronikHastalikManager>().InSingletonScope();
            kernel.Bind<IKronikHastalikDal>().To<EfKronikHastalikDal>().InSingletonScope();
            kernel.Bind<IHemogramService>().To<HemogramManager>().InSingletonScope();
            kernel.Bind<IHemogramDal>().To<EfHemogramDal>().InSingletonScope();
            kernel.Bind<IRadyolojiService>().To<RadyolojiManager>().InSingletonScope();
            kernel.Bind<IRadyolojiDal>().To<EfRadyolojiDal>().InSingletonScope();
            kernel.Bind<IBoyKiloService>().To<BoyKiloManager>().InSingletonScope();
            kernel.Bind<IBoyKiloDal>().To<EfBoyKiloDal>().InSingletonScope();
            kernel.Bind<IEKGService>().To<EKGManager>().InSingletonScope();
            kernel.Bind<IEKGDal>().To<EfEKGDal>().InSingletonScope();
            kernel.Bind<IGormeService>().To<GormeManager>().InSingletonScope();
            kernel.Bind<IGormeDal>().To<EfGormeDal>().InSingletonScope();
            kernel.Bind<IRevirTedaviService>().To<RevirTedaviManager>().InSingletonScope();
            kernel.Bind<IRevirTedaviDal>().To<EfRevirTedaviDal>().InSingletonScope();
            kernel.Bind<IPersonelMuayeneService>().To<PersonelMuayeneManager>().InSingletonScope();
            kernel.Bind<IPersonelMuayeneDal>().To<EfPersonelMuayeneDal>().InSingletonScope();
            kernel.Bind<IICDSablonuService>().To<ICDSablonuManager>().InSingletonScope();
            kernel.Bind<IICDSablonuDal>().To<EfICDSablonuDal>().InSingletonScope();
            kernel.Bind<IPeriyodikMuayeneService>().To<PeriyodikMuayeneManager>().InSingletonScope();
            kernel.Bind<IPeriyodikMuayeneDal>().To<EfPeriyodikMuayeneDal>().InSingletonScope();
            kernel.Bind<IIsKazasiService>().To<IsKazasiManager>().InSingletonScope();
            kernel.Bind<IIsKazasiDal>().To<EfIsKazasiDal>().InSingletonScope();
            kernel.Bind<IMail_OnerileriService>().To<Mail_OnerileriManager>().InSingletonScope();
            kernel.Bind<IMail_OnerileriDal>().To<EfMail_OnerileriDal>().InSingletonScope();
            kernel.Bind<IMailService>().To<MailManager>().InSingletonScope();
            kernel.Bind<IAzureMailService>().To<AzureMailManager>().InSingletonScope();
            kernel.Bind<IExcelAzureService>().To<ExcelAzureManager>().InSingletonScope();
            kernel.Bind<IMeslekHastaliklariService>().To<MeslekHastaliklariManager>().InSingletonScope();
            kernel.Bind<IMeslekHastaliklariDal>().To<EfMeslekHastaliklariDal>().InSingletonScope();
            kernel.Bind<ITehlikeliIslerService>().To<TehlikeliIslerManager>().InSingletonScope();
            kernel.Bind<ITehlikeliIslerDal>().To<EfTehlikeliIslerDal>().InSingletonScope();
            kernel.Bind<IBolumRiskiService>().To<BolumRiskiManager>().InSingletonScope();
            kernel.Bind<IBolumRiskiDal>().To<EfBolumRiskiDal>().InSingletonScope();
            kernel.Bind<IPsikolojikTestService>().To<PsikolojikTestManager>().InSingletonScope();
            kernel.Bind<IPsikolojikTestDal>().To<EfPsikolojikTestDal>().InSingletonScope();
            kernel.Bind<IKtubKtService>().To<KtubKtManager>().InSingletonScope();
            kernel.Bind<IKtubKtDal>().To<EfKtubKtDal>().InSingletonScope();
            kernel.Bind<ICalismaAnaliziService>().To<CalismaAnaliziManager>().InSingletonScope();
            kernel.Bind<ICalismaAnaliziDal>().To<EfCalismaAnaliziDal>().InSingletonScope();
            kernel.Bind<IIsgTopluEgitimiService>().To<IsgTopluEgitimiManager>().InSingletonScope();
            kernel.Bind<IIsgTopluEgitimiDal>().To<EfIsgTopluEgitimiDal>().InSingletonScope();
            kernel.Bind<IISG_TopluEgitimSablonlariService>().To<ISG_TopluEgitimSablonlariManager>().InSingletonScope();
            kernel.Bind<IISG_TopluEgitimSablonlariDal>().To<EfISG_TopluEgitimSablonlariDal>().InSingletonScope();
            kernel.Bind<ISirketUploadService>().To<SirketUploadManager>().InSingletonScope();
            kernel.Bind<ISirketUploaddDal>().To<EfSirketUploadDal>().InSingletonScope();
        }
    }
}