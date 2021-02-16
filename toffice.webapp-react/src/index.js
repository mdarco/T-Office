// import React, {Profiler} from 'react';
import * as React from 'react';
import ReactDOM from 'react-dom';
// import './wdyr';

import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

// import {GlobalInfoProvider} from './context/global-info-context';

import './tailwind.output.css';

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

ReactDOM.render(
	// <Profiler id="App" onRender={onRender}>
	<React.StrictMode>
		{/* <GlobalInfoProvider> */}
		<App />
		{/* </GlobalInfoProvider> */}
	</React.StrictMode>,
	// </Profiler>,
	document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
