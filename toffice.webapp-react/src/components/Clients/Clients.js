import * as React from 'react';
import TailwindTable from '../../shared-components/TailwindTable';
import ClientsHeader from './ClientsHeader';
import ClientsFilter from './ClientsFilter';
import {Link} from 'react-router-dom';
import ClientsService from '../../services/clientsService';

function Clients() {
	const [showFilter, setShowFilter] = React.useState(false);
	const [filter, setFilter] = React.useState({});

	const columns = React.useMemo(
		() => [
			{
				Header: 'Naziv',
				Cell: props => {
					// console.log(props);
					return (
						<>
							<p className="text-sm font-bold text-indigo-600 truncate">
								<Link
									to={`/clients/${props.cell.row.original.ID}`}
									className="hover:underline"
								>
									{props.cell.row.original.FullOwnerName}
								</Link>
							</p>
							{/* TODO: set 'FullUserName' to null or empty string on the backend */}
							{props.cell.row.original.FullUserName !== ' ' && (
								<p
									className="mt-2 flex items-center text-xs text-gray-500"
									title="Korisnik"
								>
									<svg
										className="flex-shrink-0 mr-1.5 h-5 w-5 text-gray-400"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth={2}
											d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
										/>
									</svg>
									{props.cell.row.original.FullUserName}
								</p>
							)}
						</>
					);
				}
			},
			{
				Header: 'Kontakt podaci',
				Cell: props => {
					return (
						<>
							{props.cell.row.original.OwnerAddress && (
								<p
									className="mt-2 flex items-center text-xs text-gray-500"
									title="Korisnik"
								>
									<svg
										className="flex-shrink-0 mr-1.5 h-5 w-5 text-gray-400"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth={2}
											d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
										/>
									</svg>
									{props.cell.row.original.OwnerAddress}
								</p>
							)}
							{props.cell.row.original.OwnerPhone && (
								<p
									className="mt-2 flex items-center text-xs text-gray-500"
									title="Korisnik"
								>
									<svg
										className="flex-shrink-0 mr-1.5 h-5 w-5 text-gray-400"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth={2}
											d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z"
										/>
									</svg>
									{props.cell.row.original.OwnerPhone}
								</p>
							)}
							{props.cell.row.original.OwnerEmail && (
								<p
									className="mt-2 flex items-center text-xs text-gray-500"
									title="Korisnik"
								>
									<svg
										className="flex-shrink-0 mr-1.5 h-5 w-5 text-gray-400"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth={2}
											d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
										/>
									</svg>
									{props.cell.row.original.OwnerEmail}
								</p>
							)}
						</>
					);
				}
			},
			{
				Header: 'Vozila',
				Cell: props => {
					return (
						<>
							{props.cell.row.original.Vehicles.map(vehicle => (
								<div key={vehicle.ID} className="mb-2">
									<span className="inline-flex items-center px-2.5 py-0.5 rounded text-xs font-medium bg-blue-100 text-blue-800">
										{vehicle.RegistrationNumber}
									</span>
									&nbsp;&nbsp;&nbsp;
									<span>{vehicle.Make}</span>&nbsp;
									<span>{vehicle.Model}</span>
								</div>
							))}
						</>
					);
				}
			},
			{
				Header: 'Preporuka',
				accessor: 'RecommendedBy'
			}
		],
		[]
	);

	const setFilterData = filterData => {
		/* Merge 'filterData' with paging data and use setFilter() here... */
	};

	const fetchData = ClientsService.getInstance().getFilteredClients;

	return (
		<div className="p-10">
			<ClientsHeader showFilter={showFilter} setShowFilter={setShowFilter} />
			<ClientsFilter showFilter={showFilter} setFilterData={setFilterData} />
			<TailwindTable
				columns={columns}
				filter={filter}
				fetchData={fetchData}
				fetchDataQueryKey="clients"
			/>
		</div>
	);
}

export default Clients;
