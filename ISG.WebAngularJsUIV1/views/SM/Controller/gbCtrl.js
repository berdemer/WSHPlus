'use strict';
function gbCtrl($scope, $state, $stateParams, TkSvc, ngAuthSettings, SMSvc, $rootScope, $cookies, $uibModal, $location) {
	var gbC = this;
    function isNullOrWhitespace(input) {
        return !input || !input.trim();
    }
	$scope.lokal = $location.hash();

	TkSvc.Tk.MuayeneTurleri = [{ muayene: "Revir İşlemleri" }, { muayene: "Normal Muayene İşlemleri" }, { muayene: "Acil Müdahale İşlemleri" },
				{ muayene: "Danışmanlık Hizmeti İşlemleri" }, { muayene: "Periyodik Muayene İşlemleri" }, { muayene: "Meslek Has. İşlemleri" },
				{ muayene: "Dönüş Muayenesi İşlemleri" }, { muayene: "Dışarıdan İstirahat İşlemleri" }, { muayene: "Gebelik İşlemleri" },
				 { muayene: "İş Kazası İşlemleri" }, { muayene: "İşe Başlama İşlemleri" }, { muayene: "Kontrol Muayenesi İşlemleri" },
				 { muayene: "Odio Kontrol İşlemleri" }, { muayene: "Sağlık Kurulu Raporu İşlemleri" }];

	$scope.islem = { muayene: "Revir İşlemleri" };

	$scope.islemler = TkSvc.Tk.MuayeneTurleri;

	$scope.$watch(function () { return $scope.islem; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== oldValue) {
			TkSvc.Tk.MuayeneTuru= newValue;
		}
	});

	if (!angular.isUndefined($stateParams.id)) {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			//$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.fileImgPath = ngAuthSettings.isAzure?ngAuthSettings.storageLinkService + 'personel/':ngAuthSettings.storageLinkService +uploadFolder;
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Tetkik = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
            $scope.guid = $stateParams.id;
            gbC.warning = s.data.Ikazlar.length > 0 ? true : false;
            gbC.warningLength = s.data.Ikazlar.length;
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
            });;

		});
	}

	$scope.formatDate = function (date) {
		return date===null?null: new Date(date);	 
	};


	$rootScope.SB = $cookies.get('SB') === null || $cookies.get('SB') === undefined ? undefined : JSON.parse($cookies.get('SB'));

	$scope.nbv = angular.isUndefined($rootScope.SB) ?
		{ SirketAdi: "Kayıt Yapabilmeniz İçin Sağlık Biriminizi Seçiniz!", SaglikBirimiAdi: "", SaglikBirimi_Id: null }
		: $rootScope.SB;

	$scope.$watch(function () { return $scope.nbv; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue.SaglikBirimi_Id) || newValue.SaglikBirimi_Id != null) {
			TkSvc.Tk.SaglikBirimi_Id = newValue.SaglikBirimi_Id;
			$rootScope.SB = newValue;
			var zaman = new Date();
			zaman.setTime(zaman.getTime() + 4 * 60 * 60 * 1000);//son kullanımdan 4 saat sonra iptal olacak
			$cookies.put('SB', JSON.stringify(newValue), { expires: zaman.toString() });
		}
    });
    $scope.Ikaz = function (tumu) {
        $uibModal.open({
            templateUrl: './views/SM/View/temp/Ikaz.html',
            backdrop: true,
            animation: true,
            size: 'lg',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, $timeout, IkazSvc, TkSvc, notify,
                DTOptionsBuilder, mailSvc, DTColumnDefBuilder, $filter) {
                var startTimer2 = function (sure) {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $uibModalInstance.dismiss('cancel');
                        $scope.bilgilendirme = "";
                    }, sure);
                };
                $scope.bilgilendirme = "";
                var sn = [];
                IkazSvc.GetTumIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                    sn = s;
                    $scope.Ikazlar = tumu === false ?TkSvc.Tk.TkBilgi.data.Ikazlar : sn;
                });
                $scope.simdi = new Date();
                $scope.open13 = function () {
                    $scope.popup13.opened = true;
                };
                $scope.popup13 = {
                    opened: false
                };
                $scope.dateOptionsmdl = {
                    formatYear: 'yy',
                    maxDate: new Date(2029, 5, 22)
                };
                $scope.dtOptionsmdl = DTOptionsBuilder.newOptions()
                    .withLanguageSource('/views/Personel/Controller/Turkish.json')
                    .withDOM('<"html5buttons"B>lTfgitp')
                    .withButtons([
                        { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
                        { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
                        { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Muayene Listesi', titleAttr: 'Excel 2010' },
                        { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Muayene Listesi', titleAttr: 'PDF' },
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
                    .withOption('rowCallback', rowCallback2)
                    .withOption('responsive', true);
                $scope.dtColumnDefsmdl = [// 0. kolonu gizlendi.
                    DTColumnDefBuilder.newColumnDef(0).notVisible()
                ];
                function rowCallback2(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    $('td', nRow).unbind('click');
                    $('td', nRow).bind('click', function () {
                        $scope.$apply(function () {
                            var rs = $filter('filter')($scope.Ikazlar, {
                                Ikaz_Id: aData[0]
                            })[0];
                            $scope.uyari =
                            {
                                Ikaz_Id: rs.Ikaz_Id,
                                IkazText: rs.IkazText,
                                SonucIkazText: rs.SonucIkazText,
                                MuayeneTuru: rs.MuayeneTuru,
                                SonTarih: rs.SonTarih,
                                Tarih: rs.Tarih,
                                Personel_Id: rs.Personel_Id,
                                Status: rs.Status,
                                UserId: rs.UserId
                            };
                            return nRow;
                        });

                    });
                }
                $scope.uyari =
                {
                    Ikaz_Id: null,
                    IkazText: null,
                    SonucIkazText: null,
                    MuayeneTuru: "Genel Bilgi",
                    SonTarih: null,
                    Tarih: new Date(),
                    Personel_Id:TkSvc.Tk.TkBilgi.data.Personel_Id,
                    Status: true,
                    UserId: null
                };
                $scope.remove = function () {
                    var sx = [];
                    if ($scope.uyari.Ikaz_Id !== null) {
                        IkazSvc.IkazSil($scope.uyari.Ikaz_Id).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Uyarınız Sistemden Kaldırıldı.Formunuz kapatılıyor.";
                        });
                    }
                };
                $scope.yeni = function () {
                    $scope.uyari =
                    {
                        Ikaz_Id: null,
                        IkazText: null,
                        SonucIkazText: null,
                        MuayeneTuru: "Personel Muayenesi",
                        SonTarih: null,
                        Tarih: new Date(),
                        Personel_Id:TkSvc.Tk.TkBilgi.data.Personel_Id,
                        Status: true,
                        UserId: null
                    };
                };
                var sd = function (sx) {
                    $scope.Ikazlar = sx;
                    TkSvc.Tk.TkBilgi.data.Ikazlar = sx;
                    gbC.warning = sx.length > 0 ? true : false;
                    gbC.warningLength = sx.length;
                    startTimer2(3000);
                };
                $scope.save = function () {
                    var sx = [];
                    if (angular.isUndefined($scope.uyari.Ikaz_Id) || $scope.uyari.Ikaz_Id === null) {
                        IkazSvc.IkazEkle($scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            var datestring = $scope.uyari.SonTarih.getDate() + "/" + ($scope.uyari.SonTarih.getMonth() + 1) + "/" + $scope.uyari.SonTarih.getFullYear();
                            $scope.bilgilendirme = datestring + " Tarihli uyarınız sisteme eklendi.Formunuz kapatılıyor.";

                        });
                    } else {
                        IkazSvc.IkazGuncelle($scope.uyari.Ikaz_Id, $scope.uyari).then(function (response) {
                            if (tumu === false) {
                                IkazSvc.GetAktifIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            } else {
                                IkazSvc.GetTumIkazlar(TkSvc.Tk.TkBilgi.data.Personel_Id).then(function (s) {
                                    sd(s);
                                });
                            }
                            $scope.bilgilendirme = "Güncellemeniz Yapıldı ve Eklendi.Formunuz kapatılıyor.";

                        });
                    }
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.perSMSI = function () {
                    if (isNullOrWhitespace(TkSvc.Tk.TkBilgi.data.Telefon)) {
                        return notify({
                            message: 'Personelin Cep Telefonu olmadığı için gönderim yapamazsınız.',
                            classes: 'alert-danger',
                            templateUrl: $scope.inspiniaTemplate,
                            duration: 2000,
                            position: 'right'
                        });
                    }
                    if ($scope.uyari.IkazText.trim().length > 155) {
                        return notify({
                            message: '155 karakterin üstünde SMS gönderemezsiniz',
                            classes: 'alert-danger',
                            templateUrl: $scope.inspiniaTemplate,
                            duration: 2000,
                            position: 'right'
                        });
                    }
                    var sms = { KisaMesaj: $scope.uyari.IkazText.trim(), Numaralar: [TkSvc.Tk.TkBilgi.data.Telefon.trim()] };
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
        });
    };
}

gbCtrl.$inject = ['$scope', '$state', '$stateParams', 'TkSvc', 'ngAuthSettings', 'SMSvc', '$rootScope', '$cookies', '$uibModal', '$location'];

angular
	.module('inspinia')
	.controller('gbCtrl', gbCtrl);