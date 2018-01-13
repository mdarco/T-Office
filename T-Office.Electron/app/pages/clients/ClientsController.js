(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal'];

    function ctrlFn($rootScope, $scope, $location, $uibModal) {
        // set active menu item
        angular.element("#left-panel nav ul li").removeClass("active");
        angular.element("#menuClients").addClass("active");
    }
})();
