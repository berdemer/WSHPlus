'use strict';

function radyolojiCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var RdC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	RdC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	RdC.model = {
	};

	RdC.options = {};

	RdC.fields = [
		{
			className: 'row col-sm-12',
			fieldGroup: [
					  {
						  className: 'col-sm-3',
						  key: 'RadyolojikIslem',
						  type: 'ui-select-single',
						  templateOptions: {
							  label: 'Radyolojik İşlem',
							  placeholder: 'Radyolojik İşlemi Seçiniz!',
							  required: true,
							  optionsAttr: 'bs-options',
							  ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							  valueProp: 'Grafiler',
							  labelProp: 'Grafiler',
							  options: []
						  },
						  controller: function ($scope, TkSvc) {
						      var rad = [];
						      $scope.to.loading = TkSvc.RadyolojiTanimlari().then(function (response) {
						            $scope.to.options = response;
						            return response;
						        });          
						    }
					  },
					  {
						  className: 'col-sm-9'
					  }
			]
		},
		{
			type: 'textarea',
			key: 'RadyolojikSonuc',
			templateOptions: {
			    label: 'Sonuç',
			    required: true,
				placeholder: ''
			}
		}
	];

	RdC.originalFields = angular.copy(RdC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Radyolojileri = TkSvc.Tk.TkBilgi.data.Radyolojileri;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Radyolojileri = s.data.Radyolojileri;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsRdC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Boy Kilo Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Boy Kilo  Listesi', titleAttr: 'PDF' },
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
	 .withPaginationType('full')
	 .withSelect(true)
	 .withOption('order', [1, 'desc'])
	 .withOption('lengthMenu', [3, 10, 20, 50, 100])
	 .withOption('rowCallback', rowCallback)
	 .withOption('responsive', true);

	$scope.dtColumnDefsRdC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	RdC.onSubmit = function () {
		RdC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(RdC.model.Radyoloji_Id) || RdC.model.Radyoloji_Id == null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				RdC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsRadyoloji(RdC.model, TkSvc.Tk.SaglikBirimi_Id, RdC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Radyolojileri = s.data.Radyolojileri;
					});
					$scope.yeniRdC();
				});
			} else {
				notify({
					message: 'Sağlık Birimini Girmeden Kayıt Yapamazsınız.',
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 2000,
					position: 'right'
				});
			}
		}
		else {
			TkSvc.UpdatePrsRadyoloji(RdC.model.Radyoloji_Id, RdC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Radyolojileri = s.data.Radyolojileri;
				});
				$scope.yeniRdC();
			}).catch(function (e) {
			    $scope.message = "Hata Kontrol Edin! " + e;
			    startTimer();
			});
		}
	};
	var startTimer = function () {
	    var timer = $timeout(function () {
	        $timeout.cancel(timer);
	        $scope.message = "";
	    }, 4000);
	};
	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Radyolojileri, {
					Radyoloji_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru == null ? '' : rs.MuayeneTuru.trim();
				RdC.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'radyoloji', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#radyoloji").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniRdC = function () {
		RdC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silRdC = function () {
		TkSvc.DeletePrsRadyoloji(RdC.model.Radyoloji_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Radyolojileri = s.data.Radyolojileri;
			});
			$scope.yeniRdC();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin! " + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/radyoloji/' + RdC.model.Radyoloji_Id,
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

	$scope.$watch(function () { return RdC.model.Radyoloji_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});
}
radyolojiCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('radyolojiCtrl', radyolojiCtrl);