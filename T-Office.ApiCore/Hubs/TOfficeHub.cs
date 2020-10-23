using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T_Office.ApiCore.LiteDB;
using T_Office.Models;

namespace T_Office.ApiCore.Hubs
{
    public class TOfficeHub : Hub
    {
        private readonly IAgentDataPersistence _ctx;

        public TOfficeHub(IAgentDataPersistence ctx)
        {
            _ctx = ctx;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public void InsertAgentData(AgentDataModel model) {
            _ctx.InsertAgentData(model);
        }
    }
}
