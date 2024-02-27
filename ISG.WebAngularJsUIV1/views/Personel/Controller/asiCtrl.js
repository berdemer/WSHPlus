'use strict';

function asiCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
    ngAuthSettings, uploadService, authService, OgSvc, $window) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var asic = this;

    $scope.files = [];

    $scope.dosyaList = false;

    asic.model = {

    };

    asic.options = {};

    asic.fields = [
        {
            className: 'col-sm-3',
            key: 'Asi_Tanimi',
            type: 'select',
            templateOptions: {
                label: 'Aşı',
                required: true,
                options: [
                      { name: 'Tetanoz, difteri (Td)', value: 'Tetanoz, difteri (Td)' }
                    , { name: 'Sinovac', value: 'Sinovac' }
                    , { name: 'Biontec', value: 'Biontec' }
                    , { name: 'Turkovac', value: 'Turkovac' }
                    , { name: 'Kızamık (K) / Kızamık, kabakulak,kızamıkçık (KKK)', value: 'Kızamık (K) / Kızamık, kabakulak,kızamıkçık (KKK)' }, { name: 'Hepatit B', value: 'Hepatit B' }, { name: 'İnfluenza(Grip)', value: 'İnfluenza(Grip)' }
                    ,{ name: 'Pnömokok (polisakkarid)', value: 'Pnömokok (polisakkarid)' }, { name: 'Hepatit A', value: 'Hepatit A' }, { name: 'Suçiçeği(Varisella)', value: 'Suçiçeği(Varisella)' }, { name: 'Meningokok', value: 'Meningokok' }
                    , { name: 'Polio', value: 'Polio' }, { name: 'Sarı Humma', value: 'Sarı Humma' }, { name: 'Tifo', value: 'Tifo' }, { name: 'Kolera', value: 'Kolera' }, { name: 'Kene kaynaklı ensefalit', value: 'Kene kaynaklı ensefalit' }, { name: 'Zoster Aşısı', value: 'Zoster Aşısı' }
                    , { name: 'Kızamık, Kızamıkçık, Kabakulak Aşısı (MMR)', value: 'Kızamık, Kızamıkçık, Kabakulak Aşısı (MMR)' }

                ]
            }
        },
        {
            className: 'col-sm-1',
            key: 'Dozu',
            type: 'input',
            templateOptions: {
                type: 'input',
                label: 'Dozu'
            }
        },
        {
            className: 'col-sm-2',
            key: 'Yapilma_Tarihi',
            type: 'datepicker',
            templateOptions: {
                label: 'Tarihi',
                type: 'text',
                datepickerPopup: 'dd.MMMM.yyyy',
                datepickerOptions: {
                    format: 'dd.MM.yyyy'
                }
            }
        },
        {
            className: 'col-sm-1',
            key: 'Guncelleme_Suresi_Ay',
            type: 'input',
            templateOptions: {
                label: 'Ay',
                type: 'number',
                min: 0
            }
        },
        {
            className: 'col-sm-2',
            key: 'Muhtamel_Tarih',
            type: 'datepicker',
            templateOptions: {
                label: 'Rapel',
                type: 'text',
                datepickerPopup: 'dd.MMMM.yyyy',
                datepickerOptions: {
                    format: 'dd.MM.yyyy'
                }
            }
        },
        {
            className: 'col-sm-3',
            type: 'textarea',
            key: 'Aciklama',
            templateOptions: {
                label: 'Açıklama',
                rows: 2
            }
        }
    ];

    asic.originalFields = angular.copy(asic.fields);

    if ($stateParams.id === OgSvc.Oz.guidId) {
        $scope.Asilar = OgSvc.Oz.OzBilgi.data.Asilar;
    }
    else {
        OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
            OgSvc.Oz.OzBilgi = s;
            $scope.Asilar = s.data.Asilar;
            OgSvc.Oz.guidId = $stateParams.id;
            $scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
            $scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
            $scope.Ozg = s;
            $scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
        });
    }

    $scope.isCollapsed = true;

    $scope.dtOptionsasic = DTOptionsBuilder.newOptions()
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
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'AşılarListesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'AşılarListesi', titleAttr: 'PDF' },
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
        .withPaginationType('full_numbers')
        .withSelect(true)
        .withOption('order', [1, 'desc'])
        .withOption('lengthMenu', [3, 10, 20, 50, 100])
        .withOption('rowCallback', rowCallback)
        .withOption('responsive', true);

    $scope.dtColumnDefsasic = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];

    asic.onSubmit = function () {
        asic.model.Personel_Id = OgSvc.Oz.OzBilgi.data.Personel_Id;
        if (angular.isUndefined(asic.model.Asi_Id) || asic.model.Asi_Id == null) {
            OgSvc.AddPrsAsi(asic.model).then(function (response) {
                OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
                    OgSvc.Oz.OzBilgi = s;
                    $scope.Asilar = s.data.Asilar;
                });
                $scope.yeniasic();
            });
        }
        else {
            OgSvc.UpdatePrsAsi(asic.model.Asi_Id, asic.model).then(function (response) {
                OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
                    OgSvc.Oz.OzBilgi = s;
                    $scope.Asilar = s.data.Asilar;
                });
                $scope.yeniasic();
            });
        }
    };

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                $scope.isCollapsed = false;
                var rs = $filter('filter')($scope.Asilar, {
                    Asi_Id: aData[0]
                })[0];
                rs.Asi_Tanimi = rs.Asi_Tanimi.trim();
                rs.Yapilma_Tarihi = new Date(rs.Yapilma_Tarihi);
                rs.Muhtamel_Tarih = new Date(rs.Muhtamel_Tarih);
                asic.model = rs;
                $scope.files = [];
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'asi', aData[0]).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                });
            });
            return nRow;
        });
    }

    var deselect = function () {
        var table = $("#asi").DataTable();
        table
            .rows('.selected')
            .nodes()
            .to$()
            .removeClass('selected');

    };

    $scope.yeniasic = function () {
        asic.model = {}; $scope.files = [];
        $scope.dosyaList = false;
        deselect();
    };

    $scope.silasic = function () {
        OgSvc.DeletePrsAsi(asic.model.Asi_Id).then(function (response) {
            OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
                OgSvc.Oz.OzBilgi = s;
                $scope.Asilar = s.data.Asilar;
            });
            $scope.yeniasic();
        });
    };

    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            console.log(dataUrl);
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/asi/' + asic.model.Asi_Id,
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
            if (response.data == 1) { $scope.files.splice(index, 1); }
        });
    };

    $scope.download = function (genericName) {
        debugger;
        if (genericName.indexOf('.') < 1) {
            $scope.RenderFile(genericName);
        } else {
            window.open(serviceBase + '/uploads/' + genericName, 'popUpWindow', 'height=400,width=600,left=10,top=10,,scrollbars=yes,menubar=no')
        };
    };

    $scope.RenderFile = function (genericName) {
        var s = serviceBase + ngAuthSettings.apiUploadService + 'down/' + genericName;
        $window.open(s, '_self', '');
    };

    $scope.$watch(function () { return asic.model.Asi_Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue) || newValue != null) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });

    $scope.$watchCollection('[asic.model.Yapilma_Tarihi,asic.model.Guncelleme_Suresi_Ay]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            var mnt = ayEkle(newValues[0], newValues[1]);
            asic.model.Muhtamel_Tarih = mnt;
        }
    });

    function ayEkle(date1, month1) {
        var jan312009 = new Date(date1);
        return new Date(new Date(jan312009).setMonth(jan312009.getMonth() + month1));
    }

}

asiCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
    'ngAuthSettings', 'uploadService', 'authService', 'OgSvc', '$window'];

angular
    .module('inspinia')
    .controller('asiCtrl', asiCtrl);