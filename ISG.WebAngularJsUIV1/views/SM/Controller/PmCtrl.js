angular
    .module('inspinia')
    .controller('PmCtrl', PmCtrl); 'use strict';
function PmCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams, TkSvc, SMSvc, $cookies, printer,
    ngAuthSettings, uploadService, authService, $q, $window, $timeout, notify, $http, PmSvc, $uibModal, $rootScope, mailSvc,
    personellerSvc, EreceteSvc, IkazSvc) {

    var PmC = this;
    function isNullOrWhitespace(input) {
        return !input || !input.trim();
    }
    $scope.isCollapsed = true;
    PmC.ereceteIlacAciklama = { aciklamaTuru: 2, aciklama: null };
    PmC.ereceteAciklama = { aciklamaTuru: 2, aciklama: null };
    PmC.kapnir = function () {
        $scope.isCollapsed = !$scope.isCollapsed;
    };
    var nowdate = function () {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
        var yyyy = today.getFullYear();

        return dd + '.' + mm + '.' + yyyy;
    };
    PmC.Muayene = {
        Radyoloji: [{ RadyolojikIslem: null, RadyolojikSonuc: null }],
        Ilaclar: [],
        Tanilar: [],
        Tedaviler: [],
        Erecete: {
            doktorBransKodu: null,
            doktorTcKimlikNo: null,
            doktorSertifikaKodu: null,
            protokolNo: null,
            provizyonTipi: 1,
            receteAltTuru: 1,
            receteTarihi: nowdate(),
            receteTuru: 1,
            seriNo: 1,
            takipNo: 0,
            tcKimlikNo: null,
            tesisKodu: null,
            ereceteNo: 0,
            ereceteIlacBilgisi: [],
            ereceteTaniBilgisi: [],
            ereceteAciklamaBilgisi: [{
                aciklama: null,
                aciklamaTuru: "1"
            }]
        },
        imzaliEreceteGirisReturn: null
    };

    PmC.sonuclar = [{ adi: 'İş Başı', url: './views/SM/View/temp/isBasi.html' },
    { adi: 'İstirahat', url: './views/SM/View/temp/istirahat.html' },
    { adi: 'Sevk', url: './views/SM/View/temp/sevk.html' },
    { adi: 'İşe Dönüş', url: './views/SM/View/temp/iseDonus.html' },
    { adi: 'Meslek Hastalığı', url: './views/SM/View/temp/meslekHastaligi.html' },
    { adi: 'İş Kazası', url: './views/SM/View/temp/is_Kazasi.html' },
    { adi: 'Danışma Hizmeti', url: './views/SM/View/temp/danisma.html' },
    { adi: 'Gebelik Muayenesi', url: './views/SM/View/temp/gebelikMuayenesi.html' }
    ];

    $rootScope.SB = $cookies.get('SB') === null || $cookies.get('SB') === undefined ? undefined : JSON.parse($cookies.get('SB'));

    $scope.nbv = angular.isUndefined($rootScope.SB) ?
        { SirketAdi: "Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null }
        : $rootScope.SB;

    function compareStrings(a, b) {
        a = a.toLowerCase();
        b = b.toLowerCase();
        return a < b ? -1 : a > b ? 1 : 0;
    }

    PmC.recete = function () {
        printer.print('./views/SM/View/temp/receteLandscape.html', {
            patient: {
                name: PmC.AdSoyad, date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                , Protokol: PmC.personelMuayene.Protokol, Ilaclar: PmC.Muayene.Ilaclar, tcNo: PmC.PMB.data.TcNo
            }
        });
    };

    var degeri = function (val, prop) {
        switch (true) {//true araası yapyor.
            case val === 'Hastalık' && prop === 'hastalik': return 'X'; break;
            case val === 'İş Kazası' && prop === 'isKazasi': return 'X'; break;
            case val === 'Meslek Hastalığı' && prop === 'meslekHastaligi': return 'X'; break;
            case val === 'Analık' && prop === 'analik': return 'X'; break;
            default: return ' ';
        }
    };

    PmC.rapor = function () {
        var bitistarihi = new Date(PmC.Muayene.Istirahat.BitisTarihi);
        bitistarihi.setDate(bitistarihi.getDate() - 1);
        printer.print('./views/SM/View/temp/isGoremezlik.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, adi: PmC.PMB.data.Adi, soyadi: PmC.PMB.data.Soyadi,
                sgkNo: PmC.PMB.data.SgkNo === undefined ? '' : PmC.PMB.data.SgkNo, adres: PmC.PMB.data.Adresler[0].GenelAdresBilgisi === undefined ? '' : PmC.PMB.data.Adresler[0].GenelAdresBilgisi,
                tel: PmC.PMB.data.Telefon === undefined ? '' : PmC.PMB.data.Telefon, isKazasi: degeri(PmC.Muayene.Istirahat.Nedeni, 'isKazasi'),
                meslekHastaligi: degeri(PmC.Muayene.Istirahat.Nedeni, 'meslekHastaligi'),
                hastalik: degeri(PmC.Muayene.Istirahat.Nedeni, 'hastalik'), analik: degeri(PmC.Muayene.Istirahat.Nedeni, 'analik'),
                Istirahat: { GunSayisi: PmC.Muayene.Istirahat.GunSayisi, BaslamaTarihi: new Date(PmC.Muayene.Istirahat.BaslamaTarihi).toLocaleDateString(), BitisTarihi: bitistarihi.toLocaleDateString(), Sonrasi: PmC.Muayene.Istirahat.Sonrasi, iseBaslamaTarihi: new Date(PmC.Muayene.Istirahat.BitisTarihi).toLocaleDateString() }
            }
        });
    };

    PmC.sporYapabilirRaporu = function () {
        printer.print('./views/SM/View/temp/ek5.html', {
            p: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, adi: PmC.PMB.data.Adi, soyadi: PmC.PMB.data.Soyadi,
                sgkNo: PmC.PMB.data.SgkNo === undefined ? '' : PmC.PMB.data.SgkNo, adres: PmC.PMB.data.Adresler[0].GenelAdresBilgisi === undefined ? '' : PmC.PMB.data.Adresler[0].GenelAdresBilgisi,
                tel: PmC.PMB.data.Telefon === undefined ? '' : PmC.PMB.data.Telefon, data: PmC.PMB.data,
                cinsiyet: PmC.PMB.data.PersonelDetayi.Cinsiyet === true ? "Erkek" : "Kadın"

            }
        });
    };


    PmC.sevkRaporu = function () {
        printer.print('./views/SM/View/temp/sevkRaporu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: personellerSvc.MoviesIds.SirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar),
                Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, sure: hesaplaIlkIseBaslamaZamani(PmC.PMB.data.PersonelDetayi.IlkIseBaslamaTarihi),
                tel: PmC.PMB.data.Telefon === undefined ? '' : PmC.PMB.data.Telefon, sikayetleri: PmC.Muayene.Sikayetler, bulgular: PmC.Muayene.Bulgulari,
                name: PmC.AdSoyad, bolumu: personellerSvc.MoviesIds.BolumAdi, gorevi: PmC.PMB.data.Gorevi, ilaclar: PmC.Ilaclar(PmC.Muayene.Ilaclar), Sevk: PmC.Muayene.Sevk
            }
        });
    };

    PmC.konsRaporu=function() {
        printer.print('./views/SM/View/temp/ishSevkDetayliFormu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: personellerSvc.MoviesIds.SirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar),
                Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, sure: hesaplaIlkIseBaslamaZamani(PmC.PMB.data.PersonelDetayi.IlkIseBaslamaTarihi),
                tel: PmC.PMB.data.Telefon === undefined ? '' : PmC.PMB.data.Telefon, sikayetleri: PmC.Muayene.Sikayetler, bulgular: PmC.Muayene.Bulgulari,
                name: PmC.AdSoyad, bolumu: personellerSvc.MoviesIds.BolumAdi, gorevi: PmC.PMB.data.Gorevi, ilaclar: PmC.Ilaclar(PmC.Muayene.Ilaclar), Sevk: PmC.Muayene.Sevk
            }
        });
    };

    function hesaplaIlkIseBaslamaZamani(ilkIseBaslamaTarihi) {
        var ilkTarih = new Date(ilkIseBaslamaTarihi);
        var simdikiTarih = new Date();
        var yilFarki = simdikiTarih.getFullYear() - ilkTarih.getFullYear();
        var ayFarki = simdikiTarih.getMonth() - ilkTarih.getMonth();
        if (ayFarki < 0) {
            yilFarki--;
            ayFarki += 12;
        }
        return {
            yil: yilFarki,
            ay: ayFarki
        };
    }


    PmC.isKazasiRaporu = function () {
        printer.print('./views/SM/View/temp/isKazasiRaporu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar), Istirahat: PmC.Muayene.Istirahat
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, name: PmC.AdSoyad, Ikbilgisi: PmC.Muayene.Ikbilgisi, tel: PmC.PMB.data.Telefon
            }
        });
    };


    PmC.surucuBelgesi = function () {
        printer.print('./views/SM/View/temp/surucuBelgesi.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, img: PmC.fileImgPath + PmC.img
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, name: PmC.AdSoyad, data: PmC.PMB.data
            }
        });
    };

    PmC.iseDonusRaporu = function () {
        printer.print('./views/SM/View/temp/iseDonusRaporu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo,
                name: PmC.AdSoyad, iseDonus: PmC.Muayene.iseDonus
            }
        });
    };

    PmC.GebelikRaporu = function () {
        printer.print('./views/SM/View/temp/gebelikRaporu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar), Istirahat: PmC.Muayene.Istirahat
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, name: PmC.AdSoyad, GebelikMuayenesi: PmC.Muayene.GebelikMuayenesi
            }
        });
    };

    PmC.mHastalikRaporu = function () {
        printer.print('./views/SM/View/temp/mHastalikRaporu.html', {
            patient: {
                date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo,
                name: PmC.AdSoyad, mHastalik: PmC.Muayene.mHastalik
            }
        });
    };


    PmC.AnaBilgiVerileri = function () {
        authService.userView(authService.authentication.id).then(function (data) {//birinci tamamladıkta sonra
            PmC.val = data;
            PmC.Muayene.Erecete.doktorBransKodu = PmC.val.doktorBransKodu;
            PmC.Muayene.Erecete.doktorTcKimlikNo = PmC.val.TcNo;
            PmC.Muayene.Erecete.doktorSertifikaKodu = PmC.val.doktorSertifikaKodu;
            PmC.Muayene.Erecete.tesisKodu = PmC.val.doktorTesisKodu;
            PmC.MedullaPassw = PmC.val.MedullaPassw;
            PmC.key = PmC.val.key;
        });
    };

    var dataAl = function (val) {
        PmSvc.GetPmPersonel(val).then(function (s) {
            PmSvc.pmb.PmBilgi = s;
            PmSvc.pmb.guidId = val;
            PmC.fileImgPath = ngAuthSettings.isAzure ? ngAuthSettings.storageLinkService + 'personel/' : ngAuthSettings.storageLinkService + uploadFolder;
            PmC.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
            PmC.PMB = s;
            PmC.AdSoyad = s.data.Adi.trim() + ' ' + s.data.Soyadi.trim();
            PmC.guid = val;
            PmC.PMleri = s.data.PersonelMuayeneleri;
            if (s.data.PersonelDetayi.Cinsiyet) { PmC.sonuclar.splice(7, 1); }
            PmC.warning = s.data.Ikazlar.length > 0 ? true : false;
            PmC.warningLength = s.data.Ikazlar.length;
            personellerSvc.MoviesIds.BolumId = s.bolumId;
            personellerSvc.MoviesIds.SirketId = s.sirketId;
            personellerSvc.MoviesIds.SirketAdi = s.sirketAdi;
            personellerSvc.MoviesIds.BolumAdi = s.bolumAdi;
            PmC.Muayene.Erecete.tcKimlikNo = PmC.PMB.data.TcNo;
            PmC.Muayene.Erecete.ereceteAciklamaBilgisi[0].aciklama = personellerSvc.MoviesIds.SirketAdi + " personeli olarak çalışmaktadır.";
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

    if (!angular.isUndefined($stateParams.id)) {
        dataAl($stateParams.id);
        PmC.AnaBilgiVerileri();
        SMSvc.SBStokListeleri(PmSvc.pmb.SaglikBirimi_Id, true).then(function (data) {
            $scope.IlacStoklari = [];
            angular.forEach(data, function (v) {
                if (v.StokTuru !== 'Demirbaş Malz.') { $scope.IlacStoklari.push(v); }
            });
        });
    }

    $scope.$watch(function () { return $scope.nbv; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue.SaglikBirimi_Id) || newValue.SaglikBirimi_Id !== null) {
            PmSvc.pmb.SaglikBirimi_Id = newValue.SaglikBirimi_Id;
            $rootScope.SB = newValue;
            var zaman = new Date();
            zaman.setTime(zaman.getTime() + 2 * 60 * 60 * 1000);//son kullanımdan 4 saat sonra iptal olacak
            $cookies.put('SB', JSON.stringify(newValue), { expires: zaman.toString() });
            SMSvc.SBStokListeleri(newValue.SaglikBirimi_Id, true).then(function (data) {
                var sd = [];
                angular.forEach(data, function (v) {
                    if (v.StokTuru !== 'Demirbaş Malz.') { sd.push(v); }
                });
                PmC.IlacSarfCikisi = '';
                $scope.IlacStoklari = sd.sort(function (a, b) {
                    return compareStrings(a.IlacAdi, b.IlacAdi);
                });
                angular.forEach(PmC.Muayene.Tedaviler, function (v) {
                    var index = $scope.IlacStoklari.map(function (item) {
                        return item.StokId;
                    }).indexOf(v.StokId);
                    if (index !== -1) {
                        $scope.IlacStoklari.splice(index, 1);
                    }
                });
            });
        }
    });


    PmC.personelMuayene = {};

    PmC.acKapa = true;

    $scope.inspiniaTemplate = 'views/common/notify.html';

    PmC.Tanilar = function (val) {
        var hg = [];
        angular.forEach(val, function (v) {
            hg.push(v.ad.trim());
        });
        return hg.toString();
    };

    PmC.Ilaclar = function (val) {
        var hg = [];
        angular.forEach(val, function (v) {
            hg.push(v.IlacAdi.trim());
        });
        return val.length > 0 ? hg.toString() : 'REÇETE VERİLMEDİ.!';
    };

    PmC.Sonuc = function (val) {
        if (!angular.isUndefined(val.Ikbilgisi)) {
            return 'İş Kazası > ' + val.Ikbilgisi.yaralanma.kazadanSonra.name;
        }
        if (!angular.isUndefined(val.GebelikMuayenesi)) return 'Gebelik Muayenesi';
        if (!angular.isUndefined(val.Danisma)) return 'Danışma Hizmeti';
        if (!angular.isUndefined(val.Isbasi)) return 'İş Başı';
        if (!angular.isUndefined(val.Istirahat)) return 'İstirahat (' + val.Istirahat.GunSayisi + ') Gün';
        if (!angular.isUndefined(val.Sevk)) return 'Sevk > ' + val.Sevk.bolumu;
        if (!angular.isUndefined(val.iseDonus)) return 'İşe Dönüş';
        if (!angular.isUndefined(val.mHastalik)) return 'Meslek Hastalığı';
        return null;
    };

    PmC.dateNow = new Date();

    PmC.addDay = function (date, day) {
        var datex = new Date(date);
        datex.setUTCDate(datex.getUTCDate() + day);
        return datex;
    };

    PmSvc.Bolumler().then(function (response) {
        var hg = [];
        angular.forEach(response, function (v) {
            hg.push(v.ifade === undefined ? v : v.ifade.toString().trim());
        });
        PmC.HastaneBolumleri = hg;
    });

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $scope.oneAtATime = true;

    $scope.formatDate = function (date) {
        return date === null ? null : new Date(date);
    };

    $scope.dtOptionsPmC = DTOptionsBuilder.newOptions()
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
        .withOption('rowCallback', rowCallback)
        .withOption('responsive', true);

    $scope.dtColumnDefsPmC = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];

    function hareketler(val) {
        $scope.oneAtATime = false;
        if (!angular.isUndefined(val.Sikayetler) && val.Sikayetler.length > 0) { $scope.status.open = true; } else { $scope.status.open = false; }
        if (!angular.isUndefined(val.Bulgulari) && val.Bulgulari.length > 0) { $scope.status.open2 = true; } else { $scope.status.open2 = false; }
        if (!angular.isUndefined(val.EKG.Sonuc)) { $scope.status.open5 = true; } else { $scope.status.open5 = false; }
        if (!angular.isUndefined(val.ANT.TASolKolSistol)) { $scope.status.open3 = true; } else { $scope.status.open3 = false; }
        if (!angular.isUndefined(val.Lab) || !angular.isUndefined(val.LabHemogram)) { $scope.status.open4 = true; } else { $scope.status.open4 = false; }
        if (!angular.isUndefined(val.Radyoloji) && val.Radyoloji.length > 0) { $scope.status.open6 = true; } else { $scope.status.open6 = false; }
        if (!angular.isUndefined(val.Danisma)) { $scope.status.open8 = true; } else { $scope.status.open8 = false; }
        if (!angular.isUndefined(val.MailBilgisi)) { $scope.status.open9 = true; } else { $scope.status.open9 = false; }

    }

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                //PmC.acKapa = !PmC.acKapa;
                var rs = $filter('filter')(PmC.PMleri, {
                    PersonelMuayene_Id: aData[0]
                })[0];
                $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false };
                PmC.personelMuayene = rs;
                PmC.Muayene = PmC.personelMuayene.PMJson;
                if (!PmC.Muayene.Radyoloji.length > 0) { PmC.Muayene.Radyoloji = [{ RadyolojikIslem: null, RadyolojikSonuc: null }]; }
                if (PmC.Muayene.hasOwnProperty('Ikbilgisi')) {
                    $scope.sonuc = PmC.sonuclar[5];
                } else
                    if (PmC.Muayene.hasOwnProperty('GebelikMuayenesi')) {
                        $scope.sonuc = PmC.sonuclar[7];
                    } else
                        if (PmC.Muayene.hasOwnProperty('Danisma')) {
                            $scope.sonuc = PmC.sonuclar[6];
                            $scope.status.open8 = true;
                        } else
                            if (PmC.Muayene.hasOwnProperty('Isbasi')) {
                                $scope.sonuc = PmC.sonuclar[0];
                            } else
                                if (PmC.Muayene.hasOwnProperty('Sevk')) {
                                    $scope.sonuc = PmC.sonuclar[2];
                                } else
                                    if (PmC.Muayene.hasOwnProperty('iseDonus')) {
                                        $scope.sonuc = PmC.sonuclar[3];
                                    } else
                                        if (PmC.Muayene.hasOwnProperty('mHastalik')) {
                                            $scope.sonuc = PmC.sonuclar[4];
                                        } else
                                            if (PmC.Muayene.hasOwnProperty('Istirahat')) {
                                                $scope.sonuc = PmC.sonuclar[1];
                                            } else { $scope.sonuc = null; }
                if (PmC.Muayene.hasOwnProperty('Tedaviler') && PmC.Muayene.Tedaviler.length > 0) {
                    $('#stok').removeClass('collapsed');
                } else {
                    $('#stok').addClass('collapsed');
                }
                if (PmC.Muayene.hasOwnProperty('Ikbilgisi')) {
                    $('#ilaclar').removeClass('collapsed');
                } else {
                    $('#ilaclar').addClass('collapsed');
                }
                PmC.stokFilter();
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'personel-muayene', PmC.personelMuayene.PersonelMuayene_Id).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                    if ($scope.files.length > 0)
                        $scope.status.open7 = true;
                });
                hareketler(PmC.Muayene);
            });
            return nRow;
        });
    };

    PmC.refreshSikayet = function (val) {
        var promise;
        if (!val.search) {
            promise = $q.when({});
        } else {
            promise = $http.get(serviceBase + 'api/tanim/SikayetAra/' + val.search);
        }
        return promise.then(function (response) {
            var hg = [];
            angular.forEach(response.data, function (v) {
                hg.push(v.ifade === undefined ? v : v.ifade.toString().trim());
            });
            PmC.items = hg;
        });
    };

    PmC.refreshBulgu = function (val) {
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
            PmC.items = hg;
        });
    };



    $scope.$watch('[PmC.Muayene.Lab,PmC.personelMuayene.PersonelMuayene_Id]', function (newVal, oldVal) {
        if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
            var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
            var oldVal0Len = oldVal[0] === undefined ? 0 : oldVal[0].length;
            if (newVal0Len > oldVal0Len && newVal[1] === oldVal[1]) {
                var tetkikler = [];
                angular.forEach(newVal[0], function (v) {
                    tetkikler.push(v.toString().replace(',', '.'));
                });
                var last = newVal[0][newVal[0].length - 1];
                $scope.deger = { tetkik: last, giris: null };
                $uibModal.open({
                    templateUrl: 'degerKaydi.html',
                    backdrop: true,
                    animation: true,
                    size: 'sm',
                    windowClass: "animated flipInY",
                    controller: function ($scope, $uibModalInstance, $log, deger) {
                        $scope.deger = deger;
                        $scope.submittenx = function () {
                            tetkikler[tetkikler.length - 1] = last + (deger.giris === null ? "" : " ( Sonuç : " + deger.giris + " ) ");
                            PmC.Muayene.Lab = tetkikler;
                            $uibModalInstance.dismiss('cancel');
                        };
                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                    },
                    resolve: {
                        deger: function () {
                            return $scope.deger;
                        }
                    }
                });
            }
        }
    }, true);

    PmC.hmgPrms = [{ prt: 'Eritrosit', sc: null, kst: 'RBC (Red Blood Cell, kırmızı kan hücresi, eritrosit)' },
    { prt: 'Hematokrit', sc: null, kst: 'Hct (Hematokrit; PCV: Packed Cell Volume)' }, { prt: 'Hemoglobin', sc: null, kst: 'Hgb (Hemoglobin)' },
    { prt: 'MCV', sc: null, kst: 'MCV (Mean Corpuscular Volume)' },
    { prt: 'MCH', sc: null, kst: '	MCH (Mean Corpuscular Hemoglobin)' }, { prt: 'MCHC', sc: null, kst: 'MCHC (Mean Corpuscular Hemoglobin Concentration)' },
    { prt: 'RDW', sc: null, kst: 'RDW (Red cell Distribution Width, Eritrosit dağılım genişliği)' },
    { prt: 'Lokosit', sc: null, kst: 'WBC (White Blood Cell, beyaz kan hücresi, lökosit)' },
    { prt: 'Lenfosit_Yuzde', sc: null, kst: 'Lym % ve # (Lenfosit % ve sayı)' },
    { prt: 'Monosit_Yuzde', sc: null, kst: 'Mono %  ve # (Monosit % ve sayı)' },
    { prt: 'Granülosit_Yuzde', sc: null, kst: 'Gran % ve # (Granülosit % ve sayı)' },
    { prt: 'Notrofil_Yuzde', sc: null, kst: '' }, { prt: 'Eoznofil_Yuzde', sc: null, kst: '' }, { prt: 'Bazofil_Yuzde', sc: null, kst: '' }, { prt: 'Trombosit', sc: null, kst: 'Plt (Trombosit)' },
    { prt: 'MeanPlateletVolume', sc: null, kst: '	MPV (Mean Platelet Volume)' },
    { prt: 'Platekrit', sc: null, kst: 'Pct (Platekrit)' },
    { prt: 'PDW', sc: null, kst: 'PDW (Platelet Distribution Width, Trombosit dağılım genişliği)' }];

    $scope.$watch('[PmC.Muayene.LabHemogram,PmC.personelMuayene.PersonelMuayene_Id]', function (newVal, oldVal) {
        if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
            var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
            var oldVal0Len = oldVal[0] === undefined ? 0 : oldVal[0].length;
            if (newVal0Len > oldVal0Len && newVal[1] === oldVal[1]) {
                var hemograms = [];
                angular.forEach(newVal[0], function (v) {
                    var a = v.prt.toString().split(':');
                    hemograms.push({ prt: a[1] === undefined ? a[0] : a[0] + ':' + a[1], key: a[0], value: a[1] });
                });
                var last = hemograms[hemograms.length - 1].prt;
                $scope.sonuc = { prt: last, giris: null };
                $uibModal.open({
                    templateUrl: 'sonucKaydi.html',
                    backdrop: true,
                    animation: true,
                    size: 'sm',
                    windowClass: "animated flipInY",
                    controller: function ($scope, $uibModalInstance, $log, sonuc) {
                        $scope.sonuc = sonuc;
                        $scope.submittenx = function () {
                            hemograms[hemograms.length - 1].prt = last + (sonuc.giris === null ? "" : ":" + sonuc.giris);
                            hemograms[hemograms.length - 1].key = last;
                            hemograms[hemograms.length - 1].value = sonuc.giris === null ? null : sonuc.giris;
                            PmC.Muayene.LabHemogram = hemograms;
                            $uibModalInstance.dismiss('cancel');
                        };
                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                    },
                    resolve: {
                        sonuc: function () {
                            return $scope.sonuc;
                        }
                    }
                });
            }
        }
    }, true);

    TkSvc.RadyolojiTanimlari().then(function (response) {
        PmC.RadyolojikIslemler = response;
    });


    //PmSvc.JsonGetFile('radyoloji').then(function (resp) {
    //    PmC.RadyolojikIslemler = resp;
    //});

    PmC.addRad = function () {
        PmC.Muayene.Radyoloji.push({ RadyolojikIslem: null, RadyolojikSonuc: null });
    };

    PmC.deleteRad = function (item) {
        var index = PmC.Muayene.Radyoloji.indexOf(item);
        if (index !== -1) {
            PmC.Muayene.Radyoloji.splice(index, 1);
        }
    };

    PmC.getIlac = function (val) {
        var deferred = $q.defer();
        PmSvc.Ilaclar(val).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };

    PmC.KubIlac = function (val) {
        PmSvc.GetKubKtAra(val).then(function (response) {
            var regex = /https:(.*?)\pdf/;
            var strToMatch = response.DocumentPathKub;
            var matched = regex.exec(strToMatch)[0];
            window.open(matched, '_blank');
        });
    };

    PmC.KtIlac = function (val) {
        PmSvc.GetKubKtAra(val).then(function (response) {
            var regex = /https:(.*?)\pdf/;
            var strToMatch = response.DocumentPathKt;
            var matched = regex.exec(strToMatch)[0];
            window.open(matched, '_blank');
        });
    };

    PmC.onSelect = function ($item, $model, $label) {
        PmC.Ilacx.barkod = $item.IlacBarkodu.trim();
    };
    PmC.IlacAciklamaListesi = [];

    PmC.aciklamaEkle = function (val) {
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu E-Reçeteye " + val.aciklama + " eklemek istiyormusunuz?";

                    $scope.sil = function () {
                        $scope.bekle = true;
                        var AciklamaDatasi =
                        {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField,
                            ereceteAciklamaBilgisi: [{ aciklama: val.aciklama, aciklamaTuru: val.aciklamaTuru }]
                        };
                        EreceteSvc.AciklamaEkleme(AciklamaDatasi, PmC.MedullaPassw, PmC.key).then(function (valx) {
                            if (valx.imzaliEreceteAciklamaEkleReturn.sonucKoduField === "0000") {
                                PmC.Muayene.Erecete.ereceteAciklamaBilgisi.push(val);
                                PmC.ereceteAciklama = { aciklamaTuru: 2, aciklama: null };
                                if (!angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = valx.imzaliEreceteAciklamaEkleReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (val.aciklama !== null) {
                PmC.Muayene.Erecete.ereceteAciklamaBilgisi.push(val);
                PmC.ereceteAciklama = { aciklamaTuru: 2, aciklama: null };
                if (!angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) PmC.KayitGuncellemeri();
            }
        }
    };

    PmC.Ilacx = { ereceteIlacAciklamaListesi: [] };

    PmC.IlaciSec = function (val) {
        PmC.Ilacx = val;
        PmC.IlacAciklamaListesi = PmC.Ilacx.ereceteIlacAciklamaListesi;
        if (PmC.Ilacx.ereceteIlacAciklamaListesi.length > 0) { $scope.ilAc = true; } else { $scope.ilAc = false; }
    };

    PmC.ekleIlac = function () {
        var index = PmC.Muayene.Ilaclar.map(function (item) {
            return item.IlacAdi;
        }).indexOf(PmC.Ilacx.IlacAdi);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            var lk = PmC.Ilacx.IlacAdi;
            ilaclariEreceteyeEkle('Ekle', -1, lk);
        } else {
            if (!angular.isUndefined(PmC.Ilacx.IlacAdi) && index === -1) {
                if (angular.isUndefined(PmC.Ilacx.$$hashKey)) {
                    PmC.Ilacx.ereceteIlacAciklamaListesi = PmC.IlacAciklamaListesi;
                    PmC.Muayene.Ilaclar.push(PmC.Ilacx);
                    PmC.Say = PmC.Muayene.Ilaclar.length;
                }
                angular.element('#ilAdi').trigger('focus');
            }
            PmC.IlacAciklamaListesi = [];
            PmC.ereceteIlacAciklama = { aciklamaTuru: 2, aciklama: null };
            PmC.Ilacx = { GunSayisi: 1, KullanimSayisi: 1, KutuAdeti: 1, kullanimPeriyot: 1, kullanimPeriyotBirimi: 3, kullanimSekli: 1, ereceteIlacAciklamaListesi: [] };
            ilaclariEreceteyeEkle('Ekle', -1, lk);
            PmC.KayitGuncellemeri();
        }
    };

    var ilaclariEreceteyeEkle = function (durum, index, adi) {
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    var fg = durum === "Kaldir" ? "Silmek" : "Eklemek";
                    $scope.bilgi = "Barkodlu E-Reçeteyi " + adi + " Adlı İlaçı " + fg + " İçin mi, İptal Etmek İstiyormusunuz?";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var SilDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField
                        };
                        EreceteSvc.ReceteSil(SilDatasi, PmC.MedullaPassw, PmC.key).then(function (val) {
                            if (val.imzaliEreceteSilReturn.sonucKoduField === "0000") {
                                PmC.Muayene.imzaliEreceteGirisReturn = null;
                                PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                                angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                                    PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                                        adet: v.KutuAdeti,
                                        barkod: v.barkod,
                                        ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                                        kullanimDoz1: v.GunSayisi,
                                        kullanimDoz2: v.KullanimSayisi,
                                        kullanimPeriyot: v.kullanimPeriyot,//bir günde
                                        kullanimSekli: v.kullanimSekli
                                    });
                                });
                                if (durum === 'Kaldir') {
                                    if (index !== -1) {
                                        PmC.Muayene.Ilaclar.splice(index, 1);
                                    }
                                    PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                                    angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                                        PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                                            adet: v.KutuAdeti,
                                            barkod: v.barkod,
                                            ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                                            kullanimDoz1: v.GunSayisi,
                                            kullanimDoz2: v.KullanimSayisi,
                                            kullanimPeriyot: v.kullanimPeriyot,//bir günde
                                            kullanimSekli: v.kullanimSekli
                                        });
                                    });
                                }
                                if (durum === 'Ekle') {
                                    if (!angular.isUndefined(PmC.Ilacx.IlacAdi) && index === -1) {
                                        if (angular.isUndefined(PmC.Ilacx.$$hashKey)) {
                                            PmC.Ilacx.ereceteIlacAciklamaListesi = PmC.IlacAciklamaListesi;
                                            PmC.Muayene.Ilaclar.push(PmC.Ilacx);
                                            PmC.Say = PmC.Muayene.Ilaclar.length;
                                        }
                                        angular.element('#ilAdi').trigger('focus');
                                    }
                                    PmC.IlacAciklamaListesi = [];
                                    PmC.ereceteIlacAciklama = { aciklamaTuru: 2, aciklama: null };
                                    PmC.Ilacx = { GunSayisi: 1, KullanimSayisi: 1, KutuAdeti: 1, kullanimPeriyot: 1, kullanimPeriyotBirimi: 3, kullanimSekli: 1, ereceteIlacAciklamaListesi: [] };
                                    PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                                    angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                                        PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                                            adet: v.KutuAdeti,
                                            barkod: v.barkod,
                                            ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                                            kullanimDoz1: v.GunSayisi,
                                            kullanimDoz2: v.KullanimSayisi,
                                            kullanimPeriyot: v.kullanimPeriyot,//bir günde
                                            kullanimSekli: v.kullanimSekli
                                        });
                                    });

                                }
                                PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = val.imzaliEreceteSilReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
            angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                    adet: v.KutuAdeti,
                    barkod: v.barkod,
                    ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                    kullanimDoz1: v.GunSayisi,
                    kullanimDoz2: v.KullanimSayisi,
                    kullanimPeriyot: v.kullanimPeriyot,//bir günde
                    kullanimSekli: v.kullanimSekli
                });
            });
            PmC.KayitGuncellemeri();
        }
    };

    PmC.deleteIlac = function (val) {
        var index = PmC.Muayene.Ilaclar.indexOf(val);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            ilaclariEreceteyeEkle('Kaldir', index, val.IlacAdi);
            PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
            angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                    adet: v.KutuAdeti,
                    barkod: v.barkod,
                    ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                    kullanimDoz1: v.GunSayisi,
                    kullanimDoz2: v.KullanimSayisi,
                    kullanimPeriyot: v.kullanimPeriyot,//bir günde
                    kullanimSekli: v.kullanimSekli
                });
            });
        } else {
            if (index !== -1) {
                PmC.Muayene.Ilaclar.splice(index, 1);
            }
            PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
            angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                    adet: v.KutuAdeti,
                    barkod: v.barkod,
                    ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                    kullanimDoz1: v.GunSayisi,
                    kullanimDoz2: v.KullanimSayisi,
                    kullanimPeriyot: v.kullanimPeriyot,//bir günde
                    kullanimSekli: v.kullanimSekli
                });
            });
            PmC.KayitGuncellemeri();
        }
        PmC.Say = PmC.Muayene.Ilaclar.length;
    };

    PmC.ilacAciklamaEkle = function (val) {
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu E-Reçeteye '" + val.aciklama + "' açıklamasını eklemek istiyormusunuz?";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var IlacAciklamaDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField,
                            barkod: PmC.Ilacx.barkod,
                            ereceteIlacAciklamaBilgisi: [{
                                aciklama: val.aciklama,
                                aciklamaTuru: val.aciklamaTuru
                            }]
                        };
                        EreceteSvc.IlacAciklamaEkleme(IlacAciklamaDatasi, PmC.MedullaPassw, PmC.key).then(function (valx) {
                            if (valx.imzaliEreceteIlacAciklamaEkleReturn.sonucKoduField === "0000") {
                                if (val.aciklama !== null) {
                                    PmC.IlacAciklamaListesi.push(val);
                                    PmC.IlacAciklamaListesi = [];
                                    PmC.ereceteIlacAciklama = { aciklamaTuru: 2, aciklama: null };
                                    PmC.Ilacx = { GunSayisi: 1, KullanimSayisi: 1, KutuAdeti: 1, kullanimPeriyot: 1, kullanimPeriyotBirimi: 3, kullanimSekli: 1, ereceteIlacAciklamaListesi: [] };
                                }
                                PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                                angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                                    PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                                        adet: v.KutuAdeti,
                                        barkod: v.barkod,
                                        ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                                        kullanimDoz1: v.GunSayisi,
                                        kullanimDoz2: v.KullanimSayisi,
                                        kullanimPeriyot: v.kullanimPeriyot,//bir günde
                                        kullanimSekli: v.kullanimSekli
                                    });
                                });

                                PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = valx.imzaliEreceteIlacAciklamaEkleReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (val.aciklama !== null) {
                PmC.IlacAciklamaListesi.push(val);
                PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                    PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                        adet: v.KutuAdeti,
                        barkod: v.barkod,
                        ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                        kullanimDoz1: v.GunSayisi,
                        kullanimDoz2: v.KullanimSayisi,
                        kullanimPeriyot: v.kullanimPeriyot,//bir günde
                        kullanimSekli: v.kullanimSekli
                    });
                });
                PmC.ereceteIlacAciklama = { aciklamaTuru: 2, aciklama: null };
            }
            PmC.KayitGuncellemeri();
        }
    };

    PmC.deleteIlacAciklama = function (val) {
        var index = PmC.IlacAciklamaListesi.indexOf(val);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu Reçetenin İlaç Açıklamasını Kaldırmak İçin, Silmek İstiyormusunuz?";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var SilDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField
                        };
                        EreceteSvc.ReceteSil(SilDatasi, PmC.MedullaPassw, PmC.key).then(function (val) {
                            if (val.imzaliEreceteSilReturn.sonucKoduField === "0000") {
                                PmC.Muayene.imzaliEreceteGirisReturn = null;
                                if (index !== -1) {
                                    PmC.IlacAciklamaListesi.splice(index, 1);
                                }
                                PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
                                angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                                    PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                                        adet: v.KutuAdeti,
                                        barkod: v.barkod,
                                        ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                                        kullanimDoz1: v.GunSayisi,
                                        kullanimDoz2: v.KullanimSayisi,
                                        kullanimPeriyot: v.kullanimPeriyot,//bir günde
                                        kullanimSekli: v.kullanimSekli
                                    });
                                });
                                PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = val.imzaliEreceteSilReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (index !== -1) {
                PmC.IlacAciklamaListesi.splice(index, 1);
            }
            PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
            angular.forEach(PmC.Muayene.Ilaclar, function (v) {
                PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                    adet: v.KutuAdeti,
                    barkod: v.barkod,
                    ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                    kullanimDoz1: v.GunSayisi,
                    kullanimDoz2: v.KullanimSayisi,
                    kullanimPeriyot: v.kullanimPeriyot,//bir günde
                    kullanimSekli: v.kullanimSekli
                });
            });
            PmC.KayitGuncellemeri();
        }
    };

    PmC.deleteAciklama = function (val) {
        var index = PmC.Muayene.Erecete.ereceteAciklamaBilgisi.indexOf(val);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu Reçetenin Açıklamasını Kaldırmak İçin, Silmek İstiyormusunuz?";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var SilDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField
                        };
                        EreceteSvc.ReceteSil(SilDatasi, PmC.MedullaPassw, PmC.key).then(function (val) {
                            if (val.imzaliEreceteSilReturn.sonucKoduField === "0000") {
                                PmC.Muayene.imzaliEreceteGirisReturn = null;
                                if (index !== -1) {
                                    PmC.Muayene.Erecete.ereceteAciklamaBilgisi.splice(index, 1);
                                }
                                PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = val.imzaliEreceteSilReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (index !== -1) {
                PmC.Muayene.Erecete.ereceteAciklamaBilgisi.splice(index, 1);
            }
            PmC.KayitGuncellemeri();
        }
    };

    PmC.HastalikTanimiList = function (value) {
        var deferred = $q.defer();
        SMSvc.HastalikAra(value).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };

    PmC.ekleTani = function (val) {
        var index = PmC.Muayene.Tanilar.map(function (item) {
            return item.ad;
        }).indexOf(val);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu E-Reçeteye " + val + " tanısını eklemek istiyormusunuz?";

                    $scope.sil = function () {
                        $scope.bekle = true;
                        var TaniDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField,
                            ereceteTaniBilgisi: [{ taniKodu: val.split(' ')[0].replace(/[^0-9a-zA-Z]$/g, '') }]
                        };
                        EreceteSvc.TaniEkleme(TaniDatasi, PmC.MedullaPassw, PmC.key).then(function (valx) {
                            if (valx.imzaliEreceteTaniEkleReturn.sonucKoduField === "0000") {
                                if (val !== null && !angular.isUndefined(val) && val !== '' && index === -1) {
                                    PmSvc.TaniDegerlendirmesi(val).then(function (response) {
                                        PmC.Muayene.Tanilar.push({ ad: val, kullanaciID: response.kullanici, genelID: response.genel, taniKodu: val.split(' ')[0].replace(/[^0-9a-zA-Z]$/g, '') });
                                        tanilariEreceteyeEkle();
                                        PmC.KayitGuncellemeri();
                                    });
                                    $uibModalInstance.dismiss('cancel');
                                }
                            }
                            else {
                                $scope.bilgi = valx.imzaliEreceteTaniEkleReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (val !== null && !angular.isUndefined(val) && val !== '' && index === -1) {
                PmSvc.TaniDegerlendirmesi(val).then(function (response) {
                    PmC.Muayene.Tanilar.push({ ad: val, kullanaciID: response.kullanici, genelID: response.genel, taniKodu: val.split(' ')[0].replace(/[^0-9a-zA-Z]$/g, '') });
                    tanilariEreceteyeEkle();
                    if (!angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) PmC.KayitGuncellemeri();
                });
            }
        }
        angular.element('#taniAdiInput').trigger('focus');
        PmC.Tani = '';
    };

    PmC.taniDelete = function (val) {
        var index = PmC.Muayene.Tanilar.indexOf(val);
        if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu Reçeteyi Tanıyı Kaldırmak İçin Silmek İstiyormusunuz?";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var SilDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField
                        };
                        EreceteSvc.ReceteSil(SilDatasi, PmC.MedullaPassw, PmC.key).then(function (val) {
                            if (val.imzaliEreceteSilReturn.sonucKoduField === "0000") {
                                PmC.Muayene.imzaliEreceteGirisReturn = null;
                                if (index !== -1) {
                                    PmC.Muayene.Tanilar.splice(index, 1);
                                }
                                tanilariEreceteyeEkle();
                                PmC.KayitGuncellemeri();
                                $uibModalInstance.dismiss('cancel');
                            }
                            else {
                                $scope.bilgi = val.imzaliEreceteSilReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });
                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        } else {
            if (index !== -1) {
                PmC.Muayene.Tanilar.splice(index, 1);
            }
            PmC.KayitGuncellemeri();
            tanilariEreceteyeEkle();
        }
    };

    PmC.taniSablonuEkle = function (val) {
        if (!angular.isUndefined(PmC.Muayene.Sikayetler) && !angular.isUndefined(PmC.Muayene.Ilaclar) && !angular.isUndefined(PmC.Muayene.Bulgulari)) {
            var sablon = { ICDkod: val, Sikayetler: PmC.Muayene.Sikayetler, Ilaclar: PmC.Muayene.Ilaclar, Bulgulari: PmC.Muayene.Bulgulari };
            PmSvc.AddTaniSablonu(sablon).then(function (response) {
                notify({
                    message: val + ' Tanınıza Ait Verileriniz Size Ait Bir Tanı Şablonu Olarak Yüklendi.',
                    classes: 'alert-success',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 5000,
                    position: 'Center'
                });
            });
        } else {
            notify({
                message: 'İlaç, Şikayet ve Bulgu Girmeden Tanı Şablonu Ekleyemezsiniz!',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 5000,
                position: 'Center'
            });
        }
    };

    PmC.taniSablonu = function (val) {
        $scope.oneAtATime = false;
        $scope.status = { open: true, open2: true };
        PmSvc.TaniSablonu(val).then(function (res) {
            if (!angular.isUndefined(PmC.Muayene.Sikayetler)) {
                angular.forEach(res.Sikayetler, function (v) {
                    if (PmC.Muayene.Sikayetler.indexOf(v) === -1) { PmC.Muayene.Sikayetler.push(v); }
                });
            } else {
                PmC.Muayene.Sikayetler = res.Sikayetler;
            }
            if (!angular.isUndefined(PmC.Muayene.Bulgulari)) {
                angular.forEach(res.Bulgulari, function (v) {
                    if (PmC.Muayene.Bulgulari.indexOf(v) === -1) { PmC.Muayene.Bulgulari.push(v); }
                });
            } else {
                PmC.Muayene.Bulgulari = res.Bulgulari;
            }
            PmC.Muayene.Erecete.ereceteIlacBilgisi = [];
            angular.forEach(res.Ilaclar, function (v) {
                var index = PmC.Muayene.Ilaclar.map(function (item) {
                    return item.IlacAdi;
                }).indexOf(v.IlacAdi);
                if (index === -1) {
                    PmC.Muayene.Ilaclar.push(v);
                    PmC.Muayene.Erecete.ereceteIlacBilgisi.push({
                        adet: v.KutuAdeti,
                        barkod: v.barkod,
                        ereceteIlacAciklamaBilgisi: v.ereceteIlacAciklamaListesi,
                        kullanimDoz1: v.GunSayisi,
                        kullanimDoz2: v.KullanimSayisi,
                        kullanimPeriyot: v.kullanimPeriyot,//bir günde
                        kullanimSekli: v.kullanimSekli
                    });
                }
            });
            notify({
                message: res.ICDkod + ' tanınız şablonlarınızdan verilerinize eklendi',
                classes: 'alert-success',
                templateUrl: $scope.inspiniaTemplate,
                duration: 5000,
                position: 'Center'
            });
        });
    };

    $scope.today = function () {
        $scope.dt = new Date();
    };

    $scope.today();

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.popup1 = {
        opened: false
    };

    $scope.open2 = function () {
        $scope.popup2.opened = true;
    };

    $scope.popup2 = {
        opened: false
    };

    $scope.open3 = function () {
        $scope.popup3.opened = true;
    };

    $scope.popup3 = {
        opened: false
    };

    $scope.dateOptions2 = {
        formatYear: 'yy',
        maxDate: new Date(2029, 5, 22),
        minDate: new Date()
    };

    $scope.dateOptions2 = {
        formatYear: 'yy',
        maxDate: new Date(2029, 5, 22),
        minDate: new Date()
    };

    $scope.dateOptions3 = {
        formatYear: 'yy',
        maxDate: new Date(2029, 5, 22),
        minDate: new Date(2000, 5, 22)
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        maxDate: new Date(2029, 5, 22),
        minDate: new Date()
    };

    $scope.frmat = 'dd-MMMM-yyyy';

    $scope.altInputFormats = ['d!/M!/yyyy'];

    $scope.$watchCollection('[PmC.Muayene.Istirahat.BaslamaTarihi,PmC.Muayene.Istirahat.GunSayisi]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            PmC.Muayene.Istirahat.BitisTarihi = PmC.addDay(newValues[0], newValues[1]);
        }
    });

    PmC.IsbasiMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (!PmC.Muayene.Ilaclar.length > 0 && $scope.sonuc !== PmC.sonuclar[6] && $scope.sonuc !== PmC.sonuclar[5]) {
            return notify({
                message: 'İlaçlar Girmeden Kayıt Yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        if (PmC.Muayene.hasOwnProperty('Isbasi')) {
            delete PmC.Muayene.Istirahat;
            delete PmC.Muayene.Sevk;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.mHastalik;
        }
        PmC.acKapa = false;
        Submit();
    };

    PmC.IstirahatMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (PmC.Muayene.hasOwnProperty('Istirahat')) {
            delete PmC.Muayene.Isbasi;
            delete PmC.Muayene.Sevk;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.mHastalik;
            delete PmC.Muayene.GebelikMuayenesi;
        }
        PmC.acKapa = !PmC.acKapa;
        Submit();
    };

    PmC.SevkMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (PmC.Muayene.hasOwnProperty('Sevk')) {
            delete PmC.Muayene.Istirahat;
            delete PmC.Muayene.Isbasi;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.mHastalik;
            delete PmC.Muayene.GebelikMuayenesi;
        }
        PmC.acKapa = !PmC.acKapa;
        Submit();
    };

    PmC.iseDonusMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (PmC.Muayene.hasOwnProperty('iseDonus')) {
            delete PmC.Muayene.Istirahat;
            delete PmC.Muayene.Isbasi;
            delete PmC.Muayene.Sevk;
            delete PmC.Muayene.mHastalik;
            delete PmC.Muayene.GebelikMuayenesi;
        }
        PmC.acKapa = !PmC.acKapa;
        Submit();
    };

    PmC.mHastalikMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (PmC.Muayene.hasOwnProperty('mHastalik')) {
            delete PmC.Muayene.Istirahat;
            delete PmC.Muayene.Isbasi;
            delete PmC.Muayene.Sevk;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.GebelikMuayenesi;
        }
        PmC.acKapa = !PmC.acKapa;
        Submit();
    };

    PmC.GebelikMuayeneKaydet = function () {
        kayitKOntrol(PmC.Muayene.Tanilar, "Tanı Girmeden ", length);
        if (PmC.Muayene.hasOwnProperty('GebelikMuayenesi')) {
            delete PmC.Muayene.Istirahat;
            delete PmC.Muayene.Isbasi;
            delete PmC.Muayene.Sevk;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.mHastalik;
        }
        PmC.acKapa = !PmC.acKapa;
        Submit();
    };
    $scope.message = "Kaydet";
    PmC.Renjk = false;
    PmC.Bekle = false;
    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.message = "Kaydet";
            PmC.Renjk = false;
        }, 100);
    };
    var Submit = function () {
        kayitKOntrol(PmSvc.pmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        PmC.Bekle = true; startTimer();
        if (angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id) || PmC.personelMuayene.PersonelMuayene_Id === null) {
            PmC.personelMuayene = { PMJson: JSON.stringify(PmC.Muayene), MuayeneTuru: PmC.Sonuc(PmC.Muayene) };
            PmSvc.AddPm(PmC.personelMuayene, PmSvc.pmb.SaglikBirimi_Id, PmSvc.pmb.guidId).then(function (response) {
                dataAl(PmSvc.pmb.guidId);
                PmC.selectedRow = response.PersonelMuayene_Id;
                PmC.personelMuayene = response;
                PmC.Muayene = PmC.personelMuayene.PMJson;
                PmC.Muayene.Erecete.protokolNo = PmC.personelMuayene.Protokol;
                $scope.message = "Kaydediliyor!";
                PmC.Renjk = true;
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
                PmC.Bekle = false;

            });
        } else {
            PmSvc.UpdatePm({
                PMJson: JSON.stringify(PmC.Muayene), PersonelMuayene_Id: PmC.personelMuayene.PersonelMuayene_Id, MuayeneTuru: PmC.Sonuc(PmC.Muayene),
                RevirIslem_Id: PmC.personelMuayene.RevirIslem_Id, Protokol: PmC.personelMuayene.Protokol, Personel_Id: PmC.personelMuayene.Personel_Id
            }).then(function (response) {
                dataAl(PmSvc.pmb.guidId);
                PmC.selectedRow = response.PersonelMuayene_Id;
                PmC.personelMuayene = response;
                PmC.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
                $scope.message = "Kaydediliyor!";
                PmC.Renjk = true;
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
                PmC.Bekle = false;
            });
        }
    };

    PmC.NewSubmit = function () {
        PmC.personelMuayene = {};
        PmC.Muayene = {
            Radyoloji: [{ RadyolojikIslem: null, RadyolojikSonuc: null }],
            Ilaclar: [],
            Tanilar: [],
            Tedaviler: [],
            Erecete: {
                doktorBransKodu: null,
                doktorTcKimlikNo: null,
                doktorSertifikaKodu: null,
                protokolNo: null,
                provizyonTipi: 1,
                receteAltTuru: 1,
                receteTarihi: nowdate(),
                receteTuru: 1,
                seriNo: 1,
                takipNo: 0,
                tcKimlikNo: null,
                tesisKodu: null,
                ereceteNo: 0,
                ereceteIlacBilgisi: [],
                ereceteTaniBilgisi: [],
                ereceteAciklamaBilgisi: [{
                    aciklama: null,
                    aciklamaTuru: "1"
                }]
            },
            imzaliEreceteGirisReturn: null
        };
        $scope.sonuc = null;
        PmC.acKapa = true;
        $scope.oneAtATime = true;
        $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open8: false, open9: false };
        $scope.IlacStoklari = null;
        SMSvc.SBStokListeleri(PmSvc.pmb.SaglikBirimi_Id, true).then(function (data) {
            $scope.IlacStoklari = [];
            angular.forEach(data, function (v) {
                if (v.StokTuru !== 'Demirbaş Malz.') { $scope.IlacStoklari.push(v); }
            });
        });
        $('#stok').addClass('collapsed');
        PmC.AnaBilgiVerileri();
    };

    PmC.Sil = function () {
        if (!angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id) || PmC.personelMuayene.PersonelMuayene_Id !== null) {
            if (PmC.Muayene.imzaliEreceteGirisReturn !== null) {
                PmC.EReceteSil(false);
            } else {
                PmSvc.DeletePm(PmC.personelMuayene.PersonelMuayene_Id).then(function (response) {
                    dataAl(PmSvc.pmb.guidId);
                    PmC.NewSubmit();
                }).catch(function (e) {
                    return notify({
                        message: e,
                        classes: 'alert-danger',
                        templateUrl: $scope.inspiniaTemplate,
                        duration: 7000,
                        position: 'right'
                    });
                });
            }
        }
    };

    var tanilariEreceteyeEkle = function () {
        PmC.Muayene.Erecete.ereceteTaniBilgisi = [];
        angular.forEach(PmC.Muayene.Tanilar, function (v) {
            PmC.Muayene.Erecete.ereceteTaniBilgisi.push({ taniKodu: v.taniKodu });
        });
    };

    PmC.IlacRpt = function () {
        delete PmC.Muayene.Radyoloji;
        delete PmC.Muayene.Lab;
        delete PmC.Muayene.LabHemogram;
        delete PmC.Muayene.ANT;
        delete PmC.Muayene.EKG;
        delete PmC.Muayene.Isbasi;
        delete PmC.Muayene.Istirahat;
        delete PmC.Muayene.Sevk;
        delete PmC.Muayene.mHastalik;
        delete PmC.Muayene.iseDonus;
        delete PmC.Muayene.GebelikMuayenesi;
        PmC.Muayene.Erecete.receteTarihi = nowdate();
        PmC.Muayene.Erecete.protokolNo = null;
        PmC.Muayene.imzaliEreceteGirisReturn = null;
        PmC.Muayene.Tedaviler = [];
        delete PmC.Muayene.RevirTedavi_Id;
        PmC.Muayene.Tanilar.push({
            ad: "Z76.0 Tekrar reçete verilmesi",
            kullanaciID: 0,
            genelID: 0
        });
        PmC.Muayene.Radyoloji = [{ RadyolojikIslem: null, RadyolojikSonuc: null }];
        $scope.oneAtATime = false;
        $scope.status = { open: true, open2: true, open3: false, open4: false, open5: false, open6: false, open8: false, open9: false };
        PmC.personelMuayene = {};
        $scope.sonuc = null;
        PmC.acKapa = true;
        $scope.IlacStoklari = null;
        SMSvc.SBStokListeleri(PmSvc.pmb.SaglikBirimi_Id, true).then(function (data) {
            $scope.IlacStoklari = [];
            angular.forEach(data, function (v) {
                if (v.StokTuru !== 'Demirbaş Malz.') { $scope.IlacStoklari.push(v); }
            });
        });
        $('#stok').addClass('collapsed');
    };

    $scope.dateOptions = {
        showWeeks: false
    };

    $scope.isOpen = false;

    $scope.openCalendar = function (e) {
        e.preventDefault();
        e.stopPropagation();
        $scope.isOpen = true;
    };

    $scope.uploadFiles = function (dataUrl) {
        kayitKOntrol(PmSvc.pmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/personel-muayene/' + PmC.personelMuayene.PersonelMuayene_Id,
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

    PmC.stokEkle = function () {
        var inx = 0;
        if (!angular.isUndefined(PmC.IlacSarfCikisi.IlacAdi)) {
            if (PmC.IlacSarfCikisi.StokMiktari < PmC.IlacSarfCikisi.SarfMiktari) {
                notify({
                    message: 'Stok Miktarınzı Üstünde Girişiniz Var!',
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 2000,
                    position: 'right'
                });
            }

            if (angular.isUndefined(PmC.Muayene.Sikayetler)) {
                notify({
                    message: 'Hastanın şikayetlerini giriniz!',
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 2000,
                    position: 'right'
                });
                return;
            }

            if (PmC.Muayene.hasOwnProperty('Tedaviler')) {
                var inx = PmC.Muayene.Tedaviler.map(function (item) {
                    return item.StokId;
                }).indexOf(PmC.IlacSarfCikisi.StokId);
            } else {
                PmC.Muayene.Tedaviler = [];
                inx = -1;
            }

            if (inx === -1) {//add
                PmC.Muayene.Tedaviler.push(PmC.IlacSarfCikisi);
                var index = $scope.IlacStoklari.map(function (item) {
                    return item.StokId;
                }).indexOf(PmC.IlacSarfCikisi.StokId);
                if (index !== -1) {
                    $scope.IlacStoklari.splice(index, 1);
                }
                PmC.IlacSarfCikisi = {};
            } else {//update
                PmC.IlacSarfCikisi = {};
                $scope.selectedRow = -1;
            }
        }
    };

    PmC.stokKaldir = function (val) {
        var index = PmC.Muayene.Tedaviler.map(function (item) {
            return item.StokId;
        }).indexOf(val.StokId);

        if (index !== -1) {
            PmC.Muayene.Tedaviler.splice(index, 1);
        }
        delete val.SarfMiktari;
        $scope.IlacStoklari.push(val);
        $scope.IlacStoklari = $scope.IlacStoklari.sort(function (a, b) {
            return compareStrings(a.IlacAdi, b.IlacAdi);
        });//sırala
        PmC.IlacSarfCikisi = {};
        $scope.selectedRow = -1;
    };

    $scope.rowHighilited = function (row, ilac) {
        $scope.selectedRow = row;
        PmC.IlacSarfCikisi = ilac;
    };

    PmC.stokFilter = function () {
        $scope.IlacStoklari = $scope.IlacStoklari.sort(function (a, b) {
            return compareStrings(a.IlacAdi, b.IlacAdi);
        });
        angular.forEach(PmC.Muayene.Tedaviler, function (v) {
            var index = $scope.IlacStoklari.map(function (item) {
                return item.StokId;
            }).indexOf(v.StokId);
            if (index !== -1) {
                $scope.IlacStoklari.splice(index, 1);
            }
        });
    };

    function gunFarki(date1, date2) {
        var gun_toplami = 1000 * 60 * 60 * 24;
        var date1_as = (new Date(date1)).getTime();
        var date2_as = (new Date(date2)).getTime();
        var farki = Math.abs(date1_as - date2_as);
        return Math.round(farki / gun_toplami);
    }

    $scope.$watchCollection('[PmC.Muayene.iseDonus.BaslamaTarihi,PmC.Muayene.iseDonus.BitisTarihi]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            PmC.Muayene.iseDonus.gunSayisi = gunFarki(newValues[1], newValues[0]);
        }
    });

    $scope.$watch(function () { return PmC.personelMuayene.PersonelMuayene_Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });

    $scope.$watch(function () { return $scope.sonuc; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            SMSvc.MailOnerileri(newValue.adi, PmC.PMB.sirketId, PmC.PMB.bolumId).then(function (data) {
                PmC.mailListesi = [];
                var i = 0;
                angular.forEach(data, function (v) {
                    PmC.mailListesi.push({ i: i + 1, o: true, to: 'Kime', ad: v.AdiSoyadi.trim(), email: v.MailAdresi.trim() });
                    i = i + 1;
                });
                $scope.totalItems = PmC.mailListesi.length;
            });
        }
        if (newValue === PmC.sonuclar[2] || newValue === PmC.sonuclar[1] || newValue === PmC.sonuclar[0]
            && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            delete PmC.Muayene.Ikbilgisi;
            delete PmC.Muayene.iseDonus;
            if (!PmC.Muayene.Sikayetleri.length > 0 || !PmC.Muayene.Bulgulari.length > 0 || !PmC.Muayene.Ilaclar.length > 0)
                $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false };
        }
        if (newValue === PmC.sonuclar[3] && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            PmC.Muayene.Tanilar = [];
            PmC.Muayene.Tanilar.push({
                ad: "Z02 İdari amaçlar için muayene",
                kullanaciID: 0,
                genelID: 0
            },
                {
                    ad: "Z10.0 Mesleki sağlık muayenesi",
                    kullanaciID: 0,
                    genelID: 0
                });
            PmC.Muayene.Sikayetler = [];
            PmC.Muayene.Bulgulari = [];
            delete PmC.Muayene.Ikbilgisi;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.GebelikMuayenesi;
            $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false };
        }
        if (newValue === PmC.sonuclar[4] && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            PmC.Muayene.Sikayetler = [];
            PmC.Muayene.Bulgulari = [];
            delete PmC.Muayene.Ikbilgisi;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.GebelikMuayenesi;
            PmC.Muayene.Tanilar = [];
            PmC.Muayene.Tanilar.push({
                ad: "Z57 Mesleki risk faktörlerine maruz kalma",
                kullanaciID: 0,
                genelID: 0
            });
            $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false };
        }
        if (newValue === PmC.sonuclar[5] && angular.isUndefined(PmC.Muayene.Ikbilgisi) && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            PmC.Muayene.Sikayetler = [
                "İşe Bağlı Yaralanma"
            ];
            PmC.Muayene.Bulgulari = [
                "Muhtelif Yaralanma Bulguları"
            ];
            PmC.Muayene.Ikbilgisi = {
                yaralanma: {
                    olay: null,
                    olayAlt: null,
                    arac: null,
                    aracAlt: null,
                    turu: null,
                    turuAlt: null,
                    yeri: null,
                    yeriAlt: null
                }
            };
            PmC.Muayene.Tanilar = [];
            PmC.Muayene.Tanilar.push({
                ad: "Z04.2 İş kazası sonrası muayene ve gözlem",
                kullanaciID: 0,
                genelID: 0
            });
            $scope.oneAtATime = false;
            $('#ilaclar').addClass('collapsed');
            $scope.status = { open: true, open2: true, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false };
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.GebelikMuayenesi;
        }
        if (newValue === PmC.sonuclar[6] && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            delete PmC.Muayene.Ikbilgisi;
            delete PmC.Muayene.iseDonus;
            delete PmC.Muayene.GebelikMuayenesi;
            PmC.Muayene.Sikayetler = [];
            PmC.Muayene.Bulgulari = [];
            PmC.Muayene.Tanilar = [];
            PmC.Muayene.Tanilar.push(
                {
                    ad: "Z71.9 Danışma, tanımlanmamış",
                    kullanaciID: 0,
                    genelID: 0
                },
                {
                    ad: "Z71.8 Danışma hizmetleri diğer tanımlanmış",
                    kullanaciID: 0,
                    genelID: 0
                },
                {
                    ad: "Z71 Sağlık servislerine diğer danışma ve tıbbi tavsiye için gelen kişiler, ",
                    kullanaciID: 0,
                    genelID: 0
                },
                {
                    ad: "Z71.2 Araştırma bulgularının açıklaması için görüşme yapan kişi",
                    kullanaciID: 0,
                    genelID: 0
                }
            );
            $('#ilaclar').addClass('collapsed');
            $scope.oneAtATime = false;
            $scope.status = { open: false, open2: false, open3: false, open4: false, open5: false, open6: false, open7: false, open8: true, open9: true };
        }
        if (newValue === PmC.sonuclar[7] && angular.isUndefined(PmC.Muayene.GebelikMuayenesi) && angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id)) {
            PmC.Muayene.Sikayetler = [
                "Gebelikle olağan şikayetler"
            ];
            PmC.Muayene.GebelikMuayenesi = {
                ifGebelikOncesiHastalik: false,
                ifOncekiGebelikHastalik: false,
                ifGebelikRiskFaktoru: false,
                ifsabahKusmasi: false,
                ifsirtAgrisi: false,
                ifvarisHemoroid: false,
                iftuvaletIhtiyaci: false,
                ifbedenOlcusu: false,
                ifyorgunlukStress: false,
                ifkayganzemin: false
            };
            PmC.Muayene.Bulgulari = [
                "Patalojik Bir Bulgu Saptanmadı"
            ];
            PmC.Muayene.Tanilar = [];
            PmC.Muayene.Tanilar.push({
                ad: "Z34 Normal gebeliğin gözlemi",
                kullanaciID: 0,
                genelID: 0
            });
            $scope.oneAtATime = false;
            $('#ilaclar').addClass('collapsed');
            $scope.status = { open: true, open2: true, open3: false, open4: false, open5: false, open6: false, open7: false, open8: false, open9: false };
            delete PmC.Muayene.iseDonus;
        }
        else {
            $('#ilaclar').removeClass('collapsed');
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
        PmC.sendMail = mail;
    };
    PmC.newMail = function () {
        PmC.sendMail = {};
        $scope.selectedRow2 = -1;
    };
    PmC.addMail = function (mail) {
        PmC.mailListesi.push({ i: PmC.mailListesi.length + 1, o: true, to: mail.to.trim(), ad: mail.ad.trim(), email: mail.email.trim() });
        PmC.sendMail = {};
        $scope.selectedRow2 = -1;
        $scope.totalItems = PmC.mailListesi.length;
    };
    PmC.deleteMail = function (val) {
        var index = PmC.mailListesi.map(function (item) {
            return item.i;
        }).indexOf(val.i);

        if (index !== -1) {
            PmC.mailListesi.splice(index, 1);
        }
        PmC.sendMail = {};
        $scope.selectedRow2 = -1;
        $scope.totalItems = PmC.mailListesi.length;
    };
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

    PmC.sendMaili = function () {
        kayitKOntrol(PmSvc.pmb.SaglikBirimi_Id, "Sağlık Birimini", null);
        if (PmC.mailListesi.length > 0) {
            var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
            angular.forEach(PmC.mailListesi, function (v) {
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
            mail.SagId = PmSvc.pmb.SaglikBirimi_Id;
            mail.Subject = (PmC.PMB.data.SicilNo !== null ? (PmC.PMB.data.SicilNo.trim() + ' Sicil Nolu ') : '') + PmC.AdSoyad.trim() + ' Personelin ' + $scope.sonuc.adi + ' Hakkında.(ISGplus)';
            if ($scope.sonuc.adi === 'İş Başı') {
                mailSvc.GetMail('./views/SM/View/temp/recete.html', {
                    patient: {
                        name: PmC.AdSoyad, date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                        , Protokol: PmC.personelMuayene.Protokol, Ilaclar: PmC.Muayene.Ilaclar, tcNo: PmC.PMB.data.TcNo
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'İstirahat') {
                var bitistarihi = new Date(PmC.Muayene.Istirahat.BitisTarihi);
                bitistarihi.setDate(bitistarihi.getDate() - 1);
                mailSvc.GetMail('./views/SM/View/temp/isGoremezlik.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, adi: PmC.PMB.data.Adi, soyadi: PmC.PMB.data.Soyadi,
                        sgkNo: PmC.PMB.data.SgkNo, adres: PmC.PMB.data.Adresler[0].GenelAdresBilgisi === undefined ? '' : PmC.PMB.data.Adresler[0].GenelAdresBilgisi, tel: PmC.PMB.data.Telefon, isKazasi: degeri(PmC.Muayene.Istirahat.Nedeni, 'isKazasi'),
                        meslekHastaligi: degeri(PmC.Muayene.Istirahat.Nedeni, 'meslekHastaligi'),
                        hastalik: degeri(PmC.Muayene.Istirahat.Nedeni, 'hastalik'), analik: degeri(PmC.Muayene.Istirahat.Nedeni, 'analik'),
                        Istirahat: { GunSayisi: PmC.Muayene.Istirahat.GunSayisi, BaslamaTarihi: new Date(PmC.Muayene.Istirahat.BaslamaTarihi).toLocaleDateString(), BitisTarihi: bitistarihi.toLocaleDateString(), Sonrasi: PmC.Muayene.Istirahat.Sonrasi, iseBaslamaTarihi: new Date(PmC.Muayene.Istirahat.BitisTarihi).toLocaleDateString() }
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'Sevk') {
                mailSvc.GetMail('./views/SM/View/temp/sevkRaporu.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi,
                        name: PmC.AdSoyad, Sevk: PmC.Muayene.Sevk
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'İşe Dönüş') {
                mailSvc.GetMail('./views/SM/View/temp/iseDonusRaporu.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo,
                        name: PmC.AdSoyad, iseDonus: PmC.Muayene.iseDonus
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'Meslek Hastalığı') {
                mailSvc.GetMail('./views/SM/View/temp/mHastalikRaporu.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar)
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo,
                        name: PmC.AdSoyad, mHastalik: PmC.Muayene.mHastalik
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'İş Kazası') {
                mailSvc.GetMail('./views/SM/View/temp/isKazasiRaporu.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar), Istirahat: PmC.Muayene.Istirahat
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, name: PmC.AdSoyad, Ikbilgisi: PmC.Muayene.Ikbilgisi, tel: PmC.PMB.data.Telefon
                    }
                }, mail);
            }
            if ($scope.sonuc.adi === 'Gebelik Muayenesi') {
                mailSvc.GetMail('./views/SM/View/temp/gebelikRaporu.html', {
                    patient: {
                        date: (new Date(PmC.personelMuayene.Tarih)).toLocaleDateString(), kurumu: PmC.PMB.sirketAdi, tani: PmC.Tanilar(PmC.Muayene.Tanilar), Istirahat: PmC.Muayene.Istirahat
                        , Protokol: PmC.personelMuayene.Protokol, tcNo: PmC.PMB.data.TcNo, saglikBirimi: $scope.nbv.SaglikBirimiAdi, name: PmC.AdSoyad, GebelikMuayenesi: PmC.Muayene.GebelikMuayenesi
                    }
                }, mail);
            }
        }
    };
    PmC.MailBilgisi = 'Gönder';
    $scope.$watch(function () { return $rootScope.isBeingMailed; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            if (newValue === false) {
                PmC.MailBilgisi = 'Gönder';
                delete $rootScope.isBeingMailed2;
                delete $rootScope.isBeingMailed;
                return notify({
                    message: $rootScope.mailGonerildiBilgisi === true ? 'Mail Gönderilmiştir.' : 'Mail Gönderilemedi sistemde sorun olabilir?.',
                    classes: $rootScope.mailGonerildiBilgisi === true ? 'alert-success' : 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 8000,
                    position: 'middle'
                });
            }
            if (newValue === true) {
                PmC.MailBilgisi = 'Gönderiliyor!';
            }
            if (newValue === undefined) {
                PmC.MailBilgisi = 'Gönder';
            }
        }
    });

    PmC.sendEreceteBilgiSMS = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Telefon)) {
            return notify({
                message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var sms = { KisaMesaj: "Sn." + PmC.AdSoyad.trim() + " ERecete Kodunuz:" + PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField + " Gecmis Olsun.", Numaralar: [PmC.PMB.data.Telefon.trim()] };
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

    PmC.sevkSMS = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Telefon)) {
            return notify({
                message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var sms = {
            KisaMesaj: "Sn. " + PmC.AdSoyad.trim() + " Sevkiniz " + new Date(PmC.personelMuayene.PMJson.Sevk.BaslamaTarihi).toLocaleDateString() + " tarihine " + PmC.personelMuayene.PMJson.Sevk.saglikBirimiAdi + " " + PmC.personelMuayene.PMJson.Sevk.bolumu +
                " bölümüne yapılmıştır.Devamsız sayılmamanız için muayene sonrasında alacaginiz işbaşi kagıdını IK birimine getirmeniz önemle rica olunur.Geçmiş Olsun.", Numaralar: [PmC.PMB.data.Telefon.trim()]
        };
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

    PmC.istirihatSMS = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Telefon)) {
            return notify({
                message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var sms = {
            KisaMesaj: "Sn. " + PmC.AdSoyad.trim() + " Raporunuz " + new Date(PmC.personelMuayene.PMJson.Istirahat.BaslamaTarihi).toLocaleDateString() + ' tarihinde başlamıştır. Gün sayısı:' + PmC.personelMuayene.PMJson.Istirahat.GunSayisi + "   " +
                new Date(PmC.personelMuayene.PMJson.Istirahat.BitisTarihi).toLocaleDateString() + " tarihinde işe başlıyorsunuz. Geçmiş olsun.", Numaralar: [PmC.PMB.data.Telefon.trim()]
        };
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

    PmC.sendEreceteBilgiMaili = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Mail)) {
            return notify({
                message: 'Personelin Mail adresi olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        if (PmSvc.pmb.SaglikBirimi_Id === null) {
            return notify({
                message: 'Sağlık Birimini Girmeden Mail Gönderimi Yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
        mail.To.push({ address: PmC.PMB.data.Mail.trim(), displayName: PmC.AdSoyad });
        mail.SagId = PmSvc.pmb.SaglikBirimi_Id;
        mail.Subject = (PmC.PMB.data.SicilNo !== null ? (PmC.PMB.data.SicilNo.trim() + ' Sicil Nolu ') : '') + PmC.AdSoyad.trim() + ' Personelin Erecete Hakkında.(ISGplus)';
        mailSvc.GetMail('./views/SM/View/temp/erecete.html', {
            Muayene: {
                imzaliEreceteGirisReturn: PmC.Muayene.imzaliEreceteGirisReturn
            }
        }, mail);
    };

    function capitalizeFirstLetter([first, ...rest], locale = navigator.language) {
        return [first.toLocaleUpperCase(locale), ...rest].join('');
    }

    PmC.perMail = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Mail)) {
            return notify({
                message: 'Personelin Mail adresi olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        if (PmSvc.pmb.SaglikBirimi_Id === null) {
            return notify({
                message: 'Sağlık Birimini Girmeden Mail Gönderimi Yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
        mail.To.push({ address: PmC.PMB.data.Mail.trim(), displayName: PmC.AdSoyad });
        mail.SagId = PmSvc.pmb.SaglikBirimi_Id;
        mail.Subject = 'Kişisel Bilgilendirmeniz Hakkında.(ISGplus)';
        mailSvc.GetMail('./views/SM/View/temp/bosSablon.html', {
            mailx: {
                adiField: capitalizeFirstLetter(PmC.PMB.data.Adi.toLowerCase(), 'tr'), soyadiField: PmC.PMB.data.Soyadi, bilgiField: PmC.Muayene.MailBilgisi.trim(), mailigonderenField: PmC.val.FullName, saglikBirimiField: $scope.nbv.SaglikBirimiAdi,
            }
        }, mail);
    };

    PmC.perSMS = function () {
        if (isNullOrWhitespace(PmC.PMB.data.Telefon)) {
            return notify({
                message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        if (PmC.Muayene.MailBilgisi.trim().length > 155) {
            return notify({
                message: '155 karakterin üstünde SMS gönderemezsiniz',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var sms = { KisaMesaj: PmC.Muayene.MailBilgisi.trim(), Numaralar: [PmC.PMB.data.Telefon.trim()] };
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


    PmSvc.GetYaralanmaOlaylari().then(function (s) {
        PmC.yaralanmaOlaylari = s;
    });

    $scope.$watch(function () {
        if (angular.isDefined(PmC.Muayene.Ikbilgisi) && angular.isDefined(PmC.Muayene.Ikbilgisi.yaralanma)) {
            return PmC.Muayene.Ikbilgisi.yaralanma.olay;
        }
    }, function (newValue, oldValue) {
        if (angular.isDefined(newValue)) {
            angular.forEach(PmC.yaralanmaOlaylari, function (v) {
                if (v.kaza.trim() === newValue.trim()) {
                    PmC.yaralanmaOlaylariAlt = v.alt;
                    return;
                }
            });
        }
    });

    PmSvc.GetYaralanmaAraclari().then(function (s) {
        PmC.yaralanmaAraclari = s;
    });
    //if (angular.isDefined(newValue)){}
    $scope.$watch(function () {
        if (angular.isDefined(PmC.Muayene.Ikbilgisi) && angular.isDefined(PmC.Muayene.Ikbilgisi.yaralanma)) {
            return PmC.Muayene.Ikbilgisi.yaralanma.arac;
        }
    }, function (newValue, oldValue) {
        angular.forEach(PmC.yaralanmaAraclari, function (v) {
            if (v.kaza.trim() === newValue.trim()) {
                PmC.yaralanmaAraclariAlt = v.alt;
                return;
            }
        });
    });

    PmSvc.GetYaralanmaTurleri().then(function (s) {
        PmC.yaralanmaTurleri = s;
    });

    $scope.$watch(function () {
        if (angular.isDefined(PmC.Muayene.Ikbilgisi) && angular.isDefined(PmC.Muayene.Ikbilgisi.yaralanma)) {
            return PmC.Muayene.Ikbilgisi.yaralanma.turu;
        }
    }, function (newValue, oldValue) {
        angular.forEach(PmC.yaralanmaTurleri, function (v) {
            if (v.kaza.trim() === newValue.trim()) {
                PmC.yaralanmaTurleriAlt = v.alt;
                return;
            }
        });
    });

    PmSvc.GetYaralanmaYerleri().then(function (s) {
        PmC.yaralanmaYerleri = s;
    });

    $scope.$watch(function () {
        if (angular.isDefined(PmC.Muayene.Ikbilgisi) && angular.isDefined(PmC.Muayene.Ikbilgisi.yaralanma)) {
            return PmC.Muayene.Ikbilgisi.yaralanma.yeri;
        }
    }, function (newValue, oldValue) {
        //if (newValue !== oldValue && newValue !== undefined) {
        angular.forEach(PmC.yaralanmaYerleri, function (v) {
            if (v.kaza.trim() === newValue.trim()) {
                PmC.yaralanmaYerleriAlt = v.alt;
                return;
            }
        });

    });

    $scope.$watch(function () {
        if (angular.isDefined(PmC.Muayene.GebelikMuayenesi) && angular.isDefined(PmC.Muayene.GebelikMuayenesi.SonAdetTarihi)) {
            return PmC.Muayene.GebelikMuayenesi.SonAdetTarihi;
        }
    }, function (newValue, oldValue) {
        var result = new Date(newValue);
        result.setDate(result.getDate() + 280);
        var gunSayisi = gunFarki(new Date(), newValue);
        if (angular.isDefined(PmC.Muayene.GebelikMuayenesi)) {
            PmC.Muayene.GebelikMuayenesi.DogumTarihi = result;
            PmC.Muayene.GebelikMuayenesi.HaftaSayisi = Math.floor(gunSayisi / 7);
        }
    });

    //recete bilgileri
    PmSvc.JsonGetFile('aciklamaTuru').then(function (resp) {
        PmC.aciklamaTurus = resp;
    });
    PmSvc.JsonGetFile('kullanilanDoz').then(function (resp) {
        PmC.kullanilanDozs = resp;
    });
    PmSvc.JsonGetFile('kullanilanPeriyod').then(function (resp) {
        PmC.kullanilanPeriyods = resp;
    });
    PmSvc.JsonGetFile('kullanimSekli').then(function (resp) {
        PmC.kullanimSeklis = resp;
    });
    PmSvc.JsonGetFile('receteAltTuru').then(function (resp) {
        PmC.receteAltTurus = resp;
    });
    PmSvc.JsonGetFile('provizyonTipi').then(function (resp) {
        PmC.provizyonTipis = resp;
    });
    PmSvc.JsonGetFile('receteTuru').then(function (resp) {
        PmC.receteTurus = resp;
    });




    PmC.Kullanimlar = ['Yemekten Sonra (Tok)', 'Yemekten Önce (Aç)', 'Oturma Banyosu', 'Sabah', 'Öğlen', 'Akşam', 'Gece', 'Ölçek', 'Suda Eritelerek', '2 saat aralıkla 4 kez',
        'Sabah Akşam', 'Öğlen Akşam', 'Yatmadan Önce', ' İntramüsküler (im)', 'İntravenöz(iv)', 'Haricen', 'Ayak Banyosu', '10(on) günlük tedavi dozudur', 'Majistral Uygulama',
        'Bol Su İle', 'Makat', 'İnhalasyon yapılacak', 'Damla', 'Gün Aşırı', 'Göz Banyosu', 'Subkutan (sc)', 'İntrakutan (ic)', 'Gargara yapılacak', '1(Bir) Aylık tedavi dozudur.',
        'Haftada Bir', 'Ayda Bir', 'İki Günde Bir', 'Üç Günde Bir', 'Banyo', 'İftardan Sonra', 'İftar ve Sahurda', 'Yemek Arası', 'Raporlu İlaç', 'Ayaktan Parenteral Tedavi (APAT)', 'APAT 10 Günlük Tedavi.',
        'Atuşman', 'Pansuman', 'Yara Üzerine', 'Sivilce Üzerine', 'Yanık Üzerine', 'Ağızda Emilecek', 'Dil Altı', 'Kol Altı', 'Parmak Arası'];

    PmC.EReceteKaydet = function () {
        $uibModal.open({
            templateUrl: 'imza.html',
            backdrop: true,
            animation: true,
            size: 'md',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance) {
                $scope.bekle = true;
                $scope.hataMesaji = null;
                PmC.Muayene.Erecete.protokolNo = PmC.personelMuayene.Protokol;
                EreceteSvc.ReceteYaz(PmC.Muayene.Erecete, PmC.MedullaPassw, PmC.key).then(function (val) {
                    if (val.imzaliEreceteGirisReturn.sonucKoduField === "0000") {
                        PmC.Muayene.imzaliEreceteGirisReturn = val.imzaliEreceteGirisReturn;
                        PmSvc.UpdatePm({
                            PMJson: JSON.stringify(PmC.Muayene), PersonelMuayene_Id: PmC.personelMuayene.PersonelMuayene_Id, MuayeneTuru: PmC.Sonuc(PmC.Muayene),
                            RevirIslem_Id: PmC.personelMuayene.RevirIslem_Id, Protokol: PmC.personelMuayene.Protokol, Personel_Id: PmC.personelMuayene.Personel_Id
                        }).then(function (response) {
                            dataAl(PmSvc.pmb.guidId);
                            PmC.selectedRow = response.PersonelMuayene_Id;
                            PmC.personelMuayene = response;
                            PmC.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
                            $scope.bekle = false;
                            $scope.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
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
                    } else {
                        $scope.hataMesaji = val.imzaliEreceteGirisReturn.sonucMesajiField;
                        $scope.bekle = false;
                    }
                });
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.sendEreceteBilgiMaili = function () {
                    PmC.sendEreceteBilgiMaili();
                };
                $scope.sendEreceteBilgiSMS = function () {
                    PmC.sendEreceteBilgiSMS();
                };
            }
        });
    };

    PmC.KayitGuncellemeri = function () {
        if (!angular.isUndefined(PmC.personelMuayene.PersonelMuayene_Id) && PmC.personelMuayene.PersonelMuayene_Id !== null) {
            PmSvc.UpdatePm({
                PMJson: JSON.stringify(PmC.Muayene), PersonelMuayene_Id: PmC.personelMuayene.PersonelMuayene_Id, MuayeneTuru: PmC.Sonuc(PmC.Muayene),
                RevirIslem_Id: PmC.personelMuayene.RevirIslem_Id, Protokol: PmC.personelMuayene.Protokol, Personel_Id: PmC.personelMuayene.Personel_Id
            }).then(function (response) {
                dataAl(PmSvc.pmb.guidId);
                PmC.selectedRow = response.PersonelMuayene_Id;
                PmC.personelMuayene = response;
                PmC.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
                $scope.bekle = false;
                $scope.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
            }).catch(function (e) {
                return notify({
                    message: e.data,
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 7000,
                    position: 'right'
                });
            });
        }
    };

    PmC.EReceteSil = function (guncelleEreceteyiSil) {
        if (!angular.isUndefined(PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField) || PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField !== null) {
            $uibModal.open({
                templateUrl: 'imzaSil.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance) {
                    $scope.no = PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField;
                    $scope.bilgi = "Barkodlu Reçeteyi Silmek İstiyormusunuz";
                    $scope.sil = function () {
                        $scope.bekle = true;
                        var SilDatasi = {
                            ereceteNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.ereceteNoField,
                            doktorTcKimlikNo: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.doktorTcKimlikNoField,
                            tesisKodu: PmC.Muayene.imzaliEreceteGirisReturn.ereceteDVOField.tesisKoduField
                        };
                        PmC.Muayene.Erecete.protokolNo = PmC.personelMuayene.Protokol;
                        EreceteSvc.ReceteSil(SilDatasi, PmC.MedullaPassw, PmC.key).then(function (val) {
                            if (val.imzaliEreceteSilReturn.sonucKoduField === "0000") {
                                PmC.Muayene.imzaliEreceteGirisReturn = null;
                                if (guncelleEreceteyiSil) {
                                    //sadece erceteyi siler.
                                    PmSvc.UpdatePm({
                                        PMJson: JSON.stringify(PmC.Muayene), PersonelMuayene_Id: PmC.personelMuayene.PersonelMuayene_Id, MuayeneTuru: PmC.Sonuc(PmC.Muayene),
                                        RevirIslem_Id: PmC.personelMuayene.RevirIslem_Id, Protokol: PmC.personelMuayene.Protokol, Personel_Id: PmC.personelMuayene.Personel_Id
                                    }).then(function (response) {
                                        dataAl(PmSvc.pmb.guidId);
                                        PmC.selectedRow = response.PersonelMuayene_Id;
                                        PmC.personelMuayene = response;
                                        PmC.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
                                        $scope.bekle = false;
                                        $scope.Muayene = JSON.parse(PmC.personelMuayene.PMJson);
                                        $uibModalInstance.dismiss('cancel');

                                    }).catch(function (e) {
                                        return notify({
                                            message: e.data,
                                            classes: 'alert-danger',
                                            templateUrl: $scope.inspiniaTemplate,
                                            duration: 7000,
                                            position: 'right'
                                        });
                                    });
                                } else {//hem kaydı hemde ereceteyi siler
                                    PmSvc.DeletePm(PmC.personelMuayene.PersonelMuayene_Id).then(function (response) {
                                        dataAl(PmSvc.pmb.guidId);
                                        PmC.NewSubmit();
                                        $uibModalInstance.dismiss('cancel');

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
                                }
                            } else {
                                $scope.bilgi = val.imzaliEreceteSilReturn.sonucMesajiField;
                                $scope.bekle = false;
                            }
                        });

                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                }
            });
        }
    };

    PmC.refreshLaboratuvar = function (val) {
        PmSvc.LaboratuarTetkikAra(val.search).then(function (response) {

                 var hg = [];
            angular.forEach(response, function (v) {
                hg.push(v.tetkik === undefined ? v : v.tetkik);
            });
            PmC.items = hg;
        })
    };

    PmC.Ilaclar = function (val) {
        var hg = [];
        angular.forEach(val, function (v) {
            hg.push(v.IlacAdi.trim());
        });
        return val.length > 0 ? hg.toString() : 'REÇETE VERİLMEDİ.!';
    };


    PmC.Ikaz = function (tumu) {
        $uibModal.open({
            templateUrl: './views/SM/View/temp/Ikaz.html',
            backdrop: true,
            animation: true,
            size: 'lg',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout) {
                var startTimer2 = function (sure) {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $uibModalInstance.dismiss('cancel');
                        $scope.bilgilendirme = "";
                    }, sure);
                };
                $scope.bilgilendirme = "";
                var sn = [];
                IkazSvc.GetTumIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
                    sn = s;
                    $scope.Ikazlar = tumu === false ? PmC.PMB.data.Ikazlar : sn;
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
                    Personel_Id: PmC.PMB.data.Personel_Id,
                    Status: true,
                    UserId: null
                };
                $scope.remove = function () {
                    var sx = [];
                    if ($scope.uyari.Ikaz_Id !== null) {
                        IkazSvc.IkazSil($scope.uyari.Ikaz_Id).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
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
                        MuayeneTuru: "Personel Muayenesi",
                        SonTarih: null,
                        Tarih: new Date(),
                        Personel_Id: PmC.PMB.data.Personel_Id,
                        Status: true,
                        UserId: null
                    };
                };
                var sd = function (sx) {
                    $scope.Ikazlar = sx;
                    PmC.PMB.data.Ikazlar = sx;
                    PmC.warning = sx.length > 0 ? true : false;
                    PmC.warningLength = sx.length;
                    startTimer2(3000);
                };
                $scope.save = function () {
                    var sx = [];
                    if (angular.isUndefined($scope.uyari.Ikaz_Id) || $scope.uyari.Ikaz_Id === null) {
                        IkazSvc.IkazEkle($scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            var datestring = $scope.uyari.SonTarih.getDate() + "/" + ($scope.uyari.SonTarih.getMonth() + 1) + "/" + $scope.uyari.SonTarih.getFullYear();
                            $scope.bilgilendirme = datestring + " Tarihli uyarınız sisteme eklendi.Formunuz kapatılıyor.";

                        });
                    } else {
                        IkazSvc.IkazGuncelle($scope.uyari.Ikaz_Id, $scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(PmC.PMB.data.Personel_Id).then(function (s) {
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
                    if (isNullOrWhitespace(PmC.PMB.data.Telefon)) {
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
                    var sms = { KisaMesaj: $scope.uyari.IkazText.trim(), Numaralar: [PmC.PMB.data.Telefon.trim()] };
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
}

PmCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams', 'TkSvc', 'SMSvc', '$cookies', 'printer',
    'ngAuthSettings', 'uploadService', 'authService', '$q', '$window', '$timeout', 'notify', '$http', 'PmSvc', '$uibModal', '$rootScope', 'mailSvc'
    , 'personellerSvc', 'EreceteSvc', 'IkazSvc'];

angular
    .module('inspinia')
    .controller('PmCtrl', PmCtrl);