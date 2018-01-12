﻿(function () {
    'use strict';

    angular
        .module('DFApp')
        .controller('TableSelectDialogController', ctrlFn);

    ctrlFn.$inject = ['$scope', '$uibModalInstance', 'toastr', 'tableStructure', 'tableData', 'additionalChkboxCol', 'tableSelectedData'];

    function ctrlFn($scope, $uibModalInstance, toastr, tableStructure, tableData, additionalChkboxCol, tableSelectedData) {
        $scope.viewSettings = {};

        $scope.additionalCol = additionalChkboxCol;

        $scope.chkState = {
            selectAll: false
        };

        $scope.selectedData = [];
        $scope.tableStructure = tableStructure;
        $scope.tableData = _.isArray(tableData.data) ? tableData.data : ((tableData.data.Data && _.isArray(tableData.data.Data)) ? tableData.data.Data : []);

        _.each($scope.tableData, function (dataItem) {
            dataItem.selected = false;

            if ($scope.additionalCol) {
                dataItem.selectedAdditionalCol = false;
            }
        });

        var selectedData = [];
        if (tableSelectedData && _.isArray(tableSelectedData) && tableSelectedData.length > 0) {
            selectedData = tableSelectedData;
        } else if (tableSelectedData.data && _.isArray(tableSelectedData.data) && tableSelectedData.data.length > 0) {
            selectedData = tableSelectedData.data;
        }

        _.each(selectedData, function (selectedItem) {
            var selectedItemID = selectedItem[tableStructure.idCol];
            var tableDataItem = _.find($scope.tableData, function (dataItem) {
                return (parseInt(dataItem[tableStructure.idCol]) === parseInt(selectedItemID));
            });
            if (tableDataItem) {
                tableDataItem.selected = true;

                if ($scope.additionalCol) {
                    if (selectedItem.selectedAdditionalCol) {
                        tableDataItem.selectedAdditionalCol = selectedItem.selectedAdditionalCol;
                    } else {
                        if ($scope.additionalCol.dataSource) {
                            tableDataItem.selectedAdditionalCol = selectedItem[$scope.additionalCol.dataSource];
                        }
                    }
                }

                $scope.selectedData.push(tableDataItem);
            }
        });

        $scope.chkState.selectAll = (_.map($scope.selectedData, { selected: true })).length === $scope.tableData.length;

        $scope.selectRow = function (dataItem) {
            if (dataItem.selected) {
                // add data item to selected list
                $scope.selectedData.push(dataItem);
            } else {
                // remove data item from selected list
                var removeObj = {};
                removeObj[tableStructure.idCol] = dataItem[tableStructure.idCol];
                _.pullAllBy($scope.selectedData, [removeObj], tableStructure.idCol);
                $scope.chkState.selectAll = false;
            }
        };

        $scope.selectAll = function () {
            _.each($scope.tableData, function (dataItem) {
                if ($scope.chkState.selectAll === true) {
                    if (!dataItem.selected) {
                        dataItem.selected = true;
                        $scope.selectRow(dataItem);
                    }
                } else {
                    if (dataItem.selected === true) {
                        dataItem.selected = false;
                        $scope.selectRow(dataItem);
                    }
                }
            });
        };

        $scope.save = function () {
            $uibModalInstance.close($scope.selectedData);
        };

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };

        $scope.resolveAdditionalCol = function () {
            if (additionalChkboxCol) {
                return true;
            }
            return false;
        };
    }
})();
