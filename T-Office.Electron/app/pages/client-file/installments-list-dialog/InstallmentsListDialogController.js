(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('InstallmentsListDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'ClientsService', 'toastr', 'installments'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, ClientsService, toastr, installments) {
        $scope.installments = installments;

        $scope.resolveStatusCssClass = function (installment) {
            if (installment.IsPaid) {
                return 'label label-success';
            }

            if (!installment.IsPaid) {
                var today = moment(Date.now());
                var installmentDate = moment(installment.InstallmentDate);

                if (installmentDate > today) {
                    return 'label label-info';
                } else {
                    return 'label label-danger';
                }
            }
        };

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };
    }
})();
