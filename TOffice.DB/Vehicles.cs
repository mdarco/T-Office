using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using T_Office.Models;
using System.Configuration;
using TOffice.DB.DBModel;
using Serilog;
using Serilog.Core;

namespace TOffice.DB
{
    public static class Vehicles
    {
        private static readonly Logger _logger;

        const int FIRST_INSTALLMENT_DUE_PERIOD = 15;

        static Vehicles()
        {
            _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }

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

        public static int Add(TOfficeEntities ctx, int? clientID, VehicleDataModel vehicleDataModel, bool saveChanges)
        {
            if (!clientID.HasValue)
            {
                throw new Exception("Nije definisan klijent za novo vozilo.");
            }

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

            if (saveChanges)
            {
                ctx.SaveChanges();
            }

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

                    _logger.Information("Adding new registration [model]: {@model}", model);
                    _logger.Information("Adding new registration [current date]: {date}", DateTime.Now.Date);

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

        public static void AddRegistration(TOfficeEntities ctx, int? clientRegDocDataID, VehicleRegistrationModel model, bool saveChanges)
        {
            if (!clientRegDocDataID.HasValue)
            {
                throw new Exception("Klijent je obavezan podatak prilikom unosa podataka o registraciji.");
            }

            int firstInstallmentDuePeriod = FIRST_INSTALLMENT_DUE_PERIOD;

            try
            {
                firstInstallmentDuePeriod = Int32.Parse(ConfigurationManager.AppSettings["toffice:FirstInstallmentDuePeriod"]);
            }
            catch (Exception) { }

            // add new registration
            VehicleRegistrations vehicleReg = new VehicleRegistrations()
            {
                ClientRegistrationDocumentDataID = (int)clientRegDocDataID,
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

            if (saveChanges)
            {
                ctx.SaveChanges();
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
                    //Logic with installment amount recalculation


                    //if (model.PaidAmount.HasValue)
                    //{
                        //if (model.PaidAmount == 0)
                        //{
                        //    installment.IsPaid = false;
                        //    installment.PaidAmount = null;
                        //}
                        //else
                        //{
                            /*
                             * Amount assignment logic:
                             * Discard already paid amount and:
                             *     1. if 'paid amount' <= 'installment amount' => assign paid amount to the installment
                             *         - set installment as paid if 'paid amount' == 'installment amount' and set payment date to the current date
                             *     2. if 'paid amount' > 'installment amount'
                             *         =>  assign paid amount to the installment, set installment as paid, set payment date to current date
                             */

                            // var alreadyPaidAmount = installment.PaidAmount.HasValue ? installment.PaidAmount : 0;

                            // var relevantInstallments =
                            //         ctx.VehicleRegistrationInstallments
                            //                 .Where(x => x.VehicleRegistrationID == installment.VehicleRegistrationID && x.IsPaid == false && x.ID != installment.ID)
                            //                 .OrderBy(x => x.InstallmentDate)
                            //                 .ToList();
                            //
                            // var paidInstallments =
                            //         ctx.VehicleRegistrationInstallments
                            //                 .Where(x => x.VehicleRegistrationID == installment.VehicleRegistrationID && x.IsPaid == true)
                            //                 .OrderBy(x => x.InstallmentDate)
                            //                 .ToList();
                            //
                            // var totalPaidAmount = paidInstallments.Sum(x => x.PaidAmount);

                            //var vehicleRegistration = ctx.VehicleRegistrations.FirstOrDefault(x => x.ID == installment.VehicleRegistrationID);
                            //var totalRegAmount = vehicleRegistration.TotalAmount;

                            // (1) and (2)
                            //installment.PaidAmount = model.PaidAmount;
                            //if (model.PaidAmount >= installment.Amount)
                            //{
                            //    installment.IsPaid = true;
                            //    installment.PaymentDate = DateTime.Now.Date;
                            //}
                        //}
                    //}

                    //if (model.IsPaid.HasValue)
                    //{
                    //    installment.IsPaid = (bool)model.IsPaid;

                    //    if (installment.IsPaid && !model.PaymentDate.HasValue)
                    //    {
                    //        installment.PaymentDate = DateTime.Now;
                    //    }

                    //    if (!installment.IsPaid)
                    //    {
                    //        installment.PaymentDate = null;
                    //    }
                    //}

                    //if (model.PaymentDate.HasValue && installment.IsPaid)
                    //{
                    //    installment.PaymentDate = model.PaymentDate;
                    //}

                    //if (model.IsAdminBan.HasValue)
                    //{
                    //    installment.IsAdminBan = (bool)model.IsAdminBan;
                    //}

                    //if (!string.IsNullOrEmpty(model.Note))
                    //{
                    //    installment.Note = model.Note;
                    //}

                    ctx.SaveChanges();
                }
            }
        }

        public static void ResetInstallments(int vehicleRegistrationID)
        {
            using (var ctx = new TOfficeEntities())
            {
                var installments = ctx.VehicleRegistrationInstallments.Where(x => x.VehicleRegistrationID == vehicleRegistrationID).ToList();

                var registration = ctx.VehicleRegistrations.FirstOrDefault(x => x.ID == vehicleRegistrationID);
                decimal installmentAmount = registration.TotalAmount / installments.Count();

                if (installments != null && installments.Count() > 0)
                {
                    foreach (var installment in installments)
                    {
                        installment.Amount = installmentAmount;
                        installment.PaidAmount = null;
                        installment.IsPaid = false;
                        installment.PaymentDate = null;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        public static decimal GetTotalPaidAmountByPeriod(DateTime? dateFrom, DateTime? dateTo)
        {
            using (var ctx = new TOfficeEntities())
            {
                var installments = ctx.VehicleRegistrationInstallments
                                        .Where(x =>
                                            x.IsPaid || (!x.IsPaid && x.PaidAmount.HasValue && x.PaidAmount < x.Amount)
                                        ).ToList();

                return (decimal)installments.Sum(x => x.PaidAmount);
            }
        }

        #endregion
    }
}
