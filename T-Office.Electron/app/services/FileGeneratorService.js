(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('FileGeneratorService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var urlRoot = '/file-generator';

        const PAID_INSTALLMENT_RECEIPT_TEMPLATE = 'potvrda-placanja.docx';

        var service = {
            createFileFromTemplate: createFileFromTemplate
        };

        return service;

        function createFileFromTemplate(templateName, model) {
            if (!templateName) {
                model.TemplateName = PAID_INSTALLMENT_RECEIPT_TEMPLATE;
            } else {
                model.TemplateName = templateName;
            }

            var url = WebApiBaseUrl + urlRoot + '/generate-file-from-template?nd=' + Date.now();
            return $http.post(url, model, { responseType: "arraybuffer" });
        }
    }
})();
