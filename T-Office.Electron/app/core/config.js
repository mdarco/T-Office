﻿(function () {
    'use strict';

    angular
        .module('DFApp')
        .config(configFn);

    configFn.$inject = ['$locationProvider', 'blockUIConfig', 'tagsInputConfigProvider', 'httpRequestInterceptorCacheBusterProvider'];

    function configFn($locationProvider, blockUIConfig, tagsInputConfigProvider, httpRequestInterceptorCacheBusterProvider) {
        $locationProvider.hashPrefix('');

        // angular-block-ui config
        blockUIConfig.message = 'Molim Vas, sačekajte...';

        tagsInputConfigProvider
            .setDefaults('tagsInput', {
                placeholder: ''
            });

        // angular-cache-buster
        httpRequestInterceptorCacheBusterProvider.setMatchlist([
                /.*common.*/,
                /.*member-dialog.*/,
                /.*member-doc-dialog.*/
            ], true
        );
    }
})();
