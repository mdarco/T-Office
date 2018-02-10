(function () {
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
            addVehicleFull: addVehicleFull
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
    }
})();
