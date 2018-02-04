(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientFileController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal /* , AuthenticationService */) {
        //var currentUser = AuthenticationService.getCurrentUser();
    }
})();
