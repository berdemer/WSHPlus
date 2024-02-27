'use strict';
function StokTanimlaCtrl($scope, SMSvc, DTOptionsBuilder, $q, $filter, DTColumnDefBuilder,ExcelSvc) {
	$scope.stoktableClass = 'wrapper wrapper-content animated fadeInRight col-lg-8';
	$scope.dahil = true;
	$scope.pasifize = $scope.dahil === true ? 'Pasifize' : 'Aktive';
	$scope.dahilinde = function () {
		$scope.pasifize = $scope.dahil == true?'Pasifize':'Aktive';
		SMSvc.SBStokListeleri($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
			$scope.stoklar = data;
		});
	};
	$scope.StokExcelListesi = function (deger) {
		SMSvc.SBStokListeleri($scope.birim.selected.SaglikBirimi_Id, $scope.dahil).then(function (data) {
			ExcelSvc.XlsxSave(deger == '' ? data : $(data).filter(function (i, v) {
			    return v.StokTuru == deger;
			}), deger + "StokDurumu.xlsx", "StokDurumu", [0, 3, 4, 5]);
		});
	};
	$scope.birim = {selected:''};
	$scope.stok = { StokId: '', SaglikBirimi_Id: '', IlacAdi: '', StokTuru: '', StokMiktari: '', StokMiktarBirimi: '', KritikStokMiktari: 0, Status: '' };
	$scope.StokTuru = ['İlaç', 'Sarf Malz.', 'Demirbaş Malz.', 'Diğer Malz.'];
	$scope.StokMiktarBirimi = ['Adet', 'Kg', 'Litre', 'gr', 'cm', 'metre'];
	$scope.dtOptions = DTOptionsBuilder.newOptions()
	   .withLanguageSource('/views/Personel/Controller/Turkish.json')
	   .withDOM('<"html5buttons"B>lTfgitp')
	   .withButtons([
		   { extend: 'copy', text: 'Kopyala' },
		   { extend: 'csv' },
		   { extend: 'excel', title: 'StokListesi', name: 'Excel' },
		   { extend: 'pdf', title: 'StokListesi', name: 'PDF' },
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
	   .withOption('order', [1, 'desc'])
	   .withPaginationType('full_numbers')
	   .withSelect(true)
	   .withOption('rowCallback', rowCallback)
		.withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
	   DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				var rs = $filter('filter')($scope.stoklar, {
					StokId: aData[0]
				})[0];
				$scope.stok = {
					StokId: rs.StokId, SaglikBirimi_Id: rs.SaglikBirimi_Id,
					IlacAdi: rs.IlacAdi, StokTuru: rs.StokTuru, StokMiktari: rs.StokMiktari,
					StokMiktarBirimi: aData[4], KritikStokMiktari: rs.KritikStokMiktari,
					Status: rs.Status
				};
			});
			//var table = $("#stoktable").DataTable();
			//console.log(table.rows('.selected').data().length);
			//if (!table.rows('.selected').data().length >0) { $scope.stok = {} };
		});
		return nRow;
	}

	SMSvc.AllSbirimleri().then(function (data) {
		$scope.birimler = data;
		$scope.birim = { selected: data[0] };
	});

	$scope.$watch(function () { return $scope.birim.selected; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			$scope.stoklar = [];
			$scope.birim.selected = newValue;
			SMSvc.SBStokListeleri(newValue.SaglikBirimi_Id, $scope.dahil).then(function (data) {
				$scope.stoklar = data;
			});
		}
	});

	$scope.getDrog = function (value) {//işlem gecikince sonuç boş dönüyor.
	   var deferred = $q.defer();
		SMSvc.IlacAra(value).then(function (response) {
		   deferred.resolve(response);
	   });
	   return deferred.promise;
	};

	$scope.submitForm = function () {
		if ($scope.stokFrom.$valid) {	    // Form değerleri geçerli ise
			if (angular.isUndefined($scope.stok.StokId) || $scope.stok.StokId == "") {//yeni Kayıt
				$scope.stok.SaglikBirimi_Id = $scope.birim.selected.SaglikBirimi_Id;
				SMSvc.StokEkle($scope.stok).then(function (response) {
					$scope.dahilinde();
					$scope.stok = {};
				});
			} else {
				SMSvc.StokGuncelle($scope.stok.StokId,$scope.stok).then(function (response) {
					$scope.dahilinde();
					$scope.stok = {};
				});
			}
		}
	};

	var deselect = function () {
		var table = $("#stoktable").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected'); 
	};

	$scope.newForm = function () {
		$scope.stok = {}; deselect();
	};

	$scope.updateForm = function () {
		if (!angular.isUndefined($scope.stok.StokId) || $scope.stok.StokId != "") {
			var deger=$scope.stok.Status==true?false:true;
			$scope.stok.Status=deger;
			SMSvc.StokGuncelle($scope.stok.StokId, $scope.stok).then(function (response) {
				$scope.dahilinde();
				$scope.stok = {};
			});
		}
	};
}

StokTanimlaCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', '$q', '$filter', 'DTColumnDefBuilder', 'ExcelSvc'];

angular
	.module('inspinia')
	.controller('StokTanimlaCtrl', StokTanimlaCtrl);