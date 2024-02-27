'use strict';

function OgSvc($http, $q, ngAuthSettings) {
	var OgSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _getOgPersonel = function (Guid) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/og/GuidId/' + Guid).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _OgBilgisi = function () {
	    return {
	        OzBilgi: '',
	        guidId:''
	    };
	};

	var _addPrsAllerji = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/allerji', data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	         deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsAllerji = function (key, data) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/allerji?key=' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsAllerji = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/allerji?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _addPrsAsi = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/asi', data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsAsi = function (key, data) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/asi?key=' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsAsi = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/asi?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _addPrsKronikHastalik = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/kronikHastalik', data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsKronikHastalik = function (key, data) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/kronikHastalik?key=' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsKronikHastalik = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/kronikHastalik?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _addPrsSoyGecmisi = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/soyGecmisi', data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsSoyGecmisi = function (key, data) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/soyGecmisi?key=' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsSoyGecmisi = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/soyGecmisi?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _addPrsAliskanlik = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/aliskanlik', data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsAliskanlik = function (key, data) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/aliskanlik?key=' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsAliskanlik = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/aliskanlik?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	     },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	OgSvcFactory.GetOzgecmisiPersonel = _getOgPersonel;
	OgSvcFactory.Oz = _OgBilgisi;
	OgSvcFactory.AddPrsAllerji = _addPrsAllerji;
	OgSvcFactory.UpdatePrsAllerji = _updatePrsAllerji;
	OgSvcFactory.DeletePrsAllerji = _deletePrsAllerji;
	OgSvcFactory.AddPrsAsi = _addPrsAsi;
	OgSvcFactory.UpdatePrsAsi = _updatePrsAsi;
	OgSvcFactory.DeletePrsAsi = _deletePrsAsi;
	OgSvcFactory.AddPrsKronikHastalik = _addPrsKronikHastalik;
	OgSvcFactory.UpdatePrsKronikHastalik = _updatePrsKronikHastalik;
	OgSvcFactory.DeletePrsKronikHastalik = _deletePrsKronikHastalik;
	OgSvcFactory.AddPrsSoyGecmisi = _addPrsSoyGecmisi;
	OgSvcFactory.UpdatePrsSoyGecmisi = _updatePrsSoyGecmisi;
	OgSvcFactory.DeletePrsSoyGecmisi = _deletePrsSoyGecmisi;
	OgSvcFactory.AddPrsAliskanlik = _addPrsAliskanlik;
	OgSvcFactory.UpdatePrsAliskanlik = _updatePrsAliskanlik;
	OgSvcFactory.DeletePrsAliskanlik = _deletePrsAliskanlik;
	return OgSvcFactory;
}
angular
	.module('inspinia')
	.factory('OgSvc', OgSvc);