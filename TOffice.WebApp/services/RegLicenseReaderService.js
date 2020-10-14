(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('RegLicenseReaderService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/reg-license-reader';

        var service = {
            getSmartCardResponseUrl: getSmartCardResponseUrl,
            readSmartCardData: readSmartCardData,
            // insertSmartCardResponse: insertSmartCardResponse,
            getSmartCardResponse: getSmartCardResponse,
            deleteSmartCardResponse: deleteSmartCardResponse
        };

        return service;

        function readSmartCardData(wsConnectionId) {
            var url = WebApiBaseUrl + urlRoot + '/read/' + wsConnectionId;
            return $http.get(url);
        }

        //function insertSmartCardResponse(wsConnectionId, model) {
        //    var url = WebApiBaseUrl + urlRoot + '/response/' + wsConnectionId;
        //    return $http.post(url, model);
        //}

        function getSmartCardResponseUrl(wsConnectionId) {
            return WebApiBaseUrl + urlRoot + '/get/' + wsConnectionId;
        }

        function getSmartCardResponse(wsConnectionId) {
            var url = WebApiBaseUrl + urlRoot + '/get/' + wsConnectionId;
            return $http.get(url);
        }

        function deleteSmartCardResponse(wsConnectionId) {
            var url = WebApiBaseUrl + urlRoot + '/delete/' + wsConnectionId;
            return $http.delete(url);
        }
    }
})();
