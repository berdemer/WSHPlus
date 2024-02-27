'use strict';

function sirketSvc($http, $q, ngAuthSettings) {
    var sirketSvcFactory = {};
    var serviceBase = ngAuthSettings.apiServiceBaseUri;


    var _getSirket = function (stat) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/Sirket?status='+stat).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updateSirket = function (key,sirket) {
        return $http.put(serviceBase + 'api/Sirket?key='+key,sirket ).then(function (response) {
            return response;
        });
    };

    var _insertSirket = function (sirket) {
        return $http.post(serviceBase + 'api/Sirket', sirket).then(function (response) {
            return response;
        });
    };

    var _getSirketBolumu = function (stat,sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/Sirket/bolumu/' + sirketId+'/'+stat).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updateSirketBolumu = function (key, sirketBolumu) {
        return $http.put(serviceBase + 'api/Sirket/bolumu?key=' + key, sirketBolumu).then(function (response) {
            return response;
        });
    };

    var _insertSirketBolumu = function (sirketBolumu) {
        return $http.post(serviceBase + 'api/Sirket/bolumu', sirketBolumu).then(function (response) {
            return response;
        });
    };

    var _getSirketAtama = function (sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/Sirket/atama/' + sirketId).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _insertSirketAtama = function (sirketAtama) {
        return $http.post(serviceBase + 'api/Sirket/atama', sirketAtama).then(function (response) {
            return response;
        });
    };

    var _deleteSirketAtama = function (id) {
        return $http.delete(serviceBase + 'api/Sirket/atama/'+id).then(function (response) {
            return response;
        });
    };

    var _getSirketDetayi = function (sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/Sirket/detayi/' + sirketId).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    var _insertSirketDetayi = function (sirketDetayi) {
        return $http.post(serviceBase + 'api/Sirket/detayi', sirketDetayi).then(function (response) {
            return response;
        });
    };

    var _deleteSirketDetayi = function (id) {
        return $http.delete(serviceBase + 'api/Sirket/detayi/' + id).then(function (response) {
            return response;
        });
    };

    var _updateSirketDetayi = function (id, sirketDetayi) {
        return $http.put(serviceBase + 'api/Sirket/detayi/' + id, sirketDetayi).then(function (response) {
            return response;
        });
    };

    var _getSaglikBirimi = function (Sti_id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/SaglikBirimi?id=' + Sti_id).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _insertSaglikBirimi = function (saglikBirimi) {
        return $http.post(serviceBase + 'api/SaglikBirimi', saglikBirimi).then(function (response) {
            return response;
        });
    };

    var _deleteSaglikBirimi = function (id) {
        return $http.delete(serviceBase + 'api/SaglikBirimi/' + id).then(function (response) {
            return response;
        });
    };

    var _updateSaglikBirimi = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/SaglikBirimi?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getMailListesi = function (Sti_id,Blm_id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/mail_Onerileri/bul/' + Sti_id + '/' + Blm_id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _insertMail_Onerileri = function (mail_Onerileri) {
        return $http.post(serviceBase + 'api/mail_Onerileri', mail_Onerileri).then(function (response) {
            return response;
        });
    };

    var _deleteMail_Onerileri = function (id) {
        return $http.delete(serviceBase + 'api/mail_Onerileri/' + id).then(function (response) {
            return response;
        });
    };

    var _updateMail_Onerileri = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/mail_Onerileri?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    sirketSvcFactory.GetSirket = _getSirket;
    sirketSvcFactory.GetSirketBolumu = _getSirketBolumu;
    sirketSvcFactory.GetSirketAtama = _getSirketAtama;
    sirketSvcFactory.GetSirketDetayi = _getSirketDetayi;
    sirketSvcFactory.GetSaglikBirimi = _getSaglikBirimi;
    sirketSvcFactory.GetMailListesi = _getMailListesi;
    sirketSvcFactory.UpdateSirket = _updateSirket;
    sirketSvcFactory.UpdateSirketBolumu = _updateSirketBolumu;
    sirketSvcFactory.UpdateSirketDetayi = _updateSirketDetayi;
    sirketSvcFactory.UpdateSaglikBirimi = _updateSaglikBirimi;
    sirketSvcFactory.UpdateMail_Onerileri = _updateMail_Onerileri;
    sirketSvcFactory.InsertSirket = _insertSirket;
    sirketSvcFactory.InsertSirketBolumu = _insertSirketBolumu;
    sirketSvcFactory.InsertSirketAtama = _insertSirketAtama;
    sirketSvcFactory.InsertSirketDetayi = _insertSirketDetayi;
    sirketSvcFactory.InsertSaglikBirimi = _insertSaglikBirimi;
    sirketSvcFactory.InsertMail_Onerileri = _insertMail_Onerileri;
    sirketSvcFactory.DeleteSirketAtama = _deleteSirketAtama;
    sirketSvcFactory.DeleteSirketDetayi = _deleteSirketDetayi;
    sirketSvcFactory.DeleteMail_Onerileri = _deleteMail_Onerileri;
    return sirketSvcFactory;

   
}
angular
    .module('inspinia')
    .factory('sirketSvc', sirketSvc);