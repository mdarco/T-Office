using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using T_Office.ApiCore.Hubs;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/reg-license-reader")]
    [ApiController]
    public class RegLicenseReaderController : ControllerBase
    {
        private readonly IHubContext<TOfficeHub> _hub;

        public RegLicenseReaderController(IHubContext<TOfficeHub> hub)
        {
            _hub = hub;
        }

        // using "wsConnectionId" find the corresponding connected client/group (Agent)
        // and issue the smart card reader read data command
        [Route("read/{wsConnectionId}")]
        [HttpGet]
        public IActionResult ReadSmartCard(string wsConnectionId) {
            _hub.Clients.Client(wsConnectionId).SendAsync("readSmartCardData");
            return Ok();
        }
    }
}
