'use strict';
function personellerBodyListCtrl($scope, personellerSvc, DTOptionsBuilder, $state, $document, $stateParams, $window, DTColumnDefBuilder) {
    var vm = this;
    $scope.Bekle = false; 
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
        .withOption('responsive', window.innerWidth < 1500 ? true : false);
    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(2).notVisible(),
        DTColumnDefBuilder.newColumnDef(4).notVisible(),
        DTColumnDefBuilder.newColumnDef(7).notVisible(),
        DTColumnDefBuilder.newColumnDef(8).notVisible(),
        DTColumnDefBuilder.newColumnDef(6).notVisible(),
        DTColumnDefBuilder.newColumnDef(10).notVisible(),
        DTColumnDefBuilder.newColumnDef(11).notVisible(),
        DTColumnDefBuilder.newColumnDef(12).notVisible(),
        DTColumnDefBuilder.newColumnDef(13).notVisible(),
        DTColumnDefBuilder.newColumnDef(14).notVisible(),
        DTColumnDefBuilder.newColumnDef(15).notVisible(),
        DTColumnDefBuilder.newColumnDef(16).notVisible(),
    ];

    var genel = true;
    var durum = true;
    var zx = $stateParams.sti;
    if (!angular.isUndefined($stateParams.sti) || !angular.isUndefined($stateParams.blm)) {
        personellerSvc.GetBolumPersonelleri($stateParams.blm, $stateParams.sti, angular.isUndefined(personellerSvc.MoviesIds.dahil) ? true : personellerSvc.MoviesIds.dahil, angular.isUndefined(personellerSvc.MoviesIds.aktif) ? true : personellerSvc.MoviesIds.aktif).then(function (data) {
            $scope.persons = data;
        });
        personellerSvc.MoviesIds.BolumId = $stateParams.blm === "" ? 0 : $stateParams.blm;
        personellerSvc.MoviesIds.SirketId = $stateParams.sti === "" ? 0 : $stateParams.sti;
        personellerSvc.MoviesIds.SirketAdi = $stateParams.sti === "" ? "Şirketi Seçiniz.!" : $stateParams.stiAd;
        personellerSvc.MoviesIds.BolumAdi = $stateParams.blm === "" ? "Bölümü Seçiniz.!" : $stateParams.blmAd;
    }

    if (!angular.isUndefined($stateParams.sd))
    {
        if ($stateParams.sd === "true") {
            $scope.Bekle = true;
            personellerSvc.GetAllPersList(true).then(function (data) {
                $scope.persons = data;
            }).finally(function () {
                $scope.Bekle = false;
            });
        }
        if ($stateParams.sd === "false") {
            $scope.Bekle = true;
            personellerSvc.GetAllPersList(false).then(function (data) {
                $scope.persons = data;
            }).finally(function () {
                $scope.Bekle = false;
            });
        }
    }

    $scope.$watch(function () { return personellerSvc.MoviesIds.dahil; }, function (newValue, oldValue) {
        if (newValue !== oldValue) genel=newValue;
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.aktif; }, function (newValue, oldValue) {
        if (newValue !== oldValue) durum = newValue;
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            $scope.Bekle = true;
            personellerSvc.GetSirketPersonelleri(newValue, genel,durum).then(function (data) {
                $scope.persons = data;
            }).finally(function () {
                $scope.Bekle = false;
            });
        }
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            $scope.Bekle = true;
            personellerSvc.GetBolumPersonelleri(newValue, personellerSvc.MoviesIds.SirketId, genel, durum).then(function (data) {
                $scope.persons = data;
            }).finally(function () {
                $scope.Bekle = false;
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
        if (confirm("Silmek İstediğinizden Emin misiniz?")) {
            var id = element.attributes['id'].value;
            personellerSvc.DeletePrs(id).then(function (sd) {            
                $scope.persons.splice($scope.persons.findIndex(x => x.PerGuid === id), 1);
            });
        };
    };

    $scope.xActive = function (element) {//personeli pasifize etmek için yazıldı
        var id = element.attributes['id'].value;
        personellerSvc.ActivePrs(id).then(function (deger) {
            personellerSvc.GetBolumPersonelleri(personellerSvc.MoviesIds.BolumId, personellerSvc.MoviesIds.SirketId, personellerSvc.MoviesIds.dahil, personellerSvc.MoviesIds.aktif).then(function (data) {
                $scope.persons = data;
            });
            if (deger === false) { $window.alert("Personel pasifize edildi.") } else $window.alert("Personel aktive edildi.");
        });
       
    };
}

function personellerHeadListCtrl($scope, personellerSvc, $state) {
    personellerSvc.MoviesIds.BolumId = 0;
    personellerSvc.MoviesIds.SirketId = 0;
    personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
    personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';

    $scope.dahil = true;
    $scope.aktif = true;
    $scope.dahilinde = function () {
        personellerSvc.MoviesIds.dahil = $scope.dahil;
    };

    $scope.aktifOl = function () {
        personellerSvc.MoviesIds.aktif = $scope.aktif;
    };

    $scope.tumListe = function () {
        $state.go('Personel.PersonellerFullListDetail', { sd: $scope.aktif });
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
        if (!children || typeof children === "array" && children.length === 0) {
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

personellerHeadListCtrl.$inject = ['$scope', 'personellerSvc', '$state'];
personellerBodyListCtrl.$inject = ['$scope', 'personellerSvc', 'DTOptionsBuilder', '$state', '$document', '$stateParams', '$window','DTColumnDefBuilder'];

angular
    .module('inspinia')
    .controller('personellerHeadListCtrl', personellerHeadListCtrl)
    .controller('personellerBodyListCtrl', personellerBodyListCtrl);