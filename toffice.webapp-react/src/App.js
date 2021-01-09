import * as React from 'react';
import {useKeycloak} from '@react-keycloak/web';
// import keycloak from './keycloak';

// import './App.css';

function App() {
	React.useEffect(() => {
		console.log('APP MOUNTED');
	}, []);

	// eslint-disable-next-line no-unused-vars
	const {_, keycloak} = useKeycloak();

	const btnClick = () => {
		keycloak.logout();
	};

	return (
		<div>
			{keycloak.authenticated && <button onClick={btnClick}>Logout</button>}
			{keycloak.authenticated && (
				<span>{JSON.stringify(keycloak.tokenParsed, null, 2)}</span>
			)}
		</div>
	);
}

export default App;
