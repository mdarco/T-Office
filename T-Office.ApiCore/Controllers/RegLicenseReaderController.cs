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
            this._hub = hub;
        }

        // TODO: using "wsConnectionId" find the corresponding connected client (Agent)
        // and issue the smart card reader command
        [Route("read/{wsConnectionId}")]
        [HttpGet]
        public string Read() { return string.Empty; }
    }
}
