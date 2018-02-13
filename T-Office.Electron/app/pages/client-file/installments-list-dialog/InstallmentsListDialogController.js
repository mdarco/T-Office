(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('InstallmentsListDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'ClientsService', 'toastr', 'installments'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, ClientsService, toastr, installments) {
        $scope.installments = installments;

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };
    }
})();
