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

            //.when('/members',
            //    {
            //        controller: 'MembersController',
            //        templateUrl: 'pages/members/members.html?nd=' + Date.now(),
            //        resolve: {
            //            choreos: function (ChoreosService) {
            //                return ChoreosService.getLookup().then(
            //                    function (result) {
            //                        return result.data;
            //                    }
            //                );
            //            },
            //            danceGroups: function (DanceGroupsService) {
            //                return DanceGroupsService.getLookup().then(
            //                    function (result) {
            //                        return result.data;
            //                    }
            //                );
            //            },
            //            danceSelections: function (DanceSelectionsService) {
            //                return DanceSelectionsService.getLookup().then(
            //                    function (result) {
            //                        return result.data;
            //                    }
            //                );
            //            },
            //            events: function (EventsService) {
            //                return EventsService.getLookup().then(
            //                    function (result) {
            //                        return result.data;
            //                    }
            //                );
            //            }
            //        },
            //        access: {
            //            loginRequired: true
            //        }
            //    }
            //)

            //.when('/member-file/:id',
            //    {
            //        controller: 'MemberFileController',
            //        templateUrl: 'pages/members/member-file/member-file.html?nd=' + Date.now(),
            //        resolve: {
            //            member: function ($route, MembersService) {
            //                var id = $route.current.params.id;
            //                return MembersService.get(id).then(
            //                    function (result) {
            //                        return result.data;
            //                    }
            //                );
            //            }
            //        },
            //        access: {
            //            loginRequired: true
            //        }
            //    }
            //)

            .otherwise({ redirectTo: '/home' });
    }
})();
