(function () {
    'use strict';

    var app = angular.module('TOfficeApp');

    app.constant('WebApiBaseUrl', 'http://localhost:5000/api');

    app.constant('AppParams', {
        datePickerOptions: {
            ISO_DATE_FORMAT: 'YYYY-MM-DD',
            DATE_FORMAT: 'YYYY-MM-DD',
            DISPLAY_DATE_FORMAT: 'DD.MM.YYYY',
            startingDay: 1
        },

        DEBUG: true
    });
})();
