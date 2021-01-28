import * as React from 'react';
// import {useKeycloak} from '@react-keycloak/web';
import MasterPage from './components/MasterPage/MasterPage';
// import keycloak from './keycloak';

// import './App.css';

function App() {
	// eslint-disable-next-line no-unused-vars
	// const {_, keycloak} = useKeycloak();

	// const btnClick = () => {
	// 	keycloak.logout();
	// };

	return (
		<>
			<MasterPage />
			{/* <div>
				{keycloak.authenticated && (
					<button
						className="bg-transparent hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded"
						onClick={btnClick}
					>
						Logout
					</button>
				)}
				{keycloak.authenticated && (
					<span>{JSON.stringify(keycloak.tokenParsed, null, 2)}</span>
				)}
			</div> */}
		</>
	);
}

export default App;
