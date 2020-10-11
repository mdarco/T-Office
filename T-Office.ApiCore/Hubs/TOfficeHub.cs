using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T_Office.ApiCore.Hubs
{
    public class TOfficeHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public void JoinGroupWithConnectionId(string connectionId)
        {
            Groups.AddToGroupAsync(connectionId, connectionId);
        }
    }
}
