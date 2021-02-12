import * as React from 'react';
import TailwindTable from '../../shared-components/TailwindTable';
import ClientsHeader from './ClientsHeader';
import FakeClients from '../../fake-data/clients';

// import './Clients.css';

function Clients() {
	const columns = React.useMemo(
		() => [
			{
				Header: 'Naziv',
				Cell: props => {
					// console.log(props);
					return (
						<>
							<p className="text-sm font-medium text-indigo-600 truncate">
								{props.cell.row.original.FullOwnerName}
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
											d="M5.121 17.804A13.937 13.937 0 0112 16c2.5 0 4.847.655 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0zm6 2a9 9 0 11-18 0 9 9 0 0118 0z"
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
				Header: 'Kontakt podaci'
			},
			{
				Header: 'Vozila'
			},
			{
				Header: 'Preporuka',
				accessor: 'RecommendedBy'
			}
		],
		[]
	);

	const data = FakeClients.Data;

	return (
		<div className="p-10">
			<ClientsHeader />
			<TailwindTable columns={columns} data={data} />
		</div>
	);
}

export default Clients;
