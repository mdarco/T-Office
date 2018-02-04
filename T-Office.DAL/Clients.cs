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
                        q = q.Where(x => x.OwnerPersonalNo.StartsWith(filter.JMBG) || x.UserPersonalNo.StartsWith(filter.JMBG));
                    }

                    if (!string.IsNullOrEmpty(filter.PIB))
                    {
                        q = q.Where(x => x.OwnerPIB.StartsWith(filter.PIB) || x.UserPIB.StartsWith(filter.PIB));
                    }

                    if (!string.IsNullOrEmpty(filter.ClientName))
                    {
                        q = q.Where(x => (x.OwnerName + " " + x.OwnerSurnameOrBusinessName).ToLower().Contains(filter.ClientName.ToLower()) || (x.UserName + " " + x.UserSurnameOrBusinessName).ToLower().Contains(filter.ClientName.ToLower()));
                    }

                    if (!string.IsNullOrEmpty(filter.VehicleRegNo))
                    {
                        q = q.Where(x => x.ClientRegistrationDocumentData.Any(crdd => crdd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber.ToLower() == filter.VehicleRegNo.ToLower()));
                    }

                    // paging & sorting
                    if (string.IsNullOrEmpty(filter.OrderByClause))
                    {
                        // default order
                        filter.OrderByClause = "OwnerName, UserName, OwnerSurnameOrBusinessName, UserSurnameOrBusinessName";
                    }

                    if (!filter.PageNo.HasValue || filter.PageNo < 1)
                    {
                        filter.PageNo = 1;
                    }

                    if (!filter.RecordsPerPage.HasValue || filter.RecordsPerPage < 1)
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

                                    OwnerPersonalNo = x.OwnerPersonalNo,
                                    OwnerPIB = x.OwnerPIB,
                                    OwnerName = x.OwnerName,
                                    OwnerSurnameOrBusinessName = x.OwnerSurnameOrBusinessName,
                                    OwnerAddress = x.OwnerAddress,
                                    OwnerPhone = x.OwnerPhone,
                                    OwnerEmail = x.OwnerEmail,

                                    UserPersonalNo = x.UserPersonalNo,
                                    UserPIB = x.UserPIB,
                                    UserName = x.UserName,
                                    UserSurnameOrBusinessName = x.UserSurnameOrBusinessName,
                                    UserAddress = x.UserAddress,
                                    UserPhone = x.UserPhone,
                                    UserEmail = x.UserEmail,

                                    FullOwnerName = x.OwnerName + "" + x.OwnerSurnameOrBusinessName,
                                    FullUserName = x.UserName + "" + x.UserSurnameOrBusinessName,
                                    OwnerJMBGMB = x.OwnerPersonalNo,
                                    UserJMBGMB = x.UserPersonalNo,

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

        public static void AddClientFull(RegistrationDataModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                if (model != null)
                {
                    var clientModel = model.PersonalData;

                    bool clientExists = Exist(clientModel);
                    if (clientExists)
                    {
                        throw new Exception("Klijent vec postoji.");
                    }

                    DBModel.Clients client = new DBModel.Clients()
                    {
                        OwnerPersonalNo = clientModel.OwnerPersonalNo,
                        OwnerPIB = clientModel.OwnerPIB,
                        OwnerName = clientModel.OwnerName,
                        OwnerSurnameOrBusinessName = clientModel.OwnerSurnameOrBusinessName,
                        OwnerAddress = clientModel.OwnerAddress,
                        OwnerPhone = clientModel.OwnerPhone,
                        OwnerEmail = clientModel.OwnerEmail,

                        UserPersonalNo = clientModel.UserPersonalNo,
                        UserPIB = clientModel.UserPIB,
                        UserName = clientModel.UserName,
                        UserSurnameOrBusinessName = clientModel.UserSurnameOrBusinessName,
                        UserAddress = clientModel.UserAddress,
                        UserPhone = clientModel.UserPhone,
                        UserEmail = clientModel.UserEmail,

                        RecommendedBy = clientModel.RecommendedBy
                    };

                    ctx.Clients.Add(client);

                    try
                    {
                        int registrationDocumentDataID = RegistrationDocuments.AddFromFullModel(ctx, model);

                        ClientRegistrationDocumentData clientRegDocData = new ClientRegistrationDocumentData()
                        {
                            ClientID = client.ID,
                            RegistrationDocumentDataID = registrationDocumentDataID
                        };

                        ctx.ClientRegistrationDocumentData.Add(clientRegDocData);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    throw new Exception("Ne postoje podaci o klijentu.");
                }
            }
        }

        public static bool Exist(PersonalDataModel clientModel)
        {
            using (var ctx = new TOfficeEntities())
            {
                DBModel.Clients existing = null;

                // use cases:
                // person owner - person user
                // person owner only
                if (!string.IsNullOrEmpty(clientModel.OwnerName) && !string.IsNullOrEmpty(clientModel.UserName))
                {
                    // person owner - person user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerName.ToLower() == clientModel.OwnerName.ToLower() &&
                                        x.UserName.ToLower() == clientModel.UserName.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerName))
                {
                    // person owner only
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerName.ToLower() == clientModel.OwnerName.ToLower()
                                    );
                }

                // use cases: 
                // legal entity owner - legal entity user
                // legal entity owner - person user
                // legal entity owner only
                if (!string.IsNullOrEmpty(clientModel.OwnerSurnameOrBusinessName) && !string.IsNullOrEmpty(clientModel.UserSurnameOrBusinessName))
                {
                    // legal entity owner - legal entity user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerSurnameOrBusinessName.ToLower() == clientModel.OwnerSurnameOrBusinessName.ToLower() &&
                                        x.UserSurnameOrBusinessName.ToLower() == clientModel.UserSurnameOrBusinessName.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerSurnameOrBusinessName) && !string.IsNullOrEmpty(clientModel.UserPersonalNo))
                {
                    // legal entity owner - person user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerSurnameOrBusinessName.ToLower() == clientModel.OwnerSurnameOrBusinessName.ToLower() &&
                                        x.UserName.ToLower() == clientModel.UserName.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerSurnameOrBusinessName))
                {
                    // legal entity owner only
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerSurnameOrBusinessName.ToLower() == clientModel.OwnerSurnameOrBusinessName.ToLower()
                                    );
                }

                return (existing != null);
            }
        }
    }
}
