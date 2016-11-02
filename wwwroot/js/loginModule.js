app.service('LoginService', ['$http', 'AuthenticationToken', function($http, AuthenticationToken) {
    var LOGIN_URL = "http://www.itderrickh.com/users/login/"
    return {
        login: function(credentials, callback) {
            $http.post(LOGIN_URL, credentials).then(function(result) {
                AuthenticationToken.setToken(result.data);
                callback();
            }, function(result) {
                console.log(result);
            });
        }
    }
}]);

app.controller('LoginController', ['$scope', '$location', 'LoginService', function($scope, $location, LoginService) {
    var loginCtrl = this;
    loginForm.email = "";
    loginCtrl.password = "";

    loginCtrl.submit = function() {
        var credentials = {
            "email": loginCtrl.email,
            "password": loginCtrl.password
        };

        LoginService.login(credentials, function() {
            $location.path("main");
        });
    };
}]);