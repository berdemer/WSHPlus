'use strict';

function TkSvc($http, $q, ngAuthSettings) {
	var TkSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _getTkPersonel = function (Guid) {
		var deferred = $q.defer();
		$http.get(serviceBase + 'api/tk/GuidId/' + Guid).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _TkBilgisi = function () {
		return {
			TkBilgi: '',
			guidId: '',
			SaglikBirimi_Id: null,
			MuayeneTuru: null,
			MuayeneTurleri: null
		};
	};

	var _addPrsANT = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/ant/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsANT = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/ant?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsANT = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/ant?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsBoyKilo = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/boyKilo/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsBoyKilo = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/boyKilo?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsBoyKilo = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/boyKilo?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsSFT = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/sft/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsSFT = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/sft?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsSFT = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/sft?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsOdio = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/odio/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsOdio = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/odio?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsOdio = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/odio?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsLaboratuar = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/laboratuar/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsLaboratuar = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/laboratuar?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsLaboratuar = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/laboratuar?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var apiPath = function () {
		return ngAuthSettings.isAzure ? serviceBase + 'api/laboratuar/azurtanimi':
            serviceBase + 'api/laboratuar/Tanimlari';
	};

	var _tetkikTanimlari = function (value) {
		var deferred = $q.defer();
        $http.get('views/SM/View/json/laboratuarTanimlari.json').then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsHemogram = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/hemogram/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsHemogram = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/hemogram?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsHemogram = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/hemogram?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsRadyoloji = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/radyoloji/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsRadyoloji = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/radyoloji?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsRadyoloji = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/radyoloji?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var apiPathRadyoloji = function () {
	    return ngAuthSettings.isAzure ? serviceBase + 'api/radyoloji/azurtanimi' :
			serviceBase + 'api/radyoloji/Tanimlari';
	};



	var _addPrsEKG = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/ekg/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsEKG = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/ekg?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsEKG = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/ekg?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsGorme = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/gorme/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsGorme = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/gorme?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsGorme = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/gorme?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsPansuman = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/pansuman/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsPansuman = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/pansuman?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsPansuman = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/pansuman?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsRevirTedavi = function (data, Sb_Id, prt) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/revirTedavi/' + Sb_Id + '/' + prt, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsRevirTedavi = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/revirTedavi?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsRevirTedavi = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/revirTedavi?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsIlacSarfCikisi = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + 'api/revirTedavi/sarf', data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _updatePrsIlacSarfCikisi = function (key, data) {
		var deferred = $q.defer();
		$http.put(serviceBase + 'api/revirTedavi/sarf?key=' + key, data).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _deletePrsIlacSarfCikisi = function (key) {
		var deferred = $q.defer();
		$http.delete(serviceBase + 'api/revirTedavi/sarf?id=' + key).then(function (response) {
			deferred.resolve(response.data);
		},function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _addPrsPsikolojikTest = function (data, Sb_Id, prt) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/psikolojiktest/' + Sb_Id + '/' + prt, data).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _updatePrsPsikolojikTest = function (key, data) {
	    var deferred = $q.defer();
	    $http.post(serviceBase + 'api/psikolojiktest/Putin/' + key, data).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _deletePrsPsikolojikTest = function (key) {
	    var deferred = $q.defer();
	    $http.delete(serviceBase + 'api/psikolojiktest?id=' + key).then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _beckAnksiyeteOlcegi = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/BeckAnksiyeteOlcegi.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _beckDepresyonOlcegi = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/BeckDepresyonOlcegi.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _dikkatDaginikligiTesti = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/DikkatDaginikligiTesti.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};

	var _maslachTukenmislikOlcegi = function () {
	    var deferred = $q.defer();
	    $http.get('views/SM/View/json/MaslachTukenmislikOlcegi.json').then(function (response) {
	        deferred.resolve(response.data);
	    }, function (err, status) {
	        deferred.reject(err);
	    });
	    return deferred.promise;
	};
	var _radyolojiTanimlari = function (value) {
		var deferred = $q.defer();
		$http.get('views/SM/View/json/radyoloji.json').then(function (response) {
			deferred.resolve(response.data);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	TkSvcFactory.GetTkPersonel = _getTkPersonel;
	TkSvcFactory.Tk = _TkBilgisi;
	TkSvcFactory.AddPrsANT = _addPrsANT;
	TkSvcFactory.UpdatePrsANT = _updatePrsANT;
	TkSvcFactory.DeletePrsANT = _deletePrsANT;
	TkSvcFactory.AddPrsBoyKilo = _addPrsBoyKilo;
	TkSvcFactory.UpdatePrsBoyKilo = _updatePrsBoyKilo;
	TkSvcFactory.DeletePrsBoyKilo = _deletePrsBoyKilo;
	TkSvcFactory.AddPrsSFT = _addPrsSFT;
	TkSvcFactory.UpdatePrsSFT = _updatePrsSFT;
	TkSvcFactory.DeletePrsSFT = _deletePrsSFT;
	TkSvcFactory.AddPrsOdio = _addPrsOdio;
	TkSvcFactory.UpdatePrsOdio = _updatePrsOdio;
	TkSvcFactory.DeletePrsOdio = _deletePrsOdio;
	TkSvcFactory.AddPrsLaboratuar = _addPrsLaboratuar;
	TkSvcFactory.UpdatePrsLaboratuar = _updatePrsLaboratuar;
	TkSvcFactory.DeletePrsLaboratuar = _deletePrsLaboratuar;
	TkSvcFactory.TetkikTanimlari = _tetkikTanimlari;
	TkSvcFactory.AddPrsHemogram = _addPrsHemogram;
	TkSvcFactory.UpdatePrsHemogram = _updatePrsHemogram;
	TkSvcFactory.DeletePrsHemogram = _deletePrsHemogram;
	TkSvcFactory.AddPrsRadyoloji = _addPrsRadyoloji;
	TkSvcFactory.UpdatePrsRadyoloji = _updatePrsRadyoloji;
	TkSvcFactory.DeletePrsRadyoloji = _deletePrsRadyoloji;
	TkSvcFactory.RadyolojiTanimlari = _radyolojiTanimlari;
	TkSvcFactory.AddPrsEKG = _addPrsEKG;
	TkSvcFactory.UpdatePrsEKG = _updatePrsEKG;
	TkSvcFactory.DeletePrsEKG = _deletePrsEKG;
	TkSvcFactory.AddPrsGorme = _addPrsGorme;
	TkSvcFactory.UpdatePrsGorme = _updatePrsGorme;
	TkSvcFactory.DeletePrsGorme = _deletePrsGorme;
	TkSvcFactory.AddPrsPansuman = _addPrsPansuman;
	TkSvcFactory.UpdatePrsPansuman = _updatePrsPansuman;
	TkSvcFactory.DeletePrsPansuman = _deletePrsPansuman;
	TkSvcFactory.AddPrsRevirTedavi = _addPrsRevirTedavi;
	TkSvcFactory.UpdatePrsRevirTedavi = _updatePrsRevirTedavi;
	TkSvcFactory.DeletePrsRevirTedavi = _deletePrsRevirTedavi;
	TkSvcFactory.AddPrsIlacSarfCikisi = _addPrsIlacSarfCikisi;
	TkSvcFactory.UpdatePrsIlacSarfCikisi = _updatePrsIlacSarfCikisi;
	TkSvcFactory.DeletePrsIlacSarfCikisi = _deletePrsIlacSarfCikisi;
	TkSvcFactory.AddPrsPsikolojikTest = _addPrsPsikolojikTest;
	TkSvcFactory.UpdatePrsPsikolojikTest = _updatePrsPsikolojikTest;
	TkSvcFactory.DeletePrsPsikolojikTest = _deletePrsPsikolojikTest;
	TkSvcFactory.MaslachTukenmislikOlcegi = _maslachTukenmislikOlcegi;
	TkSvcFactory.DikkatDaginikligiTesti = _dikkatDaginikligiTesti;
	TkSvcFactory.BeckDepresyonOlcegi = _beckDepresyonOlcegi;
	TkSvcFactory.BeckAnksiyeteOlcegi = _beckAnksiyeteOlcegi;

	return TkSvcFactory;
}
angular
	.module('inspinia')
	.factory('TkSvc', TkSvc);