(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('PdfService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/pdf-helper';

        var service = {
            createSimplePdf: createSimplePdf
        };

        return service;

        function createSimplePdf(model) {
            var url = WebApiBaseUrl + urlRoot + '/generate-simple-pdf?nd=' + Date.now();
            return $http.post(url, model, { responseType: "arraybuffer" });
        }
    }
})();
