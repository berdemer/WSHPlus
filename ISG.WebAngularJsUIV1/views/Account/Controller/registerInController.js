'use strict';

function registerInController($scope, $state, $timeout, authService, $stateParams) {

    var vm = this;
    vm.onSubmit = $scope.signUp;
    $scope.bilgi = 'Yeni Kayıt';
    vm.existingUsers = [];
    vm.model = {
        hide: 'kayit'
    };

    if (!angular.isUndefined($stateParams.id)) {
        authService.userView($stateParams.id).then(function (data) {
            vm.model = data;
            vm.model.hide = null;
            $scope.bilgi = 'Kayıt Güncelleme';
        });
        vm.fields = [
            {
                type: 'input',
                key: 'FirstName',
                templateOptions: {
                    label: 'Adı',
                    placeholder: 'Adınızı giriniz..',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'LastName',
                templateOptions: {
                    label: 'Soyadı',
                    placeholder: 'Soyadınızı giriniz..',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'TcNo',
                templateOptions: {
                    label: 'Tc No',
                    placeholder: 'Tc Kimlik Noyu giriniz..'
                }
            },
            {
                type: 'input',
                key: 'Email',
                templateOptions: {
                    type: 'email',
                    label: 'Mail Adresi',
                    placeholder: 'Mail adresini giriniz. xx@xx.xx',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'Meslek',
                templateOptions: {
                    label: 'Mesleği',
                    placeholder: 'Mesleğini giriniz..'
                }
            },
            {
                type: 'input',
                key: 'Gorevi',
                templateOptions: {
                    label: 'Görevi',
                    placeholder: 'Görevini giriniz..'
                }
            },
            {
                type: 'maskedInput',
                key: 'Tel',
                templateOptions: {
                    label: 'Telefon',
                    placeholder: 'Telefonu giriniz.. (505) 555-55-55 gibi',
                    mask: "(999) 999-99-99"
                }
            },
            {
                type: 'maskedInput',
                key: 'Tel1',
                templateOptions: {
                    label: 'Cep Telefonu',
                    placeholder: 'Cep Telefonu giriniz.. (505) 555-55-55 gibi',
                    mask: "(999) 999-99-99"
                }
            },
            {
                type: 'input',
                key: 'MedullaPassw',
                templateOptions: {
                    type: 'password',
                    label: 'Medulla Şifresi',
                    placeholder: 'Medulla şifrenizi giriniz..'
                }
            },
            {
                type: 'input',
                key: 'key',
                templateOptions: {
                    type: 'password',
                    label: 'E-İmza Şifresi',
                    placeholder: 'E-İmza şifrenizi giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorBransKodu',
                templateOptions: {
                    label: 'Uzmanlık Kodu',
                    placeholder: 'Uzmanlık Kodu giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorSertifikaKodu',
                templateOptions: {
                    label: 'İşyeri Uzmanı-Hekimi Sertifika No',
                    placeholder: 'Sertifika Kodu giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorTesisKodu',
                templateOptions: {
                    label: 'Tesis Kodu',
                    placeholder: 'Tesis Kodunuzu giriniz..'
                }
            },
        ];
    }
    else {
        vm.fields = [
            {
                type: 'input',
                key: 'FirstName',
                templateOptions: {
                    label: 'Adı',
                    placeholder: 'Adınızı giriniz..',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'LastName',
                templateOptions: {
                    label: 'Soyadı',
                    placeholder: 'Soyadınızı giriniz..',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'TcNo',
                templateOptions: {
                    label: 'Tc No',
                    placeholder: 'Tc Kimlik Noyu giriniz..'
                }
            },
            {
                key: 'UserName',
                type: 'input',
                templateOptions: {
                    label: 'Kullanıcı Adı',
                    required: true,
                    placeholder: 'Kullanıcı Adınızı giriniz..',
                    onKeydown: function (value, options) {
                        options.validation.show = false;
                    },
                    onBlur: function (value, options) {
                        options.validation.show = null;
                    }
                },
                asyncValidators: {
                    uniqueUsername: {
                        expression: function ($viewValue, $modelValue, scope) {
                            scope.options.templateOptions.loading = true;
                            return $timeout(function () {
                                scope.options.templateOptions.loading = false;
                                if (vm.existingUsers.indexOf($viewValue) !== -1) {
                                    $scope.message = "Bu kullanıcı adı zaten alınmış.";
                                   /* throw new Error('taken');*/
                                }
                            }, 1000);
                        },
                        message: '"Bu kullanıcı adı zaten alınmış."'
                    }
                },
                modelOptions: {
                    updateOn: 'blur'
                }
            },
            {
                type: 'input',
                key: 'Email',
                templateOptions: {
                    type: 'email',
                    label: 'Mail Adresi',
                    placeholder: 'Mail adresini giriniz. xx@xx.xx',
                    required: true
                }
            },
            {
                type: 'input',
                key: 'Password',
                templateOptions: {
                    type: 'password',
                    label: 'Şifre',
                    placeholder: 'Şifrenizi giriniz. 12Abc.? gibi',
                    required: true,
                    maxlength: 10,
                    minlength: 4
                },
                hideExpression: '!model.hide'
            },
            {
                type: 'input',
                key: 'ConfirmPassword',
                optionsTypes: ['matchField'],
                model: vm.confirmationModel,
                templateOptions: {
                    type: 'password',
                    label: 'Şifre Tekrarı',
                    placeholder: 'Şifrenizi giriniz. 12Abc.? gibi',
                    required: true,
                    maxlength: 10,
                    minlength: 4
                },
                data: {
                    fieldToMatch: 'Password',
                    modelToMatch: vm.model
                },
                hideExpression: '!model.hide'
            },
            {
                type: 'input',
                key: 'Meslek',
                templateOptions: {
                    label: 'Mesleği',
                    placeholder: 'Mesleğini giriniz..İş Yeri Hekimi ve İş Güvenliği Uzmanı gibi giriniz'
                }
            },
            {
                type: 'input',
                key: 'Gorevi',
                templateOptions: {
                    label: 'Görevi',
                    placeholder: 'Görevini giriniz..İş Yeri Hekimi ve İş Güvenliği Uzmanı gibi giriniz'
                }
            },
            {
                type: 'maskedInput',
                key: 'Tel',
                templateOptions: {
                    label: 'Telefon',
                    placeholder: 'Telefonu giriniz.. (505) 555-55-55 gibi',
                    mask: "(999) 999-99-99"
                }
            },
            {
                type: 'maskedInput',
                key: 'Tel1',
                templateOptions: {
                    label: 'Cep Telefonu',
                    placeholder: 'Cep Telefonu giriniz.. (505) 555-55-55 gibi',
                    mask: "(999) 999-99-99"
                }
            },
            {
                type: 'input',
                key: 'MedullaPassw',
                templateOptions: {
                    type: 'password',
                    label: 'Medulla Şifresi',
                    placeholder: 'Medulla şifrenizi giriniz..'
                }
            },
            {
                type: 'input',
                key: 'key',
                templateOptions: {
                    type: 'password',
                    label: 'E-İmza Şifresi',
                    placeholder: 'E-İmza şifrenizi giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorBransKodu',
                templateOptions: {
                    label: 'Uzmanlık Kodu',
                    placeholder: 'Uzmanlık Kodu giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorSertifikaKodu',
                templateOptions: {
                    label: 'İşyeri Uzmanı-Hekimi Sertifika No',
                    placeholder: 'Sertifika Kodu giriniz..'
                }
            },
            {
                type: 'input',
                key: 'doktorTesisKodu',
                templateOptions: {
                    label: 'Tesis Kodu',
                    placeholder: 'Tesis Kodunuzu giriniz..'
                }
            },
        ];

    }


    authService.fullUsers().then(function (data) {
        angular.forEach(data, function (value) {
            vm.existingUsers.push(value.UserName.toLowerCase());
        });
    });
    ///************HTML templete yüklemeden çalışmaz....**************/////
    


    vm.originalFields = angular.copy(vm.fields);


    $scope.savedSuccessfully = false;
    $scope.message = "";


    $scope.signUp = function () {

        if (angular.isUndefined($stateParams.id)) {
            authService.saveRegistration(vm.model).then(function (response) {

                $scope.savedSuccessfully = true;
                $scope.message = "Kullanıcı başarıyla kayıt altına alınmıştır.2 sn sonra kullanıcı listesi sayfasına yönlendireleceksiniz.";
                startTimer();

            },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    $scope.message = errors.join(' ') + ":nedeniyle kullanıcı kaydı başarısız olmuştur.";
                });
        }
        else {
            authService.updateRegistration(vm.model).then(function (response) {

                $scope.savedSuccessfully = true;
                $scope.message = "Kullanıcı başarıyla güncellenmiştir.2 sn sonra kullanıcı listesi sayfasına yönlendireleceksiniz.";
                startTimer();

            },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    $scope.message = errors.join(' ') + ":nedeniyle kullanıcı kaydı başarısız olmuştur.";
                });
        }
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $state.go('Account.UsersList');
        }, 2000);
    };
}

registerInController.$inject = ['$scope', '$state', '$timeout', 'authService', '$stateParams'];

angular
    .module('inspinia')
    .controller('registerInController', registerInController);