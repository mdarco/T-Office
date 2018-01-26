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

        [Route("full")]
        [HttpPost]
        public void AddClient(RegistrationDataModel model)
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
    }
}
