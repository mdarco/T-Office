import * as React from 'react';

const initialGlobalInfo = {
	WEB_API_BASE_URL: 'https://dancefactory.club/TOfficeApi/api'
};

const GlobalInfoContext = React.createContext();
GlobalInfoContext.displayName = 'GlobalInfoContext';

function GlobalInfoProvider({children}) {
	const [globalInfo, setGlobalInfo] = React.useState(initialGlobalInfo);

	const contextValue = React.useMemo(
		() => ({
			globalInfo,
			setGlobalInfo
		}),
		[globalInfo]
	);

	return (
		<GlobalInfoContext.Provider value={contextValue}>
			{children}
		</GlobalInfoContext.Provider>
	);
}

function useGlobalInfo() {
	const context = React.useContext(GlobalInfoContext);
	if (context === undefined) {
		throw new Error('useGlobalInfo must be used within GlobalInfoProvider');
	}
	return context;
}

export {GlobalInfoProvider, useGlobalInfo};
