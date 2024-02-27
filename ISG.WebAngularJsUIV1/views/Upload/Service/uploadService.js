'use strict';

function uploadService($http, $q,  ngAuthSettings) {
    var uploadServiceFactory = {};
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var serviceUploadApi = ngAuthSettings.apiUploadService;

    var _deleteFile = function (genericFileName) {
        return $http.delete(serviceBase + serviceUploadApi + genericFileName).then(function (response) { return response; });
    };


    var _deleteStiFile = function (genericFileName) {
        return $http.delete(serviceBase + serviceUploadApi +"stimg/"+ genericFileName).then(function (response) { return response; });
    };

    var _getImageId = function (IdGuid,Folder) {
        var deferred = $q.defer();
        $http.get(serviceBase + serviceUploadApi + IdGuid + '/' + Folder).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getImageStiId = function (DosyaTipi, Sirket_Id) {
        var deferred = $q.defer();
        $http.get(serviceBase + serviceUploadApi + "GetAllSti/" + DosyaTipi + '/' + Sirket_Id + '/0').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getFileStiId = function (DosyaTipi, Sirket_Id, DosyaTipiID) {
        var deferred = $q.defer();
        $http.get(serviceBase + serviceUploadApi + "GetAllSti/" + DosyaTipi + '/' + Sirket_Id + '/' + DosyaTipiID).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getFileId = function (IdGuid, Folder,idx) {
        var deferred = $q.defer();
        $http.get(serviceBase + serviceUploadApi + IdGuid + '/' + Folder + '/' + idx).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    uploadServiceFactory.GetFileId = _getFileId;
    uploadServiceFactory.GetImageId = _getImageId;
    uploadServiceFactory.DeleteFile = _deleteFile;
    uploadServiceFactory.GetFileStiId = _getFileStiId;
    uploadServiceFactory.GetImageStiId = _getImageStiId;
    uploadServiceFactory.DeleteStiFile = _deleteStiFile;

    return uploadServiceFactory;
}
angular
    .module('inspinia')
    .factory('uploadService', uploadService);





