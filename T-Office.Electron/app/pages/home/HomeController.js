﻿(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('HomeController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'toastr' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, toastr /* , AuthenticationService */) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuHome").addClass("active");

        //var currentUser = AuthenticationService.getCurrentUser();

        $scope.CLIENTS_DUE_NUMBER_OF_DAYS = 10;
        $scope.INCOMING_REGISTRATIONS_NUMBER_OF_DAYS = 10;

        $scope.clientsDue = [];
        getClientsDue();

        $scope.clientsOutstandingTotal = [];
        getClientsOutstandingTotal();

        $scope.incomingRegistrations = [];
        getIncomingRegistrations();

        function getClientsDue() {
            ClientsService.getClientsDue($scope.CLIENTS_DUE_NUMBER_OF_DAYS).then(
                (result) => {
                    if (result && result.data) {
                        $scope.clientsDue = result.data;
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
        }

        function getClientsOutstandingTotal() {
            ClientsService.getClientsOutstandingTotal().then(
                (result) => {
                    if (result && result.data) {
                        $scope.clientsOutstandingTotal = result.data;
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
        }

        function getIncomingRegistrations() {
            ClientsService.getIncomingRegistrations($scope.INCOMING_REGISTRATIONS_NUMBER_OF_DAYS).then(
                (result) => {
                    if (result && result.data) {
                        $scope.incomingRegistrations = result.data;
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
        }

        $scope.openClientDossier = function (client) {
            $location.path('/client-file/' + client.ClientID);
        };
    }
})();
