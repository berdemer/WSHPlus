'use strict';

function SMSvc($http, $q, ngAuthSettings) {

	var SMSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _getAllSbirimleri = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/ilac/SBList').then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getSBStokListeleri = function (id, status) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/ilac/SBStokList/'+id+'/'+status).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _ilacAra = function (value) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/ilac/Ara/' + value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _hastalikAra = function (value) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/icd/Ara/' + value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokEkle = function (value) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/ilac/AddIlacStok' , value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokGuncelle = function (key, value) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/ilac/UpdateIlacStok?key=' + key , value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokSil = function (key, value) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/ilac/DeleteIlacStok?key=' + key, value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokGirisiAl = function (id,st) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/ilac/StokGirisi/' +id+'/'+st).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokGirisiEkle = function (value) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/ilac/StokGirisi', value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokGirisiGuncelle = function (key, value) {
	    var deferred = $q.defer();
	    $http.put(serviceBase + 'api/ilac/StokGirisiGuncelle?key=' + key, value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _stokGirisiSil = function (key, value) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/ilac/StokGirisiSil?key=' + key, value).then(function (response) {
	        deferred.resolve(response.data);
	    },function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _mailOnerileri = function (oneri,stiId,blmId) {
	    var deferred = $q.defer();
        $http.get(serviceBase + 'api/mail_Onerileri/onerim/' + oneri + '/' + stiId + '/' + blmId).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	SMSvcFactory.AllSbirimleri = _getAllSbirimleri;
	SMSvcFactory.SBStokListeleri = _getSBStokListeleri;
	SMSvcFactory.StokEkle = _stokEkle;
	SMSvcFactory.StokGuncelle = _stokGuncelle;
	SMSvcFactory.StokSil = _stokSil;
	SMSvcFactory.StokGirisiAl = _stokGirisiAl;
	SMSvcFactory.StokGirisiEkle = _stokGirisiEkle;
	SMSvcFactory.StokGirisiGuncelle = _stokGirisiGuncelle;
	SMSvcFactory.StokGirisiSil = _stokGirisiSil;
	SMSvcFactory.IlacAra = _ilacAra;
	SMSvcFactory.HastalikAra = _hastalikAra;
	SMSvcFactory.MailOnerileri = _mailOnerileri;
	return SMSvcFactory;
}
angular
	.module('inspinia')
	.factory('SMSvc', SMSvc);