'use strict';

function boyKiloCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var Bkc = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	Bkc.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	Bkc.model = {
	};

	Bkc.options = {};

	Bkc.fields = [
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'Boy',
						templateOptions: {
							type: 'number',
							min: 0,
							label: 'Boy (cm)',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'Kilo',
						templateOptions: {
							type: 'number',
							min: 0,
							label: 'Kilo',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'Bel',
						templateOptions: {
							type: 'number',
							min: 0,
							label: 'Bel (cm)',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'Kalca',
						templateOptions: {
							type: 'number',
							min: 0,
							label: 'Kalca (cm)',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-4',
						type: 'textarea',
						key: 'Sonuc',
						templateOptions: {
							label: 'Sonuç',
							placeholder: ''
						}
					}
	];

	Bkc.originalFields = angular.copy(Bkc.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.BoyKilolari = TkSvc.Tk.TkBilgi.data.BoyKilolari;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.BoyKilolari = s.data.BoyKilolari;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsBkc = DTOptionsBuilder.newOptions()
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

	$scope.dtColumnDefsBkc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	Bkc.onSubmit = function () {
		Bkc.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(Bkc.model.BoyKilo_Id) || Bkc.model.BoyKilo_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				Bkc.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsBoyKilo(Bkc.model, TkSvc.Tk.SaglikBirimi_Id, Bkc.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.BoyKilolari = s.data.BoyKilolari;
					});
					$scope.yeniBkc();
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
			TkSvc.UpdatePrsBoyKilo(Bkc.model.BoyKilo_Id, Bkc.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.BoyKilolari = s.data.BoyKilolari;
				});
				$scope.yeniBkc();
			}).catch(function (e) {
			    $scope.message = "Hata Kontrol Edin!" + e;
			    startTimer();
			});
		}
	};

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.BoyKilolari, {
					BoyKilo_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				Bkc.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'boy-kilo', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#boyKilo").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniBkc = function () {
		Bkc.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silBkc = function () {
		TkSvc.DeletePrsBoyKilo(Bkc.model.BoyKilo_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.BoyKilolari = s.data.BoyKilolari;
			});
			$scope.yeniBkc();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin!" + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/boy-kilo/' + Bkc.model.BoyKilo_Id,
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

	$scope.$watch(function () { return Bkc.model.BoyKilo_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

	$scope.Vurgusu = function (deger, bicim) {
	    return deger === null ? '----' : deger + bicim;
	};

	$scope.BKIyorum = "";
	$scope.BKOyorum = "";

	$scope.$watchCollection('[Bkc.model.Boy,Bkc.model.Kilo]', function (newValues) {
		if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
			var bki = (newValues[1] / (Math.pow(newValues[0], 2) / 10000)).toFixed(2);
			Bkc.model.BKI = bki;
			switch(true)
			{
				case bki >39:
					$scope.BKIyorum = "Aşırı Şişman (Morbid obez).";
					Bkc.model.Sonuc = $scope.BKIyorum;
					break;
				case bki <40 && bki >30:
					$scope.BKIyorum = "Şişman (Obez).";
					Bkc.model.Sonuc = $scope.BKIyorum;
					break;
				case bki <31 && bki >24:
					$scope.BKIyorum = "Hafif Şişman.";
					Bkc.model.Sonuc = $scope.BKIyorum;
					break;
				case bki <25 && bki >19:
					$scope.BKIyorum = "Normal Kilolu.";
					Bkc.model.Sonuc = $scope.BKIyorum;
					break;
				case bki <20:
					$scope.BKIyorum = "Çok Zayıf.";
					Bkc.model.Sonuc = $scope.BKIyorum;
					break;
				default: $scope.BKIyorum = ""; Bkc.model.Sonuc = $scope.BKIyorum;
			}
		}
	});

	$scope.$watchCollection('[Bkc.model.Bel,Bkc.model.Kalca]', function (newValues) {
		if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
			$scope.BKOyorum = "";
			var bko = (newValues[0] / newValues[1]).toFixed(2);
			Bkc.model.BKO = bko === 'NaN' ? '' : bko;
			if (TkSvc.Tk.TkBilgi.data.PersonelDetayi.Cinsiyet === true && bko > 0.95) {
				$scope.BKOyorum = "Bir Erkeğe Göre Uygun Olmayan Bel Kalça Oranı.";
			}
			if (TkSvc.Tk.TkBilgi.data.PersonelDetayi.Cinsiyet === false && bko > 0.80) {
				$scope.BKOyorum = "Bir Bayana Göre Uygun Olmayan Bel Kalça Oranı.";
			}
		}

	});

	Bkc.barOptions = {
		scaleBeginAtZero: true,
		scaleShowGridLines: true,
		scaleGridLineColor: "rgba(0,0,0,.05)",
		scaleGridLineWidth: 1,
		barShowStroke: true,
		barStrokeWidth: 2,
		barValueSpacing: 5,
		barDatasetSpacing: 1
	};
	$scope.heightx = 150;
	$scope.$watch(function () { return $scope.BoyKilolari; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
		    var label = [];
		    var data = [];
			angular.forEach(newValue, function (item) {
				var date=new Date(item.Tarih);
				label.push(date.toLocaleDateString());
				data.push(item.BKI);
			});
			$scope.heightx = newValue.length > 0 ? 200 : 150;
			Bkc.barData = {
			    labels: label.length > 0?label:[''],
				datasets: [
					{
						label: "BKI indexine göre",
						fillColor: "rgba(220,220,220,0.5)",
						strokeColor: "rgba(220,220,220,0.8)",
						highlightFill: "rgba(220,220,220,0.75)",
						highlightStroke: "rgba(220,220,220,1)",
						data: data.length>0?data:[1]
					}
				]
			};
		} 
	});

	var startTimer = function () {
	    var timer = $timeout(function () {
	        $timeout.cancel(timer);
	        $scope.message = "";
	    }, 4000);
	};
}
boyKiloCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('boyKiloCtrl', boyKiloCtrl);