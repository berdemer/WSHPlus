'use strict';
function asilarCtrl($scope, SMSvc, DTOptionsBuilder, DTColumnDefBuilder, analizSvc, personellerSvc, mailSvc, notify, PmSvc, $rootScope) {
	var asC = this;
    var ay = new Date().getMonth();
	asC.year = (new Date().getFullYear()).toString();
    asC.aylarT = [{ id: 1, adi: 'Ocak' }, { id: 2, adi: 'Şubat' }, { id: 3, adi: 'Mart' }, { id: 4, adi: 'Nisan' }, { id: 5, adi: 'Mayıs' }, { id: 6, adi: 'Haziran' }, { id: 7, adi: 'Temmuz' }, { id: 8, adi: 'Ağustos' }, { id: 9, adi: 'Eylül' }, { id: 10, adi: 'Ekim' }, { id: 11, adi: 'Kasım' }, { id: 12, adi: 'Aralık' }];
    asC.ay = (asC.aylarT.filter(function (val) { return val.id == ay + 1; }))[0];

	personellerSvc.MoviesIds.BolumId = 0;
	personellerSvc.MoviesIds.SirketId = 0;
	personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
	personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
	$scope.SirketBasligi = {
		id: 0,
		name: 'Şirketi Seçiniz.!'
	};
	personellerSvc.GetSirketler(true).then(function (data) {
		$scope.collectionSirket = data;
		addToAllNodes(data);
		$scope.expandList = allNodes;
	});
	var allNodes = [];
	function addToAllNodes(children) {
		if (!children || Array.isArray(children) && children.length === 0) {
			return;
		}
		for (var i = 0; i < children.length; i++) {
			allNodes.push(children[i]);
			addToAllNodes(children[i].children);
		}
	}
	$scope.status = {
		open: false
	};
	$scope.showSelected = function (sel) {
		$scope.SirketBasligi.name = sel.name;
		$scope.SirketBasligi.id = sel.id;
		personellerSvc.MoviesIds.SirketId = sel.id;//ŞİRKETİN ID BİLGİSİ
		personellerSvc.MoviesIds.SirketAdi = sel.name;
		$scope.status.open = !$scope.status.open;
	};
	$scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
		$scope.SirketBasligi.name = personellerSvc.MoviesIds.SirketAdi;
		$scope.SirketBasligi.id = personellerSvc.MoviesIds.SirketId;
        raporlar($scope.SirketBasligi.id, asC.year, asC.ay.id );
		if (angular.isUndefined(newValue)) {
			$scope.SirketBasligi = {
				id: 0,
				name: 'Şirketi Seçiniz.!'
			};
		}
		asC.sirketAdi = $scope.SirketBasligi.id === 0 ? '' : $scope.SirketBasligi.name;
	});
	$scope.dtOptions = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Aşı Değerlendirme Raporu', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Aşı Değerlendirme Raporu', titleAttr: 'PDF' },
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
	 .withOption('lengthMenu', [20, 30, 50, 100])
	 .withOption('order', [1, 'asc'])
	 .withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
		DTColumnDefBuilder.newColumnDef(0).notVisible(),
		DTColumnDefBuilder.newColumnDef(3).notVisible(),
		DTColumnDefBuilder.newColumnDef(4).notVisible(),
	];


    var raporlar = function (Sirket_Id, yil, ay) {
        analizSvc.AsiTakibiGelenler(Sirket_Id, yil, ay).then(function (resp) {
	        asC.raporlar = resp;
	    });
	};

	$scope.$watch(function () { return asC.year; }, function (newValue, oldValue) {
	    if (newValue !== oldValue) {
            raporlar($scope.SirketBasligi.id, newValue, asC.ay.id);
	    }
	});

	function titleCase(str) {
		var splitStr = str.toLowerCase().split(' ');
		for (var i = 0; i < splitStr.length; i++) {
			splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
		}
		return splitStr.join(' ');
	}

	$scope.$watch(function () { return asC.ay; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
            raporlar($scope.SirketBasligi.id, asC.year, newValue.id);
		}
	});

	asC.sendPmBilgiMaili = function (AdSoyad, Mail, Asi_Tanimi) {
		if (isNullOrWhitespace(Mail)) {
			return notify({
				message: 'Personelin Mail adresi olmadığı için gönderim yapamazsınız.',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 2000,
				position: 'right'
			});
		}
		var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
		mail.To.push({ address: Mail.trim(), displayName: AdSoyad });
		mail.SagId = PmSvc.pmb.SaglikBirimi_Id === undefined ? 1 : PmSvc.pmb.SaglikBirimi_Id;
		mail.Subject = Asi_Tanimi+ ' Aşınız Hakkında.(ISGplus)';
		mailSvc.GetMail('./views/SM/View/temp/AsiMaili.html', {
			Muayene: {
				AdSoyad: titleCase(AdSoyad),
				Asi_Tanimi: Asi_Tanimi
			}
		}, mail)
	};

	$scope.$watch(function () { return $rootScope.isBeingMailed; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue)) {
			if (newValue === false) {
				delete $rootScope.isBeingMailed2;
				delete $rootScope.isBeingMailed;
				return notify({
					message: $rootScope.mailGonerildiBilgisi === true ? 'Mail Gönderilmiştir.' : 'Mail Gönderilemedi sistemde sorun olabilir?.',
					classes: $rootScope.mailGonerildiBilgisi === true ? 'alert-success' : 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 8000,
					position: 'middle'
				});
			}
		}
	});

	function isNullOrWhitespace(input) {
		return !input || !input.trim();
	}


	asC.sendPmBilgiSMS = function (AdSoyad, Tel, Asi_Tanimi) {
		if (isNullOrWhitespace(Tel)) {
			return notify({
				message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 2000,
				position: 'right'
			});
		}
		var sms = {
			KisaMesaj: "Sn." + AdSoyad + " Zamani gelen " + Asi_Tanimi + " asiniz icin revire gelmeniz veya iletisime gecmeniz rica olunur.", Numaralar: [Tel.trim()]
		};
		mailSvc.PostSMS(sms).then(function (resp) {
			return notify({
				message: resp.data.Bilgi,
				classes: 'alert-success',
				templateUrl: $scope.inspiniaTemplate,
				duration: 8000,
				position: 'middle'
			});
		});
	};



}

asilarCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'analizSvc', 'personellerSvc', 'mailSvc', 'notify', 'PmSvc', '$rootScope'];

angular
	.module('inspinia')
	.controller('asilarCtrl ', asilarCtrl);