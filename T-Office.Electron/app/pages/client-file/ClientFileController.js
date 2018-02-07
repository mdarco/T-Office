(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientFileController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'toastr', 'client' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, toastr, client /* , AuthenticationService */) {
        //var currentUser = AuthenticationService.getCurrentUser();

        $scope.client = client;

        alert(JSON.stringify(client, null, 2));
    }
})();
