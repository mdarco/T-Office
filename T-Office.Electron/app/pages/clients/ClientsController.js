(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', '$q', 'NgTableParams', 'ClientsService', 'UtilityService', 'RegLicenseReaderService', 'toastr'];

    function ctrlFn($rootScope, $scope, $location, $uibModal, $q, NgTableParams, ClientsService, UtilityService, RegLicenseReaderService, toastr) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuClients").addClass("active");

        // prevent 'Enter' to submit the form
        $('#searchForm').bind('keydown', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
            }
        });

        $scope.filter = {};
        $scope.totalRecords = 0;
        $scope.initialDocListLoad = true;
        $scope.showGrid = false;

        $scope.clientsTableParams = new NgTableParams(
            {
                page: 1,
                count: 10
            },
            {
                total: 0,
                getData: function (params) {
                    if ($scope.initialDocListLoad) {
                        $scope.initialDocListLoad = false;
                        return [];
                    }

                    $scope.filter = $scope.filter || {};

                    $scope.filter.PageNo = params.page();
                    $scope.filter.RecordsPerPage = params.count();

                    return ClientsService.getFiltered($scope.filter).then(
                        function (result) {
                            if (!result || !result.data || !result.data.Data) {
                                $scope.showGrid = false;
                                return [];
                            }

                            if (result.data.Data.length > 0) {
                                $scope.showGrid = true;
                            } else {
                                $scope.showGrid = false;
                            }

                            params.total(result.data.Total);
                            $scope.totalRecords = result.data.Total;

                            return result.data.Data;
                        },
                        function (error) {
                            toastr.error('Došlo je do greške prilikom pretraživanja.');
                            $scope.showGrid = false;
                        }
                    );
                }
            }
        );

        $scope.applyFilter = function () {
            $scope.clientsTableParams.reload();
        };

        $scope.clearFilter = function () {
            $scope.filter = {};
            $scope.showGrid = false;
            $scope.totalRecords = 0;
            $scope.clientsTableParams.data = {};
        };

        $scope.addClient = function () {
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
                        RegLicenseReaderService.readData().then(
                            function (result) {
                                if (result && result.data) {
                                    var data = JSON.parse(result.data);
                                    if (data.IsError) {
                                        //toastr.error('[GREŠKA] --> ' + data.ErrorMessage);
                                        toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                        return;
                                    } else {
                                        var filters_ownerName = data.Result.PersonalData.ownerName.split(' ');
                                        filters_ownerName = _.filter(filters_ownerName, function (item) {
                                            return item.replace(/\0/g, '').length > 3; // eliminates null strings
                                        });

                                        var filters_ownersSurnameOrBusinessName = data.Result.PersonalData.ownersSurnameOrBusinessName.split(' ');
                                        filters_ownersSurnameOrBusinessName = _.filter(filters_ownersSurnameOrBusinessName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var filters_usersName = data.Result.PersonalData.usersName.split(' ');
                                        filters_usersName = _.filter(filters_usersName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var filters_usersSurnameOrBusinessName = data.Result.PersonalData.usersSurnameOrBusinessName.split(' ');
                                        filters_usersSurnameOrBusinessName = _.filter(filters_usersSurnameOrBusinessName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var clientNameFilters = [
                                            ...filters_ownerName, ...filters_ownersSurnameOrBusinessName,
                                            ...filters_usersName, ...filters_usersSurnameOrBusinessName
                                        ];

                                        getExistingClients(clientNameFilters).then(
                                            function (existingClientsArray) {
                                                var existingClients = [];
                                                _.each(existingClientsArray, function (item) {
                                                    existingClients = _.concat(existingClients, item.data.Data || []);
                                                });

                                                insertClient(data.Result, existingClients || []);
                                            },
                                            function (error) {
                                                insertClient(data.Result, []);
                                            }
                                        );
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

        function insertClient(data, existingClients) {
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

                <br /><br />

                <table class="table table-condensed table-striped">
                    <caption>Postojeći klijenti</caption>
                    <tbody>
            `;

            _.each(existingClients, function (existingClient) {
                dialogHtml += '<tr>';
                dialogHtml += '<td>Vlasnik: ' + existingClient.FullOwnerName + '<br />';
                dialogHtml += 'Korisnik: ' + existingClient.FullUserName + '</td>';
                dialogHtml += '</tr>';
            });

            dialogHtml += `
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

            bootbox.dialog({
                title: '<span style="font-weight: bold;">Novi klijent</span>',
                message: dialogHtml,
                buttons: {
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    },
                    addClient: {
                        label: 'Upiši klijenta',
                        className: 'btn-success',
                        callback: function () {
                            ClientsService.addClientFull(model).then(
                                function () {
                                    toastr.success('Novi klijent uspešno upisan.');
                                },
                                function (error) {
                                    toastr.error('Došlo je do greške prilikom upisa novog klijenta.');
                                    toastr.error('[GREŠKA] --> ' + error.statusText);
                                    return false;
                                }
                            );
                        }
                    }
                }
            });
        }

        function getExistingClients(filters) {
            var promises = [];
            _.each(filters, function (filter) {
                promises.push(ClientsService.getFiltered({ ClientName: filter }));
            });

            return $q.all(promises);
        }

        $scope.addClientManually = function () {
            alert('U razvoju..');
        };

        $scope.openClientDossier = function (client) {
            $location.path('/client-file/' + client.ID);
        };
    }
})();
