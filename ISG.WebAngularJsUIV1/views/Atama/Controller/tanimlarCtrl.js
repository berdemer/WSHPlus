'use strict';
function tanimlarCtrl($scope, DTOptionsBuilder, tanimlarSvc, $timeout) {

	$scope.tanims = {tanim_Id:"", tanimAdi: "", ifade: "", tanimKisaltmasi: "", ifadeBagimliligi: "", aciklama:"" };
	$scope.dtOptions = DTOptionsBuilder.newOptions()
	.withLanguageSource('/views/Personel/Controller/Turkish.json')
	.withOption('aLengthMenu', [5, 10, 20, 50, 100])
	.withOption('iDisplayLength', 5)
	.withPaginationType('full_numbers')
		.withOption('responsive', window.innerWidth < 1500 ? true : false).withOption('paging', false).withOption('searching', false);

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


	$scope.satirBilgisi = function (aData) {
		$scope.tanims.tanimAdi = aData.tanimAdi;
		$scope.tanims.ifade = aData.ifade;
		$scope.tanims.tanimKisaltmasi = aData.tanimKisaltmasi;
		$scope.tanims.ifadeBagimliligi = aData.ifadeBagimliligi;
		$scope.tanims.aciklama = aData.aciklama;
		$scope.tanims.tanim_Id = aData.tanim_Id;
		$scope.tanimItem = aData.tanimAdi;
	};

	var gettanim = function () {
		tanimlarSvc.GetTanimlar().then(function (response) {
			$scope.message = "";
			$scope.tanimlar = response.anaListe;
			$scope.tanimlarx = response.uniqListe;
			$scope.totalItems = response.anaListe.length;
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
		});
		$scope.message = "Bekleyin Dosya Yüklemesi başladı....";
	};

	$scope.newTanims = function () {
		$scope.tanims = { tanim_Id: "", ifade: "", tanimKisaltmasi: "", ifadeBagimliligi: "", aciklama: "" };
	};
	$scope.saveTanims = function () {
		if ($scope.tanims.tanim_Id == undefined || $scope.tanims.tanim_Id == "") {
			tanimlarSvc.AddTanimView($scope.tanims).then(function (response) {
				gettanim();
			   // $scope.tanimlar.push();
			}).catch(function (e) {

			});
		} else {
			tanimlarSvc.UpdateTanimView($scope.tanims.tanim_Id,$scope.tanims).then(function (response) {
				gettanim();
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};

	$scope.deleteTanims = function () {
		if ($scope.tanims.tanim_Id != undefined || $scope.tanims.tanim_Id != "") {
			tanimlarSvc.DeleteTanimView($scope.tanims.tanim_Id).then(function (response) {
				gettanim();
				// $scope.tanimlar.push();
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};

	$timeout(function () {        
		gettanim();
	}, 750);
}



tanimlarCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'tanimlarSvc', '$timeout'];

angular
	.module('inspinia')
	.controller('tanimlarCtrl', tanimlarCtrl);