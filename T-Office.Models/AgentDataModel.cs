﻿using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class AgentDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string AgentId { get; set; }
        public string WsConnectionId { get; set; }
    }
}
