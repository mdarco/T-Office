(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('CostsByPeriodController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'UtilityService', 'toastr' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, UtilityService, toastr /* , AuthenticationService */) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuReport_CostsByPeriod").addClass("active");

        //var currentUser = AuthenticationService.getCurrentUser();

        $scope.filter = {};
        $scope.costsByPeriod = [];

        $scope.totalCredit = 0;
        $scope.totalDebt = 0;
        $scope.totalInstallmentsPaid = 0;

        $scope.applyFilter = function () {
            getCostsByPeriod();
            getTotalInstallmentsPaid();
        };

        $scope.clearFilter = function () {
            $scope.filter = {};
            $scope.costsByPeriod = [];

            $scope.totalCredit = 0;
            $scope.totalDebt = 0;
            $scope.totalInstallmentsPaid = 0;
        };

        function getTotalInstallmentsPaid() {
            var model = {
                IsPaid: true
            };

            if ($scope.filter.DateFrom_Temp) {
                model.StartDate = UtilityService.convertDateToISODateString($scope.filter.DateFrom_Temp);
            }

            if ($scope.filter.DateTo_Temp) {
                model.EndDate = UtilityService.convertDateToISODateString($scope.filter.DateTo_Temp);
            }

            ClientsService.getTotalInstallmentsAmount(model).then(
                (result) => {
                    if (result && result.data) {
                        $scope.totalInstallmentsPaid = result.data;
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
        }

        function getCostsByPeriod() {
            if ($scope.filter.DateFrom_Temp) {
                $scope.filter.DateFrom = UtilityService.convertDateToISODateString($scope.filter.DateFrom_Temp);
            }

            if ($scope.filter.DateTo_Temp) {
                $scope.filter.DateTo = UtilityService.convertDateToISODateString($scope.filter.DateTo_Temp);
            }

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
        }

        function calculateTotals() {
            $scope.totalCredit = 0;
            $scope.totalDebt = 0;

            _.each($scope.costsByPeriod, (cost) => {
                $scope.totalCredit += cost.TotalCreditAmount;
                $scope.totalDebt += cost.TotalDebtAmount;
            });
        }

        $scope.openClientDossier = function (cost) {
            $location.path('/client-file/' + cost.ClientID);
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
