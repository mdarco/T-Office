(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .controller('ClientsController', ctrlFn);

    ctrlFn.$inject = ['$rootScope', '$scope', '$location', '$uibModal'];

    function ctrlFn($rootScope, $scope, $location, $uibModal) {
        // set active menu item
        $("#left-panel nav ul li").removeClass("active");
        $("#menuClients").addClass("active");
    }
})();
