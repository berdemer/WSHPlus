'use strict';
function calismaGecmisiCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder, DTColumnDefBuilder, $filter
	, Upload, $window, uploadService, ngAuthSettings) {
	var cgc = this;
	cgc.model = {
		Calisma_Gecmisi_Id: undefined, Calistigi_Yer_Adi: undefined, Ise_Baslama_Tarihi: undefined,
		Isden_Cikis_Tarihi: undefined, Gorevi: undefined, Unvani: undefined, Personel_Id: undefined
	};

	cgc.onSubmit = function () {
	    cgc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
	    if (angular.isUndefined(cgc.model.Calisma_Gecmisi_Id) || cgc.model.Calisma_Gecmisi_Id === null) {//yeni Kayıt
	        personellerSvc.AddPrsCalisma_Gecmisi(cgc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.CalismaGecmisleri = s.data.Calisma_Gecmisi;
	            });
	            $scope.yeniCGC();
	        });
	    } else {//düzenle
	        personellerSvc.UpdatePrsCalisma_Gecmisi(cgc.model.Calisma_Gecmisi_Id, cgc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.CalismaGecmisleri = s.data.Calisma_Gecmisi;
	            });
	            $scope.yeniCGC();
	        });
	    }
	};
	cgc.options = {};

	cgc.fields = [
				{
					className: 'col-sm-2',
					key: 'Ise_Baslama_Tarihi',
					type: 'datepicker',
					templateOptions: {
						label: 'İşe Başlama Tarihi',
						type: 'text',
						required: true,
						datepickerPopup: 'dd.MMMM.yyyy',
						datepickerOptions: {
							format: 'dd.MM.yyyy'
						}
					}
				},
				{
					className: 'col-sm-2',
					key: 'Isden_Cikis_Tarihi',
					type: 'datepicker',
					templateOptions: {
						label: 'İş Çıkış Tarihi',
						type: 'text',
						datepickerPopup: 'dd.MMMM.yyyy',
						datepickerOptions: {
							format: 'dd.MM.yyyy'
						}
					}
				},
				{
					className: 'col-sm-2',
					type: 'input',
					key: 'Gorevi',
					templateOptions: {
						type: 'input',
						label: 'Görevi'
					}
				},
				{
					className: 'col-sm-2',
					type: 'input',
					key: 'Unvani',
					templateOptions: {
						type: 'input',
						label: 'Ünvanı'
					}
				},
				{
					className: 'col-sm-4',
					type: 'textarea',
					key: 'Calistigi_Yer_Adi',
					templateOptions: {
						label: 'Çalıştığı Şirket',
						required: true
					}
				}
	];

	cgc.originalFields = angular.copy(cgc.fields);


	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.CalismaGecmisleri = personellerSvc.MoviesIds.personel.data.Calisma_Gecmisi;
	} else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.CalismaGecmisleri = s.data.Calisma_Gecmisi;
		});
	}
	$scope.isCollapsed = true;
	$scope.dtOptionsg = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Calisma Gecmisleri Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Calisma Gecmisleri Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsg = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.CalismaGecmisleri, {
					Calisma_Gecmisi_Id: aData[0]
				})[0];
				cgc.model = {
					Calisma_Gecmisi_Id: aData[0], Calistigi_Yer_Adi: aData[2], Ise_Baslama_Tarihi: new Date(rs.Ise_Baslama_Tarihi),
					Isden_Cikis_Tarihi: new Date(rs.Isden_Cikis_Tarihi), Gorevi: aData[5], Unvani: aData[6], Personel_Id: rs.Personel_Id
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'calisma-gecmisi', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
		});
		return nRow;
	}

	var deselect = function () {
		var table = $("#calismaGecmisi").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniCGC = function () {
		cgc.model = {};
		$scope.dosyaList = false;
		$scope.files = []; deselect();
	};

	$scope.silCGC = function () {
		personellerSvc.DeletePrsCalisma_Gecmisi(cgc.model.Calisma_Gecmisi_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.CalismaGecmisleri = s.data.Calisma_Gecmisi;
			});
			$scope.yeniCGC();
		});
	};

	$scope.files = [];

	$scope.dosyaList = false;

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/calisma-gecmisi/' + cgc.model.Calisma_Gecmisi_Id,
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

	$scope.$watch(function () { return cgc.model.Calisma_Gecmisi_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

calismaGecmisiCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter',
'Upload', '$window', 'uploadService', 'ngAuthSettings'];

angular
	.module('inspinia')
	.controller('calismaGecmisiCtrl', calismaGecmisiCtrl);