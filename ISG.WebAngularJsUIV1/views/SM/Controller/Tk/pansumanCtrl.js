'use strict';

function pansumanCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var PnsC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	PnsC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	PnsC.model = {
	};

	PnsC.options = {};

	PnsC.fields = [
					{
						className: 'row col-sm-12',
						fieldGroup: [
							 {
								 className: 'col-sm-2',
								 key: 'IsKazasi',
								 type: 'select',
								 defaultValue: false,
								 templateOptions: {
									 label: 'İş Kazası',
									 options: [{ name: 'Yok', value: false }, { name: 'Var', value: true }]
								 }
							 },
							{
								className: 'col-sm-2',
								key: 'YaraCesidi',
								type: 'select',
								templateOptions: {
									label: 'Travma Çeşidi',
									required: true,
									options: [{ name: 'Sıyrık yarası', value: 'Sıyrık yarası' }, { name: 'Kesik yarası', value: 'Kesik yarası' }, { name: 'Ezik yarası', value: 'Ezik yarası' }, { name: 'Delici yara', value: 'Delici yara' },
									{ name: 'Parçalı yara', value: 'Parçalı yara' }, { name: 'Enfekte yara', value: 'Enfekte yara' }, { name: 'Yanık', value: 'Yanık' }, { name: 'Burkulma', value: 'Burkulma' },{ name: 'Çıkık', value: 'Çıkık' }]
								}
							},
							{
								className: 'col-sm-4',
								type: 'input',
								key: 'YaraYeri',
								templateOptions: {
									required: true,
									placeholder: 'Ör: Elde 3 cm lik parçalı yara',
									label: 'Travma Lokalizasyonu'
								}
							},
							{
								className: 'col-sm-4',
								key: 'PansumanTuru',
								type: 'select',
								templateOptions: {
									label: 'Uygulama',
									required: true,
									options: [{ name: 'Açık Pansuman', value: 'Açık Pansuman' }, { name: 'Kapalı Pansuman', value: 'Kapalı Pansuman' },
										{ name: 'Bandaj', value: 'Bandaj' }, { name: 'Soğuk Kompress Uygulama', value: 'Soğuk Kompress Uygulama' },
										{ name: 'Sıcak Kompress Uygulama', value: 'Sıcak Kompress Uygulama' }, { name: 'Tespit Uygulaması', value: 'Tespit Uygulaması' }]
								}
							}
						]
					},
					{
				className: 'row col-sm-12',
				fieldGroup: [
						   {
							   className: 'col-sm-5',
							   key: 'PansumaninAmaci',
							   type: 'select',
							   defaultValue: false,
							   templateOptions: {
								   label: 'Uygulamanın Amacı',
								   options: [{ name: 'Yarayı dış etkenlerden ve enfeksiyonlardan korumak', value: '1' },
									   { name: 'Yarada bulunan akıntıyı emmek ve uzaklaştırmak', value: '2' },
									   { name: 'Kanamayı durdurmak', value: '3' }, { name: 'Yaraya ilaç uygulamak', value: '4' },
								   { name: 'Yara ve çevresindeki dokuyu desteklemek', value: '5' }, { name: 'Ağrıyı azaltmak ve ısı kaybını önlemek', value: '6' },
								   { name: 'Nemli ortam sağlamak, ödemi önlemek', value: '7' }, { name: 'İyileşme sürecini hızlandırmak', value: '8' }]
							   }
						   },
						   {
								className: 'col-sm-3',
								key: 'SuturBicimi',
								defaultValue:'Yok' ,
								type: 'select',
								templateOptions: {
									label: 'Girişim Yöntemi',
									options: [{ name: 'Yok', value: 'Yok' }, { name: 'Küçük Sütür', value: 'Küçük Sütür' }, { name: 'Orta Sütür', value: 'Orta Sütür' }, { name: 'Büyük Sütür', value: 'Büyük Sütür' },
									{ name: 'Abse Drenajı', value: 'Abse Drenajı' }, { name: 'Tırnak Çekimi', value: 'Tırnak Çekimi' }, { name: 'Yabancı Cisim Çıkarma', value: 'Yabancı Cisim Çıkarma' }]
								}
							},
						   {
							   className: 'col-sm-4',
							   type: 'textarea',
							   key: 'Sonuc',
							   templateOptions: {
								   label: 'Değerlendirme',
								   rows: 2,
								   placeholder: 'Tüm değerlendirmelerinizi yazınız.'
							   }
						   }
				]
			}
	];
	PnsC.originalFields = angular.copy(PnsC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Pansumanlari = TkSvc.Tk.TkBilgi.data.Pansumanlari;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Pansumanlari = s.data.Pansumanlari;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsPnsC = DTOptionsBuilder.newOptions()
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

	$scope.dtColumnDefsPnsC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	PnsC.onSubmit = function () {
		PnsC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(PnsC.model.Pansuman_Id) || PnsC.model.Pansuman_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				PnsC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsPansuman(PnsC.model, TkSvc.Tk.SaglikBirimi_Id, PnsC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Pansumanlari = s.data.Pansumanlari;
					});
					$scope.yeniPnsC();
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
			TkSvc.UpdatePrsPansuman(PnsC.model.Pansuman_Id, PnsC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Pansumanlari = s.data.Pansumanlari;
				});
				$scope.yeniPnsC();
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
				var rs = $filter('filter')($scope.Pansumanlari, {
					Pansuman_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				rs.YaraCesidi = angular.isUndefined(rs.YaraCesidi) || rs. YaraCesidi=== null ? '' : rs.YaraCesidi.trim();
				rs.PansumaninAmaci = angular.isUndefined(rs.PansumaninAmaci) || rs.PansumaninAmaci === null ? '' : rs.PansumaninAmaci.trim();
				rs.PansumanTuru = angular.isUndefined(rs.PansumanTuru) || rs.PansumanTuru === null ? '' : rs.PansumanTuru.trim();
				rs.SuturBicimi = angular.isUndefined(rs.SuturBicimi) || rs.SuturBicimi === null ? '' : rs.SuturBicimi.trim();
				PnsC.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'pansuman', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#pansuman").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniPnsC = function () {
		PnsC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silPnsC = function () {
		TkSvc.DeletePrsPansuman(PnsC.model.Pansuman_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Pansumanlari = s.data.Pansumanlari;
			});
			$scope.yeniPnsC();
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! " + e;
			startTimer();
		});
	};

	$scope.lastSave = function () {
		var last = $scope.Pansumanlari[$scope.Pansumanlari.length - 1];
		PnsC.model.IsKazasi = last.IsKazasi;
		PnsC.model.YaraYeri = last.YaraYeri;
		PnsC.model.Sonuc = last.Sonuc;
		PnsC.model.YaraCesidi = angular.isUndefined(last.YaraCesidi) || last.YaraCesidi === null ? '' : last.YaraCesidi.trim();
		PnsC.model.PansumaninAmaci = angular.isUndefined(last.PansumaninAmaci) || last.PansumaninAmaci === null ? '' : last.PansumaninAmaci.trim();
		PnsC.model.PansumanTuru = angular.isUndefined(last.PansumanTuru) || last.PansumanTuru === null ? '' : last.PansumanTuru.trim();
		PnsC.model.SuturBicimi = angular.isUndefined(last.SuturBicimi) || last.SuturBicimi === null ? '' : last.SuturBicimi.trim();
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/pansuman/' + PnsC.model.Pansuman_Id,
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
	$scope.$watch(function () { return PnsC.model.Pansuman_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});
}
pansumanCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('pansumanCtrl', pansumanCtrl);