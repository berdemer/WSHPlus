'use strict';

function egitimSvc($http, $q, ngAuthSettings) {
	var egitimSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _egitimBilgisi = function () {
		return {
			id: null,
			data:null			
		};
	};

	var _egtmAlList = function (StiId, Year) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/isgTopluEgitimi/EgtmAlList/' + StiId + '/' + Year ).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _topluEgitimListesi = function (StiId,Year) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/isgTopluEgitimi/TopluEgitimListesi/' + Year + '/' + StiId).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getISGSablonlari = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/isgTopluEgitimi/sablon').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addISGSablonlari = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/isgTopluEgitimi/sablon', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deleteISGSablonlari = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/isgTopluEgitimi/sablon/' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addIsgTopluEgitimi = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/isgTopluEgitimi', data).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updateIsgTopluEgitimi = function (id,data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/isgTopluEgitimi/'+id, data).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deleteTopluEgitim = function (id) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/isgTopluEgitimi?id=' + id).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getTopluEgitim = function (id) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/isgTopluEgitimi/' + id).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};



	egitimSvcFactory.Egitim = _egitimBilgisi;
	egitimSvcFactory.GetISGSablonlari = _getISGSablonlari;
	egitimSvcFactory.AddISGSablonlari = _addISGSablonlari;
	egitimSvcFactory.DeleteISGSablonlari = _deleteISGSablonlari;
	egitimSvcFactory.AddIsgTopluEgitimi = _addIsgTopluEgitimi;
	egitimSvcFactory.UpdateIsgTopluEgitimi = _updateIsgTopluEgitimi;
	egitimSvcFactory.TopluEgitimListesi = _topluEgitimListesi;
	egitimSvcFactory.DeleteTopluEgitim = _deleteTopluEgitim;
	egitimSvcFactory.TopluEgitimBilgiServisi = _egitimBilgisi;
	egitimSvcFactory.GetTopluEgitim = _getTopluEgitim;
	egitimSvcFactory.EgtmAlList = _egtmAlList;

	return egitimSvcFactory;
}
angular
	.module('inspinia')
	.factory('egitimSvc', egitimSvc);