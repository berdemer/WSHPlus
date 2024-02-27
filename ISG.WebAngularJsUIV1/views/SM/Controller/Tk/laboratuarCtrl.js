'use strict';

function laboratuarCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify, $uibModal) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var LbC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	LbC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	LbC.model = {
	};

	LbC.options = {};

	TkSvc.TetkikTanimlari().then(function (response) {
	    var lab = []; var labor = [];
	    LbC.tetkikTanimlari = response;
	    angular.forEach(response, function (v) {
	        if (lab.indexOf(v.laboratuar) === -1) {
	            lab.push(v.laboratuar);
	        }
	    });
	    angular.forEach(lab, function (v) {
	        labor.push({ name: v, value: v });
	    });
	    LbC.fields = [
			{
			    className: 'row col-sm-12',
			    fieldGroup: [
						  {
						      className: 'col-sm-3',
						      key: 'Grubu',
						      type: 'ui-select-single',
						      defaultValue: labor[0].value,
						      templateOptions: {
						          label: 'Laboratuvar',
						          placeholder: 'Laboratuvarı Seçiniz!',
						          optionsAttr: 'bs-options',
						          ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						          valueProp: 'value',
						          labelProp: 'name',
						          options: labor
						      }
						  },
						  {
						      className: 'col-sm-9'
						  }
			    ]
			},
			{
			    className: 'row col-sm-12',
			    fieldGroup: [
					   {
					       className: 'row col-sm-12',
					       key: 'Sonuc',
					       type: 'ui-select-multiple',
					       templateOptions: {
					           optionsAttr: 'bs-options',
					           ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
					           label: 'Tetkikler',
					           required: true,
					           valueProp: 'tetkik',
					           labelProp: 'tetkik',
					           placeholder: 'Tetkiklerinizi Seçiniz!',
					           options: []
					       },
					       controller: function ($scope) {
					           $scope.$watch(function () { return LbC.model.Grubu; }, function (newValue, oldValue, theScope) {
					               if (!angular.isUndefined(newValue) || newValue !== null) {
					                   if ($scope.model[$scope.options.key] && oldValue) {
					                       $scope.model[$scope.options.key] = '';
					                   }
					                   $scope.to.options = $filter('filter')(LbC.tetkikTanimlari, {
					                       laboratuar: newValue
					                   });
					               }
					           });
					       }
					   }
			    ]
			}
	    ];
	});
	LbC.originalFields = angular.copy(LbC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Laboratuarlari = TkSvc.Tk.TkBilgi.data.Laboratuarlari;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Laboratuarlari = s.data.Laboratuarlari;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsLbC = DTOptionsBuilder.newOptions()
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

	$scope.dtColumnDefsLbC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	LbC.onSubmit = function () {
		LbC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		$scope.kayit = true;
		var hg = [];
		angular.forEach(LbC.model.Sonuc, function (v) {
		    hg.push(v.tetkik === undefined ? v : v.tetkik);
		});
		LbC.model.Sonuc = hg.toString();
		if (angular.isUndefined(LbC.model.Laboratuar_Id) || LbC.model.Laboratuar_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				LbC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsLaboratuar(LbC.model, TkSvc.Tk.SaglikBirimi_Id, LbC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Laboratuarlari = s.data.Laboratuarlari;
					});
					$scope.yeniLbC();
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
			TkSvc.UpdatePrsLaboratuar(LbC.model.Laboratuar_Id, LbC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Laboratuarlari = s.data.Laboratuarlari;
				});
				$scope.yeniLbC();
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
				var rs = $filter('filter')($scope.Laboratuarlari, {
					Laboratuar_Id : aData[0]
				})[0];
				var tetkiklerx = [];
				angular.forEach(rs.Sonuc.split(","), function (v) {
				    tetkiklerx.push({ tetkik: v });
				});
				LbC.model = {
					id: aData[1], Personel_Id: rs.Personel_Id, UserId: rs.UserId, Sonuc: rs.Sonuc === "" ? [{ tetkik: "Tetkik Kaydı Yok" }] : tetkiklerx,
					MuayeneTuru: angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim(),
					Laboratuar_Id:rs.Laboratuar_Id,RevirIslem_Id:rs.RevirIslem_Id,Protokol:rs.Protokol,Tarih:new Date(rs.Tarih)
				}; 
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'laboratuar', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#laboratuar").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniLbC = function () {
		LbC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		
	};

	$scope.silLbC = function () {
		TkSvc.DeletePrsLaboratuar(LbC.model.Laboratuar_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Laboratuarlari = s.data.Laboratuarlari;
			});
			$scope.yeniLbC();
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! " + e;
			startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/laboratuar/' + LbC.model.Laboratuar_Id,
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

	$scope.$watch(function () { return LbC.model.Laboratuar_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

	$scope.kayit = false;

	$scope.$watch('[LbC.model.Sonuc,LbC.model.Laboratuar_Id]', function (newVal, oldVal) {
		if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
			var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
			var oldVal0Len = oldVal[0] === undefined ? 0 : oldVal[0].length;
			var tetkikler = [];
			if (newVal0Len < oldVal0Len && newVal[1] === oldVal[1]) {
				tetkikler = [];
				angular.forEach(newVal[0], function (v) {
				    tetkikler.push({ tetkik: v });
				});
				LbC.model.Sonuc = tetkikler;
			}
			if (newVal0Len > oldVal0Len && newVal[1] === oldVal[1] && $scope.kayit === false) {
				tetkikler = [];
				angular.forEach(newVal[0], function (v) {
					tetkikler.push({ tetkik: v.toString().replace(',', '.') });
				});
				var last = tetkikler[tetkikler.length - 1].tetkik;
				$scope.deger = { tetkik: last, giris: null };
				$uibModal.open({
					templateUrl: 'degerKaydi.html',
					backdrop: true,
					animation: true,
					size: 'sm',
					windowClass: "animated flipInY",
					controller: function ($scope, $uibModalInstance, $log, deger) {
						$scope.deger = deger;
						$scope.submittenx = function () {
						    tetkikler[tetkikler.length - 1].tetkik = last + (deger.giris === null ? "" : " ( Sonuç : " + deger.giris + " ) ");
						    LbC.model.Sonuc = tetkikler;
						    $uibModalInstance.dismiss('cancel');
						};
						$scope.cancel = function () {
							$uibModalInstance.dismiss('cancel');
						};
					},
					resolve: {
						deger: function () {
							return $scope.deger;
						}
					}
				});
			}
			$scope.kayit = false;
		}
	}, true);
}
laboratuarCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify', '$uibModal'];

angular
	.module('inspinia')
	.controller('laboratuarCtrl', laboratuarCtrl);