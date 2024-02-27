'use strict';

function isgEgitimiCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder, DTColumnDefBuilder, $filter
    , Upload, $window, uploadService, ngAuthSettings, tanimlarSvc, SMSvc) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var iec = this;

    if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
        $scope.IsgEgitimleri = personellerSvc.MoviesIds.personel.data.IsgEgitimleri;
    } else {
        personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
            personellerSvc.MoviesIds.personel = s;
            $scope.IsgEgitimleri = s.data.IsgEgitimleri;
        });
    }

    iec.model = {
        Egitim_Id: undefined, Egitim_Turu: undefined, Tanimi: undefined, Egitim_Suresi: undefined,
        VerildigiTarih: undefined, 
        Personel_Id: undefined, IsgEgitimiVerenPersonel: undefined
    };

    iec.options = {};

    var pdt = []; var isgUsers = []; var birimler = [];

    SMSvc.AllSbirimleri().then(function (data) {
        angular.forEach(data, function (s) {
            birimler.push({ name: s.SirketAdi.trim() + ' ' + s.SaglikBirimiAdi, value: s.SaglikBirimi_Id })
        });
    });

    tanimlarSvc.GetTanimAdiList("Personel Eğitim Tablosu").then(function (response) {
        angular.forEach(response, function (value) { pdt.push({ name: value.ifade.trim(), value: value.ifade.trim() }); });
    });
    personellerSvc.GetFullIsgUsers().then(function (r) {
        angular.forEach(r, function (value) {
            isgUsers.push({ userId: value.Id.trim(), adi: value.FullName.trim(), gorevi: value.Gorevi !== undefined ? value.Gorevi.trim() : '', meslegi: value.Meslek !== undefined ? value.Meslek.trim() : '' });
        });
    });
    $scope.yukleme = function () {
        iec.fields = [
            {
                className: 'row col-sm-12',
                fieldGroup: [
                    {
                        className: 'col-sm-7',
                        key: 'Egitim_Turu',
                        type: 'ui-select-single',
                        templateOptions: {
                            optionsAttr: 'bs-options',
                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                            label: 'Genel Tanımı',
                            required: true,
                            valueProp: 'value',
                            labelProp: 'name',
                            options: pdt
                        }
                    },
                    {
                        className: 'col-sm-5',
                        key: 'Tanimi',
                        type: 'input',
                        templateOptions: {
                            type: 'input',
                            label: 'Eğitim Türü',
                            required: true,
                        }
                    },
                ]
            },
            {
                className: 'row col-sm-12',
                fieldGroup: [
                    {
                        className: 'col-sm-2',
                        key: 'Egitim_Suresi',
                        type: 'input',
                        templateOptions: {
                            label: 'Egitim Süresi(Dakika)',
                            type: 'number',
                            min: 0,
                            required: true,
                        }
                    },
                    {
                        className: 'col-sm-4',
                        key: 'VerildigiTarih',
                        type: 'datepicker',
                        templateOptions: {
                            label: 'Verildiği Tarih',
                            type: 'text',
                            datepickerPopup: 'dd.MMMM.yyyy',
                            datepickerOptions: {
                                format: 'dd.MM.yyyy'
                            },
                            required: true,
                        }
                    },
                    {
                        className: 'col-sm-6',
                        key: 'IsgEgitimiVerenPersonel',
                        type: 'ui-select-single',
                        templateOptions: {
                            optionsAttr: 'bs-options',
                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                            label: 'İsg Egitimi Veren Personel',
                            required: true,
                            valueProp: 'adi',
                            labelProp: 'adi',
                            options: isgUsers
                        }
                    }
                ]
            },
        ];
        iec.originalFields = angular.copy(iec.fields);
    };
    $scope.isCollapsed = true;
    $scope.dtOptionsiec = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            {
                text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
                className: 'addButton', action: function (e, dt, node, config) {
                    $scope.$apply(function () {
                        $scope.isCollapsed = !$scope.isCollapsed;
                    });
                }
            },
            { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
            { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Isg Egitimleri Listesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Isg Egitimleri Listesi', titleAttr: 'PDF' },
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
        .withOption('rowCallback', rowCallback)
        .withOption('responsive', window.innerWidth < 1500 ? true : false);

    $scope.dtColumnDefsiec = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible(),
        DTColumnDefBuilder.newColumnDef(1).notVisible(),
    ];

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                $scope.isCollapsed = false;
                var rs = $filter('filter')($scope.IsgEgitimleri, {
                    Egitim_Id: aData[0]
                })[0];
                iec.model = {
                    Egitim_Id: aData[0], Egitim_Turu: aData[2], Tanimi: rs.Tanimi, Egitim_Suresi: rs.Egitim_Suresi, 
                    VerildigiTarih: new Date(rs.VerildigiTarih), 
                    Personel_Id: rs.Personel_Id, IsgEgitimiVerenPersonel: rs.IsgEgitimiVerenPersonel
                };
                $scope.files = [];
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'isg-egitimi', aData[0]).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                });
            });
        });
        return nRow;
    }

    iec.onSubmit = function () {
        iec.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
        if (angular.isUndefined(iec.model.Egitim_Id) || iec.model.Egitim_Id === null) {
            //personellerSvc.AddPrsIsgEgitimi(iec.model).then(function (response) {
            //    personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
            //        personellerSvc.MoviesIds.personel = s;
            //        $scope.IsgEgitimleri = s.data.IsgEgitimleri;
            //    });
            //    $scope.yeniiec();
            //});
            alert("Yeni kayıt girişi yapamazsınız.Toplu Eğitim bölümünden giriş yapınız. ");
        } else {
            personellerSvc.UpdatePrsIsgEgitimi(iec.model.Egitim_Id, iec.model).then(function (response) {
                personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                    personellerSvc.MoviesIds.personel = s;
                    $scope.IsgEgitimleri = s.data.IsgEgitimleri;
                });
                $scope.yeniiec();
            });
        }
    };

    var deselect = function () {
        var table = $("#isgEgitimi").DataTable();
        table
            .rows('.selected')
            .nodes()
            .to$()
            .removeClass('selected');
    };

    $scope.yeniiec = function () {
        iec.model = {};
        $scope.dosyaList = false;
        $scope.files = []; deselect();
    };

    $scope.siliec = function () {
        personellerSvc.DeletePrsIsgEgitimi(iec.model.Egitim_Id).then(function (response) {
            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                personellerSvc.MoviesIds.personel = s;
                $scope.IsgEgitimleri = s.data.IsgEgitimleri;
            });
            $scope.yeniiec();
        });
    };

    $scope.files = [];

    $scope.dosyaList = false;



    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + 'api/Img/' + $stateParams.id + '/IsgEgitimi/' + iec.model.Egitim_Id,
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

    $scope.$watch(function () { return iec.model.Egitim_Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue) || newValue !== null) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });

    function ayEkle(date1, month1) {
        var jan312009 = new Date(date1);
        return new Date(new Date(jan312009).setMonth(jan312009.getMonth() + month1));
    }

    $scope.$watch('[iec.model.VerildigiTarih,iec.model.Guncelleme_Suresi_Ay]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            var mnt = ayEkle(newValues[0], newValues[1]);
            iec.model.GuncellenecekTarih = mnt;
        }
    });
}

isgEgitimiCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc',
    'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter',
    'Upload', '$window', 'uploadService', 'ngAuthSettings', 'tanimlarSvc', 'SMSvc'];

angular
    .module('inspinia')
    .controller('isgEgitimiCtrl', isgEgitimiCtrl);