'use strict';

angular.module('ngApp')

  .factory('AuthenticationService', ['$rootScope', '$cookieStore', 'Restangular', '$http', '$q', '$location',

    function ($rootScope, $cookieStore, Restangular, $http, $q, $location) {

      var UserModel = {
        // Set default user state
        userName: null,
        isLoggedIn: false,
        access: 1,
        token: null
      };

      // The getToken call will initialize a token request from the oAuth service.
      // This will be done through the $http service rather than Restangular due
      // to some restrictions on the data format and that the token does not follow
      // the rest of the API's URL format which is embedded to Restangular during
      // application configuration. We also use jQuery's $.param here which is okay
      // since we need jQuery anyway.
      //
      // For a non-jQuery solution, see below link: 
      // http://victorblog.com/2012/12/20/make-angularjs-http-service-behave-like-jquery-ajax/

      var getToken = function (userName, password) {

          // Parametrize params for the token endpoint
          var params = $.param({
              userName: userName,
              password: password,
              grant_type: 'password'
          });

          // Send the $http request and return the promise
          return $http.post('/token', params, {
              headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
          });

      };

        // Return the service
	    return {

		signIn: function(userName, password) {

		        var dfd = $q.defer();

		        // Start the ajax loader
		        $rootScope.ajaxLoading = true;

                // Request a token with username & password
		        getToken(userName, password).then(function (response) {
		            
                    // Set new user model
		            UserModel = {
		                userName   : response.data.userName,
		                isLoggedIn : true,
		                access     : parseInt(response.data.access),
		                token      : 'Bearer ' + response.data.access_token
		            };

		            // Set user cookie
		            $cookieStore.put('user', UserModel);

		            // Set default headers
		            Restangular.setDefaultHeaders({ Authorization: $cookieStore.get('user').token });

                // Resolve the promise
		            dfd.resolve();

                // If request for token fails, reject the promise
		        }, function () { dfd.reject(); });

		        return dfd.promise;
		    },

		    signOut: function() {
			    
		        var dfd = $q.defer();

		        // Call api
		        Restangular.one('accounts/logout').post().then(function () {
		            
		            // Remove the cookie
		            $cookieStore.remove('user');

		            // Set the UserModel back to defaults
		            UserModel = {
		                userName   : null,
		                isLoggedIn : false,
		                access     : 1,
		                token      : null
		            };

                // Resolve the promise
		            dfd.resolve();

		        }, function () { dfd.reject(); });

		        return dfd.promise;
		    },

		    updateUserState: function () {

		        var dfd  = $q.defer(),
		            user = $cookieStore.get('user');

		        // Check user cookie
		        if (user !== undefined && user.token !== undefined) {

		            // While defining token, we'll temporarily log in user and set the cookie values
		            // These will be overriden later -> We dont want the user to have to wait ro be
		            // redirected while we determine if they're legit. We'll just move them
		            // away later if they're not
		            UserModel = user;

		            // Set default headers
		            Restangular.setDefaultHeaders({ Authorization: user.token });

		            // Get user details
		            Restangular.one('accounts', 'user-info').get().then(function (response) {

		                // Update user model
		                UserModel = {
		                    userName   : response.userName,
		                    isLoggedIn : true,
		                    access     : parseInt(response.access),
		                    token      : user.token
		                };

		                dfd.resolve();

		            }, function () {

		                // If authentication fails, move the user back to the front immediately
		                $location.path('/sign-in').replace();

                        // Reject promise
		                dfd.reject();
		            });

		        } else {

                    // If user cookie is undefined or the token is not set, reject the promise
		            dfd.reject();

		        }

		        return dfd.promise;
		    },

		    isLoggedIn: function() { return UserModel.isLoggedIn; },
		    get: function () { return UserModel; }

	    };

}]);