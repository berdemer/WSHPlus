'use strict';

function egtSertifikasyonCtrl($scope, ngAuthSettings,$q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	var EgSC = this;
	if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {

	};
}
egtSertifikasyonCtrl.$inject = ['$scope', 'ngAuthSettings','$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('egtSertifikasyonCtrl', egtSertifikasyonCtrl);