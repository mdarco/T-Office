import apiConfig from '../services/apiConfig';
import axios from 'axios';

const webApiBaseUrl = apiConfig.WEB_API_BASE_URL;
const urlPrefix = '/clients';

class ClientsService {
	// constructor() {
	// 	this._data = [];
	// }

	getFilteredClients(filter) {
		const url = `${webApiBaseUrl}/${urlPrefix}/filtered`;
		// console.log('getFilteredClients() POST filter: ', filter);
		return axios.post(url, filter);
	}
}

const clientsService = new ClientsService();
Object.freeze(clientsService);

export default clientsService;
