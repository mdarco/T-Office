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
            addClientFull: addClientFull
        };

        return service;

        function getFiltered(filter) {
            var url = WebApiBaseUrl + urlRoot + '/filtered?nd=' + Date.now();
            return $http.post(url, filter);
        }

        function addClientFull(model) {
            var url = WebApiBaseUrl + urlRoot + '/full';
            return $http.post(url, model);
        }
    }
})();
