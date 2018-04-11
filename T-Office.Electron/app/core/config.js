(function () {
    'use strict';

    angular
        .module('TOfficeApp')
        .config(configFn);

    configFn.$inject = ['$locationProvider', 'blockUIConfig', 'tagsInputConfigProvider', 'httpRequestInterceptorCacheBusterProvider', 'RollbarProvider'];

    function configFn($locationProvider, blockUIConfig, tagsInputConfigProvider, httpRequestInterceptorCacheBusterProvider, RollbarProvider) {
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

        // ngRollbar
        RollbarProvider.init({
            accessToken: "3dbcb9f66f60452b9dbff463f0790fae",
            captureUncaught: true,
            payload: {
                environment: 'T-Office Client App'
            }
        });
    }
})();
