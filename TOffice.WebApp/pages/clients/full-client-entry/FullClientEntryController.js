(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('FullClientEntryController', ctrlFn);

    ctrlFn.$inject = ['$scope', '$location', '$uibModal', '$q', 'ClientsService', 'UtilityService', 'RegLicenseReaderService', 'AgentDataService', 'PollingService', 'toastr', 'blockUI'];

    function ctrlFn($scope, $location, $uibModal, $q, ClientsService, UtilityService, RegLicenseReaderService, AgentDataService, PollingService, toastr, blockUI) {
        $scope.context = {
            AlertMessage: '-',
            IsNewClient: false,
            IsDriversLicenceDataPresent: false
        };

        $scope.contextData = {};
        $scope.regLicenceData = {};
        $scope.model = {
            OneTimePayment: false,
            VehicleRegistrationData: {
                RegistrationDateFromPicker: null
            }
        };

        $scope.getDriversLicenceData = function () {
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
                callback: function (result) {
                    if (result) {
                        blockUI.start();

                        var agentId = sessionStorage.getItem('tofficeAgentId');
                        console.log('Agent ID for RegLicenseReader service [FullClientEntryController]: ' + agentId);

                        AgentDataService.get(agentId).then(agentData => {
                            console.log('Agent WS connection ID for RegLicenseReader service [FullClientEntryController]: ' + agentData.data.WsConnectionId);
                            RegLicenseReaderService.readSmartCardData(agentData.data.WsConnectionId).then(
                                function () {
                                    var responseUrl = RegLicenseReaderService.getSmartCardResponseUrl(agentData.data.WsConnectionId);
                                    PollingService.initPolling('smartCardReader_' + Date.now(), responseUrl)
                                        .then(smartCardResponse => {
                                            console.log('%cPolling response successful.', 'color: green;');
                                            console.log(smartCardResponse);

                                            RegLicenseReaderService.deleteSmartCardResponse(agentData.data.WsConnectionId)
                                                .then((/* isDeleted */) => { // isDeleted ? console.log('Smart card response deleted.') : console.log('Smart card response NOT deleted.');
                                                })
                                                .catch(err => {
                                                    console.error('Error deleting smart card response: ' + err.statusText);
                                                });

                                            if (smartCardResponse.IsError) {
                                                toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                                toastr.error(smartCardResponse.ErrorMessage);
                                                blockUI.stop();
                                                return;
                                            } else {
                                                const smartCardData = JSON.parse(smartCardResponse.data.Data);

                                                // used to show reg. license data on screen
                                                const clientData = smartCardData.Result;
                                                ClientsService.eliminateNullStringsFromClientData(clientData);
                                                $scope.regLicenceData = Object.assign({}, clientData);

                                                // used to transfer data model to the backend
                                                const clientDataModel = ClientsService.createInsertClientDataModel(clientData);
                                                $scope.model = Object.assign({}, $scope.model, clientDataModel);

                                                ClientsService.simpleExist(clientDataModel).then(
                                                    result => {
                                                        if (result && result.data) {
                                                            $scope.contextData = Object.assign({}, result.data);
                                                            setContext(result.data);
                                                        }
                                                    },
                                                    error => {
                                                        toastr.error('Došlo je do greške prilikom provere klijenta.');
                                                        console.error(error.statusText);
                                                    }
                                                );

                                                $scope.context.IsDriversLicenceDataPresent = true;

                                                blockUI.stop();
                                            }
                                        })
                                        .catch(errorMsg => {
                                            blockUI.stop();
                                            toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                            console.error(errorMsg);
                                        });
                                },
                                function (error) {
                                    toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                    toastr.error('[GREŠKA] --> ' + error.statusText);
                                    $scope.regLicenceData = {};
                                    $scope.context.IsDriversLicenceDataPresent = false;
                                    blockUI.stop();
                                    return;
                                }
                            );
                        });
                    }
                }
            });
        };

        $scope.chkboxOneTimePaymentClicked = function () {
            if ($scope.model.OneTimePayment === true) {
                $scope.model.VehicleRegistrationData.NumberOfInstallments = 1;
            } else {
                $scope.model.VehicleRegistrationData.NumberOfInstallments = '';
            }
        };

        $scope.save = function () {
            const isValidVehicleRegData = validateVehicleRegData();
            if (!isValidVehicleRegData) {
                toastr.warning('Podaci o novoj registraciji nisu uneti ili nisu validni.');
                return;
            } else {
                // convert reg. date to ISO date string
                $scope.model.VehicleRegistrationData.RegistrationDate = UtilityService.convertDateToISODateString($scope.model.VehicleRegistrationData.RegistrationDateFromPicker);

                console.log('Adding new registration [model]');
                console.log($scope.model);

                ClientsService.fullClientDataEntry($scope.model).then(
                    function () {
                        if ($scope.context.IsNewClient) {
                            toastr.success('Novi klijent uspešno upisan.');
                        } else if (!$scope.contextData.IsExistingVehicle) {
                            toastr.success('Podaci za novo vozilo uspešno upisani.');
                        } else {
                            toastr.success('Podaci za vozilo uspešno ažurirani.');
                        }
                    },
                    function (error) {
                        toastr.error('Došlo je do greške prilikom snimanja podataka.');
                        toastr.error('[GREŠKA] --> ' + error.statusText);
                        console.log('Full client entry error:');
                        console.log(error);
                        return false;
                    }
                );
            }
        };

        function validateVehicleRegData() {
            return (
                (Boolean($scope.model.VehicleRegistrationData.RegistrationDateFromPicker) && $scope.model.VehicleRegistrationData.RegistrationDateFromPicker !== '') &&
                (Boolean($scope.model.VehicleRegistrationData.TotalAmount) && $scope.model.VehicleRegistrationData.TotalAmount !== '' && _.isNumber($scope.model.VehicleRegistrationData.TotalAmount)) &&
                (Boolean($scope.model.VehicleRegistrationData.NumberOfInstallments) && $scope.model.VehicleRegistrationData.NumberOfInstallments !== '' && _.isInteger($scope.model.VehicleRegistrationData.NumberOfInstallments))
            );
        };

        function setContext(contextData) {
            $scope.context.IsNewClient = !contextData.IsExistingOwner && !contextData.IsExistingUser;

            $scope.context.AlertMessage = '';
            if (contextData.IsExistingOwner) {
                $scope.context.AlertMessage += 'Postojeći vlasnik | ';
            } else {
                $scope.context.AlertMessage += 'Novi vlasnik | ';
            }

            if (contextData.IsExistingUser) {
                $scope.context.AlertMessage += 'Postojeći korisnik | ';
            } else {
                $scope.context.AlertMessage += 'Novi korisnik | ';
            }

            if (contextData.IsExistingVehicle) {
                $scope.context.AlertMessage += 'Postojeće vozilo';
            } else {
                $scope.context.AlertMessage += 'Novo vozilo';
            }
        }

        // date picker support
        $scope.datePickers = {};
        $scope.openDatePicker = function (pickerFor, event) {
            event.preventDefault();
            event.stopPropagation();

            $scope.datePickers['datePickerOpened_' + pickerFor] = true;
        };
    }
})();
