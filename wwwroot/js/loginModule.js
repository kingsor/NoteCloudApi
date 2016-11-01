app.controller('LoginController', ['$scope', function($scope) {
    var loginCtrl = this;
    loginForm.email = "";
    loginCtrl.password = "";

    loginCtrl.submit = function() {
        var credentials = {
            "email": loginForm.email,
            "password": loginCtrl.password
        };

        alert(credentials.email);
    };
}]);