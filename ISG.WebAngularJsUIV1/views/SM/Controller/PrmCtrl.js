'use strict';
function PrmCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams, TkSvc, SMSvc, $cookies, printer, mailSvc,
    ngAuthSettings, uploadService, authService, $q, $window, $timeout, notify, $http, PmSvc, $uibModal, $rootScope, $location, $anchorScroll,
    $document, analizSvc) {
    var PrmC = this;
    PrmC.url = './views/SM/View/temp/calismaAnalizi.html';
    PmSvc.JsonGetFile('isgumSablon').then(function (response) {
        PrmC.mSablonu = response;
        PrmC.disSablonu = response;
    });

    analizSvc.MeslekListesi().then(function (response) {
        PrmC.MeslekListesi = response;
        PrmC.MeslekListesi.unshift({ MeslekAdi: null, Meslek_Kodu:null});
    });
    PrmC.meslekDurumu = null;


    PmSvc.JsonGetFile('meslekler').then(function (response) {
        PrmC.Meslekler = response.calisanMeslegi;
    });
    PmSvc.JsonGetFile('calisma').then(function (response) {
        PrmC.calismaDatalari = response;
    });
    PmSvc.JsonGetFile('kanaatSonuclari').then(function (response) {
        PrmC.kanaatSonuclari = response.data;
    });

    $scope.$watch(function () { return PrmC.meslekDurumu; }, function (newValue, oldValue) {
        analizSvc.MeslekAra(encodeURI(newValue.Meslek_Kodu)).then(function (response) {
            PrmC.mSablonu = JSON.parse(response.CAJson);
        });
    });

    PrmC.BolumAraCalisanAnalizi = function (blmId, stiId) {
        analizSvc.BolumAra(blmId, stiId).then(function (response) {
            if (response !== null) {
                PrmC.CalismaAnaliziModeli = response;
                PrmC.mSablonu = JSON.parse(response.CAJson);
                PrmC.meslekDurumu.MeslekAdi = PrmC.CalismaAnaliziModeli.MeslekAdi;
                PrmC.meslekDurumu.Meslek_Kodu = PrmC.CalismaAnaliziModeli.Meslek_Kodu;
            }
        });
    };

    $scope.$watch(function () { return angular.toJson([PrmC.periyodikMuayene]); }, function (newValue, oldValue) {
        if (newValue !== oldValue && PrmC.gitme === false) {
            PmSvc.prmb.PeriyodikMuayene = (JSON.parse(newValue))[0];
        };
        if (!angular.isUndefined(PrmC.periyodikMuayene.Odio)) {
            if (PrmC.periyodikMuayene.Odio === null || PrmC.periyodikMuayene.Odio === "")
            {
                PrmC.mSablonu.isitmeTestiYapildiMi = false;
            }
            else
            {
                PrmC.mSablonu.isitmeTestiYapildiMi = true;
            }
        };
        if (!angular.isUndefined(PrmC.periyodikMuayene.Sft)) {
            if (PrmC.periyodikMuayene.Sft === null || PrmC.periyodikMuayene.Sft === "") {
                PrmC.mSablonu.solunumFonkTestiYapildiMi = false;
            }
            else {
                PrmC.mSablonu.solunumFonkTestiYapildiMi = true;
            }
        }
        if (!angular.isUndefined(PrmC.periyodikMuayene.Radyoloji)) {
            if (PrmC.periyodikMuayene.Radyoloji === null || PrmC.periyodikMuayene.Radyoloji === "") {
                PrmC.mSablonu.rontgenYapildiMi = false;
            }
            else {
                PrmC.mSablonu.rontgenYapildiMi = true;
            }
        }
        if (!angular.isUndefined(PrmC.periyodikMuayene.Laboratuar)) {
            if (PrmC.periyodikMuayene.Laboratuar === null || PrmC.periyodikMuayene.Laboratuar === "") {
                PrmC.mSablonu.kanTetkikiYapildiMi = false;
            }
            else {
                PrmC.mSablonu.kanTetkikiYapildiMi = true;
            }
        }
        if (!angular.isUndefined(PrmC.periyodikMuayene.Hemogram)) {
            if (PrmC.periyodikMuayene.Hemogram === null || PrmC.periyodikMuayene.Hemogram === "") {
                PrmC.mSablonu.kanTetkikiYapildiMi = false;
            }
            else {
                PrmC.mSablonu.kanTetkikiYapildiMi = true;
            }
        }
        if (!angular.isUndefined(PrmC.periyodikMuayene.IdrarTesti)) {
            if (PrmC.periyodikMuayene.IdrarTesti === null || PrmC.periyodikMuayene.IdrarTesti === "") {
                PrmC.mSablonu.idrarTetkikiYapildiMi = false;
            }
            else {
                PrmC.mSablonu.idrarTetkikiYapildiMi = true;
            }
        }
        PrmC.disSablonu.calisanMeslegi = PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi;
        PrmC.disSablonu.calismaZamani = PrmC.periyodikMuayene.CalismaAnalizi.calismaZamani;
        PrmC.disSablonu.calismaPozisyonu = PrmC.periyodikMuayene.CalismaAnalizi.calismaPozisyonu;
        PrmC.disSablonu.aracKullanimi = PrmC.periyodikMuayene.CalismaAnalizi.aracKullanimi;
        PrmC.disSablonu.fizikiOrtamSinifi = PrmC.periyodikMuayene.CalismaAnalizi.fizikiOrtamSinifi;
        PrmC.disSablonu.havaSinifi = PrmC.periyodikMuayene.CalismaAnalizi.havaSinifi;
        PrmC.disSablonu.gurultuSinifi = PrmC.periyodikMuayene.CalismaAnalizi.gurultuSinifi;
        PrmC.disSablonu.elektrikSinifi = PrmC.periyodikMuayene.CalismaAnalizi.elektrikSinifi;
        PrmC.disSablonu.tozYonetmeligiSinifi = PrmC.periyodikMuayene.CalismaAnalizi.tozYonetmeligiSinifi;
        PrmC.disSablonu.kimyasalMaddeSinifi = PrmC.periyodikMuayene.CalismaAnalizi.kimyasalMaddeSinifi;
        PrmC.disSablonu.biyolojikEtkenlerSinifi = PrmC.periyodikMuayene.CalismaAnalizi.biyolojikEtkenlerSinifi;
        PrmC.disSablonu.isEkipmanlari = PrmC.periyodikMuayene.CalismaAnalizi.isEkipmanlari;
    });



    var yeni = {
        Nedeni: "Periyodik Muayene",
        Kanaat: {
            Kosullari: [],
            Sonucu: "Çalışması Elverişlidir"
        },
        FM: {
            goz: [
                "Görme Keskinliği Normal",
                "Renk Körlüğü Yok"
            ],
            kbb: [
                "Normal"
            ],
            deri: [
                "Turgor Tonus Normal"
            ],
            kardiyoloji: [
                "Kalp Sesleri Normal",
                "Üfürüm Yok"
            ],
            solunum: [
                "Solunum Sesleri Normal",
                "Göğüs Kafesi solunuma eşit katılıyor ve Düzgün"
            ],
            sindirim: [
                "Batın Normal",
                "Organamegali Saptanmadı"
            ],
            uroloji: [
                "Muayene Normal"
            ],
            ortopedi: [
                "Muayene Normal",
                "Kontraktür Yok",
                "Kas Gücü Normal"
            ],
            noroloji: [
                "Muayene Normal",
                "Refleksler Normal",
                "Hareket ve Koordinasyon Normal"
            ],
            psikiatri: [
                "Duygulanım Normal",
                "Depresif bulgu saptanmadı"
            ]
        },
        Yakinmalar: {
            "01": false,
            "02": false,
            "03": false,
            "04": false,
            "05": false,
            "06": false,
            "07": false
        },
        OzelSorular: {
            "01": false,
            "02": false,
            "03": false,
            "04": false,
            "05": false,
            "06": false
        },
        Hastaliklar: {
            "10": false,
            "04": false,
            "05": false,
            "06": false,
            "07": false,
            "08": false,
            "03": false,
            "02": false,
            "01": false,
            "09": false
        },
        CalismaAnalizi: {
            calisanMeslegi: {
                Kod: null,
                MeslekAdi: null
            },
            calismaPozisyonu: {
                No: null,
                pozisyon: null
            },
            calismaZamani: {
                No: null,
                zamanDilimi: null
            },
            aracKullanimi: {
                No: null,
                kullanimSekli: null
            },
            calismaSekli: {
                firmaKodu: null,
                parola: null
            },
            calismaOrtami: {
                turKodu: 0,
                turAdi: "",
                firmaKodu: "",
                parola: ""
            },
            fizikiOrtamSinifi: [],
            havaSinifi: [],
            gurultuSinifi: [],
            elektrikSinifi: [],
            tozYonetmeligiSinifi: [],
            kimyasalMaddeSinifi: [],
            biyolojikEtkenlerSinifi: [],
            isEkipmanlari: [],
            rontgenYapildiMi: false,
            isitmeTestiYapildiMi: false,
            solunumFonkTestiYapildiMi: false,
            kanTetkikiYapildiMi: false,
            idrarTetkikiYapildiMi: false,
        },
    };
    
    $scope.$watch(function () { return PrmC.mSablonu; }, function (newValue, oldValue) {
        yeni.CalismaAnalizi = newValue;
    });

    $scope.$watch(function () { return PrmC.CalismaAnaliziModeli.CalismaAnalizi_Id }, function (newValue, oldValue) {
        if (newValue === null) {
            PrmC.BolumeKaydetBilgisi = "Bolume Şablonu Yeni Kaydet"
        } else PrmC.BolumeKaydetBilgisi = "Bolume Şablonu Güncel Kaydet";
    });

    $scope.$watch(function () { return PrmC.mSablonu.calisanMeslegi.Kod  }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            if (oldValue.Kod!== null)
                PrmC.CalismaAnaliziModeli.CalismaAnalizi_Id = null;
            PrmC.disSablonu.calisanMeslegi = PrmC.mSablonu.calisanMeslegi;
        }
    });

    $scope.selectTypeAhead = function ($item) {
        PrmC.mSablonu.calisanMeslegi.Kod = $item.Kod;
        $scope.typeAheadFlag = true;
    };

    $scope.$watch(function () { return PrmC.mSablonu.calisanMeslegi}, function (newValue, oldValue) {
        if ($scope.typeAheadFlag) {
            $scope.typeAheadFlag = false;
        } else {
            PrmC.mSablonu.calisanMeslegi=newValue;
        }
    });

    PrmC.calismaAnalizi =
        function isNullOrWhitespace(input) {
            return !input || !input.trim();
        }

    PrmC.checkBrowser = !!$window.chrome && !!$window.chrome.webstore;

    PrmC.translation = {
        buttonDefaultText: 'Kanaat Koşullarnızı Seçiniz',
        checkAll: 'Tümünü Seç',
        uncheckAll: 'Tümü İptal'
    };

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    PrmC.acKapa = true;
    PrmC.acilKapa = false;
    PrmC.AcilKapa = function () {
        $scope.oneAtATime = false;
        $scope.status = {
            open1: PrmC.acilKapa, open2: PrmC.acilKapa, open3: PrmC.acilKapa, open4: PrmC.acilKapa, open5: PrmC.acilKapa, open6: PrmC.acilKapa, open7: PrmC.acilKapa, open8: PrmC.acilKapa, open9: PrmC.acilKapa, open10: PrmC.acilKapa,
            open11: PrmC.acilKapa, open12: PrmC.acilKapa, open13: PrmC.acilKapa, open14: PrmC.acilKapa, open15: PrmC.acilKapa, open16: PrmC.acilKapa, open17: PrmC.acilKapa, open18: PrmC.acilKapa, open19: PrmC.acilKapa, open20: PrmC.acilKapa,
            open21: PrmC.acilKapa, open22: PrmC.acilKapa, open23: PrmC.acilKapa, open24: PrmC.acilKapa, open25: PrmC.acilKapa, open26: PrmC.acilKapa
        };
    };

    $rootScope.SB = $cookies.get('SB') === null || $cookies.get('SB') === undefined ? undefined : JSON.parse($cookies.get('SB'));

    $scope.nbv = angular.isUndefined($rootScope.SB) ?
        ($scope.birimler.count === 1 ? { SirketAdi: $scope.birimler[0].SirketAdi, SaglikBirimiAdi: $scope.birimler[0].SaglikBirimiAdi, SaglikBirimi_Id: $scope.birimler[0].SaglikBirimi_Id } : { SirketAdi: "Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null })
        : $rootScope.SB;

    $scope.$watch(function () { return $scope.nbv; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue.SaglikBirimi_Id) || newValue.SaglikBirimi_Id !== null) {
            PmSvc.prmb.SaglikBirimi_Id = newValue.SaglikBirimi_Id;
            $rootScope.SB = newValue;
            var zaman = new Date();
            zaman.setTime(zaman.getTime() + 2 * 60 * 60 * 1000);//son kullanımdan 2 saat sonra iptal olacak
            $cookies.put('SB', JSON.stringify(newValue), { expires: zaman.toString() });
        }
    });

    var dataAl = function (val) {
        PmSvc.GetPrmPersonel(val).then(function (s) {
            PmSvc.prmb.prmbilgi = s;
            PmSvc.prmb.guidId = val;
            PmSvc.prmb.PeriyodikMuayeneAktarimi = true;
            PrmC.warning = s.data.Ikazlar.length > 0 ? true : false;
            PrmC.warningLength = s.data.Ikazlar.length;
            //PrmC.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
            PrmC.fileImgPath = ngAuthSettings.isAzure ? ngAuthSettings.storageLinkService + 'personel/' : ngAuthSettings.storageLinkService + uploadFolder;
            PrmC.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
            PrmC.PRMB = s;
            PrmC.AdSoyad = s.data.Adi.trim() + ' ' + s.data.Soyadi.trim();
            PrmC.guid = val;
            PrmC.PrMleri = s.data.PeriyodikMuayeneleri;
            PrmC.TAtotalItems = PrmC.PRMB.data.ANTlari.length;
            $scope.tansiyonlar = PrmC.PRMB.data.ANTlari;
            $scope.BoyKilolari = PrmC.PRMB.data.BoyKilolari;
            PrmC.BKtotalItems = PrmC.PRMB.data.BoyKilolari.length;
            $scope.Odiolar = PrmC.PRMB.data.Odiolar;
            PrmC.OdiototalItems = PrmC.PRMB.data.Odiolar.length;
            $scope.SFTleri = PrmC.PRMB.data.SFTleri;
            PrmC.SfttotalItems = PrmC.PRMB.data.SFTleri.length;
            $scope.Radyolojileri = PrmC.PRMB.data.Radyolojileri;
            PrmC.RADtotalItems = PrmC.PRMB.data.Radyolojileri.length;
            $scope.Laboratuarlari = PrmC.PRMB.data.Laboratuarlari;
            PrmC.LABtotalItems = PrmC.PRMB.data.Laboratuarlari.length;
            $scope.Hemogramlar = PrmC.PRMB.data.Hemogramlar;
            PrmC.HEMtotalItems = PrmC.PRMB.data.Hemogramlar.length;
            $scope.EKGleri = PrmC.PRMB.data.EKGleri;
            PrmC.EKGtotalItems = PrmC.PRMB.data.EKGleri.length;
            PrmC.mailListesi = [];
            var i = 0;
            angular.forEach(PrmC.PRMB.mailListeOnerileri, function (v) {
                PrmC.mailListesi.push({ i: i + 1, o: true, to: 'Kime', ad: v.AdiSoyadi.trim(), email: v.MailAdresi.trim() });
                i = i + 1;
            });
            $scope.totalItems = PrmC.mailListesi.length;
            if (angular.isUndefined(PrmC.prm.PeriyodikMuayene_Id) || PrmC.prm.PeriyodikMuayene_Id === null) { PrmC.BolumAraCalisanAnalizi(PrmC.PRMB.bolumId, PrmC.PRMB.sirketId) };
            uploadService.GetImageStiId("logo", PrmC.PRMB.sirketId).then(function (response) {
                $scope.picFile = response[0].LocalFilePath;
            });

            SMSvc.AllSbirimleri().then(function (data) {
                var rs = $filter('filter')(data, {
                    SirketId: s.sirketId
                });
                $scope.birimler = data;
                var index = rs.map(function (item) {
                    return item.SirketId;
                }).indexOf($scope.nbv.SirketId);
                if (index === -1) {
                    $scope.nbv = { SirketAdi: "Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null };
                };
            });
        });
    };

    $scope.maxSize = 2;

    PrmC.TAviewby = 3;
    PrmC.TAcurrentPage = 1;
    PrmC.TAitemsPerPage = PrmC.TAviewby;
    PrmC.TAsetItemsPerPage = function (num) {
        PrmC.TAitemsPerPage = num;
    };
    PrmC.TArowSec = function (row, ta) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.TA =
            (ta.TASagKolSistol !== null && ta.TASagKolDiastol !== null ? ' Sağ Kol:' + $scope.Tansiyon(ta.TASagKolSistol, ta.TASagKolDiastol) : ' ') +
            (ta.TASolKolSistol !== null && ta.TASolKolDiastol !== null ? ' Sol Kol:' + $scope.Tansiyon(ta.TASolKolSistol, ta.TASolKolDiastol) : ' ') +
            (ta.Nabiz != null ? ' Nabız:' + $scope.Nabiz(ta.Nabiz) : '');
    };

    PrmC.BKviewby = 3;
    PrmC.BKcurrentPage = 1;
    PrmC.BKitemsPerPage = PrmC.BKviewby;
    PrmC.BKsetItemsPerPage = function (num) {
        PrmC.BKitemsPerPage = num;
    };
    PrmC.BKrowSec = function (row, boyKilo) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.BoyKilo =
            (boyKilo.Boy != null ? ' Boy:' + $scope.Vurgusu(boyKilo.Boy, ' Cm') : ' ') +
            (boyKilo.Kilo != null ? ' Kilo:' + $scope.Vurgusu(boyKilo.Kilo, ' Kg') : ' ') +
            (boyKilo.Bel != null ? ' Bel:' + $scope.Vurgusu(boyKilo.Bel, ' Cm') : ' ') +
            (boyKilo.Kalca != null ? ' Kalça:' + $scope.Vurgusu(boyKilo.Kalca, ' Cm') : ' ') +
            (boyKilo.BKI != null ? ' BKI:' + $scope.Vurgusu(boyKilo.BKI, ' Kg/m²') : ' ') +
            (boyKilo.BKO != null ? ' BKO:' + boyKilo.BKO : ' ')
            ;
    };

    PrmC.Odioviewby = 3;
    PrmC.OdiocurrentPage = 1;
    PrmC.OdioitemsPerPage = PrmC.Odioviewby;
    PrmC.OdiosetItemsPerPage = function (num) { PrmC.OdioitemsPerPage = num; };
    PrmC.OdiorowSec = function (row, odio) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.Odio =
            (odio.SagOrtalama != null ? ' Sağ Kulak:' + $scope.Vurgusu(odio.SagOrtalama, ' dB') : ' ') +
            (odio.SolOrtalama, ' dB' != null ? ' Sol Kulak:' + $scope.Vurgusu(odio.SolOrtalama, ' dB') : ' ')
            ;
    };

    PrmC.OdioNormal = function () {
        PrmC.periyodikMuayene.Odio = 'Bilateral İşitme Bulgusu Normaldir.';
    };

    PrmC.Sftviewby = 3;
    PrmC.SftcurrentPage = 1;
    PrmC.SftitemsPerPage = PrmC.Sftviewby;
    PrmC.SftsetItemsPerPage = function (num) {
        PrmC.SftitemsPerPage = num;
    };
    PrmC.SftrowSec = function (row, sft) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.Sft =
            (sft.FVC != null ? ' FVC:' + sft.FVC : ' ') +
            (sft.FEV1 != null ? ' FEV1:' + sft.FEV1 : ' ') +
            (sft.Fev1Fvc != null ? ' FEV1FVC:' + sft.Fev1Fvc : ' ') +
            (sft.PEF != null ? ' PEF:' + sft.PEF : ' ');
    };

    PrmC.RADviewby = 3;
    PrmC.RADcurrentPage = 1;
    PrmC.RADitemsPerPage = PrmC.RADviewby;
    PrmC.RADsetItemsPerPage = function (num) {
        PrmC.RADitemsPerPage = num;
    };
    PrmC.RADrowSec = function (row, radyoloji) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.Radyoloji =
            (radyoloji.RadyolojikIslem != null ? ' İşlem:' + radyoloji.RadyolojikIslem : ' ') +
            (radyoloji.RadyolojikSonuc != null ? ' Sonuç:' + radyoloji.RadyolojikSonuc : ' ');
    };

    PrmC.LABviewby = 3;
    PrmC.LABcurrentPage = 1;
    PrmC.LABitemsPerPage = PrmC.LABviewby;
    PrmC.LABsetItemsPerPage = function (num) {
        PrmC.LABitemsPerPage = num;
    };
    PrmC.LABrowSec = function (row, laboratuar) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.Laboratuar =
            (laboratuar.Sonuc != null ? '' + laboratuar.Sonuc : ' ');
    };

    PrmC.HEMviewby = 3;
    PrmC.HEMcurrentPage = 1;
    PrmC.HEMitemsPerPage = PrmC.HEMviewby;
    PrmC.HEMsetItemsPerPage = function (num) {
        PrmC.HEMitemsPerPage = num;
    };
    PrmC.HEMrowSec = function (row, hemogram) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.Hemogram =
            hemogram != null ? $scope.Hem(hemogram.Eritrosit, hemogram.Hematokrit, hemogram.Hemoglobin, hemogram.Lokosit, hemogram.MCV, hemogram.MCH, hemogram.MCHC, hemogram.Trombosit, hemogram.Platekrit) : ' ';
    };

    PrmC.EKGviewby = 3;
    PrmC.EKGcurrentPage = 1;
    PrmC.EKGitemsPerPage = PrmC.EKGviewby;
    PrmC.EKGsetItemsPerPage = function (num) {
        PrmC.EKGitemsPerPage = num;
    };
    PrmC.EKGrowSec = function (row, ekg) {
        $scope.selectedRow = row;
        PrmC.periyodikMuayene.EKG = ekg.Sonuc != null ? ekg.Sonuc : ' ';
    };

    if (!angular.isUndefined($stateParams.id)) {
        dataAl($stateParams.id);
    }

    $scope.yukleme = function () {//sayfa yüklemesi bittikten sonra
        if (angular.isUndefined(PmSvc.prmb.AccordionDurumu)) {
            $scope.status = {
                open1: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false, open10: false,
                open11: false, open12: false, open13: false, open14: false, open15: false, open16: false, open17: false, open18: false, open19: false, open20: false,
                open21: false, open22: false, open23: false, open24: false, open25: PrmC.acilKapa, open26: PrmC.acilKapa
            };
            PmSvc.prmb.PeriyodikMuayene = null;
        }
        if ($stateParams.id === PmSvc.prmb.guidId) {
            $scope.status = (JSON.parse(PmSvc.prmb.AccordionDurumu))[0];
            PrmC.periyodikMuayene = PmSvc.prmb.PeriyodikMuayene === '' ? yeni : PmSvc.prmb.PeriyodikMuayene;
            $scope.oneAtATime = false;
            var lokal = $location.hash();
            $location.hash(lokal);
            $timeout(function () { $anchorScroll(); }, 1500);
        } else {
            PmSvc.JsonGetFile('isgumSablon').then(function (response) {
                PrmC.mSablonu = response;
                yeni.CalismaAnalizi = response;
                PrmC.periyodikMuayene = yeni;
                PmSvc.prmb.PeriyodikMuayene = yeni;
            });

        }
    };



    $scope.dtOptionsPrmC = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
            { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Periyodik Muayene Listesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Periyodik Muayene Listesi', titleAttr: 'PDF' },
            {
                extend: 'print', text: '<i class="fa fa-print"></i>', titleAttr: 'Yazdır',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');
                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ])
        .withPaginationType('full')
        .withSelect(true)
        .withOption('order', [0, 'desc'])
        .withOption('lengthMenu', [3, 10, 20, 50, 100])
        .withOption('rowCallback', rowCallback)
        .withOption('responsive', true);

    $scope.dtColumnDefsPrmC = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];
    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                var rs = $filter('filter')(PrmC.PrMleri, {
                    PeriyodikMuayene_Id: aData[0]
                })[0];
                PrmC.prm = rs;
                PrmC.periyodikMuayene = PrmC.prm.PMJson;
                PrmC.mSablonu = {};
                PrmC.mSablonu = PrmC.prm.PMJson.CalismaAnalizi;
                $scope.files = [];
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'periyodikMuayene', PrmC.periyodikMuayene.PeriyodikMuayene_Id).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                });
            });
            return nRow;
        });
    }

    $scope.uploadFiles = function (dataUrl) {
        kayitKOntrol(PmSvc.prmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/periyodikMuayene/' + PrmC.periyodikMuayene.PeriyodikMuayene_Id,
                method: 'POST',
                data: {
                    file: dataUrl
                }
            }).then(function (response) {
                angular.forEach(response.data, function (data) {
                    if ($scope.files === undefined) { $scope.files = []; }
                    $scope.files.push(data);
                });
            });
        }
        else {
            $scope.dosyaList = false;
        }
    };

    $scope.deleten = function (file, index) {
        uploadService.DeleteFile(file + '/x').then(function (response) {
            if (response.data === 1) { $scope.files.splice(index, 1); }
        });
    };

    $scope.download = function (f) {
        debugger;        
        if (f.GenericName.indexOf('.') < 1) {
            //$scope.RenderFile(genericName);
            var s = serviceBase + ngAuthSettings.apiUploadService + 'down/' + f.GenericName;
            $window.open(s, '_self', '');
        } else {
            window.open(f.LocalFilePath, 'popUpWindow', 'height=1000,width=1200,left=10,top=10,,scrollbars=yes,menubar=no')
        }
    };

    $scope.oneAtATime = true;
    $scope.formatDate = function (date) {
        return date === null ? null : new Date(date);
    };
    $scope.varYok = function (value) {
        return value === true ? 'Var' : 'Yok';
    };
    $scope.evetHayir = function (value) {
        return value === true ? 'Evet' : 'Hayır';
    };
    PrmC.yakinmalar = [{ tanim: 'Balgamlı öksürük', kod: '01' }, { tanim: 'Nefes darlığı', kod: '02' }, { tanim: 'Göğüs ağrısı', kod: '03' }, { tanim: 'Çarpıntı', kod: '04' },
    { tanim: 'Sırt veya Bel Ağrısı', kod: '05' }, { tanim: 'İshal veya kabızlık', kod: '06' }, { tanim: 'Eklemlerde ağrı', kod: '07' }];
    PrmC.hastaliklar = [{ tanim: 'Kalp hastalığı', kod: '01' }, { tanim: 'Şeker hastalığı', kod: '02' }, { tanim: 'Böbrek rahatsızlığı', kod: '03' }, { tanim: 'Sarılık', kod: '04' },
    { tanim: 'Mide ve oniki parmak ülseri', kod: '05' }, { tanim: 'İşitme kaybı', kod: '06' }, { tanim: 'Görme bozukluğu', kod: '07' },
    { tanim: 'Sinir sistemi hastalığı', kod: '08' }, { tanim: 'Deri hastalığı', kod: '09' }, { tanim: 'Besin zehirlenmesi', kod: '10' }];
    PrmC.ozelSorular = [{ tanim: 'Son bir yıl içinde hastanede yattınız mı?', kod: '01' },
    { tanim: 'Son bir yıl içinde önemli bir ameliyat geçirdiniz mi?', kod: '02' }, { tanim: 'Son bir yıl içinde iş kazası geçirdiniz mi?', kod: '03' },
    { tanim: 'Son bir yıl içinde Meslek Hastalıkları Şüphesi ile ilgili tetkik veya muayaneye tabi tutuldunuz mu ?', kod: '04' },
    { tanim: 'Son bir yıl içinde Maluliyet aldınız mı?', kod: '05' }, { tanim: 'Şu anda herhangi bir tedavi görüyor musunuz?', kod: '06' }];
    PrmC.refreshBulgu = function (val) {
        var promise;
        if (!val.search) {
            promise = $q.when({});
        } else {
            promise = $http.get(serviceBase + 'api/tanim/BulguAra/' + val.search);
        }
        return promise.then(function (response) {
            var hg = [];
            angular.forEach(response.data, function (v) {
                hg.push(v.ifade === undefined ? v : v.ifade.toString().trim());
            });
            PrmC.items = hg;
        });
    };
    PrmC.FMler = [{ tanim: 'Göz', kod: 'goz' }, { tanim: 'Kulak-Burun-Boğaz', kod: 'kbb' }, { tanim: 'Deri', kod: 'deri' }, { tanim: 'Kardiyovasküler sistem', kod: 'kardiyoloji' },
    { tanim: 'Solunum sistemi', kod: 'solunum' }, { tanim: 'Sindirim sistemi', kod: 'sindirim' }, { tanim: 'Ürogenital sistem', kod: 'uroloji' },
    { tanim: 'Kas-iskelet sistemi', kod: 'ortopedi' }, { tanim: 'Nörolojik muayene', kod: 'noroloji' }, { tanim: 'Psikiyatrik muayene', kod: 'psikiatri' }, { tanim: 'Diğer', kod: 'diger' }];
    $scope.Tansiyon = function (sistol, diyastol) {
        return sistol === null || diyastol === null ? '----' : sistol + ' / ' + diyastol + ' mmHg';
    };
    $scope.Nabiz = function (nabiz) {
        return nabiz === null ? '----' : nabiz + ' /dk';
    };
    $scope.Vurgusu = function (deger, bicim) {
        return deger === null ? '----' : deger + bicim;
    };

    $scope.Hem = function (Eritrosit, Hematokrit, Hemoglobin, Lokosit, MCV, MCH, MCHC, Trombosit, Platekrit) {
        var a = Eritrosit !== null ? 'RBC:' + Eritrosit : '';
        var b = Hemoglobin !== null ? ',Hgb:' + Hemoglobin : '';
        var c = Hematokrit !== null ? ',Hct:' + Hematokrit : '';
        var d = MCV !== null ? ',MCV:' + MCV : '';
        var e = MCH !== null ? ',MCH:' + MCH : '';
        var f = MCHC !== null ? ',MCHC:' + MCHC : '';
        var g = Lokosit !== null ? ',WBC:' + Lokosit : '';
        var h = Trombosit !== null ? ',Plt:' + Trombosit : '';
        var i = Platekrit !== null ? ',Pct:' + Platekrit : '';
        return a + b + c + d + e + f + g + h + i;
    };
    $scope.$watch(function () { return angular.toJson([$scope.status]); }, function (newValue, oldValue) {
        if (newValue !== oldValue && PrmC.gitme === false) {
            PmSvc.prmb.AccordionDurumu = newValue;
        }
    });

    PrmC.gitme = false;

    $timeout(function () {
        $scope.genislik = angular.element(document.getElementById('ana')).prop('offsetWidth');
    }, 50);

    $document.on('scroll', function () {
        $scope.$apply(function () {
            if (PrmC.checkBrowser === true) { $(".fikset").css('top', Math.max(0, PrmC.acKapa === true ? 230 - $window.scrollY : 430 + ($('#Prm').height() > 1 ? $('#Prm').height() - 70 : 0) - $window.scrollY)); } else {
                $(".zop").css('top', Math.max(0, PrmC.acKapa === true ? $window.scrollY - 220 : $window.scrollY - 430));
            }
        });
    });

    $scope.$watch(function () {
        return $('#Prm').height();
    },
        function (newValue, oldValue) {
            if (PrmC.checkBrowser === true) $(".fikset").css('top', PrmC.acKapa === false ? 430 + (newValue > 1 ? newValue - 70 : 0) : 230);
            //$anchorScroll.yOffset = 700;		
        }, true);

    PrmC.acKapa = true;
    $scope.showhide = function () {
        PrmC.acKapa = !PrmC.acKapa;
        if (PrmC.checkBrowser === true) $(".fikset").css('top', PrmC.acKapa === false ? 430 + ($('#Prm').height() > 1 ? $('#Prm').height() - 70 : 0) : 230);
        var ibox = $('#cv').closest('div.ibox');
        var icon = $('#cv').find('i:first');
        var content = ibox.find('div.ibox-content');
        content.slideToggle(400);
        // Toggle icon from up to down
        icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
        ibox.toggleClass('').toggleClass('border-bottom');
        $timeout(function () {
            ibox.resize();
            ibox.find('[id^=map-]').resize();
        }, 75);
    };
    PrmC.prm = {};
    PrmC.Kayit = function () {
        Submit();
    };

    PrmC.pMuayeneSil = function () {
        if (!angular.isUndefined(PrmC.prm.PeriyodikMuayene_Id) || PrmC.prm.PeriyodikMuayene_Id !== null) {
            PmSvc.DeletePrm(PrmC.prm.PeriyodikMuayene_Id).then(function (response) {
                dataAl(PmSvc.prmb.guidId);
                PmSvc.JsonGetFile('isgumSablon').then(function (response) {
                    PrmC.mSablonu = response;
                    PrmC.periyodikMuayene = yeni;
                    PrmC.periyodikMuayene.CalismaAnalizi = response
                    PrmC.prm = {};
                });
            }).catch(function (e) {
                return notify({
                    message: e,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            }).finally(function () {
            });
        };
    };

    $scope.message = "Muayene Kaydet";
    PrmC.Renjk = false;
    PrmC.Bekle = false;

    function kayitKOntrol(veri, tanimi, sorgulaması) {
        switch (true) {
            case (sorgulaması === null):
                if (veri === null) {
                    notify({
                        message: tanimi + ' Girmeden Kayıt Yapamazsınız.',
                        classes: 'alert-danger',
                        templateUrl: $scope.inspiniaTemplate,
                        duration: 2000,
                        position: 'right'
                    });
                    throw true;
                }
                break;
            case (sorgulaması === length):
                if (!veri.length > 0) {
                    notify({
                        message: tanimi + ' Girmeden Kayıt Yapamazsınız.',
                        classes: 'alert-danger',
                        templateUrl: $scope.inspiniaTemplate,
                        duration: 2000,
                        position: 'right'
                    });
                    throw true;
                }
                break;
            case (sorgulaması === ""):
                if (veri === "") {
                    notify({
                        message: tanimi + ' Girmeden Kayıt Yapamazsınız.',
                        classes: 'alert-danger',
                        templateUrl: $scope.inspiniaTemplate,
                        duration: 2000,
                        position: 'right'
                    });
                    throw true;
                }
                break;
            case (sorgulaması === {}):
                if (veri === {}) {
                    notify({
                        message: tanimi + ' Girmeden Kayıt Yapamazsınız.',
                        classes: 'alert-danger',
                        templateUrl: $scope.inspiniaTemplate,
                        duration: 2000,
                        position: 'right'
                    });
                    return;
                }
                break;
        }
    };

    var Submit = function () {
        kayitKOntrol(PmSvc.prmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.Kod, "ISCO08 Meslek Kodlarını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calismaZamani.No, "Çalışma Zamanını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calismaPozisyonu.No, "Çalışma Pozisyonunu", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.aracKullanimi.No, "Araç Kullanımını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.fizikiOrtamSinifi, "Fiziki Ortamını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.havaSinifi, "Hava Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.gurultuSinifi, "Gürültü Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.elektrikSinifi, "Elektrik Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.tozYonetmeligiSinifi, "Toz Yönetmeliği Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.kimyasalMaddeSinifi, "Kimyasal Madde Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.biyolojikEtkenlerSinifi, "Biyolojik Etkenleri Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.isEkipmanlari, "İş Ekipmanları Sınıfını", length);

        PrmC.Bekle = true; startTimer();
        if (angular.isUndefined(PrmC.prm.PeriyodikMuayene_Id) || PrmC.prm.PeriyodikMuayene_Id === null) {
            PrmC.prm = { PMJson: JSON.stringify(PrmC.periyodikMuayene), MuayeneTuru: PrmC.periyodikMuayene.Nedeni };
            PmSvc.AddPrm(PrmC.prm, PmSvc.prmb.SaglikBirimi_Id, PmSvc.prmb.guidId).then(function (response) {
                dataAl(PmSvc.prmb.guidId);
                PrmC.selectedRow = response.PeriyodikMuayene_Id;
                PrmC.prm = response;
                PrmC.periyodikMuayene = PrmC.prm.PMJson;
                $scope.message = "Kaydediliyor!";
                PrmC.Renjk = true;
                startTimer();
            }).catch(function (e) {
                return notify({
                    message: e,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            }).finally(function () {
                PrmC.Bekle = false;

            });
        } else {
            PmSvc.UpdatePrm({
                PMJson: JSON.stringify(PrmC.periyodikMuayene), PeriyodikMuayene_Id: PrmC.prm.PeriyodikMuayene_Id, MuayeneTuru: PrmC.periyodikMuayene.Nedeni,
                RevirIslem_Id: PrmC.prm.RevirIslem_Id, Protokol: PrmC.prm.Protokol, Personel_Id: PrmC.prm.Personel_Id
            }).then(function (response) {
                dataAl(PmSvc.prmb.guidId);
                PrmC.selectedRow = response.PeriyodikMuayene_Id;
                PrmC.prm = response;
                PrmC.periyodikMuayene = JSON.parse(PrmC.prm.PMJson);
                PrmC.Muayene = JSON.parse(PrmC.prm.PMJson);
                $scope.message = "Kaydediliyor!";
                PrmC.Renjk = true;
                startTimer();
            }).catch(function (e) {
                return notify({
                    message: e.data,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            }).finally(function () {
                PrmC.Bekle = false;
            });
        }
    };

    PrmC.NewSubmit = function () {
        PmSvc.JsonGetFile('isgumSablon').then(function (response) {
            PrmC.mSablonu = response;
            PrmC.periyodikMuayene = yeni;
            PrmC.periyodikMuayene.CalismaAnalizi = response
            PrmC.prm = {};
        });
    };

    PrmC.pMuayene = function () {
        printer.print('./views/SM/View/temp/pMuayene.html', {
            data: PrmC.PRMB.data, img: PrmC.fileImgPath + PrmC.img, bolumAdi: PrmC.PRMB.bolumAdi, sirketAdi: PrmC.PRMB.sirketAdi, sirketDetayi: PrmC.PRMB.sirketDetayi,
            calismaBilgisi: PrmC.PRMB.calismaBilgisi, adres: PrmC.PRMB.adres, pM: PrmC.periyodikMuayene, MuayeneTuru: PrmC.prm.MuayeneTuru, Protokol: PrmC.prm.Protokol,
            Tarih: PrmC.prm.Tarih, saglikBirimi: $rootScope.SB.SaglikBirimiAdi, logo: $scope.picFile
        });
    };

    PrmC.pBilgisi = function () {
        printer.print('./views/SM/View/temp/yuklenici.html', {
            data: PrmC.PRMB.data, img: PrmC.fileImgPath + PrmC.img, bolumAdi: PrmC.PRMB.bolumAdi, sirketAdi: PrmC.PRMB.sirketAdi, sirketDetayi: PrmC.PRMB.sirketDetayi,
            calismaBilgisi: PrmC.PRMB.calismaBilgisi, adres: PrmC.PRMB.adres, pM: PrmC.periyodikMuayene, MuayeneTuru: PrmC.prm.MuayeneTuru, Protokol: PrmC.prm.Protokol,
            Tarih: PrmC.prm.Tarih, saglikBirimi: $rootScope.SB.SaglikBirimiAdi
        });
    };

    $scope.$watch(function () { return PrmC.prm.Protokol; }, function (newValue, oldValue) {
        if (newValue !== undefined) {
            $scope.status.open14 = true;
        } else {
            $scope.status.open14 = false;
        }
    });
    $scope.viewby = 4;
    $scope.currentPage = 1;
    $scope.itemsPerPage = $scope.viewby;
    $scope.pageStartUp = function () {
        $scope.currentPage = 1;
    };
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
    };

    $scope.selectedRow2 = -1;
    $scope.setClickedRow = function (index, mail) {
        $scope.selectedRow2 = index;
        PrmC.sendMail = mail;
    };
    PrmC.newMail = function () {
        PrmC.sendMail = {};
        $scope.selectedRow2 = -1;
    };
    PrmC.addMail = function (mail) {
        PrmC.mailListesi.push({ i: PrmC.mailListesi.length + 1, o: true, to: mail.to.trim(), ad: mail.ad.trim(), email: mail.email.trim() });
        PrmC.sendMail = {};
        $scope.selectedRow2 = -1;
        $scope.totalItems = PrmC.mailListesi.length;
    };
    PrmC.deleteMail = function (val) {
        var index = PrmC.mailListesi.map(function (item) {
            return item.i;
        }).indexOf(val.i);

        if (index !== -1) {
            PrmC.mailListesi.splice(index, 1);
        }
        PrmC.sendMail = {};
        $scope.selectedRow2 = -1;
        $scope.totalItems = PrmC.mailListesi.length;
    };
    PrmC.sendMaili = function () {
        kayitKOntrol(PmSvc.prmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        if (PrmC.mailListesi.length > 0) {
            var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
            angular.forEach(PrmC.mailListesi, function (v) {
                if (v.o === true) {
                    if (v.to === 'Kime') {
                        mail.To.push({ address: v.email, displayName: v.ad });
                    }
                    if (v.to === 'Bilgi') {
                        mail.CC.push({ address: v.email, displayName: v.ad });
                    }
                    if (v.to === 'Gizli') {
                        mail.Bcc.push({ address: v.email, displayName: v.ad });
                    }
                }
            });
            mail.SagId = PmSvc.prmb.SaglikBirimi_Id;
            var konubasi = '';
            if (!angular.isUndefined(PmSvc.prmb.prmbilgi.data.SicilNo) && PmSvc.prmb.prmbilgi.data.SicilNo !== null) {
                konubasi = PmSvc.prmb.prmbilgi.data.SicilNo.trim() + ' Sicil Nolu ';
            }
            mail.Subject = konubasi + PmSvc.prmb.prmbilgi.data.Adi.trim() + ' ' + PmSvc.prmb.prmbilgi.data.Soyadi.trim() + ' Personelin ' + PrmC.prm.MuayeneTuru.trim() + '.(ISGplus)';
            mailSvc.GetMail('./views/SM/View/temp/PrmMaili.html', {
                data: PrmC.PRMB.data, img: PrmC.fileImgPath + PrmC.img, bolumAdi: PrmC.PRMB.bolumAdi, sirketAdi: PrmC.PRMB.sirketAdi, sirketDetayi: PrmC.PRMB.sirketDetayi,
                calismaBilgisi: PrmC.PRMB.calismaBilgisi, adres: PrmC.PRMB.adres, pM: PrmC.periyodikMuayene, MuayeneTuru: PrmC.prm.MuayeneTuru, Protokol: PrmC.prm.Protokol,
                Tarih: PrmC.prm.Tarih, saglikBirimi: $rootScope.SB.SaglikBirimiAdi
            }, mail);

        }
    };
    PrmC.MailBilgisi = 'Gönder';
    $scope.$watch(function () { return $rootScope.isBeingMailed2; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            if (newValue === false) {
                PrmC.MailBilgisi = 'Gönder';
                delete $rootScope.isBeingMailed2;
                delete $rootScope.isBeingMailed;
                return notify({
                    message: $rootScope.mailGonerildiBilgisi === true ? 'Mail Gönderilmiştir.' : 'Mail Gönderilemedi sistemde sorun olabilir?.',
                    classes: $rootScope.mailGonerildiBilgisi === true ? 'alert-success' : 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 8000,
                    position: 'right'
                });
            }
            if (newValue === true) {
                PrmC.MailBilgisi = 'Gönderiliyor!';
            }
            if (newValue === undefined) {
                PrmC.MailBilgisi = 'Gönder';
            }
        }
    });

    PrmC.Ikaz = function (tumu) {
        $uibModal.open({
            templateUrl: './views/SM/View/temp/Ikaz.html',
            backdrop: true,
            animation: true,
            size: 'lg',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout, IkazSvc) {
                var startTimer2 = function (sure) {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $uibModalInstance.dismiss('cancel');
                        $scope.bilgilendirme = "";
                    }, sure);
                };
                $scope.bilgilendirme = "";
                var sn = [];
                IkazSvc.GetTumIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                    sn = s;
                    $scope.Ikazlar = tumu === false ? PrmC.PRMB.data.Ikazlar : sn;
                });
                $scope.simdi = new Date();
                $scope.open13 = function () {
                    $scope.popup13.opened = true;
                };
                $scope.popup13 = {
                    opened: false
                };
                $scope.dateOptionsmdl = {
                    formatYear: 'yy',
                    maxDate: new Date(2029, 5, 22)
                };
                $scope.dtOptionsmdl = DTOptionsBuilder.newOptions()
                    .withLanguageSource('/views/Personel/Controller/Turkish.json')
                    .withDOM('<"html5buttons"B>lTfgitp')
                    .withButtons([
                        { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
                        { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
                        { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Muayene Listesi', titleAttr: 'Excel 2010' },
                        { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Muayene Listesi', titleAttr: 'PDF' },
                        {
                            extend: 'print', text: '<i class="fa fa-print"></i>', titleAttr: 'Yazdır',
                            customize: function (win) {
                                $(win.document.body).addClass('white-bg');
                                $(win.document.body).css('font-size', '10px');
                                $(win.document.body).find('table')
                                    .addClass('compact')
                                    .css('font-size', 'inherit');
                            }
                        }
                    ])
                    .withPaginationType('full')
                    .withSelect(true)
                    .withOption('order', [0, 'desc'])
                    .withOption('lengthMenu', [3, 10, 20, 50, 100])
                    .withOption('rowCallback', rowCallback2)
                    .withOption('responsive', true);
                $scope.dtColumnDefsmdl = [// 0. kolonu gizlendi.
                    DTColumnDefBuilder.newColumnDef(0).notVisible()
                ];
                function rowCallback2(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    $('td', nRow).unbind('click');
                    $('td', nRow).bind('click', function () {
                        $scope.$apply(function () {
                            var rs = $filter('filter')($scope.Ikazlar, {
                                Ikaz_Id: aData[0]
                            })[0];
                            $scope.uyari =
                            {
                                Ikaz_Id: rs.Ikaz_Id,
                                IkazText: rs.IkazText,
                                SonucIkazText: rs.SonucIkazText,
                                MuayeneTuru: rs.MuayeneTuru,
                                SonTarih: rs.SonTarih,
                                Tarih: rs.Tarih,
                                Personel_Id: rs.Personel_Id,
                                Status: rs.Status,
                                UserId: rs.UserId
                            };
                            return nRow;
                        });

                    });
                }
                $scope.uyari =
                {
                    Ikaz_Id: null,
                    IkazText: null,
                    SonucIkazText: null,
                    MuayeneTuru: "Personel Muayenesi",
                    SonTarih: null,
                    Tarih: new Date(),
                    Personel_Id: PrmC.PRMB.data.Personel_Id,
                    Status: true,
                    UserId: null
                };
                $scope.remove = function () {
                    var sx = [];
                    if ($scope.uyari.Ikaz_Id !== null) {
                        IkazSvc.IkazSil($scope.uyari.Ikaz_Id).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Uyarınız Sistemden Kaldırıldı.Formunuz kapatılıyor.";
                        });
                    }
                };
                $scope.yeni = function () {
                    $scope.uyari =
                    {
                        Ikaz_Id: null,
                        IkazText: null,
                        SonucIkazText: null,
                        MuayeneTuru: "Periyodik Muayene",
                        SonTarih: null,
                        Tarih: new Date(),
                        Personel_Id: PrmC.PRMB.data.Personel_Id,
                        Status: true,
                        UserId: null
                    };
                };
                var sd = function (sx) {
                    $scope.Ikazlar = sx;
                    PrmC.PRMB.data.Ikazlar = sx;
                    PrmC.warning = sx.length > 0 ? true : false;
                    PrmC.warningLength = sx.length;
                    startTimer2(3000);
                };
                $scope.save = function () {
                    var sx = [];
                    if (angular.isUndefined($scope.uyari.Ikaz_Id) || $scope.uyari.Ikaz_Id === null) {
                        IkazSvc.IkazEkle($scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            var datestring = $scope.uyari.SonTarih.getDate() + "/" + ($scope.uyari.SonTarih.getMonth() + 1) + "/" + $scope.uyari.SonTarih.getFullYear();
                            $scope.bilgilendirme = datestring + " Tarihli uyarınız sisteme eklendi.Formunuz kapatılıyor.";

                        });
                    } else {
                        IkazSvc.IkazGuncelle($scope.uyari.Ikaz_Id, $scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PrmC.PRMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Güncellemeniz Yapıldı ve Eklendi.Formunuz kapatılıyor.";

                        });
                    }
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.perSMSI = function () {
                    if (isNullOrWhitespace(PrmC.PRMB.data.Telefon)) {
                        return notify({
                            message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                            classes: 'alert-danger',
                            templateUrl: $scope.inspiniaTemplate,
                            duration: 2000,
                            position: 'right'
                        });
                    }
                    if ($scope.uyari.IkazText.trim().length > 155) {
                        return notify({
                            message: '155 karakterin üstünde SMS gönderemezsiniz',
                            classes: 'alert-danger',
                            templateUrl: $scope.inspiniaTemplate,
                            duration: 2000,
                            position: 'right'
                        });
                    }
                    var sms = { KisaMesaj: $scope.uyari.IkazText.trim(), Numaralar: [PrmC.PRMB.data.Telefon.trim()] };
                    mailSvc.PostSMS(sms).then(function (resp) {
                        return notify({
                            message: resp.data.Bilgi,
                            classes: 'alert-success',
                            templateUrl: $scope.inspiniaTemplate,
                            duration: 8000,
                            position: 'middle'
                        });
                    });
                };

            }
        });
    };

    PrmC.CalismaAnaliziModeli = { CalismaAnalizi_Id: null, CAJson: null, Sirket_Id: null, Bolum_Id: null, MeslekAdi: null, Meslek_Kodu: null };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.message = "Muayene Kaydet";
            PrmC.Renjk = false;
        }, 2000);
    };


    PrmC.SablonKayit = function () {
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.Kod, "ISCO08 Meslek Kodlarını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calismaZamani.No, "Çalışma Zamanını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.calismaPozisyonu.No, "Çalışma Pozisyonunu", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.aracKullanimi.No, "Araç Kullanımını", null);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.fizikiOrtamSinifi, "Fiziki Ortamını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.havaSinifi, "Hava Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.gurultuSinifi, "Gürültü Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.elektrikSinifi, "Elektrik Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.tozYonetmeligiSinifi, "Toz Yönetmeliği Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.kimyasalMaddeSinifi, "Kimyasal Madde Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.biyolojikEtkenlerSinifi, "Biyolojik Etkenleri Sınıfını", length);
        kayitKOntrol(PrmC.periyodikMuayene.CalismaAnalizi.isEkipmanlari, "İş Ekipmanları Sınıfını", length);       
        if (angular.isUndefined(PrmC.CalismaAnaliziModeli.CalismaAnalizi_Id) || PrmC.CalismaAnaliziModeli.CalismaAnalizi_Id === null) {
            //güncel kaydı varsa varsa sil yenisini ekle
            analizSvc.BolumAra(PrmC.PRMB.bolumId, PrmC.PRMB.sirketId).then(function (response) {
                if (response !== null) {
                    analizSvc.DeleteCalismaAnalizi(response.CalismaAnalizi_Id).then(function (response) { });
                }
            });
            PrmC.prmModel = { CAJson: JSON.stringify(PrmC.disSablonu), Sirket_Id: PrmC.PRMB.sirketId, Bolum_Id: PrmC.PRMB.bolumId, MeslekAdi: PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.MeslekAdi, Meslek_Kodu: PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.Kod };
            analizSvc.AddCalismaAnalizi(PrmC.prmModel).then(function (response) {
                PrmC.disSablonu = JSON.parse(response.CAJson);
                PrmC.Meslekler = null;
                PmSvc.JsonGetFile('meslekler').then(function (response) {
                    PrmC.Meslekler = response.calisanMeslegi;
                });
                PrmC.CalismaAnaliziModeli = response;
                notify({
                    message: ' Kayıt Girilmiştir.',
                    classes: 'alert-success',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 2000,
                    position: 'right'
                });
            }).catch(function (e) {
                return notify({
                    message: e,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            }).finally(function () {

            });

        } else {
            analizSvc.UpdateCalismaAnalizi(PrmC.CalismaAnaliziModeli.CalismaAnalizi_Id,
                { CAJson: JSON.stringify(PrmC.disSablonu), Sirket_Id: PrmC.PRMB.sirketId, Bolum_Id: PrmC.PRMB.bolumId, MeslekAdi: PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.MeslekAdi, Meslek_Kodu: PrmC.periyodikMuayene.CalismaAnalizi.calisanMeslegi.Kod }
            ).then(function (response) {
                PrmC.disSablonu = JSON.parse(response.CAJson);
                PrmC.CalismaAnaliziModeli = response;
                notify({
                    message: ' Güncelleme Yapılmıştır.',
                    classes: 'alert-success',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 2000,
                    position: 'right'
                });
            }).catch(function (e) {
                return notify({
                    message: e.data,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            }).finally(function () {

            });
        }
    };

}

PrmCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams', 'TkSvc', 'SMSvc', '$cookies', 'printer', 'mailSvc',
    'ngAuthSettings', 'uploadService', 'authService', '$q', '$window', '$timeout', 'notify', '$http', 'PmSvc', '$uibModal', '$rootScope', '$location', '$anchorScroll', '$document','analizSvc'];

angular
    .module('inspinia')
    .controller('PrmCtrl', PrmCtrl);

