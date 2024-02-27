'use strict';
function profilCtrl($scope, authService, ngAuthSettings, uploadService, notify) {
    var prC = this;
    $scope.servis = ngAuthSettings.apiServiceBaseUri;
    function _getAllData() {
        authService.userView(authService.authentication.id).then(function (data) {//birinci tamamladıkta sonra
            $scope.idl = data;
            prC = data;
            $scope.Roller = data.Roles;
            authService.GetAllRoles().then(function (roller) {//ikinci servis kullanılır.
                $scope.rols = roller;
                angular.forEach(data.Roles, function (UserRol) {//ikinci diziden çekilir.
                    var index = $scope.rols.map(function (item) {
                        return item.Name;//json taramasında kullanılır
                    }).indexOf(UserRol);//rols dizinde hangi rol indexini bulur 
                    if (index !== -1) {
                        $scope.rols.splice(index, 1);
                    }
                });
            });
        });
    }
    _getAllData();
    uploadService.GetImageId(authService.authentication.id, 'kimlik').then(function (data) {
        $scope.listImage = data;
    });

    $scope.passwordUpdate = function (userId, newPassword) {
        authService.adminUpdatePassword(userId, newPassword).then(function (resp) {
            notify({
                message: 'Başarılı bir şekilde güncellenmiştir.',
                classes: 'alert-success',
                templateUrl: $scope.inspiniaTemplate,
                duration: 3000,
                position: 'center'
            });
        });
    };
}

profilCtrl.$inject = ['$scope', 'authService', 'ngAuthSettings', 'uploadService', 'notify'];

angular
    .module('inspinia')
    .controller('profilCtrl ', profilCtrl);