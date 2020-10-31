(function () {
    'use strict';

    angular
        .module('TOfficeApp')
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
                /.*reg-dialog.*/,
                /.*reg-doc-dialog.*/,
                /.*installments-list-dialog.*/
            ], true
        );
    }
})();
