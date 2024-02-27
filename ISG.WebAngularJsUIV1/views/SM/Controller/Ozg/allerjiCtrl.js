'use strict';

function allerjiCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload,$stateParams,
	 ngAuthSettings, uploadService, authService, OgSvc, SMSvc, $q, $window) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var allc = this;

	$scope.files = [];

	$scope.dosyaList = false;

	allc.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	allc.model = {
		
	};

	allc.options = {};

	allc.fields = [
			{
				className: 'col-sm-4',
				type: 'typhead',
				key: 'AllerjiOykusu',
				templateOptions: {
					label: 'Allerji Öyküsü',
					placeholder: '"Allerji Öyküsü" olarak aratın',
					required: true,
					options: [],
					onKeyup: function ($viewValue, $scope) {
						if (typeof $viewValue != 'undefined') {
							return $scope.templateOptions.options = allc.HastalikTanimiList($viewValue);
						}
					}
				}
			},
			{
				className: 'col-sm-2',
				type: 'input',
				key: 'AllerjiSuresi',
				templateOptions: {
					type: 'number',
					min: 0,
					label: 'Süresi',
					placeholder: 'Allerji Süresi Yıl'
				}
			},
			{
				className: 'col-sm-3',
				key: 'AllerjiCesiti',
				type: 'ui-select-single',
				templateOptions: {
					label: 'Allerji Ceşiti',
					optionsAttr: 'bs-options',
					ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
					valueProp: 'value',
					labelProp: 'name',
					required: true,
					options: [{ name: 'Allerjik Purpura(ürtiker)', value: 'purpura' }, { name: 'Allerjik Rinit', value: 'Rinit' }, { name: 'Allerjik Astım', value: 'Astım' }, { name: 'Allerjik Konjuktivit', value: 'Konjuktivit' },
						 { name: 'Allerjik Kontakt Dermatit', value: 'Dermatit' }, { name: 'Allerjik Gastroenterit', value: 'Enterit' },{ name: 'Anaflaksi', value: 'Anaflaksi' }
					]
				}
			},
			{
					className: 'col-sm-3',
					type: 'input',
					key: 'AllerjiEtkeni',
					templateOptions: {
						type: 'input',
						label: 'Allerji Etkeni',
						placeholder: 'Allerji Etkeni Madde Tanımı'
					}
				}
	];

	allc.originalFields = angular.copy(allc.fields);

	if ($stateParams.id === OgSvc.Oz.guidId) {
		$scope.Allerjiler = OgSvc.Oz.OzBilgi.data.Allerjiler;
	}
	else {
		OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
			OgSvc.Oz.OzBilgi = s;
			$scope.Allerjiler = s.data.Allerjiler;
			OgSvc.Oz.guidId = $stateParams.id;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed=true;

	$scope.dtOptionsallc = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Allerjiler Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Allerjiler Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsallc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	allc.onSubmit = function () {
		allc.model.Personel_Id = OgSvc.Oz.OzBilgi.data.Personel_Id;
		if (angular.isUndefined(allc.model.Allerji_Id) || allc.model.Allerji_Id == null) {
			OgSvc.AddPrsAllerji(allc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.Allerjiler = s.data.Allerjiler;
				});
				$scope.yeniallc();
			});
		}
		else
		{
			OgSvc.UpdatePrsAllerji(allc.model.Allerji_Id, allc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.Allerjiler = s.data.Allerjiler;
				});
				$scope.yeniallc();
			});
		}
	};

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Allerjiler, {
					Allerji_Id: aData[0]
				})[0];
				allc.model = {
					Allerji_Id: rs.Allerji_Id, AllerjiOykusu: rs.AllerjiOykusu, AllerjiCesiti: rs.AllerjiCesiti, AllerjiEtkeni: rs.AllerjiEtkeni, AllerjiSuresi: rs.AllerjiSuresi,
					Personel_Id: rs.Personel_Id, UserId: rs.UserId
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'allerji', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}	

	var deselect = function () {
		var table = $("#allerji").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniallc = function () {
		allc.model = {}; $scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silallc = function () {
		OgSvc.DeletePrsAllerji(allc.model.Allerji_Id).then(function (response) {
			OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
				OgSvc.Oz.OzBilgi = s;
				$scope.Allerjiler = s.data.Allerjiler;
			});
			$scope.yeniallc();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/allerji/' + allc.model.Allerji_Id,
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
			if (response.data == 1) { $scope.files.splice(index, 1); }
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

	$scope.$watch(function () { return allc.model.Allerji_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

allerjiCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'OgSvc', 'SMSvc', '$q', '$window'];

angular
	.module('inspinia')
	.controller('allerjiCtrl', allerjiCtrl);