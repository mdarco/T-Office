/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from 'react';
import ClientsFilter from './ClientsFilter';

function ClientsHeader() {
	const [showFilter, setShowFilter] = React.useState(false);

	const toggleFilter = () => {
		setShowFilter(!showFilter);
	};

	return (
		<>
			<div className="bg-white px-4 py-5 border-b border-gray-200 sm:px-6 mb-5 bg-gray-800 rounded-lg">
				<div className="-ml-4 -mt-4 flex justify-between items-center flex-wrap sm:flex-nowrap">
					<div className="ml-4 mt-4">
						<div className="flex items-center">
							<h3 className="text-lg leading-6 font-medium text-gray-300">
								Klijenti
							</h3>
							&nbsp;&nbsp;&nbsp;
							{/* filter button */}
							<button
								type="button"
								className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-30"
								title="Pretraživanje klijenata"
								onClick={() => toggleFilter()}
							>
								{/* Heroicon name: filter */}
								<svg
									className="h-5 w-5 text-gray-400"
									xmlns="http://www.w3.org/2000/svg"
									fill="none"
									viewBox="0 0 24 24"
									stroke="currentColor"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth={2}
										d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z"
									/>
								</svg>
							</button>
						</div>
					</div>
					<div className="ml-4 mt-4 flex-shrink-0 flex">
						<button
							type="button"
							className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
						>
							{/* Heroicon name: plus-circle */}
							<svg
								className="-ml-1 mr-2 h-5 w-5 text-gray-400"
								xmlns="http://www.w3.org/2000/svg"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
								aria-hidden="true"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth={2}
									d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z"
								/>
							</svg>
							<span>Unos novog klijenta</span>
						</button>
						<button
							type="button"
							className="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
						>
							{/* Heroicon name: document-text */}
							<svg
								className="-ml-1 mr-2 h-5 w-5 text-gray-400"
								xmlns="http://www.w3.org/2000/svg"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth={2}
									d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
								/>
							</svg>
							<span>Čitanje saobraćajne dozvole</span>
						</button>
					</div>
				</div>
			</div>
			{/* Filter panel */}
			<ClientsFilter showFilter={showFilter} />
		</>
	);
}

export default ClientsHeader;
