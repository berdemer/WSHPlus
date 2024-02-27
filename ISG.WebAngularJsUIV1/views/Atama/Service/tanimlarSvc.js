'use strict';

function tanimlarSvc($http, $q, ngAuthSettings) {
	var tanimlarSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _deleteTanimView = function (id) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/tanim/' +id).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updateTanimView = function (key,data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/tanim/Update/'+key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addTanimView = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/tanim', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getTanimlar = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/tanim/Gfx').then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getIlceler = function (key) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/tanim/ilceler?tanimAd=' + key).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _getTanimAdiList = function (key) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/tanim?tanimAd='+key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	tanimlarSvcFactory.GetTanimlar = _getTanimlar;
	tanimlarSvcFactory.DeleteTanimView = _deleteTanimView;
	tanimlarSvcFactory.UpdateTanimView = _updateTanimView;
	tanimlarSvcFactory.AddTanimView = _addTanimView;
	tanimlarSvcFactory.GetTanimAdiList = _getTanimAdiList;
	tanimlarSvcFactory.GetIlceler = _getIlceler;
	return tanimlarSvcFactory;
}
angular
	.module('inspinia')
	.factory('tanimlarSvc', tanimlarSvc);

