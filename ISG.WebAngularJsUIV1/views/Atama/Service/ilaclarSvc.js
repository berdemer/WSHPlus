'use strict';

function ilaclarSvc($http, $q, ngAuthSettings, $state, $resource) {

    var ilaclarSvcFactory = {};

    var serviceBase = ngAuthSettings.apiServiceBaseUri;


    var _getExtractllaclar = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/ilac/ExtractIlacList', data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    var apiPath = function (data) {
        return ngAuthSettings.isAzure ? serviceBase + 'api/azurExcelDepo/ExcelDataAl/' + data.fileNames + '/' + data.sheet.id :
            serviceBase + 'api/ilac/ExcelDataAl/' + data.fileNames + '/' + data.sheet + '/' + data.HDR;
    };

    var _getExcelData = function (data) {
        var deferred = $q.defer();
        $http.get(apiPath(data)).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _tabloTemizle = function (data) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/ilac/IlaclariSil').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _alanGet = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/ilac/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _kubKt = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/ilac/kubKtlistesi').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addIlacList = function (ilacList) {
        return $http.post(serviceBase + 'api/ilac/InsertIlacList', ilacList).then(function (response) {
            return response;
        });
    };
    //ÖNEMLİ:***response yanıtları farklı olur. addilacist response.data  dönüşü ilacfiyatlist ise response düz olarak alınır.
    var _ilacFiyatListesi = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/ilac/IlacFiyatList', data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    /////////////ICD10 servis alanı

    var _getExtractIcd = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/azurExcelDepo/ExcelDataAl/', data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _alanGetICD = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/icd/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addICD10List = function (icdList) {
        return $http.post(serviceBase + 'api/icd/InsertICD10List', icdList).then(function (response) {
            return response;
        });
    };

    var _icd10UpdateList = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/icd/ICD10UpdateList', data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    //////////////resimler servis alanı

    var _alanGetResimler = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/resimler/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    ///////////////rad servis alanı

    var _addRadList = function (radList) {
        return $http.post(serviceBase + 'api/radyoloji/InsertRadyolojiList', radList).then(function (response) {
            return response;
        });
    };

    var _alanGetRad = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/radyoloji/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    ///////////////Odio servis alanı

    var _addOdioList = function (odioList) {
        return $http.post(serviceBase + 'api/odio/InsertOdioList', odioList).then(function (response) {
            return response;
        });
    };

    var _alanGetOdio = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/odio/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    ///////////////SFT servis alanı

    var _addSftList = function (sftList) {
        return $http.post(serviceBase + 'api/sft/InsertSftList', sftList).then(function (response) {
            return response;
        });
    };

    var _alanGetSft = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/sft/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    ///////////////Lab servis alanı

    var _addBiyokimyaList = function (biyokimyaList) {
        return $http.post(serviceBase + 'api/laboratuar/InsertBiyokimyaList', biyokimyaList).then(function (response) {
            return response;
        });
    };

    var _alanGetBiyokimya = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/laboratuar/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    ///////////////Hemogram servis alanı

    var _addHemogramList = function (hemogramList) {
        return $http.post(serviceBase + 'api/hemogram/InsertHemogramList', hemogramList).then(function (response) {
            return response;
        });
    };

    var _alanGetHemogram = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/hemogram/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getAzureExtractImages = function (KlasorAdi,DosyaAdi,TcNo,list) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/resimler/tpazur/' + KlasorAdi + '/' + DosyaAdi + '/' + TcNo,list).
            then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    ilaclarSvcFactory.GetExtractllaclar = _getExtractllaclar;
    ilaclarSvcFactory.AddIlacList = _addIlacList;
    ilaclarSvcFactory.GetExcelData = _getExcelData;
    ilaclarSvcFactory.AlanGet = _alanGet;
    ilaclarSvcFactory.IlacFiyatListesi = _ilacFiyatListesi;
    ilaclarSvcFactory.IlacTablosuTemizle = _tabloTemizle;
    ilaclarSvcFactory.KubKt = _kubKt;

    ilaclarSvcFactory.GetExtractIcd = _getExtractIcd;
    ilaclarSvcFactory.AddICD10List = _addICD10List;
    ilaclarSvcFactory.AlanGetICD = _alanGetICD;
    ilaclarSvcFactory.ICDD10UpdateList = _icd10UpdateList;

    ilaclarSvcFactory.AlanGetResimler = _alanGetResimler;

    ilaclarSvcFactory.AlanGetRad = _alanGetRad;
    ilaclarSvcFactory.AddRadList = _addRadList;

    ilaclarSvcFactory.AlanGetOdio = _alanGetOdio;
    ilaclarSvcFactory.AddOdioList = _addOdioList;

    ilaclarSvcFactory.AlanGetSft = _alanGetSft;
    ilaclarSvcFactory.AddSftList = _addSftList;

    ilaclarSvcFactory.AlanGetBiyokimya = _alanGetBiyokimya;
    ilaclarSvcFactory.AddBiyokimyaList = _addBiyokimyaList;

    ilaclarSvcFactory.AlanGetHemogram = _alanGetHemogram;
    ilaclarSvcFactory.AddHemogramList = _addHemogramList;

    ilaclarSvcFactory.GetAzureExtractImages = _getAzureExtractImages;
    return ilaclarSvcFactory;
}
angular
    .module('inspinia')
    .factory('ilaclarSvc', ilaclarSvc);