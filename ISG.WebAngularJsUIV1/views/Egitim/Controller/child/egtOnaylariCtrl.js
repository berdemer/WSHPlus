'use strict';

function egtOnaylariCtrl($scope, ngAuthSettings,$q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	var EgOC = this;
	if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {

	};
}
egtOnaylariCtrl.$inject = ['$scope', 'ngAuthSettings','$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('egtOnaylariCtrl', egtOnaylariCtrl);