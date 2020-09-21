(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('UtilityService', serviceFn);

    serviceFn.$inject = ['$http', 'WebApiBaseUrl'];

    function serviceFn($http, WebApiBaseUrl) {
        var service = {
            convertISODateStringToDate: convertISODateStringToDate,
            convertDateToISODateString: convertDateToISODateString,
            convertSerbianDateStringToISODateString: convertSerbianDateStringToISODateString,
            convertSerbianJoinedDateStringToISODateString: convertSerbianJoinedDateStringToISODateString
        };

        return service;

        function convertISODateStringToDate(isoDateString) {
            if (isoDateString) {
                var isoDateParts = isoDateString.split('T');
                if (isoDateParts && isoDateParts[0]) {
                    var dateParts = isoDateParts[0].split('-');
                    if (dateParts && dateParts.length === 3) {
                        return new Date(parseInt(dateParts[0]), parseInt(dateParts[1]) - 1, parseInt(dateParts[2]));
                    }
                }
            }
        }

        function convertDateToISODateString(date) {
            if (_.isDate(date)) {
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();

                return year + '-' + (month < 10 ? '0' + month : month) + '-' + (day < 10 ? '0' + day : day);
            }
        }

        function convertSerbianDateStringToISODateString(serbianDateString) {
            // serbian date string: dd.mm.yyyy
            var parts = serbianDateString.split('.');
            return parts[2] + '-' + parts[1] + '-' + parts[0];
        }

        function convertSerbianJoinedDateStringToISODateString(serbianDateString) {
            // serbian joined date string: ddmmyyyy
            return serbianDateString[4] + serbianDateString[5] + serbianDateString[6] + serbianDateString[7] + '-' + serbianDateString[2] + serbianDateString[3] + '-' + serbianDateString[0] + serbianDateString[1];
        }
    }
})();
