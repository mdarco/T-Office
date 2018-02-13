(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('RegDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'client', 'vehicle'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, client, vehicle) {
        $scope.client = client;
        $scope.vehicle = vehicle;
    }
})();
