(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('PollingService', serviceFn);

    serviceFn.$inject = ['$http'];

    function serviceFn($http) {
        var DEFAULT_POLLING_TIME = 5000; // 5 sec
        var polls = [];

        var service = {
            start: startPolling,
            stop: stopPolling
        };

        return service;

        function startPolling(name, url, pollingTime, callback) {
            if (!polls[name]) {
                var poller = function () {
                    return $http.get(url).then(function (response) {
                        callback(response);
                    });
                }
            }
            poller();
            polls[name] = setInterval(poller, pollingTime || DEFAULT_POLLING_TIME);
        };

        function stopPolling(name) {
            clearInterval(polls[name]);
            delete polls[name];
        };
    }
})();
