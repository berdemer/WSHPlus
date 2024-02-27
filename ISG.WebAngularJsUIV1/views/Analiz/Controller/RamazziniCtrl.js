'use strict';

function RamazziniCtrl($scope, personellerSvc, $state, analizSvc, $q, ngAuthSettings, $stateParams) {
    var rmzC = this;
    rmzC.ramaClass = 'wrapper wrapper-content animated fadeInRight col-sm-7';
    rmzC.bolumClass = 'wrapper wrapper-content animated fadeInRight col-sm-5';
    personellerSvc.MoviesIds.BolumId = 0;
    personellerSvc.MoviesIds.SirketId = 0;
    personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
    personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';

    var fileImgPath = function () {
        return ngAuthSettings.storageLinkService + (ngAuthSettings.isAzure ? 'ramazzini/Ramazzini.png' : ngAuthSettings.uploadFolder + 'Ramazzini.png');
    };

    rmzC.fileImgPath = fileImgPath();

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
        addToAllNodes(data);
        $scope.expandList = allNodes;
    });

    var allNodes = [];
    function addToAllNodes(children) {
        if (!children || Array.isArray(children) && children.length === 0) {
            return;
        }
        for (var i = 0; i < children.length; i++) {
            allNodes.push(children[i]);
            addToAllNodes(children[i].children);
        }
    }

    $scope.status = {
        open: false,
        open2: false
    };

    $scope.showSelected = function (sel) {
        $scope.SirketBasligi.name = sel.name;
        $scope.SirketBasligi.id = sel.id;
        personellerSvc.MoviesIds.SirketId = sel.id;//ŞİRKETİN ID BİLGİSİ
        personellerSvc.MoviesIds.SirketAdi = sel.name;
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
        personellerSvc.MoviesIds.BolumId = sel2.id;//bölümün Id bilgisi
        personellerSvc.MoviesIds.BolumAdi = sel2.name;
        $scope.status.open2 = !$scope.status.open2;
    };

    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        $scope.SirketBasligi.name = personellerSvc.MoviesIds.SirketAdi;
        $scope.SirketBasligi.id = personellerSvc.MoviesIds.SirketId;
        personellerSvc.GetSirketBolumleri(true, newValue).then(function (data) {
            $scope.collectionBolum = data;
        });
        rmzC.BolumRisk = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        if (angular.isUndefined(newValue)) {
            $scope.SirketBasligi = {
                id: 0,
                name: 'Şirketi Seçiniz.!'
            };
        }
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
        $scope.BolumBasligi.Bolum_Id = personellerSvc.MoviesIds.BolumId;
        $scope.BolumBasligi.name = personellerSvc.MoviesIds.BolumAdi;
        if (angular.isUndefined(newValue)) {
            $scope.BolumBasligi = {
                id: 0,
                name: 'Bölümü Seçiniz.!'
            };
            rmzC.BolumRisk = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        } else {
            analizSvc.BolumRiski($scope.SirketBasligi.id, newValue).then(function (resp) {
                rmzC.BolumRisk = resp.blmRiski === null ? rmzC.BolumRisk : resp.blmRiski;
            });
        }
    });

    analizSvc.GroupList().then(function (response) {
        rmzC.gruplarListesi = response;
    });

    rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
    rmzC.BolumRisk = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };

    $scope.$watch(function () { return rmzC.grubu; }, function (newValue, oldValue) {
        if (newValue !== oldValue && newValue.length > 2) {
            rmzC.islerSearch = null; rmzC.grupSearch = null;
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
            analizSvc.GroupSelect(newValue).then(function (response) {
                rmzC.Analiz.gruplistesi.push(response.grup);
                rmzC.Analiz.isler = response.isler;
                rmzC.Analiz.meslekHastaliklari = response.meslekHastaliklari;
            });
        }
        if (newValue === "") {
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        }
    });

    $scope.$watch(function () { return rmzC.grupSearch; }, function (newValue, oldValue) {
        if (newValue !== oldValue && newValue.length > 2) {
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
            analizSvc.GroupSearch(newValue).then(function (response) {
                rmzC.Analiz.gruplistesi = response.gruplistesi;
                rmzC.Analiz.isler = response.isler;
                rmzC.Analiz.meslekHastaliklari = response.meslekHastaliklari;
            });
        }
        if (newValue === "") {
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        }
    });

    $scope.$watch(function () { return rmzC.islerSearch; }, function (newValue, oldValue) {
        if (newValue !== oldValue && newValue.length > 2) {
            rmzC.grubu = null; rmzC.hastalikSearch = null;
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
            analizSvc.Isler(newValue).then(function (response) {
                var qx = [];
                angular.forEach(response.gruplistesi, function (value, key) {
                    this.push(value.grubu);
                }, qx);
                rmzC.Analiz.gruplistesi = qx;
                rmzC.Analiz.isler = response.isler;
                rmzC.Analiz.meslekHastaliklari = response.meslekHastaliklari;
            });
        }
        if (newValue === "") {
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        }
    });

    $scope.$watch(function () { return rmzC.hastalikSearch; }, function (newValue, oldValue) {
        if (newValue !== oldValue && newValue.length > 2) {
            rmzC.grubu = null; rmzC.islerSearch = null;
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
            analizSvc.MeslekHastaliklari(newValue).then(function (response) {
                var qx = [];
                angular.forEach(response.gruplistesi, function (value, key) {
                    this.push(value.grubu);
                }, qx);
                rmzC.Analiz.gruplistesi = qx;
                rmzC.Analiz.isler = response.isler;
                rmzC.Analiz.meslekHastaliklari = response.meslekHastaliklari;
            });
        }
        if (newValue === "") {
            rmzC.Analiz = { meslekHastaliklari: [], isler: [], gruplistesi: [], grup: null };
        }
    });

    rmzC.aktar = function (obj, val) {
        if ($scope.SirketBasligi.id > 0 && $scope.BolumBasligi.id) {
            var index = null;
            switch (true) {
                case val === 'isler':
                    index = rmzC.Analiz.isler.indexOf(obj);
                    rmzC.Analiz.isler.splice(index, 1);
                    rmzC.BolumRisk.isler.push(obj);
                    save();
                    break;
                case val === 'meslekHastaliklari':
                    index = rmzC.Analiz.meslekHastaliklari.map(function (item) {
                        return item.meslekHastalik;
                    }).indexOf(obj.meslekHastalik);
                    rmzC.Analiz.meslekHastaliklari.splice(index, 1);
                    rmzC.BolumRisk.meslekHastaliklari.push(obj);
                    save();
                    break;
                case val === 'grup':
                    index = rmzC.Analiz.gruplistesi.indexOf(obj);
                    rmzC.Analiz.gruplistesi.splice(index, 1);
                    rmzC.BolumRisk.gruplistesi.push(obj);
                    save();
                    break;
                default:
            }
        }

    };

    rmzC.BolumRiskRemove = function (obj, val) {
        var index = null;
        switch (val) {
            case 'gruplistesi':
                index = rmzC.BolumRisk.gruplistesi.indexOf(obj);
                rmzC.BolumRisk.gruplistesi.splice(index, 1);
                save();
                break;
            case 'isler':
                index = rmzC.BolumRisk.isler.indexOf(obj);
                rmzC.BolumRisk.isler.splice(index, 1);
                save();
                break;
            case 'meslekHastaliklari':
                index = rmzC.BolumRisk.meslekHastaliklari.map(function (item) {
                    return item.meslekHastalik;
                }).indexOf(obj.meslekHastalik);
                rmzC.BolumRisk.meslekHastaliklari.splice(index, 1);
                save();
                break;
        }
    };

    var save = function () {
        if ($scope.SirketBasligi.id > 0 && $scope.BolumBasligi.id) {
            var blmRisk = { PMJson: JSON.stringify(rmzC.BolumRisk), Sirket_Id: $scope.SirketBasligi.id, Bolum_Id: $scope.BolumBasligi.id };
            analizSvc.BolumRiskiAdd(blmRisk).then(function (resp) {
                rmzC.bRiski = resp;
            });
        }
    };

    if (!angular.isUndefined($stateParams.sti) && !angular.isUndefined($stateParams.blm)) {
        analizSvc.BolumRiski($stateParams.sti, $stateParams.blm).then(function (resp) {
            $scope.SirketBasligi = {
                id: $stateParams.sti,
                name: $stateParams.sAdi
            };
            $scope.BolumBasligi = {
                id: $stateParams.blm,
                name: $stateParams.bAdi
            };
            rmzC.BolumRisk = resp.blmRiski === null ? rmzC.BolumRisk : resp.blmRiski;
        });
    }
}

RamazziniCtrl.$inject = ['$scope', 'personellerSvc', '$state', 'analizSvc', '$q', 'ngAuthSettings', '$stateParams'];
angular
    .module('inspinia')
    .controller('RamazziniCtrl', RamazziniCtrl);