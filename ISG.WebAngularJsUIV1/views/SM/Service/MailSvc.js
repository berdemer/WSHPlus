'use strict';

function mailSvc($rootScope, $compile, $http, $timeout, $q, ngAuthSettings) {
	var mailSvcFactory = {};
	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var _addIk = function (data) {
		var deferred = $q.defer();
		$http.post(serviceBase + '/api/mail', data).then(function (response) {
			deferred.resolve(response);
		}, function (err, status) {
			deferred.reject(err);
		});
		return deferred.promise;
	};

	var _getMail = function (templateUrl, data, mail) {
	    $rootScope.isBeingMailed = true;
		$rootScope.isBeingMailed2 = true;
		$rootScope.mailGonerildiBilgisi = false;
		$http.get(templateUrl).then(function (templateData) {
			var template = templateData.data;
			var mailScope = $rootScope.$new();
			angular.extend(mailScope, data);
			var element = $compile($('<div style="max-height:500px">' + template + '</div>'))(mailScope);
			var renderAndPrintPromise = $q.defer();
			var htmlOlusmasiniBekle = function () {
				if (mailScope.$$phase) {
					$timeout(htmlOlusmasiniBekle);
				} else {
					mail.Body = "<!doctype html>" +
										  "<html>" +
											  '<body>' +
												  element.html() +
											  '</body>' +
										  "</html>";

					_addIk(mail).then(function (bilgi) {
						console.log(bilgi);
						$rootScope.mailGonerildiBilgisi = bilgi.data;
					    renderAndPrintPromise.resolve(true);
					    $rootScope.isBeingMailed = false;
					    $rootScope.isBeingMailed2 = false;
					});
					
					mailScope.$destroy();
					
				}
				return renderAndPrintPromise.promise;
			};
			htmlOlusmasiniBekle();
		});
    };

    var _postSMS = function (data) {
        //$rootScope.smsGonderimi = true;
        var deferred = $q.defer();
        $http.post(serviceBase + '/api/sms', data).then(function (response) {
            //$rootScope.smsGonderimi = false;
            deferred.resolve(response);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getRaporSMS = function (val) {
        var deferred = $q.defer();
        $http.get(serviceBase + '/api/sms?msj=' + val).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };


    mailSvcFactory.GetMail = _getMail;
    mailSvcFactory.PostSMS = _postSMS;
	mailSvcFactory.GetRaporSMS = _getRaporSMS;
	mailSvcFactory.GetHTMLMail = _addIk;

	return mailSvcFactory;
}

angular
	.module('inspinia')
	.factory('mailSvc', mailSvc);