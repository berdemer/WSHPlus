'use strict';

function usersListController($scope, $compile, DTOptionsBuilder,screenSize, DTColumnBuilder, 
    $resource, ngAuthSettings, $state, $uibModal, $log, authService,notify) {
    var vm = this;
    vm.message = '';
    vm.userNew = newUser;
    $scope.inspiniaTemplate = 'views/common/notify.html';
    vm.table = function() {
        return $resource(ngAuthSettings.apiServiceBaseUri + 'api/accounts/users').query().$promise;
    };

    vm.dtOptions =
        DTOptionsBuilder
        .fromFnPromise(vm.table)
         .withDataProp('data')
            .withOption('responsive', window.innerWidth < 1500 ? true : false)
                .withLanguageSource('/views/Personel/Controller/Turkish.json')
         .withOption('lengthMenu', [3, 10, 20, 50, 100])
         .withOption('iDisplayLength', 3)
         .withPaginationType('full_numbers')
       //  .withOption('initComplete', function (settings) {
       //    $compile(angular.element('#' + settings.sTableId).contents())($scope);
       //});
         .withOption('createdRow', function (row, data, dataIndex) {
             if (data["LockoutEnabled"] === 1) {
                 $('td', row).css('background-color', 'rgba(241, 223, 220, 0.78)');
             }
            $compile(angular.element(row).contents())($scope);
         });

   
   // (screenSize.is('xs, sm')) ? DTColumnBuilder.newColumn('Id').withTitle('ID').notVisible() :
    vm.dtColumns = [
        DTColumnBuilder.newColumn('Id').withTitle('ID'),
        DTColumnBuilder.newColumn('FullName').withTitle('Adı Soyadı'),
        DTColumnBuilder.newColumn('UserName').withTitle('Kullanıcı Adı'),
        DTColumnBuilder.newColumn(null).withTitle('Actions').notSortable()
        .renderWith(function (data, type, full, meta) {
            var init = 'as' + data["$id"]+'='+data["LockoutEnabled"];
            var model = 'as' + data["$id"];
            var classın = data["LockoutEnabled"] ? 0 :1;
            return '<a class="btn btn-primary" onclick=\"angular.element(this).scope().view(\'' + data.Id +
                '\')\" tooltip-placement="bottom" uib-tooltip="Kullanıcı Detayları"><i class="fa fa-user-secret"></i></a>&nbsp;' +
                ' <button class="btn btn-warning" onclick=\"angular.element(this).scope().edit(\'' + data.Id + '\')\">' +
                '<i class="fa fa-edit" tooltip-placement="bottom" uib-tooltip="Güncelleme yapabilirsiniz."></i>' +
                '</button>&nbsp;' +
                '<button class="btn btn-danger"  onclick=\"angular.element(this).scope().delete(\'' + data.Id + '\')\" tooltip-placement="bottom" uib-tooltip="Kullanıcı kaldırabilirsiniz">' +
                '<i class="fa fa-trash-o"></i>' +
                '</button>&nbsp;' +
                '<button type="button" ng-class="{activeZx:' + classın + '}" ng-init="' + init +
                '" ng-model="' + model + '" onclick=\"angular.element(this).scope().lockoutEnabled(\''
                + data.Id + '\',\'' + !(Boolean(data.LockoutEnabled)) + '\',\'' + data["FullName"] +
                '\')\" class="btn btn-success" uib-btn-checkbox tooltip-placement="bottom" uib-tooltip="Pasifize edebilirsiniz.">' +
                '   <i class="fa fa-lock"></i></button>&nbsp;' +
                '<button class="btn btn-info"  onclick=\"angular.element(this).scope().fileManager(\'' + data.Id + '\')\" tooltip-placement="bottom" uib-tooltip="Kullanıcı resmi yükleme">' +
                '<i class="fa fa-file-image-o"></i></button>';
                
        })
    ];



    vm.reloadData = reloadData;

    vm.dtInstance = {};

    $scope.lockoutEnabled = function (id, isLockout, name) {
        var a;
        var c;
        if (isLockout === 'true') {
            a = 'pasifize';
            c = 'alert-info';
        } else {
            a = 'aktive';
            c = 'alert-success';
        }
        authService.SetLockoutEnabled(id, isLockout).then(function (response) {
            vm.dtInstance.reloadData();
            notify({
                message: name + ' başarılı bir şekilde ' + a + ' edilmiştir.',
                classes: c,
                templateUrl: $scope.inspiniaTemplate,
                duration: 2500,
                position: 'center'
            });
        });
    };

    function newUser() {
        $state.go('Account.RegisterIn');
       // vm.dtInstance.reloadData();
    }

    $scope.edit = function (id) {
        $state.go('Account.UserUpdate', { id: id });
        //  vm.dtInstance.reloadData();
    };

    $scope.delete = function (id) {
        //modelin açılışı
        var modalInstance = $uibModal.open({//Modal oluşturma...
            animation: true,
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: 'sm',
            windowClass: "animated flipInY",
            resolve: {
                items: function () {
                    return id;//parametre girişi buradan verilir.
                }
            }
        });
        //modelin kapanışı     selecteditem değer alır..
        modalInstance.result.then(function (selectedItem) {
            if (selectedItem.kapanıs === true) vm.dtInstance.reloadData();
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };

    $scope.view = function (id) {
        $state.go('Account.UsersList.UserView', { id: id }, { reload: true });
    };

    $scope.fileManager = function (id) {

        $state.go('Upload.FileManager', { id: id ,folder:'kimlik'});
    };


    function reloadData() {
        var resetPaging = true;
        vm.dtInstance.reloadData(callback, resetPaging);
    }

    function callback(json) {
        console.log(json);
    }

}

usersListController.$inject = ['$scope', '$compile', 'DTOptionsBuilder', 'screenSize',
    'DTColumnBuilder', '$resource', 'ngAuthSettings', '$state', '$uibModal', '$log', 'authService', 'notify'];



function ModalInstanceCtrl($scope, $uibModalInstance, items, authService, $timeout) {

    authService.userView(items).then(function (data) {
        $scope.items = data;
    });

    $scope.ok = function () {

        authService.deleteRegistration($scope.items.Id).then(function (response) {
            $scope.message = "Kullanıcı başarıyla silinmiştir.";
            $scope.succsess = true;
            startTimer();

        },
        function (response) {
        var errors = [];
        for (var key in response.data.modelState) {
            for (var i = 0; i < response.data.modelState[key].length; i++) {
                errors.push(response.data.modelState[key][i]);
            }
        }
        $scope.succsess = false;
        $scope.message = errors.join(' ') + ":nedeniyle silinme işlemi başarısız olmuştur.";
    });
       
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            var sd = { kapanıs: true, bilgi: "kjlkj" };
            $uibModalInstance.close(sd);//modelin kapanışına değer gönderir.
        }, 1500);
    };

}

ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance', 'items', 'authService', '$timeout'];

angular
    .module('inspinia')
    .controller('usersListController', usersListController)
    .controller('ModalInstanceCtrl', ModalInstanceCtrl);