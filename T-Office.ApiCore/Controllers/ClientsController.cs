using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_Office.Models;
using TOffice.DB;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        [Route("filtered")]
        [HttpPost]
        public ApiTableResponseModel<ClientModel> GetFilteredClients(ClientFilterModel filter)
        {
            return Clients.GetClientsFiltered(filter);
        }

        [Route("simple-exist")]
        [HttpPost]
        public RegLicenseDataExistModel SimpleExist(RegistrationDataModel model)
        {
            return Clients.SimpleExist(model);
        }

        [Route("full-client-data-entry")]
        [HttpPost]
        public void FullClientDataEntry(RegistrationDataModel model)
        {
            Clients.FullClientEntry(model);
        }

        [Route("{id}")]
        [HttpGet]
        public ClientModel GetClient(int id)
        {
            return Clients.GetClient(id);
        }

        [Route("{id}")]
        [HttpPut]
        public void EditClient(int id, ClientModel model)
        {
            Clients.EditClient(id, model);
        }

        [Route("full")]
        [HttpPost]
        public void AddClientFull(RegistrationDataModel model)
        {
            Clients.AddClientFull(model);
        }

        [Route("")]
        [HttpPost]
        public void AddClient(ClientModel model)
        {
            Clients.AddClient(model);
        }

        [Route("{id}")]
        [HttpDelete]
        public void DeleteClient(int id)
        {
            Clients.DeleteClient(id);
        }

        #region Registration and vehicle data

        [Route("{id}/reg-doc-data")]
        [HttpPost]
        public void AddRegistrationDocumentData(int id, RegistrationDataModel model)
        {
            RegistrationDocuments.Add(id, model);
        }

        #endregion

        #region Vehicles

        [Route("{clientID}/vehicles")]
        [HttpGet]
        public List<VehicleDataModel> GetVehicles(int clientID)
        {
            return Clients.GetVehicles(clientID);
        }

        [Route("{clientID}/vehicles/full")]
        [HttpPost]
        public void AddVehicleFull(int clientID, RegistrationDataModel model)
        {
            Vehicles.AddFromFullModel(clientID, model);
        }

        #endregion

        #region Registrations

        [Route("{clientID}/vehicles/{vehicleID}/registrations")]
        [HttpGet]
        public List<VehicleRegistrationModel> GetVehicleRegistrations(int vehicleID)
        {
            return Vehicles.GetRegistrations(vehicleID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations")]
        [HttpPost]
        public void AddVehicleRegistration(int clientID, int vehicleID, VehicleRegistrationModel model)
        {
            Vehicles.AddRegistration(clientID, vehicleID, model);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}")]
        [HttpPut]
        public void EditVehicleRegistration(int vehicleRegistrationID, VehicleRegistrationModel model)
        {
            Vehicles.EditRegistration(vehicleRegistrationID, model);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}")]
        [HttpDelete]
        public void DeleteVehicleRegistration(int clientID, int vehicleID, int vehicleRegistrationID)
        {
            Vehicles.DeleteRegistration(vehicleRegistrationID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments")]
        [HttpGet]
        public List<InstallmentModel> GetVehicleRegistrationInstallments(int vehicleRegistrationID)
        {
            return Vehicles.GetRegistrationInstallments(vehicleRegistrationID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments/reset")]
        [HttpGet]
        public void ResetVehicleRegistrationInstallments(int vehicleRegistrationID)
        {
            Vehicles.ResetInstallments(vehicleRegistrationID);
        }

        [Route("{clientID}/vehicles/{vehicleID}/registrations/{vehicleRegistrationID}/installments/{installmentID}")]
        [HttpPut]
        public void EditVehicleRegistrationInstallment(int installmentID, InstallmentModel model)
        {
            Vehicles.EditInstallment(installmentID, model);
        }

        #endregion

        #region Analytics

        [Route("analytics/clients-due/{numberOfDays}")]
        [HttpGet]
        public List<ClientDueModel> GetClientsDue(int numberOfDays)
        {
            return Clients.GetClientsDue(numberOfDays);
        }

        [Route("analytics/clients-outstanding-total")]
        [HttpGet]
        public List<ClientTotalOutstandingModel> GetClientsOutstandingTotal()
        {
            return Clients.GetClientsOutstandingTotal();
        }

        [Route("analytics/incoming-registrations/{numberOfDays}")]
        [HttpGet]
        public List<ClientDueModel> GetIncomingRegistrations(int numberOfDays)
        {
            return Clients.GetVehiclesWithIncomingRegistrations(numberOfDays);
        }

        [Route("analytics/total-paid-amount-by-period")]
        [HttpPost]
        public decimal GetTotalPaidAmoountByPeriod(DateRangeModel model)
        {
            return Vehicles.GetTotalPaidAmountByPeriod(model.DateFrom, model.DateTo);
        }

        #endregion

        #region Reports

        [Route("reports/costs-by-period")]
        [HttpPost]
        public List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        {
            return Clients.GetCostsByPeriod(filter);
        }

        [Route("reports/total-installments-amount")]
        [HttpPost]
        public decimal? GetTotalInstallmentsAmount(TotalInstallmentsAmountFilter filter)
        {
            return Clients.GetTotalInstallmentsAmount(filter.StartDate, filter.EndDate, filter.IsPaid);
        }

        #endregion
    }
}
