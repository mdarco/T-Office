(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientFileController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'RegLicenseReaderService', 'UtilityService', 'AgentDataService', 'blockUI', 'toastr', 'client' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, RegLicenseReaderService, UtilityService, AgentDataService, blockUI, toastr, client /* , AuthenticationService */) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuClients").addClass("active");

        //var currentUser = AuthenticationService.getCurrentUser();
        blockUI.stop();

        $scope.client = client;

        $scope.selectedVehicle = {
            vehicle: null,
            visible: false
        };

        $scope.vehicleRegistrations = [];

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
                        var agentId = sessionStorage.getItem('tofficeAgentId');
                        console.log('Agent ID for RegLicenseReader service [ClientFileController]: ' + agentId);

                        AgentDataService.get(agentId).then(agentData => {
                            console.log('Agent WS connection ID for RegLicenseReader service [ClientFileController]: ' + agentData.WsConnectionId);
                            RegLicenseReaderService.readSmartCardData(agentData.WsConnectionId).then(
                                function (result) {
                                    if (result && result.data) {
                                        var data = JSON.parse(result.data);
                                        if (data.IsError) {
                                            //toastr.error('[GREŠKA] --> ' + data.ErrorMessage);
                                            toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                            return;
                                        } else {
                                            insertVehicle(data);
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
                        });
                    }
                }
            });
        };

        $scope.addVehicleManually = function () {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/client-file/reg-doc-dialog/reg-doc-dialog.html',
                controller: 'RegDocDialogController',
                resolve: {
                    clientID: function () {
                        return $scope.client.ID;
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            dialog.result.then(
                function () {
                    getVehicles();
                },
                function () {
                    // modal dismissed => do nothing
                }
            );
        };

        function insertVehicle(licenseData) {
            var data = licenseData.Result;

            var dialogHtml = `
                <table class="table table-condensed table-striped">
                    <tbody>
                        <tr>
                            <td style="color: blue;">Vlasnik</td>
                            <td>${data.PersonalData.ownersName || ''} ${data.PersonalData.ownersSurnameOrBusinessName || ''}, ${data.PersonalData.ownerAddress || ''}</td>
                        </tr>
                        <tr>
                            <td style="color: blue;">Korisnik</td>
                            <td>${data.PersonalData.usersName || ''} ${data.PersonalData.usersSurnameOrBusinessName || ''}, ${data.PersonalData.usersAddress || ''}</td>
                        </tr>
                        <tr>
                            <td style="color: blue;">Vozilo</td>
                            <td>${data.VehicleData.vehicleMake || ''} ${data.VehicleData.commercialDescription || ''} (${data.VehicleData.registrationNumberOfVehicle || ''})</td>
                        </tr>
                    </tbody>
                </table>
            `;

            var model = {
                DocumentData: {
                    IssuingState: data.DocumentData.stateIssuing.replace(/\0/g, ''),
                    CompetentAuthority: data.DocumentData.competentAuthority.replace(/\0/g, ''),
                    IssuingAuthority: data.DocumentData.authorityIssuing.replace(/\0/g, ''),
                    UnambiguousNumber: data.DocumentData.unambiguousNumber.replace(/\0/g, ''),
                    IssuingDate: UtilityService.convertSerbianDateStringToISODateString(data.DocumentData.issuingDate.replace(/\0/g, '')),
                    ExpiryDate: UtilityService.convertSerbianDateStringToISODateString(data.DocumentData.expiryDate.replace(/\0/g, '')),
                    SerialNumber: data.DocumentData.serialNumber.replace(/\0/g, '')
                },

                VehicleData: {
                    RegistrationNumber: data.VehicleData.registrationNumberOfVehicle.replace(/\0/g, ''),
                    FirstRegistrationDate: UtilityService.convertSerbianDateStringToISODateString(data.VehicleData.dateOfFirstRegistration.replace(/\0/g, '')),
                    ProductionYear: data.VehicleData.yearOfProduction.replace(/\0/g, ''),
                    Make: data.VehicleData.vehicleMake.replace(/\0/g, ''),
                    Model: data.VehicleData.commercialDescription.replace(/\0/g, ''),
                    Type: data.VehicleData.vehicleType.replace(/\0/g, ''),
                    EnginePowerKW: data.VehicleData.maximumNetPower.replace(/\0/g, ''),
                    EngineCapacity: data.VehicleData.engineCapacity.replace(/\0/g, ''),
                    FuelType: data.VehicleData.typeOfFuel.replace(/\0/g, ''),
                    PowerWeightRatio: data.VehicleData.powerWeightRatio.replace(/\0/g, ''),
                    Mass: data.VehicleData.vehicleMass.replace(/\0/g, ''),
                    MaxPermissibleLadenMass: data.VehicleData.maximumPermissibleLadenMass.replace(/\0/g, ''),
                    TypeApprovalNumber: data.VehicleData.typeApprovalNumber.replace(/\0/g, ''),
                    NumberOfSeats: data.VehicleData.numberOfSeats.replace(/\0/g, ''),
                    NumberOfStandingPlaces: data.VehicleData.numberOfStandingPlaces.replace(/\0/g, ''),
                    EngineIDNumber: data.VehicleData.engineIDNumber.replace(/\0/g, ''),
                    VehicleIDNumber: data.VehicleData.vehicleIDNumber.replace(/\0/g, ''),
                    NumberOfAxles: data.VehicleData.numberOfAxles.replace(/\0/g, ''),
                    Category: data.VehicleData.vehicleCategory.replace(/\0/g, ''),
                    Color: data.VehicleData.colourOfVehicle.replace(/\0/g, ''),
                    RestrictionToChangeOwner: UtilityService.convertSerbianJoinedDateStringToISODateString(data.VehicleData.restrictionToChangeOwner.replace(/\0/g, '')),
                    Load: data.VehicleData.vehicleLoad.replace(/\0/g, '')
                },

                PersonalData: {
                    OwnerPersonalNo: data.PersonalData.ownersPersonalNo.replace(/\0/g, ''),
                    OwnerName: data.PersonalData.ownerName.replace(/\0/g, ''),
                    OwnerSurnameOrBusinessName: data.PersonalData.ownersSurnameOrBusinessName.replace(/\0/g, ''),
                    OwnerAddress: data.PersonalData.ownerAddress.replace(/\0/g, ''),
                    UserPersonalNo: data.PersonalData.usersPersonalNo.replace(/\0/g, ''),
                    UserName: data.PersonalData.usersName.replace(/\0/g, ''),
                    UserSurnameOrBusinessName: data.PersonalData.usersSurnameOrBusinessName.replace(/\0/g, ''),
                    UserAddress: data.PersonalData.usersAddress.replace(/\0/g, '')
                }
            };

            var dlg = bootbox.dialog({
                size: 'large',
                title: '<span style="font-weight: bold;">Vozilo za klijenta</span>',
                message: dialogHtml,
                buttons: {
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    },
                    addVehicle: {
                        label: 'Upiši novo vozilo',
                        className: 'btn-success',
                        callback: function () {
                            // check if the client already has the vehicle with the same reg no.
                            var existingVehicle = _.find($scope.client.Vehicles, { 'RegistrationNumber': model.VehicleData.RegistrationNumber });
                            if (existingVehicle) {
                                toastr.warning('Vozilo ' + model.VehicleData.RegistrationNumber + ' već postoji.');
                                bootbox.hideAll();
                                return;
                            }

                            // check if the client from the reg license is the same as the current client
                            var isClientOk = clientMatch(model);
                            if (!isClientOk) {
                                // ask for confirmation
                                bootbox.confirm({
                                    title: 'T-Office',
                                    message: 'Izgleda da klijent na saobraćajnoj dozvoli nije isti kao trenutno izabrani klijent.<br />Da li ste sigurni da želite da unesete novo vozilo?',
                                    buttons: {
                                        cancel: {
                                            label: 'Odustani',
                                            className: 'btn-primary'
                                        },
                                        confirm: {
                                            label: 'Upiši',
                                            className: 'btn-success'
                                        }
                                    },
                                    callback: function (confirmResult) {
                                        if (confirmResult) {
                                            _insertVehicle(model);
                                        } else {
                                            bootbox.hideAll();
                                        }
                                    }
                                });
                            } else {
                                _insertVehicle(model);
                            }
                        }
                    }
                }
            });
        }

        function _insertVehicle(model) {
            ClientsService.addVehicleFull($scope.client.ID, model).then(
                function () {
                    toastr.success('Novo vozilo uspešno upisano.');
                    bootbox.hideAll();

                    // refresh vehicles list
                    getVehicles();

                },
                function (error) {
                    toastr.error('Došlo je do greške prilikom upisa novog vozila.');
                    toastr.error(error.statusText);
                    return false;
                }
            );
        }

        function clientMatch(model) {
            var currentClient = $scope.client;
            var regLicenseClient = model.PersonalData;

            if (currentClient.OwnerPersonalNo.toLowerCase() !== regLicenseClient.OwnerPersonalNo.toLowerCase()) {
                return false;
            }

            if ((currentClient.OwnerName + ' ' + currentClient.OwnerSurnameOrBusinessName).toLowerCase() !== (regLicenseClient.OwnerName + ' ' + regLicenseClient.OwnerSurnameOrBusinessName).toLowerCase()) {
                return false;
            }

            if (currentClient.UserPersonalNo.toLowerCase() !== regLicenseClient.UserPersonalNo.toLowerCase()) {
                return false;
            }

            if ((currentClient.UserName + ' ' + currentClient.UserSurnameOrBusinessName).toLowerCase() !== (regLicenseClient.UserName + ' ' + regLicenseClient.UserSurnameOrBusinessName).toLowerCase()) {
                return false;
            }

            return true;
        }

        $scope.getVehicleRegistrations = function (vehicle) {
            ClientsService.getVehicleRegistrations($scope.client.ID, vehicle.ID).then(
                (result) => {
                    if (result && result.data) {
                        $scope.selectedVehicle = {
                            vehicle: vehicle,
                            visible: true
                        };

                        $scope.vehicleRegistrations = result.data;
                    }
                },
                (error) => {
                    toastr.error('Došlo je do greške prilikom preuzimanja registracija za vozilo ' + vehicle.RegistrationNumber);
                }
            );
        };

        $scope.addVehicleRegistration = function () {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/client-file/reg-dialog/reg-dialog.html',
                controller: 'RegDialogController',
                resolve: {
                    client: function () {
                        return angular.copy($scope.client);
                    },
                    vehicle: function () {
                        return angular.copy($scope.selectedVehicle.vehicle);
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            dialog.result.then(
                function () {
                    $scope.getVehicleRegistrations($scope.selectedVehicle.vehicle);
                },
                function () {
                    // modal dismissed => do nothing
                }
            );
        };

        $scope.editVehicleRegistration = function (reg, dataField) {
            if (dataField === 'NextRegistrationDate' || dataField === 'RegistrationDate') {
                openDateFieldDialog(dataField, reg[dataField]).then(
                    function (result) {
                        var editObj = {};
                        editObj[dataField] = UtilityService.convertDateToISODateString(result.Value);

                        ClientsService.editVehicleRegistration(client.ClientID, $scope.selectedVehicle.vehicle.VehicleID, reg.ID, editObj).then(
                            function () {
                                toastr.success('Podatak uspešno ažuriran.');
                                reg[dataField] = result.Value;
                            },
                            function (error) {
                                toastr.error(error.statusText);
                            }
                        );
                    },
                    function (error) { }
                );
            }
        };

        $scope.showInstallments = function (vehicleRegistration) {
            var dialogOpts = {
                size: 'lg',
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/client-file/installments-list-dialog/installments-list-dialog.html',
                controller: 'InstallmentsListDialogController',
                resolve: {
                    installments: function () {
                        return vehicleRegistration.Installments;
                    },
                    context: function () {
                        return {
                            ClientID: $scope.client.ID,
                            VehicleID: $scope.selectedVehicle.vehicle.ID,
                            VehicleRegistrationID: vehicleRegistration.ID,
                            Client: angular.copy($scope.client),
                            Vehicle: angular.copy($scope.selectedVehicle.vehicle),
                            VehicleRegistration: angular.copy(vehicleRegistration)
                        };
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            dialog.result.then(
                function () {

                },
                function () {
                    // modal dismissed => re-calculate total outstanding amount for vehicle registration
                    //calculateTotalOutstandingAmount(vehicleRegistration);
                    $scope.getVehicleRegistrations($scope.selectedVehicle.vehicle);
                }
            );
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
            // console.log(dataField);
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

                case 'RegistrationDate':
                    return 'Broj registracije';

                case 'NextRegistrationDate':
                    return 'Datum isteka registracije';

                default:
                    return dataField;
            }
        }

        $scope.resolveVehicleRegistrationCssClass = function (registration) {
            var installments = registration.Installments;

            if (installments && installments.length > 0) {
                var isProblematic = false;
                var isCompleted = true;

                // return false; breaks the _.each() loop
                _.each(installments, installment => {
                    if (!installment.IsPaid) {
                        isCompleted = false;

                        if (!isProblematic) {
                            var today = moment(Date.now());
                            var installmentDate = moment(installment.InstallmentDate);

                            if (installmentDate < today) {
                                isProblematic = true;
                            }
                        }
                    }
                });

                if (isProblematic) {
                    return 'toffice-alert-row';
                }

                if (isCompleted) {
                    return 'toffice-ok-row';
                }
            }

            return '';
        };

        function getVehicles() {
            ClientsService.getVehicles($scope.client.ID).then(
                (result) => {
                    if (result && result.data) {
                        $scope.client.Vehicles = result.data;

                        $scope.selectedVehicle = {
                            vehicle: null,
                            visible: false
                        };
                    }
                },
                (error) => {
                    toastr.error('Došlo je do greške na serveru prilikom ažuriranja spiska vozila.');
                    toastr.error(error.statusText);
                }
            );
        }

        function calculateTotalOutstandingAmount(reg) {
            var installments = reg.Installments;
            if (installments && installments.length > 0) {
                reg.TotalOutstandingAmount = 0;
                _.each(installments, (installment) => {
                    reg.TotalOutstandingAmount += installment.Amount;
                    if (installment.PaidAmount && installment.PaidAmount > 0) {
                        reg.TotalOutstandingAmount -= installment.PaidAmount;
                    }
                });
            }
        }

        // helpers
        function openDateFieldDialog(dataField, date) {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/common/date-dialog/date-dialog.html',
                controller: 'DateDialogController',
                resolve: {
                    settings: function () {
                        return {
                            DisplayTitle: 'T-Office',
                            LabelTitle: resolveDataFieldLabel(dataField),
                            DateValue: _.isDate(date) ? date : UtilityService.convertISODateStringToDate(date)
                        };
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            return dialog.result;
        }
    }
})();
