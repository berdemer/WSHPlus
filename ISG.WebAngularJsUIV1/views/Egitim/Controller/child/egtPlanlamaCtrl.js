'use strict';

function egtPlanlamaCtrl($scope, ngAuthSettings,$q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	var EgPC = this;
	if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {

	};
}
egtPlanlamaCtrl.$inject = ['$scope', 'ngAuthSettings','$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('egtPlanlamaCtrl', egtPlanlamaCtrl);