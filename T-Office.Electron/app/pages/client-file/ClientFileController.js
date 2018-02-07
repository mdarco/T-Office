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

        $scope.editClient = function (dataField) {
            openTextFieldDialog($scope.client[dataField]).then(
                function (result) {
                    var editObj = {};
                    editObj[dataField] = result;

                    ClientsService.editClient($scope.client.ID, editObj).then(
                        function () {
                            toastr.success('Podatak uspešno ažuriran.');
                            $scope.client[dataField] = result;
                        },
                        function (error) {
                            toastr.error(error.statusText);
                        }
                    );
                }
            );
        };

        function openTextFieldDialog(text) {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/common/text-field-dialog/text-field-dialog.html',
                controller: 'TextFieldDialogController',
                resolve: {
                    settings: function () {
                        return {
                            FieldValue: text
                        };
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            return dialog.result;
        }

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
