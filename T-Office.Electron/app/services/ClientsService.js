﻿(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('ClientsService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/clients';

        var service = {
            getFiltered: getFiltered,
            getClient: getClient,
            addClientFull: addClientFull,
            addClient: addClient,
            editClient: editClient,

            getVehicles: getVehicles,
            addVehicleFull: addVehicleFull,

            getVehicleRegistrations: getVehicleRegistrations,
            addVehicleRegistration: addVehicleRegistration,
            editVehicleRegistration: editVehicleRegistration,
            deleteVehicleRegistration: deleteVehicleRegistration,
            getVehicleRegistrationInstallments: getVehicleRegistrationInstallments,
            editVehicleRegistrationInstallment: editVehicleRegistrationInstallment,

            getClientsDue: getClientsDue,
            getClientsOutstandingTotal: getClientsOutstandingTotal,

            getCostsByPeriod: getCostsByPeriod
        };

        return service;

        function getFiltered(filter) {
            var url = WebApiBaseUrl + urlRoot + '/filtered?nd=' + Date.now();
            return $http.post(url, filter);
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

        //#endregion

        //#region Reports

        function getCostsByPeriod(filter) {
            var url = WebApiBaseUrl + urlRoot + '/reports/costs-by-period?nd=' + Date.now();
            return $http.post(url, filter);
        }

        //#endregion
    }
})();
