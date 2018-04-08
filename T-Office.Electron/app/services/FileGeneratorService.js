(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('FileGeneratorService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/file-generator';

        var service = {
            createFileFromTemplate: createFileFromTemplate
        };

        return service;

        function createFileFromTemplate(templateName, model) {
            var url = WebApiBaseUrl + urlRoot + '/generate-file-from-template/' + templateName + '?nd=' + Date.now();
            return $http.post(url, model, { responseType: "arraybuffer" });
        }
    }
})();
