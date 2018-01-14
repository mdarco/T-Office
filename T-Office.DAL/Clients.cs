using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using T_Office.DAL.DBModel;
using T_Office.Models;

namespace T_Office.DAL
{
    public static class Clients
    {
        public static ApiTableResponseModel<ClientModel> GetClientsFiltered(ClientFilterModel filter)
        {
            using (var ctx = new TOfficeEntities())
            {
                ApiTableResponseModel<ClientModel> response = new ApiTableResponseModel<ClientModel>();

                if (filter != null)
                {
                    var q = ctx.Clients
                            .Include("ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData")
                            .AsQueryable();

                    if (!string.IsNullOrEmpty(filter.JMBG))
                    {
                        q = q.Where(x => x.JMBG.StartsWith(filter.JMBG));
                    }

                    if (!string.IsNullOrEmpty(filter.PIB))
                    {
                        q = q.Where(x => x.PIB.StartsWith(filter.PIB));
                    }

                    if (!string.IsNullOrEmpty(filter.CompanyName))
                    {
                        q = q.Where(x => x.CompanyName.ToLower().Contains(filter.CompanyName.ToLower()));
                    }

                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        q = q.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(filter.Name.ToLower()));
                    }

                    if (!string.IsNullOrEmpty(filter.VehicleRegNo))
                    {
                        q = q.Where(x => x.ClientRegistrationDocumentData.Any(crdd => crdd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber.ToLower() == filter.VehicleRegNo.ToLower()));
                    }

                    // paging & sorting
                    if (string.IsNullOrEmpty(filter.OrderByClause))
                    {
                        // default order
                        filter.OrderByClause = "CompanyName, FirstName, LastName";
                    }

                    if (filter.PageNo < 1)
                    {
                        filter.PageNo = 1;
                    }

                    if (filter.RecordsPerPage < 1)
                    {
                        // unlimited
                        filter.RecordsPerPage = 1000000;
                    }

                    response.Total = q.Count();

                    var Data =
                        q.ToList()
                            .Select(x =>
                                new ClientModel()
                                {
                                    ID = x.ID,
                                    JMBG = x.JMBG,
                                    PIB = x.PIB,
                                    CompanyName = x.CompanyName,
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    FullName = x.FirstName + " " + x.LastName,
                                    Address = x.Address,
                                    ClientType = (!string.IsNullOrEmpty(x.JMBG)) ? "Fizičko lice" : "Pravno lice",

                                    Vehicles = 
                                        x.ClientRegistrationDocumentData
                                            .Select(dd => 
                                                new VehicleDataModel()
                                                {
                                                    ID = dd.RegistrationDocumentData.RegistrationVehicleData.ID,
                                                    VehicleIDNumber = dd.RegistrationDocumentData.RegistrationVehicleData.VehicleIDNumber,
                                                    Make = dd.RegistrationDocumentData.RegistrationVehicleData.Make,
                                                    Model = dd.RegistrationDocumentData.RegistrationVehicleData.Model,
                                                    RegistrationNumber = dd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber
                                                }
                                            )
                                            .ToList()
                                }
                            )
                            .OrderBy(filter.OrderByClause)
                            .Skip(((int)filter.PageNo - 1) * (int)filter.RecordsPerPage)
                            .Take((int)filter.RecordsPerPage);

                    response.Data = Data;
                }

                return response;
            }
        }

        public static void AddClient(TOfficeEntities ctx, ClientModel model)
        {
            if (model != null)
            {
                DBModel.Clients client = new DBModel.Clients();
                client.JMBG = model.JMBG;
                client.PIB = model.PIB;
                client.CompanyName = model.CompanyName;
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.Address = model.Address;
                client.Phone = model.Phone;
                client.RecommendedBy = model.RecommendedBy;

                ctx.Clients.Add(client);
            }
            else
            {
                throw new Exception("Ne postoje podaci o klijentu.");
            }
        }
    }
}
