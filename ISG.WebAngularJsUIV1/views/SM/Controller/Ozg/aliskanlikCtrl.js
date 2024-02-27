'use strict';

function aliskanlikCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, OgSvc, SMSvc, $q, $window) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var Alic = this;

	$scope.files = [];

	$scope.dosyaList = false;

	Alic.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	Alic.model = {

	};

	Alic.options = {};

	Alic.fields = [
				{
					className: 'col-sm-3',
					key: 'Madde',
					type: 'select',
					templateOptions: {
						label: 'Madde',
						required: true,
						options: [{ name: 'Tütün', value: 'Tütün' }, { name: 'Alkol', value: 'Alkol' }, { name: 'Esrar (marihuana) tipi', value: 'Esrar (marihuana) tipi' }, { name: 'Eroin,Morfin(Opioid)', value: 'Eroin,Morfin(Opioid)' },
							{ name: 'Barbitürat', value: 'Barbitürat' }, { name: 'Amfetamin', value: 'Amfetamin' }, { name: 'Kokain', value: 'Kokain' }, { name: 'Halüsinojen (LSD)', value: 'Halüsinojen (LSD)' }, { name: '‘Khat’ tipi bağımlılık', value: '‘Khat’ tipi bağımlılık' },
							{ name: 'Uçucu solvent tipi bağımlılık', value: 'Uçucu solvent tipi bağımlılık' }
						],
						valueProp: 'value',
						labelProp: 'name'
					}
				},
				{
					className: 'col-sm-2',
					key: 'BaslamaTarihi',
					type: 'datepicker',
					templateOptions: {
						label: 'Başlama Tarihi',
						required: true,
						type: 'text',
						datepickerPopup: 'dd.MMMM.yyyy',
						datepickerOptions: {
							format: 'dd.MM.yyyy'
						}
					}
				},
				{
					className: 'col-sm-2',
					key: 'BitisTarihi',
					type: 'datepicker',
					templateOptions: {
						label: 'Bitiş Tarihi',
						type: 'text',
						datepickerPopup: 'dd.MMMM.yyyy',
						datepickerOptions: {
							format: 'dd.MM.yyyy'
						}
					}
				},
				{
					className: 'col-sm-2',
					key: 'SiklikDurumu',
					type: 'select',
					templateOptions: {
						label: 'Kullanım Şekli',
						options: [{ name: 'Günde 1', value: 'Günde 1' }, { name: 'Günde 5', value: 'Günde 5' }, { name: 'Günde 10', value: 'Günde 10' }, { name: 'Günde 20', value: 'Günde 20' },
							{ name: 'Haftada 2-3', value: 'Haftada 2-3' }, { name: 'Ayda 4-5', value: 'Ayda 4-5' }, { name: 'Ayda 1-2', value: 'Ayda 1-2' }
						]
					}
				},
				{
					className: 'col-sm-3',
					type: 'textarea',
					key: 'Aciklama',
					templateOptions: {
						label: 'Açıklama',
						rows: 2
					}
				}
	];

	Alic.originalFields = angular.copy(Alic.fields);

	if ($stateParams.id === OgSvc.Oz.guidId) {
		$scope.Aliskanliklar = OgSvc.Oz.OzBilgi.data.Aliskanliklar;
	}
	else {
		OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
			OgSvc.Oz.OzBilgi = s;
			$scope.Aliskanliklar = s.data.Aliskanliklar;
			OgSvc.Oz.guidId = $stateParams.id;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsAlic = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'AliskanliklarListesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Aliskanliklar Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsAlic = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	Alic.onSubmit = function () {
		Alic.model.Personel_Id = OgSvc.Oz.OzBilgi.data.Personel_Id;
		if (angular.isUndefined(Alic.model.Aliskanlik_Id) || Alic.model.Aliskanlik_Id == null) {
			OgSvc.AddPrsAliskanlik(Alic.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.Aliskanliklar = s.data.Aliskanliklar;
				});
				$scope.yeniAlic();
			});
		}
		else {
			OgSvc.UpdatePrsAliskanlik(Alic.model.Aliskanlik_Id, Alic.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.Aliskanliklar = s.data.Aliskanliklar;
				});
				$scope.yeniAlic();
			});
		}
	};

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Aliskanliklar, {
					Aliskanlik_Id: aData[0]
				})[0];
				Alic.model = {
					Aliskanlik_Id: rs.Aliskanlik_Id, Madde: rs.Madde.trim(), BaslamaTarihi: new Date(rs.BaslamaTarihi), BitisTarihi: rs.BitisTarihi == null ? null : new Date(rs.BitisTarihi),
					SiklikDurumu: rs.SiklikDurumu.trim(), Aciklama: rs.Aciklama, Personel_Id: rs.Personel_Id, UserId: rs.UserId
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'aliskanlik', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#aliskanlik").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniAlic = function () {
		Alic.model = {}; $scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silAlic = function () {
		OgSvc.DeletePrsAliskanlik(Alic.model.Aliskanlik_Id).then(function (response) {
			OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
				OgSvc.Oz.OzBilgi = s;
				$scope.Aliskanliklar = s.data.Aliskanliklar;
			});
			$scope.yeniAlic();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/aliskanlik/' + Alic.model.Aliskanlik_Id,
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

	$scope.$watch(function () { return Alic.model.Aliskanlik_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

aliskanlikCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'OgSvc', 'SMSvc', '$q', '$window'];

angular
	.module('inspinia')
	.controller('aliskanlikCtrl', aliskanlikCtrl);