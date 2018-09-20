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
        const int FIRST_INSTALLMENT_DUE_PERIOD = 15;

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
                                                .Include(t => t.ClientRegistrationDocumentData.RegistrationDocumentData.RegistrationVehicleData)
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
                                NextRegistrationDate = x.NextRegistrationDate,
                                TotalAmount = x.TotalAmount,
                                NumberOfInstallments = x.NumberOfInstallments,

                                Installments = 
                                    x.VehicleRegistrationInstallments.Select(vri =>
                                        new InstallmentModel()
                                        {
                                            ID = vri.ID,
                                            InstallmentDate = vri.InstallmentDate,
                                            Amount = vri.Amount,
                                            PaidAmount = vri.PaidAmount,
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
                        RegistrationDate = model.RegistrationDate.HasValue ? (DateTime)model.RegistrationDate : DateTime.Now.Date,
                        TotalAmount = (decimal)model.TotalAmount,
                        NumberOfInstallments = model.NumberOfInstallments,
                        NextRegistrationDate = model.RegistrationDate.HasValue ? ((DateTime)model.RegistrationDate).AddYears(1) : DateTime.Now.Date.AddYears(1)
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

        public static void EditRegistration(int vehicleRegistrationID, VehicleRegistrationModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                var vr = ctx.VehicleRegistrations.FirstOrDefault(x => x.ID == vehicleRegistrationID);
                if (vr != null)
                {
                    if (model.RegistrationDate.HasValue)
                    {
                        vr.RegistrationDate = ((DateTime)model.RegistrationDate).Date;
                    }

                    if (model.NextRegistrationDate.HasValue)
                    {
                        vr.NextRegistrationDate = ((DateTime)model.NextRegistrationDate).Date;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        public static void DeleteRegistration(int vehicleRegistrationID)
        {
            using (var ctx = new TOfficeEntities())
            {
                var reg = ctx.VehicleRegistrations
                                    .Include(t => t.VehicleRegistrationInstallments)
                                    .FirstOrDefault(x => x.ID == vehicleRegistrationID);

                if (reg != null)
                {
                    // delete installments
                    for (int i = reg.VehicleRegistrationInstallments.ToList().Count() - 1; i >= 0; i--)
                    {
                        ctx.VehicleRegistrationInstallments.Remove(reg.VehicleRegistrationInstallments.ElementAt(i));
                    }

                    // delete registration
                    ctx.VehicleRegistrations.Remove(reg);

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
                                    PaidAmount = x.PaidAmount,
                                    IsAdminBan = x.IsAdminBan,
                                    IsPaid = x.IsPaid,
                                    Note = x.Note,
                                    PaymentDate = x.PaymentDate
                                }
                            )
                            .OrderByDescending(x => x.InstallmentDate)
                            .ToList();
            }
        }

        public static void EditInstallment(int installmentID, InstallmentModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                var installment = ctx.VehicleRegistrationInstallments.FirstOrDefault(x => x.ID == installmentID);
                if (installment != null)
                {
                    if (model.PaidAmount.HasValue)
                    {
                        var relevantInstallments =
                                ctx.VehicleRegistrationInstallments
                                        .Where(x => x.VehicleRegistrationID == installment.VehicleRegistrationID && x.InstallmentDate >= installment.InstallmentDate)
                                        .OrderBy(x => x.InstallmentDate)
                                        .ToList();

                        decimal remainingPaidAmount = (decimal)model.PaidAmount;
                        foreach (var relevantInstallment in relevantInstallments)
                        {
                            decimal installmentAmountDue = relevantInstallment.Amount - ((relevantInstallment.PaidAmount.HasValue) ? (decimal)relevantInstallment.PaidAmount : 0);

                            decimal installmentAmountToBePaid = installmentAmountDue;
                            if (remainingPaidAmount < installmentAmountDue)
                            {
                                installmentAmountToBePaid = remainingPaidAmount;
                            }

                            if (remainingPaidAmount > 0)
                            {
                                remainingPaidAmount = remainingPaidAmount - installmentAmountToBePaid;

                                relevantInstallment.PaidAmount = installmentAmountToBePaid + (relevantInstallment.PaidAmount.HasValue ? relevantInstallment.PaidAmount : 0);
                                relevantInstallment.IsPaid = (relevantInstallment.PaidAmount == relevantInstallment.Amount);

                                if (relevantInstallment.IsPaid)
                                {
                                    relevantInstallment.PaymentDate = DateTime.Now.Date;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (model.PaidAmount == 0)
                        {
                            installment.IsPaid = false;
                        }
                    }

                    if (model.IsPaid.HasValue)
                    {
                        installment.IsPaid = (bool)model.IsPaid;

                        if (installment.IsPaid && !model.PaymentDate.HasValue)
                        {
                            installment.PaymentDate = DateTime.Now;
                        }

                        if (!installment.IsPaid)
                        {
                            installment.PaymentDate = null;
                        }
                    }

                    if (model.PaymentDate.HasValue && installment.IsPaid)
                    {
                        installment.PaymentDate = model.PaymentDate;
                    }

                    if (model.IsAdminBan.HasValue)
                    {
                        installment.IsAdminBan = (bool)model.IsAdminBan;
                    }

                    if (!string.IsNullOrEmpty(model.Note))
                    {
                        installment.Note = model.Note;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        #endregion
    }
}
