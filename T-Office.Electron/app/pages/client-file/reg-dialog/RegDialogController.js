(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('RegDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'ClientsService', 'toastr', 'client', 'vehicle'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, ClientsService, toastr, client, vehicle) {
        $scope.client = client;
        $scope.vehicle = vehicle;
        $scope.model = {};

        $scope.save = function () {
            var modelValidation = validate();
            if (modelValidation.error) {
                toastr.warning(modelValidation.errorMsg);
                return;
            }

            ClientsService.addVehicleRegistration(client.ID, vehicle.ID, $scope.model).then(
                function () {
                    toastr.success('Registracija je uspešno upisana.');
                    $uibModalInstance.close();
                },
                function (error) {
                    toastr.error(error.statusText);
                }
            );
        };

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };

        function validate() {
            if (!$scope.model.TotalAmount || ($scope.model.TotalAmount && $scope.model.TotalAmount === '') ||
                !$scope.model.NumberOfInstallments || ($scope.model.NumberOfInstallments && $scope.model.NumberOfInstallments === '')) {
                return { error: true, errorMsg: 'Nisu popunjena sva obavezna polja.' };
            }

            return { error: false };
        }

        $scope.chkboxOneTimePaymentClicked = function () {
            if ($scope.model.OneTimePayment === true) {
                $scope.model.NumberOfInstallments = 1;
            } else {
                $scope.model.NumberOfInstallments = '';
            }
        };
    }
})();
