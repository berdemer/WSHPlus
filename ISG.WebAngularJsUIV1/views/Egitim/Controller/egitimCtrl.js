'use strict';
function egitimCtrl($scope, personellerSvc, $location, $state, sirketSvc, $sce, notify) {
	var egtC = this;
	$scope.lokal = $location.hash();//sayfanın bulunduğu yeri tespit ediyoruz.
	egtC.year = (new Date().getFullYear()).toString();
	personellerSvc.MoviesIds.SirketId = 0;
	personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
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
		if (angular.isUndefined(newValue)) {
			$scope.SirketBasligi = {
				id: 0,
				name: 'Şirketi Seçiniz.!'
			};
		} else {
			//https://stackoverflow.com/questions/28042598/how-to-state-go
			$state.go('Egitim.egt.EgtKayit', {//url yönlendirmesi
				id: $scope.SirketBasligi.id, year: egtC.year 
			});
		};
		egtC.sirketAdi = $scope.SirketBasligi.id === 0 ? '' : $scope.SirketBasligi.name;
		
		sirketSvc.GetSirketDetayi(newValue).then(function (data) {
			if (data !== null) {
				egtC.IsverenTemsicisi =data.SirketYetkilisi;
				switch (data.TehlikeGrubu) {
					case 0:
						egtC.tehlikeGrubu = "<b>Az Tehlikeli</b> (8 saat eğitim süresi)"
						break;
					case 1:
						egtC.tehlikeGrubu = "<b>Tehlikeli</b> (12 saat eğitim süresi)"
						break;
					default:
						egtC.tehlikeGrubu = "<b>Çok Tehlikeli</b> (16 saat eğitim süresi)"
				}
            }
		});
	});




	$scope.$watch(function () { return egtC.year; }, function (newValue, oldValue) {
		if (angular.isUndefined(newValue)) {
			$scope.SirketBasligi = {
				id: 0,
				name: 'Şirketi Seçiniz.!'
			};
		} else {
			$state.go('Egitim.egt.EgtKayit', {//url yönlendirmesi
				id: $scope.SirketBasligi.id, year: egtC.year
			});
		};
	});


}

egitimCtrl.$inject = ['$scope', 'personellerSvc', '$location', '$state', 'sirketSvc', '$sce','notify'];

angular
	.module('inspinia')
	.controller('egitimCtrl', egitimCtrl);

