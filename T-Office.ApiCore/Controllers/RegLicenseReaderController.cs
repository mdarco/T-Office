using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using T_Office.ApiCore.Hubs;
using T_Office.ApiCore.LiteDB;
using T_Office.Models;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/reg-license-reader")]
    [ApiController]
    public class RegLicenseReaderController : ControllerBase
    {
        private readonly IHubContext<TOfficeHub> _hub;
        private readonly LiteDBContext _ctx;

        public RegLicenseReaderController(IHubContext<TOfficeHub> hub, LiteDBContext ctx)
        {
            _hub = hub;
            _ctx = ctx;
        }

        // using "wsConnectionId" find the corresponding connected client/group (Agent)
        // and issue the smart card reader read data command
        [Route("read/{wsConnectionId}")]
        [HttpGet]
        public IActionResult ReadSmartCardData(string wsConnectionId) {
            _hub.Clients.Client(wsConnectionId).SendAsync("readSmartCardData");
            return Ok();
        }

        [Route("response/{wsConnectionId}")]
        [HttpPost]
        public IActionResult InsertSmartCardReaderResponse(string wsConnectionId, SmartCardReaderResponseModel model)
        {
            // save smart card reader data to in-memory DB with 'wsConnectionId' as a key
            // WebApp will poll it for the result and once it obtains it, it will delete the key and data from the in-memory DB
            _ctx.InsertSmartCardResponse(model);
            return Ok();
        }

        [Route("get/{wsConnectionId}")]
        [HttpGet]
        public IActionResult GetSmartCardReaderResponse(string wsConnectionId)
        {
            SmartCardReaderResponseModel model = _ctx.GetSmartCardResponse(wsConnectionId);
            return Ok(model);
        }

        [Route("delete/{wsConnectionId}")]
        [HttpDelete]
        public IActionResult DeleteSmartCardReaderResponse(string wsConnectionId)
        {
            _ctx.DeleteSmartCardResponse(wsConnectionId);
            return Ok();
        }
    }
}
