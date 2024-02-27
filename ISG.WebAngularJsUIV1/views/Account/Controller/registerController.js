'use strict';

function registerController($scope, $location, $timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        UserName: "",
        Password: "",
        ConfirmPassword: "",
        Email: "",
        FirstName: "",
        LastName:""
    };

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "Kayıt yapılmıştır...2 sn sonra giriş sayfasına yönlendirileceksiniz.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Kayıt Hatası:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    };

}

registerController.$inject = ['$scope', '$location', '$timeout', 'authService'];

angular
    .module('inspinia')
    .controller('registerController', registerController);