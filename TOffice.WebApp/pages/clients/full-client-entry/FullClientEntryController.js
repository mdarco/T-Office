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

        $scope.regLicenceData = {};
        $scope.model = {
            VehicleReg: {
                OneTimePayment: false
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

                                            // use smart card data to insert new client
                                            if (smartCardResponse.IsError) {
                                                toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                                toastr.error(smartCardResponse.ErrorMessage);
                                                blockUI.stop();
                                                return;
                                            } else {
                                                console.log('Trying to insert new client..');
                                                const smartCardData = JSON.parse(smartCardResponse.data.Data);
                                                const clientData = smartCardData.Result;

                                                ClientsService.eliminateNullStringsFromClientData(clientData);

                                                var existModel = {
                                                    PersonalData: {
                                                        OwnerPersonalNo: '',
                                                        OwnerName: '',
                                                        OwnerSurnameOrBusinessName: '',
                                                        UserPersonalNo: '',
                                                        UserName: '',
                                                        UserSurnameOrBusinessName: ''
                                                    },
                                                    VehicleData: {
                                                        RegistrationNumber: ''
                                                    }
                                                };

                                                ClientsService.simpleExist(existModel).then(
                                                    result => {
                                                        console.log('Simple exist result:');
                                                        console.log(result);

                                                        if (result && result.data) {
                                                            setContext(result.data);
                                                        }
                                                    },
                                                    error => {
                                                        toastr.error('Došlo je do greške prilikom provere klijenta.');
                                                        console.error(error.statusText);
                                                    }
                                                );

                                                $scope.regLicenceData = clientData;
                                                $scope.context.IsDriversLicenceDataPresent = true;

                                                blockUI.stop();
                                            }
                                        })
                                        .catch(errorMsg => {
                                            console.error(errorMsg);
                                            blockUI.stop();
                                        });
                                },
                                function (error) {
                                    toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                    toastr.error('[GREŠKA] --> ' + error.statusText);
                                    $scope.regLicenceData = {};
                                    $scope.context.IsDriversLicenceDataPresent = false;
                                    return;
                                }
                            );
                        });
                    }
                }
            });
        };

        $scope.chkboxOneTimePaymentClicked = function () {
            if ($scope.model.VehicleReg.OneTimePayment === true) {
                $scope.model.VehicleReg.NumberOfInstallments = 1;
            } else {
                $scope.model.VehicleReg.NumberOfInstallments = '';
            }
        };

        //#region InsertClient

        //function insertClient(data, existingClients) {
        //    var dialogHtml = `
        //        <table class="table table-condensed table-striped">
        //            <tbody>
        //                <tr>
        //                    <td style="color: blue;">Vlasnik</td>
        //                    <td>${data.PersonalData.ownersName || ''} ${data.PersonalData.ownersSurnameOrBusinessName || ''}, ${data.PersonalData.ownerAddress || ''}</td>
        //                </tr>
        //                <tr>
        //                    <td style="color: blue;">Korisnik</td>
        //                    <td>${data.PersonalData.usersName || ''} ${data.PersonalData.usersSurnameOrBusinessName || ''}, ${data.PersonalData.usersAddress || ''}</td>
        //                </tr>
        //                <tr>
        //                    <td style="color: blue;">Vozilo</td>
        //                    <td>${data.VehicleData.vehicleMake || ''} ${data.VehicleData.commercialDescription || ''} (${data.VehicleData.registrationNumberOfVehicle || ''})</td>
        //                </tr>
        //            </tbody>
        //        </table>

        //        <br /><br />

        //        <table class="table table-condensed table-striped">
        //            <caption>Postojeći klijenti</caption>
        //            <tbody>
        //    `;

        //    _.each(existingClients, function (existingClient) {
        //        dialogHtml += '<tr>';
        //        dialogHtml += '<td>Vlasnik: [' + existingClient.OwnerJMBGMB + '] ' + existingClient.FullOwnerName + '(' + existingClient.OwnerAddress + ')<br />';
        //        dialogHtml += 'Korisnik: [' + existingClient.UserJMBGMB + '] ' + existingClient.FullUserName + '(' + existingClient.UserAddress + ')</td>';
        //        dialogHtml += '<td><a href="#/client-file/' + existingClient.ID + '" class="btn btn-xs btn-primary" onclick="bootbox.hideAll()">Detalji</></td>';
        //        dialogHtml += '</tr>';
        //    });

        //    dialogHtml += `
        //            </tbody>
        //        </table>
        //    `;

        //    var model = {
        //        DocumentData: {
        //            IssuingState: data.DocumentData.stateIssuing.replace(/\0/g, ''),
        //            CompetentAuthority: data.DocumentData.competentAuthority.replace(/\0/g, ''),
        //            IssuingAuthority: data.DocumentData.authorityIssuing.replace(/\0/g, ''),
        //            UnambiguousNumber: data.DocumentData.unambiguousNumber.replace(/\0/g, ''),
        //            IssuingDate: UtilityService.convertSerbianDateStringToISODateString(data.DocumentData.issuingDate.replace(/\0/g, '')),
        //            ExpiryDate: UtilityService.convertSerbianDateStringToISODateString(data.DocumentData.expiryDate.replace(/\0/g, '')),
        //            SerialNumber: data.DocumentData.serialNumber.replace(/\0/g, '')
        //        },

        //        VehicleData: {
        //            RegistrationNumber: data.VehicleData.registrationNumberOfVehicle.replace(/\0/g, ''),
        //            FirstRegistrationDate: UtilityService.convertSerbianDateStringToISODateString(data.VehicleData.dateOfFirstRegistration.replace(/\0/g, '')),
        //            ProductionYear: data.VehicleData.yearOfProduction.replace(/\0/g, ''),
        //            Make: data.VehicleData.vehicleMake.replace(/\0/g, ''),
        //            Model: data.VehicleData.commercialDescription.replace(/\0/g, ''),
        //            Type: data.VehicleData.vehicleType.replace(/\0/g, ''),
        //            EnginePowerKW: data.VehicleData.maximumNetPower.replace(/\0/g, ''),
        //            EngineCapacity: data.VehicleData.engineCapacity.replace(/\0/g, ''),
        //            FuelType: data.VehicleData.typeOfFuel.replace(/\0/g, ''),
        //            PowerWeightRatio: data.VehicleData.powerWeightRatio.replace(/\0/g, ''),
        //            Mass: data.VehicleData.vehicleMass.replace(/\0/g, ''),
        //            MaxPermissibleLadenMass: data.VehicleData.maximumPermissibleLadenMass.replace(/\0/g, ''),
        //            TypeApprovalNumber: data.VehicleData.typeApprovalNumber.replace(/\0/g, ''),
        //            NumberOfSeats: data.VehicleData.numberOfSeats.replace(/\0/g, ''),
        //            NumberOfStandingPlaces: data.VehicleData.numberOfStandingPlaces.replace(/\0/g, ''),
        //            EngineIDNumber: data.VehicleData.engineIDNumber.replace(/\0/g, ''),
        //            VehicleIDNumber: data.VehicleData.vehicleIDNumber.replace(/\0/g, ''),
        //            NumberOfAxles: data.VehicleData.numberOfAxles.replace(/\0/g, ''),
        //            Category: data.VehicleData.vehicleCategory.replace(/\0/g, ''),
        //            Color: data.VehicleData.colourOfVehicle.replace(/\0/g, ''),
        //            RestrictionToChangeOwner: UtilityService.convertSerbianJoinedDateStringToISODateString(data.VehicleData.restrictionToChangeOwner.replace(/\0/g, '')),
        //            Load: data.VehicleData.vehicleLoad.replace(/\0/g, '')
        //        },

        //        PersonalData: {
        //            OwnerPersonalNo: data.PersonalData.ownersPersonalNo.replace(/\0/g, ''),
        //            OwnerName: data.PersonalData.ownerName.replace(/\0/g, ''),
        //            OwnerSurnameOrBusinessName: data.PersonalData.ownersSurnameOrBusinessName.replace(/\0/g, ''),
        //            OwnerAddress: data.PersonalData.ownerAddress.replace(/\0/g, ''),
        //            UserPersonalNo: data.PersonalData.usersPersonalNo.replace(/\0/g, ''),
        //            UserName: data.PersonalData.usersName.replace(/\0/g, ''),
        //            UserSurnameOrBusinessName: data.PersonalData.usersSurnameOrBusinessName.replace(/\0/g, ''),
        //            UserAddress: data.PersonalData.usersAddress.replace(/\0/g, '')
        //        }
        //    };

        //    var dlg = bootbox.dialog({
        //        size: 'large',
        //        title: '<span style="font-weight: bold;">Klijent</span>',
        //        message: dialogHtml,
        //        buttons: {
        //            cancel: {
        //                label: 'Odustani',
        //                className: 'btn-primary'
        //            },
        //            addClient: {
        //                label: 'Upiši novog klijenta',
        //                className: 'btn-success',
        //                callback: function () {
        //                    ClientsService.addClientFull(model).then(
        //                        function () {
        //                            toastr.success('Novi klijent uspešno upisan.');
        //                        },
        //                        function (error) {
        //                            toastr.error('Došlo je do greške prilikom upisa novog klijenta.');
        //                            toastr.error('[GREŠKA] --> ' + error.statusText);
        //                            return false;
        //                        }
        //                    );
        //                }
        //            }
        //        }
        //    });
        //}

        //#endregion

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
