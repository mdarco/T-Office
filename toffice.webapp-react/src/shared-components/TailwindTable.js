/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from 'react';
import {useTable, usePagination} from 'react-table';
// import {useTable} from 'react-table';
import NoData from './NoData';
import TailwindTablePagination from './TailwindTablePagination';

function TailwindTable({columns, data, total}) {
	const {
		getTableProps,
		getTableBodyProps,
		headers,
		// rows,
		prepareRow,

		// instead of 'rows' - to show only the rows for the active page
		page,
		pageOptions,
		// pageCount,
		setPageSize,
		gotoPage,
		nextPage,
		previousPage,
		canNextPage,
		canPreviousPage,
		state: {pageIndex, pageSize}
	} = useTable(
		{
			columns,
			data,
			initialState: {pageIndex: 0, pageSize: 10}
			// turned on for our server-side pagination (our own data fetching)
			// manualPagination: true
			// we also have to provide our own 'pageCount'
			// pageCount: controlledPageCount
		},
		usePagination
	);

	if (total === 0) {
		return <NoData />;
	}

	return (
		<>
			<TailwindTablePagination
				pageIndex={pageIndex}
				pageSize={pageSize}
				pageOptions={pageOptions}
				setPageSize={setPageSize}
				gotoPage={gotoPage}
				previousPage={previousPage}
				nextPage={nextPage}
				canPreviousPage={canPreviousPage}
				canNextPage={canNextPage}
			/>

			<div className="flex flex-col">
				<div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
					<div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
						<div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
							<table
								{...getTableProps()}
								className="min-w-full divide-y divide-gray-200"
							>
								<thead className="bg-gray-50">
									<tr>
										{headers.map(column => (
											<th
												{...column.getHeaderProps()}
												scope="col"
												className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
											>
												{column.render('Header')}
											</th>
										))}
									</tr>
								</thead>
								<tbody {...getTableBodyProps()}>
									{page.map((row, i) => {
										prepareRow(row);
										return (
											<tr
												{...row.getRowProps()}
												className={`hover:bg-gray-100 ${
													row.index % 2 === 0 ? 'bg-white' : 'bg-gray-50'
												}`}
											>
												{row.cells.map(cell => {
													return (
														<td
															{...cell.getCellProps()}
															className="px-6 py-4 whitespace-nowrap text-sm font-medium"
														>
															{cell.render('Cell')}
														</td>
													);
												})}
											</tr>
										);
									})}
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</div>

			<div className="mt-5">
				<TailwindTablePagination
					pageIndex={pageIndex}
					pageSize={pageSize}
					pageOptions={pageOptions}
					setPageSize={setPageSize}
					gotoPage={gotoPage}
					previousPage={previousPage}
					nextPage={nextPage}
					canPreviousPage={canPreviousPage}
					canNextPage={canNextPage}
				/>
			</div>
		</>
	);
}

export default TailwindTable;
