'use strict';

function revirTedaviCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify, $http) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var RtC = this;

	$scope.tanimla = function (value) {
		if (typeof value === 'string') {
			return value;
		} else {
			var hg = [];
			angular.forEach(value, function (v) {
			    hg.push(v.ifade === undefined ? v : v.ifade);
			});
			return hg.toString();
		}
	};

	$scope.tedavi = function (value) {
		var hg = [];
		angular.forEach(value, function (v) {
		    hg.push(v.IlacAdi + ': ' + v.SarfMiktari + ' ');
		});
		return hg.toString();
	};

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	RtC.protokolGirisi = $scope.opsiyonlar[1];

	$scope.files = [];

	$scope.dosyaList = false;

	RtC.model = {
		IlacSarfCikislari: []
	};
	RtC.IlacSarfCikisi = {
	};
	RtC.options = {};

	function refreshSikayet(val, field) {
		var promise;
		if (!val) {
			promise = $q.when({});
		} else {
			promise = $http.get(serviceBase + 'api/tanim/SikayetAra/' + val);
		}
		return promise.then(function (response) {
			field.templateOptions.options = response.data;
		});
	}

	RtC.fields = [
				{
					key: 'Sikayeti',
					type: 'ui-select-multiplesample',
					templateOptions: {
						required: true,
						optionsAttr: 'bs-options',
						ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
						label: 'Şikayetleri',
						valueProp: 'ifade',
						labelProp: 'ifade',
						placeholder: 'Şikayet Arama ↓┘',
						options: [],
						refresh: refreshSikayet,
						refreshDelay: 250
					}
				},
				{
					type: 'typhead',
					key: 'Tani',
					templateOptions: {
						label: 'Hastalık Tanısı',
						placeholder: 'Muhtamel Hastalık Tanımı Yapınız',
						options: [],
						onKeyup: function ($viewValue, $scope) {
							if (typeof $viewValue !== 'undefined') {
								return $scope.templateOptions.options = RtC.HastalikTanimiList($viewValue);
							}
						}
					}
				},
				{
					type: 'textarea',
					key: 'HastaninIlaclari',
					templateOptions: {
						label: 'Hastanın Getirdiği İlaçlar',
						rows: 2,
						placeholder: 'Tüm ilaç,aşı serum vs. yazınız.'
					}
				},
				{
					type: 'textarea',
					key: 'Sonuc',
					templateOptions: {
						label: 'Değerlendirme',
						rows: 2,
						placeholder: 'Tüm tedavi değerlendirmelerinizi yazınız.'
					}
				}
	];

	RtC.originalFields = angular.copy(RtC.fields);
	RtC.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};
	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.RevirTedavileri = TkSvc.Tk.TkBilgi.data.RevirTedavileri;
		if (TkSvc.Tk.SaglikBirimi_Id !== null) {
		    SMSvc.SBStokListeleri(TkSvc.Tk.SaglikBirimi_Id, true).then(function (data) {
		        $scope.IlacStoklari = [];
			    angular.forEach(data, function (v) {
			        if (v.StokTuru !== 'Demirbaş Malz.') { $scope.IlacStoklari.push(v); }
				});
			});
		}
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				SMSvc.SBStokListeleri(TkSvc.Tk.SaglikBirimi_Id, true).then(function (data) {
				    $scope.IlacStoklari = [];
				    angular.forEach(data, function (v) {
				        if (v.StokTuru !== 'Demirbaş Malz.') { $scope.IlacStoklari.push(v); }
				    });
				});
			}
			$scope.RevirTedavileri = s.data.RevirTedavileri;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsRtC = DTOptionsBuilder.newOptions()
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

	$scope.dtColumnDefsRtC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.dtOptionsRtCx = DTOptionsBuilder.newOptions()
		 .withLanguageSource('/views/Personel/Controller/Turkish.json')
		 .withPaginationType('full')
		 .withSelect(true)
		 .withOption('order', [1, 'desc'])
		 .withOption('rowCallback', rowCallbackx)
		 .withOption('responsive', true);

	function rowCallbackx(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				if (angular.isUndefined(RtC.model.RevirTedavi_Id) || RtC.model.RevirTedavi_Id == null) {
					var rs = $filter('filter')(RtC.model.IlacSarfCikislari, {
						StokId: aData[1]
					})[0];
				} else {
					var rs = $filter('filter')(RtC.model.IlacSarfCikislari, {
						IlacSarfCikisi_Id: aData[0]
					})[0];
				}
				rs.IlacAdi = angular.isUndefined(rs.IlacAdi) || rs.IlacAdi == null ? '' : rs.IlacAdi.trim();
				RtC.IlacSarfCikisi = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'revir-tedavi', aData[0]).then(function (response) {
				    $scope.files = response;
				    $scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	$scope.dtColumnDefsRtCx = [
	 DTColumnDefBuilder.newColumnDef(0).notVisible(),
	 DTColumnDefBuilder.newColumnDef(1).notVisible(),
	 DTColumnDefBuilder.newColumnDef(2).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	RtC.onSubmit = function () {
		RtC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		var hg = [];
		angular.forEach(RtC.model.Sikayeti, function (v) {
		    hg.push(v.ifade === undefined ? v : v.ifade);
		});
		RtC.model.Sikayeti = hg.toString();
		if (angular.isUndefined(RtC.model.RevirTedavi_Id) || RtC.model.RevirTedavi_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				RtC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsRevirTedavi(RtC.model, TkSvc.Tk.SaglikBirimi_Id, RtC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.RevirTedavileri = s.data.RevirTedavileri;
					});
					$scope.yeniRtC();
				});
			} else {
				notify({
					message: 'Sağlık Birimini Girmeden Kayıt Yapamazsınız.',
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 2000,
					position: 'center'
				});
			}
		}
		else {
			TkSvc.UpdatePrsRevirTedavi(RtC.model.RevirTedavi_Id, RtC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.RevirTedavileri = s.data.RevirTedavileri;
				});
				$scope.yeniRtC();
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
				var rs = $filter('filter')($scope.RevirTedavileri, {
					RevirTedavi_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				RtC.model = rs;
				if (typeof rs.Sikayeti === 'string') {
					var sikayetler = [];
					angular.forEach(rs.Sikayeti.toString().split(","), function (v) {
					    sikayetler.push({ ifade: v });
					});
					RtC.model.Sikayeti = sikayetler;

				}
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

	$scope.yeniRtC = function () {
		RtC.model = { IlacSarfCikislari: [] };
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silRtC = function () {
		TkSvc.DeletePrsRevirTedavi(RtC.model.RevirTedavi_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.RevirTedavileri = s.data.RevirTedavileri;
			});
			$scope.yeniRtC();
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! " + e;
			startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/revir-tedavi/' + RtC.model.RevirTedavi_Id,
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

	$scope.$watch(function () { return RtC.model.RevirTedavi_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

	$scope.$watch('[RtC.model.Sikayeti,RevirTedavi_Id]', function (newVal, oldVal) {
		if (Object.prototype.toString.call(newVal[0] === '[object Array]')) {
			var newVal0Len = newVal[0] === undefined ? 0 : newVal[0].length;
			var oldVal0Len = oldVal[0] == undefined ? 0 : oldVal[0].length;
			if (newVal0Len < oldVal0Len && newVal[1] === oldVal[1]) {
				if (!newVal[0][0].hasOwnProperty('ifade')) {
					var sikayetler = [];
					angular.forEach(newVal[0].toString().split(","), function (v) {
					    sikayetler.push({ ifade: v });
					});
					RtC.model.Sikayeti = sikayetler;
				}
			}
			if (newVal0Len > 0) { $scope.StokGoster = false; } else { $scope.StokGoster = true; }
		}
	}, true);

	$scope.$watch(function () { return RtC.model; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			if (newValue.hasOwnProperty('Sikayeti')) {
			    $scope.StokGoster = false;
			} else $scope.StokGoster = true;
		}
	});

	$scope.$watch(function () { return RtC.model.IlacSarfCikislari; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
		    SMSvc.SBStokListeleri(TkSvc.Tk.SaglikBirimi_Id, true).then(function (data) {
		        var sd = [];
		        angular.forEach(data, function (v) {
		            if (v.StokTuru !== 'Demirbaş Malz.') { sd.push(v); }
		        });
				$scope.IlacStoklari = sd.sort(function (a, b) {
					return compareStrings(a.IlacAdi, b.IlacAdi);
				});
				angular.forEach(newValue, function (v) {
					var index = $scope.IlacStoklari.map(function (item) {
						return item.StokId;
					}).indexOf(v.StokId);
					if (index !== -1) {
						$scope.IlacStoklari.splice(index, 1);
					}
				});
			});
		}
	});

	function compareStrings(a, b) {
		a = a.toLowerCase();
		b = b.toLowerCase();
		return a < b ? -1 : a > b ? 1 : 0;
	}

	$scope.$watch(function () { return TkSvc.Tk.SaglikBirimi_Id; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
			    SMSvc.SBStokListeleri(TkSvc.Tk.SaglikBirimi_Id, true).then(function (data) {
			        var sd = [];
			        angular.forEach(data, function (v) {
			            if (v.StokTuru !== 'Demirbaş Malz.') { sd.push(v); }
			        });
					$scope.IlacStoklari = sd.sort(function (a, b) {
						return compareStrings(a.IlacAdi, b.IlacAdi);
					});
					angular.forEach(RtC.model.IlacSarfCikislari, function (v) {
						var index = $scope.IlacStoklari.map(function (item) {
							return item.StokId;
						}).indexOf(v.StokId);
						if (index !== -1) {
							$scope.IlacStoklari.splice(index, 1);
						}
					});
				});
			}
		}
	});

	RtC.stokEkle = function () {
		if (!angular.isUndefined(RtC.IlacSarfCikisi.IlacAdi)) {
			if (RtC.IlacSarfCikisi.StokMiktari < RtC.IlacSarfCikisi.SarfMiktari) {
				notify({
					message: 'Stok Miktarınzı Üstünde Girişiniz Var!',
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 2000,
					position: 'right'
				});
			}
			if (RtC.model.hasOwnProperty('IlacSarfCikislari')) {
				var inx = RtC.model.IlacSarfCikislari.map(function (item) {
					return item.StokId;
				}).indexOf(RtC.IlacSarfCikisi.StokId);
			} else {
				RtC.model.IlacSarfCikislari = [];
				var inx = -1;
			}
			if (inx === -1) {
				if (angular.isUndefined(RtC.model.RevirTedavi_Id) || RtC.model.RevirTedavi_Id === null) {
					RtC.model.IlacSarfCikislari.push(RtC.IlacSarfCikisi);
					var index = $scope.IlacStoklari.map(function (item) {
						return item.StokId;
					}).indexOf(RtC.IlacSarfCikisi.StokId);
					if (index !== -1) {
						$scope.IlacStoklari.splice(index, 1);
					}
					RtC.IlacSarfCikisi = {};
				}
				else {
					RtC.IlacSarfCikisi.RevirTedavi_Id = RtC.model.RevirTedavi_Id;
					TkSvc.AddPrsIlacSarfCikisi(RtC.IlacSarfCikisi).then(function (response) {
						RtC.model.IlacSarfCikislari.push(response);
						var index = $scope.IlacStoklari.map(function (item) {
							return item.StokId;
						}).indexOf(RtC.IlacSarfCikisi.StokId);
						if (index !== -1) {
							$scope.IlacStoklari.splice(index, 1);
						}
						RtC.IlacSarfCikisi = {};
					});
				}
			}
			else {
				if (angular.isUndefined(RtC.model.RevirTedavi_Id) || RtC.model.RevirTedavi_Id === null) {
					RtC.IlacSarfCikisi = {};
				} else {
					RtC.IlacSarfCikisi.RevirTedavi_Id = RtC.model.RevirTedavi_Id;
					TkSvc.UpdatePrsIlacSarfCikisi(RtC.IlacSarfCikisi.IlacSarfCikisi_Id, RtC.IlacSarfCikisi).then(function (response) {
						RtC.IlacSarfCikisi = {};
					}).catch(function (e) {
						$scope.message = "Hata Kontrol Edin! " + e;
						startTimer();
					});
				}
			}
		} else {
			notify({
				message: 'İlaç Adını Giriniz...',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 2000,
				position: 'right'
			});
		}

	};

	RtC.stokKaldir = function () {
		if (angular.isUndefined(RtC.model.RevirTedavi_Id) || RtC.model.RevirTedavi_Id === null) {
			var index = RtC.model.IlacSarfCikislari.map(function (item) {
				return item.StokId;
			}).indexOf(RtC.IlacSarfCikisi.StokId);
			if (index !== -1) {
				RtC.model.IlacSarfCikislari.splice(index, 1);
			}
			delete RtC.IlacSarfCikisi.SarfMiktari;
			$scope.IlacStoklari.push(RtC.IlacSarfCikisi);
			$scope.IlacStoklari = $scope.IlacStoklari.sort(function (a, b) {
				return compareStrings(a.IlacAdi, b.IlacAdi);
			});
			RtC.IlacSarfCikisi = {};
		} else {
			TkSvc.DeletePrsIlacSarfCikisi(RtC.IlacSarfCikisi.IlacSarfCikisi_Id).then(function (response) {
				var index = RtC.model.IlacSarfCikislari.map(function (item) {
					return item.StokId;
				}).indexOf(RtC.IlacSarfCikisi.StokId);
				if (index !== -1) {
					RtC.model.IlacSarfCikislari.splice(index, 1);
				}
				delete RtC.IlacSarfCikisi.SarfMiktari;
				$scope.IlacStoklari.push(RtC.IlacSarfCikisi);
				$scope.IlacStoklari = $scope.IlacStoklari.sort(function (a, b) {
					return compareStrings(a.IlacAdi, b.IlacAdi);
				});
				RtC.IlacSarfCikisi = {};
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! " + e;
				startTimer();
			});
		}
	};
}
revirTedaviCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify', '$http'];

angular
	.module('inspinia')
	.controller('revirTedaviCtrl', revirTedaviCtrl);