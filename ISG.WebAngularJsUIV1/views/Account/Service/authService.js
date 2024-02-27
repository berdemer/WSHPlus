'use strict';

function authService($http, $q, localStorageService, ngAuthSettings, jwtHelper) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false,
        roles: "",
        id: "",
        token: ""
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _userView = function (stateParamsId) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/accounts/user/' + stateParamsId).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _saveRegistration = function (registration) {
        return $http.post(serviceBase + 'api/accounts/create', registration).then(function (response) {
            return response;
        });
    };

    var _updateRegistration = function (registration) {
        return $http.put(serviceBase + 'api/accounts/update', registration).then(function (response) {
            return response;
        });

    };


    var _deleteRegistration = function (stateParamsId) {
        return $http.delete(serviceBase + 'api/accounts/user/' + stateParamsId).then(function (response) {
            return response;
        });
    };

    var _setLockoutEnabled = function (stateParamsId, isLockoutEnabled) {
        return $http.get(serviceBase + 'api/accounts/Lockout/' + stateParamsId + '/' + isLockoutEnabled).then(function (response) {
            return response;
        });

    };

    var _getAllRoles = function () {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/roles').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _adminUpdatePassword = function (userId, newPassword) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/accounts/aup/' + userId + '/' + newPassword).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _saveRole = function (registration) {
        return $http.post(serviceBase + 'api/roles/create', registration).then(function (response) {
            return response;
        });
    };

    var _deleteRole = function (stateParamsId) {
        return $http.delete(serviceBase + 'api/roles/' + stateParamsId).then(function (response) {
            return response;
        });
    };

    var _fullUsers = function () {
        var deferred = $q.defer();
        $http({
            url: serviceBase + 'api/accounts/users',
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        }).then(function (response) {
            return deferred.resolve(response.data);
        }, function (error) {
            return console.log();
        });
        return deferred.promise;
    };



    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName.trim() + "&password=" + loginData.password.trim();

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'oauth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.data.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true, roles: jwtHelper.decodeToken(response.data.access_token).role, id: jwtHelper.decodeToken(response.data.access_token).nameid });
                localStorageService.set('rolesData', { role: jwtHelper.decodeToken(response.data.access_token).role });
            }
            else {
                localStorageService.set('authorizationData', { token: response.data.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false, roles: jwtHelper.decodeToken(response.data.access_token).role, id: jwtHelper.decodeToken(response.data.access_token).nameid });
                localStorageService.set('rolesData', { role: jwtHelper.decodeToken(response.data.access_token).role });
            }
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            _authentication.useRefreshTokens = loginData.useRefreshTokens;
            _authentication.roles = jwtHelper.decodeToken(response.data.access_token).role;
            _authentication.id = jwtHelper.decodeToken(response.data.access_token).nameid;
            _authentication.token = response.data.access_token;
            deferred.resolve(response);

        }, function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };


    var _logOut = function () {

        localStorageService.remove('authorizationData');
        localStorageService.remove('rolesData');
        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;
        _authentication.roles = "";
        _authentication.id = "";
        _authentication.token = "";
    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
            _authentication.roles = authData.roles;
            _authentication.id = authData.id;
            _authentication.token = authData.token;
        }
    };

    var _assignRolesToUser = function (id, roles) {
        return $http.put(serviceBase + 'api/accounts/user/' + id + '/roles', roles).then(function (response) {
            return response;
        });
    };

    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'oauth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

                    localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: response.data.refresh_token, useRefreshTokens: true, roles: jwtHelper.decodeToken(response.data.access_token).role, id: jwtHelper.decodeToken(response.data.access_token).nameid });
                    localStorageService.set('rolesData', { role: jwtHelper.decodeToken(response.data.access_token).role });
                    deferred.resolve(response);

                }, function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };



    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/accounts/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false, roles: jwtHelper.decodeToken(response.access_token).role, id: jwtHelper.decodeToken(response.access_token).nameid });
            localStorageService.set('rolesData', { role: jwtHelper.decodeToken(response.access_token).role });
            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;
            _authentication.roles = jwtHelper.decodeToken(response.access_token).role;
            _authentication.token = response.access_token;
            _authentication.id = jwtHelper.decodeToken(response.access_token).nameid;
            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/accounts/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.useRefreshTokens = false;
            _authentication.roles = jwtHelper.decodeToken(response.access_token).role;
            _authentication.token = response.access_token;
            _authentication.id = jwtHelper.decodeToken(response.access_token).nameid;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;
    authServiceFactory.adminUpdatePassword = _adminUpdatePassword;
    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;
    authServiceFactory.fullUsers = _fullUsers;
    authServiceFactory.updateRegistration = _updateRegistration;
    authServiceFactory.deleteRegistration = _deleteRegistration;
    authServiceFactory.userView = _userView;
    authServiceFactory.SetLockoutEnabled = _setLockoutEnabled;
    authServiceFactory.GetAllRoles = _getAllRoles;
    authServiceFactory.SaveRole = _saveRole;
    authServiceFactory.DeleteRole = _deleteRole;
    authServiceFactory.AssignRolesToUser = _assignRolesToUser;
    return authServiceFactory;
}


angular
    .module('inspinia')
    .factory('authService', authService);