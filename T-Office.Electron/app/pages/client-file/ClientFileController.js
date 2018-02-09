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
            openTextFieldDialog(dataField, $scope.client[dataField]).then(
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
                },
                function (error) {}
            );
        };

        $scope.addVehicle = function () {
            bootbox.confirm({
                message: "<span style='font-weight: bold; font-size: large;'>Stavite saobraćajnu dozvolu u čitač.</span>",
                buttons: {
                    confirm: {
                        label: 'Ok',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    }
                },
                callback: function (dialogResult) {
                    if (dialogResult) {
                        RegLicenseReaderService.readData().then(
                            function (result) {
                                if (result && result.data) {
                                    var data = JSON.parse(result.data);
                                    if (data.IsError) {
                                        //toastr.error('[GREŠKA] --> ' + data.ErrorMessage);
                                        toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                        return;
                                    } else {
                                        var licenseData = data.Result;
                                        // TODO: check if the reg license belongs to the correct client
                                    }
                                } else {
                                    toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                    return;
                                }
                            },
                            function (error) {
                                toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                toastr.error('[GREŠKA] --> ' + error.statusText);
                                return;
                            }
                        );
                    }
                }
            });
        };

        function openTextFieldDialog(dataField, text) {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/common/text-field-dialog/text-field-dialog.html',
                controller: 'TextFieldDialogController',
                resolve: {
                    settings: function () {
                        return {
                            FieldValue: text,
                            DisplayTitle: 'T-Office',
                            FieldLabel: resolveDataFieldLabel(dataField)
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

        function resolveDataFieldLabel(dataField) {
            switch (dataField) {
                case 'OwnerPersonalNo':
                case 'UserPersonalNo':
                    return 'JMBG/MB';

                case 'OwnerPIB':
                case 'UserPIB':
                    return 'PIB';

                case 'OwnerName':
                case 'UserName':
                    return 'Ime';

                case 'OwnerSurnameOrBusinessName':
                case 'UserSurnameOrBusinessName':
                    return 'Prezime/Naziv firme';

                case 'OwnerAddress':
                case 'UserAddress':
                    return 'Adresa';

                case 'OwnerPhone':
                case 'UserPhone':
                    return 'Telefon';

                case 'OwnerEmail':
                case 'UserEmail':
                    return 'E-mail';

                case 'RecommendedBy':
                    return 'Preporuka';

                default:
                    return '';
            }
        }
    }
})();
