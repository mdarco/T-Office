using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.ApiCore.LiteDB
{
    public interface IAgentDataPersistence
    {
        public void InsertAgentData(AgentDataModel model);
        public AgentDataModel GetAgentData(string agentId);
        public bool DeleteAgentData(string agentId);
    }
}
