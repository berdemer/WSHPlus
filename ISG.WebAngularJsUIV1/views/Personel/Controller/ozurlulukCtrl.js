'use strict';

function ozurlulukCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder,
	DTColumnDefBuilder, $filter, Upload, $timeout, ngAuthSettings, uploadService, authService, $window, OgSvc, SMSvc, $q) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var ozurc = this;

	$scope.files = [];

	$scope.dosyaList = false;
	ozurc.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	ozurc.model = {
		Ozurluluk_Id: undefined,HastalikTanimi: undefined, Oran: undefined, Derecesi: undefined, HastahaneAdi: undefined, Aciklama: undefined, Personel_Id: undefined
	};

	ozurc.options = {};

	ozurc.fields = [
				{
					className: 'row col-sm-12',
					fieldGroup: [
								{
									className: 'col-sm-6',
									type: 'typhead',
									key: 'HastalikTanimi',
									templateOptions: {
										label: 'Hastalığın Adı',
										required: true,
										options: [],
										onKeyup: function ($viewValue, $scope) {
											if (typeof $viewValue !== 'undefined') {
												return $scope.templateOptions.options = ozurc.HastalikTanimiList($viewValue);
											}
										}
									}
								},
								{
									className: 'col-sm-2',
									type: 'input',
									key: 'Oran',
									templateOptions: {
										required: true,
										type: 'number',
										min:0,
										label: 'Oranı'
									}
								},
								{
									className: 'col-sm-4',
									key: 'Derecesi',
									type: 'ui-select-single',
									templateOptions: {
										label: 'Derecesi',
										optionsAttr: 'bs-options',
										ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
										valueProp: 'value',
										labelProp: 'name',
										options: [{ name: '3. derece %40-60 arası', value: '3. Derece' }, { name: '2. derece %60-80 arası', value: '2. Derece' }, { name: '1. derece %80-100 arası', value: '1. Derece' }]
									}
								}
					]
				},
				{
					className: 'row col-sm-12',
					fieldGroup: [
					 {
						 className: 'col-sm-4',
						 type: 'textarea',
						 key: 'HastahaneAdi',
						 templateOptions: {
							 label: 'Hastahane Adı'
							}
					 },
					 {
						 className: 'col-sm-4',
						 type: 'textarea',
						 key: 'Aciklama',
						 templateOptions: {
							 label: 'Açıklama'
						 }
					 },
					 {
						 className: 'col-sm-4'
					 }
					]
				}
	];

	ozurc.originalFields = angular.copy(ozurc.fields);

	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.Ozurlulukler = personellerSvc.MoviesIds.personel.data.Ozurlulukler;
	} else
		if ($stateParams.id === OgSvc.Oz.guidId) {
		$scope.Ozurlulukler = OgSvc.Oz.OzBilgi.data.Ozurlulukler;
	}
	else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.Ozurlulukler = s.data.Ozurlulukler;
		});
	}
	$scope.isCollapsed = true;
	$scope.dtOptionsozurc = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Ozurlulukler Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Ozurlulukler Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsozurc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Ozurlulukler, {
					Ozurluluk_Id: aData[0]
				})[0];
				ozurc.model = {
					Ozurluluk_Id: aData[0], HastalikTanimi: aData[2], Oran:rs.Oran, Derecesi: aData[4], HastahaneAdi: aData[5], Aciklama: aData[6], Personel_Id: rs.Personel_Id
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'ozurluluk', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
		});
		return nRow;
	}

	ozurc.onSubmit = function () {
	    ozurc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id === undefined ? OgSvc.Oz.OzBilgi.data.Personel_Id : personellerSvc.MoviesIds.personel.data.Personel_Id;
	    if (angular.isUndefined(ozurc.model.Ozurluluk_Id) || ozurc.model.Ozurluluk_Id === null) {
	        personellerSvc.AddPrsOzurluluk(ozurc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.Ozurlulukler = s.data.Ozurlulukler;
	            });
	            $scope.yeniozurc();
	        });
	    } else {
	        personellerSvc.UpdatePrsOzurluluk(ozurc.model.Ozurluluk_Id, ozurc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.Ozurlulukler = s.data.Ozurlulukler;
	            });
	            $scope.yeniozurc();
	        });
	    }
	};

	var deselect = function () {
	  var  table = $("#ozur").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniozurc = function () {
		ozurc.model = {}; $scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silozurc = function () {
		personellerSvc.DeletePrsOzurluluk(ozurc.model.Ozurluluk_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.Ozurlulukler = s.data.Ozurlulukler;
			});
			$scope.yeniozurc();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/ozurluluk/' + ozurc.model.Ozurluluk_Id,
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

	$scope.$watch(function () { return ozurc.model.Ozurluluk_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

ozurlulukCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc',
	'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$timeout',
	'ngAuthSettings', 'uploadService', 'authService', '$window', 'OgSvc', 'SMSvc', '$q'];

angular
	.module('inspinia')
	.controller('ozurlulukCtrl', ozurlulukCtrl);