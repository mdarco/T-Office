/** @jsxImportSource @welldone-software/why-did-you-render */

/* eslint-disable jsx-a11y/anchor-is-valid */
import * as React from 'react';
import {Transition} from '@headlessui/react';
import {useKeycloak} from '@react-keycloak/web';
import {useGlobalInfo} from '../../context/global-info-context';
import {BrowserRouter as Router, NavLink} from 'react-router-dom';
import AppRouter from '../Routes/AppRouter';
import Avatar from 'react-avatar';

// import './MasterPage.css';

function MasterPage() {
	// eslint-disable-next-line no-unused-vars
	const {initialized, keycloak} = useKeycloak();
	const {globalInfo, setGlobalInfo} = useGlobalInfo();
	const [sidebarOpen, setSidebarOpen] = React.useState(false);

	React.useEffect(() => {
		if (initialized) {
			setGlobalInfo(globalInfoPreviousValue => {
				return {
					...globalInfoPreviousValue,
					userInfo: {
						userFullName: keycloak?.idTokenParsed?.name,
						username: keycloak?.idTokenParsed?.preferred_username
					}
				};
			});
		}
	}, [
		keycloak?.idTokenParsed?.name,
		keycloak?.idTokenParsed?.preferred_username,
		initialized,
		setGlobalInfo
	]);

	return (
		<Router>
			{/* requires Tailwind CSS v2.0+ */}
			<div className="h-screen flex overflow-hidden bg-gray-100">
				{/* Off-canvas menu for mobile, show/hide based on off-canvas menu state */}
				<Transition show={sidebarOpen} className="md:hidden">
					{/* <div className="md:hidden"> */}
					<div className="fixed inset-0 flex z-40">
						{/* 
						Off-canvas menu overlay, show/hide based on off-canvas menu state.
						Entering: "transition-opacity ease-linear duration-300"
							From: "opacity-0"
							To: "opacity-100"
						Leaving: "transition-opacity ease-linear duration-300"
							From: "opacity-100"
							To: "opacity-0"
					 */}
						<Transition.Child
							enter="transition-opacity ease-linear duration-300"
							enterFrom="opacity-0"
							enterTo="opacity-100"
							leave="transition-opacity ease-linear duration-300"
							leaveFrom="opacity-100"
							leaveTo="opacity-0"
							className="fixed inset-0"
						>
							{/* <div className="fixed inset-0"> */}
							<div className="absolute inset-0 bg-gray-600 opacity-75"></div>
							{/* </div> */}
						</Transition.Child>
						{/* 
						Off-canvas menu, show/hide based on off-canvas menu state.
						Entering: "transition ease-in-out duration-300 transform"
							From: "-translate-x-full"
							To: "translate-x-0"
						Leaving: "transition ease-in-out duration-300 transform"
							From: "translate-x-0"
							To: "-translate-x-full"
					 */}
						<Transition.Child
							enter="transition ease-in-out duration-300 transform"
							enterFrom="-translate-x-full"
							enterTo="translate-x-0"
							leave="transition ease-in-out duration-300 transform"
							leaveFrom="translate-x-0"
							leaveTo="-translate-x-full"
							className="relative flex-1 flex flex-col max-w-xs w-full bg-gray-800"
						>
							{/* <div className="relative flex-1 flex flex-col max-w-xs w-full bg-gray-800"> */}
							<div className="absolute top-0 right-0 -mr-12 pt-2">
								<button
									className="ml-1 flex items-center justify-center h-10 w-10 rounded-full focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white"
									onClick={() => {
										setSidebarOpen(false);
									}}
								>
									<span className="sr-only">Close sidebar</span>
									{/* Heroicon name: x */}
									<svg
										className="h-6 w-6 text-white"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
										aria-hidden="true"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth="2"
											d="M6 18L18 6M6 6l12 12"
										/>
									</svg>
								</button>
							</div>
							<div className="flex-1 h-0 pt-5 pb-4 overflow-y-auto">
								<div className="flex-shrink-0 flex items-center px-4">
									<img
										className="h-8 w-auto"
										src="https://tailwindui.com/img/logos/workflow-logo-indigo-500-mark-white-text.svg"
										alt="Workflow"
									/>
								</div>
								<nav className="mt-5 px-2 space-y-1">
									<NavLink
										to="/home"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: home */}
										<svg
											className="text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
											/>
										</svg>
										Početna
									</NavLink>

									<NavLink
										to="/clients"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: users */}
										<svg
											className="text-gray-400 group-hover:text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"
											/>
										</svg>
										Klijenti
									</NavLink>

									<NavLink
										to="/reports"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: folder */}
										<svg
											className="text-gray-400 group-hover:text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z"
											/>
										</svg>
										Izveštaji
									</NavLink>
								</nav>
							</div>
							<div className="flex-shrink-0 flex bg-gray-700 p-4">
								<a href="#" className="flex-shrink-0 group block">
									<div className="flex items-center">
										<div>
											<img
												className="inline-block h-10 w-10 rounded-full"
												src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
												alt=""
											/>
										</div>
										<div className="ml-3">
											<p className="text-base font-medium text-white">
												Tom Cook
											</p>
											<p className="text-sm font-medium text-gray-400 group-hover:text-gray-300">
												View profile
											</p>
										</div>
									</div>
								</a>
							</div>
							{/* </div> */}
						</Transition.Child>

						<div className="flex-shrink-0 w-14">
							{/* Force sidebar to shrink to fit close icon */}
						</div>
					</div>
					{/* </div> */}
				</Transition>

				{/* Static sidebar for desktop */}
				<div className="hidden md:flex md:flex-shrink-0">
					<div className="flex flex-col w-64">
						{/* Sidebar component, can be swapped with another sidebar */}
						<div className="flex flex-col h-0 flex-1 bg-gray-800">
							<div className="flex-1 flex flex-col pt-5 pb-4 overflow-y-auto">
								<div className="flex items-center flex-shrink-0 px-4">
									<img
										className="h-8 w-auto"
										src="https://tailwindui.com/img/logos/workflow-logo-indigo-500-mark-white-text.svg"
										alt="Workflow"
									/>
								</div>
								<nav className="mt-5 flex-1 px-2 bg-gray-800 space-y-1">
									<NavLink
										to="/home"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: home */}
										<svg
											className="text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
											/>
										</svg>
										Početna
									</NavLink>

									<NavLink
										to="/clients"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: users */}
										<svg
											className="text-gray-400 group-hover:text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"
											/>
										</svg>
										Klijenti
									</NavLink>

									<NavLink
										to="/reports"
										className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
										activeClassName="bg-gray-900 text-white group flex items-center px-2 py-2 text-base font-medium rounded-md"
									>
										{/* Heroicon name: folder */}
										<svg
											className="text-gray-400 group-hover:text-gray-300 mr-4 h-6 w-6"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
											aria-hidden="true"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z"
											/>
										</svg>
										Izveštaji
									</NavLink>
								</nav>
							</div>

							{/* logout button */}
							<div className="p-2">
								<a
									href="#"
									className="text-gray-300 hover:bg-gray-700 hover:text-white group flex items-center px-2 py-2 text-sm font-medium rounded-md"
									onClick={() => {
										keycloak.logout();
									}}
								>
									{/* Heroicon name: logout */}
									<svg
										className="-ml-1 mr-3 h-5 w-5"
										xmlns="http://www.w3.org/2000/svg"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth={2}
											d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"
										/>
									</svg>
									Odjava
								</a>
							</div>

							<div className="flex-shrink-0 flex bg-gray-700 p-4">
								<a href="#" className="flex-shrink-0 w-full group block">
									<div className="flex items-center">
										<div>
											<Avatar
												name={globalInfo?.userInfo?.userFullName}
												size="48"
												className="inline-block h-9 w-9 rounded-full"
											/>
										</div>
										<div className="ml-3">
											<p className="text-sm font-medium text-white">
												{globalInfo?.userInfo?.userFullName}
											</p>
											<p className="text-xs font-medium text-gray-300 group-hover:text-gray-200">
												({globalInfo?.userInfo?.username})
											</p>
										</div>
									</div>
								</a>
							</div>
						</div>
					</div>
				</div>

				<div className="flex flex-col w-0 flex-1 overflow-hidden">
					<div className="md:hidden pl-1 pt-1 sm:pl-3 sm:pt-3">
						<button
							className="-ml-0.5 -mt-0.5 h-12 w-12 inline-flex items-center justify-center rounded-md text-gray-500 hover:text-gray-900 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-indigo-500"
							onClick={() => {
								setSidebarOpen(true);
							}}
						>
							<span className="sr-only">Open sidebar</span>
							{/* Heroicon name: menu */}
							<svg
								className="h-6 w-6"
								xmlns="http://www.w3.org/2000/svg"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
								aria-hidden="true"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth="2"
									d="M4 6h16M4 12h16M4 18h16"
								/>
							</svg>
						</button>
					</div>

					<main
						className="flex-1 relative z-0 overflow-y-auto focus:outline-none"
						tabIndex="0"
					>
						<div>
							<AppRouter />
						</div>
					</main>
				</div>
			</div>
		</Router>
	);
}

// MasterPage.whyDidYouRender = true;

export default MasterPage;
