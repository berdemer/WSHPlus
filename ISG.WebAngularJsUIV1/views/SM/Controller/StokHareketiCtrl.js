'use strict';
function StokHareketiCtrl($scope, SMSvc, DTOptionsBuilder, $q, $filter, $timeout,
    DTColumnDefBuilder, ExcelSvc, ngAuthSettings, Upload, $window, uploadService) {

    $scope.exportData = function () {
        SMSvc.StokGirisiAl($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
            ExcelSvc.XlsxSave(data, "StokGirişi.xlsx", "StokGirisi", [0, 1, 2, 14, 15, 16,17]);
        });
    };
    $scope.exportDataMiyad = function () {
        SMSvc.StokGirisiAl($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
            ExcelSvc.XlsxSave($(data).filter(function (i, v) { return new Date() > new Date(v.MiadTarihi);}), "StokGirişiMiyad.xlsx", "StokGirisi", [0, 1, 2, 14, 15, 16,17]);
        });
    };
    $scope.StokExcelListesi = function (deger) {
        SMSvc.SBStokListeleri($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
            ExcelSvc.XlsxSave(deger === '' ? data : $(data).filter(function (i, v) {
                return v.StokTuru === deger;
            }), deger+"StokDurumu.xlsx", "StokDurumu", [0,3,4,5]);
        });
    };
    $scope.formatDate = function (date) {
        var dateOut = new Date(date);
        return dateOut;
    };
    var sh = this;
    sh.model = {};
    sh.options = {};
    sh.originalFields = angular.copy(sh.fields);
    sh.onSubmit = function () {
        sh.model.SaglikBirimi_Id = $scope.birim.selected.SaglikBirimi_Id;
        if (angular.isUndefined(sh.model.Id) || sh.model.Id === null) {//yeni Kayıt
            SMSvc.StokGirisiEkle(sh.model).then(function (response) {
            tabloCalistir();
            });
            $scope.yeni();
        } else {//düzenle
            SMSvc.StokGirisiGuncelle(sh.model.Id, sh.model).then(function (response) {
            tabloCalistir();
            });
        }
    };
    $scope.yeni = function () {
        sh.model = {};
        $scope.selectedRow = -1;
        $scope.files = [];
        $scope.dosyaList = false;
    };
    $scope.activ = function () {
        sh.model.SaglikBirimi_Id = $scope.birim.selected.SaglikBirimi_Id;
        SMSvc.StokGirisiSil(sh.model.Id, sh.model).then(function (response) {
            tabloCalistir();
            $scope.yeni();
            $scope.selectedRow = -1;
        });
    };
    $scope.stoktableClass = 'wrapper wrapper-content animated fadeInRight col-lg-4';
    $scope.stokgiristableClass = 'wrapper wrapper-content animated fadeInRight col-lg-8';   
    $scope.dahil = true;
    $scope.pasifize = $scope.dahil === true ? 'Aktive' : 'Pasifize';
    $scope.pasifizen = $scope.dahil === true ? 'Pasifize' : 'Aktive';
    $scope.dahilinde = function () {
        $scope.pasifize = $scope.dahil === true ? 'Aktive' : 'Pasifize';
        $scope.pasifizen = $scope.dahil === true ?'Pasifize':'Aktive';
        tabloCalistir();
        $scope.selectedRow = -1;
    };
    function tabloCalistir() {
        SMSvc.SBStokListeleri($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
            $scope.stoklar = data;
        });
        SMSvc.StokGirisiAl($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
            var yeni = [];
            angular.forEach(data, function (item) {
                item.KritikMiadUyarisi = new Date('0001-01-01') !== new Date(item.KritikMiadTarihi) ? new Date() > new Date(item.KritikMiadTarihi) : false;
                item.MiadUyarisi = new Date('0001-01-01') !== new Date(item.MiadTarihi) ? new Date() > new Date(item.MiadTarihi) && item.KritikMiadUyarisi : false;
                yeni.push(item);
            });
            $scope.stokGirisleri = yeni;
            $scope.totalItems = $scope.stokGirisleri.length;
        });
    }
    $scope.birim = { selected: '' };
    $scope.stok = {};
    $scope.dtOptions = DTOptionsBuilder.newOptions()
       .withLanguageSource('/views/Personel/Controller/Turkish.json')
       .withDOM('<"html5buttons"B>ltip')
       .withButtons([
           { extend: 'excel', title: 'StokListesi', name: 'Excel' }
       ])
       .withPaginationType('full')
       .withSelect(true)
       .withOption('lengthMenu', [3,10,20,50,100])
       .withOption('responsive', true);
    $scope.dtColumnDefs = [// 0. kolonu gizlendi.
       DTColumnDefBuilder.newColumnDef(0).notVisible()
    ];
    SMSvc.AllSbirimleri().then(function (data) {
        $scope.birimler = data;
        $scope.birim = { selected: data[0] };
    });
    $scope.$watch(function () { return $scope.birim.selected; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            $scope.stoklar = [];
            $scope.stokGirisleri = [];
            sh.model = {};
            $scope.birim.selected = newValue;
            SMSvc.SBStokListeleri(newValue.SaglikBirimi_Id, $scope.dahil).then(function (data) {
                $scope.stoklar = data;
                sh.fields = [
                {
                    className: 'row col-sm-12',
                    fieldGroup: [
                                {
                                    className: 'col-sm-4',
                                    key: 'StokId',
                                    type: 'ui-select-single',
                                    templateOptions: {
                                        label: 'Stok Adı',
                                        optionsAttr: 'bs-options',
                                        required: true,
                                        ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                        valueProp: 'StokId',
                                        labelProp: 'IlacAdi',
                                        options: $scope.stoklar
                                    }
                                },
                                {
                                    className: 'col-sm-3',
                                    key: 'MiadTarihi',
                                    type: 'datepicker',
                                    templateOptions: {
                                        label: 'Miad Tarihi',
                                        type: 'text',
                                        datepickerPopup: 'dd.MMMM.yyyy',
                                        datepickerOptions: {
                                            format: 'dd.MM.yyyy'
                                        }
                                    }
                                },
                                {
                                    className: 'col-sm-3',
                                    key: 'KritikMiadTarihi',
                                    type: 'datepicker',
                                    templateOptions: {
                                        label: 'Kritik Miad Tarihi',
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
                                    key: 'Maliyet',
                                    defaultValue: '0.00',
                                    templateOptions: {
                                        label: 'Maliyet',
                                        pattern: '^[+-]?[0-9]{1,}(?:[0-9]*(?:[.,][0-9]{1})?|(?:,[0-9]{3})*(?:\.[0-9]{1,2})?|(?:\.[0-9]{3})*(?:,[0-9]{1,2})?)$'
                                    }
                                }
                    ]
                },
                {
                    className: 'row col-sm-12',
                    fieldGroup: [
                            {
                                className: 'col-sm-2',
                                type: 'input',
                                defaultValue: 0,
                                key: 'KutuMiktari',
                                templateOptions: {
                                    type: 'number',
                                    min: 0,
                                    label: 'Kutu Sayısı'
                                }
                            },
                            {
                                className: 'col-sm-2',
                                type: 'input',
                                defaultValue: 0,
                                key: 'KutuIcindekiMiktar',
                                templateOptions: {
                                    type: 'number',
                                    min: 0,
                                    label: 'İçindeki Sayı'
                                }
                            },
                            {
                                className: 'col-sm-2',
                                type: 'input',
                                defaultValue: 0,
                                key: 'ToplamMiktar',
                                templateOptions: {
                                    type: 'number',
                                    min: 0,
                                    required: true,
                                    label: 'Toplam Miktar'
                                }
                            },
                            {
                                className: 'col-sm-1',
                                type: 'input',
                                defaultValue: 0,
                                key: 'ArtanMiadTelefMiktari',
                                templateOptions: {
                                    type: 'number',
                                    min: 0,
                                    label: 'Telef'
                                }
                            },
                            {
                                className: 'col-sm-2',
                                key: 'ArtanTelefNedeni',
                                type: 'select',
                                templateOptions: {
                                    label: 'Nedeni',
                                    options: [{ name: 'Miyad Aşımı', value: 'Miyad Aşımı' }, { name: 'Defarmasyon', value: 'Defarmasyon' },
                                        { name: 'Kırılma', value: 'Kırılma' }, { name: 'Arıza', value: 'Arıza' }, { name: 'Sevk Edildi', value: 'Sevk Edildi' }, { name: 'Diğer', value: 'Diğer' }],
                                    placeholder: 'Telef Nedeni'
                                },
                                expressionProperties: {
                                    'templateOptions.disabled': '!model.ArtanMiadTelefMiktari'
                                }
                            },
                            {
                                className: 'col-sm-3',
                                type: 'textarea',
                                key: 'StokEkBilgisi',
                                templateOptions: {
                                    label: 'Stok Ek Bilgi',
                                    rows: 2,
                                    placeholder: 'Demirbaş kalemlerinin marka model vs girebilirsiniz.'
                                }
                            }
                    ]
                }
                ];
            });
            SMSvc.StokGirisiAl(newValue.SaglikBirimi_Id, $scope.dahil).then(function (data) {
                var yeni = [];
                angular.forEach(data, function (item) {
                    item.KritikMiadUyarisi = item.KritikMiadTarihi !== '0001-01-01T00:00:00' ? new Date() > new Date(item.KritikMiadTarihi) : false;
                    item.MiadUyarisi = item.MiadTarihi !== '0001-01-01T00:00:00' ? new Date() > new Date(item.MiadTarihi) && item.KritikMiadUyarisi : false;
                    yeni.push(item);
                });
                $scope.stokGirisleri = yeni;
                $scope.totalItems = $scope.stokGirisleri.length;
            });
        }
    });
    $scope.selectedRow = -1;
    $scope.setClickedRow = function (index, stok) {
        $scope.selectedRow = index;
        sh.model = {
            Id: stok.Id, StokId: stok.StokId, SaglikBirimi_Id: stok.SaglikBirimi_Id, KutuIcindekiMiktar: stok.KutuIcindekiMiktar, ArtanTelefNedeni: stok.ArtanTelefNedeni, StokEkBilgisi: stok.StokEkBilgisi,
            KutuMiktari: stok.KutuMiktari, ToplamMiktar: stok.ToplamMiktar, MiadTarihi: new Date(stok.MiadTarihi), KritikMiadTarihi: new Date(stok.KritikMiadTarihi),
            ArtanMiadTelefMiktari: stok.ArtanMiadTelefMiktari, Tarih: stok.Tarih, Maliyet: stok.Maliyet, Status: $scope.dahil, SaglikBirimi_Id: $scope.birim.selected.SaglikBirimi_Id
        };
        if (stok.MiadUyarisi === true && stok.KritikMiadUyarisi === true) { $scope.Bilgi = stok.IlacAdi + ' Miad süresi dolmuştur.'; } else { $scope.Bilgi = ''; }
        if (stok.MiadUyarisi === false && stok.KritikMiadUyarisi === true) { $scope.Bilgi = stok.IlacAdi + ' Kritik Miad tarihi geçilmiş olabilir.'; }
        //**upload-download
        $scope.files = [];
        $scope.dosyaList = false;
        uploadService.GetFileId(stok.Id, 'stok-hareketleri', stok.Id).then(function (response) {
            $scope.files = response;
            $scope.dosyaList = true;
        });
        //***upload-download
    };
    $scope.qx = "";
    $scope.clearInput = function () {
        $scope.qx = "";
        $scope.totalItems = $scope.stokGirisleri.length;
    };
    $scope.viewby = 3;
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
    $scope.$watchCollection('[sh.model.KutuMiktari,sh.model.KutuIcindekiMiktar]', function (newValues) {
        if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
            sh.model.ToplamMiktar = newValues[1] * newValues[0];
        }
    });

    $scope.$watchCollection('[sh.model.MiadTarihi]', function (newValues) {
        if (!angular.isUndefined(newValues[0])) {
            sh.model.KritikMiadTarihi = ayCikar(newValues[0], 4);
        }
    });

    function ayCikar(date1, month1) {
        var jan312009 = new Date(date1);
        return new Date(new Date(jan312009).setMonth(jan312009.getMonth() - month1));
    }
    ///***upload-download bölümü
    $scope.files = [];
    $scope.dosyaList = false;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + sh.model.Id + '/stok-hareketleri/' + sh.model.Id,
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

    $scope.$watch(function () { return sh.model.Id; }, function (newValue, oldValue) {
        if (!angular.isUndefined(newValue) || newValue !== null) {
            $scope.fileButtonShow = true;
        } else { $scope.fileButtonShow = false; }
    });
    //***upload download bitimi($scope.yeni)($scope.setClickedRow)ilişkili kod yerleri
    //, ngAuthSettings, Upload, $window, uploadService referens alanları

}

StokHareketiCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', '$q', '$filter',
    '$timeout', 'DTColumnDefBuilder', 'ExcelSvc', 'ngAuthSettings', 'Upload', '$window', 'uploadService'];

angular
    .module('inspinia')
    .controller('StokHareketiCtrl', StokHareketiCtrl);