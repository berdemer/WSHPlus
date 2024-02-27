'use strict';

function egtKayitCtrl($scope, ngAuthSettings, $q, notify, $timeout, $uibModal, $rootScope, uploadService,
    $stateParams, personellerSvc, sirketSvc, tanimlarSvc, egitimSvc, mailSvc, PmSvc) {
    var EgKC = this;
    EgKC.egitimYapisi = {
        'isgProfTckNo': null,
        'egitimTarihi': null,
        'egitimYer': null,
        'egitimtur': null,
        'sgkTescilNo': null,
        'egiticiTckNo': null,
        'imzalıDeger': null,
        'firmaKodu': null,
        'belgeTipi': null,
        'egitimObjects': [],
        'calisanObjects': []
    };
    var teknik = false; var saglik = false; var diger = false; var genel = false; var picFile = null;
    EgKC.dersler = { 'KaydaGidecekDersler': [] };
    personellerSvc.GetFullIsgUsers().then(function (r) {//uzman listesi
        angular.forEach(r, function (value) {
            EgKC.isgUsers.push({ userId: value.Id, adi: value.FullName, gorevi: value.Gorevi !== undefined ? value.Gorevi : '', meslegi: value.Meslek !== undefined ? value.Meslek : '', TcNo: value.TcNo !== undefined ? value.TcNo : '' });
        });
    });
    EgKC.egitimYerleri = [{ 'K': 0, 'N': 'İşyeri dışında' }, { 'K': 1, 'N': 'İşyeri içinde' }];
    EgKC.egitimTurleri = [{ 'K': 0, 'N': 'Uzaktan Eğitim' }, { 'K': 1, 'N': 'Yüzyüze Eğitim' }];
    EgKC.belgeTipleri = [{ 'K': 1, 'N': 'İşyeri hekimi' }, { 'K': 2, 'N': 'İş güvenliği uzmanı' }];
    $scope.onloadFun = function () {
        if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {
            $scope.fileImgPath = ngAuthSettings.isAzure ? ngAuthSettings.storageLinkService + 'personel/' : ngAuthSettings.storageLinkService + uploadFolder;
            sirketSvc.GetSirketDetayi($stateParams.id).then(function (data) {
                EgKC.egitimYapisi.sgkTescilNo = data !== null ? data.SGKSicilNo : '';
            });
            if (egitimSvc.Egitim.id === null) {
                egitimSvc.Egitim.data = null;
                EgKC.egitimYapisi = {
                    'isgProfTckNo': null,
                    'egitimTarihi': null,
                    'egitimYer': null,
                    'egitimtur': null,
                    'sgkTescilNo': null,
                    'egiticiTckNo': null,
                    'imzalıDeger': null,
                    'firmaKodu': null,
                    'belgeTipi': null,
                    'egitimObjects': [],
                    'calisanObjects': []
                };
                EgKC.dersler = { 'KaydaGidecekDersler': [] };
                EgKC.Katilimciler = [];
                EgKC.profesyoneli = null;
                EgKC.egitimTarihi = null;
                EgKC.IsgTopluEgitimi_Id = null;
            };
            if (egitimSvc.Egitim.id !== null) {
                var we = egitimSvc.Egitim.data;
                EgKC.IsgTopluEgitimi_Id = egitimSvc.Egitim.id;
                EgKC.egitimYapisi = we.egitimYapisi;
                EgKC.dersler = we.dersler;
                angular.forEach(we.dersler.DerseKatilanlar, function (v) {
                    v.DurumId = egitimSvc.Egitim.id;
                    v.KayitDurumu = true;
                });
                EgKC.Katilimciler = we.dersler.DerseKatilanlar;
                EgKC.profesyoneli = { userId: null, adi: we.dersler.IsgProfesyoneliAdi, gorevi: null, meslegi: null, TcNo: we.egitimYapisi.isgProfTckNo };
                EgKC.egitimTarihi = new Date(we.dersler.egitimTarihi);
               
            };
            uploadService.GetImageStiId("logo", $stateParams.id).then(function (response) {
               picFile = response[0].LocalFilePath;
            });
        };
    };
    EgKC.getRepos = function (text) {
        var deferred = $q.defer();
        $scope.msg = true;
        personellerSvc.PersonelEgitimSearch(text, $stateParams.id).then(function (data) {
            angular.forEach(data.PersonelCards, function (v, k) {
                v.Photo = $scope.fileImgPath + v.Photo;
            });
            deferred.resolve(data.PersonelCards);
        }).catch(function (e) {
            $scope.message = e.data;
        }).finally(function () {
            $scope.msg = false;
        });
        return deferred.promise;
    };
    EgKC.EgitimTurleri = []
    EgKC.Salonlar = [];
    tanimlarSvc.GetTanimAdiList("Toplanti Odasi").then(function (response) {
        angular.forEach(response, function (value, key) {
            EgKC.Salonlar.push({
                salon: value.ifade !== null ? value.ifade.trim() : ''
            });
        });
    });
    tanimlarSvc.GetTanimAdiList("Personel Eğitim Tablosu").then(function (response) {
        angular.forEach(response, function (value, key) {
            EgKC.EgitimTurleri.push({
                konu: value.ifade !== null ? value.ifade.trim() : '',
                kod: value.tanimKisaltmasi !== null ? value.tanimKisaltmasi.trim() : '',
                tipi: value.ifadeBagimliligi !== null ? value.ifadeBagimliligi.trim() : '',
                tanimlama: value.ifadeBagimliligi.trim() + key
            });
        });
    });
    EgKC.EgitimObject = [];
    EgKC.egitimTarihi = new Date();
    $scope.$watch(function () { return EgKC.egitimTarihi; }, function (newValue, oldValue) {
        EgKC.dersler.egitimTarihi = EgKC.egitimTarihi;
        EgKC.egitimYapisi.egitimTarihi = newValue.toLocaleDateString('tr-TR').replaceAll('.', '-');
    });
    $scope.$watch(function () { return EgKC.profesyoneli; }, function (newValue, oldValue) {
        EgKC.egitimYapisi.egiticiTckNo = newValue.TcNo;
        EgKC.egitimYapisi.isgProfTckNo = newValue.TcNo;
        EgKC.dersler.IsgProfesyoneliAdi = newValue.adi;
    });
    EgKC.isgUsers = [];
    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };
    $scope.popup1 = {
        opened: false
    };
    $scope.dateOptions1 = {
        formatYear: 'yy',
        maxDate: new Date(2050, 5, 22)
    };
    EgKC.acKapa = true;
    $scope.showhide = function () {
        EgKC.acKapa = !EgKC.acKapa;
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
    function NumToTime(num) {
        var h = Math.floor(num / 60);
        var m = num % 60;
        h = h < 10 ? '0' + h : h;
        m = m < 10 ? '0' + m : m;
        return h + ':' + m;
    }
    EgKC.konularDegisimi = function (sure, obje, egt) {
        if (egt.tanimlama.search('Diger') === -1) {
            if (obje === true) {
                if (EgKC.egitimYapisi.egitimObjects.findIndex(a => a.egitimKoduId === parseInt(egt.kod)) > -1) {
                    EgKC.egitimYapisi.egitimObjects.splice(EgKC.egitimYapisi.egitimObjects.findIndex(a => a.egitimKoduId === parseInt(egt.kod)), 1);
                    EgKC.dersler.KaydaGidecekDersler.splice(EgKC.dersler['KaydaGidecekDersler'].findIndex(a => a.egitimKoduId === parseInt(egt.kod)), 1);
                };
                if (sure > 0) {
                    EgKC.egitimYapisi.egitimObjects.push({ egitimKoduId: parseInt(egt.kod), egitimSuresi: parseInt(sure) });
                    EgKC.dersler.KaydaGidecekDersler.push({ egitimKoduId: parseInt(egt.kod), konu: egt.konu, tipi: egt.tipi, egitimSuresi: parseInt(sure) });
                };
            } else {
                EgKC.egitimYapisi.egitimObjects.splice(EgKC.egitimYapisi.egitimObjects.findIndex(a => a.egitimKoduId === parseInt(egt.kod)), 1);
                EgKC.dersler.KaydaGidecekDersler.splice(EgKC.dersler['KaydaGidecekDersler'].findIndex(a => a.egitimKoduId === parseInt(egt.kod)), 1);
            };
        };
    };
    $scope.$watchCollection(function () { return EgKC.dersler.genelKonular; }, function (newValue, oldValue) {
        var baslikAdları = Object.keys(EgKC.dersler.genelKonular);
        EgKC.genelderssuresi = 0;
        angular.forEach(baslikAdları, function (v) {//v =>item adı
            if (newValue[v] === false) {
                newValue[v + 'sure'] = 0;
            };
            if (v.search('sure') > -1) {
                EgKC.genelderssuresi = EgKC.genelderssuresi + parseInt(newValue[v])
            };
        });
        if (genel) EgKC.dersler['DersSuresi'] = EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi;//sıfırların yerine diğer toplamlar gelecek
        if (genel) EgKC.dersler['DersSuresiSaat'] = NumToTime(EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi);//sıfırların yerine diğer toplamlar gelecek
        genel = true;
    });
    $scope.$watchCollection(function () { return EgKC.dersler.saglikKonular; }, function (newValue, oldValue) {
        var baslikAdları = Object.keys(EgKC.dersler.saglikKonular);
        EgKC.saglikderssuresi = 0;
        angular.forEach(baslikAdları, function (v) {//v =>item adı
            if (newValue[v] === false) {
                newValue[v + 'sure'] = 0;
            };
            if (v.search('sure') > -1) {
                EgKC.saglikderssuresi = EgKC.saglikderssuresi + parseInt(newValue[v])
            };
        });
        if (saglik) EgKC.dersler['DersSuresi'] = EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi;//sıfırların yerine diğer toplamlar gelecek
        if (saglik) EgKC.dersler['DersSuresiSaat'] = NumToTime(EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi);//sıfırların yerine diğer toplamlar gelecek
        saglik = true;
    });
    $scope.$watchCollection(function () { return EgKC.dersler.teknikKonular; }, function (newValue, oldValue) {
        var baslikAdları = Object.keys(EgKC.dersler.teknikKonular);
        EgKC.teknikderssuresi = 0;
        angular.forEach(baslikAdları, function (v) {//v =>item adı
            if (newValue[v] === false) {
                newValue[v + 'sure'] = 0;
            };
            if (v.search('sure') > -1) {
                EgKC.teknikderssuresi = EgKC.teknikderssuresi + parseInt(newValue[v])
            };
        });
        if (teknik) EgKC.dersler['DersSuresi'] = EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi;//sıfırların yerine diğer toplamlar gelecek
        if (teknik) EgKC.dersler['DersSuresiSaat'] = NumToTime(EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi);//sıfırların yerine diğer toplamlar gelecek
        teknik = true;
    });
    $scope.$watchCollection(function () { return EgKC.dersler.digerKonular; }, function (newValue, oldValue) {
        var baslikAdları = Object.keys(EgKC.dersler.digerKonular);
        EgKC.digerderssuresi = 0;
        angular.forEach(baslikAdları, function (v) {//v =>item adı
            if (newValue[v] === false) {
                newValue[v + 'sure'] = 0;
            };
            if (v.search('sure') > -1) {
                EgKC.digerderssuresi = EgKC.digerderssuresi + parseInt(newValue[v])
            };
        });
        if (deger) EgKC.dersler['DersSuresi'] = EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi;//sıfırların yerine diğer toplamlar gelecek
        if (deger) EgKC.dersler['DersSuresiSaat'] = NumToTime(EgKC.teknikderssuresi + EgKC.digerderssuresi + EgKC.genelderssuresi + EgKC.saglikderssuresi);//sıfırların yerine diğer toplamlar gelecek
        deger = true;
    });
    EgKC.SablonYapisi = {

        egitimYapisi: {
            'egitimYer': null,
            'egitimtur': null,
            'belgeTipi': null,
            'egitimObjects': [],
        }, dersler: {}, SablonTanimi: null
    };
    EgKC.SablonYapisiSistemdeVarmi = false;
    EgKC.sablonKayitModeli = function () {
        $uibModal.open({
            templateUrl: 'SablonKaydet.html',
            backdrop: true,
            animation: true,
            size: 'sm',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout) {
                var kayıtzamani = function () {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $scope.message = "";
                        $uibModalInstance.dismiss('cancel');
                    }, 3000);
                };
                $scope.ekleTani = function (val) {
                    if (EgKC.SablonYapisiSistemdeVarmi === false) {
                        EgKC.SablonYapisi = {
                            egitimYapisi: {
                                egitimYer: EgKC.egitimYapisi.egitimYer,
                                egitimtur: EgKC.egitimYapisi.egitimtur,
                                belgeTipi: EgKC.egitimYapisi.belgeTipi,
                                egitimObjects: EgKC.egitimYapisi.egitimObjects
                            },
                            dersler: EgKC.dersler,
                            SablonTanimi: $scope.SablonTanimi
                        };
                        egitimSvc.AddISGSablonlari({ EgitimJson: JSON.stringify(EgKC.SablonYapisi), Egitim_Turu: $scope.SablonTanimi }).then(function (val) {
                            $scope.message = "Kayıt yapılıyor! Bekleyiniz...";
                            egitimSvc.GetISGSablonlari().then(function (response) {
                                EgKC.Sablonlar = response;
                            });
                            EgKC.sablon = EgKC.Sablonlar[EgKC.Sablonlar.length - 1];
                            kayıtzamani();
                        });
                    };
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
    };
    egitimSvc.GetISGSablonlari().then(function (response) {
        EgKC.Sablonlar = response;
    });
    $scope.$watchCollection(function () { return EgKC.sablon; }, function (newValue, oldValue) {
        if (newValue !== undefined) {
            var sablon = JSON.parse(newValue.EgitimJson);
            EgKC.egitimYapisi.egitimYer = sablon.egitimYapisi.egitimYer;
            EgKC.egitimYapisi.egitimtur = sablon.egitimYapisi.egitimtur;
            EgKC.egitimYapisi.belgeTipi = sablon.egitimYapisi.belgeTipi;
            EgKC.egitimYapisi.egitimObjects = sablon.egitimYapisi.egitimObjects;
            EgKC.dersler = sablon.dersler;
            EgKC.dersler.egitimTarihi = EgKC.egitimTarihi ?? EgKC.egitimTarihi;
            EgKC.dersler.IsgProfesyoneliAdi = EgKC.profesyoneli.adi ?? EgKC.profesyoneli.adi;//boş ve geçersiz değilse
            if (EgKC.IsgTopluEgitimi_Id !== null) {
                EgKC.dersler.DerseKatilanlar = egitimSvc.Egitim.data.dersler.DerseKatilanlar;
            };
        };
    });
    EgKC.sablonSil = function (v) {
        if (confirm(v.Egitim_Turu + " Şablonunu Silmek İstediğinizden Eminmisin?")) {
            egitimSvc.DeleteISGSablonlari(v.ISG_TopluEgitimSablonlariId).then(function (val) {
                egitimSvc.GetISGSablonlari().then(function (response) {
                    EgKC.Sablonlar = response;
                    alert("Silindi " + v.Egitim_Turu);
                });
            });
        }
    };
    $scope.$watchCollection(function () { return EgKC.dersler.KaydaGidecekDersler; }, function (newValue, oldValue) {
        if (!angular.isUndefined(EgKC.IsgTopluEgitimi_Id) || EgKC.IsgTopluEgitimi_Id !== null) {
            EgKC.derslerinOzeti = [];
            angular.forEach(EgKC.dersler.KaydaGidecekDersler, function (v, k) {
                EgKC.derslerinOzeti.push({ tanim: v.konu, sure: v.egitimSuresi });
            });
        }
    });
    $scope.$watchCollection(function () { return EgKC.egitimYapisi.egitimObjects; }, function (newValue, oldValue) {
        if (angular.isUndefined(EgKC.IsgTopluEgitimi_Id) || EgKC.IsgTopluEgitimi_Id === null) {
            EgKC.derslerinOzeti = [];
            angular.forEach(EgKC.egitimYapisi.egitimObjects, function (v, k) {
                EgKC.derslerinOzeti.push({ tanim: EgKC.EgitimTurleri[EgKC.EgitimTurleri.findIndex(a => a.kod === v.egitimKoduId.toString())].konu, sure: v.egitimSuresi });
            });
        }
    });
    EgKC.Katilimciler = [];
    EgKC.ekleKatilimci = function (repo, e) {
        e.preventDefault();
        repo.DurumId = null;
        repo.KayitDurumu = false;
        repo.DurumBilgisi = null;
        if (repo.hasOwnProperty("TcNo")) {
            if ((EgKC.Katilimciler.findIndex(x => x.TcNo === repo.TcNo)) === -1)
                if (EgKC.Katilimciler.length < 201) EgKC.Katilimciler.push(repo); else
                    alert("200 katılımcıdan fazla giremezsiniz!")
            else alert("Benzer kayıt giriyorsunuz!");
            EgKC.dersler['DerseKatilanlar'] = EgKC.Katilimciler;
        };
        EgKC.repo = null;
    };
    EgKC.removeRow = function (task) {
        var index = EgKC.Katilimciler.indexOf(task);
        EgKC.Katilimciler.splice(index, 1);
    };
    $scope.$watchCollection(function () { return EgKC.Katilimciler; }, function (newValue, oldValue) {
        EgKC.KatilimciSayisi = "Katılımcı Sayımız: " + newValue.length;
        EgKC.egitimYapisi.calisanObjects = [];
        angular.forEach(newValue, function (v) {//v =>item adı
            EgKC.egitimYapisi.calisanObjects.push(parseInt(v.TcNo));
        });
    });
    EgKC.TumunuKaydet = function () {
        $uibModal.open({
            templateUrl: 'KayitTuru.html',
            backdrop: true,
            animation: true,
            size: 'sm',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout) {
                this.$onInit = function () {//sayfa yüklemeden önce
                    if (angular.isUndefined(EgKC.IsgTopluEgitimi_Id) || EgKC.IsgTopluEgitimi_Id === null) {
                        if (EgKC.egitimTarihi > new Date()) $scope.DurumKayitlari = [{ 'durum': 'Planla', 'id': 2 }]; else
                            $scope.DurumKayitlari = [{ 'durum': 'Kaydet', 'id': 1 }, { 'durum': 'Bakanlığa Gönder', 'id': 3 },];
                    }
                    else
                        if (EgKC.egitimTarihi <= new Date(Date.now() + (3600 * 1000 * 24))) $scope.DurumKayitlari = [{ 'durum': 'Kaydet', 'id': 1 }];
                        else $scope.DurumKayitlari = [{ 'durum': 'Planla', 'id': 2 }];
                };
                var kayıtzamani = function () {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $scope.message = "";
                        $uibModalInstance.dismiss('cancel');
                    }, 2400);
                };
                $scope.NitelikliKayit = function (val) {
                    if (val === undefined || val === null) {
                        $scope.message = "Gönderim Tipini Seçiniz...";
                        return;
                    }
                    EgKC.dersler['status'] = val.id;
                    EgKC.dersler['nitelikDurumu'] = val.durum;
                    EgKC.egitimYapisi.egiticiTckNo = EgKC.profesyoneli.TcNo;
                    EgKC.egitimYapisi.isgProfTckNo = EgKC.profesyoneli.TcNo;
                    EgKC.dersler.IsgProfesyoneliAdi = EgKC.profesyoneli.adi;

                    if (angular.isUndefined(EgKC.IsgTopluEgitimi_Id) || EgKC.IsgTopluEgitimi_Id === null) {
                        $scope.message = "Kayıt yapılıyor! Bekleyiniz...";
                        $scope.messageIco = true;
                        egitimSvc.AddIsgTopluEgitimi({
                            'egitimYapisi': EgKC.egitimYapisi, 'dersler': EgKC.dersler, 'Sirket_id': $stateParams.id,
                            'Year': $stateParams.year, 'sorguNo': ''
                        }).then(function (resp) {
                            EgKC.egitimYapisi = JSON.parse(resp.IsgTopluEgitimiJson).egitimYapisi;
                            EgKC.dersler = JSON.parse(resp.IsgTopluEgitimiJson).dersler;
                            EgKC.Katilimciler = JSON.parse(resp.IsgTopluEgitimiJson).dersler.DerseKatilanlar;
                            EgKC.IsgTopluEgitimi_Id = resp.IsgTopluEgitimi_Id;
                            angular.forEach(EgKC.Katilimciler, function (v) {
                                v.DurumId = resp.IsgTopluEgitimi_Id;
                                v.KayitDurumu = true;
                            });
                            EgKC.dersler.DerseKatilanlar = EgKC.Katilimciler;
                        }).catch(function (e) {
                            $scope.message = e.data;
                            kayıtzamani();
                        }).finally(function () {
                            $scope.message = "Tamamlandı...";
                            $scope.messageIco = false;
                            kayıtzamani();
                        });
                    } else {
                        $scope.message = "Verileriniz Güncelleniyor! Bekleyiniz...";
                        $scope.messageIco = true;
                        egitimSvc.UpdateIsgTopluEgitimi(EgKC.IsgTopluEgitimi_Id, {
                            'egitimYapisi': EgKC.egitimYapisi, 'dersler': EgKC.dersler, 'Sirket_id': $stateParams.id,
                            'Year': $stateParams.year, 'sorguNo': ''
                        }).then(function (resp) {
                            EgKC.egitimYapisi = JSON.parse(resp.IsgTopluEgitimiJson).egitimYapisi;
                            EgKC.dersler = JSON.parse(resp.IsgTopluEgitimiJson).dersler;
                            EgKC.Katilimciler = JSON.parse(resp.IsgTopluEgitimiJson).dersler.DerseKatilanlar;
                            EgKC.IsgTopluEgitimi_Id = resp.IsgTopluEgitimi_Id;
                            angular.forEach(EgKC.Katilimciler, function (v) {
                                v.DurumId = resp.IsgTopluEgitimi_Id;
                                v.KayitDurumu = true;
                            });
                            EgKC.dersler.DerseKatilanlar = EgKC.Katilimciler;
                        }).catch(function (e) {
                            $scope.message = e.data;
                        }).finally(function () {
                            $scope.message = "Tamamlandı...";
                            $scope.messageIco = false;
                            kayıtzamani();
                        });
                    };
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
    };
    function isNullOrWhitespace(input) {
        return !input || !input.trim();
    }
    $scope.inspiniaTemplate = 'views/common/notify.html';
    function titleCase(str) {
        var splitStr = str.toLowerCase().split(' ');
        for (var i = 0; i < splitStr.length; i++) {
            splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
        }
        return splitStr.join(' ');
    }
    EgKC.MailAt = function (AdSoyad, Mail) {
        if (isNullOrWhitespace(Mail)) {
            return notify({
                message: 'Personelin Mail adresi olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
        mail.To.push({ address: Mail.trim(), displayName: AdSoyad });
        mail.SagId = PmSvc.pmb.SaglikBirimi_Id === undefined ? 1 : PmSvc.pmb.SaglikBirimi_Id;
        mail.Subject = 'ISG Eğitiminiz Planlaması Hakkında.(ISGplus)';
        mailSvc.GetMail('./views/Egitim/View/temp/EgMail.html', {
            xx: {
                AdSoyad: titleCase(AdSoyad),
                egitimYapisi: EgKC.egitimYapisi,
                dersler: EgKC.dersler
            }
        }, mail);
    };
    $scope.$watch(function () { return $rootScope.isBeingMailed; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            if (newValue === false) {
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
        }
    });
    EgKC.SMSAt = function (AdSoyad, Tel) {
        if (isNullOrWhitespace(Tel) || isNullOrWhitespace(EgKC.egitimYapisi.egitimTarihi) ||
            isNullOrWhitespace(EgKC.dersler.saat) || isNullOrWhitespace(EgKC.dersler.salon)) {
            return notify({
                message: 'Personelin eksik bilgileri nedeniyle gönderim yapamazsınız.(Saat salon tarih eğitici)',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        }
        var sms = {
            KisaMesaj: "Sn." + titleCase(AdSoyad) + " " + EgKC.egitimYapisi.egitimTarihi + " tarihinde saat " + EgKC.dersler.saat + " da "
                + EgKC.dersler.salon + " eğitim salonunda " + EgKC.dersler.IsgProfesyoneliAdi +
                " tarafından ISG Eğitimi verilecektir. Katılım yapamayacağınız takdirde ISG Birimine bilgilendirme yapmanız rica olunur.",
            Numaralar: [Tel.trim()]
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

    EgKC.KatilimFormu = function () {
        $uibModal.open({
            templateUrl: 'KatilimFormu.html',
            backdrop: true,
            animation: true,
            size: 'sm',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, printer, $filter) {
                $scope.sayfa = 1;
                var printSayfalari = [];
                var sayfaOlustur = function (adet) {
                    var yapi = { AdiSoyadi: '', SicilNo: '', Gorevi: '' };
                    var digerSayfalar = [];
                    for (var i = 0; i < 20; i++) {//sonraki sayfalar için
                        digerSayfalar.push(yapi);
                    }
                    if (adet > 0) {
                        i = 0;
                        while (i < parseInt(adet)) {
                            printSayfalari.push({ pg: digerSayfalar });
                            i++;
                        };
                    };
                    return printSayfalari;
                };
                $scope.Yazdir = function (unCheck) {
                    if (unCheck) {
                        var anaFormKatilanlari = [];
                        angular.forEach(EgKC.dersler.DerseKatilanlar, function (v) {
                            anaFormKatilanlari.push({ AdiSoyadi: v.AdiSoyadi, SicilNo: v.SicilNo, Gorevi: v.Gorevi })
                        });
                        anaFormKatilanlari = $filter('orderBy')(anaFormKatilanlari, 'AdiSoyadi');//alfabetik sıralamaya almak için                      
                        var basilacakSayfalar = anaFormKatilanlari.length > 11 ? Math.floor((anaFormKatilanlari.length - 11) / 20)+1 : 1;
                        sayfaOlustur(basilacakSayfalar);
                        var basSayfa = [];
                        for (var i = 0; i < 11; i++) {//ilk fayfa için
                            basSayfa.push({ AdiSoyadi: '', SicilNo: '', Gorevi: '' });
                        };
                        basSayfa.splice(0, anaFormKatilanlari.length, ...anaFormKatilanlari.splice(0, 11));

                        angular.forEach(printSayfalari, function (v, i) {
                                v.pg = [];
                                var digerSayfalar = [];
                                for (var i = 0; i < 20; i++) {
                                    digerSayfalar.push({ AdiSoyadi: '', SicilNo: '', Gorevi: '' });
                                };
                                v.pg = digerSayfalar;
                                v.pg.splice(0, anaFormKatilanlari.length > 20 ? 20 : anaFormKatilanlari.length, ...anaFormKatilanlari.splice(0, 20));
                        });
                    } else {
                        var basSayfa = [];
                        for (var i = 0; i < 11; i++) {
                            basSayfa.push({ AdiSoyadi: '', SicilNo: '', Gorevi: '' });
                        };
                        var digerSayfalar = [];
                        for (var i = 0; i < 20; i++) {
                            digerSayfalar.push({ AdiSoyadi: '', SicilNo: '', Gorevi: '' });
                        }
                        if ($scope.sayfa > 0) {
                            i = 0;
                            while (i < parseInt($scope.sayfa-1)) {
                                printSayfalari.push({ pg: digerSayfalar });
                                i++;
                            };
                        };
                    };
                    printer.print('./views/Egitim/View/temp/egitimKatilimFormu.html', {
                        personeller1: basSayfa, personeller2: printSayfalari,
                        dersler: EgKC.dersler, egitimYapisi: EgKC.egitimYapisi,
                        derslerinOzeti: EgKC.derslerinOzeti,
                        et: (EgKC.egitimYapisi.egitimYer === 1 ? "İş Yeri İçinde" : "İş Yeri Dışında"),
                        logo:picFile
                    });
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
    };

    EgKC.tumPersonelleriEkle = function () {
        $scope.messageIcoEkle = true;
        personellerSvc.SirketEgtPerListesi($stateParams.id).then(function (data) {
            if (data.PersonelCards.length < 201) {
                EgKC.Katilimciler = [];
                angular.forEach(data.PersonelCards, function (v, k) {
                    v.Photo = $scope.fileImgPath + v.Photo;
                    EgKC.Katilimciler.push(v);
                });
                EgKC.dersler['DerseKatilanlar'] = EgKC.Katilimciler;
            } else {
                alert("200 katılımcıdan fazla giremezsiniz!");
            };
        }).catch(function (e) {
            $scope.message = e.data;
        }).finally(function () {
            $scope.messageIcoEkle = false;
        });
    };
}
egtKayitCtrl.$inject = ['$scope', 'ngAuthSettings', '$q', 'notify', '$timeout',
    '$uibModal', '$rootScope','uploadService', '$stateParams', 'personellerSvc', 'sirketSvc', 'tanimlarSvc', 'egitimSvc', 'mailSvc', 'PmSvc'];

angular
    .module('inspinia')
    .controller('egtKayitCtrl', egtKayitCtrl);