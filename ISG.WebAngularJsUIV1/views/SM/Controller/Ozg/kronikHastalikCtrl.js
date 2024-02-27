'use strict';

function kronikHastalikCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, OgSvc, SMSvc, $q, $http, $window, $uibModal) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var Khc = this;

	$scope.varYok = function (value) {
		return value === true ? 'Var' : 'Yok';
	};

	$scope.files = [];

	$scope.sortDate = function (comment) {

	    return new Date(comment.Tarih);
	};

	$scope.dosyaList = false;

	Khc.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	Khc.model = {

	};

	Khc.options = {};

	Khc.fields = [
			{
				className: 'row col-sm-12',
				fieldGroup: [
				{
					className: 'col-sm-4',
					type: 'typhead',
					key: 'HastalikAdi',
					templateOptions: {
						label: 'Hastalık',
						placeholder: 'Hastalık Tanımı',
						required: true,
						options: [],
						onKeyup: function ($viewValue, $scope) {
							if (typeof $viewValue !== 'undefined') {
								return $scope.templateOptions.options = Khc.HastalikTanimiList($viewValue);
							}
						}
					}
				},
				{
					className: 'col-sm-2',
					type: 'input',
					key: 'HastalikYilSuresi',
					templateOptions: {
						type: 'number',
						min: 0,
						placeholder: 'Yıl',
						label: 'Süresi'
					}
				},
				{
					className: 'col-sm-2',
					key: 'AmeliyatVarmi',
					type: 'select',
					defaultValue: false,
					templateOptions: {
						label: 'Ameliyat',
						options: [{ name: 'Yok', value: false }, { name: 'Var', value: true }]
					}
				},
				{
					className: 'col-sm-2',
					key: 'IsKazasi',
					type: 'select',
					defaultValue: false,
					templateOptions: {
						label: 'İş_Kazası',
						options: [{ name: 'Yok', value: false }, { name: 'Var', value: true }]
					}
				},
				{
					className: 'col-sm-2',
					key: 'HastalikOzurDurumu',
					type: 'select',
					defaultValue: false,
					templateOptions: {
						label: 'Engellilik',
						options: [{ name: 'Yok', value: false }, { name: 'Var', value: true }]
					}
				}
				]
			},
			{
				className: 'row col-sm-12',
				fieldGroup: [
				{
					className: 'col-sm-9',//http://jsbin.com/pemibedusi/edit?html
					key: 'KullandigiIlaclar',
					type: 'ui-select-multiple-async',
					templateOptions: {
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						label: 'İlaçları',
						valueProp: 'IlacAdi',
						labelProp: 'IlacAdi',
						placeholder: 'İlaç Arama',
						options: [],
						refresh: refreshAddresses,
						refreshDelay: 250
					}
				},
				{
					className: 'col-sm-3'
				}
				]
			}
	];

	Khc.originalFields = angular.copy(Khc.fields);


	function refreshAddresses(address, field) {
		var promise;
		if (!address) {
			promise = $q.when({});
		} else {
			promise = $http.get(serviceBase + 'api/ilac/Ara/' + address);
		}
		return promise.then(function (response) {
			field.templateOptions.options = response.data;
		});
	}

	$scope.getDrog = function (value) {//işlem gecikince sonuç boş dönüyor.
		var deferred = $q.defer();
		SMSvc.IlacAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	if ($stateParams.id === OgSvc.Oz.guidId) {
		$scope.KronikHastaliklar = OgSvc.Oz.OzBilgi.data.KronikHastaliklar;
	}
	else {
		OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
			OgSvc.Oz.OzBilgi = s;
			$scope.KronikHastaliklar = s.data.KronikHastaliklar;
			OgSvc.Oz.guidId = $stateParams.id;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsKhc = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 {
			 text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
			 className: 'addButton', action: function (e, dt, node, config) {
				 $scope.$apply(function () {
				     $scope.isCollapsed = !$scope.isCollapsed;
				     $scope.dosyaList = !$scope.dosyaList;
				 });
			 }
		 },
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'KronikHastaliklar Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'KronikHastaliklar Listesi', titleAttr: 'PDF' },
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
	 .withPaginationType('full_numbers')
	 .withSelect(true)
	 .withOption('order', [1, 'desc'])
	 .withOption('lengthMenu', [3, 10, 20, 50, 100])
	 .withOption('rowCallback', rowCallback)
	 .withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefsKhc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	Khc.onSubmit = function () {
		$scope.kayit = true;
		Khc.model.Personel_Id = OgSvc.Oz.OzBilgi.data.Personel_Id;
		var hg = [];
		angular.forEach(Khc.model.KullandigiIlaclar, function (v) {
		    hg.push(v.IlacAdi === undefined ? v : v.IlacAdi);
		});
		Khc.model.KullandigiIlaclar = hg.toString();
		if (angular.isUndefined(Khc.model.KronikHastalik_Id) || Khc.model.KronikHastalik_Id === null) {
			OgSvc.AddPrsKronikHastalik(Khc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.KronikHastaliklar = s.data.KronikHastaliklar;
				});
				$scope.yeniKhc();
			});
		}
		else {
			OgSvc.UpdatePrsKronikHastalik(Khc.model.KronikHastalik_Id, Khc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.KronikHastaliklar = s.data.KronikHastaliklar;
				});
				$scope.yeniKhc();
			});
		}
	};

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.KronikHastaliklar, {
					KronikHastalik_Id: aData[0]
				})[0];
				var ilaclar = [];
				angular.forEach(rs.KullandigiIlaclar.split(","), function (v) {
				    ilaclar.push({ IlacAdi: v });
				});
				
				Khc.model = {
					id: aData[1], KronikHastalik_Id: rs.KronikHastalik_Id, HastalikAdi: rs.HastalikAdi, KullandigiIlaclar: rs.KullandigiIlaclar === "" ? [{ IlacAdi: "İlaç Kaydı Yok" }] : ilaclar,
					HastalikYilSuresi: rs.HastalikYilSuresi, Personel_Id: rs.Personel_Id, UserId: rs.UserId, AmeliyatVarmi: rs.AmeliyatVarmi
				   , IsKazasi: rs.IsKazasi, HastalikOzurDurumu: rs.HastalikOzurDurumu
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'kronik-hastalik',aData[0] ).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#kronikHastalik").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniKhc = function () {
		Khc.model = {
			HastalikAdi: "", KullandigiIlaclar: [],
			HastalikYilSuresi: 0, Personel_Id: "", UserId: "", AmeliyatVarmi: false
		 , IsKazasi: false, HastalikOzurDurumu: false
		};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silKhc = function () {
		OgSvc.DeletePrsKronikHastalik(Khc.model.KronikHastalik_Id).then(function (response) {
			OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
				OgSvc.Oz.OzBilgi = s;
				$scope.KronikHastaliklar = s.data.KronikHastaliklar;
			});
			$scope.yeniKhc();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/kronik-hastalik/' + Khc.model.KronikHastalik_Id,
				method: 'POST',
				data: {
					file: dataUrl
				}
			}).then(function (response) {
			    angular.forEach(response.data, function (data) {
			        $scope.files.push(data);
			    });
			});
		}
		else {
			$scope.dosyaList = false;
		}
	};

	$scope.deleten = function (file, index) {
		uploadService.DeleteFile(file + '/x').then(function (response) {
			if (response.data === 1) { $scope.files.splice(index, 1); }
		});
	};

	$scope.download = function (f) {
		debugger;
		if (f.GenericName.indexOf('.') < 1) {
			//$scope.RenderFile(genericName);
			var s = serviceBase + ngAuthSettings.apiUploadService + 'down/' + f.GenericName;
			$window.open(s, '_self', '');
		} else {
			window.open(f.LocalFilePath, 'popUpWindow', 'height=1000,width=1200,left=10,top=10,,scrollbars=yes,menubar=no')
		}
	};

	$scope.$watch(function () { return Khc.model.KronikHastalik_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});
	$scope.kayit = false;
	$scope.$watch('[Khc.model.KullandigiIlaclar,Khc.model.id]', function (newVal, oldVal) {
	    if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
	        var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
	        var oldVal0Len = oldVal[0] === undefined ? 0 : oldVal[0].length;
	        var ilaclar = [];
	        if (newVal0Len < oldVal0Len && newVal[1] === oldVal[1]) {
				ilaclar = [];
				angular.forEach(newVal[0], function (v) {
				    ilaclar.push({ IlacAdi: v });
				});
				Khc.model.KullandigiIlaclar = ilaclar;
			}
	        if (newVal0Len > oldVal0Len && newVal[1] === oldVal[1] && $scope.kayit === false) {
				ilaclar = [];
				angular.forEach(newVal[0], function (v) {
					ilaclar.push({ IlacAdi: v.toString().replace(',', '.') });
				});
				var last = ilaclar[ilaclar.length - 1].IlacAdi;
				$scope.doz = {ilacAdi:last,giris:null};
				$uibModal.open({
					templateUrl: 'dozKaydi.html',
					backdrop: true,
					animation: true,
					size: 'sm',
					windowClass: "animated flipInY",
					controller: function ($scope, $uibModalInstance, $log, doz) {
						$scope.doz = doz;
						$scope.submittenx = function () {
						    ilaclar[ilaclar.length - 1].IlacAdi = last + (doz.giris === null ? "" : " ( Dozu : " + doz.giris + " ) ");
						    Khc.model.KullandigiIlaclar = ilaclar;
						    $uibModalInstance.dismiss('cancel');
						};
						$scope.cancel = function () {
							$uibModalInstance.dismiss('cancel');
						};
					},
					resolve: {
						doz: function () {
							return $scope.doz;
						}
					}
				});
			}
			$scope.kayit = false;
		}
	}, true);
}

kronikHastalikCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'OgSvc', 'SMSvc', '$q', '$http', '$window', '$uibModal'];

angular
	.module('inspinia')
	.controller('kronikHastalikCtrl', kronikHastalikCtrl);