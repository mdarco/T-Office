(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('InstallmentsListDialogController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', '$uibModalInstance', 'ClientsService', 'UtilityService', 'FileGeneratorService', 'Blob', 'FileSaver', 'toastr', 'installments', 'context'];

    function ctrlFn($rootScope, $scope, $location, $uibModal, $uibModalInstance, ClientsService, UtilityService, FileGeneratorService, Blob, FileSaver, toastr, installments, context) {
        $scope.installments = installments;
        $scope.vehicle = context.Vehicle;
        $scope.vehicleRegistration = context.VehicleRegistration;
        $scope.client = context.Client;

        calculateTotalSum();
        function calculateTotalSum() {
            $scope.totalSum = 0;
            if (installments && installments.length > 0) {
                _.each(installments, installment => {
                    $scope.totalSum += installment.Amount;
                });
            }
        }

        calculateTotalPaid();
        function calculateTotalPaid() {
            $scope.totalPaid = 0;
            if ($scope.installments && $scope.installments.length > 0) {
                _.each($scope.installments, installment => {
                    if (installment.PaidAmount) {
                        $scope.totalPaid += parseFloat(installment.PaidAmount);
                    }
                });
            }
        }

        $scope.editInstallment = function (installment, dataField) {
            if (dataField === 'IsPaid' || dataField === 'IsAdminBan') {
                // boolean fields
                var msg = null;
                var newFieldValue = null;

                if (dataField === 'IsPaid') {
                    if (!installment.PaidAmount || installment.PaidAmount === '' || installment.PaidAmount === '0') {
                        toastr.warning('Nije uneta plaćena suma.');
                        return;
                    }

                    if (!installment.IsPaid) {
                        msg = 'Rata je plaćena?';
                    } else {
                        msg = 'Poništavate plaćanje rate?';
                    }

                    newFieldValue = !installment.IsPaid;
                } else {
                    if (!installment.IsAdminBan) {
                        msg = 'Aktivirati administrativnu zabranu?';
                    } else {
                        msg = 'Deaktivirati administrativnu zabranu?';
                    }

                    newFieldValue = !installment.IsAdminBan;
                }

                bootbox.confirm({
                    message: msg,
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
                    callback: function (confirmResult) {
                        if (confirmResult) {
                            var editObj = {};
                            editObj[dataField] = newFieldValue;

                            ClientsService.editVehicleRegistrationInstallment(context.ClientID, context.VehicleID, context.VehicleRegistrationID, installment.ID, editObj).then(
                                function () {
                                    toastr.success('Podatak uspešno ažuriran.');
                                    installment[dataField] = newFieldValue;

                                    if (newFieldValue) {
                                        installment.PaymentDate = new Date();
                                    } else {
                                        installment.PaymentDate = null;
                                    }
                                },
                                function (error) {
                                    toastr.error(error.statusText);
                                }
                            );
                        }
                    }
                });
            } else if (dataField === 'PaymentDate') {
                // date field
                if (!installment.IsPaid) {
                    toastr.warning('Rata nije plaćena.');
                    return;
                }

                openDateFieldDialog(dataField, installment[dataField]).then(
                    function (result) {
                        var editObj = {};
                        editObj[dataField] = UtilityService.convertDateToISODateString(result.Value);

                        ClientsService.editVehicleRegistrationInstallment(context.ClientID, context.VehicleID, context.VehicleRegistrationID, installment.ID, editObj).then(
                            function () {
                                toastr.success('Podatak uspešno ažuriran.');
                                installment[dataField] = result.Value;
                            },
                            function (error) {
                                toastr.error(error.statusText);
                            }
                        );
                    },
                    function (error) { }
                );
            } else {
                // text field
                openTextFieldDialog(dataField, installment[dataField]).then(
                    function (result) {
                        var editObj = {};
                        editObj[dataField] = result;

                        ClientsService.editVehicleRegistrationInstallment(context.ClientID, context.VehicleID, context.VehicleRegistrationID, installment.ID, editObj).then(
                            function () {
                                toastr.success('Podatak uspešno ažuriran.');

                                if (dataField === 'PaidAmount') {
                                    refreshInstallmentsList();
                                } else {
                                    installment[dataField] = result;
                                }
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

        $scope.printReceipt = function (installment) {
            if (!installment.PaidAmount || installment.PaidAmount === 0) {
                toastr.warning('Nije uneta plaćena suma.');
                return;
            }

            var model = {
                Owner: $scope.client.FullOwnerName,
                User: $scope.client.FullUserName,
                Vehicle: `[${$scope.vehicle.RegistrationNumber}] ${$scope.vehicle.Make} ${$scope.vehicle.Model}`,
                InstallmentAmount: installment.Amount,
                InstallmentPaidAmount: installment.PaidAmount,
                PaymentDate: installment.PaymentDate
            };

            FileGeneratorService.createFileFromTemplate(null, model).then(
                (response) => {
                    if (response && response.data) {
                        var file = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
                        FileSaver.saveAs(file, 'potvrda-placanja.docx');
                    }
                },
                (error) => {
                    toastr.error(error.statusText);
                }
            );
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

        function openDateFieldDialog(dataField, date) {
            var dialogOpts = {
                backdrop: 'static',
                keyboard: false,
                backdropClick: false,
                templateUrl: 'pages/common/date-dialog/date-dialog.html',
                controller: 'DateDialogController',
                resolve: {
                    settings: function () {
                        return {
                            DisplayTitle: 'T-Office',
                            LabelTitle: 'Datum plaćanja',
                            DateValue: _.isDate(date) ? date : UtilityService.convertISODateStringToDate(date)
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

        function refreshInstallmentsList() {
            ClientsService.getVehicleRegistrationInstallments(context.ClientID, context.VehicleID, context.VehicleRegistrationID).then(response => {
                if (response && response.data) {
                    $scope.installments = response.data;
                    calculateTotalPaid();
                } else {
                    $scope.installments = [];
                }
            }).catch(error => {
                toastr.error('Došlo je do greške prilikom osvežavanja spiska rata.\nZatvorite dijalog, pa pokušajte ponovo.');
            });
        }
    }
})();
