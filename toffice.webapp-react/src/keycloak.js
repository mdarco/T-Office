import Keycloak from 'keycloak-js';

const keycloakConfig = {
	url: 'http://localhost:8080/auth',
	realm: 'test-realm',
	clientId: 'react-demo',
	onLoad: 'login-required',
	// checkLoginIframe: false,
	promiseType: 'native'
};

const keycloak = new Keycloak(keycloakConfig);

export default keycloak;
