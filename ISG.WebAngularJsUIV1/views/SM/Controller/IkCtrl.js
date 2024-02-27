'use strict';
function IkCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams, TkSvc, SMSvc, $cookies, printer, tanimlarSvc,
	 ngAuthSettings, uploadService, authService, $q, $window, $timeout, notify, $http, PmSvc, $uibModal, $rootScope, $location, $anchorScroll, $document,WizardHandler) {
	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	$scope.servis = serviceBase;
	var IkC = this;
	var dataAl = function (val) {
		PmSvc.GetIkPersonel(val).then(function (s) {
			PmSvc.prmb.prmbilgi = s;
			PmSvc.prmb.guidId = val;
			IkC.guid = val;
			//IkC.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			IkC.fileImgPath = ngAuthSettings.isAzure ? ngAuthSettings.storageLinkService + 'personel/' : ngAuthSettings.storageLinkService + uploadFolder;
			IkC.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			IkC.IkB = s;
			IkC.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
			IkC.Iklari = s.data.IsKazalari;
			SMSvc.AllSbirimleri().then(function (data) {
				var rs = $filter('filter')(data, {
					SirketId: s.sirketId
				});
				$scope.birimler = data;
				var index = rs.map(function (item) {
					return item.SirketId;
				}).indexOf($scope.nbv.SirketId);
				if (index === -1) {
					$scope.nbv = { SirketAdi: "Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null };
				};
			});
		});
	};

	if (!angular.isUndefined($stateParams.id)) {
		dataAl($stateParams.id);
	}

	$scope.showhide = function () {
		IkC.acKapa = !IkC.acKapa;
		if (IkC.checkBrowser === true) $(".fikset").css('top', IkC.acKapa === false ? 430 + ($('#Ik').height() > 1 ? $('#Ik').height() - 70 : 0) : 230);
		var ibox = $('#cv').closest('div.ibox');
		var icon = $('#cv').find('i:first');
		var content = ibox.find('div.ibox-content');
		content.slideToggle(400);
		// Toggle icon from up to down
		icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
		ibox.toggleClass('').toggleClass('border-bottom');
		$timeout(function () {
			ibox.resize();
			ibox.find('[id^=map-]').resize();
		}, 75);
	};

	$rootScope.SB = $cookies.get('SB') === null || $cookies.get('SB') === undefined ? undefined : JSON.parse($cookies.get('SB'));

	$scope.nbv = angular.isUndefined($rootScope.SB) ?
		{ SirketAdi: "Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null }
		: $rootScope.SB;

	$scope.$watch(function () { return $scope.nbv; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue.SaglikBirimi_Id) || newValue.SaglikBirimi_Id !== null) {
			PmSvc.prmb.SaglikBirimi_Id = newValue.SaglikBirimi_Id;
			$rootScope.SB = newValue;
			var zaman = new Date();
			zaman.setTime(zaman.getTime() + 4 * 60 * 60 * 1000);//son kullanımdan 4 saat sonra iptal olacak
			$cookies.put('SB', JSON.stringify(newValue), { expires: zaman.toString() });
		}
	});


	$scope.dtOptionsIkC = DTOptionsBuilder.newOptions()
			 .withLanguageSource('/views/Personel/Controller/Turkish.json')
			 .withDOM('<"html5buttons"B>lTfgitp')
			 .withButtons([
				 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
				 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
				 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Periyodik Muayene Listesi', titleAttr: 'Excel 2010' },
				 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Periyodik Muayene Listesi', titleAttr: 'PDF' },
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
			 .withOption('order', [0, 'desc'])
			 .withOption('lengthMenu', [3, 10, 20, 50, 100])
			 .withOption('rowCallback', rowCallback)
			 .withOption('responsive', true);

	$scope.dtColumnDefsIkC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				var rs = $filter('filter')(IkC.Iklari, {
					IsKazasi_Id: aData[0]
				})[0];
				IkC.ik = rs;
				IkC.Ikbilgisi = IkC.ik.PMJson;
				IkC.listImage = [];
				uploadService.GetFileId($stateParams.id, 'is-kazasi', IkC.ik.IsKazasi_Id).then(function (response) {
					IkC.listImage = response;
				});
			});
			return nRow;
		});
	}

	$scope.oneAtATime = true;

	$scope.formatDate = function (date) {
		return date === null ? null : new Date(date);
	};

	IkC.il = []; IkC.ilceler = [];

	var yeni = {
		erkekIsciSayisi: 0,
		kadinIsciSayisi: 0,
		ozurluIsciSayisi: 0,
		hukumluIsciSayisi: 0,
		kaza: { il: null, ulke: "TÜRKİYE",ilce:null },
		kBilgi: {
			sebebOlayi: null,
			sebebOlayiAlt: null,
			sebebArac: null,
			sebebAracAlt: null,
			yeri: null,
			yeriAlt: null,
			goren: null,
			sahitler: []
		},
		faaliyet: {
			genel: null,
			genelAlt: null,
			ozel: null,
			ozelAlt: null,
			arac: null,
			aracAlt: null
		},
		yaralanma: {
			olay: null,
			olayAlt: null,
			arac: null,
			aracAlt: null,
			turu: null,
			turuAlt: null,
			yeri: null,
			yeriAlt: null
		},
		calisilan: {
			cevre: null,
			cevreAlt: null,
			tibbiSonrasi: {
				il: {
					val: null
				},
				ilce: {
					val: null
				},
				tarih: null,
				adSoyad: null,
				adresi: null,
				saat: null
			}
		},
		bildirimiHazirlayan: {
			bilgisi: null
		}
	};

	IkC.Ikbilgisi = yeni;

	PmSvc.GetIl().then(function (response) {
		angular.forEach(response, function (value) {
		    IkC.il.push({ val: value.il.trim() });
		});
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.kaza.il; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			IkC.ilceler = [];
			PmSvc.GetIlce().then(function (response) {
				angular.forEach(response, function (value) {
				    if (value.il === newValue)
				        IkC.ilceler.push({ val: value.ilce.trim() });
				});
			});
		}
	});

	$scope.open2 = function () {
		$scope.popup2.opened = true;
	};

	$scope.popup2 = {
		opened: false
	};

	$scope.dateOptions2 = {
		formatYear: 'yy',
		maxDate: new Date(2029, 5, 22)
	};

	$scope.open3 = function () {
		$scope.popup3.opened = true;
	};

	$scope.popup3 = {
		opened: false
	};

	$scope.dateOptions3 = {
		formatYear: 'yy',
		maxDate: new Date(2029, 5, 22)
	};

	$scope.open4 = function () {
		$scope.popup4.opened = true;
	};

	$scope.popup4 = {
		opened: false
	};

	$scope.dateOptions4 = {
		formatYear: 'yy',
		maxDate: new Date(2029, 5, 22)
	};

	$scope.altInputFormats = ['d!/M!/yyyy'];

	PmSvc.GetKazayaSebepOlanOlay().then(function (s) {
		IkC.sebebOlaylari = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.kBilgi.sebebOlayi; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.sebebOlaylari, function (v) {
				if (v.kaza === newValue) {
					IkC.sebebAltOlaylari = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetKazayaSebepOlanArac().then(function (s) {
		IkC.sebebAraclari = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.kBilgi.sebebArac; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.sebebAraclari, function (v) {
				if (v.kaza === newValue) {
					IkC.sebebAltAraclari = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetIsKazasininGerceklestigiYer().then(function (s) {
		IkC.yerleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.kBilgi.yeri; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.yerleri, function (v) {
				if (v.kaza === newValue) {
					IkC.yerleriAlt = v.alt;
					return;
				}
			});
		}
	});

	IkC.sahidiGoren = false;

	$scope.$watch(function () { return IkC.Ikbilgisi.kBilgi.goren; }, function (newValue, oldValue) {
		if (newValue === 'Kazayı Gören Var') {
			IkC.sahidiGoren = true;
			if (angular.isUndefined(IkC.ik.IsKazasi_Id) || IkC.ik.IsKazasi_Id === null || IkC.Ikbilgisi.kBilgi.sahitler.length===0)
				IkC.Ikbilgisi.kBilgi.sahitler.push({ tcNo: null, adSoyad: null, tel: null, il: null, ilce: null, adres: null, mail: null });
			if (IkC.Ikbilgisi.kBilgi.sahitler.length > 0) {
			    angular.forEach(IkC.Ikbilgisi.kBilgi.sahitler, function (val) {
			        IkC.ilDegisimi(val);
			    });
			}
		} else {
			IkC.sahidiGoren = false;
			IkC.Ikbilgisi.kBilgi.sahitler = [];
		}
	});

	IkC.ilDegisimi = function (val) {
		var ind = IkC.Ikbilgisi.kBilgi.sahitler.indexOf(val);
		IkC.ilceler[ind] = [];
		tanimlarSvc.GetIlceler(val.il).then(function (response) {
			angular.forEach(response, function (value) {
			    IkC.ilceler[ind].push({ val: value.ifade.trim() });
			});
		});
	};

	IkC.addSahit = function () {
		IkC.Ikbilgisi.kBilgi.sahitler.push({ tcNo: null, adSoyad: null, tel: null, il: null, ilce: null, adres: null, mail: null });
	};

	IkC.deleteSahit = function (item) {
		var index = IkC.Ikbilgisi.kBilgi.sahitler.indexOf(item);
		if (index !== -1) {
			IkC.Ikbilgisi.kBilgi.sahitler.splice(index, 1);
		}
	};

	PmSvc.GetGenelFaaliyetleri().then(function (s) {
		IkC.genelFaaliyetleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.faaliyet.genel; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.genelFaaliyetleri, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.genelFaaliyetleriAlt = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetozelFaaliyetleri().then(function (s) {
		IkC.ozelFaaliyetleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.faaliyet.ozel;}, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.ozelFaaliyetleri, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.ozelFaaliyetleriAlt = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetAracFaaliyetleri().then(function (s) {
		IkC.aracFaaliyetleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.faaliyet.arac; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.aracFaaliyetleri, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.aracFaaliyetleriAlt = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetYaralanmaOlaylari().then(function (s) {
		IkC.yaralanmaOlaylari = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.yaralanma.olay; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
		    angular.forEach(IkC.yaralanmaOlaylari, function (v) {
		        if (v.kaza.trim() === newValue.trim()) {
		            IkC.yaralanmaOlaylariAlt = v.alt;
		            return;
		        }
		    });
		}
	});

	PmSvc.GetYaralanmaAraclari().then(function (s) {
		IkC.yaralanmaAraclari = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.yaralanma.arac; }, function (newValue, oldValue) {
		if (newValue !== oldValue && newValue !== undefined) {
			angular.forEach(IkC.yaralanmaAraclari, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.yaralanmaAraclariAlt = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetYaralanmaTurleri().then(function (s) {
		IkC.yaralanmaTurleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.yaralanma.turu; }, function (newValue, oldValue) {
		if (newValue !== oldValue && typeof newValue === 'string') {
			angular.forEach(IkC.yaralanmaTurleri, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.yaralanmaTurleriAlt = v.alt;
					return;
				}
			});
		}
	});

	PmSvc.GetYaralanmaYerleri().then(function (s) {
		IkC.yaralanmaYerleri = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.yaralanma.yeri; }, function (newValue, oldValue) {
		if (newValue !== oldValue && newValue !== undefined) {
			angular.forEach(IkC.yaralanmaYerleri, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.yaralanmaYerleriAlt = v.alt;
					return;
				}
			});
		}
	});

	IkC.calisilanOrtamlar = ["Çalışılan ortamı belirtilmemiş Sürekli olarak çalıştığı sabit işyeri (örn: Atölye, İşyeri, Büro, Ek Bina vb...) ", "Sabit olmayan geçici işyeri (örn: Açık alan, İnşaat alanı, İş seyahati, Başka işyerinde toplantı) ", "Diğer çalışılan ortam"];

	PmSvc.GetCalisilanCevreler().then(function (s) {
		IkC.calisilanCevreler = s;
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.cevre; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			angular.forEach(IkC.calisilanCevreler, function (v) {
				if (v.kaza.trim() === newValue.trim()) {
					IkC.calisilanCevrelerAlt = v.alt;
					return;
				}
			});
		}
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.tibbiSonrasi.il; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			IkC.ilcelerS = [];
			PmSvc.GetIlce().then(function (response) {
				angular.forEach(response, function (value) {
				    if (value.il === newValue)
				        IkC.ilcelerS.push({ val: value.ilce.trim() });
				});
			});
		}
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.kazadanSonra; }, function (newValue, oldValue) {
		if (newValue !== 'Çalışmayı Bir Süre Sonra Bıraktı') {
			delete IkC.Ikbilgisi.calisilan.calismayiBiraktigiTarihi;
			delete IkC.Ikbilgisi.calisilan.calismayiBiraktigiSaati;
		}
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.kazaninGerceklestigiYer; }, function (newValue, oldValue) {
		if (newValue !== 'İşyerinde') {
			delete IkC.Ikbilgisi.calisilan.kazaninIsyerindeGerceklestigiOrtam;
		} else {
			delete IkC.Ikbilgisi.calisilan.kazaninIsyeriHaricindeGerceklestigiBolum;
		}
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.isgoremezligi; }, function (newValue, oldValue) {
		if (newValue !== 'Var') {
			delete IkC.Ikbilgisi.calisilan.isgoremezligiSonucu;
			delete IkC.Ikbilgisi.calisilan.isgoremezligiSuresi;
		}
	});

	$scope.$watch(function () { return IkC.Ikbilgisi.calisilan.tibbiMudahale; }, function (newValue, oldValue) {
		if (newValue === 'Daha Sonra Yapıldı') {
			delete IkC.Ikbilgisi.calisilan.tibbiMudahaleYapanAdSoyad;
		}
		if (newValue === 'Derhal Yapıldı') {
			delete IkC.Ikbilgisi.calisilan.tibbiSonrasi;
		}
		if (newValue === 'Yapılmadı') {
			delete IkC.Ikbilgisi.calisilan.tibbiSonrasi;
			delete IkC.Ikbilgisi.calisilan.tibbiMudahaleYapanAdSoyad;
		}
	});

	IkC.ik = {};
	IkC.Kayit = function () {
		Submit();
	};
	IkC.Renjk = false;
	$scope.message = "Kaydet";
	var startTimer = function () {
	    var timer = $timeout(function () {
	        $timeout.cancel(timer);
	        $scope.message = "Kaydet";
	        IkC.Renjk = false;
	    }, 2000);
	};

	IkC.Kayit = function () {
		if ($scope.exitValidation8() === true) {
			Submit();
		}
	};

	var Submit = function () {
		if (PmSvc.prmb.SaglikBirimi_Id === null) {
			return notify({
				message: 'Sağlık Birimini Girmeden Kayıt Yapamazsınız.',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 2000,
				position: 'right'
			});
		}
		if (angular.isUndefined(IkC.ik.IsKazasi_Id) || IkC.ik.IsKazasi_Id === null) {
			IkC.ik = { PMJson: JSON.stringify(IkC.Ikbilgisi), MuayeneTuru: 'İş Kazası' };
			PmSvc.AddIk(IkC.ik, PmSvc.prmb.SaglikBirimi_Id, PmSvc.prmb.guidId).then(function (response) {
				dataAl(PmSvc.prmb.guidId);
				IkC.selectedRow = response.IsKazasi_Id;
				IkC.ik = response;
				IkC.Ikbilgisi = IkC.ik.PMJson;
				$scope.message = "Kaydediliyor!";
				IkC.Renjk = true;
				startTimer();
			}).catch(function (e) {
				return notify({
					message: e.data,
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 7000,
					position: 'right'
				});
			}).finally(function () {
				IkC.Bekle = false;

			});
		} else {
			PmSvc.UpdateIk({
				PMJson: JSON.stringify(IkC.Ikbilgisi), IsKazasi_Id: IkC.ik.IsKazasi_Id, MuayeneTuru: IkC.Ikbilgisi.Nedeni,
				RevirIslem_Id: IkC.ik.RevirIslem_Id, Protokol: IkC.ik.Protokol, Personel_Id: IkC.ik.Personel_Id
			}).then(function (response) {
				dataAl(PmSvc.prmb.guidId);
				IkC.selectedRow = response.IsKazasi_Id;
				IkC.ik = response;
				IkC.Ikbilgisi = JSON.parse(IkC.ik.PMJson);
				$scope.message = "Kaydediliyor!";
				IkC.Renjk = true;
				startTimer();
			}).catch(function (e) {
				return notify({
					message: e.data,
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 7000,
					position: 'right'
				});
			}).finally(function () {
				IkC.Bekle = false;
			});
		}
	};

	IkC.NewSubmit = function () {
		IkC.Ikbilgisi = yeni;
		IkC.ik = {};
		WizardHandler.wizard().goTo(0);
		WizardHandler.wizard().reset();
	};

	IkC.ResimGit = function () {
	    WizardHandler.wizard().goTo(8);
	};

	var valSifirla = function () {
	    $timeout(function () {
	        IkC.ValidationInfo = "";
	    }, 3000);
	};
	var sor = function (v) { return !v; };//empty null undefined sorguluyor.

	$scope.exitValidation1 = function () {
	    if (sor(IkC.Ikbilgisi.bagliBulunduguIl)) {
	        IkC.ValidationInfo = "İşyerinin Bağlı Bulunduğu İli ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.isYeriSicilNo)) {
	        IkC.ValidationInfo = "İş Yeri Sicil No Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.isYeriUnvani)) {
	        IkC.ValidationInfo = "İş Yeri Ünvanı Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.isYeriAdresi)) {
	        IkC.ValidationInfo = "İş Yeri Adresi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.vardiyaBaslamaSaati)) {
	        IkC.ValidationInfo = "Kaza Günü İşyeri Vardiya Başlangıç Saati Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.vardiyaBitisSaati)) {
	        IkC.ValidationInfo = "Kaza Günü İşyeri Vardiya Bitiş Saati Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.isYeriDurumu)) {
	        IkC.ValidationInfo = "Kaza Sonrası İş Yerinin Durumu Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation2 = function () {
	    if (sor(IkC.Ikbilgisi.bildirimiHazirlayan.bilgisi)) {
	        IkC.ValidationInfo = "Bildirimi Hazırlayanı Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.bildirimiHazirlayan.adSoyad)) {
	        IkC.ValidationInfo = "Adını Soyadını Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.bildirimiHazirlayan.tcNo)) {
	        IkC.ValidationInfo = "Tc Kimlik Nosunu Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation3 = function () {
	    if (sor(IkC.Ikbilgisi.kaza.ulke)) {
	        IkC.ValidationInfo = "Kaza Ülkesini Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kaza.il)) {
	        IkC.ValidationInfo = "İli Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kaza.ilce)) {
	        IkC.ValidationInfo = "İlçeyi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kaza.tarihi)) {
	        IkC.ValidationInfo = "Tarihi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kaza.saati)) {
	        IkC.ValidationInfo = "Saati Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kaza.adresi)) {
	        IkC.ValidationInfo = "Adresi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation4 = function () {
	    if (sor(IkC.Ikbilgisi.kBilgi.sebebOlayi)) {
	        IkC.ValidationInfo = "Kazaya Sebeb Olan Olayı Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kBilgi.sebebArac)) {
	        IkC.ValidationInfo = "Kazaya Sebeb Olan Olay Araç/Gereçi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kBilgi.yeri)) {
	        IkC.ValidationInfo = "İş Kazasının Gerçekleştiği Yer/Bölümü Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kBilgi.aciklama)) {
	        IkC.ValidationInfo = "Kazanın Oluş Şekli ve Sebebini Açıklayınız";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.kBilgi.kazaKisiSayisi)) {
	        IkC.ValidationInfo = "Kazaya Uğrayan Kişi Sayısını Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation5 = function () {
	    if (sor(IkC.Ikbilgisi.kBilgi.goren)) {
	        IkC.ValidationInfo = "Kazayı Göreni Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (IkC.Ikbilgisi.kBilgi.goren === 'Kazayı Gören Var') {
	        var bak = true;
	        angular.forEach(IkC.Ikbilgisi.kBilgi.sahitler, function (v) {
	            if (sor(v.tcNo)) {
	                IkC.ValidationInfo = "Tc Noyu Ekleyin";
	                valSifirla();
	                bak = false;
	            }
	            if (sor(v.tel)) {
	                IkC.ValidationInfo = "Telefonu Ekleyin";
	                valSifirla();
	                bak = false;
	            } if (sor(v.adSoyad)) {
	                IkC.ValidationInfo = "Adını Soyadını ekleyin";
	                valSifirla();
	                bak = false;
	            }
	        });
	        return bak;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation6 = function () {
	    if (sor(IkC.Ikbilgisi.faaliyet.genel)) {
	        IkC.ValidationInfo = "Kaza Anında Kazazedenin Yürütmekte Olduğu Genel Faaliyetini Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.faaliyet.ozel)) {
	        IkC.ValidationInfo = "Kazadan Az Önceki Zamandan Kazazedenin Yürüttüğü Özel Faaliyetini Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.faaliyet.arac)) {
	        IkC.ValidationInfo = "Özel Faaliyet Sırasında Kullandığı Araç/Gereçi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation7 = function () {
	    if (sor(IkC.Ikbilgisi.yaralanma.olay)) {
	        IkC.ValidationInfo = "Yaralanmaya Neden Olan Olayını Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.yaralanma.arac)) {
	        IkC.ValidationInfo = "Yaralanmaya Neden Araç/Gereçi Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.yaralanma.turu)) {
	        IkC.ValidationInfo = "Yaranın Türünü Ekleyin";
	        valSifirla();
	        return false;
	    }
	    if (sor(IkC.Ikbilgisi.yaralanma.yeri)) {
	        IkC.ValidationInfo = "Yaranın Vucuttaki Yerini Ekleyin";
	        valSifirla();
	        return false;
	    }
	    else {
	        return true;
	    }
	};
	$scope.exitValidation8 = function () {
	    if (sor(IkC.Ikbilgisi.calisilan.ortam)) {
	        IkC.ValidationInfo = "Çalışılan Ortamı Ekleyin";
	        valSifirla();
	        return false;
	    } else
	        if (sor(IkC.Ikbilgisi.calisilan.cevre)) {
	            IkC.ValidationInfo = "Çalışılan Çevreyi Ekleyin";
	            valSifirla();
	            return false;
	        } else
	            if (sor(IkC.Ikbilgisi.calisilan.isbasiSaati)) {
	                IkC.ValidationInfo = "Kaza Gününde İşbaşı Saatini Ekleyin";
	                valSifirla();
	                return false;
	            } else
	                if (sor(IkC.Ikbilgisi.calisilan.kazadanSonra)) {
	                    IkC.ValidationInfo = "Kazadan Sonra Sigortalı Ne Yaptı Ekleyin";
	                    valSifirla();
	                    return false;
	                } else
	                    if (sor(IkC.Ikbilgisi.calisilan.calismayiBiraktigiTarihi) && IkC.Ikbilgisi.calisilan.kazadanSonra === 'Çalışmayı Bir Süre Sonra Bıraktı') {
	                        IkC.ValidationInfo = "Çalışmayı Bıraktığı Tarihi Ekleyin";
	                        valSifirla();
	                        return false;
	                    } else
	                        if (sor(IkC.Ikbilgisi.calisilan.calismayiBiraktigiSaati) && IkC.Ikbilgisi.calisilan.kazadanSonra === 'Çalışmayı Bir Süre Sonra Bıraktı') {
	                            IkC.ValidationInfo = "Çalışmayı Bıraktığı Saati Ekleyin";
	                            valSifirla();
	                            return false;
	                        } else
	                            if (sor(IkC.Ikbilgisi.calisilan.calismayiBiraktigiSaati) && IkC.Ikbilgisi.calisilan.kazadanSonra === 'Çalışmayı Bir Süre Sonra Bıraktı') {
	                                IkC.ValidationInfo = "Çalışmayı Bıraktığı Saati Ekleyin";
	                                valSifirla();
	                                return false;
	                            } else
	                                if (sor(IkC.Ikbilgisi.calisilan.ortam)) {
	                                    IkC.ValidationInfo = "Çalışılan Ortamı Ekleyin";
	                                    valSifirla();
	                                    return false;
	                                } else
	                                    if (sor(IkC.Ikbilgisi.calisilan.kazaninIsyerindeGerceklestigiOrtam) && IkC.Ikbilgisi.calisilan.kazaninGerceklestigiYer === 'İşyerinde') {
	                                        IkC.ValidationInfo = "Gerçekleştiği Ortamı Ekleyin";
	                                        valSifirla();
	                                        return false;
	                                    } else
	                                        if (sor(IkC.Ikbilgisi.calisilan.kazaninIsyeriHaricindeGerceklestigiBolum) && IkC.Ikbilgisi.calisilan.kazaninGerceklestigiYer === 'İşyeri Dışında') {
	                                            IkC.ValidationInfo = "Gerçekleştiği Bölümü Ekleyin";
	                                            valSifirla();
	                                            return false;
	                                        } else
	                                            if (sor(IkC.Ikbilgisi.calisilan.isgoremezligi)) {
	                                                IkC.ValidationInfo = "Kaza Sonucu İş Göremezliği Ekleyin";
	                                                valSifirla();
	                                                return false;
	                                            } else
	                                                if (sor(IkC.Ikbilgisi.calisilan.isgoremezligiSonucu) && IkC.Ikbilgisi.calisilan.isgoremezligi === 'Var') {
	                                                    IkC.ValidationInfo = "Kaza Sonucu İş Göremezliği Sonucunu Ekleyin";
	                                                    valSifirla();
	                                                    return false;
	                                                } else
	                                                    if (sor(IkC.Ikbilgisi.calisilan.isgoremezligiSuresi) && IkC.Ikbilgisi.calisilan.isgoremezligi === 'Var') {
	                                                        IkC.ValidationInfo = "Kazadan Dolayı Sigortalının İş Günü Kaybını Ekleyin";
	                                                        valSifirla();
	                                                        return false;
	                                                    } else
	                                                        if (sor(IkC.Ikbilgisi.calisilan.tibbiMudahale)) {
	                                                            IkC.ValidationInfo = "Tıbbi Müdahale Yapıldımı Ekleyin";
	                                                            valSifirla();
	                                                            return false;
	                                                        } else
	                                                            if (IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Derhal Yapıldı' && sor(IkC.Ikbilgisi.calisilan.tibbiMudahaleYapanAdSoyad)) {
	                                                                IkC.ValidationInfo = "Tıbbi Müdahale Yapan Kişinin Adı Soyadını Ekleyin";
	                                                                valSifirla();
	                                                                return false;
	                                                            } else
	                                                                if (sor(IkC.Ikbilgisi.calisilan.tibbiSonrasi.il) && IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Daha Sonra Yapıldı') {
	                                                                    IkC.ValidationInfo = "Tıbbi Müdahalenin Yapıldığı İli Ekleyin";
	                                                                    valSifirla();
	                                                                    return false;
	                                                                } else
	                                                                    if (sor(IkC.Ikbilgisi.calisilan.tibbiSonrasi.tarih) && IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Daha Sonra Yapıldı') {
	                                                                        IkC.ValidationInfo = "Tıbbi Müdahale Tarihini Ekleyin";
	                                                                        valSifirla();
	                                                                        return false;
	                                                                    } else
	                                                                        if (sor(IkC.Ikbilgisi.calisilan.tibbiSonrasi.adSoyad) && IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Daha Sonra Yapıldı') {
	                                                                            IkC.ValidationInfo = "Tıbbi Müdahale Yapan Kişinin Adı Soyadını Ekleyin";
	                                                                            valSifirla();
	                                                                            return false;
	                                                                        } else
	                                                                            if (sor(IkC.Ikbilgisi.calisilan.tibbiSonrasi.ilce) && IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Daha Sonra Yapıldı') {
	                                                                                IkC.ValidationInfo = "Tıbbi Müdahalenin Yapıldığı İlçeyi Ekleyin";
	                                                                                valSifirla();
	                                                                                return false;
	                                                                            } else
	                                                                                if (sor(IkC.Ikbilgisi.calisilan.tibbiSonrasi.saat) && IkC.Ikbilgisi.calisilan.tibbiMudahale === 'Daha Sonra Yapıldı') {
	                                                                                    IkC.ValidationInfo = "Tıbbi Müdahale Saatini Ekleyin";
	                                                                                    valSifirla();
	                                                                                    return false;
	                                                                                }
	                                                                                else {
	                                                                                    return true;
	                                                                                }
	};

	IkC.pIsKazasi = function () {
		var afg = IkC.Ikbilgisi;
		afg.as = IkC.ik;
		afg.ac = IkC.IkB;
		printer.print('./views/SM/View/temp/isKazasi.html', afg);
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
			    url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/is-kazasi/' + IkC.ik.IsKazasi_Id,
				method: 'POST',
				data: {
					file: dataUrl
				}
			}).then(function (response) {
			    angular.forEach(response.data, function (data) {
			        if (IkC.listImage === undefined) { IkC.listImage = []; }
			        IkC.listImage.push(data);
			    });
			});
		}
		else {
			$scope.dosyaList = false;
		}
	};

	$scope.deleten = function (file, index) {
		uploadService.DeleteFile(file + '/x').then(function (response) {
			if (response.data === 1) { IkC.listImage.splice(index, 1); }
		});
	};
	$scope.myInterval = 5000;
	$scope.noWrapSlides = false;
	$scope.active = 0;
	$scope.adim = null;
	$scope.$watch('width', function (old, newv) {
	    IkC.gecerliAdim = old;
	});

}

IkCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams', 'TkSvc', 'SMSvc', '$cookies', 'printer', 'tanimlarSvc',
	'ngAuthSettings', 'uploadService', 'authService', '$q', '$window', '$timeout', 'notify', '$http', 'PmSvc',
	'$uibModal', '$rootScope', '$location', '$anchorScroll', '$document', 'WizardHandler'];

angular
	.module('inspinia')
	.controller('IkCtrl', IkCtrl);