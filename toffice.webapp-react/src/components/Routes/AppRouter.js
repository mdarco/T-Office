import * as React from 'react';
import {Switch, Route} from 'react-router-dom';
import Home from './../Home/Home';
import Clients from '../Clients/Clients';
import Reports from '../Reports/Reports';
import NotFound from '../NotFound/NotFound';

// import './AppRouter.css';

function AppRouter() {
	return (
		<Switch>
			<Route path="/" component={Home} exact />
			<Route path="/home" component={Home} />
			<Route path="/clients" component={Clients} />
			<Route path="/reports" component={Reports} />
			<Route component={NotFound} />
		</Switch>
	);
}

export default AppRouter;
