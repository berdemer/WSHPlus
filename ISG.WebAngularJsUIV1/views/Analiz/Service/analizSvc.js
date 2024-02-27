'use strict';

function analizSvc($http, $q, ngAuthSettings, authService) {
	var analizSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _groupList = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/groupList').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _groupSelect = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/groupSelect/' + val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};


	var _groupSearch = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/groupSearch/' + val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _meslekHastaliklari = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/meslekHastaliklari/' + val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _isler = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/isler/' + val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _bolumRiski = function (sti, blm) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/bolumRisk/' + sti + '/' + blm).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _bolumRiskiAdd = function (val) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/analiz/bolumRiski', val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _yillikDegerlendirme = function (year, sbId) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/revirIslem/RevirAnaliz/' + year + '/' + sbId).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _isKazasiListesi = function (Sirket_Id, muayeneDurumu, muayeneSonucu, year) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/MuDuList/' + Sirket_Id + '/' + muayeneDurumu + '/' + muayeneSonucu + '/' + year).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _bolumlerinIsKazaSayilari = function (Sirket_Id, muayeneDurumu, muayeneSonucu, year) {
	    var deferred = $q.defer();
	    $http.get(serviceBase + 'api/analiz/BoIsKSayilari/' + Sirket_Id + '/' + muayeneDurumu + '/' + muayeneSonucu + '/' + year).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
    };

    var _periyodikMuayeneTakibiGelenler = function (Sirket_Id, yil, ay, sure) {
	    var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/PerMuaTakGelen/' + Sirket_Id + '/' + yil + '/' + ay + '/' + sure).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

    var _asiTakibiGelenler = function (Sirket_Id, yil, ay) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/AsiTakGelen/' + Sirket_Id + '/' + yil + '/' + ay).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

	var _raporlularAnalizi = function (Sirket_Id, yil, ay,muayeneTuru) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/analiz/DisRaporlularAnalizi/' + muayeneTuru+'/' + Sirket_Id + '/' + yil + '/' + ay).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};


    var _revirIslemleri = function (Sirket_Id, date,month,year) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/RevirIslemleri/' + Sirket_Id + '/' + date + '/' + month + '/' + year).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }; 

    var _aylikRevirIslemleri = function (Sirket_Id, date, month, year) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/AylikRevirIslemleri/' + Sirket_Id + '/' + date + '/' + month + '/' + year).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }; 

    var _personelBilgisi = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/accounts/user/' + authService.authentication.id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }; 

    var _engelliTakibi = function (Sirket_Id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/EngelliTakibi/' + Sirket_Id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _kronikHastaTakibi = function (Sirket_Id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/KronikHastaTakibi/' + Sirket_Id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _allerjiHastaTakibi = function (Sirket_Id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/AllerjiHastaTakibi/' + Sirket_Id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _aliskanlikHastaTakibi = function (Sirket_Id) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/analiz/AliskanlikHastaTakibi/' + Sirket_Id).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

	var _addCalismaAnalizi = function (val) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/calismaanalizi', val).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updateCalismaAnalizi = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/calismaanalizi?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deleteCalismaAnalizi = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/calismaanalizi?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _meslekAra = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/calismaanalizi/meslekAra/' + val+'/').then(function (response) {
			//decimal değer gönderdin '/' olmazsa api görmüyor.
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _bolumAra = function (blmId, stiId) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/calismaanalizi/bolumAra/' + blmId + '/' + stiId).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _meslekListesi = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/calismaanalizi/MeslekListesi').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	analizSvcFactory.MeslekListesi = _meslekListesi;
	analizSvcFactory.MeslekAra = _meslekAra;
	analizSvcFactory.BolumAra = _bolumAra;
	analizSvcFactory.DeleteCalismaAnalizi = _deleteCalismaAnalizi;
	analizSvcFactory.UpdateCalismaAnalizi = _updateCalismaAnalizi;
	analizSvcFactory.AddCalismaAnalizi = _addCalismaAnalizi;
	analizSvcFactory.Isler = _isler;
	analizSvcFactory.MeslekHastaliklari = _meslekHastaliklari;
	analizSvcFactory.GroupSelect = _groupSelect;
	analizSvcFactory.GroupList = _groupList;
	analizSvcFactory.GroupSearch = _groupSearch;
	analizSvcFactory.BolumRiski = _bolumRiski;
	analizSvcFactory.BolumRiskiAdd = _bolumRiskiAdd;
	analizSvcFactory.YillikDegerlendirme = _yillikDegerlendirme;
	analizSvcFactory.IsKazasiListesi = _isKazasiListesi;
    analizSvcFactory.BolumlerinIsKazaSayilari = _bolumlerinIsKazaSayilari;
    analizSvcFactory.PeriyodikMuayeneTakibiGelenler = _periyodikMuayeneTakibiGelenler;
    analizSvcFactory.AsiTakibiGelenler = _asiTakibiGelenler;
    analizSvcFactory.PersonelBilgisi = _personelBilgisi;
    analizSvcFactory.RevirIslemleri = _revirIslemleri;
    analizSvcFactory.EngelliTakibi = _engelliTakibi;
    analizSvcFactory.KronikHastaTakibi = _kronikHastaTakibi;
    analizSvcFactory.AllerjiHastaTakibi = _allerjiHastaTakibi;
    analizSvcFactory.AliskanlikHastaTakibi = _aliskanlikHastaTakibi;
	analizSvcFactory.RaporlularAnalizi = _raporlularAnalizi;
	return analizSvcFactory;
}

angular
	.module('inspinia')
	.factory('analizSvc', analizSvc);