(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('CostsByPeriodController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal', 'ClientsService', 'toastr' /* , 'AuthenticationService' */];

    function ctrlFn($rootScope, $scope, $location, $uibModal, ClientsService, toastr /* , AuthenticationService */) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuReport_CostsByPeriod").addClass("active");

        //var currentUser = AuthenticationService.getCurrentUser();
    }
})();
