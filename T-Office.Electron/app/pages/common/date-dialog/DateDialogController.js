﻿(function () {
    'use strict';

    angular
        .module('DFApp')
        .controller('DateDialogController', ctrlFn);

    ctrlFn.$inject = ['$scope', '$uibModalInstance', 'toastr', 'settings'];

    function ctrlFn($scope, $uibModalInstance, toastr, settings) {
        $scope.viewSettings = settings;

        $scope.dateObj = {};

        if (settings.DateValue) {
            $scope.dateObj.Value = settings.DateValue;
        }

        $scope.save = function () {
            var modelValidation = validate();
            if (modelValidation.error) {
                toastr.warning(modelValidation.errorMsg);
                return;
            }

            $uibModalInstance.close($scope.dateObj);
        };

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };

        // helpers
        function validate() {
            if (!$scope.dateObj.Value || ($scope.dateObj.Value && $scope.dateObj.Value === '')) {
                return { error: true, errorMsg: sprintf('%s je obavezan podatak.', $scope.viewSettings.LabelTitle) };
            }

            return { error: false };
        }

        // date picker support
        $scope.datePickers = {};
        $scope.openDatePicker = function (pickerFor, event) {
            event.preventDefault();
            event.stopPropagation();

            $scope.datePickers['datePickerOpened_' + pickerFor] = true;
        };
    }
})();
