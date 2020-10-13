(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .factory('PollingService_v2', serviceFn);

    serviceFn.$inject = ['$q', '$http', '$interval'];

    function serviceFn($q, $http, $interval) {
        var DEFAULT_RETRY_INTERVAL = 1000; // 1 sec
        var DEFAULT_RETRY_COUNT = 10;
        var DEFAULT_TIMEOUT_INTERVAL = 5 * 60 * 1000; // 5 min

        var currentlyRunningRequest = null;

        var service = {
            getResult: getResult,
            cancel: cancel
        };

        return service;

        function getResult(url, retryInterval, retryCount, timeoutInterval) {
            var deferred = $q.defer();

            cancel(); // Cancel any previous requests

            var _retryCount = 0;

            var getData = function () {
                $http.get(url).then(
                    function (data) {
                        if (data) {
                            cancel();
                            deferred.resolve(data);
                        } else if (hasTimedOut()) {
                            deferred.reject('Time out');
                        } else if (_retryCount === (retryCount || DEFAULT_RETRY_COUNT)) {
                            deferred.reject('Retry count exceeded');
                        } else {
                            _retryCount++;
                            currentlyRunningRequest = $interval(getData, retryInterval || DEFAULT_RETRY_INTERVAL, 1);
                        }
                    }, function (error) {
                        deferred.reject(error);
                    }
                );
            };
            getData();

            var startTime = new Date().getTime();

            var hasTimedOut = function () {
                var endTime = new Date().getTime();
                return (endTime - startTime) > (timeoutInterval || DEFAULT_TIMEOUT_INTERVAL);
            };

            return deferred.promise;
        };

        function cancel() {
            if (currentlyRunningRequest) {
                $interval.cancel(currentlyRunningRequest);
            }
            currentlyRunningRequest = null;
        };
    }
})();
