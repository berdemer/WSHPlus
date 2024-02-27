'use strict';

function kkdCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder,
	DTColumnDefBuilder, $filter, Upload, $timeout, ngAuthSettings, uploadService, authService, $window, OgSvc, SMSvc, $q) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var kkdC = this;

	$scope.files = [];

	$scope.dosyaList = false;

	kkdC.model = {
	};

	kkdC.options = {};

	kkdC.fields = [
				{
					className: 'row col-sm-12',
					fieldGroup: [
								{
									className: 'col-sm-5',
									type: 'input',
									key: 'Kkd_Tanimi',
									templateOptions: {
										required: true,
										type: 'input',
										label: 'Kişisel Koruyucu Tanımı'
									}
								},
								{
									className: 'col-sm-5',
									type: 'input',
									key: 'Maruziyet',
									templateOptions: {
										required: true,
										type: 'input',
										label: 'İş Maruziyeti'
									}
								},
								{
									className: 'col-sm-2',
									type: 'input',
									key: 'Guncelleme_Suresi_Ay',
									templateOptions: {
										required: true,
										type: 'number',
										label: 'Kullanım Süresi'
									}
								}
					]
				},
				{
					className: 'row col-sm-12',
					fieldGroup: [
					 {
						 className: 'col-sm-6',
						 type: 'textarea',
						 key: 'Aciklama',
						 templateOptions: {
							 label: 'Açıklama',
							 rows: 2
							}
					 },
					 {
						 className: 'col-sm-2'
					 },
					 {
						 className: 'col-sm-4'
					 }
					]
				}
	];

	kkdC.originalFields = angular.copy(kkdC.fields);

	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.Kkdleri = personellerSvc.MoviesIds.personel.data.KkdLeri;
	}
	else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.Kkdleri = s.data.KkdLeri;
		});
	}

	$scope.isCollapsed = true;
	$scope.dtOptionskkdC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'KkdLeri Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'KkdLeri Listesi', titleAttr: 'PDF' },
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
	 .withOption('responsive', true);

	$scope.dtColumnDefskkdC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Kkdleri, {
					Kkd_Id: aData[0]
				})[0];
				kkdC.model = rs;
					//{
					//Kkd_Id: aData[0], Kkd_Tanimi: rs.Kkd_Tanimi, Alinma_Tarihi: rs.Alinma_Tarihi, Maruziyet: rs.Maruziyet, Guncelleme_Suresi_Ay: rs.Guncelleme_Suresi_Ay, Aciklama: rs.Aciklama, Personel_Id: rs.Personel_Id
					//};

				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'kkd', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
		});
		return nRow;
	}

	kkdC.onSubmit = function () {
	    kkdC.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
	    if (angular.isUndefined(kkdC.model.Kkd_Id) || kkdC.model.Kkd_Id === null) {
	        personellerSvc.AddPrsKkd(kkdC.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.Kkdleri = s.data.KkdLeri;
	            });
	            $scope.yenikkdC();
	        });
	    } else {
	        personellerSvc.UpdatePrsKkd(kkdC.model.Kkd_Id, kkdC.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.Kkdleri = s.data.KkdLeri;
	            });
	            $scope.yenikkdC();
	        });
	    }
	};

	var deselect = function () {
		var table = $("#kkd").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yenikkdC = function () {
		kkdC.model = {}; $scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silkkdC = function () {
		personellerSvc.DeletePrsKkd(kkdC.model.Kkd_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.Kkdleri = s.data.KkdLeri;
			});
			$scope.yenikkdC();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/kkd/' + kkdC.model.Kkd_Id,
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
		uploadService.DeleteFile(file+'/x').then(function (response) {
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

	$scope.$watch(function () { return kkdC.model.Kkd_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

kkdCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc',
	'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$timeout',
	'ngAuthSettings', 'uploadService', 'authService', '$window', 'OgSvc', 'SMSvc', '$q'];

angular
	.module('inspinia')
	.controller('kkdCtrl', kkdCtrl);