(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('PollingService', serviceFn);

    serviceFn.$inject = ['$http'];

    function serviceFn($http) {
        const DEFAULT_POLLING_INTERVAL = 2000; // 2 sec
        const DEFAULT_RETRY_COUNT = 10;
        var polls = [];

        var service = {
            start: startPolling,
            stop: stopPolling,
            initPolling: initPolling
        };

        return service;

        function startPolling(name, url, pollingInterval, callback) {
            if (!polls[name]) {
                var poller = function () {
                    return $http.get(url).then(function (response) {
                        callback(response);
                    });
                }
            }
            poller();
            polls[name] = setInterval(poller, pollingInterval || DEFAULT_POLLING_INTERVAL);
        };

        function stopPolling(name) {
            clearInterval(polls[name]);
            delete polls[name];
        };

        function initPolling(name, responseUrl, pollingInterval, retryCount) {
            // poll the api endpoint for the response
            let promise = new Promise((resolve, reject) => {
                console.log('Polling started.');

                let _retryCount = 0;
                startPolling(name, responseUrl, pollingInterval || DEFAULT_POLLING_INTERVAL, response => {
                    if (!response.data) {
                        console.log('Polling callback response: ' + JSON.stringify(JSON.decycle(response)));

                        if (_retryCount === (retryCount || DEFAULT_RETRY_COUNT)) {
                            stopPolling(name);
                            reject('Polling canceled: retry count limit exceeded.');
                        } else {
                            _retryCount++;
                            console.warn('Retrying - count = ' + retryCount);
                        }
                    } else {
                        stopPolling(name);
                        resolve(response);
                    }
                });
            });

            return promise;
        }
    }
})();
