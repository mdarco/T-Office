(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('CostsByPeriodController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$window', '$filter', '$uibModal', 'ClientsService', 'UtilityService', 'PdfService', 'Blob', 'FileSaver', 'toastr' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $window, $filter, $uibModal, ClientsService, UtilityService, PdfService, Blob, FileSaver, toastr /* , AuthenticationService */) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuReport_CostsByPeriod").addClass("active");

        //var currentUser = AuthenticationService.getCurrentUser();

        $scope.filter = {};
        $scope.costsByPeriod = [];

        $scope.totalCredit = 0;
        $scope.totalDebt = 0;
        $scope.totalPaid = 0;

        $scope.applyFilter = function () {
            getCostsByPeriod();
        };

        $scope.clearFilter = function () {
            $scope.filter = {};
            $scope.costsByPeriod = [];

            $scope.totalCredit = 0;
            $scope.totalDebt = 0;
            $scope.totalPaid = 0;
        };

        function getCostsByPeriod() {
            if ($scope.filter.DateFrom_Temp) {
                $scope.filter.DateFrom = UtilityService.convertDateToISODateString($scope.filter.DateFrom_Temp);
            }

            console.log('DateFrom: ' + $scope.filter.DateFrom);

            if ($scope.filter.DateTo_Temp) {
                $scope.filter.DateTo = UtilityService.convertDateToISODateString($scope.filter.DateTo_Temp);
            }

            console.log('DateFrom: ' + $scope.filter.DateTo);

            ClientsService.getCostsByPeriod($scope.filter).then(
                (result) => {
                    if (result && result.data) {
                        $scope.costsByPeriod = result.data;
                        calculateTotals();
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );

            ClientsService.getTotalPaidAmountByPeriod($scope.filter).then(
                (result) => {
                    if (result && result.data) {
                        console.log(result.data);
                        $scope.totalPaid = result.data;
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
        }

        function calculateTotals() {
            $scope.totalCredit = 0;
            $scope.totalDebt = 0;

            _.each($scope.costsByPeriod, (cost) => {
                $scope.totalCredit += cost.TotalCreditAmount;

                if (cost.TotalDebtAmount > 0) {
                    $scope.totalDebt += cost.TotalDebtAmount;
                }
            });
        }

        $scope.openClientDossier = function (cost) {
            $location.path('/client-file/' + cost.ClientID);
        };

        function openPdf(pdfData) {
            var file = new Blob([pdfData], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            $window.open(fileURL);
        }

        $scope.print = function () {
            var headerData = ['Klijent', 'Vozilo', 'Suma', 'Dugovanje'];

            var columnData = [];
            var totalDebt = 0.0;
            _.each($scope.costsByPeriod, item => {
                totalDebt += (item.TotalDebtAmount > 0) ? item.TotalDebtAmount : 0;
                var column = [
                    'Vlasnik: ' + item.VehicleOwner + '\nKorisnik: ' + item.VehicleUser,
                    '[ ' + item.RegistrationNumber + ' ] ' + item.VehicleModel,
                    $filter('currency')(item.TotalCreditAmount, '', 2),
                    (item.TotalDebtAmount > 0) ? $filter('currency')(item.TotalDebtAmount, '', 2) : '0.0'
                ];
                columnData.push(column);
            });

            // total debt summary
            columnData.push([
                '', '',
                'Ukupno dugovanje: ' + $filter('currency')(totalDebt, '', 2)
            ]);

            var dateFrom_Display = !$scope.filter.DateFrom_Temp ? '' : moment($scope.filter.DateFrom_Temp).format('DD.MM.YYYY');
            var dateTo_Display = !$scope.filter.DateTo_Temp ? '' : moment($scope.filter.DateTo_Temp).format('DD.MM.YYYY');

            var model = {
                Title: 'Troškovi za period (' + dateFrom_Display + ' - ' + dateTo_Display + ')',
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

        // date picker support
        $scope.datePickers = {};
        $scope.openDatePicker = function (pickerFor, event) {
            event.preventDefault();
            event.stopPropagation();

            $scope.datePickers['datePickerOpened_' + pickerFor] = true;
        };
    }
})();
