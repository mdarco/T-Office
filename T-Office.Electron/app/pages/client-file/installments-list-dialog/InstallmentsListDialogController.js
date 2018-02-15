(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('InstallmentsListDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', '$uibModalInstance', 'ClientsService', 'toastr', 'installments', 'context'];

    function ctrlFn($rootScope, $scope, $location, $uibModal, $uibModalInstance, ClientsService, toastr, installments, context) {
        $scope.installments = installments;

        $scope.editInstallment = function (installment, dataField) {
            if (dataField === 'IsPaid' || dataField === 'IsAdminBan') {
                // boolean fields

            } else if (dataField === 'PaymentDate') {
                // date field

            } else {
                // text field
                openTextFieldDialog(dataField, installment[dataField]).then(
                    function (result) {
                        var editObj = {};
                        editObj[dataField] = result;

                        ClientsService.editVehicleRegistrationInstallment(context.ClientID, context.VehicleID, context.VehicleRegistrationID, installment.ID, editObj).then(
                            function () {
                                toastr.success('Podatak uspešno ažuriran.');
                                installment[dataField] = result;
                            },
                            function (error) {
                                toastr.error(error.statusText);
                            }
                        );
                    },
                    function (error) { }
                );
            }
        };

        $scope.resolveStatusCssClass = function (installment) {
            if (installment.IsPaid) {
                return 'label label-success';
            }

            if (!installment.IsPaid) {
                var today = moment(Date.now());
                var installmentDate = moment(installment.InstallmentDate);

                if (installmentDate > today) {
                    return 'label label-info';
                } else {
                    return 'label label-danger';
                }
            }
        };

        $scope.close = function () {
            $uibModalInstance.dismiss();
        };

        function openTextFieldDialog(dataField, text) {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/common/text-field-dialog/text-field-dialog.html',
                controller: 'TextFieldDialogController',
                resolve: {
                    settings: function () {
                        return {
                            FieldValue: text,
                            DisplayTitle: 'T-Office',
                            FieldLabel: resolveDataFieldLabel(dataField)
                        };
                    }
                }
            };

            var dialog = $uibModal.open(dialogOpts);

            return dialog.result;
        }

        function resolveDataFieldLabel(dataField) {
            switch (dataField) {
                case 'PaymentDate':
                    return 'Datum plaćanja';

                case 'Note':
                    return 'Komentar';

                default:
                    return '';
            }
        }
    }
})();
