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

        [Route("{id}")]
        [HttpDelete]
        public void DeleteClient(int id)
        {
            try
            {
                DAL.Clients.DeleteClient(id);
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

        #region Registration and vehicle data

        [Route("{id}/reg-doc-data")]
        [HttpPost]
        public void AddRegistrationDocumentData(int id, RegistrationDataModel model)
        {
            try
            {
                DAL.RegistrationDocuments.Add(id, model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ReasonPhrase = ex.Message
                    });
            }
        }

        #endregion

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

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments/reset")]
        [HttpGet]
        public void ResetVehicleRegistrationInstallments(int vehicleRegistrationID)
        {
            DAL.Vehicles.ResetInstallments(vehicleRegistrationID);
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

        [Route("analytics/incoming-registrations/{numberOfDays}")]
        [HttpGet]
        public List<ClientDueModel> GetIncomingRegistrations(int numberOfDays)
        {
            return DAL.Clients.GetVehiclesWithIncomingRegistrations(numberOfDays);
        }

        [Route("analytics/total-paid-amount-by-period")]
        [HttpPost]
        public decimal GetTotalPaidAmoountByPeriod(DateRangeModel model)
        {
            return DAL.Vehicles.GetTotalPaidAmountByPeriod(model.DateFrom, model.DateTo);
        }

        #endregion

        #region Reports

        [Route("reports/costs-by-period")]
        [HttpPost]
        public List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        {
            return DAL.Clients.GetCostsByPeriod(filter);
        }

        [Route("reports/total-installments-amount")]
        [HttpPost]
        public decimal? GetTotalInstallmentsAmount(TotalInstallmentsAmountFilter filter)
        {
            return DAL.Clients.GetTotalInstallmentsAmount(filter.StartDate, filter.EndDate, filter.IsPaid);
        }

        #endregion
    }
}
