/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from 'react';
// import {useTable, usePagination} from 'react-table';
import {useTable} from 'react-table';

function TailwindTable({columns, data}) {
	const {
		getTableProps,
		getTableBodyProps,
		headers,
		rows,
		prepareRow
	} = useTable({
		columns,
		data
	});

	return (
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
								{rows.map(row => {
									prepareRow(row);
									return (
										<tr {...row.getRowProps()} className="bg-white">
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
	);
}

export default TailwindTable;
