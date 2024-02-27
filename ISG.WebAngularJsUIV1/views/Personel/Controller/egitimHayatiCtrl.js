'use strict';
function egitimHayatiCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder, DTColumnDefBuilder, $filter
	, Upload, $window, uploadService, ngAuthSettings) {
	var ehc = this;

	ehc.model = {
		EgitimHayati_Id: undefined, Personel_Id: undefined, Egitim_seviyesi: undefined, Okul_Adi: undefined,
		Baslama_Tarihi: undefined, Bitis_Tarihi: undefined, Meslek_Tanimi: undefined
	};

	ehc.options = {};

	ehc.fields = [
					{
						className: 'row col-sm-12',
						fieldGroup: [
									{
										className: 'col-sm-3',
										key: 'Egitim_seviyesi',
										type: 'ui-select-single',
										templateOptions: {
											optionsAttr: 'bs-options',
											ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
											label: 'EgitimSeviyesi',
											required: true,
											valueProp: 'value',
											labelProp: 'name',
											placeholder: 'Seviye Seçiniz',
											options: personellerSvc.SD.egitimSeviyesi
										}
									},
									{
										className: 'col-sm-5',
										type: 'input',
										key: 'Okul_Adi',
										templateOptions: {
											type: 'input',
											label: 'Okulun Adı'
										}
									},
									{
										className: 'col-sm-4',
										type: 'input',
										key: 'Meslek_Tanimi',
										templateOptions: {
											type: 'input',
											label: 'Edilinen Meslek'
										}
									}
						]
					},
					{
						className: 'row col-sm-12',
						fieldGroup: [
						{
							className: 'col-sm-3',
							key: 'Baslama_Tarihi',
							type: 'datepicker',
							templateOptions: {
								label: 'Okula Başlama Tarihi',
								type: 'text',
								required: true,
								datepickerPopup: 'dd.MMMM.yyyy',
								datepickerOptions: {
									format: 'dd.MM.yyyy'
								}
							}
						},
						{
							className: 'col-sm-3',
							key: 'Bitis_Tarihi',
							type: 'datepicker',
							templateOptions: {
								label: 'Okul Bitiş Tarihi',
								type: 'text',
								datepickerPopup: 'dd.MMMM.yyyy',
								datepickerOptions: {
									format: 'dd.MM.yyyy'
								}
							}
						},
						{ className: 'col-sm-3' },
						{
							className: 'col-sm-3'
						}
						]
					}
	];

	ehc.originalFields = angular.copy(ehc.fields);


	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.EgitimHayatlari = personellerSvc.MoviesIds.personel.data.EgitimHayatlari;
	} else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.EgitimHayatlari = s.data.EgitimHayatlari;
		});
	}
	$scope.isCollapsed = true;
	$scope.dtOptionseh = DTOptionsBuilder.newOptions()
	  .withLanguageSource('/views/Personel/Controller/Turkish.json')
	  .withDOM('<"html5buttons"B>lTfgitp')
	  .withButtons([
		 {
			 text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
			 className: 'addButton', action: function (e, dt, node, config) {
				 $scope.$apply(function () {
					 $scope.isCollapsed = !$scope.isCollapsed;
				 });
			 }
		 },
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Egitim Hayatlari Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Egitim Hayatlari Listesi', titleAttr: 'PDF' },
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
	  .withOption('rowCallback', rowCallback)
	  .withOption('responsive', true);

	$scope.dtColumnDefseh = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.EgitimHayatlari, {
					EgitimHayati_Id: aData[0]
				})[0];
				ehc.model = {
					EgitimHayati_Id: aData[0], Egitim_seviyesi: aData[2], Baslama_Tarihi: new Date(rs.Baslama_Tarihi),
					Bitis_Tarihi: new Date(rs.Bitis_Tarihi), Meslek_Tanimi: rs.Meslek_Tanimi, Okul_Adi: aData[3], Personel_Id: rs.Personel_Id
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'egitim-hayati', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
		});
		return nRow;
	}

	ehc.onSubmit = function () {
	    ehc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
	    if (angular.isUndefined(ehc.model.EgitimHayati_Id) || ehc.model.EgitimHayati_Id === null) {//yeni Kayıt
	        personellerSvc.AddPrsEgitimHayati(ehc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.EgitimHayatlari = s.data.EgitimHayatlari;
	            });
	            $scope.yeniehc();
	        });
	    } else {//düzenle
	        personellerSvc.UpdatePrsEgitimHayati(ehc.model.EgitimHayati_Id, ehc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.EgitimHayatlari = s.data.EgitimHayatlari;
	            });
	            $scope.yeniehc();
	        });
	    }
	};

	var deselect = function () {
		var table = $("#egitimHayati").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniEHC = function () {
		ehc.model = {};
		$scope.dosyaList = false;
		$scope.files = [];
	};

	$scope.silEHC = function () {
		personellerSvc.DeletePrsEgitimHayati(ehc.model.EgitimHayati_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.EgitimHayatlari = s.data.EgitimHayatlari;
			});
			$scope.yeniehc();
		});
	};

	$scope.files = [];

	$scope.dosyaList = false;

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/egitim-hayati/' + ehc.model.EgitimHayati_Id,
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

	$scope.$watch(function () { return ehc.model.EgitimHayati_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

egitimHayatiCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter',
'Upload', '$window', 'uploadService', 'ngAuthSettings'];

angular
	.module('inspinia')
	.controller('egitimHayatiCtrl', egitimHayatiCtrl);