'use strict';

function egtAlanlarCtrl($scope, uploadService, DTColumnDefBuilder, DTOptionsBuilder, $stateParams, egitimSvc, notify, $uibModal,$rootScope) {

    var EgAC = this; var logo = null;
    function isNullOrWhitespace(input) {
        return !input || !input.trim();
    }
    $scope.inspiniaTemplate = 'views/common/notify.html';
    if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {
        EgAC.egitimAlanlar = [];
        EgAC.Bekle = true;
        uploadService.GetImageStiId("logo", $stateParams.id).then(function (response) {
            logo = response[0].LocalFilePath;
        });
        egitimSvc.EgtmAlList($stateParams.id, $stateParams.year).then(function (r) {
            angular.forEach(r, function (v) {
                v.DurumSertifika = false;
                EgAC.egitimAlanlar.push(v);
            });
        }).catch(function (e) {
            return notify({
                message: e,
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 7000,
                position: 'right'
            });
        }).finally(function () {
            EgAC.Bekle = false;
        });
    };
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            { extend: 'copy', text: 'Kopyala' },
            { extend: 'csv' },
            { extend: 'excel', title: 'PersonelEğitimListesi', name: 'Excel' },
            { extend: 'pdf', title: 'PersonelEğitimListesi', name: 'PDF' },
            {
                extend: 'print', text: 'Yazdır',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ])
        .withPaginationType('full_numbers')
        .withOption('order', [4, 'desc'])//toplam ders saısından
        .withOption('responsive', window.innerWidth < 1500 ? true : false);
    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(1).notVisible(),
        DTColumnDefBuilder.newColumnDef(2).notVisible(),
        DTColumnDefBuilder.newColumnDef(8).notVisible(),
        DTColumnDefBuilder.newColumnDef(9).notVisible(),
        DTColumnDefBuilder.newColumnDef(10).notVisible(),
        DTColumnDefBuilder.newColumnDef(11).notVisible(),
        DTColumnDefBuilder.newColumnDef(12).notVisible(),
        DTColumnDefBuilder.newColumnDef(13).notVisible(),
        DTColumnDefBuilder.newColumnDef(14).notVisible(),
        DTColumnDefBuilder.newColumnDef(15).notVisible(),
        DTColumnDefBuilder.newColumnDef(16).notVisible(),
    ];
    $scope.sms = function (element) {
        var b = JSON.parse(element.attributes['id'].value);
        if (isNullOrWhitespace(b.Telefon)) {
            return notify({
                message: 'Personelin Telefonu olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        } else {
            $uibModal.open({
                templateUrl: 'SMSBilgi.html',
                backdrop: true,
                animation: true,
                size: 'sm',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance, mailSvc, notify) {
                    this.$onInit = function () {
                        $scope.txt = `Sn. ${b.AdiSoyadi},Zorunlu Olduğunuz ISG Eğitim Katılım Süreniz: ${b.tehlikeGrubu} Katıldığınız Ders Sayınız:${b.ISG_Toplam_KatilimSayisi} Bu duruma göre katılımınız ${b.BasarimYuzdesi} oranında tamamlanmıştır. Gösterdiğiniz hassasiyet için teşekkür ederiz.`;
                    };
                    $scope.Gonder = function () {
                        mailSvc.PostSMS({ KisaMesaj: $scope.txt, Numaralar: [b.Telefon.trim()] }).then(function (resp) {
                            return notify({
                                message: resp.data.Bilgi,
                                classes: 'alert-success',
                                templateUrl: $scope.inspiniaTemplate,
                                duration: 8000,
                                position: 'middle'
                            });
                        });
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
    };
    $scope.mail = function (element) {
        var b = JSON.parse(element.attributes['id'].value);
        if (isNullOrWhitespace(b.Mail)) {
            return notify({
                message: 'Personelin Mail adresi olmadığı için gönderim yapamazsınız.',
                classes: 'alert-danger',
                templateUrl: $scope.inspiniaTemplate,
                duration: 2000,
                position: 'right'
            });
        } else {
            $uibModal.open({
                templateUrl: 'MailBilgi.html',
                backdrop: true,
                animation: true,
                size: 'xl',
                windowClass: "animated fadeInDown",
                controller: function ($scope, $uibModalInstance,notify, PmSvc, mailSvc) {                  
                    this.$onInit = function () {//sayfa yüklemeden önce
                        $scope.inspiniaTemplate = 'views/common/notify.html';
                        var bilgi = "";
                        angular.forEach(b.detayliEgitim, function (v) {
                            bilgi = bilgi + `<li><span>&nbsp;${v.Egitim_Turu}&nbsp; &nbsp;(${v.Egitim_Suresi} dk)&nbsp;--${v.IsgEgitimiVerenPersonel}</span></li>`;
                        });   
                     $scope.txt = `<h4><span style="font-weight: bold;">Sn. ${b.AdiSoyadi},</span></h4><br><div class="row" style="line-height: 2;">
<span>6331 sayılı İş Sağlığı ve Güvenliği Kanunu kapsamında yer alan Çalışanların İş Sağlığı Ve Güvenliği Eğitimlerinin Usul Ve Esasları Hakkında Yönetmeliğe göre adınıza yasal zorunluluk olan ISG eğitiminiz hakkında;</span>
</div><div class="row" style="line-height: 2;"><span>Zorunlu Olduğunuz Katılım Süreniz: ${b.tehlikeGrubu}</span></div><div class="row" style="line-height: 2;"><span>Katıldığınız Ders Sayınız:${b.ISG_Toplam_KatilimSayisi}</span>
</div><div class="row" style="line-height: 2;"><span>Katıldığınız Ders Süreniz: ${b.ISG_Toplam_Suresi_Saat} saat</span></div><div class="row" style="line-height: 2;"><span>Katıldığınız Dersler;</span></div><div class="row" style="line-height: 2;">
<ol>${bilgi}</ol><div>Bu duruma göre katılımınız ${b.BasarimYuzdesi} oranında tamamlanmıştır. Gösterdiğiniz hassasiyet için teşekkür ederiz.</div></div><br>
<div class="row"><span>Sağlıklı ve Kazasız Günler Dileklerimizle.</span></div>`;
                    };
                    $scope.Gonder = function () {
                        $scope.Bilgi = "Lütfen Bekleyiniz.Mailiniz Gönderiliyor.";
                        var mail = { To: [], CC: [], Bcc: [], Subject: '', Body: '', Resimler: [], SagId: '' };
                        mail.To.push({ address: b.Mail.trim(), displayName: b.AdiSoyadi });
                        mail.Body = `<!doctype html><html><body>${$scope.txt}</body></html>`;
                        mail.SagId = PmSvc.pmb.SaglikBirimi_Id === undefined ? 1 : PmSvc.pmb.SaglikBirimi_Id;
                        mail.Subject = 'ISG Eğitiminiz Planlaması Hakkında Genel Bilgilendirme.(ISGplus)';
                        mailSvc.GetHTMLMail(mail).then(function (r) {
                            console.log(r);
                        }).catch(function (e) {
                            return notify({
                                message: e,
                                classes: 'alert-danger',
                                templateUrl: $scope.inspiniaTemplate,
                                duration: 7000,
                                position: 'right'
                            });
                        }).finally(function () {
                            $scope.Bilgi = "";
                            $uibModalInstance.dismiss('cancel');
                        });;
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
    $scope.sertifika = function (element) {
        $uibModal.open({
            templateUrl: 'Sertifika.html',
            backdrop: "static",
            animation: true,
            size: 'sm',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, personellerSvc, printer) {
                var id = JSON.parse(element.attributes['id'].value);
                this.$onInit = function () {//sayfa yüklemeden önce
                    $scope.isgUsers = [];
                    personellerSvc.GetFullIsgUsers().then(function (r) {//uzman listesi
                        angular.forEach(r, function (value) {
                            $scope.isgUsers.push({ adi: value.FullName, gorevi: value.Gorevi !== undefined ? value.Gorevi : '', meslegi: value.Meslek !== undefined ? value.Meslek : '', kodu: value.doktorSertifikaKodu !== undefined ? value.doktorSertifikaKodu : '' });
                        });
                    });
                };
                $scope.SertifikaYazdir = function (val) {
                    printer.print('./views/Egitim/View/temp/Sertifika.html', {
                        b: id, isgUzmanlari: val, logo:logo
                    });
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
    };
    $scope.detay = function (element) {
        $uibModal.open({
            templateUrl: 'isgEgitimiAlanlar.html',
            backdrop: true,
            animation: true,
            size: 'xl',
            windowClass: "animated fadeInDown",
            controller: function ($scope, $uibModalInstance, DTOptionsBuilder, DTColumnDefBuilder) {
                $scope.IsgEgitimleri = JSON.parse(element.attributes['id'].value);
                $scope.formatDate = function (date) {
                    return date === null ? null : new Date(date);
                };
                $scope.dtOptionsiec = DTOptionsBuilder.newOptions()
                    .withLanguageSource('/views/Personel/Controller/Turkish.json')
                    .withDOM('<"html5buttons"B>lTfgitp')
                    .withButtons([
                        {
                            text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
                            className: 'addButton', action: function (e, dt, node, config) {
                                $scope.$apply(function () {
                                    $scope.isCollapsed = !$scope.isCollapsed;
                                });
                            }
                        },
                        { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
                        { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
                        { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Isg Egitimleri Listesi', titleAttr: 'Excel 2010' },
                        { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Isg Egitimleri Listesi', titleAttr: 'PDF' },
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
                    .withPaginationType('full_numbers')
                    .withSelect(true)
                    .withOption('order', [1, 'desc'])
                    .withOption('responsive', window.innerWidth < 1500 ? true : false);

                $scope.dtColumnDefsiec = [// 0. kolonu gizlendi.
                    DTColumnDefBuilder.newColumnDef(0).notVisible(),
                    DTColumnDefBuilder.newColumnDef(1).notVisible(),
                ];
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
    };

}
egtAlanlarCtrl.$inject = ['$scope', 'uploadService', 'DTColumnDefBuilder', 'DTOptionsBuilder', '$stateParams', 'egitimSvc', 'notify', '$uibModal', 'printer', '$rootScope'];

angular
    .module('inspinia')
    .controller('egtAlanlarCtrl', egtAlanlarCtrl);