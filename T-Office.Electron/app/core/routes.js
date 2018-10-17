(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .config(routeFn);

    routeFn.$inject = ['$routeProvider'];

    function routeFn($routeProvider) {
        $routeProvider
            //.when('/login',
            //    {
            //        controller: 'LoginController',
            //        templateUrl: 'pages/login/login.html?nd=' + Date.now()
            //    }
            //)

            .when('/home',
                {
                    controller: 'HomeController',
                    templateUrl: 'pages/home/home.html?nd=' + Date.now()
                }
            )

            .when('/clients',
                {
                    controller: 'ClientsController',
                    templateUrl: 'pages/clients/clients.html?nd=' + Date.now()
                }
            )

            .when('/full-client-entry',
                {
                    controller: 'FullClientEntryController',
                    templateUrl: 'pages/clients/full-client-entry/full-client-entry.html?nd=' + Date.now()
                }
            )

            .when('/client-file/:id',
                {
                    controller: 'ClientFileController',
                    templateUrl: 'pages/client-file/client-file.html?nd=' + Date.now(),
                    resolve: {
                        client: function ($route, ClientsService, toastr) {
                            var id = $route.current.params.id;
                            return ClientsService.getClient(id).then(
                                function (result) {
                                    return result.data;
                                },
                                function (error) {
                                    toastr.error(error.statusText);
                                }
                            );
                        }
                    }
                }
            )

            .when('/reports/costs-by-period',
                {
                    controller: 'CostsByPeriodController',
                    templateUrl: 'pages/reports/costs-by-period/costs-by-period.html?nd=' + Date.now()
                }
            )

            .otherwise({ redirectTo: '/home' });
    }
})();
