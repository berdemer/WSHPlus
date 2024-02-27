'use strict';
function OgCtrl($scope, $state, $stateParams, OgSvc, ngAuthSettings, PmSvc, $location, $uibModal, IkazSvc, mailSvc, DTOptionsBuilder, DTColumnDefBuilder) {
    var ogC = this;
    $scope.lokal = $location.hash();
    if (!angular.isUndefined($stateParams.id)) {
        OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
            OgSvc.Oz.OzBilgi = s;
            OgSvc.Oz.guidId = $stateParams.id;
            $scope.guid = $stateParams.id;
            //$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
            $scope.fileImgPath = ngAuthSettings.isAzure ? ngAuthSettings.storageLinkService + 'personel/' : ngAuthSettings.storageLinkService + uploadFolder;
            $scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
            $scope.Ozg = s;
            ogC.bilgi = s;
            $scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
            ogC.bilgi = PmSvc.prmb.PeriyodikMuayeneAktarimi === true && OgSvc.Oz.guidId === PmSvc.prmb.guidId ? true : false;
            ogC.warning = s.data.Ikazlar.length > 0 ? true : false;
            ogC.warningLength = s.data.Ikazlar.length;
        });
    }
    $scope.formatDate = function (date) {
        return date === null ? null : new Date(date);
    };

    ogC.Ikaz = function (tumu) {
        $uibModal.open({
            templateUrl: './views/SM/View/temp/Ikaz.html',
            backdrop: true,
            animation: true,
            size: 'lg',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout, OgSvc, $filter) {
                var startTimer2 = function (sure) {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $uibModalInstance.dismiss('cancel');
                        $scope.bilgilendirme = "";
                    }, sure);
                };
                $scope.bilgilendirme = "";
                var sn = [];
                IkazSvc.GetTumIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                    sn = s;
                    $scope.Ikazlar = tumu === false ? OgSvc.Oz.OzBilgi.data.Ikazlar : sn;
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
                            extend: 'print', text: '<i class="fa fa-print"></i>', titleAttr: 'Yazdýr',
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
                    Personel_Id: OgSvc.Oz.OzBilgi.data.Personel_Id,
                    Status: true,
                    UserId: null
                };

                var sd = function (sx) {
                    $scope.Ikazlar = sx;
                    OgSvc.Oz.OzBilgi.data.Ikazlar = sx;
                    ogC.warning = sx.length > 0 ? true : false;
                    ogC.warningLength = sx.length;
                    startTimer2(3000);
                };

                $scope.remove = function () {
                    var sx = [];
                    if ($scope.uyari.Ikaz_Id !== null) {
                        IkazSvc.IkazSil($scope.uyari.Ikaz_Id).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Uyarýnýz Sistemden Kaldýrýldý.Formunuz kapatýlýyor.";
                        });
                    }
                };

                $scope.yeni = function () {
                    $scope.uyari =
                    {
                        Ikaz_Id: null,
                        IkazText: null,
                        SonucIkazText: null,
                        MuayeneTuru: "Özgeçmiþ Bilgisi",
                        SonTarih: null,
                        Tarih: new Date(),
                        Personel_Id: OgSvc.Oz.OzBilgi.data.Personel_Id,
                        Status: true,
                        UserId: null
                    };
                };
                $scope.save = function () {
                    var sx = [];
                    if (angular.isUndefined($scope.uyari.Ikaz_Id) || $scope.uyari.Ikaz_Id === null) {
                        IkazSvc.IkazEkle($scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            var datestring = $scope.uyari.SonTarih.getDate() + "/" + ($scope.uyari.SonTarih.getMonth() + 1) + "/" + $scope.uyari.SonTarih.getFullYear();
                            $scope.bilgilendirme = datestring + " Tarihli uyarýnýz sisteme eklendi.Formunuz kapatýlýyor.";
                        });
                    } else {
                        IkazSvc.IkazGuncelle($scope.uyari.Ikaz_Id, $scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(OgSvc.Oz.OzBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Güncellemeniz Yapýldý ve Eklendi.Formunuz kapatýlýyor.";
                        });
                    }
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.perSMSI = function () {
                    if (isNullOrWhitespace(OgSvc.Oz.OzBilgi.data.Telefon)) {
                        return notify({
                            message: 'Personelin Cep Telefonu olmadýðý için gönderim yapamazsýnýz.',
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
                    var sms = { KisaMesaj: $scope.uyari.IkazText.trim(), Numaralar: [OgSvc.Oz.OzBilgi.data.Telefon.trim()] };
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

OgCtrl.$inject = ['$scope', '$state', '$stateParams', 'OgSvc', 'ngAuthSettings',
    'PmSvc', '$location', '$uibModal', 'IkazSvc', 'mailSvc', 'DTOptionsBuilder',
    'DTColumnDefBuilder'];

angular
    .module('inspinia')
    .controller('OgCtrl', OgCtrl);