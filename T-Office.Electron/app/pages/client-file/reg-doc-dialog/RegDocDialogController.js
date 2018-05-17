(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('RegDocDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModalInstance', 'ClientsService', 'toastr', 'clientID'];

    function ctrlFn($rootScope, $scope, $location, $uibModalInstance, ClientsService, toastr, clientID) {
        $scope.model = {
            DocumentData: {},
            VehicleData: {}
        };

        $scope.save = function () {
            var modelValidation = validate();
            if (modelValidation.error) {
                toastr.warning(modelValidation.errorMsg);
                return;
            }

            ClientsService.addRegDocData(clientID, $scope.model).then(
                function () {
                    toastr.success('Podaci su uspešno upisani.');
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
            if (
                (!$scope.model.DocumentData.UnambiguousNumber || ($scope.model.DocumentData.UnambiguousNumber && $scope.model.DocumentData.UnambiguousNumber === '')) ||
                (!$scope.model.VehicleData.RegistrationNumber || ($scope.model.VehicleData.RegistrationNumber && $scope.model.VehicleData.RegistrationNumber === ''))
            ) {
                return { error: true, errorMsg: 'Obavezni podaci: registarski broj vozila i broj saobraćajne dozvole.' };
            }

            return { error: false };
        }
    }
})();
