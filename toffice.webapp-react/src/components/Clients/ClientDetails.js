import * as React from 'react';
import {useParams} from 'react-router';

// import './ClientDetails.css';

function ClientDetails() {
	const {id} = useParams();

	return <div>{`Client details - client ID: ${id}`}</div>;
}

export default ClientDetails;
