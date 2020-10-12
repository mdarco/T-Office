using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class SmartCardReaderResponseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string WsConnectionId { get; set; }
        public string Data { get; set; }
    }
}
