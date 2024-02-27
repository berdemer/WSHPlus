'use strict';

function IkazSvc($http, $q, ngAuthSettings) {
	var IkazSvcFactory = {};
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var _getAktifIkazlar = function (id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/ikaz/' + id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getTumIkazlar = function (id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/ikaz/tumu/' + id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _ikazGuncelle = function (key, value) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/ikaz?key=' + key, value).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _ikazEkle = function (value) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/ikaz', value).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _ikazSil = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/ikaz?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    IkazSvcFactory.GetAktifIkazlar = _getAktifIkazlar;
    IkazSvcFactory.IkazGuncelle = _ikazGuncelle;
    IkazSvcFactory.IkazEkle = _ikazEkle;
    IkazSvcFactory.IkazSil = _ikazSil;
    IkazSvcFactory.GetTumIkazlar = _getTumIkazlar;

	return IkazSvcFactory;
}
angular
	.module('inspinia')
	.factory('IkazSvc', IkazSvc);