import apiConfig from '../services/apiConfig';
import axios from 'axios';

const webApiBaseUrl = apiConfig.WEB_API_BASE_URL;
const urlPrefix = '/clients';

export default class ClientsService {
	static instance = null;

	static getInstance() {
		if (!this.instance) {
			this.instance = new ClientsService();
		}
	}

	getFilteredClients(filter) {
		const url = `${webApiBaseUrl}/${urlPrefix}/filtered`;
		console.log('Clients filtered POST URL: ', url);
		console.log('Clients filtered POST filter: ', filter);
		return axios.post(url, filter);
	}
}
