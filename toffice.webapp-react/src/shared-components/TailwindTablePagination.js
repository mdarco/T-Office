/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from 'react';

function TailwindTablePagination({
	pageIndex,
	pageSize,
	pageOptions,
	setPageSize,
	gotoPage,
	previousPage,
	nextPage,
	canPreviousPage,
	canNextPage
}) {
	return (
		<div className="bg-white px-4 py-5 border-b border-gray-200 sm:px-6 mb-5 rounded-lg">
			<div className="-ml-4 -mt-4 flex justify-between items-center flex-wrap sm:flex-nowrap">
				<div className="ml-4 mt-4">
					<div className="flex items-center">
						<h3 className="text-sm leading-6 font-medium text-gray-900">
							Strana: {pageIndex + 1} / {pageOptions.length}
						</h3>
					</div>
				</div>

				<div className="ml-4 mt-4 flex-shrink-0 flex">
					{/* page buttons */}
					<button
						type="button"
						className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-30"
						title="Prva strana"
						disabled={!canPreviousPage}
						onClick={() => gotoPage(0)}
					>
						{/* Heroicon name: chevron-double-left */}
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
								d="M11 19l-7-7 7-7m8 14l-7-7 7-7"
							/>
						</svg>
					</button>
					<button
						type="button"
						className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-30"
						title="Prethodna strana"
						disabled={!canPreviousPage}
						onClick={() => previousPage(0)}
					>
						{/* Heroicon name: chevron-left */}
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
								d="M15 19l-7-7 7-7"
							/>
						</svg>
					</button>
					<button
						type="button"
						className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-30"
						title="Sledeća strana"
						disabled={!canNextPage}
						onClick={() => nextPage(0)}
					>
						{/* Heroicon name: chevron-right */}
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
								d="M9 5l7 7-7 7"
							/>
						</svg>
					</button>
					<button
						type="button"
						className="relative inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-30"
						title="Poslednja strana"
						disabled={!canNextPage}
						onClick={() => gotoPage(pageOptions.length - 1)}
					>
						{/* Heroicon name: chevron-double-right */}
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
								d="M13 5l7 7-7 7M5 5l7 7-7 7"
							/>
						</svg>
					</button>
				</div>

				{/* page size combo */}
				<div className="ml-4 mt-4 flex-shrink-0 flex">
					<select
						id="paginationPageSize"
						name="paginationPageSize"
						className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
						value={pageSize}
						onChange={event => {
							setPageSize(Number(event.target.value));
						}}
					>
						{[10, 20, 30, 40, 50].map(ps => (
							<option key={ps} value={ps}>
								Prikaži {ps}
							</option>
						))}
					</select>
				</div>
			</div>
		</div>
	);
}

export default TailwindTablePagination;
