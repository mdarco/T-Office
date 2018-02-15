using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using T_Office.Models;

namespace T_Office.Test
{
    [TestClass]
    public class Clients
    {
        [TestMethod]
        public void GetClientsDue()
        {
            const int NUMBER_OF_DAYS = 10;

            List<ClientDueModel> clientsDue = DAL.Clients.GetClientsDue(NUMBER_OF_DAYS);
        }
    }
}
