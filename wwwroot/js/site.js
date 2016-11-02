var app = angular.module('noteCloud', ['ngRoute']);

app.service('APIInterceptor', ['$rootScope', 'AuthenticationToken', '$location', function($rootScope, AuthenticationToken, $location) {
    var service = this;
    service.request = function(config) {
        var access_token = AuthenticationToken.getToken();
        if (access_token != null && access_token != "") {
            config.headers.Authorize = access_token;
        }

        return config;
    };
    service.responseError = function(response) {
        if (response.status === 401) {
            $rootScope.$broadcast('unauthorized');
        }
        return response;
    };
}])

app.config(['$routeProvider', '$httpProvider', function($routeProvider, $httpProvider) {
    $routeProvider
    .when("/", {
        templateUrl: "/views/login.html",
        controller: "LoginController",
        controllerAs: "loginCtrl"
    })
    .when('/main', {
        templateUrl: "/views/main.html",
        controller: "MainController",
        controllerAs: "mainCtrl"
    });

    $httpProvider.interceptors.push('APIInterceptor');
}]);

app.factory('AuthenticationToken', ['$window', function($window) {
    return {
        getToken: function() {
            return $window.localStorage['authToken'];
        },
        setToken: function(token) {
            $window.localStorage['authToken'] = token;
        },
        destroyToken: function() {
            $window.localStorage['authToken'] = null;
        }
    };
}]);