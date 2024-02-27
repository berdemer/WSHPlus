'use strict';

function personellerSvc($http, $q, ngAuthSettings, $state, $resource) {

    var personellerSvcFactory = {};

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var _cardDataView = function () {
        return {
            perSearch: '',//genel arama bilgisi
            SearchBool: false,//arama bilgisi sorgulaması?
            SirBool: false,//şirket sorgulaması yapıldı
            BolBool: false,//bolum sorgulaması yapldı
            TotalItems: 0,
            PageCount: 4,
            PageNumber: 0,
            kontrolGecisleri: '',
            loading:false
        };
    };

    var _tcNoKontrol = function (k) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/TcKont/'+k).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };



    var _personelSearch = function (durum, arama, baslangic, uzunluk) {
        baslangic = isNaN(baslangic) ? 0 : baslangic;
        uzunluk = angular.isUndefined(uzunluk) ? 4 : uzunluk;
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/PrCdVw/' + durum + '/' + encodeURIComponent(arama) + '/' + baslangic + '/' + uzunluk).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _personelEgitimSearch = function (arama, sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/PrEgList/' + encodeURIComponent(arama) + '/' + sirketId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _sirketEgtPerListesi = function (sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/PrEgTmList/' + sirketId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _sirketSearch = function (sirketID, genel, durum, baslangic, uzunluk) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/SPCardList/' + sirketID + '/' + genel + '/' + durum + '/' + baslangic + '/' + uzunluk).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _bolumSearch = function (bolumID, sirketID, genel, durum, baslangic, uzunluk) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/BPCardList/' + bolumID + '/' + sirketID + '/' + genel + '/' + durum + '/' + baslangic + '/' + uzunluk).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getExtractPersoneller = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/personel/ExtractPrsList', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
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
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _alanGet = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/Alanlar').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getSirketler = function (stat) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/Sirketler/' + stat).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getSirketBolumleri = function (stat, sirketId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/SirketBolumleri/' + sirketId + '/' + stat).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsList = function (prsList) {
        return $http.post(serviceBase + 'api/personel/InsertPrsList', prsList).then(function (response) {
            return response;
        });
    };

    var _updatePrsList = function (prsList) {
        return $http.post(serviceBase + 'api/personel/UpdatePrsList', prsList).then(function (response) {
            return response;
        });
    };


    //kontroller arası veri paylaşımı için kullanıldı.
    //http://jsfiddle.net/27mk1n1o/  http://blogs.4ward.it/sharing-data-between-controllers-in-angularjs/

    var _sirketinBolumunIdleri = function () {
        return {
            SirketId: '',
            BolumId: '',
            SirketAdi: '',
            BolumAdi: '',
            dahil: true,
            aktif: true,
            fileImgPath: '',
            guidId: '',
            personel: ''
        };
    };

    var _getSirketPersonelleri = function (Sirkt, Genel, Status) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/SpecialSirktPersList/' + Sirkt + '/' + Genel + '/' + Status).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getBolumPersonelleri = function (Bolum, Sirkt, Genel, Status) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/SpecialBolumPersList/' + Bolum + '/' + Sirkt + '/' + Genel + '/' + Status).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsView = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/personel/insertPersonelView', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    var _updatePrsView = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/personel/updatePersonelView', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrs = function (id) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/personel?id=' + id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _activePrs = function (id) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/personel/Aktif/' + id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    var _getGuidPersonel = function (Guid) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/GuidId/' + Guid).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getAllPersList = function (durum) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/personel/AllPersList/' + durum).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getPicture = function (idx) {
        $state.go('Upload.FileManager', { id: idx, folder: 'personel' });
    };

    var _addPrsAddress = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/adres', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsAddress = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/adres?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsAddress = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/adres?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _standartDatalar = function () {
        return {
            unvanlar: [],
            gorevler: [],
            kanGrupları: [],
            egitimSeviyesi: [],
            muayeneNedeni: [],
            hekimler: []
        };
    };

    var _addPrsCalisma_Durumu = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/calisma_Durumu', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsCalisma_Durumu = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/calisma_Durumu?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsCalisma_Durumu = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/calisma_Durumu?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsCalisma_Gecmisi = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/calisma_Gecmisi', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsCalisma_Gecmisi = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/calisma_Gecmisi?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsCalisma_Gecmisi = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/calisma_Gecmisi?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsEgitimHayati = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/egitimHayati', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsEgitimHayati = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/egitimHayati?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsEgitimHayati = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/egitimHayati?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsDisRapor = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/disRapor', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsDisRapor = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/disRapor?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsDisRapor = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/disRapor?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsIcRapor = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/icRapor', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsIcRapor = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/icRapor?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsIcRapor = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/icRapor?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getIsgUser = function (name) {
        var deferred = $q.defer();
        $http({
            url: serviceBase + 'api/accounts/GetIsgUser?name=' + name,
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        }).then(function (response) {
            return deferred.resolve(response.data);
        }, function (error) {
            deferred.reject(error);
        });
        return deferred.promise;
    };

    var _getFullIsgUsers = function () {
        return $resource(ngAuthSettings.apiServiceBaseUri + 'api/accounts/users').query().$promise;
    };

    var _addPrsOzurluluk = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/ozurluluk', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsOzurluluk = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/ozurluluk?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsOzurluluk = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/ozurluluk?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsIsgEgitimi = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/isgEgitimi', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsIsgEgitimi = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/isgEgitimi?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsIsgEgitimi = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/isgEgitimi?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _addPrsKkd = function (data) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/kkd', data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updatePrsKkd = function (key, data) {
        var deferred = $q.defer();
        $http.put(serviceBase + 'api/kkd?key=' + key, data).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deletePrsKkd = function (key) {
        var deferred = $q.defer();
        $http.delete(serviceBase + 'api/kkd?id=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getAllUsersInRole = function (key) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/roles/GetAllUsersInRole?rolex=' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    personellerSvcFactory.CDV = _cardDataView;
    personellerSvcFactory.PersonelSearch = _personelSearch;
    personellerSvcFactory.SirketSearch = _sirketSearch;
    personellerSvcFactory.BolumSearch = _bolumSearch;
    personellerSvcFactory.AddPrsIsgEgitimi = _addPrsIsgEgitimi;
    personellerSvcFactory.UpdatePrsIsgEgitimi = _updatePrsIsgEgitimi;
    personellerSvcFactory.DeletePrsIsgEgitimi = _deletePrsIsgEgitimi;
    personellerSvcFactory.GetFullIsgUsers = _getFullIsgUsers;
    personellerSvcFactory.AddPrsOzurluluk = _addPrsOzurluluk;
    personellerSvcFactory.UpdatePrsOzurluluk = _updatePrsOzurluluk;
    personellerSvcFactory.DeletePrsOzurluluk = _deletePrsOzurluluk;
    personellerSvcFactory.GetIsgUser = _getIsgUser;
    personellerSvcFactory.AddPrsIcRapor = _addPrsIcRapor;
    personellerSvcFactory.UpdatePrsIcRapor = _updatePrsIcRapor;
    personellerSvcFactory.DeletePrsIcRapor = _deletePrsIcRapor;
    personellerSvcFactory.AddPrsDisRapor = _addPrsDisRapor;
    personellerSvcFactory.UpdatePrsDisRapor = _updatePrsDisRapor;
    personellerSvcFactory.DeletePrsDisRapor = _deletePrsDisRapor;
    personellerSvcFactory.AddPrsEgitimHayati = _addPrsEgitimHayati;
    personellerSvcFactory.UpdatePrsEgitimHayati = _updatePrsEgitimHayati;
    personellerSvcFactory.DeletePrsEgitimHayati = _deletePrsEgitimHayati;
    personellerSvcFactory.AddPrsCalisma_Gecmisi = _addPrsCalisma_Gecmisi;
    personellerSvcFactory.UpdatePrsCalisma_Gecmisi = _updatePrsCalisma_Gecmisi;
    personellerSvcFactory.DeletePrsCalisma_Gecmisi = _deletePrsCalisma_Gecmisi;
    personellerSvcFactory.AddPrsCalisma_Durumu = _addPrsCalisma_Durumu;
    personellerSvcFactory.UpdatePrsCalisma_Durumu = _updatePrsCalisma_Durumu;
    personellerSvcFactory.DeletePrsCalisma_Durumu = _deletePrsCalisma_Durumu;
    personellerSvcFactory.SD = _standartDatalar;
    personellerSvcFactory.DeletePrsAddress = _deletePrsAddress;
    personellerSvcFactory.UpdatePrsAddress = _updatePrsAddress;
    personellerSvcFactory.AddPrsAddress = _addPrsAddress;
    personellerSvcFactory.DeletePrs = _deletePrs;
    personellerSvcFactory.GetPicture = _getPicture;
    personellerSvcFactory.GetExtractPersoneller = _getExtractPersoneller;
    personellerSvcFactory.AddPrsList = _addPrsList;
    personellerSvcFactory.UpdatePrsList = _updatePrsList;
    personellerSvcFactory.GetSirketBolumleri = _getSirketBolumleri;
    personellerSvcFactory.GetSirketler = _getSirketler;
    personellerSvcFactory.GetExcelData = _getExcelData;
    personellerSvcFactory.AlanGet = _alanGet;
    personellerSvcFactory.MoviesIds = _sirketinBolumunIdleri;//kontroller arası aktarım için yazıldı.
    personellerSvcFactory.GetSirketPersonelleri = _getSirketPersonelleri;
    personellerSvcFactory.GetBolumPersonelleri = _getBolumPersonelleri;
    personellerSvcFactory.AddPrsView = _addPrsView;
    personellerSvcFactory.UpdatePrsView = _updatePrsView;
    personellerSvcFactory.ActivePrs = _activePrs;
    personellerSvcFactory.GetGuidPersonel = _getGuidPersonel;
    personellerSvcFactory.GetAllPersList = _getAllPersList;
    personellerSvcFactory.AddPrsKkd = _addPrsKkd;
    personellerSvcFactory.UpdatePrsKkd = _updatePrsKkd;
    personellerSvcFactory.DeletePrsKkd = _deletePrsKkd;
    personellerSvcFactory.GetAllUsersInRole = _getAllUsersInRole;
    personellerSvcFactory.PersonelEgitimSearch = _personelEgitimSearch;
    personellerSvcFactory.SirketEgtPerListesi = _sirketEgtPerListesi;
    personellerSvcFactory.TcNoKontrol = _tcNoKontrol;
    return personellerSvcFactory;
}

angular
    .module('inspinia')
    .factory('personellerSvc', personellerSvc);