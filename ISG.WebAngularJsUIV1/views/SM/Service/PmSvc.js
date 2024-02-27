'use strict';

function PmSvc($http, $q, ngAuthSettings) {
	var PmSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _getPmPersonel = function (Guid) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/pm/' + Guid).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getIlac = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/ilac/Ara/' + val).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
    };

    var _getKubKtAra = function (val) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/ilac/KubKtAra',val).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

	var _getBolumler = function () {
		var deferred = $q.defer();
		$http.get(serviceBase + '/api/tanim?tanimAd=Sevk Bölüm Tablosu').then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _PmBilgisi = function () {
		return {
			PmBilgi: '',
			guidId: '',
			SaglikBirimi_Id:''
		};
	};

	var _PrmBilgisi = function () {
		return {
			PmBilgi: '',
			guidId: '',
			SaglikBirimi_Id: '',
			PeriyodikMuayene: {},
			PeriyodikMuayeneAktarimi: false,
			AccordionDurumu: ''
		};
	};

	var _getTaniDegerlendirmesi = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + '/api/pm/st?val=' + val.substr(0, 8)).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addTaniSablonu = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + '/api/pm/st', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getTaniSablonu = function (val) {
		var deferred = $q.defer();
		$http.get(serviceBase + '/api/pm/std/' + val).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deleteTaniSablonu = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + '/api/pm/std?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPm = function (data, Sb_Id,perguid) {
		var deferred = $q.defer();
		$http.post(serviceBase + '/api/pm/' + Sb_Id+'/'+perguid, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePm = function (data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/pm', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

    var _deletePm = function (key) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/pm/Sil/' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };



	var _getPrmPersonel = function (Guid) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/prm/' + Guid).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrm = function (data, Sb_Id, perguid) {
		var deferred = $q.defer();
		$http.post(serviceBase + '/api/prm/' + Sb_Id + '/' + perguid, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrm = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/prm/update', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

    var _deletePrm = function (key) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/prm/Sil/' + key).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

	var _getIkPersonel = function (Guid) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/Ik/' + Guid).then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getIsKazasininGerceklestigiYer = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/IsKazasininGerceklestigiYer.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getKazayaSebepOlanArac = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/KazayaSebepOlanArac.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getKazayaSebepOlanOlay = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/KazayaSebepOlanOlay.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getGenelFaaliyetleri = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/GenelFaaliyetleri.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getozelFaaliyetleri = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/OzelFaaliyetleri.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getAracFaaliyetleri = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/AracFaaliyetleri.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getYaralanmaOlaylari = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/YaralanmaOlaylari.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getYaralanmaAraclari = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/YaralanmaAraclari.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getYaralanmaTurleri = function () {
			var deferred = $q.defer();
			$http.get('views/SM/View/json/YaralanmaTurleri.json').then(function (response) {
				deferred.resolve(response.data);
			}, function (err, status) {
				deferred.reject(err);
			});
			return deferred.promise;
	};

	var _getYaralanmaYerleri = function () {
				var deferred = $q.defer();
				$http.get('views/SM/View/json/YaralanmaYerleri.json').then(function (response) {
					deferred.resolve(response.data);
				}, function (err, status) {
					deferred.reject(err);
				});
				return deferred.promise;
	};

	var _getCalisilanCevreler = function () {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/CalisilanCevreler.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getIl = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/il-bolge.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _getIlce = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/il-ilce.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _addIk = function (data, Sb_Id, perguid) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + '/api/Ik/' + Sb_Id + '/' + perguid, data).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updateIk = function (data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/Ik/update', data).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
    };

    var _jsonGet = function (veri) {
        var deferred = $q.defer();
        $http.get('views/SM/View/json/'+veri+'.json').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
	};

	var _laboratuarTetkikAra = function (aramaTerimi) {
		var deferred = $q.defer();

		if (typeof aramaTerimi !== 'string') {
			// Arama terimi string değilse, hata döndür veya boş liste döndür
			deferred.reject(new Error("Arama terimi bir string olmalıdır."));
			return deferred.promise;
		}

		$http.get('views/SM/View/json/laboratuarTanimlari.json').then(function (response) {
			var filtrelenmisSonuclar = response.data.filter(function (item) {
				return item.tetkik.toLowerCase().includes(aramaTerimi.toLowerCase());
			});
			//var hg = [];
			//angular.forEach(filtrelenmisSonuclar, function (v) {
			//	if (v.tetkik !== undefined) {
			//		hg.push(v.tetkik);
			//	}
			//});
			deferred.resolve(filtrelenmisSonuclar);
		}, function (err) {
			deferred.reject(err);
		});

		return deferred.promise;
	};

    PmSvcFactory.JsonGetFile = _jsonGet;
	PmSvcFactory.GetPmPersonel = _getPmPersonel;
	PmSvcFactory.pmb = _PmBilgisi;
	PmSvcFactory.prmb = _PrmBilgisi;
	PmSvcFactory.Ilaclar = _getIlac;
	PmSvcFactory.Bolumler = _getBolumler;
	PmSvcFactory.TaniDegerlendirmesi = _getTaniDegerlendirmesi;
	PmSvcFactory.AddTaniSablonu = _addTaniSablonu;
	PmSvcFactory.TaniSablonu = _getTaniSablonu;
	PmSvcFactory.DeleteTaniSablonu = _deleteTaniSablonu;
	PmSvcFactory.AddPm = _addPm;
    PmSvcFactory.UpdatePm = _updatePm;
    PmSvcFactory.DeletePm = _deletePm;
    PmSvcFactory.DeletePrm = _deletePrm;
	PmSvcFactory.GetPrmPersonel = _getPrmPersonel;
	PmSvcFactory.AddPrm = _addPrm;
	PmSvcFactory.UpdatePrm = _updatePrm;
	PmSvcFactory.GetIkPersonel = _getIkPersonel;
	PmSvcFactory.GetIsKazasininGerceklestigiYer = _getIsKazasininGerceklestigiYer;
	PmSvcFactory.GetKazayaSebepOlanArac = _getKazayaSebepOlanArac;
	PmSvcFactory.GetKazayaSebepOlanOlay = _getKazayaSebepOlanOlay;
	PmSvcFactory.GetGenelFaaliyetleri = _getGenelFaaliyetleri;
	PmSvcFactory.GetozelFaaliyetleri = _getozelFaaliyetleri;
	PmSvcFactory.GetAracFaaliyetleri = _getAracFaaliyetleri;
	PmSvcFactory.GetYaralanmaOlaylari = _getYaralanmaOlaylari;
	PmSvcFactory.GetYaralanmaAraclari = _getYaralanmaAraclari;
	PmSvcFactory.GetYaralanmaTurleri = _getYaralanmaTurleri;
	PmSvcFactory.GetYaralanmaYerleri = _getYaralanmaYerleri;
	PmSvcFactory.GetCalisilanCevreler = _getCalisilanCevreler;
	PmSvcFactory.GetIl = _getIl;
	PmSvcFactory.GetIlce = _getIlce;
	PmSvcFactory.AddIk = _addIk;
    PmSvcFactory.UpdateIk = _updateIk;
    PmSvcFactory.GetKubKtAra = _getKubKtAra;
	PmSvcFactory.LaboratuarTetkikAra = _laboratuarTetkikAra;

	return PmSvcFactory;
}
angular
	.module('inspinia')
	.factory('PmSvc', PmSvc);