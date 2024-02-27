'use strict';
function SMCardsCtrl($scope, personellerSvc, ngAuthSettings, $rootScope, $stateParams) {

    var genel = true;

    $scope.init = function () {
		if (!angular.isUndefined($stateParams.id)) {
			personellerSvc.CDV.loading = true;
            personellerSvc.PersonelSearch(true, $stateParams.id, personellerSvc.CDV.PageNumber * personellerSvc.CDV.PageCount, personellerSvc.CDV.PageCount).then(function (data) {
                $scope.Cards = data.PersonelCards;
                personellerSvc.CDV.PageNumber = data.DisplayStart / data.DisplayLength;
                personellerSvc.CDV.PageCount = data.DisplayLength;
                personellerSvc.CDV.TotalItems = data.TotalItems;
			}).finally(function () {
				personellerSvc.CDV.loading = false;
			});
        }
    };

	$scope.onDon = function (a) {
		$rootScope.$broadcast(a);
	};
	$scope.arkaDon = function (a) {
		$rootScope.$broadcast(a);
	};

	$scope.$watch(function () { return personellerSvc.MoviesIds.dahil; }, function (newValue, oldValue) {
		if (newValue !== oldValue) genel = newValue;
		cardViewItems();
	});

	$scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			personellerSvc.CDV.SearchBool = false;
			personellerSvc.CDV.SirBool = true;
			personellerSvc.CDV.BolBool = false;
			cardViewItems();
		}
	});

	$scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			personellerSvc.CDV.SearchBool = false;
			personellerSvc.CDV.SirBool = false;
			personellerSvc.CDV.BolBool = true;
			cardViewItems();
		}
	});

	$scope.$watch(function () { return personellerSvc.CDV.perSearch; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			if (newValue === '') {
				$scope.Cards = [];
				personellerSvc.CDV.PageNumber = 0;
				personellerSvc.CDV.TotalItems = 0;
			}
			personellerSvc.MoviesIds.BolumId = 0;
			personellerSvc.MoviesIds.SirketId = 0;
			personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
			personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
			personellerSvc.CDV.SearchBool = true;
			personellerSvc.CDV.SirBool = false;
			personellerSvc.CDV.BolBool = false;
			cardViewItems();
		}
	});

	$scope.$watch(function () { return personellerSvc.CDV.PageCount; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			cardViewItems();
		}
	});

	$scope.$watch(function () { return personellerSvc.CDV.PageNumber; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			cardViewItems();
		}
	});

	var cardViewItems = function () {
		if (personellerSvc.CDV.SearchBool) {
			personellerSvc.CDV.loading = true;
			personellerSvc.PersonelSearch(true, personellerSvc.CDV.perSearch, personellerSvc.CDV.PageNumber * personellerSvc.CDV.PageCount, personellerSvc.CDV.PageCount).then(function (data) {
				$scope.Cards = data.PersonelCards;
				personellerSvc.CDV.PageNumber = data.DisplayStart / data.DisplayLength;
				personellerSvc.CDV.PageCount = data.DisplayLength;
				personellerSvc.CDV.TotalItems = data.TotalItems;
			}).finally(function () {
				personellerSvc.CDV.loading = false;
			});
		}
		else
			if (personellerSvc.CDV.SirBool) {
				personellerSvc.CDV.loading = true;
				personellerSvc.SirketSearch(personellerSvc.MoviesIds.SirketId, genel, true, personellerSvc.CDV.PageNumber * personellerSvc.CDV.PageCount, personellerSvc.CDV.PageCount).then(function (data) {
					$scope.Cards = data.PersonelCards;
					personellerSvc.CDV.PageNumber = data.DisplayStart / data.DisplayLength;
					personellerSvc.CDV.PageCount = data.DisplayLength;
					personellerSvc.CDV.TotalItems = data.TotalItems;
				}).finally(function () {
					personellerSvc.CDV.loading = false;
				});
			}
			else
				if (personellerSvc.CDV.BolBool) {
					personellerSvc.CDV.loading = true;
					personellerSvc.BolumSearch(personellerSvc.MoviesIds.BolumId,personellerSvc.MoviesIds.SirketId, genel, true, personellerSvc.CDV.PageNumber * personellerSvc.CDV.PageCount, personellerSvc.CDV.PageCount).then(function (data) {
						$scope.Cards = data.PersonelCards;
						personellerSvc.CDV.PageNumber = data.DisplayStart / data.DisplayLength;
						personellerSvc.CDV.PageCount = data.DisplayLength;
						personellerSvc.CDV.TotalItems = data.TotalItems;
					}).finally(function () {
						personellerSvc.CDV.loading = false;
					});
				}
				else return false;
	};

	var fileImgPath = function () {
		return ngAuthSettings.storageLinkService + (ngAuthSettings.isAzure ? 'personel/' : ngAuthSettings.uploadFolder);
	};
				
	$scope.fileImgPath = fileImgPath();

	
}

function SMHeadCardsCtrl($scope, personellerSvc) {

	$scope.$watch('width', function (old, newv) {
		personellerSvc.CDV.PageCount = old > 767 ? 4 : 1;
		$scope.viewBy = old > 767 ? 4 : 1;
		$scope.sakla = old > 767 ? true : false;
	});

	personellerSvc.MoviesIds.BolumId = 0;
	personellerSvc.MoviesIds.SirketId = 0;
	personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
	personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
	personellerSvc.CDV.PageNumber = 0;

	$scope.setItemsPerPage = function () {
		$scope.itemsPerPage = $scope.viewBy;
		personellerSvc.CDV.PageCount = $scope.viewBy;
	};

	$scope.dahil = true;
	$scope.dahilinde = function () {
		personellerSvc.MoviesIds.dahil = $scope.dahil;
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
		if (!children || typeof  children === "array" && children.length === 0) {
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

	var kayıt = function () {
		return $scope.TotalItems === undefined || '' || 0 ? '' : $scope.TotalItems + ' personelden ' +
	((personellerSvc.CDV.PageNumber + 1) * personellerSvc.CDV.PageCount - (personellerSvc.CDV.PageCount - 1)) + ' - ' +
	 ((personellerSvc.CDV.PageNumber + 1) * personellerSvc.CDV.PageCount > $scope.TotalItems ? $scope.TotalItems : (personellerSvc.CDV.PageNumber + 1) * personellerSvc.CDV.PageCount) +
	' arasındaki personeller gösteriliyor';
	};

	$scope.$watch(function () { return $scope.searchInputVal; }, function (newValue, oldValue) {
		personellerSvc.CDV.perSearch = newValue;
		$scope.kayitBilgisi = kayıt();
		$scope.status = {
			open: false,
			open2: false
		};
	});

	$scope.$watch(function () { return $scope.CurrentPage; }, function (newValue, oldValue) {
		personellerSvc.CDV.PageNumber = $scope.CurrentPage === undefined ? 0 : newValue - 1;
		$scope.kayitBilgisi = kayıt();
		$scope.status = {
			open: false,
			open2: false
		};
	});

	$scope.$watch(function () { return $scope.viewBy;}, function (newValue, oldValue) {
		personellerSvc.CDV.PageCount = newValue;
		$scope.kayitBilgisi = kayıt();
		$scope.status = {
			open: false,
			open2: false
		};
	});

	$scope.$watch(function () { return personellerSvc.CDV.TotalItems; }, function (newValue, oldValue) {
		if (newValue === 0) { $scope.gizle = false; } else { $scope.gizle = true; }
		$scope.TotalItems = newValue;
		$scope.kayitBilgisi = kayıt();
	});
	$scope.Loading = false;
	$scope.$watch(function () { return personellerSvc.CDV.loading }, function (newValue, oldValue) {
		$scope.Loading = newValue;
	});

}

SMHeadCardsCtrl.$inject = ['$scope', 'personellerSvc'];
SMCardsCtrl.$inject = ['$scope', 'personellerSvc', 'ngAuthSettings', '$rootScope', '$stateParams'];


angular
	.module('inspinia')
	.controller('SMHeadCardsCtrl', SMHeadCardsCtrl)
	.controller('SMCardsCtrl', SMCardsCtrl);