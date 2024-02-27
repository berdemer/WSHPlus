'use strict';

function EreceteSvc($http, $q, ngAuthSettings) {
	var EreceteSvcFactory = {};
    var imzaServiceBase = ngAuthSettings.eimzaLinki;

    //Rcete servisleri
    var _receteYaz = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase+'api/imza/Recete/' + medulla + '/' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

    var _receteSil = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/receteSil/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _receteSorgula = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/receteSorgula/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _receteListeSorgula = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/receteListeSorgula/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _taniEkleme = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/TaniEkleme/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _aciklamaEkleme = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/AciklamaEkleme/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _ilacAciklamaEkleme = function (data, medulla, key) {
        var deferred = $q.defer();
        $http.post(imzaServiceBase + 'api/imza/IlacAciklamaEkleme/' + medulla + '/' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    //ilaç tanýmlama servis yardýmcýlarý 
    /** NormalReceteIlacListesi-->listeTipi
    AktifIlacListesi
    PasifIlacListesi
    KirmiziReceteIlacListesi
    TuruncuReceteIlacListesi
    MorReceteIlacListesi
    YesilReceteIlacListesi
    RaporTeshisListesi
    EtkinMaddeListesi
    YurtdisiIlacEtkinMaddeListesi
    AktifYurtdisiIlacListesi
    NormalReceteYurtdisiIlacListesi
    KirmiziReceteYurtdisiIlacListesi
    TuruncuReceteYurtdisiIlacListesi
    MorReceteYurtdisiIlacListesi
    YesilReceteYurtdisiIlacListesi
     **/

    var _getIlacListeleri = function (listeTipi,medullaSifresi,Tcno,tesisKodu) {
        var deferred = $q.defer();
        $http.get(imzaServiceBase + 'api/Yardimci/' + listeTipi + '/' + medullaSifresi + '/' + Tcno + '/' + tesisKodu).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _etkinMaddeSutListesi = function (medullaSifresi, Tcno, tesisKodu, etkinMaddeKodu) {
        var deferred = $q.defer();
        $http.get(imzaServiceBase + 'api/Yardimci/EtkinMaddeSutListesi/' + medullaSifresi + '/' + Tcno + '/' + tesisKodu + '/'+etkinMaddeKodu).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    EreceteSvcFactory.ReceteYaz = _receteYaz;
    EreceteSvcFactory.ReceteSil = _receteSil;
    EreceteSvcFactory.ReceteSorgula = _receteSorgula;
    EreceteSvcFactory.ReceteListeSorgula = _receteListeSorgula;
    EreceteSvcFactory.TaniEkleme = _taniEkleme;
    EreceteSvcFactory.AciklamaEkleme = _aciklamaEkleme;
    EreceteSvcFactory.IlacAciklamaEkleme = _ilacAciklamaEkleme;
    EreceteSvcFactory.IlacListeleri = _getIlacListeleri;
    EreceteSvcFactory.EtkinMaddeSutListesi = _etkinMaddeSutListesi;


	return EreceteSvcFactory;
}
angular
	.module('inspinia')
	.factory('EreceteSvc', EreceteSvc);