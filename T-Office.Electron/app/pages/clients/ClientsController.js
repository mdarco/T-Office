﻿(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'NgTableParams', 'ClientsService', 'RegLicenseReaderService', 'toastr'];

    function ctrlFn($rootScope, $scope, $location, $uibModal, NgTableParams, ClientsService, RegLicenseReaderService, toastr) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuClients").addClass("active");

        // prevent 'Enter' to submit the form
        $('#searchForm').bind('keydown', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
            }
        });

        $scope.filter = {};
        $scope.totalRecords = 0;
        $scope.initialDocListLoad = true;
        $scope.showGrid = false;

        $scope.clientsTableParams = new NgTableParams(
            {
                page: 1,
                count: 10
            },
            {
                total: 0,
                getData: function (params) {
                    if ($scope.initialDocListLoad) {
                        $scope.initialDocListLoad = false;
                        return [];
                    }

                    $scope.filter = $scope.filter || {};

                    $scope.filter.PageNo = params.page();
                    $scope.filter.RecordsPerPage = params.count();

                    return ClientsService.getFiltered($scope.filter).then(
                        function (result) {
                            if (!result || !result.data || !result.data.Data) {
                                $scope.showGrid = false;
                                return [];
                            }

                            if (result.data.Data.length > 0) {
                                $scope.showGrid = true;
                            } else {
                                $scope.showGrid = false;
                            }

                            params.total(result.data.Total);
                            $scope.totalRecords = result.data.Total;

                            return result.data.Data;
                        },
                        function (error) {
                            toastr.error('Došlo je do greške prilikom pretraživanja.');
                            $scope.showGrid = false;
                        }
                    );
                }
            }
        );

        $scope.applyFilter = function () {
            $scope.clientsTableParams.reload();
        };

        $scope.clearFilter = function () {
            $scope.filter = {};
            $scope.showGrid = false;
            $scope.totalRecords = 0;
            $scope.clientsTableParams.data = {};
        };

        $scope.addClient = function () {
            RegLicenseReaderService.readData().then(
                function (result) {
                    if (result && result.data) {
                        var data = JSON.parse(result.data);
                        alert(data);
                    } else {
                        toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                        return;
                    }
                },
                function (error) {
                    toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                    toastr.error('[GREŠKA] --> ' + error.statusText);
                    return;
                }
            );
        };

        $scope.openClientDossier = function (client) {
            $location.path('/client-file/' + client.ID);
        };
    }
})();
