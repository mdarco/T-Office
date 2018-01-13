(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('NavigationController', navigation);

    navigation.$inject = ['$rootScope', '$scope', '$location', /* 'AuthenticationService', */ 'toastr', 'AppParams'];

    function navigation($rootScope, $scope, $location, /* AuthenticationService, */ toastr, AppParams) {
        
    }
})();
