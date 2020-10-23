using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T_Office.ApiCore.LiteDB;
using T_Office.Models;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/agent-data")]
    [ApiController]
    public class AgentDataController : ControllerBase
    {
        private readonly IAgentDataPersistence _ctx;

        public AgentDataController(IAgentDataPersistence ctx)
        {
            _ctx = ctx;
        }

        [Route("{agentId}")]
        [HttpGet]
        public IActionResult GetAgentData(string agentId)
        {
            AgentDataModel model = _ctx.GetAgentData(agentId);
            return Ok(model);
        }

        [Route("{agentId}")]
        [HttpPost]
        public IActionResult InsertAgentData(string agentId, AgentDataModel model)
        {
            // save agent data { agentId, wsConnectionId } to in-memory DB with 'agentId' as a key
            // WebApp will use it to issue requests to agent via api
            _ctx.InsertAgentData(model);
            return Ok();
        }

        [Route("{agentId}")]
        [HttpDelete]
        public IActionResult DeleteAgentData(string agentId)
        {
            bool result = _ctx.DeleteAgentData(agentId);
            return Ok(result);
        }
    }
}
