using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using T_Office.DAL.DBModel;
using T_Office.Models;
using System.Configuration;

namespace T_Office.DAL
{
    public static class Vehicles
    {
        const int FIRST_INSTALLMENT_DUE_PERIOD = 5;

        public static void AddFromFullModel(int clientID, RegistrationDataModel model)
        {
            var vehicleDataModel = model.VehicleData;

            using (var ctx = new TOfficeEntities())
            {
                var client = ctx.Clients.FirstOrDefault(c => c.ID == clientID);
                if (client == null)
                {
                    throw new Exception("Klijent ne postoji.");
                }

                try
                {
                    int registrationDocumentDataID = RegistrationDocuments.AddFromFullModel(ctx, model);

                    ClientRegistrationDocumentData clientRegDocData = new ClientRegistrationDocumentData()
                    {
                        ClientID = clientID,
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
        }

        public static int AddFromFullModel(TOfficeEntities ctx, RegistrationDataModel model)
        {
            var vehicleDataModel = model.VehicleData;

            RegistrationVehicleData regVehicleData = new RegistrationVehicleData()
            {
                Category = vehicleDataModel.Category,
                Load = vehicleDataModel.Load,
                Make = vehicleDataModel.Make,
                Mass = vehicleDataModel.Mass,
                MaxPermissibleLadenMass = vehicleDataModel.MaxPermissibleLadenMass,
                Color = vehicleDataModel.Color,
                EngineCapacity = vehicleDataModel.EngineCapacity,
                EngineIDNumber = vehicleDataModel.EngineIDNumber,
                EnginePowerKW = vehicleDataModel.EnginePowerKW,
                FirstRegistrationDate = vehicleDataModel.FirstRegistrationDate,
                FuelType = vehicleDataModel.FuelType,
                Model = vehicleDataModel.Model,
                NumberOfAxles = vehicleDataModel.NumberOfAxles,
                NumberOfSeats = vehicleDataModel.NumberOfSeats,
                NumberOfStandingPlaces = vehicleDataModel.NumberOfStandingPlaces,
                PowerWeightRatio = vehicleDataModel.PowerWeightRatio,
                ProductionYear = vehicleDataModel.ProductionYear,
                RegistrationNumber = vehicleDataModel.RegistrationNumber,
                RestrictionToChangeOwner = vehicleDataModel.RestrictionToChangeOwner,
                Type = vehicleDataModel.Type,
                TypeApprovalNumber = vehicleDataModel.TypeApprovalNumber,
                VehicleIDNumber = vehicleDataModel.VehicleIDNumber
            };

            ctx.RegistrationVehicleData.Add(regVehicleData);

            return regVehicleData.ID;
        }

        #region Registrations

        public static List<VehicleRegistrationModel> GetRegistrations(int vehicleID)
        {
            List<VehicleRegistrationModel> result = new List<VehicleRegistrationModel>();

            using (var ctx = new TOfficeEntities())
            {
                var vehicleRegistrations = ctx.VehicleRegistrations
                                                .Include("ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData")
                                                .Include(t => t.VehicleRegistrationInstallments)
                                                .Where(x => x.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData.ID == vehicleID);

                if (vehicleRegistrations != null && vehicleRegistrations.Count() > 0)
                {
                    result =
                        vehicleRegistrations.Select(x =>
                            new VehicleRegistrationModel()
                            {
                                ID = x.ID,
                                RegistrationDate = x.RegistrationDate,
                                TotalAmount = x.TotalAmount,
                                NumberOfInstallments = x.NumberOfInstallments,

                                Installments = 
                                    x.VehicleRegistrationInstallments.Select(vri =>
                                        new InstallmentModel()
                                        {
                                            ID = vri.ID,
                                            InstallmentDate = vri.InstallmentDate,
                                            Amount = vri.Amount,
                                            IsPaid = vri.IsPaid,
                                            PaymentDate = vri.PaymentDate,
                                            IsAdminBan = vri.IsAdminBan,
                                            Note = vri.Note
                                        }
                                    )
                                    .OrderByDescending(order => order.InstallmentDate)
                                    .ToList()
                            }
                        )
                        .OrderByDescending(order => order.RegistrationDate)
                        .ToList();
                }
            }

            return result;
        }

        public static void AddRegistration(int clientID, int vehicleID, VehicleRegistrationModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                var clientRegDocData = ctx.ClientRegistrationDocumentData
                                            .Include(t => t.RegistrationDocumentData.RegistrationVehicleData)
                                            .FirstOrDefault(c => c.ClientID == clientID && c.RegistrationDocumentData.RegistrationVehicleData.ID == vehicleID);

                if (clientRegDocData != null)
                {
                    int firstInstallmentDuePeriod = FIRST_INSTALLMENT_DUE_PERIOD;

                    try
                    {
                        firstInstallmentDuePeriod = Int32.Parse(ConfigurationManager.AppSettings["toffice:FirstInstallmentDuePeriod"]);
                    }
                    catch (Exception) {}

                    // add new registration
                    VehicleRegistrations vehicleReg = new VehicleRegistrations()
                    {
                        ClientRegistrationDocumentDataID = clientRegDocData.ID,
                        RegistrationDate = model.RegistrationDate.HasValue ? (DateTime)model.RegistrationDate : DateTime.Now,
                        TotalAmount = model.TotalAmount,
                        NumberOfInstallments = model.NumberOfInstallments
                    };
                    ctx.VehicleRegistrations.Add(vehicleReg);

                    // add installments
                    int monthAdditionFactor = 1;
                    for (int i = 1; i <= model.NumberOfInstallments; i++)
                    {
                        VehicleRegistrationInstallments installment = new VehicleRegistrationInstallments()
                        {
                            IsPaid = false,
                            IsAdminBan = false,
                            Amount = (decimal)(model.TotalAmount / model.NumberOfInstallments)
                        };

                        if (i == 1)
                        {
                            installment.InstallmentDate = DateTime.Now.AddDays(firstInstallmentDuePeriod);
                        }
                        else
                        {
                            installment.InstallmentDate = DateTime.Now.AddMonths(monthAdditionFactor++);
                        }

                        ctx.VehicleRegistrationInstallments.Add(installment);
                    };

                    ctx.SaveChanges();
                }
            }
        }

        public static List<InstallmentModel> GetRegistrationInstallments(int vehicleRegistrationID)
        {
            using (var ctx = new TOfficeEntities())
            {
                return ctx.VehicleRegistrationInstallments
                            .Where(i => i.VehicleRegistrationID == vehicleRegistrationID)
                            .Select(x =>
                                new InstallmentModel()
                                {
                                    ID = x.ID,
                                    InstallmentDate = x.InstallmentDate,
                                    Amount = x.Amount,
                                    IsAdminBan = x.IsAdminBan,
                                    IsPaid = x.IsPaid,
                                    Note = x.Note,
                                    PaymentDate = x.PaymentDate
                                }
                            )
                            .OrderBy(x => x.InstallmentDate)
                            .ToList();
            }
        }

        #endregion
    }
}
