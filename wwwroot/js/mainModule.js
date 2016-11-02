app.service('NoteGroupService', ['$http', 'AuthenticationToken',  function($http, AuthenticationToken) {
    var GET_GROUPS_URL = "http://www.itderrickh.com/notegroups/user/";
    return {
        getAllNoteGroups: function(callback) {
            var currentUser = AuthenticationToken.getCurrentUser();
            $http({
                url: GET_GROUPS_URL + currentUser.userId,
                method: 'GET',
                withCredentials: true,
                headers: {
                    'Authorize': AuthenticationToken.getToken(),
                }
            }).then(function(result) {
                callback(result.data);
            });
        },
        addNoteGroup: function() {

        }
    }
}]);

app.controller('MainController', ['$scope', 'NoteGroupService', function($scope, NoteGroupService) {
    var mainCtrl = this;
    mainCtrl.noteGroups = [];
    
    NoteGroupService.getAllNoteGroups(function(data) {
        mainCtrl.noteGroups = data;
    });
}]);