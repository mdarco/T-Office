(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('ClientsService', serviceFn);

    serviceFn.$inject = ['$http', '$q', 'UtilityService', 'WebApiBaseUrl'];

    function serviceFn($http, $q, UtilityService, WebApiBaseUrl) {
        var urlRoot = '/clients';

        var service = {
            getFiltered: getFiltered,
            simpleExist: simpleExist,
            getClient: getClient,
            addClientFull: addClientFull,
            addClient: addClient,
            editClient: editClient,
            deleteClient: deleteClient,

            // fullClientDataEntry: fullClientDataEntry,

            addRegDocData: addRegDocData,

            getVehicles: getVehicles,
            addVehicleFull: addVehicleFull,

            getVehicleRegistrations: getVehicleRegistrations,
            addVehicleRegistration: addVehicleRegistration,
            editVehicleRegistration: editVehicleRegistration,
            deleteVehicleRegistration: deleteVehicleRegistration,
            getVehicleRegistrationInstallments: getVehicleRegistrationInstallments,
            editVehicleRegistrationInstallment: editVehicleRegistrationInstallment,
            resetVehicleRegistrationInstallments: resetVehicleRegistrationInstallments,

            getClientsDue: getClientsDue,
            getClientsOutstandingTotal: getClientsOutstandingTotal,
            getIncomingRegistrations: getIncomingRegistrations,
            getTotalPaidAmountByPeriod: getTotalPaidAmountByPeriod,

            getCostsByPeriod: getCostsByPeriod,
            getTotalInstallmentsAmount: getTotalInstallmentsAmount,

            // utilities
            getClientNameFilters: getClientNameFilters,
            getExistingClients: getExistingClients,
            formatExistingClients: formatExistingClients,
            createInsertClientDialogHtml: createInsertClientDialogHtml,
            createInsertClientDataModel: createInsertClientDataModel
        };

        return service;

        function getFiltered(filter) {
            var url = WebApiBaseUrl + urlRoot + '/filtered?nd=' + Date.now();
            return $http.post(url, filter);
        }

        function simpleExist(model) {
            var url = WebApiBaseUrl + urlRoot + '/simple-exist?nd=' + Date.now();
            return $http.post(url, model);
        }

        function getClient(id) {
            var url = WebApiBaseUrl + urlRoot + '/' + id + '?nd=' + Date.now();
            return $http.get(url);
        }

        function addClientFull(model) {
            var url = WebApiBaseUrl + urlRoot + '/full';
            return $http.post(url, model);
        }

        function addClient(model) {
            var url = WebApiBaseUrl + urlRoot;
            return $http.post(url, model);
        }

        function editClient(id, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + id;
            return $http.put(url, model);
        }

        function deleteClient(id) {
            var url = WebApiBaseUrl + urlRoot + '/' + id;
            return $http.delete(url);
        }

        function fullClientEntry(model) {
            var url = WebApiBaseUrl + urlRoot + '/full-client-data-entry?nd=' + Date.now();
            return $http.post(url, model);
        }

        //#region Utilities

        function getClientNameFilters(data) {
            var filters_ownerName = data.PersonalData.ownerName.split(' ');
            filters_ownerName = _.filter(filters_ownerName, function (item) {
                return item.replace(/\0/g, '').length > 3; // eliminates null strings
            });

            var filters_ownersSurnameOrBusinessName = data.PersonalData.ownersSurnameOrBusinessName.split(' ');
            filters_ownersSurnameOrBusinessName = _.filter(filters_ownersSurnameOrBusinessName, function (item) {
                return item.replace(/\0/g, '').length > 3;
            });

            var filters_usersName = data.PersonalData.usersName.split(' ');
            filters_usersName = _.filter(filters_usersName, function (item) {
                return item.replace(/\0/g, '').length > 3;
            });

            var filters_usersSurnameOrBusinessName = data.PersonalData.usersSurnameOrBusinessName.split(' ');
            filters_usersSurnameOrBusinessName = _.filter(filters_usersSurnameOrBusinessName, function (item) {
                return item.replace(/\0/g, '').length > 3;
            });

            return [
                ...filters_ownerName, ...filters_ownersSurnameOrBusinessName,
                ...filters_usersName, ...filters_usersSurnameOrBusinessName
            ];
        }

        function getExistingClients(data) {
            const filters = getClientNameFilters(data);

            var promises = [];
            _.each(filters, function (filter) {
                promises.push(getFiltered({ ClientName: filter }));
            });

            return $q.all(promises);
        }

        function formatExistingClients(existingClientsArray) {
            let formattedExistingClients = [];
            _.each(existingClientsArray, function (item) {
                formattedExistingClients = _.concat(formattedExistingClients, item.data.Data || []);
            });

            return _.uniqBy(formattedExistingClients, ['FullOwnerName', 'FullUserName']);
        }

        function createInsertClientDialogHtml(data, existingClients) {
            let dialogHtml = `
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
                dialogHtml += '<td>Vlasnik: [' + existingClient.OwnerJMBGMB + '] ' + existingClient.FullOwnerName + '(' + existingClient.OwnerAddress + ')<br />';
                dialogHtml += 'Korisnik: [' + existingClient.UserJMBGMB + '] ' + existingClient.FullUserName + '(' + existingClient.UserAddress + ')</td>';
                dialogHtml += '<td><a href="#/client-file/' + existingClient.ID + '" class="btn btn-xs btn-primary" onclick="bootbox.hideAll()">Detalji</></td>';
                dialogHtml += '</tr>';
            });

            dialogHtml += `
                    </tbody>
                </table>
            `;

            return dialogHtml;
        }

        function createInsertClientDataModel(data) {
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

            return model;
        }

        //#endregion

        //#region Registration document and vehicle data

        function addRegDocData(id, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + id + '/reg-doc-data';
            return $http.post(url, model);
        }

        //#endregion

        //#region Vehicles

        function getVehicles(clientID) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles?nd=' + Date.now();
            return $http.get(url);
        }

        function addVehicleFull(clientID, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/full';
            return $http.post(url, model);
        }

        //#endregion

        //#region Registrations

        function getVehicleRegistrations(clientID, vehicleID) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations' + '?nd=' + Date.now();
            return $http.get(url);
        }

        function addVehicleRegistration(clientID, vehicleID, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations';
            return $http.post(url, model);
        }

        function editVehicleRegistration(clientID, vehicleID, vehicleRegistrationID, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations/' + vehicleRegistrationID;
            return $http.put(url, model);
        }

        function deleteVehicleRegistration(clientID, vehicleID, vehicleRegistrationID) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations/' + vehicleRegistrationID;
            return $http.delete(url);
        }

        function getVehicleRegistrationInstallments(clientID, vehicleID, vehicleRegistrationID) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations/' + vehicleRegistrationID + '/installments' + '?nd=' + Date.now();
            return $http.get(url);
        }

        function editVehicleRegistrationInstallment(clientID, vehicleID, vehicleRegistrationID, installmentID, model) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations/' + vehicleRegistrationID + '/installments/' + installmentID;
            return $http.put(url, model);
        }

        function resetVehicleRegistrationInstallments(clientID, vehicleID, vehicleRegistrationID) {
            var url = WebApiBaseUrl + urlRoot + '/' + clientID + '/vehicles/' + vehicleID + '/registrations/' + vehicleRegistrationID + '/installments/reset';
            return $http.get(url);
        }

        //#endregion

        //#region Analytics

        function getClientsDue(numberOfDays) {
            var url = WebApiBaseUrl + urlRoot + '/analytics/clients-due/' + numberOfDays + '?nd=' + Date.now();
            return $http.get(url);
        }

        function getClientsOutstandingTotal() {
            var url = WebApiBaseUrl + urlRoot + '/analytics/clients-outstanding-total?nd=' + Date.now();
            return $http.get(url);
        }

        function getIncomingRegistrations(numberOfDays) {
            var url = WebApiBaseUrl + urlRoot + '/analytics/incoming-registrations/' + numberOfDays + '?nd=' + Date.now();
            return $http.get(url);
        }

        function getTotalPaidAmountByPeriod(model) {
            var url = WebApiBaseUrl + urlRoot + '/analytics/total-paid-amount-by-period?nd=' + Date.now();
            return $http.post(url, model);
        }

        //#endregion

        //#region Reports

        function getCostsByPeriod(filter) {
            var url = WebApiBaseUrl + urlRoot + '/reports/costs-by-period?nd=' + Date.now();
            return $http.post(url, filter);
        }

        function getTotalInstallmentsAmount(filter) {
            var url = WebApiBaseUrl + urlRoot + '/reports/total-installments-amount?nd=' + Date.now();
            return $http.post(url, filter);
        }

        //#endregion
    }
})();
