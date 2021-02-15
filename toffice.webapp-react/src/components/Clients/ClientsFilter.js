import * as React from 'react';

function ClientsFilter({showFilter}) {
	return (
		<>
			{showFilter && (
				<div className="px-4 py-5 bg-blue-100 sm:px-6 mb-5 rounded-lg">
					<div className="grid grid-cols-6 gap-6">
						<div className="col-span-6 sm:col-span-2">
							{/* Client name */}
							<label
								htmlFor="clientName"
								className="block text-sm font-medium text-gray-700"
							>
								Naziv
							</label>
							<div className="mt-1">
								<input
									type="text"
									name="clientName"
									id="clientName"
									className="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
								/>
							</div>
						</div>

						<div className="col-span-6 sm:col-span-2">
							{/* Registration number */}
							<label
								htmlFor="regNo"
								className="block text-sm font-medium text-gray-700"
							>
								Broj registracije
							</label>
							<div className="mt-1">
								<input
									type="text"
									name="regNo"
									id="regNo"
									className="shadow-sm focus:ring-indigo-500 focus:border-indigo-500 block w-full sm:text-sm border-gray-300 rounded-md"
								/>
							</div>
						</div>

						<div className="col-span-6 sm:col-span-2">
							{/* dummy label to position the buttons properly */}
							<label className="block">&nbsp;</label>
							{/* Apply filter button */}
							<button
								type="button"
								className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-900 hover:bg-blue-700 focus:outline-none"
							>
								Pronađi
							</button>
							&nbsp;
							{/* Cancel filter button */}
							<button
								type="button"
								className="inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md shadow-sm text-gray-700 bg-white hover:bg-gray-50 focus:outline-none"
							>
								Poništi
							</button>
						</div>
					</div>
				</div>
			)}
		</>
	);
}

export default ClientsFilter;
