﻿using System;
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

                                    FullOwnerName = x.OwnerName + "" + x.OwnerSurnameOrBusinessName,
                                    FullUserName = x.UserName + "" + x.UserSurnameOrBusinessName,
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

                            FullOwnerName = client.OwnerName + "" + client.OwnerSurnameOrBusinessName,
                            FullUserName = client.UserName + "" + client.UserSurnameOrBusinessName,
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
                        bool existingOwnerName = Exist(model);
                        if (existingOwnerName)
                        {
                            throw new Exception("Klijent vec postoji.");
                        }

                        client.OwnerName = model.OwnerName;
                    }

                    if (!string.IsNullOrEmpty(model.OwnerSurnameOrBusinessName))
                    {
                        bool existingOwnerSurname = Exist(model);
                        if (existingOwnerSurname)
                        {
                            throw new Exception("Klijent vec postoji.");
                        }

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

        #region Analytics

        public static List<ClientDueModel> GetClientsDue(int numberOfDays)
        {
            using (var ctx = new TOfficeEntities())
            {
                var installments = ctx.VehicleRegistrationInstallments
                                        .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData)
                                        .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
                                        .Where(x => 
                                            (DbFunctions.TruncateTime(x.InstallmentDate) <= DbFunctions.TruncateTime(DbFunctions.AddDays(DateTime.Now, numberOfDays))) && !x.IsPaid &&
                                            (DbFunctions.TruncateTime(x.InstallmentDate) >= DbFunctions.TruncateTime(DateTime.Now))
                                         )
                                        .ToList();

                return installments.Select(x =>
                    new ClientDueModel()
                    {
                        ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
                        FullOwnerName = 
                            x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName,
                        FullUserName =
                            x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.UserName + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.UserSurnameOrBusinessName,
                        RegistrationDate = x.VehicleRegistrations.RegistrationDate,
                        FullVehicleName =
                            x.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Make + x.VehicleRegistrations.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.Model,
                        InstallmentAmount = x.Amount,
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
                            .Where(x => !x.IsPaid && DbFunctions.TruncateTime(x.InstallmentDate) < DbFunctions.TruncateTime(DateTime.Now))
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
                                    TotalAmount = gr.Sum(item => item.Amount)
                                }
                            )
                            .OrderByDescending(x => x.TotalAmount)
                            .ToList();
            }
        }

        #endregion

        #region Reports

        public static List<CostsByPeriodModel> GetCostsByPeriod(CostsByPeriodFilter filter)
        {
            using (var ctx = new TOfficeEntities())
            {
                if (!filter.DateFrom.HasValue)
                {
                    // set min date
                    filter.DateFrom = new DateTime(1950, 1, 1);
                }

                if (!filter.DateTo.HasValue)
                {
                    filter.DateTo = DateTime.Now;
                }

                var credits = ctx.VehicleRegistrations
                                    .Include(t => t.ClientRegistrationDocumentData.Clients)
                                    .Where(vr =>
                                        (DbFunctions.TruncateTime(vr.RegistrationDate) >= DbFunctions.TruncateTime(filter.DateFrom)) &&
                                        DbFunctions.TruncateTime(vr.RegistrationDate) <= DbFunctions.TruncateTime(filter.DateTo)
                                    )
                                    .GroupBy(x =>
                                        new
                                        {
                                            ClientID = x.ClientRegistrationDocumentData.ClientID,
                                            Owner = (x.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
                                            User = (x.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName)
                                        }
                                    )
                                    .Select(gr =>
                                        new CostsByPeriodModel()
                                        {
                                            ClientID = gr.Key.ClientID,
                                            Owner = gr.Key.Owner,
                                            User = gr.Key.User,
                                            TotalCreditAmount = gr.Sum(item => item.TotalAmount)
                                        }
                                    )
                                    .ToList();

                var debts = ctx.VehicleRegistrationInstallments
                                    .Include(t => t.VehicleRegistrations.ClientRegistrationDocumentData.Clients)
                                    .Where(vri => !vri.IsPaid &&
                                        (DbFunctions.TruncateTime(vri.InstallmentDate) >= DbFunctions.TruncateTime(filter.DateFrom)) &&
                                        (DbFunctions.TruncateTime(vri.InstallmentDate) <= DbFunctions.TruncateTime(filter.DateTo))
                                    )
                                    .GroupBy(x =>
                                        new
                                        {
                                            ClientID = x.VehicleRegistrations.ClientRegistrationDocumentData.ClientID,
                                            Owner = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName),
                                            User = (x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerName + " " + x.VehicleRegistrations.ClientRegistrationDocumentData.Clients.OwnerSurnameOrBusinessName)
                                        }
                                    )
                                    .Select(gr =>
                                        new CostsByPeriodModel()
                                        {
                                            ClientID = gr.Key.ClientID,
                                            Owner = gr.Key.Owner,
                                            User = gr.Key.User,
                                            TotalDebtAmount = gr.Sum(item => item.Amount)
                                        }
                                    )
                                    .ToList();

                return credits.Join(debts,
                            c => c.ClientID,
                            d => d.ClientID,
                            (c, d) => new CostsByPeriodModel()
                            {
                                ClientID = c.ClientID,
                                Owner = c.Owner,
                                User = c.User,
                                TotalCreditAmount = c.TotalCreditAmount,
                                TotalDebtAmount = d.TotalDebtAmount
                            }
                       )
                       .ToList();
            }
        }

        #endregion
    }
}
