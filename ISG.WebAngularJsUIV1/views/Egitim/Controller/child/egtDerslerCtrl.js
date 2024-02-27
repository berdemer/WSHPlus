'use strict';

function egtDerslerCtrl($scope, $q, DTOptionsBuilder, $location, $stateParams, egitimSvc, DTColumnDefBuilder) {

    var EgDC = this;
    var tableResize = window.innerWidth < 1200 ? true : false;
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            { extend: 'copy', text: 'Kopyala' },
            { extend: 'csv' },
            { extend: 'excel', title: 'PersonelDersListesi', name: 'Excel' },
            { extend: 'pdf', title: 'PersonelDersListesi', name: 'PDF' },
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
        .withOption('responsive', tableResize);
    if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {
        $scope.messageIcoEkle = true;
		egitimSvc.TopluEgitimListesi($stateParams.id,$stateParams.year).then(function (r) {
			EgDC.egitimler = r;
        }).catch(function (e) {
            $scope.message = e.data;
        }).finally(function () {
            $scope.messageIcoEkle = false;
        });
	};
    EgDC.TopluEgitimSil = function (v) {
        if (confirm("Silmek İstediğinizden Eminmisin?")) {
            $scope.messageIcoEkle = false;
            egitimSvc.DeleteTopluEgitim(v).then(function (val) {
                egitimSvc.TopluEgitimListesi($stateParams.id, $stateParams.year).then(function (r) {
                    EgDC.egitimler= r;                   
                });
            }).catch(function (e) {
                $scope.message = e.data;
            }).finally(function () {
                $scope.messageIcoEkle = false;
            });
        };
    };
    EgDC.TopluEgitimDetaylari = function (v) {
        $scope.messageIcoEkle = true;
        egitimSvc.GetTopluEgitim(v).then(function (val) {
            egitimSvc.Egitim.id = val.Id;
            egitimSvc.Egitim.data = val.JData;
            $location.path('/egitim/egt/EgtKayit/' + $stateParams.id + '/' + $stateParams.year);
        }).catch(function (e) {
            $scope.message = e.data;
        }).finally(function () {
            $scope.messageIcoEkle = false;
        });
    };
}
egtDerslerCtrl.$inject = ['$scope', '$q', 'DTOptionsBuilder', '$location', '$stateParams', 'egitimSvc', 'DTColumnDefBuilder'];

angular
	.module('inspinia')
	.controller('egtDerslerCtrl', egtDerslerCtrl);