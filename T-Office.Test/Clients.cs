using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using T_Office.Models;

namespace T_Office.Test
{
    [TestClass]
    public class Clients
    {
        //[TestMethod]
        //public void GetClientsDue()
        //{
        //    const int NUMBER_OF_DAYS = 10;

        //    List<ClientDueModel> clientsDue = DAL.Clients.GetClientsDue(NUMBER_OF_DAYS);
        //}

        //[TestMethod]
        //public void GetClientsOutstandingTotal()
        //{
        //    List<ClientTotalOutstandingModel> clientsOutstandingTotal = TOffice.DB.Clients.GetClientsOutstandingTotal();
        //}

        //[TestMethod]
        //public void GetCostsByPeriod()
        //{
        //    CostsByPeriodFilter filter = new CostsByPeriodFilter()
        //    {
        //        DateFrom = null,
        //        DateTo = null
        //    };

        //    List<CostsByPeriodModel> costsByPeriod = TOffice.DB.Clients.GetCostsByPeriod(filter);
        //}

        //[TestMethod]
        //public void GetClientsFiltered()
        //{
        //    ClientFilterModel filter = new ClientFilterModel
        //    {
        //        ClientName = "Aleksandar"
        //    };

        //    ApiTableResponseModel<ClientModel> result = TOffice.DB.Clients.GetClientsFiltered(filter);

        //    foreach(var item in result.Data)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Client: " + item.FullOwnerName);
        //    }

        //    Assert.IsTrue(result.Data.Count() > 0);
        //}

        //[TestMethod]
        //public void GetClient()
        //{
        //    ClientModel result = TOffice.DB.Clients.GetClient(10);

        //    System.Diagnostics.Debug.WriteLine("Client: " + result.FullOwnerName);

        //    Assert.IsTrue(result != null);
        //}
    }
}
