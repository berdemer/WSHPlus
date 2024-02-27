'use strict';
function fileManagerController($scope, Upload, $stateParams, ngAuthSettings, uploadService, authService, personellerSvc) {
    $scope.progress = 0;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var serviceUploadApi = ngAuthSettings.apiUploadService;
    $scope.listImage = {};

    uploadService.GetImageId($stateParams.id, $stateParams.folder).then(function (data) {
        $scope.listImage = data;
    });

    if ($stateParams.folder === 'kimlik') {
        authService.userView($stateParams.id).then(function (data) {
            $scope.bilgi = data;
        });
    }

    if ($stateParams.folder === 'personel') {
        personellerSvc.GetGuidPersonel($stateParams.id).then(function (data) {
            $scope.bilgi = { FullName: data.data.Adi + ' ' + data.data.Soyadi, Gorevi: data.data.Gorevi, id: data.data.PerGuid };
        });
    }

    $scope.servis = serviceUploadApi;

    function replaceAll(str, from, to) {
        var idx = str.indexOf(from);

        while (idx > -1) {
            str = str.replace(from, to);
            idx = str.indexOf(from);
        }
        return str;
    }

    $scope.deleten = function (file,index) {
        var genericFileName = replaceAll(file, '.', '/');
        uploadService.DeleteFile(genericFileName).then(function (response) {
            if (response.data === 1) { $scope.listImage.splice(index, 1); }
        });
    };

    $scope.upload = function (dataUrl) {
        Upload.upload({
            url: serviceBase + serviceUploadApi + $stateParams.id + '/' + $stateParams.folder,
            method: 'POST',
            data: {
                file: Upload.dataUrltoBlob(dataUrl)
            }
        }).then(function (response) {
            $scope.listImage.push(response.data[0]);//response.data array ilk objecti alındı.
        }, function (response) {
            if (response.status > 0) $scope.errorMsg = response.status
                + ': ' + response.data;
        }, function (evt) {
            $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
        });
    };

}

fileManagerController.$inject = ['$scope', 'Upload', 
    '$stateParams', 'ngAuthSettings', 'uploadService', 'authService','personellerSvc'];

angular
    .module('inspinia')
    .controller('fileManagerController', fileManagerController);
