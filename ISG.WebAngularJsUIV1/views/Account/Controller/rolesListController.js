'use strict';

function rolesListController(authService, $scope, $timeout, notify) {

    var vm = this;
    vm.roller = {};
    vm.searchRole = '';
    vm.existingRoles = [];
    vm.gridData = {};
    vm.model = {};
    vm.save = _save;
    vm.delete = _delete;
    vm.GetAllData = _getAllData;
    vm.model = {};

    function _getAllData() {
        authService.GetAllRoles().then(function (data) {
            vm.gridData = data;
            angular.forEach(data, function (value) {
                vm.existingRoles.push(value.Name.toLowerCase());
            });
        });
    }
  
    vm.GetAllData();
    ///************HTML templete yüklemeden çalışmaz....**************/////
    vm.fields = [
         {
             key: 'Name',
             type: 'input-loader',
             templateOptions: {
                 label: 'Yetki Adı',
                 placeholder: 'Yetkilendirme giriniz..',
                 //    required: true,
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
                             if (vm.existingRoles.indexOf($viewValue.toLowerCase()) !== -1) {
                                 throw new Error('taken');
                             }
                         }, 1000);
                     },
                     message: '"Bu yetki adı zaten alınmış."'
                 }
             }
         }
    ];

    vm.originalFields = angular.copy(vm.fields);

    function _save() {
        authService.SaveRole(vm.model).then(function (response) {
            notify({
                message: 'Başarılı bir şekilde güncellenmiştir',
                classes: 'alert-success',
                templateUrl: $scope.inspiniaTemplate,
                duration: 3000,
                position: 'center'
            });
            vm.GetAllData();
        },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                notify({
                    message: 'Güncellenme yapılamadı..Hata! sonucu var',
                    classes: 'alert-warning',
                    templateUrl: $scope.inspiniaTemplate,
                    duration: 3000,
                    position: 'center'
                });
            });
    } 

    function _delete(id) {
        authService.DeleteRole(id).then(function (response) {
            notify({
                message: 'Silinme işlemi tamamlanmıştır',
                classes: 'alert-warning',
                templateUrl: $scope.inspiniaTemplate,
                duration: 3000,
                position: 'center'
            });
            vm.GetAllData();
        },
        function (response) {
            var errors = [];
            for (var key in response.data.modelState) {
                for (var i = 0; i < response.data.modelState[key].length; i++) {
                    errors.push(response.data.modelState[key][i]);
                }
            }
        });
    }

}
rolesListController.$inject = ['authService', '$scope', '$timeout', 'notify'];

angular
    .module('inspinia')
    .controller('rolesListController', rolesListController);