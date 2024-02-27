'use strict';

function isKazasiListesiCtrl($scope, personellerSvc, $state, analizSvc, $q, ngAuthSettings, DTOptionsBuilder, DTColumnDefBuilder) {
	var iklC = this;
	iklC.year = (new Date().getFullYear()).toString();
	iklC.sonuclar = [{ id: 'hepsi', adi: 'Tümü' }, { id: 'Isbasi', adi: 'İş Başı' }, { id: 'Sevk', adi: 'Sevk' }, { id: 'Istirahat', adi: 'İstirahat' }];
	iklC.sonuc = iklC.sonuclar[0];
	personellerSvc.MoviesIds.BolumId = 0;
	personellerSvc.MoviesIds.SirketId = 0;
	personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
	personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
	$scope.SirketBasligi = {
		id: 0,
		name: 'Şirketi Seçiniz.!'
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
		open: false
	};
	$scope.showSelected = function (sel) {
		$scope.SirketBasligi.name = sel.name;
		$scope.SirketBasligi.id = sel.id;
		personellerSvc.MoviesIds.SirketId = sel.id;//ŞİRKETİN ID BİLGİSİ
		personellerSvc.MoviesIds.SirketAdi = sel.name;
		$scope.status.open = !$scope.status.open;
	};
	$scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
		$scope.SirketBasligi.name = personellerSvc.MoviesIds.SirketAdi;
		$scope.SirketBasligi.id = personellerSvc.MoviesIds.SirketId;
		listeAl($scope.SirketBasligi.id, 'Ikbilgisi', iklC.sonuc.id, iklC.year);
		if (angular.isUndefined(newValue)) {
			$scope.SirketBasligi = {
				id: 0,
				name: 'Şirketi Seçiniz.!'
			};           
		}
		iklC.sirketAdi = $scope.SirketBasligi.id === 0 ? '' : $scope.SirketBasligi.name;
	});
	var listeAl = function (Sirket_Id, muayeneDurumu, muayeneSonucu, year) {
		analizSvc.IsKazasiListesi(Sirket_Id, muayeneDurumu, muayeneSonucu, year).then(function (resp) {
			iklC.kazaListesi = resp;
		});
	};
	$scope.$watch(function () { return iklC.year; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			listeAl($scope.SirketBasligi.id, 'Ikbilgisi', iklC.sonuc.id, newValue);
		}
	});
	iklC.Durumu = function (kaza) {
		var sd = "";
		if (kaza.TxtData.Isbasi !== undefined) {
			sd = 'İş Başı';
		}
		if (kaza.TxtData.Sevk !== undefined) {
			sd = 'Sevk Edildi';
		}
		if (kaza.TxtData.Istirahat !== undefined) {
			sd = 'İstirahat ' + kaza.TxtData.Istirahat.GunSayisi + ' Gün';
		}
		return sd;
	};
	$scope.$watch(function () { return iklC.sonuc; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			listeAl($scope.SirketBasligi.id, 'Ikbilgisi', newValue.id, iklC.year);
		}
	});

	$scope.dtOptions = DTOptionsBuilder.newOptions()
	.withLanguageSource('/views/Personel/Controller/Turkish.json')
	.withDOM('<"html5buttons"B>lTfgitp')
	.withButtons([
		{ extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		{ extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		{ extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'İş Kazaları Listesi', titleAttr: 'Excel 2010' },
		{ extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'İş Kazaları Listesi', titleAttr: 'PDF' },
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
	.withPaginationType('full')
	.withOption('lengthMenu', [5, 10, 20, 40,50])
	.withOption('responsive', window.innerWidth < 1500 ? true : false);
	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

}

isKazasiListesiCtrl.$inject = ['$scope', 'personellerSvc', '$state', 'analizSvc', '$q', 'ngAuthSettings', 'DTOptionsBuilder', 'DTColumnDefBuilder'];
angular
	.module('inspinia')
	.controller('isKazasiListesiCtrl', isKazasiListesiCtrl);