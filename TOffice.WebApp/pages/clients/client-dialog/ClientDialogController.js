(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'ClientsService', 'toastr'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, ClientsService, toastr) {
        $scope.model = {};

        $scope.save = function () {
            var modelValidation = validate();
            if (modelValidation.error) {
                toastr.warning(modelValidation.errorMsg);
                return;
            }

            ClientsService.addClient($scope.model).then(
                function () {
                    toastr.success('Klijent je uspešno upisan.');
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
            if (!$scope.model.OwnerSurnameOrBusinessName || ($scope.model.OwnerSurnameOrBusinessName && $scope.model.OwnerSurnameOrBusinessName === '')) {
                return { error: true, errorMsg: 'Prezime vlasnika ili naziv firme je obavezan podatak.' };
            }

            return { error: false };
        }
    }
})();
