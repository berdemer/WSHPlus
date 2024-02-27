'use strict';
function SMHeadListCtrl($scope, personellerSvc, $state) {

    personellerSvc.MoviesIds.BolumId = 0;
    personellerSvc.MoviesIds.SirketId = 0;
    personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
    personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';

    $scope.dahil = true;
    $scope.dahilinde = function () {
        personellerSvc.MoviesIds.dahil = $scope.dahil;
    };

    $scope.tumListe = function () {
        $state.go('Personel.PersonellerFullListDetail', { sd: true });
    };

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
        if (!children || typeof children == "array" && children.length == 0) {
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
        }
    });
}

function SMBodyListCtrl($scope, personellerSvc, DTOptionsBuilder, $state, $document, $stateParams) {
    var vm = this;

    $scope.alerti = function (asd) { alert(asd); };

    $scope.dtOptions = DTOptionsBuilder.newOptions()
       .withLanguageSource('/views/Personel/Controller/Turkish.json')
       .withDOM('<"html5buttons"B>lTfgitp')
       .withButtons([
           { extend: 'copy', text: 'Kopyala' },
           { extend: 'csv' },
           { extend: 'excel', title: 'PersonelListesi', name: 'Excel' },
           { extend: 'pdf', title: 'PersonelListesi', name: 'PDF' },
           {
               extend: 'print', text: 'Yazdır',
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
        .withOption('responsive', window.innerWidth < 1500 ? true : false);
    var genel = true;

    if (!angular.isUndefined($stateParams.sti) || !angular.isUndefined($stateParams.blm)) {
        personellerSvc.GetBolumPersonelleri($stateParams.blm, $stateParams.sti, genel).then(function (data) {
            $scope.persons = data;
        });
        personellerSvc.MoviesIds.BolumId = $stateParams.blm == "" || $stateParams.blm == null ? 0 : $stateParams.blm;
        personellerSvc.MoviesIds.SirketId = $stateParams.sti == "" || $stateParams.sti == null? 0 : $stateParams.sti;
        personellerSvc.MoviesIds.SirketAdi = $stateParams.sti == "" || $stateParams.sti == null ? "Şirketi Seçiniz.!" : $stateParams.stiAd;
        personellerSvc.MoviesIds.BolumAdi = $stateParams.blm == "" || $stateParams.blm == null ? "Bölümü Seçiniz.!" : $stateParams.blmAd;
    }

    if (!angular.isUndefined($stateParams.sd)) {
        if ($stateParams.sd == "true") {
            personellerSvc.GetAllPersList(true).then(function (data) {
                $scope.persons = data;
            });
        }
        if ($stateParams.sd == "false") {
            personellerSvc.GetAllPersList(false).then(function (data) {
                $scope.persons = data;
            });
        }
    }

    $scope.$watch(function () { return personellerSvc.MoviesIds.dahil; }, function (newValue, oldValue) {
        if (newValue !== oldValue) genel = newValue;
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            personellerSvc.GetSirketPersonelleri(newValue, genel).then(function (data) {
                $scope.persons = data;
            });
        }
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            personellerSvc.GetBolumPersonelleri(newValue, personellerSvc.MoviesIds.SirketId, genel).then(function (data) {
                $scope.persons = data;
            });
        }
    });

    $scope.xedit = function (element) {
        var id = element.attributes['id'].value;
        $state.go('Personel.Pers.GenericUpdate', { id: id });
    };
    $scope.xpicture = function (element) {
        var id = element.attributes['id'].value;
        personellerSvc.GetPicture(id);
    };
    $scope.xdelete = function (element) {
        var id = element.attributes['id'].value;
        personellerSvc.DeletePrs(id).then(function () {
            personellerSvc.GetBolumPersonelleri(personellerSvc.MoviesIds.BolumId, personellerSvc.MoviesIds.SirketId, true).then(function (data) {
                $scope.persons = data;
            });
        });
    };
}

SMHeadListCtrl.$inject = ['$scope', 'personellerSvc', '$state'];
SMBodyListCtrl.$inject = ['$scope', 'personellerSvc', 'DTOptionsBuilder', '$state', '$document', '$stateParams'];

angular
    .module('inspinia')
    .controller('SMHeadListCtrl', SMHeadListCtrl)
    .controller('SMBodyListCtrl', SMBodyListCtrl);