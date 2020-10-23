using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.ApiCore.LiteDB
{
    public interface ISmartCardReaderPersistence
    {
        public void InsertSmartCardResponse(SmartCardReaderResponseModel model);
        public SmartCardReaderResponseModel GetSmartCardResponse(string wsConnectionId);
        public bool DeleteSmartCardResponse(string wsConnectionId);
    }
}
