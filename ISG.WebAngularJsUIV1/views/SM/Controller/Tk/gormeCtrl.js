'use strict';

function gormeCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var GrC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	GrC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	GrC.model = {
	};

	GrC.options = {};

	GrC.fields = [
		{
			className: 'row col-sm-12',
			fieldGroup: [
				{
					className: 'col-sm-3',
					key: 'GozKapagi',
					type: 'ui-select-single',
					defaultValue: 'Normal',
					templateOptions: {
						label: 'G.Kapakları',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Normal' }, { v: 'Her iki kapak şiş' }, { v: 'Sag Şiş' }, { v: 'Sol Şiş' }, { v: 'Her iki kapak kızarık' }, { v: 'Sağ Kızarık' }, { v: 'Sol Kızarık' }]
					}
				},
				{
					className: 'col-sm-3',
					key: 'GozKuresi',
					type: 'ui-select-single',
					defaultValue: 'Normal',
					templateOptions: {
						label: 'G.Küresi',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Normal' }, { v: 'Sağda Çökme' }, { v: 'Solda Çökme' }, { v: 'Her ikisinde çökme' }, { v: 'sağda çıkma' }, { v: 'solda çıkma' }, { v: 'Her ikisinde çıkma' } ]
					}
				},
				{
					className: 'col-sm-3',
					key: 'GozKaymasi',
					type: 'ui-select-single',
					defaultValue: 'Yok',
					templateOptions: {
						label: 'G.Kayma',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Yok' }, { v: 'Sağ içe kayma' }, { v: 'Sol içe kayma' }, { v: 'Her iki gözde içe kayma' },
							{ v: 'Sağ dışa kayma' }, { v: 'Sol dışa kayma' }, {v:'Her iki gözde dşa kayma'} ]
					}
				},
				{
					className: 'col-sm-3',
					key: 'GozdeKizariklik',
					type: 'ui-select-single',
					defaultValue: 'Yok',
					templateOptions: {
						label: 'G.Kızarıklık',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Sağda Kızarıklık' }, { v: 'Solda Kızarıklık' }, { v: 'Her iki gözde kızarıklık' }, { v: 'Yok' }]
					}
				}
			]
		},
		{
			className: 'row col-sm-12',
			fieldGroup: [
					
					{
						className: 'col-sm-3',
						key: 'IsikRefleksi',
						type: 'ui-select-single',
						defaultValue: 'Var',
						templateOptions: {
							label: 'Işık Refleksi',
							optionsAttr: 'bs-options',
							ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							valueProp: 'v',
							labelProp: 'v',
							options: [{ v: 'Var' }, { v: 'Sağda yok' }, { v: 'Solda Yok' }, { v: 'Her ikisinde yok' }]
						}
					},
					{
						className: 'col-sm-3',
						key: 'GormeAlani',
						type: 'ui-select-single',
						defaultValue: 'Normal',
						templateOptions: {
							label: 'Görme Alanı',
							optionsAttr: 'bs-options',
							ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							valueProp: 'v',
							labelProp: 'v',
							options: [{ v: 'Normal' }, { v: '%10 Kayıp' }, { v: '%20 Kayıp' }, { v: '%40 Kayıp' }, { v: '%60 dan fazla Kayıp' }]
						}
					},
					{
						className: 'col-sm-3',
						key: 'GozTonusu',
						type: 'ui-select-single',
						defaultValue: 'Normal',
						templateOptions: {
							label: 'Göz Tonusu',
							optionsAttr: 'bs-options',
							ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							valueProp: 'v',
							labelProp: 'v',
							options: [{ v: 'Normal' }, { v: 'Hipotonus (Yumuşak)' }, { v: 'Hipertonus (Sert)' }]
						}
					},
					{
						className: 'col-sm-3',
						key: 'PupillaninDurumu',
						type: 'ui-select-single',
						defaultValue: 'Normal',
						templateOptions: {
							label: 'Pupillanın Durumu ',
							optionsAttr: 'bs-options',
							ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							valueProp: 'v',
							labelProp: 'v',
							options: [{ v: 'Normal' }, { v: 'Sağ Küçük' }, { v: 'Sol Küçük' }, { v: 'Her ikisi küçük' }, { v: 'Sağ Büyük' }, { v: 'Sol Büyük' }, { v: 'Her İkisi Büyük' }]
						}
					}
			]
		},
		{
			className: 'row col-sm-12',
			fieldGroup: [
				{
					className: 'row col-sm-2',
					type: 'textarea',
					key: 'Fundoskopi',
					templateOptions: {
						label: 'Fundoskopi',
						rows: 2
					}
				},
				{
					className: 'col-sm-4',
					key: 'GormeKeskinligi',
					type: 'ui-select-single',
					defaultValue: '10/10 Tam görme',
					templateOptions: {
						label: 'Görme Keskinligi',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: '10/10 Tam görme' }, { v: '5/10 Sürücüler için kabul edilebilir.' }, { v: '1/10 Legal körlük' },
							{ v: 'PS Parmak sayma' }, { v: 'EH El hareketleri' }, { v: 'P+P+ Persepsiyon ve projeksiyon' }, { v: 'P+ Işık algılama' }, { v: 'Absolu	Işık algılama yok' }]
					}
				},
				{
					className: 'col-sm-3',
					key: 'DerinlikDuyusu',
					type: 'ui-select-single',
					defaultValue: 'Var',
					templateOptions: {
						label: 'Derinlik Duyusu',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Var' }, { v: 'Yok' }]
					}
				},
				{
					className: 'col-sm-3',
					key: 'RenkKorlugu',
					type: 'ui-select-single',
					defaultValue: 'Yok',
					templateOptions: {
						label: 'Renk Körlüğü',
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						valueProp: 'v',
						labelProp: 'v',
						options: [{ v: 'Var' }, { v: 'Yok' }]
					}
				}
			]
		},
		{
			type: 'textarea',
			key: 'Sonuc',
			templateOptions: {
			    label: 'Değerlendirme',
			    required: true,
				placeholder: ''
			}
		}
	];
	GrC.originalFields = angular.copy(GrC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Gormeleri = TkSvc.Tk.TkBilgi.data.Gormeleri;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Gormeleri = s.data.Gormeleri;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsGrC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Görme Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Görme Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsGrC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	GrC.onSubmit = function () {
		GrC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(GrC.model.Gorme_Id) || GrC.model.Gorme_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				GrC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsGorme(GrC.model, TkSvc.Tk.SaglikBirimi_Id, GrC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Gormeleri = s.data.Gormeleri;
					});
					$scope.yeniGrC();
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
			TkSvc.UpdatePrsGorme(GrC.model.Gorme_Id, GrC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Gormeleri = s.data.Gormeleri;
				});
				$scope.yeniGrC();
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
				var rs = $filter('filter')($scope.Gormeleri, {
					Gorme_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				rs.GozKapagi = angular.isUndefined(rs.GozKapagi) || rs.GozKapagi === null ? '' : rs.GozKapagi.trim();
				rs.GozKuresi = angular.isUndefined(rs.GozKuresi) || rs.GozKuresi === null ? '' : rs.GozKuresi.trim();
				rs.GozKaymasi = angular.isUndefined(rs.GozKaymasi) || rs.GozKaymasi === null ? '' : rs.GozKaymasi.trim();
				rs.GozdeKizariklik = angular.isUndefined(rs.GozdeKizariklik) || rs.GozdeKizariklik === null ? '' : rs.GozdeKizariklik.trim();
				rs.PupillaninDurumu = angular.isUndefined(rs.PupillaninDurumu) || rs.PupillaninDurumu === null ? '' : rs.PupillaninDurumu.trim();
				rs.IsikRefleksi = angular.isUndefined(rs.IsikRefleksi) || rs.IsikRefleksi === null ? '' : rs.IsikRefleksi.trim();
				rs.GormeAlani = angular.isUndefined(rs.GormeAlani) || rs.GormeAlani === null ? '' : rs.GormeAlani.trim();
				rs.GozTonusu = angular.isUndefined(rs.GozTonusu) || rs.GozTonusu === null ? '' : rs.GozTonusu.trim();
				rs.GormeKeskinligi = angular.isUndefined(rs.GormeKeskinligi) || rs.GormeKeskinligi === null ? '' : rs.GormeKeskinligi.trim();
				rs.DerinlikDuyusu = angular.isUndefined(rs.DerinlikDuyusu) || rs.DerinlikDuyusu === null ? '' : rs.DerinlikDuyusu.trim();
				rs.RenkKorlugu  = angular.isUndefined(rs.RenkKorlugu ) || rs.RenkKorlugu  === null ? '' : rs.RenkKorlugu .trim();
				GrC.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'gorme', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}
	var deselect = function () {
		var table = $("#gorme").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniGrC = function () {
		GrC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silGrC = function () {
		TkSvc.DeletePrsGorme(GrC.model.Gorme_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Gormeleri = s.data.Gormeleri;
			});
			$scope.yeniGrC();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin! " + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/gorme/' + GrC.model.Gorme_Id,
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

	$scope.$watch(function () { return GrC.model.Gorme_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});
}
gormeCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('gormeCtrl', gormeCtrl);