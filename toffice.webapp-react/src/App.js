import * as React from 'react';
import MasterPage from './components/MasterPage/MasterPage';

import {ReactKeycloakProvider} from '@react-keycloak/web';
import keycloak from './keycloak';

// import './App.css';

// const onRender = (
// 	id,
// 	phase,
// 	actualDuration,
// 	baseDuration,
// 	startTime,
// 	commitTime,
// 	interactions
// ) => {
// 	const performanceData = [
// 		`id: ${id}`,
// 		`phase: ${phase}`,
// 		`interactions: ${JSON.stringify([...interactions])}`
// 	].join('\n');
// 	console.log(performanceData);
// };

// const keycloakEventLogger = (event, error) => {
// 	console.log('Keycloak event:', event, error);
// };

// const keycloakTokenLogger = tokens => {
// 	console.log('Keycloak tokens:', tokens);
// };

function App() {
	return (
		// <React.Profiler id="MasterPage" onRender={onRender}>
		<ReactKeycloakProvider
			authClient={keycloak}
			initOptions={{onLoad: 'login-required'}}
			// onEvent={keycloakEventLogger}
			// onTokens={keycloakTokenLogger}
		>
			<MasterPage />
		</ReactKeycloakProvider>
		// </React.Profiler>
	);
}

export default App;
