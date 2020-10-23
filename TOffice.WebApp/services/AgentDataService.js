(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('AgentDataService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/agent-data';

        var service = {
            get: get
        };

        return service;

        function get(agentId) {
            var url = WebApiBaseUrl + urlRoot + '/' + agentId;
            return $http.get(url);
        }
    }
})();
