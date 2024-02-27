'use strict';

function psikolojikTestCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
    ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify, $uibModal, $log) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    $scope.fileButtonShow = false;
    $scope.testShow = false;
    $scope.$watch(function () { return ptC.model.PsikolojikTest_Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue)) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });


    TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

    var ptC = this;

    $scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

    ptC.protokolGirisi = $scope.opsiyonlar[0];

    $scope.files = [];

    $scope.dosyaList = false;

    ptC.model = {
    };

    ptC.options = {};

    ptC.fields = [
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-6',
                    key: 'TestAdi',
                    type: 'ui-select-single',
                    defaultValue: 'Normal',
                    templateOptions: {
                        label: 'Test Adı',
                        required: true,
                        optionsAttr: 'bs-options',
                        ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                        valueProp: 'z',
                        labelProp: 'v',
                        options: [{ v: 'Maslach Tükenmişlik Ölçeği', z: 'MaslachTuk' }, { v: 'Beck Depresyon Ölçeği', z: 'BeckDepresyon' }, { v: 'Beck Anksiyete Ölçeği', z: 'BeckAnksiyete' }, { v: 'Dikkat Dağınıklığı Testi', z: 'DikkatTesti' }]
                    }
                },
                {
                    className: 'col-sm-6'

                }
            ]
        },
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-9',
                    type: 'textarea',
                    key: 'Sonuc',
                    templateOptions: {
                        label: 'Sonuç',
                        required: true,
                        placeholder: ''
                    }
                }
            ]
        }

    ];

    ptC.originalFields = angular.copy(ptC.fields);

    if ($stateParams.id === TkSvc.Tk.guidId) {
        $scope.PsikolojikTestler = TkSvc.Tk.TkBilgi.data.PsikolojikTestler;
    }
    else {
        TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
            TkSvc.Tk.TkBilgi = s;
            TkSvc.Tk.guidId = $stateParams.id;
            $scope.PsikolojikTestler = s.data.PsikolojikTestler;
            $scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
            $scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
            $scope.Ozg = s;
            $scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
        });
    }

    $scope.isCollapsed = true;

    $scope.dtOptionsptC = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            {
                text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
                className: 'addButton', action: function (e, dt, node, config) {
                    $scope.$apply(function () {
                        $scope.isCollapsed = !$scope.isCollapsed;
                        $scope.dosyaList = !$scope.dosyaList;
                    });
                }
            },
            { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
            { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Tansiyon Listesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Tansiyon Listesi', titleAttr: 'PDF' },
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
        .withOption('order', [1, 'desc'])
        .withOption('lengthMenu', [3, 10, 20, 50, 100])
        .withOption('rowCallback', rowCallback)
        .withOption('responsive', true);

    $scope.dtColumnDefsptC = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];

    $scope.inspiniaTemplate = 'views/common/notify.html';

    ptC.onSubmit = function () {
        ptC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
        if (angular.isUndefined(ptC.model.PsikolojikTest_Id) || ptC.model.PsikolojikTest_Id === null) {
            if (TkSvc.Tk.SaglikBirimi_Id !== null) {
                ptC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
                TkSvc.AddPrsPsikolojikTest(ptC.model, TkSvc.Tk.SaglikBirimi_Id, ptC.protokolGirisi.value).then(function (response) {
                    TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
                        TkSvc.Tk.TkBilgi = s;
                        $scope.PsikolojikTestler = s.data.PsikolojikTestler;
                    });
                    $scope.yeniptC();
                });
            } else {
                notify({
                    message: 'Sağlık Birimini Girmeden Kayıt Yapamazsınız.',
                    classes: 'alert-danger',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 2000,
                    position: 'right'
                });
            }
        }
        else {
            TkSvc.UpdatePrsPsikolojikTest(ptC.model.PsikolojikTest_Id, ptC.model).then(function (response) {
                TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
                    TkSvc.Tk.TkBilgi = s;
                    $scope.PsikolojikTestler = s.data.PsikolojikTestler;
                });
                $scope.yeniptC();
            }).catch(function (e) {
                $scope.message = "Hata Kontrol Edin! " + e;
                startTimer();
            });
        }
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.message = "";
        }, 4000);
    };

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                $scope.isCollapsed = false;
                var rs = $filter('filter')($scope.PsikolojikTestler, {
                    PsikolojikTest_Id: aData[0]
                })[0];
                rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
                //rs.Tarih = new Date(rs.Tarih);
                ptC.model = rs;
                $scope.files = [];
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'pst', aData[0]).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                });
            });
            return nRow;
        });
    }

    var deselect = function () {
        var table = $("#pst").DataTable();
        table
            .rows('.selected')
            .nodes()
            .to$()
            .removeClass('selected');

    };

    $scope.yeniptC = function () {
        ptC.model = {}; $scope.files = [];
        $scope.dosyaList = false;
        deselect();
    };

    $scope.silptC = function () {
        TkSvc.DeletePrsPsikolojikTest(ptC.model.PsikolojikTest_Id).then(function (response) {
            TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
                TkSvc.Tk.TkBilgi = s;
                $scope.PsikolojikTestler = s.data.PsikolojikTestler;
            });
            $scope.yeniptC();
        }).catch(function (e) {
            $scope.message = "Hata Kontrol Edin! " + e;
            startTimer();
        });
    };

    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/pst/' + ptC.model.PsikolojikTest_Id,
                method: 'POST',
                data: {
                    file: dataUrl
                }
            }).then(function (response) {
                angular.forEach(response.data, function (data) {
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

    ptC.testAdiBul = function (item) {
        var testler = [{ v: 'Maslach Tükenmişlik Ölçeği', z: 'MaslachTuk' }, { v: 'Beck Depresyon Ölçeği', z: 'BeckDepresyon' }, { v: 'Beck Anksiyete Ölçeği', z: 'BeckAnksiyete' }, { v: 'Dikkat Dağınıklığı Testi', z: 'DikkatTesti' }];
        var index = testler.map(function (item) {
            return item.z;
        }).indexOf(item);
        return testler[index].v;
    };

    ptC.testYap = function () {
        //modelin açılışı
        var modalInstance = $uibModal.open({//Modal oluşturma...
            animation: true,
            templateUrl: 'ModalContent.html',
            controller: 'ModalCtrl',
            controllerAs: 'mC',
            size: 'lg',
            resolve: {
                items: function () {
                    return ptC.model.TestAdi;//parametre girişi buradan verilir.
                }
            }
        });
        modalInstance.result.then(function (sonuc) {
            ptC.model.Sonuc = sonuc;//ok ise dönecek yer
        }, function () {//iptal ise dönecek yer
                $log.info('Modal dismissed at: ' + new Date() );
        });
    };

}

psikolojikTestCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
    'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify', '$uibModal','$log'];


function ModalCtrl($scope, $uibModalInstance, items, TkSvc, $timeout) {
    var mC = this;
    var testler = [{ v: 'Maslach Tükenmişlik Ölçeği', z: 'MaslachTuk' }, { v: 'Beck Depresyon Ölçeği', z: 'BeckDepresyon' }, { v: 'Beck Anksiyete Ölçeği', z: 'BeckAnksiyete' }, { v: 'Dikkat Dağınıklığı Testi', z: 'DikkatTesti' }];
    var index = testler.map(function (item) {
        return item.z;
    }).indexOf(items);
    mC.items = testler[index].v;
    mC.leftBtnDisplay = true;
    mC.rightBtnDisplay = true;
    mC.testSorulari = [];
    mC.testSay = 0;
    mC.yorumSayfasi = true;
    var sorgula = function () {
        switch (items) {
            case 'MaslachTuk':
                TkSvc.MaslachTukenmislikOlcegi().then(function (resp) {
                    mC.testSorulari = resp;
                    mC.testSorulariSayisi = mC.testSorulari.length;
                    mC.test = mC.testSorulari[0];
                });
                break;
            case 'BeckDepresyon':
                TkSvc.BeckDepresyonOlcegi().then(function (resp) {
                    mC.testSorulari = resp;
                    mC.testSorulariSayisi = mC.testSorulari.length;
                    mC.test = mC.testSorulari[0];
                });
                break;
            case 'BeckAnksiyete':
                TkSvc.BeckAnksiyeteOlcegi().then(function (resp) {
                    mC.testSorulari = resp;
                    mC.testSorulariSayisi = mC.testSorulari.length;
                    mC.test = mC.testSorulari[0];
                });
                break;
            case 'DikkatTesti':
                TkSvc.DikkatDaginikligiTesti().then(function (resp) {
                    mC.testSorulari = resp;
                    mC.testSorulariSayisi = mC.testSorulari.length;
                    mC.test = mC.testSorulari[0];
                });
                break;
        }
        mC.leftBtnDisplay = false;
    };
    sorgula();
    mC.yorum = "Test Değerlendirelemedi.";
    mC.leftSend = function () {
        mC.testSay = mC.testSay - 1;
        mC.leftBtnDisplay = mC.testSay === 0 ? false : true;
        mC.rightBtnDisplay = mC.testSay === mC.testSorulariSayisi ? false : true;
        mC.test = mC.testSorulari[mC.testSay];
        mC.yorumSayfasi = true;
    };
    mC.rightSend = function () {
        mC.testSay = mC.testSay + 1;
        mC.leftBtnDisplay = mC.testSay === 0 ? false : true;
        mC.rightBtnDisplay = mC.testSay === mC.testSorulariSayisi ? false : true;
        mC.test = mC.testSorulari[mC.testSay];
        mC.yorumSayfasi = mC.testSay === mC.testSorulariSayisi ? false : true;
        if (mC.testSay === mC.testSorulariSayisi) {
            if (items === "MaslachTuk") mC.yorum = mC.MaslachTukenmislikOlcegi();
            if (items === "BeckDepresyon") mC.yorum = mC.BeckDepresyonOlcegi();
            if (items === "BeckAnksiyete") mC.yorum = mC.BeckAnksiyeteOlcegi();
            if (items === "DikkatTesti") mC.yorum = mC.DikkatDaginikligiTesti();
        }
    };
    mC.yalin = function (str) {
        switch (str) {
            case 'a':
                mC.testSorulari[mC.testSay].b.active = "false";
                mC.testSorulari[mC.testSay].c.active = "false";
                mC.testSorulari[mC.testSay].d.active = "false";
                mC.testSorulari[mC.testSay].e.active = "false";
                mC.testSorulari[mC.testSay].f.active = "false";
                break;
            case 'b':
                mC.testSorulari[mC.testSay].a.active = "false";
                mC.testSorulari[mC.testSay].c.active = "false";
                mC.testSorulari[mC.testSay].d.active = "false";
                mC.testSorulari[mC.testSay].e.active = "false";
                mC.testSorulari[mC.testSay].f.active = "false";
                break;
            case 'c':
                mC.testSorulari[mC.testSay].a.active = "false";
                mC.testSorulari[mC.testSay].b.active = "false";
                mC.testSorulari[mC.testSay].d.active = "false";
                mC.testSorulari[mC.testSay].e.active = "false";
                mC.testSorulari[mC.testSay].f.active = "false";
                break;
            case 'd':
                mC.testSorulari[mC.testSay].a.active = "false";
                mC.testSorulari[mC.testSay].b.active = "false";
                mC.testSorulari[mC.testSay].c.active = "false";
                mC.testSorulari[mC.testSay].e.active = "false";
                mC.testSorulari[mC.testSay].f.active = "false";
                break;
            case 'e':
                mC.testSorulari[mC.testSay].a.active = "false";
                mC.testSorulari[mC.testSay].b.active = "false";
                mC.testSorulari[mC.testSay].c.active = "false";
                mC.testSorulari[mC.testSay].d.active = "false";
                mC.testSorulari[mC.testSay].f.active = "false";
                break;
            case 'f':
                mC.testSorulari[mC.testSay].a.active = "false";
                mC.testSorulari[mC.testSay].b.active = "false";
                mC.testSorulari[mC.testSay].c.active = "false";
                mC.testSorulari[mC.testSay].d.active = "false";
                mC.testSorulari[mC.testSay].e.active = "false";
                break;
        }
    };

    mC.MaslachTukenmislikOlcegi = function () {
        var Duygusal_Tükenmislik = 0;
        var Duyarsızlasma = 0;
        var Kisisel_Basarisizlik = 0;
        var DT = [1, 2, 3, 6, 8, 13, 14, 16, 20];
        var DY = [5, 10, 11, 15, 22];
        var KB = [4, 7, 9, 12, 17, 18, 19, 21];
        var i = 1;
        angular.forEach(mC.testSorulari, function (v) {
            if (DT.indexOf(i) > -1) {
                if (v.a.active === true)
                    Duygusal_Tükenmislik = Duygusal_Tükenmislik + parseInt(v.a.val);
                if (v.b.active === true)
                    Duygusal_Tükenmislik = Duygusal_Tükenmislik + parseInt(v.b.val);
                if (v.c.active === true)
                    Duygusal_Tükenmislik = Duygusal_Tükenmislik + parseInt(v.c.val);
                if (v.d.active === true)
                    Duygusal_Tükenmislik = Duygusal_Tükenmislik + parseInt(v.d.val);
                if (v.e.active === true)
                    Duygusal_Tükenmislik = Duygusal_Tükenmislik + parseInt(v.e.val);
            }
            /////////////////////////////////////////////////////////////
            if ((v.a.active === true) && (DY.indexOf(i) > -1))
                Duyarsızlasma = Duyarsızlasma + parseInt(v.a.val);
            if ((v.b.active === true) && (DY.indexOf(i) > -1))
                Duyarsızlasma = Duyarsızlasma + parseInt(v.b.val);
            if ((v.c.active === true) && (DY.indexOf(i) > -1))
                Duyarsızlasma = Duyarsızlasma + parseInt(v.c.val);
            if ((v.d.active === true) && (DY.indexOf(i) > -1))
                Duyarsızlasma = Duyarsızlasma + parseInt(v.d.val);
            if ((v.e.active === true) && (DY.indexOf(i) > -1))
                Duyarsızlasma = Duyarsızlasma + parseInt(v.e.val);
            ///////////////////////////////////////////////////////////////
            if ((v.a.active === true) && (KB.indexOf(i) > -1))
                Kisisel_Basarisizlik = Kisisel_Basarisizlik + parseInt(v.a.val);
            if ((v.b.active === true) && (KB.indexOf(i) > -1))
                Kisisel_Basarisizlik = Kisisel_Basarisizlik + parseInt(v.b.val);
            if ((v.c.active === true) && (KB.indexOf(i) > -1))
                Kisisel_Basarisizlik = Kisisel_Basarisizlik + parseInt(v.c.val);
            if ((v.d.active === true) && (KB.indexOf(i) > -1))
                Kisisel_Basarisizlik = Kisisel_Basarisizlik + parseInt(v.d.val);
            if ((v.e.active === true) && (KB.indexOf(i) > -1))
                Kisisel_Basarisizlik = Kisisel_Basarisizlik + parseInt(v.e.val);
            i = i + 1;
        });
        return "Duygusal Tükenmişlik Değeri: %" + percentageCalculator(Duygusal_Tükenmislik ,9)+
            " Duyarsızlaştırma: %" + percentageCalculator(Duyarsızlasma,5) + " Kişisel Başarısızlık Durumu: %"
            + percentageCalculator(Kisisel_Basarisizlik,8);
    };

    function percentageCalculator(deger, sayı) {
        return (deger/(sayı * 4) * 100).toFixed(2)
    }

    mC.BeckDepresyonOlcegi = function () {
        var puanSkoru = 0;
        var degerlendirme = "";
        angular.forEach(mC.testSorulari, function (v) {
            if (v.a.active === true)
                puanSkoru = puanSkoru + parseInt(v.a.val);
            if (v.b.active === true)
                puanSkoru = puanSkoru + parseInt(v.b.val);
            if (v.c.active === true)
                puanSkoru = puanSkoru + parseInt(v.c.val);
            if (v.d.active === true)
                puanSkoru = puanSkoru + parseInt(v.d.val);
        });
        if (10 > puanSkoru) degerlendirme = "Minimal depresyon";
        if ((9 < puanSkoru) && (17 > puanSkoru)) degerlendirme = "Hafif depresyon";
        if ((16 < puanSkoru) && (30 > puanSkoru)) degerlendirme = "Orta depresyon";
        if ((29 < puanSkoru) && (64 > puanSkoru)) degerlendirme = "Şiddetli depresyon";
        return degerlendirme;
    };

    mC.BeckAnksiyeteOlcegi = function () {
        var puanSkoru = 0;
        var degerlendirme = "";
        angular.forEach(mC.testSorulari, function (v) {
            if (v.a.active === true)
                puanSkoru = puanSkoru + parseInt(v.a.val);
            if (v.b.active === true)
                puanSkoru = puanSkoru + parseInt(v.b.val);
            if (v.c.active === true)
                puanSkoru = puanSkoru + parseInt(v.c.val);
            if (v.d.active === true)
                puanSkoru = puanSkoru + parseInt(v.d.val);
        });
        if (8 > puanSkoru) degerlendirme = "Normal";
        if ((7 < puanSkoru) && (16 > puanSkoru)) degerlendirme = "Hafif anksiyete ";
        if ((15 < puanSkoru) && (26 > puanSkoru)) degerlendirme = "Orta anksiyete ";
        if ((25 < puanSkoru) && (64 > puanSkoru)) degerlendirme = "Şiddetli anksiyete ";
        return degerlendirme;
    };

    mC.DikkatDaginikligiTesti = function () {
        var puanSkoru = 0;
        var degerlendirme = "";
        angular.forEach(mC.testSorulari, function (v) {
            if (v.a.active === true)
                puanSkoru = puanSkoru + parseInt(v.a.val);
            if (v.b.active === true)
                puanSkoru = puanSkoru + parseInt(v.b.val);
            if (v.c.active === true)
                puanSkoru = puanSkoru + parseInt(v.c.val);
            if (v.d.active === true)
                puanSkoru = puanSkoru + parseInt(v.d.val);
            if (v.e.active === true)
                puanSkoru = puanSkoru + parseInt(v.e.val);
            if (v.f.active === true)
                puanSkoru = puanSkoru + parseInt(v.f.val);
        });
        if (49 > puanSkoru) degerlendirme = "Dikkat Eksikliği Yok.";
        if ((50 < puanSkoru) && (70 > puanSkoru)) degerlendirme = "Şüpheli Dikkat Eksikliği";
        if (69 < puanSkoru) degerlendirme = "Dikkat Eksikliği Olabilir.";
        return degerlendirme;
    };

    $scope.ok = function () {
        startTimer();
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $uibModalInstance.close(mC.yorum);//modelin kapanışına değer gönderir.
        }, 1500);
    };

}

ModalCtrl.$inject = ['$scope', '$uibModalInstance', 'items', 'TkSvc', '$timeout'];


angular
    .module('inspinia')
    .controller('psikolojikTestCtrl', psikolojikTestCtrl)
    .controller('ModalCtrl', ModalCtrl);