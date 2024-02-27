'use strict';

function disRaporCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder, DTColumnDefBuilder, $filter
    , Upload, $window, uploadService, ngAuthSettings, SMSvc, $q) {
    var drc = this;
    drc.model = {
        DisRapor_Id: undefined, MuayeneTuru: undefined, Tani: undefined, BaslangicTarihi: undefined, BitisTarihi: undefined, SureGun: undefined, DoktorAdi: undefined, RaporuVerenSaglikBirimi: undefined,
        Personel_Id: undefined, Revir_Id: undefined, Sirket_Id: undefined, Bolum_Id: undefined
    };

    drc.options = {};

    drc.HastalikTanimiList = function (value) {
        var deferred = $q.defer();
        SMSvc.HastalikAra(value).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };
    function adjustDateForTimeOffset(dateToAdjust) {
        var offsetMs = dateToAdjust.getTimezoneOffset() * 60000;
        return new Date(dateToAdjust.getTime() - offsetMs);
    }



    drc.fields = [
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-2',
                    key: 'MuayeneTuru',
                    type: 'ui-select-single',
                    templateOptions: {
                        optionsAttr: 'bs-options',
                        ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                        label: 'Muayene Turu',
                        required: true,
                        valueProp: 'value',
                        labelProp: 'name',
                        placeholder: 'Seviye Seçiniz',
                        options: personellerSvc.SD.muayeneNedeni
                    }
                },
                {
                    className: 'col-sm-5',
                    type: 'input',
                    key: 'RaporuVerenSaglikBirimi',
                    templateOptions: {
                        type: 'input',
                        label: 'Kurumun Adı'
                    }
                },
                {
                    className: 'col-sm-5',
                    type: 'typhead',
                    key: 'Tani',
                    templateOptions: {
                        label: 'Hastalık',
                        placeholder: 'Hastalık Tanımı',
                        options: [],
                        onKeyup: function ($viewValue, $scope) {
                            if (typeof $viewValue !== 'undefined') {
                                return $scope.templateOptions.options = drc.HastalikTanimiList($viewValue);
                            }
                        }
                    }
                },

            ]
        },
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-4',
                    type: 'input',
                    key: 'DoktorAdi',
                    templateOptions: {
                        type: 'input',
                        label: 'Doktorun Adı ve Soyadı'
                    }
                },
                {
                    className: 'col-sm-3',
                    key: 'BaslangicTarihi',
                    type: 'datepicker',
                    templateOptions: {
                        label: 'Başlama Tarihi',
                        type: 'text',
                        required: true,
                        datepickerPopup: 'dd.MMMM.yyyy',
                        datepickerOptions: {
                            format: 'dd.MM.yyyy'
                        }
                    }
                },
                {
                    className: 'col-sm-3',
                    key: 'BitisTarihi',
                    type: 'datepicker',
                    templateOptions: {
                        label: 'Bitiş Tarihi',
                        type: 'text',
                        datepickerPopup: 'dd.MMMM.yyyy',
                        datepickerOptions: {
                            format: 'dd.MM.yyyy'
                        }
                    }
                },
                {
                    className: 'col-sm-2',
                    type: 'input',
                    key: 'SureGun',
                    templateOptions: {
                        type: 'number',
                        min: 0,
                        label: 'Gün Sayısı'
                    }
                }
            ]
        }
    ];

    drc.originalFields = angular.copy(drc.fields);

    if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
        $scope.DisRaporlari = personellerSvc.MoviesIds.personel.data.DisRaporlari;
    } else {
        personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
            personellerSvc.MoviesIds.personel = s;
            $scope.DisRaporlari = s.data.DisRaporlari;
        });
    }
    $scope.isCollapsed = true;
    $scope.dtOptionsdrc = DTOptionsBuilder.newOptions()
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
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Dis Raporlari Listesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Dis Raporlari Listesi', titleAttr: 'PDF' },
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
        .withOption('rowCallback', rowCallback)
        .withSelect(true)
        .withOption('order', [1, 'desc'])
        .withOption('responsive', true);

    $scope.dtColumnDefsdrc = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.$apply(function () {
                $scope.isCollapsed = false;
                var rs = $filter('filter')($scope.DisRaporlari, {
                    DisRapor_Id: aData[0]
                })[0];
                drc.model = {
                    DisRapor_Id: aData[0], MuayeneTuru: aData[2], Tani: aData[3], BaslangicTarihi: new Date(rs.BaslangicTarihi),
                    BitisTarihi: new Date(rs.BitisTarihi), SureGun: aData[2], DoktorAdi: aData[7], RaporuVerenSaglikBirimi: aData[8],
                    Personel_Id: rs.Personel_Id, Revir_Id: rs.Revir_Id, Sirket_Id: rs.Sirket_Id, Bolum_Id: rs.Bolum_Id
                };
                $scope.files = [];
                $scope.dosyaList = false;
                uploadService.GetFileId($stateParams.id, 'dis-rapor', aData[0]).then(function (response) {
                    $scope.files = response;
                    $scope.dosyaList = true;
                });
            });
        });
        return nRow;
    }

    drc.onSubmit = function () {
        drc.model.BaslangicTarihi = adjustDateForTimeOffset(drc.model.BaslangicTarihi );
        drc.model.BitisTarihi = adjustDateForTimeOffset(drc.model.BitisTarihi );
        drc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
        drc.model.Sirket_Id = personellerSvc.MoviesIds.SirketId;
        drc.model.Bolum_Id = personellerSvc.MoviesIds.BolumId;
        if (angular.isUndefined(drc.model.DisRapor_Id) || drc.model.DisRapor_Id === null) {//yeni Kayıt
            personellerSvc.AddPrsDisRapor(drc.model).then(function (response) {
                personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                    personellerSvc.MoviesIds.personel = s;
                    $scope.DisRaporlari = s.data.DisRaporlari;
                });
                $scope.yeniDRC();
            });
        } else {//düzenle
            personellerSvc.UpdatePrsDisRapor(drc.model.DisRapor_Id, drc.model).then(function (response) {
                personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                    personellerSvc.MoviesIds.personel = s;
                    $scope.DisRaporlari = s.data.DisRaporlari;
                });
                $scope.yeniDRC();
            });
        }
    };

    var deselect = function () {
        var table = $("#disRapor").DataTable();
        table
            .rows('.selected')
            .nodes()
            .to$()
            .removeClass('selected');

    };

    $scope.yeniDRC = function () {
        drc.model = {};
        $scope.dosyaList = false;
        $scope.files = []; deselect();
    };

    $scope.silDRC = function () {
        personellerSvc.DeletePrsDisRapor(drc.model.DisRapor_Id).then(function (response) {
            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                personellerSvc.MoviesIds.personel = s;
                $scope.DisRaporlari = s.data.DisRaporlari;
            });
            $scope.yeniDRC();
        });
    };

    function gunFarki(date1, date2) {
        var gun_toplami = 1000 * 60 * 60 * 24;
        var date1_as = date1.getTime();
        var date2_as = date2.getTime();
        var farki = Math.abs(date1_as - date2_as);
        return Math.round(farki / gun_toplami);
    }

    $scope.$watchCollection('[drc.model.BaslangicTarihi,drc.model.BitisTarihi]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            drc.model.SureGun = gunFarki(newValues[1], newValues[0]);
        }
    });

    $scope.files = [];

    $scope.dosyaList = false;

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/dis-rapor/' + drc.model.DisRapor_Id,
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

    $scope.$watch(function () { return drc.model.DisRapor_Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue) || newValue !== null) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });

}

disRaporCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc',
    'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter',
    'Upload', '$window', 'uploadService', 'ngAuthSettings', "SMSvc", "$q"];

angular
    .module('inspinia')
    .controller('disRaporCtrl', disRaporCtrl);