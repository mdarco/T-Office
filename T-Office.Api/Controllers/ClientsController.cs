using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using T_Office.Models;

namespace T_Office.Api.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        [Route("filtered")]
        [HttpPost]
        public ApiTableResponseModel<ClientModel> GetFilteredClients(ClientFilterModel filter)
        {
            return DAL.Clients.GetClientsFiltered(filter);
        }

        [Route("{id}")]
        [HttpGet]
        public ClientModel GetClient(int id)
        {
            return DAL.Clients.GetClient(id);
        }

        [Route("{id}")]
        [HttpPut]
        public void EditClient(int id, ClientModel model)
        {
            DAL.Clients.EditClient(id, model);
        }

        [Route("full")]
        [HttpPost]
        public void AddClientFull(RegistrationDataModel model)
        {
            try
            {
                DAL.Clients.AddClientFull(model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = ex.Message
                    });
            }
        }

        [Route("")]
        [HttpPost]
        public void AddClient(ClientModel model)
        {
            try
            {
                DAL.Clients.AddClient(model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = ex.Message
                    });
            }
        }

        #region Vehicles

        [Route("{clientID}/vehicles")]
        [HttpGet]
        public List<VehicleDataModel> GetVehicles(int clientID)
        {
            return DAL.Clients.GetVehicles(clientID);
        }

        [Route("{clientID}/vehicles/full")]
        [HttpPost]
        public void AddVehicleFull(int clientID, RegistrationDataModel model)
        {
            DAL.Vehicles.AddFromFullModel(clientID, model);
        }

        #endregion

        #region Registrations

        [Route("{clientID}/vehicles/{vehicleID}/registrations")]
        [HttpGet]
        public List<VehicleRegistrationModel> GetVehicleRegistrations(int vehicleID)
        {
            return DAL.Vehicles.GetRegistrations(vehicleID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations")]
        [HttpPost]
        public void AddVehicleRegistration(int clientID, int vehicleID, VehicleRegistrationModel model)
        {
            DAL.Vehicles.AddRegistration(clientID, vehicleID, model);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}")]
        [HttpPut]
        public void EditVehicleRegistration(int vehicleRegistrationID, VehicleRegistrationModel model)
        {
            DAL.Vehicles.EditRegistration(vehicleRegistrationID, model);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}")]
        [HttpDelete]
        public void DeleteVehicleRegistration(int clientID, int vehicleID, int vehicleRegistrationID)
        {
            DAL.Vehicles.DeleteRegistration(vehicleRegistrationID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments")]
        [HttpGet]
        public List<InstallmentModel> GetVehicleRegistrationInstallments(int vehicleRegistrationID)
        {
            return DAL.Vehicles.GetRegistrationInstallments(vehicleRegistrationID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments/{installmentID}")]
        [HttpPut]
        public void EditVehicleRegistrationInstallment(int installmentID, InstallmentModel model)
        {
            DAL.Vehicles.EditInstallment(installmentID, model);
        }

        #endregion

        #region Analytics

        [Route("analytics/clients-due/{numberOfDays}")]
        [HttpGet]
        public List<ClientDueModel> GetClientsDue(int numberOfDays)
        {
            return DAL.Clients.GetClientsDue(numberOfDays);
        }

        [Route("analytics/clients-outstanding-total")]
        [HttpGet]
        public List<ClientTotalOutstandingModel> GetClientsOutstandingTotal()
        {
            return DAL.Clients.GetClientsOutstandingTotal();
        }

        #endregion

        #region Reports

        [Route("reports/costs-by-period")]
        [HttpPost]
        public List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        {
            return DAL.Clients.GetCostsByPeriod(filter);
        }

        #endregion
    }
}
