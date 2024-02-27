'use strict';
function imzaCtrl($scope, authService, notify) {
    var imC = this;
    function _getAllData() {
        authService.userView(authService.authentication.id).then(function (data) {//birinci tamamladıkta sonra
            imC.val = data;         
        });
    }
    _getAllData();

    imC.submitForm = function () {
        authService.updateRegistration(imC.val).then(function (response) {
            notify({
                message: 'Başarılı bir şekilde güncellenmiştir.',
                classes: 'alert-success',
                templateUrl: $scope.inspiniaTemplate,
                duration: 3000,
                position: 'center'
            });
        },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                };
                notify({
                    message: errors.join(' '),
                    classes: 'alert-alert',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 3000,
                    position: 'center'
                });
            });
    };

}

imzaCtrl.$inject = ['$scope', 'authService', 'notify'];

angular
    .module('inspinia')
    .controller('imzaCtrl ', imzaCtrl);