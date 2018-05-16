(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('HomeController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$window', '$filter', '$uibModal', 'ClientsService', 'PdfService', 'Blob', 'FileSaver', 'toastr' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $window, $filter, $uibModal, ClientsService, PdfService, Blob, FileSaver, toastr /* , AuthenticationService */) {
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

        $scope.totalUnpaidInstallmentsAmount = 0;
        getTotalUnpaidInstallmentsAmount();

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

        function getTotalUnpaidInstallmentsAmount() {
            //var filter = { IsPaid: false };
            ClientsService.getTotalInstallmentsAmount({}).then(
                (result) => {
                    if (result && result.data) {
                        $scope.totalUnpaidInstallmentsAmount = result.data;
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

        function openPdf(pdfData) {
            var file = new Blob([pdfData], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            $window.open(fileURL);
        }

        $scope.printClientsDue = function () {
            var headerData = ['Datum dospeća', 'Suma', 'Klijent', 'Vozilo', 'Datum registracije'];

            var columnData = [];
            _.each($scope.clientsDue, item => {
                var column = [
                    $filter('date')(item.InstallmentDate, 'dd.MM.yyyy'),
                    $filter('currency')(item.InstallmentAmount, '', 2),
                    'Vlasnik: ' + item.FullOwnerName + '\nKorisnik: ' + item.FullUserName,
                    item.FullVehicleName,
                    $filter('date')(item.RegistrationDate, 'dd.MM.yyyy')
                ];
                columnData.push(column);
            });

            var model = {
                Title: 'Dospeće rata u narednih ' + $scope.CLIENTS_DUE_NUMBER_OF_DAYS + ' dana',
                HeaderData: headerData,
                ColumnData: columnData
            };

            PdfService.createSimplePdf(model).then(
                (response) => {
                    openPdf(response.data);
                },
                (error) => {
                    toastr.error('Došlo je do greške prilikom generisanja PDF dokumenta.');
                    toastr.error(error.statusText);
                }
            );
        };

        $scope.printClientsOutstandingTotal = function () {
            var headerData = ['Klijent', 'Ukupna suma dugovanja'];

            var columnData = [];
            _.each($scope.clientsOutstandingTotal, item => {
                var column = [
                    'Vlasnik: ' + item.Owner + '\nKorisnik: ' + item.User,
                    $filter('currency')(item.TotalAmount, '', 2)
                ];
                columnData.push(column);
            });

            var model = {
                Title: 'Dospele rate',
                HeaderData: headerData,
                ColumnData: columnData
            };

            PdfService.createSimplePdf(model).then(
                (response) => {
                    openPdf(response.data);
                },
                (error) => {
                    toastr.error('Došlo je do greške prilikom generisanja PDF dokumenta.');
                    toastr.error(error.statusText);
                }
            );
        };

        $scope.printIncomingRegistrations = function () {
            var headerData = ['Klijent', 'Vozilo', 'Datum isteka registracije'];

            var columnData = [];
            _.each($scope.clientsDue, item => {
                var column = [
                    'Vlasnik: ' + item.FullOwnerName + '\nKorisnik: ' + item.FullUserName,
                    item.FullVehicleName,
                    $filter('date')(item.NextRegistrationDate, 'dd.MM.yyyy')
                ];
                columnData.push(column);
            });

            var model = {
                Title: 'Istek registracija u narednih ' + $scope.INCOMING_REGISTRATIONS_NUMBER_OF_DAYS + ' dana',
                HeaderData: headerData,
                ColumnData: columnData
            };

            PdfService.createSimplePdf(model).then(
                (response) => {
                    openPdf(response.data);
                },
                (error) => {
                    toastr.error('Došlo je do greške prilikom generisanja PDF dokumenta.');
                    toastr.error(error.statusText);
                }
            );
        };
    }
})();
