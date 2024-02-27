/**
 * INSPINIA - Responsive Admin Theme
 *
 */

/**
 * MainCtrl - controller
 */
function MainCtrl($scope, $location, authService, ngAuthSettings) {


	$scope.logOut = function () {
		authService.logOut();
		$location.path('/login');
	}


	authService.fillAuthData();
	$scope.authentication = authService.authentication;//authentication olan yerleri göstermek için yapýldý.


	var app = angular.module('formlyExample', ['formly', 'formlyBootstrap']);

	app.run(function (formlyConfig) {
		// Put types in here.
	});

		var vm = this;
		// funcation assignment
		vm.onSubmit = onSubmit;



		vm.model = {text2:true};
		vm.options = {};

		vm.fields = [
		  {
			  key: 'text',
			  type: 'input',
			  templateOptions: {
				  label: 'Input',
				  placeholder: 'Formly is terrific!'
			  }
		  },
		  {
			  key: 'text2',
			  type: 'checkbox',
			  templateOptions: {
				  label: 'Input2',
				  placeholder: 'Formly is terrific!'
			  }
		  }
		];
	$scope.birimler = [{ SirketAdi: "test", SaglikBirimiAdi: "test", SaglikBirimi_Id: 0 },
		{ SirketAdi: "test", SaglikBirimiAdi: "test", SaglikBirimi_Id: 0 }, { SirketAdi: "test", SaglikBirimiAdi: "test", SaglikBirimi_Id: 0 }];
	$scope.nbv = { SirketAdi: "test", SaglikBirimiAdi: "test", SaglikBirimi_Id: 0 };
		vm.originalFields = angular.copy(vm.fields);

		// function definition
		function onSubmit() {
			vm.options.updateInitialValue();
			alert(JSON.stringify(vm.model), null, 2);
		}
};

MainCtrl.$inject = ['$scope', '$location', 'authService', 'ngAuthSettings'];

angular
	.module('inspinia')
	.controller('MainCtrl', MainCtrl)