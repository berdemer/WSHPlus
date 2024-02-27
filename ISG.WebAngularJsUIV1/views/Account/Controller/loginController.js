'use strict';

 function loginController ($scope, $location, authService, ngAuthSettings) {

     var fileImgPath = function () {
         return ngAuthSettings.storageLinkService + ngAuthSettings.uploadFolder + 'De_Morbis_Artificum.png';
     };

     $scope.fileImgPath = fileImgPath();

     var fileImgPath2 = function () {
         return ngAuthSettings.storageLinkService + ngAuthSettings.uploadFolder + 'wch.png';
     };

     $scope.fileImgPath2 = fileImgPath2();

     $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/sm/SMCards/');

        },
         function (err) {
             $scope.message = err.data.error_description;
         });
    };

    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount === 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/sm/SMCards/');

            }
            else {
                //Obtain access token and redirect to orders
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/sm/SMCards/');

                },
             function (err) {
                 $scope.message = err.error_description;
             });
            }

        });
    };
 }


loginController.$inject = ['$scope', '$location', 'authService', 'ngAuthSettings'];

angular
    .module('inspinia')
    .controller('loginController', loginController);