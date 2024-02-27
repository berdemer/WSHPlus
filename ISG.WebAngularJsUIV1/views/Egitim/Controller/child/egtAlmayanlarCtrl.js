'use strict';

function egtAlmayanlarCtrl($scope, ngAuthSettings,$q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	var EgAmC = this;
	if ((!angular.isUndefined($stateParams.id)) && (!angular.isUndefined($stateParams.year))) {

	};
}
egtAlmayanlarCtrl.$inject = ['$scope', 'ngAuthSettings','$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('egtAlmayanlarCtrl', egtAlmayanlarCtrl);