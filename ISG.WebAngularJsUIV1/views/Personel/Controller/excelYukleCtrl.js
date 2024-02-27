'use strict';
function excelYukleCtrl($scope, Upload, $timeout, ngAuthSettings, personellerSvc, $filter) {

    $scope.$watch('rows', function (val) {
        var results = $filter('filter')($scope.rows, {
            durum: true
        });
        $scope.selectTotalItems = results.length === 0 ? "" : results.length + " personeli seçtiniz...";
    }, true);

    $scope.q = "";

    $scope.clearInput = function () {
        $scope.totalItems = $scope.rows.length;
        $scope.queryData = [
        ];
        $scope.q = "";
        if (isNaN($scope.aramaSonlandir)) {
            $scope.aramaSonlandir = "Çok teşekkürler çok iyisiniz.. :D";
            $timeout(function () {
                $scope.aramaSonlandir = "";
            }, 4000);
        }
    };

    $scope.Karsilastima = false;
    $scope.tabloGoster = false;
    $scope.viewby = 5;
    //$scope.totalItems = $scope.rows.length;
    $scope.currentPage = 1;
    $scope.itemsPerPage = $scope.viewby;
    $scope.maxSize = 2;
    $scope.pageStartUp = function () {
        $scope.currentPage = 1;
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
        // $scope.currentPage = 1;
    };
    $scope.isCollapsed = true;
    $scope.isCollapsed2 = true;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    $scope.SheetNames = {
    };
    $scope.SheetInfo = {
        fileNames: '',
        sheet: '',
        HDR: true
    };
    var apiPath = function () {
        return ngAuthSettings.isAzure ? 'api/azurExcelDepo/upload' : 'api/laboratuar/upload';
    };
    $scope.isAzure = ngAuthSettings.isAzure;
    $scope.uploadPic = function (file) {
        Upload.upload({
            url: serviceBase + apiPath(),
            data: {
                file: file
            }
        }).then(function (response) {
            $timeout(function () {
                $scope.isCollapsed = false;
                file.result = response.data;
                $scope.SheetNames = response.data.SheetNames;
                $scope.SheetInfo.fileNames = response.data.fileName;
            });
        }, function (response) {
            if (response.status > 0)
                $scope.errorMsg = response.status + ': ' + response.data;
        });
    };

    $scope.getAllExcelData = function (SheetInfo) {
        personellerSvc.GetExcelData(SheetInfo).then(function (data) {
            $scope.tabloGoster = true;
            $scope.rows = data;
            $scope.cols = Object.keys($scope.rows[0]);
            $scope.totalItems = $scope.rows.length;
        });
    };

    $scope.user = {
        status: ''
    };
    $scope.GeneralPersonelData = {
    };



    $scope.GeneralPersonelDataEkle = function (oldDat, newDat) {
        for (var prop in $scope.GeneralPersonelData) {//data içindeki propertiesler alınır.
            if ($scope.GeneralPersonelData.hasOwnProperty(prop)) {//key ayıklanır
                if ($scope.GeneralPersonelData[prop] === newDat) {//değerler karşılaştırılır
                    delete $scope.GeneralPersonelData[prop];//varsa silinir.
                }
            }
        }
        $scope.GeneralPersonelData[oldDat] = newDat;
    };

    $scope.AlanGet = function () {
        personellerSvc.AlanGet().then(function (data) {
            var keys = [
            ]; //key lerden zincir oluşturuldu.
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    keys.push(key);
                }
            }
            $scope.GPDkey = keys;
        });
    };
    $scope.AlanGet();
    $scope.showStatus = function () {
        var selected = $filter('filter')($scope.GPDkey, {
            value: $scope.user.status
        });
        return $scope.user.status && selected.length ? selected[0].text : null;
    };
    /////////////sirketler ve bölümler
    $scope.SirketBasligi = {
        id: 0,
        name: 'Şirketi Seçiniz.!'
    };
    $scope.BolumBasligi = {
        id: 0,
        name: 'Bölümü Seçiniz.!'
    };
    personellerSvc.GetSirketler(true).then(function (data) {
        $scope.collectionSirket = data;
    });
    $scope.status = {
        open: false,
        open2: false
    };
    $scope.showSelected = function (sel) {
        $scope.SirketBasligi.name = sel.name;
        $scope.SirketBasligi.id = sel.id;
        $scope.BolumBasligi = {
            id: 0,
            name: 'Bölümü Seçiniz.!'
        };
        $scope.status.open = !$scope.status.open;
        if (sel.id > 0)
            $scope.status.open2 = !$scope.status.open2;
        personellerSvc.GetSirketBolumleri(true, sel.id).then(function (data) {
            $scope.collectionBolum = data;
        });
    };
    $scope.showSelectedBolum = function (sel2) {
        $scope.BolumBasligi.name = sel2.name;
        $scope.BolumBasligi.id = sel2.id;
        $scope.status.open2 = !$scope.status.open2;
    };
    $scope.getData = function (rows, q) { //searc aranan liste farklı alana kaydedilir.
        $scope.queryData = $filter('filter')(rows, q);
        $scope.totalItems = $scope.queryData.length;
    };
    $scope.checkboxClick = function (bool) {
        if (bool === false) {
            angular.forEach($scope.rows, function (data) {
                data.durum = false;
            });
        } else {
            if (isNaN($scope.q)) {
                angular.forEach($scope.queryData, function (data) { //her data id si
                    var result = $filter('filter')($scope.rows, {
                        $id: data.$id
                    })[0]; // row degeri bulunur
                    result.durum = bool; // değiştirilir
                });
            } else {
                angular.forEach($scope.rows, function (data) {
                    data.durum = true;
                });
            }
        }
    };
    /*
  [dataları göndermek için yazıldı.]
  */
    $scope.currentPage2 = 1;
    $scope.maxSize2 = 2;
    $scope.viewby2 = 3;
    $scope.itemsPerPage2 = $scope.viewby2;

    $scope.pageStartUp2 = function () {
        $scope.currentPage2 = 1;
    };
    $scope.setPage2 = function (pageNo) {
        $scope.currentPage2 = pageNo;
    };
    $scope.setItemsPerPage2 = function (num) {
        $scope.itemsPerPage2 = num;
    };

    $scope.KayitGoster = false;

    $scope.Send = function () {
        $scope.surecBilgisi = "Lütfen Bekleyin....";
        var results = $filter('filter')($scope.rows, {
            durum: true//seçili olanlar result aktarılır.
        });
        var PrsList = [];
        angular.forEach(results, function (data) { //secilen her data web api uygun hale getirilir.
            var newData = {};//yeni object neslesi oluşturuldu.
            newData.id = data.$id;
            angular.forEach($scope.GeneralPersonelData, function (value, key) {
                newData[key] = data[value.toString()];
            });
            newData.Sirket = $scope.SirketBasligi.name;
            newData.Sirket_Id = $scope.SirketBasligi.id;
            newData.Bolum = $scope.BolumBasligi.name;
            newData.Bolum_id = $scope.BolumBasligi.id;
            PrsList.push(newData);
        });
        var SetPrsList = [];
        personellerSvc.AddPrsList(PrsList).then(function (response) {
            $scope.Karsilastima = false;
            $scope.KayitGoster = true;
            angular.forEach(response.data, function (value) {
                SetPrsList.push(value);
                if (value.durum === true) {
                    var index = $scope.rows.map(function (item) {
                        return item.$id;//json taramasında kullanılır
                    }).indexOf(value.Donus_Id);
                    $scope.rows.splice(index, 1);
                }
            });
            $scope.satirlar = SetPrsList;
            $scope.kolonlar = Object.keys($scope.satirlar[0]);
            $scope.totalItems2 = $scope.satirlar.length;
            })
       .finally(function () {
           $scope.totalItems = $scope.rows.length;
           $scope.surecBilgisi = "";
           $scope.aramaSonlandir ="<<=== Arama kutusunu boşaltın..!";
       });
    };


    $scope.Extract = function () {
        var PrsList = [];
        $scope.surecBilgisi = "Lütfen Bekleyin....";
        if ($scope.GeneralPersonelData !== {}) {
            angular.forEach($scope.rows, function (data) {
                var newData = {};
                newData.id = data.$id;

                angular.forEach($scope.GeneralPersonelData, function (value, key) {
                    newData[key] = data[value.toString()];
                });
                PrsList.push(newData);
            });
            personellerSvc.GetExtractPersoneller(PrsList).then(function (response) {
                angular.forEach(response, function (value) {
                    if (value.durum === true) {
                        var index = $scope.rows.map(function (item) {
                            return item.$id;
                        }).indexOf(value.Donus_Id);
                        $scope.rows.splice(index, 1);
                    }
                });
            })
       .finally(function () {
           $scope.totalItems = $scope.rows.length;
           $scope.surecBilgisi = "Ayıklama Başarılı bir şekilde tamamlandı.";
           $timeout(function () {
               $scope.surecBilgisi = "";
           }, 4000);
       });
        } else {
            $scope.surecBilgisi = "Sutunlardan Tc No yu belirleyiniz...";
            $timeout(function () {
                $scope.surecBilgisi = "";
            }, 4000);
        }
    };


    $scope.Update = function () {
        $scope.surecBilgisi = "Lütfen Bekleyin....";
        var results = $filter('filter')($scope.rows, {
            durum: true//seçili olanlar result aktarılır.
        });
        var PrsList = [];
        angular.forEach(results, function (data) { //secilen her data web api uygun hale getirilir.
            var newData = {};//yeni object neslesi oluşturuldu.
            newData.id = data.$id;
            angular.forEach($scope.GeneralPersonelData, function (value, key) {
                newData[key] = data[value.toString()];
            });
            newData.Sirket = $scope.SirketBasligi.name;
            newData.Sirket_Id = $scope.SirketBasligi.id;
            newData.Bolum = $scope.BolumBasligi.name;
            newData.Bolum_id = $scope.BolumBasligi.id;
            PrsList.push(newData);
        });
        var SetPrsList = [];
        personellerSvc.UpdatePrsList(PrsList).then(function (response) {
            $scope.Karsilastima = false;
            $scope.KayitGoster = true;
            angular.forEach(response.data, function (value) {
                SetPrsList.push(value);
                if (value.durum === true) {
                    var index = $scope.rows.map(function (item) {
                        return item.$id;//json taramasında kullanılır
                    }).indexOf(value.Donus_Id);
                    $scope.rows.splice(index, 1);
                }
            });
            $scope.satirlar = SetPrsList;
            $scope.kolonlar = Object.keys($scope.satirlar[0]);
            $scope.totalItems2 = $scope.satirlar.length;
        })
            .finally(function () {
                $scope.totalItems = $scope.rows.length;
                $scope.surecBilgisi = "";
                $scope.aramaSonlandir = "<<=== Arama kutusunu boşaltın..!";
            });
    };

}
excelYukleCtrl.$inject = [
  '$scope',
  'Upload',
  '$timeout',
  'ngAuthSettings',
  'personellerSvc',
  '$filter'
];
angular.module('inspinia').controller('excelYukleCtrl', excelYukleCtrl);

