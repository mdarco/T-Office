import * as React from 'react';
import MasterPage from './components/MasterPage/MasterPage';

import {ReactKeycloakProvider} from '@react-keycloak/web';
import keycloak from './keycloak';

import {QueryClient, QueryClientProvider} from 'react-query';

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

const queryClient = new QueryClient();

function App() {
	return (
		// <React.Profiler id="MasterPage" onRender={onRender}>
		<ReactKeycloakProvider
			authClient={keycloak}
			initOptions={{onLoad: 'login-required'}}
			// onEvent={keycloakEventLogger}
			// onTokens={keycloakTokenLogger}
		>
			<QueryClientProvider client={queryClient}>
				<MasterPage />
			</QueryClientProvider>
		</ReactKeycloakProvider>
		// </React.Profiler>
	);
}

export default App;
