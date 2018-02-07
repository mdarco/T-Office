(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientFileController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'toastr', 'client' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, toastr, client /* , AuthenticationService */) {
        //var currentUser = AuthenticationService.getCurrentUser();

        $scope.client = client;

        calculateVehicleNextRegDate();

        function calculateVehicleNextRegDate() {
            var vehicles = $scope.client.Vehicles;
            if (vehicles.length > 0) {
                _.each(vehicles, function (vehicle) {
                    if (vehicle.FirstRegistrationDate) {
                        var diff = (new Date()).getFullYear() - moment(vehicle.FirstRegistrationDate).year();
                        var date_FirstReg = moment(vehicle.FirstRegistrationDate);
                        vehicle.NextRegistrationDate = (date_FirstReg.add(diff, 'y')).toDate();
                    }
                });
            }
        }
    }
})();
