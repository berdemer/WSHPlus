'use strict';

function personelViewCrtl($scope, personellerSvc, $uibModal, $http, $q, $timeout,
    tanimlarSvc, $state, $stateParams, uploadService, ngAuthSettings, DTOptionsBuilder, Upload, $window, $location, PmSvc) {
    $scope.foto = true;
    $scope.files = [];
    $scope.dosyaList = false;
    var vm = this;
    vm.bilgi = PmSvc.prmb.PeriyodikMuayeneAktarimi === true && $stateParams.id === PmSvc.prmb.guidId ? true : false;
    vm.onSubmit = onSubmit;
    vm.model = { Sirket_Id: null, Bolum_Id: null };
    function empCont(name) {
        return name ? name.trim() : null;
    }

    $scope.windowWidth = $window.innerWidth;
    $scope.genislik = $scope.windowWidth > 1000 ? { 'width': '1200px' } : null;

    //$scope.$on('$viewContentLoading', function (event, viewConfig) {//sayfaya giriş ve çıkışlar kontrol edilmesiiçin yazıldı.
    //	console.log('content loading: ', event, viewConfig)
    //});

    $scope.formatDate = function (date) {
        var dateOut = new Date(date);
        return dateOut;
    };

    personellerSvc.MoviesIds.fileImgPath = '';
    personellerSvc.MoviesIds.guidId = '';

    if (!angular.isUndefined($stateParams.id)) {
        personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
            personellerSvc.MoviesIds.personel = s;//datayı genele yaymanın en kısa yolu
            $scope.files = [];
            uploadService.GetImageId($stateParams.id, 'personel').then(function (data) {
                personellerSvc.MoviesIds.guidId = $stateParams.id;
                $scope.guidId = $stateParams.id;
                personellerSvc.MoviesIds.fileImgPath = ngAuthSettings.apiServiceBaseUri + data[0].LocalFilePath + data[0].GenericName;
            });

            uploadService.GetImageId($stateParams.id, 'Kisisel').then(function (data) {
                $scope.files = data;
                $scope.dosyaList = true;
            });

            vm.model = {
                Sirket_Id: s.data.Sirket_Id, Bolum_Id: s.data.Bolum_Id, Personel_Id: s.data.Personel_Id, PerGuid: s.data.PerGuid, Adi: s.data.Adi.trim(), Soyadi: s.data.Soyadi.trim(),
                TcNo: s.data.TcNo.trim(), SgkNo: empCont(s.data.SgkNo), KanGrubu: empCont(s.data.KanGrubu), EgitimSeviyesi: empCont(s.data.PersonelDetayi.EgitimSeviyesi),
                Gorevi: empCont(s.data.Gorevi), KadroDurumu: empCont(s.data.KadroDurumu), Mail: empCont(s.data.Mail), Telefon: empCont(s.data.Telefon),
                SicilNo: empCont(s.data.SicilNo), DogumYeri: empCont(s.data.PersonelDetayi.DogumYeri), Cinsiyet: s.data.PersonelDetayi.Cinsiyet, DogumTarihi: (new Date(s.data.PersonelDetayi.DogumTarihi)).toLocaleDateString('tr-TR'),
                Uyruk: empCont(s.data.PersonelDetayi.Uyruk), AskerlikDurumu: empCont(s.data.PersonelDetayi.AskerlikDurumu),
                MedeniHali: empCont(s.data.PersonelDetayi.MedeniHali), IlkIseBaslamaTarihi: new Date(s.data.PersonelDetayi.IlkIseBaslamaTarihi),
                CocukSayisi: s.data.PersonelDetayi.CocukSayisi, KardesSayisi: s.data.PersonelDetayi.KardesSayisi, Kardes_Sag_Bilgisi: empCont(s.data.PersonelDetayi.Kardes_Sag_Bilgisi), anne_adi: s.data.PersonelDetayi.anne_adi,
                anne_sag: s.data.PersonelDetayi.anne_sag, anne_sag_bilgisi: empCont(s.data.PersonelDetayi.anne_sag_bilgisi), baba_adi: empCont(s.data.PersonelDetayi.baba_adi), baba_sag: s.data.PersonelDetayi.baba_sag,
                baba_sag_bilgisi: s.data.PersonelDetayi.baba_sag_bilgisi
            };
            personellerSvc.MoviesIds.BolumId = s.data.Bolum_Id;
            personellerSvc.MoviesIds.SirketId = s.data.Sirket_Id;
            personellerSvc.MoviesIds.SirketAdi = s.sirketAdi;
            personellerSvc.MoviesIds.BolumAdi = s.bolumAdi;
            $scope.PerGuid = $stateParams.id;
            $scope.PersonelAdSoyadi = s.data.Adi.trim() + ' ' + s.data.Soyadi.trim();
            $scope.foto = false;
        });
    } else {
        personellerSvc.MoviesIds.BolumId = 0;
        personellerSvc.MoviesIds.SirketId = 0;
        personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
        personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
    }

    vm.options = {};

    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            vm.model.Sirket_Id = newValue;
            vm.model.Bolum_Id = 0;
        }
        if (newValue > 0) {
            $scope.savedSuccessfully = false;
            $scope.message = "";
        }
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
        if (newValue !== oldValue) {
            vm.model.Bolum_Id = newValue;
        }
    });

    $scope.savedSuccessfully = false;
    $scope.message = "";
    var EgitimSeviyesi = [];
    var kanList = [];
    var unvanlar = [];
    var gorevler = [];
    var DogumYeri = []; var md = [];
    var uyrukList = [{ name: 'Türkiye', value: 'Türkiye' }, { name: 'Yabancı', value: 'Yabancı' }];



    tanimlarSvc.GetTanimAdiList("kanList").then(function (response) {
        angular.forEach(response, function (value) {
            kanList.push({ name: value.ifade.trim(), value: value.ifade.trim() });
        });
        personellerSvc.SD.kanGrupları = angular.copy(kanList);
        tanimlarSvc.GetTanimAdiList("Muayene Neden Tablosu").then(function (response) {
            angular.forEach(response, function (value) {
                md.push({ name: value.ifade.trim(), value: value.ifade.trim() });
            });
            personellerSvc.SD.muayeneNedeni = angular.copy(md);
        });

        tanimlarSvc.GetTanimAdiList("Eğitim Durum Tablosu").then(function (response) {
            angular.forEach(response, function (value) {
                EgitimSeviyesi.push({ name: value.ifade.trim(), value: value.ifade.trim() });
            });
            personellerSvc.SD.egitimSeviyesi = angular.copy(EgitimSeviyesi);

            tanimlarSvc.GetTanimAdiList("unvanlar").then(function (response) {
                angular.forEach(response, function (value) {
                    unvanlar.push({ val: value.ifade.trim() });
                });
                personellerSvc.SD.unvanlar = angular.copy(unvanlar);
                tanimlarSvc.GetTanimAdiList("gorevler").then(function (response) {
                    angular.forEach(response, function (value) {
                        gorevler.push({ val: value.ifade.trim() });
                    });
                    personellerSvc.SD.gorevler = angular.copy(gorevler);
                    tanimlarSvc.GetTanimAdiList("Şehir").then(function (response) {
                        angular.forEach(response, function (value) {
                            DogumYeri.push({ val: value.ifade.trim() });
                        });
                        vm.fields = [
                            {
                                className: 'row col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-6',
                                        type: 'input',
                                        key: 'Adi',
                                        templateOptions: {
                                            label: 'Adı',
                                            required: true
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.Adi = formScope.model.Adi.toLocaleUpperCase('tr-TR');
                                            }
                                        }
                                    },
                                    {
                                        className: 'col-sm-6',
                                        type: 'input',
                                        key: 'Soyadi',
                                        templateOptions: {
                                            label: 'Soyadı',
                                            required: true
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.Soyadi = formScope.model.Soyadi.toLocaleUpperCase('tr-TR');
                                            }
                                        },
                                        expressionProperties: {
                                            'templateOptions.disabled': '!model.Adi'
                                        }
                                    }
                                ]
                            },//adı soyadı
                            {
                                className: 'row col-sm-12',
                                fieldGroup: [
                                    //{
                                    //    template: '<hr style="border:0;border-bottom:1px dashed #EEE;background:#E8DFDF" />',
                                    //},
                                    {
                                        className: 'col-sm-3',
                                        type: 'input',
                                        key: 'TcNo',
                                        templateOptions: {
                                            label: 'TC No',
                                            required: true,
                                            onKeydown: function (value, options) {
                                                options.validation.show = false;
                                            },
                                            onBlur: function (value, options) {
                                                options.validation.show = null;
                                            }
                                        },
                                        expressionProperties: {
                                            'templateOptions.disabled': '!model.Soyadi'
                                        },
                                        asyncValidators: {
                                            uniqueUsername: {
                                                expression: function ($viewValue, $modelValue, scope) {
                                                    scope.options.templateOptions.loading = true;
                                                    return $timeout(function () {
                                                        scope.options.templateOptions.loading = false;
                                                        personellerSvc.TcNoKontrol($viewValue).then(function (response) {
                                                            if (response && angular.isUndefined($stateParams.id)) {
                                                                    $uibModal.open({
                                                                        templateUrl: 'mukerrer.html',
                                                                        backdrop: "static",
                                                                        animation: true,
                                                                        size: 'sm',
                                                                        windowClass: "animated fadeInDown",
                                                                        controller: function ($scope, $uibModalInstance, personellerSvc) {
                                                                            this.$onInit = function () {//sayfa yüklemeden önce);
                                                                                $scope.Bilgi = response.durum ? 'sistemde aktif halde bulunmaktadır.' : 'sistemde pasif halde bulunmaktadır.'
                                                                                $scope.PersonelAdSoyadi = response.adi + ' '+response.soyadi;
                                                                            };
                                                                            $scope.Gonder = function () {
                                                                                if (response.durum === false) {
                                                                                   personellerSvc.ActivePrs(response.gui).then(function (deger) {
                                                                                  });
                                                                                };                                                                               
                                                                                $state.go('Personel.Pers.GenericUpdate', {
                                                                                    id: response.gui
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
                                                                $scope.message = "Bu kullanıcının TC si sistemde var.";
                                                            } 
                                                        }).catch(function (e) {
                                                            $scope.message = e.data;                                                     
                                                        });;
                                                    }, 1000);
                                                },
                                                message: '"Bu kullanıcının TC si sistemde var."'
                                            }
                                        },
                                        modelOptions: {
                                            updateOn: 'blur'
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        type: 'maskedInput',
                                        key: 'Telefon',
                                        templateOptions: {
                                            label: 'Telefon',
                                            placeholder: 'Telefonu giriniz.. (505) 555-55-55 gibi',
                                            mask: "(999) 999-99-99"
                                        }
                                    }, {
                                        className: 'col-sm-3',
                                        type: 'input',
                                        key: 'Mail',
                                        templateOptions: {
                                            maxlength: "55",
                                            type: 'email',
                                            label: 'Mail'
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'KanGrubu',
                                        type: 'ui-select-single',
                                        templateOptions: {
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            label: 'Kan Grubu',
                                            //required: true,
                                            valueProp: 'value',
                                            labelProp: 'name',
                                            placeholder: 'Grubu Seçiniz',
                                            options: personellerSvc.SD.kanGrupları
                                        }
                                    }
                                ]
                            },//tc ve sgk
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    //{
                                    //    template: '<hr style="border:0;border-bottom:1px dashed #EEE;background:#E8DFDF" />',
                                    //},
                                    {
                                        className: 'col-sm-4',
                                        key: 'Gorevi',
                                        type: 'ui-select-single',
                                        defaultValue: 'Personel',
                                        templateOptions: {
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            label: 'Görevi',
                                            //required: true,
                                            valueProp: 'val',
                                            labelProp: 'val',
                                            placeholder: 'Görevini Seçiniz',
                                            options: gorevler
                                        }

                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'KadroDurumu',
                                        type: 'ui-select-single',
                                        defaultValue: 'İşçi',
                                        templateOptions: {
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            label: 'Kadro/Ünvan Durumu',
                                            //required: true,
                                            valueProp: 'val',
                                            labelProp: 'val',
                                            placeholder: 'Kadrosunu Seçiniz',
                                            options: unvanlar
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        type: 'input',
                                        key: 'SgkNo',
                                        templateOptions: {
                                            label: 'Sgk No'
                                            //mask: "* 9999 99 99 9999999 999 99-99"
                                        }
                                    },
                                    {
                                        className: 'col-sm-2',
                                        key: 'SicilNo',
                                        type: 'input',
                                        templateOptions: {
                                            label: 'Sicil No'
                                        }
                                    },

                                ]
                            },
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-3',
                                        key: 'EgitimSeviyesi',
                                        type: 'ui-select-single',
                                        defaultValue: 'LISE VE DENGI O',
                                        templateOptions: {
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            label: 'EgitimSeviyesi',
                                            //required: true,
                                            valueProp: 'value',
                                            labelProp: 'name',
                                            placeholder: 'Seviye Seçiniz',
                                            options: personellerSvc.SD.egitimSeviyesi
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'DogumYeri',
                                        type: 'ui-select-single',
                                        templateOptions: {
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            label: 'DogumYeri',
                                            //required: true,
                                            valueProp: 'val',
                                            labelProp: 'val',
                                            options: DogumYeri
                                        }
                                    },
                                    //{
                                    //    className: 'col-sm-3',
                                    //    key: 'DogumTarihi',
                                    //    type: 'datepicker',
                                    //    templateOptions: {
                                    //        label: 'Dogum Tarihi',
                                    //        type: 'text',
                                    //        datepickerPopup: 'dd.MMMM.yyyy',
                                    //        datepickerOptions: {
                                    //            format: 'dd.MM.yyyy'
                                    //        }
                                    //    }
                                    //},
                                    {
                                        className: 'col-sm-3',
                                        key: 'DogumTarihi',
                                        type: 'maskedInput',
                                        templateOptions: {
                                            label: 'Dogum Tarihi',
                                            mask: "99.99.9999"
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'AskerlikDurumu',
                                        type: 'ui-select-single',
                                        templateOptions: {
                                            label: 'Askerlik',
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            valueProp: 'value',
                                            labelProp: 'name',
                                            options: [{ name: 'Yapmadı', value: 'Yapmadı' }, { name: 'Yaptı', value: 'yapti' }, { name: 'Tecilli', value: 'tecilli' }]
                                        }
                                    }
                                ]
                            },
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-3',
                                        key: 'Cinsiyet',
                                        type: 'select',
                                        defaultValue: true,
                                        templateOptions: {
                                            label: 'Cinsiyet',
                                            options: [{ name: 'Kadın', value: false }, { name: 'Erkek', value: true }]
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'Uyruk',
                                        type: 'ui-select-single',
                                        defaultValue: 'Türkiye',
                                        templateOptions: {
                                            label: 'Uyruk',
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            valueProp: 'value',
                                            labelProp: 'name',
                                            options: uyrukList
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'MedeniHali',
                                        type: 'ui-select-single',
                                        defaultValue: 'Bekar',
                                        templateOptions: {
                                            label: 'Medeni Hali',
                                            optionsAttr: 'bs-options',
                                            ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                                            valueProp: 'value',
                                            labelProp: 'name',
                                            options: [{ name: 'Bekar', value: 'Bekar' }, { name: 'Evli', value: 'Evli' }, { name: 'Dul', value: 'Dul' }]
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'IlkIseBaslamaTarihi',
                                        type: 'datepicker',
                                        defaultValue: new Date(),
                                        templateOptions: {
                                            label: 'İlk İş Tarihi',
                                            type: 'text',
                                            datepickerPopup: 'dd.MMMM.yyyy',
                                            datepickerOptions: {
                                                format: 'dd.MM.yyyy'
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-3',
                                        type: 'input',
                                        defaultValue: 0,
                                        key: 'CocukSayisi',
                                        templateOptions: {
                                            type: 'number',
                                            min: 0,
                                            max: 25,
                                            label: 'Çocuk Sayısı'
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        type: 'input',
                                        defaultValue: 0,
                                        key: 'KardesSayisi',
                                        templateOptions: {
                                            type: 'number',
                                            min: 0,
                                            max: 25,
                                            label: 'Kardeş Sayısı'
                                        }
                                    },
                                    {
                                        className: 'col-sm-6',
                                        type: 'input',
                                        key: 'Kardes_Sag_Bilgisi',
                                        defaultValue: 'Sağlıklı/Sağlıklı',
                                        templateOptions: {
                                            type: 'input',
                                            label: 'Kardeş Sağlık Bilgisi'
                                        }
                                    }
                                ]
                            },
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-4',
                                        key: 'anne_adi',
                                        type: 'input',
                                        templateOptions: {
                                            label: 'Anne Ad ve Soyadı'
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.anne_adi = null ?? formScope.model.anne_adi.toLocaleUpperCase('tr-TR');
                                            }
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'anne_sag',
                                        type: 'select',
                                        defaultValue: true,
                                        templateOptions: {
                                            label: 'Anne Sağlık Durumu',
                                            options: [{ name: 'Sağ', value: true }, { name: 'Vefat', value: false }]
                                        }
                                    },
                                    {
                                        className: 'col-sm-5',
                                        key: 'anne_sag_bilgisi',
                                        type: 'input',
                                        defaultValue: 'Sağlıklı',
                                        templateOptions: {
                                            label: 'Anne Sağlık Durumu Bilgisi',
                                            placeholder: 'HT,DM2,KAH,MI,KBY,KOAH,FMF,Talasemi gibi Kronik Hasta Bilgisi',
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.anne_sag_bilgisi = null ?? formScope.model.anne_sag_bilgisi.toLocaleUpperCase('tr-TR');
                                            }
                                        }
                                    }
                                ]
                            },
                            {
                                className: 'row  col-sm-12',
                                fieldGroup: [
                                    {
                                        className: 'col-sm-4',
                                        key: 'baba_adi',
                                        type: 'input',
                                        templateOptions: {
                                            label: 'Baba Ad ve Soyadı'
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.baba_adi = null ?? formScope.model.baba_adi.toLocaleUpperCase('tr-TR');
                                            }
                                        }
                                    },
                                    {
                                        className: 'col-sm-3',
                                        key: 'baba_sag',
                                        type: 'select',
                                        defaultValue: true,
                                        templateOptions: {
                                            label: 'Baba Sağlık Durumu',
                                            options: [{ name: 'Sağ', value: true }, { name: 'Vefat', value: false }]
                                        }
                                    },
                                    {
                                        className: 'col-sm-5',
                                        key: 'baba_sag_bilgisi',
                                        defaultValue: 'Sağlıklı',
                                        type: 'input',
                                        templateOptions: {
                                            label: 'Baba Sağlık Durumu Bilgisi',
                                            placeholder: 'HT,DM2,KAH,MI,KBY,KOAH,FMF,Talasemi gibi Kronik Hasta Bilgisi',
                                        },
                                        watcher: {
                                            listener: function (field, newValue, oldValue, formScope, stopWatching) {
                                                formScope.model.baba_sag_bilgisi = null ?? formScope.model.baba_sag_bilgisi.toLocaleUpperCase('tr-TR');
                                            }
                                        }
                                    }
                                ]
                            }
                        ];
                        vm.originalFields = angular.copy(vm.fields);

                    });
                });
            });
        });
    });

    function adjustDateForTimeOffset(dateToAdjust) {
        var offsetMs = dateToAdjust.getTimezoneOffset() * 60000;
        return new Date(dateToAdjust.getTime() - offsetMs);
    }
    function StringTrDateFormat(daten) {
        switch (vm.model.DogumTarihi.length) {
            case 8:
                return new Date((daten).substr(2, 2) + '.' + (daten).substr(0, 2) + '.' + (daten).substr(4, 4));
                break;//ay,gun,yıl
            default:
                var zamanDilimi = daten.split('.');
                return new Date(zamanDilimi[1] + '.' + zamanDilimi[0] + '.' + zamanDilimi[2]);
        }
    }

    function onSubmit() {
        if (vm.model.Sirket_Id > 0 && vm.model.Bolum_Id > 0) {
            vm.model.DogumTarihi = adjustDateForTimeOffset(StringTrDateFormat(vm.model.DogumTarihi));
            console.log(vm.model.DogumTarihi);
            vm.model.IlkIseBaslamaTarihi = adjustDateForTimeOffset(vm.model.IlkIseBaslamaTarihi);
            if (angular.isUndefined($stateParams.id)) {
                personellerSvc.AddPrsView(vm.model).then(function (response) {
                    $scope.savedSuccessfully = true;
                    $scope.message = vm.model.Adi + " " + vm.model.Soyadi + " Personeliniz başarıyla kayıt altına alınmıştır.";
                    vm.model.Personel_Id = response.Personel_Id;
                    vm.model.DogumTarihi = (new Date(vm.model.DogumTarihi)).toLocaleDateString('tr-TR');
                    $scope.PerGuid = response.PerGuid;
                    startTimer();
                    $scope.foto = false;
                }).catch(function (e) {
                    $scope.savedSuccessfully = false;
                    $scope.message = "Hata Kontrol Edin! Boşluk bırakmayın! " + e.data;
                    startTimer();
                });
            } else {
                personellerSvc.UpdatePrsView(vm.model).then(function (response) {
                    $scope.savedSuccessfully = true;
                    $scope.message = vm.model.Adi + " " + vm.model.Soyadi + " Personeliniz başarıyla güncellenmiştir.";
                    console.log($scope.message);
                    vm.model.Personel_Id = response.Personel_Id;
                    vm.model.DogumTarihi = (new Date(vm.model.DogumTarihi)).toLocaleDateString('tr-TR');
                    $scope.PerGuid = response.PerGuid;
                    startTimer();
                    $scope.foto = false;
                }).catch(function (e) {
                    $scope.savedSuccessfully = false;
                    $scope.message = "Hata Kontrol Edin! Boşluk bırakmayın! " + e.data;
                    startTimer();
                });

            }

        } else {
            $scope.savedSuccessfully = false;
            $scope.message = vm.model.Sirket_Id > 0 ? "Bölümü seçiniz !" : "Şirketi seçiniz !";
            startTimer();
        }
    }

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $scope.savedSuccessfully = false;
            $scope.message = "";
        }, 4000);
    };

    $scope.FotoGet = function () {
        personellerSvc.GetPicture($scope.PerGuid);
    };

    $scope.uploadFiles = function (dataUrl) {
        if (dataUrl && dataUrl.length) {
            $scope.dosyaList = true;
            Upload.upload({
                url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/kisisel',
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

    $scope.deleten = function (file, index) {
        uploadService.DeleteFile(file + '/x').then(function (response) {
            if (response.data === 1) { $scope.files.splice(index, 1); }
        });
    };

    $scope.lokal = $location.hash();
}

function personelHeadViewCtrl($scope, personellerSvc, $state) {
    var phvC = this;

    $scope.dahil = true;
    $scope.sdx = false;
    $scope.SirketBasligi = {
        id: 0,
        name: 'Şirketi Seçiniz.!'
    };

    $scope.BolumBasligi = {
        id: 0,
        name: 'Bölümü Seçiniz.!'
    };

    personellerSvc.GetSirketler(true).then(function (data) {
        $scope.collectionSirket = data;
        addToAllNodes(data);
        $scope.expandList = allNodes;
    });

    var allNodes = [];

    function addToAllNodes(children) {
        if (!children || typeof children === "array" && children.length === 0) {
            return;
        }
        for (var i = 0; i < children.length; i++) {
            allNodes.push(children[i]);
            addToAllNodes(children[i].children);
        }
    }

    $scope.status = {
        open: false,
        open2: false
    };

    $scope.showSelected = function (sel) {
        $scope.SirketBasligi.name = sel.name;
        $scope.SirketBasligi.id = sel.id;
        personellerSvc.MoviesIds.SirketId = sel.id;//ŞİRKETİN ID BİLGİSİ
        personellerSvc.MoviesIds.SirketAdi = sel.name;
        $scope.BolumBasligi = {
            id: 0,
            name: 'Bölümü Seçiniz.!'
        };
        $scope.status.open = !$scope.status.open;
        if (sel.id > 0)
            $scope.status.open2 = !$scope.status.open2;
        personellerSvc.GetSirketBolumleri(true, sel.id).then(function (data) {
            $scope.collectionBolum = data;
        });
    };

    $scope.showSelectedBolum = function (sel2) {
        $scope.BolumBasligi.name = sel2.name;
        $scope.BolumBasligi.id = sel2.id;
        personellerSvc.MoviesIds.BolumId = sel2.id;//bölümün Id bilgisi
        personellerSvc.MoviesIds.BolumAdi = sel2.name;
        $scope.status.open2 = !$scope.status.open2;
    };

    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        $scope.SirketBasligi.name = personellerSvc.MoviesIds.SirketAdi;
        $scope.SirketBasligi.id = personellerSvc.MoviesIds.SirketId;
        personellerSvc.GetSirketBolumleri(true, newValue).then(function (data) {
            $scope.collectionBolum = data;
        });
    });

    $scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
        $scope.BolumBasligi.Bolum_Id = personellerSvc.MoviesIds.BolumId;
        $scope.BolumBasligi.name = personellerSvc.MoviesIds.BolumAdi;
    });

    $scope.listeyeDon = function () {
        $state.go('Personel.PersonellerListDetail', {
            sti: $scope.SirketBasligi.id, blm: $scope.BolumBasligi.Bolum_Id,
            stiAd: $scope.SirketBasligi.name, blmAd: $scope.BolumBasligi.name
        });
    };

    $scope.$watch(function () { return personellerSvc.MoviesIds.fileImgPath; }, function (newValue, oldValue) {
        if (!newValue === '') { $scope.fileImgPath = personellerSvc.MoviesIds.fileImgPath; $scope.sdx = true; $scope.PerGuid = personellerSvc.MoviesIds.guidId; } else { $scope.fileImgPath = ''; $scope.sdx = false; }
    });

    $scope.FotoGetx = function () {
        personellerSvc.GetPicture($scope.PerGuid);
    };
}

personelHeadViewCtrl.$inject = ['$scope', 'personellerSvc', '$state'];
personelViewCrtl.$inject = ['$scope', 'personellerSvc', '$uibModal', '$http', '$q', '$timeout',
    'tanimlarSvc', '$state', '$stateParams', 'uploadService', 'ngAuthSettings', 'DTOptionsBuilder', 'Upload', '$window', '$location', 'PmSvc'];

angular
    .module('inspinia')
    .controller('personelHeadViewCtrl', personelHeadViewCtrl)
    .controller('personelViewCrtl', personelViewCrtl);