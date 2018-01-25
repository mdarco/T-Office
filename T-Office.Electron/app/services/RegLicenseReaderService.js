(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('RegLicenseReaderService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/reg-license-reader';

        var service = {
            readData: readData
        };

        return service;

        function readData() {
            var url = WebApiBaseUrl + urlRoot + '/read?nd=' + Date.now();
            return $http.get(url);
        }
    }
})();
