(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$scope', '$location', '$uibModal', '$q', 'NgTableParams', 'ClientsService', 'UtilityService', 'RegLicenseReaderService', 'PollingService', 'AgentDataService', 'toastr', 'blockUI'];

    function ctrlFn($scope, $location, $uibModal, $q, NgTableParams, ClientsService, UtilityService, RegLicenseReaderService, PollingService, AgentDataService, toastr, blockUI) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuClients").addClass("active");

        // prevent 'Enter' to submit the form
        $('#searchForm').bind('keydown', function (e) {
            if (e.keyCode === 13) {
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
                        blockUI.start();

                        var agentId = sessionStorage.getItem('tofficeAgentId');
                        console.log('Agent ID for RegLicenseReader service [ClientsController]: ' + agentId);

                        AgentDataService.get(agentId).then(agentData => {
                            console.log('Agent data for RegLicenseReader service [ClientsController]: ' + JSON.stringify(JSON.decycle(agentData.data)));
                            RegLicenseReaderService.readSmartCardData(agentData.data.WsConnectionId).then(
                                function () {
                                    var responseUrl = RegLicenseReaderService.getSmartCardResponseUrl(agentData.data.WsConnectionId);
                                    PollingService.initPolling('smartCardReader_' + Date.now(), responseUrl)
                                        .then(smartCardResponse => {
                                            console.log('%cPolling response successful.', 'color: green;');
                                            console.log(smartCardResponse);

                                            RegLicenseReaderService.deleteSmartCardResponse(agentData.data.WsConnectionId)
                                                .then((/* isDeleted */) => { // isDeleted ? console.log('Smart card response deleted.') : console.log('Smart card response NOT deleted.');
                                                })
                                                .catch(err => {
                                                    console.error('Error deleting smart card response: ' + err.statusText);
                                                });

                                            // use smart card data to insert new client
                                            if (smartCardResponse.IsError) {
                                                toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                                toastr.error(smartCardResponse.ErrorMessage);
                                                blockUI.stop();
                                                return;
                                            } else {
                                                console.log('Trying to insert new client..');
                                                const smartCardData = JSON.parse(smartCardResponse.data.Data);
                                                const clientData = smartCardData.Result;

                                                ClientsService.getExistingClients(clientData).then(
                                                    function (existingClientsArray) {
                                                        const existingClients = ClientsService.formatExistingClients(existingClientsArray);
                                                        console.log('List of existing clients obtained:');
                                                        console.log(existingClients);
                                                        blockUI.stop();
                                                        insertClient(clientData, existingClients || []);
                                                    },
                                                    function (error) {
                                                        console.warn('List of existing clients cannot be obtained - inserting will be checked in the backend.');
                                                        blockUI.stop();
                                                        insertClient(clientData, []);
                                                    }
                                                );
                                            }
                                        })
                                        .catch(errorMsg => {
                                            console.error(errorMsg);
                                            blockUI.stop();
                                        });
                                },
                                function (error) {
                                    console.error('Read smart card data error:');
                                    console.error(error);
                                    toastr.error('Došlo je do greške prilikom čitanja saobraćajne dozvole.');
                                    toastr.error('[GREŠKA] --> ' + error.statusText);
                                    blockUI.stop();
                                    return;
                                }
                            );
                        });
                    }
                }
            });
        };

        function insertClient(data, existingClients) {
            let dialogHtml = ClientsService.createInsertClientDialogHtml(data, existingClients);
            let model = ClientsService.createInsertClientDataModel(data);

            //console.log('Dialog HTML:');
            //console.log(dialogHtml);

            //console.log('Data model:');
            //console.log(model);

            var dlg = bootbox.dialog({
                size: 'large',
                title: '<span style="font-weight: bold;">Klijent</span>',
                message: dialogHtml,
                buttons: {
                    cancel: {
                        label: 'Odustani',
                        className: 'btn-primary'
                    },
                    addClient: {
                        label: 'Upiši novog klijenta',
                        className: 'btn-success',
                        callback: function () {
                            ClientsService.addClientFull(model).then(
                                function () {
                                    toastr.success('Novi klijent uspešno upisan.');
                                },
                                function (error) {
                                    toastr.error('Došlo je do greške prilikom upisa novog klijenta.');
                                    toastr.error('[GREŠKA] --> ' + error.statusText);
                                    return false;
                                }
                            );
                        }
                    }
                }
            });
        }

        $scope.deleteClient = function (client) {
            bootbox.confirm({
                message: "Klijent i svi njegovi podaci biće obrisani. Da li ste sigurni?",
                buttons: {
                    confirm: {
                        label: 'Da',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'Ne',
                        className: 'btn-primary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        ClientsService.deleteClient(client.ID).then(() => {
                            $scope.applyFilter();
                        }).catch(error => {
                            toastr.error('Došlo je do greške na serveru prilikom brisanja klijenta.');
                        });
                    }
                }
            });
        };

        $scope.addClientManually = function () {
            var dialogOpts = {
                //size: 'lg',
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/clients/client-dialog/client-dialog.html',
                controller: 'ClientDialogController'//,
                //resolve: {
                //    ageCategories: function (LookupsService) {
                //        return LookupsService.getAgeCategories().then(
                //            function (result) {
                //                return result.data;
                //            }
                //        );
                //    }
                //}
            };

            var dialog = $uibModal.open(dialogOpts);

            dialog.result.then(
                function () {
                    $scope.applyFilter();
                },
                function () {
                    // modal dismissed => do nothing
                }
            );
        };

        $scope.openClientDossier = function (client) {
            $location.path('/client-file/' + client.ID);
        };
    }
})();
