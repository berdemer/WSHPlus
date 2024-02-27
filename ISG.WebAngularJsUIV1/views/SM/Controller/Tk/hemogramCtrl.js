'use strict';

function hemogramCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify, $uibModal) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var hmgPrms = [
        { prt: 'Eritrosit', sc: null, kst: 'RBC (Red Blood Cell, kırmızı kan hücresi, eritrosit)' },
		{ prt: 'Hematokrit', sc: null, kst: 'Hct (Hematokrit; PCV: Packed Cell Volume)' }, { prt: 'Hemoglobin', sc: null, kst: 'Hgb (Hemoglobin)' },
		{ prt: 'MCV', sc: null, kst: 'MCV (Mean Corpuscular Volume)' },
		{ prt: 'MCH', sc: null, kst: '	MCH (Mean Corpuscular Hemoglobin)' }, { prt: 'MCHC', sc: null, kst: 'MCHC (Mean Corpuscular Hemoglobin Concentration)' },
		{ prt: 'RDW', sc: null, kst: 'RDW (Red cell Distribution Width, Eritrosit dağılım genişliği)' },
		{ prt: 'Lokosit', sc: null, kst: 'WBC (White Blood Cell, beyaz kan hücresi, lökosit)' },
		{ prt: 'Lenfosit_Yuzde', sc: null, kst: 'Lym % ve # (Lenfosit % ve sayı)' },
		{ prt: 'Monosit_Yuzde', sc: null, kst: 'Mono %  ve # (Monosit % ve sayı)' },
		{ prt: 'Granülosit_Yuzde', sc: null, kst: 'Gran % ve # (Granülosit % ve sayı)' },
		{ prt: 'Notrofil_Yuzde', sc: null, kst: '' }, { prt: 'Eoznofil_Yuzde', sc: null, kst: '' }, { prt: 'Bazofil_Yuzde', sc: null, kst: '' }, { prt: 'Trombosit', sc: null, kst: 'Plt (Trombosit)' },
		{ prt: 'MeanPlateletVolume', sc: null, kst: '	MPV (Mean Platelet Volume)' },
		{ prt: 'Platekrit', sc: null, kst: 'Pct (Platekrit)' }, { prt: 'PDW', sc: null, kst: 'PDW (Platelet Distribution Width, Trombosit dağılım genişliği)' }];

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var HmC = this;

	$scope.Hem = function (Eritrosit, Hematokrit, Hemoglobin, Lokosit, MCV,
        MCH, MCHC, Trombosit, Platekrit,Normal) {
		var a = Eritrosit !== null ? 'RBC:' + Eritrosit : '';
		var b = Hemoglobin !== null ? ',Hgb:' + Hemoglobin : '';
		var c = Hematokrit !== null ? ',Hct:' + Hematokrit : '';
		var d = MCV !== null ? ',MCV:' + MCV : '';
		var e = MCH !== null ? ',MCH:' + MCH : '';
		var f = MCHC !== null ? ',MCHC:' + MCHC : '';
		var g = Lokosit !== null ? ',WBC:' + Lokosit : '';
		var h = Trombosit !== null ? ',Plt:' + Trombosit : '';
		var i = Platekrit !== null ? ',Pct:' + Platekrit : '';
		return a + b + c + d + e + f + g + h + i;
	};

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	HmC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	HmC.model = {
	};

	HmC.options = {};

	HmC.fields = [
					{
						className: 'col-sm-12',
						key: 'hm',
						type: 'ui-select-multiplex',
						templateOptions: {
							optionsAttr: 'bs-options',
							ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							label: 'Hemogram Girişi',
							valueProp: 'prt',
							labelProp: 'prt',
							labelProp1: 'kst',
							required: true,
							placeholder: 'Parametre Arama',
							options: hmgPrms
						}
					}
	];

	HmC.originalFields = angular.copy(HmC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Hemogramlar = TkSvc.Tk.TkBilgi.data.Hemogramlar;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Hemogramlar = s.data.Hemogramlar;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsHmC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Hemogram Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Hemogram  Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsHmC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	HmC.onSubmit = function () {
		$scope.kayit = true;
		HmC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(HmC.model.Hemogram_Id) || HmC.model.Hemogram_Id === null) {
		    angular.forEach(HmC.model.hm, function (v) {
		        HmC.model[v.key] = v.value;
		    });
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				HmC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsHemogram(HmC.model, TkSvc.Tk.SaglikBirimi_Id, HmC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Hemogramlar = s.data.Hemogramlar;
					});
					$scope.yeniHmC();
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
		    var guncel = { Sonuc: HmC.model.Sonuc , MuayeneTuru: HmC.model.MuayeneTuru , RevirIslem_Id: HmC.model.RevirIslem_Id 
		    , Personel_Id: HmC.model.Personel_Id , Protokol: HmC.model.Protokol , Tarih: HmC.model.Tarih , UserId: HmC.model.UserId , Hemogram_Id: HmC.model.Hemogram_Id };
		    angular.forEach(HmC.model.hm, function (v) {
		        var a = v.prt.toString().split(':');
		        var c = {};
		        c[a[0]]=a[1];
		        angular.extend(guncel,c);
		    });
			TkSvc.UpdatePrsHemogram(HmC.model.Hemogram_Id, guncel).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Hemogramlar = s.data.Hemogramlar;
				});
				$scope.yeniHmC();
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
				var rs = $filter('filter')($scope.Hemogramlar, {
					Hemogram_Id: aData[0]
				})[0];
				rs.hm = [];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				HmC.model = rs;
				rs.Eritrosit!==null? HmC.model.hm.push({ prt: 'Eritrosit:' + rs.Eritrosit, key: 'Eritrosit', value: rs.Eritrosit }):'';
				rs.Hematokrit!==null?HmC.model.hm.push({ prt: 'Hematokrit:' + rs.Hematokrit, key: 'Hematokrit', value: rs.Hematokrit }):'';
				rs.Hemoglobin!==null?HmC.model.hm.push({ prt: 'Hemoglobin:' + rs.Hemoglobin, key: 'Hemoglobin', value: rs.Hemoglobin }):'';
				rs.MCV!==null?HmC.model.hm.push({ prt: 'MCV:' + rs.MCV, key: 'MCV', value: rs.MCV }):'';
				rs.MCH!==null?HmC.model.hm.push({ prt: 'MCH:' + rs.MCH, key: 'MCH', value: rs.MCH }):'';
				rs.MCHC!==null?HmC.model.hm.push({ prt: 'MCHC:' + rs.MCHC, key: 'MCHC', value: rs.MCHC }):'';
				rs.RDW!==null?HmC.model.hm.push({ prt: 'RDW:' + rs.RDW, key: 'RDW', value: rs.RDW }):'';
				rs.Lokosit!==null?HmC.model.hm.push({ prt: 'Lokosit:' + rs.Lokosit, key: 'Lokosit', value: rs.Lokosit }):'';
				rs.Lenfosit_Yuzde!==null?HmC.model.hm.push({ prt: 'Lenfosit_Yuzde:' + rs.Lenfosit_Yuzde, key: 'Lenfosit_Yuzde', value: rs.Lenfosit_Yuzde }):'';
				rs.Monosit_Yuzde!==null?HmC.model.hm.push({ prt: 'Monosit_Yuzde:' + rs.Monosit_Yuzde, key: 'Monosit_Yuzde', value: rs.Monosit_Yuzde }):'';
				rs.Granülosit_Yuzde!==null?HmC.model.hm.push({ prt: 'Granülosit_Yuzde:' +rs.Granülosit_Yuzde , key: 'Granülosit_Yuzde', value: rs.Granülosit_Yuzde }):'';
				rs.Notrofil_Yuzde!==null?HmC.model.hm.push({ prt: 'Notrofil_Yuzde:' +rs.Notrofil_Yuzde , key: 'Notrofil_Yuzde', value: rs.Notrofil_Yuzde }):'';
				rs.Eoznofil_Yuzde!==null?HmC.model.hm.push({ prt: 'Eoznofil_Yuzde:' + rs.Eoznofil_Yuzde, key: 'Eoznofil_Yuzde', value: rs.Eoznofil_Yuzde }):'';
				rs.Bazofil_Yuzde!==null?HmC.model.hm.push({ prt: 'Bazofil_Yuzde:' + rs.Bazofil_Yuzde, key: 'Bazofil_Yuzde', value: rs.Bazofil_Yuzde }):'';
				rs.Trombosit!==null?HmC.model.hm.push({ prt: 'Trombosit:' + rs.Trombosit, key: 'Trombosit', value: rs.Trombosit }):'';
				rs.MeanPlateletVolume!==null?HmC.model.hm.push({ prt: 'MeanPlateletVolume:' + rs.MeanPlateletVolume, key: 'MeanPlateletVolume', value: rs.MeanPlateletVolume }):'';
				rs.Platekrit!==null?HmC.model.hm.push({ prt: 'Platekrit:' + rs.Platekrit, key: 'Platekrit', value: rs.Platekrit }):'';
				rs.PDW!==null?HmC.model.hm.push({ prt: 'PDW:' + rs.PDW, key: 'PDW', value: rs.PDW }):'';

				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'hemogram', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#hemogram").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniHmC = function () {
		HmC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silHmC = function () {
		TkSvc.DeletePrsHemogram(HmC.model.Hemogram_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Hemogramlar = s.data.Hemogramlar;
			});
			$scope.yeniHmC();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin! " + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/hemogram/' + HmC.model.Hemogram_Id,
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

	$scope.$watch(function () { return HmC.model.Hemogram_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});
	
	$scope.kayit = false;
	$scope.$watch('[HmC.model.hm,HmC.model.Hemogram_Id]', function (newVal, oldVal) {
		if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
			var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
			var oldVal0Len = oldVal[0] === undefined ? 0 : oldVal[0].length;
			if (newVal0Len < oldVal0Len && newVal[1] === oldVal[1]) {
				var hemograms = [];
				angular.forEach(newVal[0], function (v) {
					var a = newVal.toString().split(':');
					hemograms.push({ prt: v, key: a[0], value: a[1] });
				});
				HmC.model.hm = hemograms;
			}
			if (newVal0Len > oldVal0Len && newVal[1] === oldVal[1] && $scope.kayit === false) {
				var hemograms = [];
				angular.forEach(newVal[0], function (v) {
				    var a = v.toString().split(':');
					hemograms.push({ prt: v, key: a[0],value:a[1]});
				});
				var last = hemograms[hemograms.length - 1].prt;
				$scope.sonuc = { prt: last, giris: null };
				$uibModal.open({
					templateUrl: 'sonucKaydi.html',
					backdrop: true,
					animation: true,
					size: 'sm',
					windowClass: "animated flipInY",
					controller: function ($scope, $uibModalInstance, $log, sonuc) {
						$scope.sonuc = sonuc;
						$scope.submittenx = function () {
						    hemograms[hemograms.length - 1].prt = last + (sonuc.giris === null ? "" : ":" + sonuc.giris);
						    hemograms[hemograms.length - 1].key = last;
						    hemograms[hemograms.length - 1].value = sonuc.giris === null ? null : sonuc.giris;
						    HmC.model.hm = hemograms;
						    $uibModalInstance.dismiss('cancel');
						};
						$scope.cancel = function () {
							$uibModalInstance.dismiss('cancel');
						};
					},
					resolve: {
						sonuc: function () {
							return $scope.sonuc;
						}
					}
				});
			}
			$scope.kayit = false;
		}
	}, true);
}
hemogramCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify', '$uibModal'];

angular
	.module('inspinia')
	.controller('hemogramCtrl', hemogramCtrl);