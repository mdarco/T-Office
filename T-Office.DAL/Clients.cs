using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using T_Office.DAL.DBModel;
using T_Office.Models;
using System.Data.SqlClient;
using Dapper;

namespace T_Office.DAL
{
    public static class Clients
    {
        private static readonly string connectionString = DALHelper.GetSqlConnectionStringFromEF();

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
                        q = q.Where(x => x.ClientRegistrationDocumentData.Any(crdd => crdd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber.ToLower().Contains(filter.VehicleRegNo.ToLower())));
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

                                    FullOwnerName = x.OwnerName + " " + x.OwnerSurnameOrBusinessName,
                                    FullUserName = x.UserName + " " + x.UserSurnameOrBusinessName,
                                    OwnerJMBGMB = x.OwnerPersonalNo,
                                    UserJMBGMB = x.UserPersonalNo,
                                    RecommendedBy = x.RecommendedBy,

                                    Vehicles = 
                                        x.ClientRegistrationDocumentData
                                            .Select(dd => 
                                                new VehicleDataModel()
                                                {
                                                    ID = dd.RegistrationDocumentData.RegistrationVehicleData.ID,
                                                    VehicleIDNumber = dd.RegistrationDocumentData.RegistrationVehicleData.VehicleIDNumber,
                                                    Make = dd.RegistrationDocumentData.RegistrationVehicleData.Make,
                                                    Model = dd.RegistrationDocumentData.RegistrationVehicleData.Model,
                                                    RegistrationNumber = dd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber,
                                                    FirstRegistrationDate = dd.RegistrationDocumentData.RegistrationVehicleData.FirstRegistrationDate
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

        public static ClientModel GetClient(int id)
        {
            using (var ctx = new TOfficeEntities())
            {
                var client = ctx.Clients
                                    .Include("ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData")
                                    .FirstOrDefault(c => c.ID == id);

                if (client != null)
                {
                    return
                        new ClientModel()
                        {
                            ID = client.ID,

                            OwnerPersonalNo = client.OwnerPersonalNo,
                            OwnerPIB = client.OwnerPIB,
                            OwnerName = client.OwnerName,
                            OwnerSurnameOrBusinessName = client.OwnerSurnameOrBusinessName,
                            OwnerAddress = client.OwnerAddress,
                            OwnerPhone = client.OwnerPhone,
                            OwnerEmail = client.OwnerEmail,

                            UserPersonalNo = client.UserPersonalNo,
                            UserPIB = client.UserPIB,
                            UserName = client.UserName,
                            UserSurnameOrBusinessName = client.UserSurnameOrBusinessName,
                            UserAddress = client.UserAddress,
                            UserPhone = client.UserPhone,
                            UserEmail = client.UserEmail,

                            FullOwnerName = client.OwnerName + " " + client.OwnerSurnameOrBusinessName,
                            FullUserName = client.UserName + " " + client.UserSurnameOrBusinessName,
                            OwnerJMBGMB = client.OwnerPersonalNo,
                            UserJMBGMB = client.UserPersonalNo,
                            RecommendedBy = client.RecommendedBy,

                            Vehicles =
                                client.ClientRegistrationDocumentData
                                    .Select(dd =>
                                        new VehicleDataModel()
                                        {
                                            ID = dd.RegistrationDocumentData.RegistrationVehicleData.ID,
                                            VehicleIDNumber = dd.RegistrationDocumentData.RegistrationVehicleData.VehicleIDNumber,
                                            Make = dd.RegistrationDocumentData.RegistrationVehicleData.Make,
                                            Model = dd.RegistrationDocumentData.RegistrationVehicleData.Model,
                                            RegistrationNumber = dd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber,
                                            FirstRegistrationDate = dd.RegistrationDocumentData.RegistrationVehicleData.FirstRegistrationDate
                                        }
                                    )
                                    .ToList()
                        };
                }
                else
                {
                    throw new Exception("Klijent ne postoji.");
                }
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

        public static void AddClient(ClientModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                if (model != null)
                {
                    bool clientExists = Exist(model);
                    if (clientExists)
                    {
                        throw new Exception("Klijent vec postoji.");
                    }

                    DBModel.Clients client = new DBModel.Clients()
                    {
                        OwnerPersonalNo = model.OwnerPersonalNo,
                        OwnerPIB = model.OwnerPIB,
                        OwnerName = model.OwnerName,
                        OwnerSurnameOrBusinessName = model.OwnerSurnameOrBusinessName,
                        OwnerAddress = model.OwnerAddress,
                        OwnerPhone = model.OwnerPhone,
                        OwnerEmail = model.OwnerEmail,

                        UserPersonalNo = model.UserPersonalNo,
                        UserPIB = model.UserPIB,
                        UserName = model.UserName,
                        UserSurnameOrBusinessName = model.UserSurnameOrBusinessName,
                        UserAddress = model.UserAddress,
                        UserPhone = model.UserPhone,
                        UserEmail = model.UserEmail,

                        RecommendedBy = model.RecommendedBy
                    };

                    ctx.Clients.Add(client);

                    try
                    {
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

        public static int? AddClient(TOfficeEntities ctx, ClientModel model, bool saveChanges)
        {
            if (model != null)
            {
                DBModel.Clients client = new DBModel.Clients()
                {
                    OwnerPersonalNo = model.OwnerPersonalNo,
                    OwnerPIB = model.OwnerPIB,
                    OwnerName = model.OwnerName,
                    OwnerSurnameOrBusinessName = model.OwnerSurnameOrBusinessName,
                    OwnerAddress = model.OwnerAddress,
                    OwnerPhone = model.OwnerPhone,
                    OwnerEmail = model.OwnerEmail,

                    UserPersonalNo = model.UserPersonalNo,
                    UserPIB = model.UserPIB,
                    UserName = model.UserName,
                    UserSurnameOrBusinessName = model.UserSurnameOrBusinessName,
                    UserAddress = model.UserAddress,
                    UserPhone = model.UserPhone,
                    UserEmail = model.UserEmail,

                    RecommendedBy = model.RecommendedBy
                };

                ctx.Clients.Add(client);

                if (saveChanges)
                {
                    ctx.SaveChanges();
                }

                return client.ID;
            }
            else
            {
                throw new Exception("Ne postoje podaci o klijentu.");
            }
        }

        public static void FullClientEntry(RegistrationDataModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                if (model != null)
                {
                    RegLicenseDataExistModel existModel = SimpleExist(model);

                    int? clientID = null;
                    int? vehicleID = null;
                    int? regDocID = null;
                    int? clientRegDocID = null;

                    // client
                    if (!existModel.IsExistingOwner)
                    {
                        clientID = AddClient(ctx, model.PersonalData, false);
                    }
                    else
                    {
                        var existingOwner =
                                ctx.Clients.FirstOrDefault(x =>
                                    (!string.IsNullOrEmpty(x.OwnerPersonalNo) && (x.OwnerPersonalNo.Trim() == model.PersonalData.OwnerPersonalNo.Trim())) ||
                                    (!string.IsNullOrEmpty(x.OwnerName.Trim() + x.OwnerSurnameOrBusinessName.Trim()) && (x.OwnerName.Trim() + x.OwnerSurnameOrBusinessName.Trim() == model.PersonalData.OwnerName.Trim() + model.PersonalData.OwnerSurnameOrBusinessName.Trim()))
                                );

                        if (existingOwner != null)
                        {
                            clientID = existingOwner.ID;
                        }
                    }

                    // vehicle (+ registration data)
                    if (!existModel.IsExistingVehicle)
                    {
                        vehicleID = Vehicles.Add(ctx, clientID, model.VehicleData, false);
                        regDocID = RegistrationDocuments.Add(ctx, clientID, vehicleID, model.DocumentData, false);
                        clientRegDocID = RegistrationDocuments.AddClientRegDocData(ctx, clientID, regDocID, false);
                    }
                    else
                    {
                        var existingVehicle =
                                ctx.RegistrationVehicleData.FirstOrDefault(x =>
                                    x.RegistrationNumber.Trim() == model.VehicleData.RegistrationNumber.Trim()
                                );

                        if (existingVehicle != null)
                        {
                            vehicleID = existingVehicle.ID;
                        }

                        var existingClientRegDoc = ctx.ClientRegistrationDocumentData.FirstOrDefault(x =>
                                                        x.ClientID == clientID && x.RegistrationDocumentDataID == regDocID
                                                   );

                        if (existingClientRegDoc != null)
                        {
                            clientRegDocID = existingClientRegDoc.ID;
                        }
                    }

                    // vehicle registration
                    Vehicles.AddRegistration(ctx, clientRegDocID, model.VehicleRegistrationData, false);

                    ctx.SaveChanges();
                }
                else
                {
                    throw new Exception("Ne postoje podaci o klijentu.");
                }
            }
        }

        public static void EditClient(int id, ClientModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                var client = ctx.Clients.FirstOrDefault(c => c.ID == id);
                if (client != null)
                {
                    if (!string.IsNullOrEmpty(model.OwnerPersonalNo))
                    {
                        var existingOwnerPersonalNo = ctx.Clients.FirstOrDefault(x => x.OwnerPersonalNo.ToLower() == model.OwnerPersonalNo.ToLower());
                        if (existingOwnerPersonalNo != null)
                        {
                            throw new Exception("Klijent vec postoji (JMBG/MB).");
                        }

                        client.OwnerPersonalNo = model.OwnerPersonalNo;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerPIB))
                    {
                        var existingOwnerPIB = ctx.Clients.FirstOrDefault(x => x.OwnerPIB.ToLower() == model.OwnerPIB.ToLower());
                        if (existingOwnerPIB != null)
                        {
                            throw new Exception("Klijent vec postoji (PIB).");
                        }

                        client.OwnerPIB = model.OwnerPIB;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerName))
                    {
                        client.OwnerName = model.OwnerName;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerSurnameOrBusinessName))
                    {
                        client.OwnerSurnameOrBusinessName = model.OwnerSurnameOrBusinessName;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerAddress))
                    {
                        client.OwnerAddress = model.OwnerAddress;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerPhone))
                    {
                        client.OwnerPhone = model.OwnerPhone;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerEmail))
                    {
                        client.OwnerEmail = model.OwnerEmail;
                    }

                    if (!string.IsNullOrEmpty(model.UserPersonalNo))
                    {
                        client.UserPersonalNo = model.UserPersonalNo;
                    }

                    if (!string.IsNullOrEmpty(model.UserPIB))
                    {
                        client.UserPIB = model.UserPIB;
                    }

                    if (!string.IsNullOrEmpty(model.UserName))
                    {
                        client.UserName = model.UserName;
                    }

                    if (!string.IsNullOrEmpty(model.UserSurnameOrBusinessName))
                    {
                        client.UserSurnameOrBusinessName = model.UserSurnameOrBusinessName;
                    }

                    if (!string.IsNullOrEmpty(model.UserAddress))
                    {
                        client.UserAddress = model.UserAddress;
                    }

                    if (!string.IsNullOrEmpty(model.UserPhone))
                    {
                        client.UserPhone = model.UserPhone;
                    }

                    if (!string.IsNullOrEmpty(model.UserEmail))
                    {
                        client.UserEmail = model.UserEmail;
                    }

                    if (!string.IsNullOrEmpty(model.RecommendedBy))
                    {
                        client.RecommendedBy = model.RecommendedBy;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        public static bool Exist(ClientModel clientModel)
        {
            using (var ctx = new TOfficeEntities())
            {
                DBModel.Clients existing = null;

                // use cases:
                // person owner - person user
                // person owner only
                if (!string.IsNullOrEmpty(clientModel.OwnerPersonalNo) && !string.IsNullOrEmpty(clientModel.UserPersonalNo))
                {
                    // person owner - person user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerPersonalNo.ToLower() == clientModel.OwnerPersonalNo.ToLower() &&
                                        x.UserPersonalNo.ToLower() == clientModel.UserPersonalNo.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerPersonalNo))
                {
                    // person owner only
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerPersonalNo.ToLower() == clientModel.OwnerPersonalNo.ToLower()
                                    );
                }

                // use cases: 
                // legal entity owner - legal entity user
                // legal entity owner - person user
                // legal entity owner only
                if (!string.IsNullOrEmpty(clientModel.OwnerPIB) && !string.IsNullOrEmpty(clientModel.UserPIB))
                {
                    // legal entity owner - legal entity user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerPIB.ToLower() == clientModel.UserPIB.ToLower() &&
                                        x.OwnerPIB.ToLower() == clientModel.UserPIB.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerPIB) && !string.IsNullOrEmpty(clientModel.UserPersonalNo))
                {
                    // legal entity owner - person user
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerPIB.ToLower() == clientModel.OwnerPIB.ToLower() &&
                                        x.UserPersonalNo.ToLower() == clientModel.UserPersonalNo.ToLower()
                                    );
                }
                else if (!string.IsNullOrEmpty(clientModel.OwnerPIB))
                {
                    // legal entity owner only
                    existing = ctx.Clients
                                    .FirstOrDefault(x =>
                                        x.OwnerPIB.ToLower() == clientModel.OwnerPIB.ToLower()
                                    );
                }

                return (existing != null);
            }
        }

        public static RegLicenseDataExistModel SimpleExist(RegistrationDataModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                RegLicenseDataExistModel result = new RegLicenseDataExistModel()
                {
                    IsExistingOwner = false, IsExistingUser = false, IsExistingVehicle = false
                };

                var existingOwner =
                    ctx.Clients.FirstOrDefault(x =>
                        (!string.IsNullOrEmpty(x.OwnerPersonalNo) && (x.OwnerPersonalNo.Trim() == model.PersonalData.OwnerPersonalNo.Trim())) ||
                        (!string.IsNullOrEmpty(x.OwnerName.Trim() + x.OwnerSurnameOrBusinessName.Trim()) && (x.OwnerName.Trim() + x.OwnerSurnameOrBusinessName.Trim() == model.PersonalData.OwnerName.Trim() + model.PersonalData.OwnerSurnameOrBusinessName.Trim()))
                    );
                result.IsExistingOwner = (existingOwner != null);

                var existingUser =
                    ctx.Clients.FirstOrDefault(x =>
                        (!string.IsNullOrEmpty(x.UserPersonalNo) && (x.UserPersonalNo.Trim() == model.PersonalData.UserPersonalNo.Trim())) ||
                        (!string.IsNullOrEmpty(x.UserName.Trim() + x.UserSurnameOrBusinessName.Trim()) && (x.UserName.Trim() + x.UserSurnameOrBusinessName.Trim() == model.PersonalData.UserName.Trim() + model.PersonalData.UserSurnameOrBusinessName.Trim()))
                    );
                result.IsExistingUser = (existingUser != null);

                var existingVehicle =
                    ctx.RegistrationVehicleData.FirstOrDefault(x =>
                        x.RegistrationNumber.Trim() == model.VehicleData.RegistrationNumber.Trim()
                    );
                result.IsExistingVehicle = (existingVehicle != null);

                return result;
            }
        }

        public static List<VehicleDataModel> GetVehicles(int clientID)
        {
            List<VehicleDataModel> result = new List<VehicleDataModel>();

            using (var ctx = new TOfficeEntities())
            {
                var client = ctx.Clients
                            .Include("ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData")
                            .FirstOrDefault(c => c.ID == clientID);

                if (client != null)
                {
                    result = client.ClientRegistrationDocumentData
                                .Select(dd =>
                                    new VehicleDataModel()
                                    {
                                        ID = dd.RegistrationDocumentData.RegistrationVehicleData.ID,
                                        VehicleIDNumber = dd.RegistrationDocumentData.RegistrationVehicleData.VehicleIDNumber,
                                        Make = dd.RegistrationDocumentData.RegistrationVehicleData.Make,
                                        Model = dd.RegistrationDocumentData.RegistrationVehicleData.Model,
                                        RegistrationNumber = dd.RegistrationDocumentData.RegistrationVehicleData.RegistrationNumber,
                                        FirstRegistrationDate = dd.RegistrationDocumentData.RegistrationVehicleData.FirstRegistrationDate
                                    }
                                )
                                .ToList();
                }
            }

            return result;
        }

        public static void DeleteClient(int id)
        {
            using (var ctx = new TOfficeEntities())
            {
                var client = 
                    ctx.Clients
                        .Include("ClientRegistrationDocumentData.VehicleRegistrations.VehicleRegistrationInstallments")
                        .FirstOrDefault(c => c.ID == id);

                if (client != null)
                {
                    for (int i = client.ClientRegistrationDocumentData.Count() - 1; i >= 0; i--)
                    {
                        var regDoc = client.ClientRegistrationDocumentData.ElementAt(i);

                        for (int j = regDoc.VehicleRegistrations.Count() - 1; j >= 0; j--)
                        {
                            var reg = regDoc.VehicleRegistrations.ElementAt(j);

                            for (int k = reg.VehicleRegistrationInstallments.Count() -1; k >= 0; k--)
                            {
                                ctx.VehicleRegistrationInstallments.Remove(reg.VehicleRegistrationInstallments.ElementAt(k));
                            }

                            ctx.VehicleRegistrations.Remove(reg);
                        }

                        ctx.ClientRegistrationDocumentData.Remove(regDoc);
                    }

                    ctx.Clients.Remove(client);

                    ctx.SaveChanges();
                }
            }
        }

        #region Analytics

        public static List<ClientDueModel> GetClientsDue(int numberOfDays)
        {
            using (var ctx = new TOfficeEntities())
            {
                var installments = 
                    ctx.VehicleRegistrationInstallments
                        .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData)
                        .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
                        .Where(x => 
                            (DbFunctions.TruncateTime(x.InstallmentDate) <= DbFunctions.TruncateTime(DbFunctions.AddDays(DateTime.Now, numberOfDays))) &&
                            (DbFunctions.TruncateTime(x.InstallmentDate) >= DbFunctions.TruncateTime(DateTime.Now)) &&
                            !x.IsPaid
                            )
                        .ToList();

                return installments.Select(x =>
                    new ClientDueModel()
                    {
                        ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
                        FullOwnerName = 
                            x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName,
                        FullUserName =
                            x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.UserName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.UserSurnameOrBusinessName,
                        RegistrationDate = x.VehicleRegistrations.RegistrationDate,
                        FullVehicleName =
                            x.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Make + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Model,
                        InstallmentAmount = x.Amount,
                        InstallmentPaidAmount = x.PaidAmount,
                        InstallmentDate = x.InstallmentDate
                    }
                )
                .OrderByDescending(x => x.InstallmentDate)
                .ToList();
            }
        }

        public static List<ClientTotalOutstandingModel> GetClientsOutstandingTotal()
        {
            using (var ctx = new TOfficeEntities())
            {
                return ctx.VehicleRegistrationInstallments
                            .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
                            .Where(x => DbFunctions.TruncateTime(x.InstallmentDate) < DbFunctions.TruncateTime(DateTime.Now))
                            .GroupBy(x =>
                                new
                                {
                                    ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
                                    Owner = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
                                    User = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName)
                                }
                            )
                            .Select(gr =>
                                new ClientTotalOutstandingModel()
                                {
                                    ClientID = gr.Key.ClientID,
                                    Owner = gr.Key.Owner,
                                    User = gr.Key.User,
                                    TotalAmount = gr.Sum(item => !item.PaidAmount.HasValue ? item.Amount : item.Amount - item.PaidAmount)
                                }
                            )
                            .OrderByDescending(x => x.TotalAmount)
                            .ToList();
            }
        }

        public static List<ClientDueModel> GetVehiclesWithIncomingRegistrations(int numberOfDays)
        {
            using (var ctx = new TOfficeEntities())
            {
                var registrations = ctx.VehicleRegistrations
                                            .Include(t => t.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData)
                                            .Include(t => t.ClientRegistrationDocumentData.Clients)
                                            .Where(x => 
                                                (DbFunctions.TruncateTime(x.NextRegistrationDate) <= DbFunctions.TruncateTime(DbFunctions.AddDays(DateTime.Now, numberOfDays))) &&
                                                (DbFunctions.TruncateTime(x.NextRegistrationDate) >= DbFunctions.TruncateTime(DateTime.Now))
                                            )
                                            .OrderByDescending(x => x.NextRegistrationDate)
                                            .ToList();

                return registrations.Select(x =>
                    new ClientDueModel()
                    {
                        ClientID = x.ClientRegistrationDocumentData.ClientID,
                        FullOwnerName =
                            x.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName,
                        FullUserName =
                            x.ClientRegistrationDocumentData.Clients.UserName + " " + x.ClientRegistrationDocumentData.Clients.UserSurnameOrBusinessName,
                        RegistrationDate = x.RegistrationDate,
                        NextRegistrationDate = x.NextRegistrationDate,
                        FullVehicleName =
                            x.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Make + " " + x.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Model
                    }
                )
                .OrderByDescending(x => x.InstallmentDate)
                .ToList();
            }
        }

        #endregion

        #region Reports

        //public static List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        //{
        //    using (var ctx = new TOfficeEntities())
        //    {
        //        if (!filter.DateFrom.HasValue)
        //        {
        //            // set min date
        //            filter.DateFrom = new DateTime(1950, 1, 1);
        //        }

        //        if (!filter.DateTo.HasValue)
        //        {
        //            filter.DateTo = DateTime.Now;
        //        }

        //        var credits = ctx.VehicleRegistrations
        //                            .Include(t => t.ClientRegistrationDocumentData.Clients)
        //                            .Where(vr =>
        //                                (DbFunctions.TruncateTime(vr.RegistrationDate) >= DbFunctions.TruncateTime(filter.DateFrom)) &&
        //                                DbFunctions.TruncateTime(vr.RegistrationDate) <= DbFunctions.TruncateTime(filter.DateTo)
        //                            )
        //                            .GroupBy(x =>
        //                                new
        //                                {
        //                                    ClientID = x.ClientRegistrationDocumentData.ClientID,
        //                                    Owner = (x.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
        //                                    User = (x.ClientRegistrationDocumentData.Clients.UserName + " " + x.ClientRegistrationDocumentData.Clients.UserSurnameOrBusinessName)
        //                                }
        //                            )
        //                            .Select(gr =>
        //                                new CostsByPeriodModel()
        //                                {
        //                                    ClientID = gr.Key.ClientID,
        //                                    Owner = gr.Key.Owner,
        //                                    User = gr.Key.User,
        //                                    TotalCreditAmount = gr.Sum(item => item.TotalAmount)
        //                                }
        //                            )
        //                            .ToList();

        //        var debts = ctx.VehicleRegistrationInstallments
        //                            .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
        //                            .Where(vri => /* !vri.IsPaid && vri.VehicleRegistrations.NumberOfInstallments > 1 && */
        //                                (DbFunctions.TruncateTime(vri.InstallmentDate) >= DbFunctions.TruncateTime(filter.DateFrom)) &&
        //                                (DbFunctions.TruncateTime(vri.InstallmentDate) <= DbFunctions.TruncateTime(filter.DateTo))
        //                            )
        //                            .GroupBy(x =>
        //                                new
        //                                {
        //                                    ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
        //                                    Owner = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
        //                                    User = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName)
        //                                }
        //                            )
        //                            .Select(gr =>
        //                                new CostsByPeriodModel()
        //                                {
        //                                    ClientID = gr.Key.ClientID,
        //                                    Owner = gr.Key.Owner,
        //                                    User = gr.Key.User,
        //                                    //TotalDebtAmount = gr.Sum(item => item.Amount)
        //                                    TotalDebtAmount = gr.Sum(item => !item.PaidAmount.HasValue ? item.Amount : item.Amount - item.PaidAmount)
        //                                }
        //                            )
        //                            .ToList();

        //        var installmentsPaid = ctx.VehicleRegistrationInstallments
        //                                    .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
        //                                    .Where(vri => vri.IsPaid && /* vri.VehicleRegistrations.NumberOfInstallments > 1 && */
        //                                        (DbFunctions.TruncateTime(vri.PaymentDate) >= DbFunctions.TruncateTime(filter.DateFrom)) &&
        //                                        (DbFunctions.TruncateTime(vri.PaymentDate) <= DbFunctions.TruncateTime(filter.DateTo))
        //                                    )
        //                                    .GroupBy(x =>
        //                                        new
        //                                        {
        //                                            ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
        //                                            Owner = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
        //                                            User = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName)
        //                                        }
        //                                    )
        //                                    .Select(gr =>
        //                                        new CostsByPeriodModel()
        //                                        {
        //                                            ClientID = gr.Key.ClientID,
        //                                            Owner = gr.Key.Owner,
        //                                            User = gr.Key.User,
        //                                            //TotalDebtAmount = gr.Sum(item => item.Amount)
        //                                            //TotalDebtAmount = gr.Sum(item => !item.PaidAmount.HasValue ? item.Amount : item.Amount - item.PaidAmount)
        //                                            TotalInstallmentsPaid = gr.Sum(item => item.PaidAmount)
        //                                        }
        //                                    )
        //                                    .ToList();

        //        // perform left outer join (done with GroupJoin() in EF)
        //        var joined = credits.GroupJoin(debts,
        //                    c => c.ClientID,
        //                    d => d.ClientID,
        //                    (c, d) => new { c, d }
        //               )
        //               .SelectMany(
        //                    x => x.d.DefaultIfEmpty(),
        //                    (c, d) => new CostsByPeriodModel()
        //                    {
        //                        ClientID = c.c.ClientID,
        //                        Owner = c.c.Owner,
        //                        User = c.c.User,
        //                        TotalCreditAmount = c.c.TotalCreditAmount,
        //                        TotalDebtAmount = (d != null) ? d.TotalDebtAmount : 0
        //                    }
        //                )
        //                //.Where(x => x.TotalDebtAmount > 0)
        //                .OrderByDescending(x => x.TotalDebtAmount)
        //                .ToList();

        //        return joined.GroupJoin(installmentsPaid,
        //                    c => c.ClientID,
        //                    d => d.ClientID,
        //                    (c, d) => new { c, d }
        //               )
        //               .SelectMany(
        //                    x => x.d.DefaultIfEmpty(),
        //                    (c, d) => new CostsByPeriodModel()
        //                    {
        //                        ClientID = c.c.ClientID,
        //                        Owner = c.c.Owner,
        //                        User = c.c.User,
        //                        TotalCreditAmount = c.c.TotalCreditAmount,
        //                        TotalDebtAmount = (c.c != null) ? c.c.TotalDebtAmount : 0,
        //                        TotalInstallmentsPaid = (d != null) ? d.TotalInstallmentsPaid : 0
        //                    }
        //                )
        //                .ToList();
        //    }
        //}

        public static List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        {
            using (var ctx = new TOfficeEntities())
            {
                string spName = "dbo.Report_CostsByPeriod";

                List<CostsByPeriodModel> result = new List<CostsByPeriodModel>();

                using (var connection = new SqlConnection(connectionString))
                {
                    var spParams = new DynamicParameters();
                    spParams.Add("@dateFrom", filter.DateFrom, dbType: System.Data.DbType.DateTime);
                    spParams.Add("@dateTo", filter.DateTo, dbType: System.Data.DbType.DateTime);

                    result = connection.Query<CostsByPeriodModel>(spName, spParams, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

                return result;
            }
        }

        public static decimal? GetTotalInstallmentsAmount(DateTime? startDate, DateTime? endDate, bool? isPaid)
        {
            using (var ctx = new TOfficeEntities())
            {
                var q = ctx.VehicleRegistrationInstallments
                                .Include(t => t.VehicleRegistrations)
                                //.Where(x => x.VehicleRegistrations.NumberOfInstallments > 1)
                                .AsQueryable();

                if (isPaid.HasValue)
                {
                    q = q.Where(x => x.IsPaid == isPaid);
                }

                if (startDate.HasValue)
                {
                    if ((bool)isPaid)
                    {
                        q = q.Where(x => DbFunctions.TruncateTime(x.PaymentDate) >= DbFunctions.TruncateTime(startDate));
                    }
                    else
                    {
                        q = q.Where(x => DbFunctions.TruncateTime(x.InstallmentDate) >= DbFunctions.TruncateTime(startDate));
                    }
                }

                if (endDate.HasValue)
                {
                    if ((bool)isPaid)
                    {
                        q = q.Where(x => DbFunctions.TruncateTime(x.PaymentDate) <= DbFunctions.TruncateTime(endDate));
                    }
                    else
                    {
                        q = q.Where(x => DbFunctions.TruncateTime(x.InstallmentDate) <= DbFunctions.TruncateTime(endDate));
                    }
                }

                if (q.ToList().Count() > 0)
                {
                    return q.Sum(x => x.PaidAmount.HasValue ? x.Amount - (decimal)x.PaidAmount : x.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion
    }
}
