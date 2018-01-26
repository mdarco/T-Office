(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', '$q', 'NgTableParams', 'ClientsService', 'RegLicenseReaderService', 'toastr'];

    function ctrlFn($rootScope, $scope, $location, $uibModal, $q, NgTableParams, ClientsService, RegLicenseReaderService, toastr) {
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
            bootbox.confirm({
                message: "<span style='font-weight: bold; font-size: large;'>Stavite saobraćajnu dozvolu u čitač.</span>",
                buttons: {
                    confirm: {
                        label: 'Ok',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        RegLicenseReaderService.readData().then(
                            function (result) {
                                if (result && result.data) {
                                    var data = JSON.parse(result.data);
                                    if (data.IsError) {
                                        //toastr.error('[GREŠKA] --> ' + data.ErrorMessage);
                                        toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                        return;
                                    } else {
                                        var filters_ownerName = data.Result.PersonalData.ownerName.split(' ');
                                        filters_ownerName = _.filter(filters_ownerName, function (item) {
                                            return item.replace(/\0/g, '').length > 3; // eliminates null strings
                                        });

                                        var filters_ownersSurnameOrBusinessName = data.Result.PersonalData.ownersSurnameOrBusinessName.split(' ');
                                        filters_ownersSurnameOrBusinessName = _.filter(filters_ownersSurnameOrBusinessName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var filters_usersName = data.Result.PersonalData.usersName.split(' ');
                                        filters_usersName = _.filter(filters_usersName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var filters_usersSurnameOrBusinessName = data.Result.PersonalData.usersSurnameOrBusinessName.split(' ');
                                        filters_usersSurnameOrBusinessName = _.filter(filters_usersSurnameOrBusinessName, function (item) {
                                            return item.replace(/\0/g, '').length > 3;
                                        });

                                        var clientNameFilters = [
                                            ...filters_ownerName, ...filters_ownersSurnameOrBusinessName,
                                            ...filters_usersName, ...filters_usersSurnameOrBusinessName
                                        ];

                                        getExistingClients(clientNameFilters).then(
                                            function (existingClients) {
                                                insertClient(data.Result, existingClients || []);
                                            },
                                            function (error) {
                                                insertClient(data.Result, []);
                                            }
                                        );
                                    }
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
                    }
                }
            });
        };

        function insertClient(data, existingClients) {
            var dialogHtml = `
                <table class="table table-condensed table-striped">
                    <tbody>
                        <tr>
                            <td style="color: blue;">Vlasnik</td>
                            <td>${data.PersonalData.ownersName || ''} ${data.PersonalData.ownersSurnameOrBusinessName || ''}, ${data.PersonalData.ownerAddress || ''}</td>
                        </tr>
                        <tr>
                            <td style="color: blue;">Korisnik</td>
                            <td>${data.PersonalData.usersName || ''} ${data.PersonalData.usersSurnameOrBusinessName || ''}, ${data.PersonalData.usersAddress || ''}</td>
                        </tr>
                        <tr>
                            <td style="color: blue;">Vozilo</td>
                            <td>${data.VehicleData.vehicleMake || ''} ${data.VehicleData.commercialDescription || ''} (${data.VehicleData.registrationNumberOfVehicle || ''})</td>
                        </tr>
                    </tbody>
                </table>

                <br /><br />

                <table class="table table-condensed table-striped">
                    <caption>Postojeći klijenti</caption>
                    <tbody>
            `;

            _.each(existingClients, function (existingClient) {
                dialogHtml += '<tr><td>' + existingClient.FullName + '</td></tr>';
            });

            dialogHtml += `
                    </tbody>
                </table>
            `;

            bootbox.dialog({
                title: '<span style="font-weight: bold;">Novi klijent</span>',
                message: dialogHtml,
                buttons: {
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    },
                    insertOwner: {
                        label: 'Upiši vlasnika kao klijenta',
                        className: 'btn-success',
                        callback: {
                            // TODO: insert owner as client
                        }
                    },
                    insertUser: {
                        label: 'Upiši korisnika kao klijenta',
                        className: 'btn-success',
                        callback: {
                            // TODO: insert user as client
                        }
                    }
                }
            });
        }

        $scope.addClientManually = function () {
            alert('U razvoju..');
        };

        $scope.openClientDossier = function (client) {
            $location.path('/client-file/' + client.ID);
        };
    }
})();
