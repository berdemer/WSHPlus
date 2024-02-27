'use strict';
function adressUpdateCrtl($scope, personellerSvc, DTOptionsBuilder,
    $stateParams, $filter, DTColumnDefBuilder) {
    $scope.place = null;
    var adrs = this;
    adrs.model = { Adres_Id: null, Personel_Id: null };
    adrs.options = {};

    adrs.onSubmit = function () {
        adrs.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
        if (angular.isUndefined(adrs.model.Adres_Id) || adrs.model.Adres_Id == null) {//yeni Kayıt
            personellerSvc.AddPrsAddress(adrs.model).then(function (response) {
                personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                    personellerSvc.MoviesIds.personel = s;
                    $scope.Adress = s.data.Adresler;
                });
                $scope.yeni();
            });
        } else {//düzenle
            personellerSvc.UpdatePrsAddress(adrs.model.Adres_Id, adrs.model).then(function (response) {
                personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                    personellerSvc.MoviesIds.personel = s;
                    $scope.Adress = s.data.Adresler;
                });
                $scope.yeni();
            });
        }
    };

    $scope.isCollapsed = true;

    $scope.dtOptions = DTOptionsBuilder.newOptions()
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
            { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Adres Listesi', titleAttr: 'Excel 2010' },
            { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Adres Listesi', titleAttr: 'PDF' },
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

    $scope.dtColumnDefs = [// 0. kolonu gizlendi.
        DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];

    if ($stateParams.id == personellerSvc.MoviesIds.guidId) {
        $scope.Adress = personellerSvc.MoviesIds.personel.data.Adresler;
    } else {
        personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
            personellerSvc.MoviesIds.personel = s;
            $scope.Adress = s.data.Adresler;
        });
    }

    function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
        $('td', nRow).unbind('click');
        $('td', nRow).bind('click', function () {
            $scope.isCollapsed = false;
            $scope.$apply(function () {
                adrs.model.Adres_Id = aData[0];
                adrs.model.Adres_Turu = aData[1];
                adrs.model.GenelAdresBilgisi = aData[2];
                adrs.model.EkAdresBilgisi = aData[3];
                adrs.model.MapLokasyonu = aData[4];
            });
        });
        return nRow;
    }

    adrs.fields = [
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-6',
                    key: 'Adres_Turu',
                    type: 'ui-select-single',
                    templateOptions: {
                        label: 'Adres Türü',
                        optionsAttr: 'bs-options',
                        ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                        valueProp: 'value',
                        labelProp: 'name',
                        options: [{ name: 'Ev Adresi', value: 'Ev Adresi' }, { name: 'Yazlık Adresi', value: 'Yazlık Adresi' }, { name: 'İş Adresi', value: 'İş Adresi' }]
                    }
                },
                {
                    className: 'col-sm-6',
                    type: 'input',
                    key: 'MapLokasyonu',
                    templateOptions: {
                        label: 'Harita Lokasyonu'
                    }
                }
            ]
        },
        {
            className: 'row col-sm-12',
            fieldGroup: [
                {
                    className: 'col-sm-6',
                    type: 'textarea',
                    key: 'GenelAdresBilgisi',
                    templateOptions: {
                        label: 'Genel Adres Bilgisi',
                        required: true, rows: 3
                    },
                    watcher: {
                        listener: function (field, newValue, oldValue, formScope, stopWatching) {
                            formScope.model.GenelAdresBilgisi = formScope.model.GenelAdresBilgisi.toLocaleUpperCase('tr-TR');
                        }
                    }
                },
                {
                    className: 'col-sm-6',
                    type: 'textarea',
                    key: 'EkAdresBilgisi',
                    templateOptions: {
                        label: 'Ek Adres Bilgisi', rows: 3
                    },
                    watcher: {
                        listener: function (field, newValue, oldValue, formScope, stopWatching) {
                            formScope.model.EkAdresBilgisi = formScope.EkAdresBilgisi.Soyadi.toLocaleUpperCase('tr-TR');
                        }
                    }
                }
            ]
        }
    ];

    adrs.originalFields = angular.copy(adrs.fields);

    $scope.zx = function () {
        adrs.model.MapLokasyonu = document.getElementById('info').innerHTML;
        adrs.model.GenelAdresBilgisi = document.getElementById('address').innerHTML;
    };

    $scope.codexAddress = function () {
        codeAddress(adrs.model.GenelAdresBilgisi);
    };

    var deselect = function () {
        var table = $("#adress").DataTable();
        table
            .rows('.selected')
            .nodes()
            .to$()
            .removeClass('selected');

    };

    $scope.yeni = function () {
        adrs.model.Adres_Id = null;
        adrs.model.Adres_Turu = '';
        adrs.model.GenelAdresBilgisi = '';
        adrs.model.EkAdresBilgisi = '';
        adrs.model.MapLokasyonu = '';
        deselect();
    };

    $scope.sil = function () {
        personellerSvc.DeletePrsAddress(adrs.model.Adres_Id).then(function (response) {
            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
                personellerSvc.MoviesIds.personel = s;
                $scope.Adress = s.data.Adresler;
            });
            $scope.yeni();
        });
    };

}
adressUpdateCrtl.$inject = ['$scope', 'personellerSvc', 'DTOptionsBuilder',
    '$stateParams', '$filter', 'DTColumnDefBuilder'];

angular
    .module('inspinia')
    .controller('adressUpdateCrtl', adressUpdateCrtl);