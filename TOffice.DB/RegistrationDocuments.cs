using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Office.Models;
using TOffice.DB.DBModel;

namespace TOffice.DB
{
    public static class RegistrationDocuments
    {
        public static int AddFromFullModel(TOfficeEntities ctx, RegistrationDataModel model)
        {
            var documentDataModel = model.DocumentData;

            RegistrationDocumentData regDocData = new RegistrationDocumentData()
            {
                CompetentAuthority = documentDataModel.CompetentAuthority,
                ExpiryDate = documentDataModel.ExpiryDate,
                IssuingAuthority = documentDataModel.IssuingAuthority,
                IssuingDate = documentDataModel.IssuingDate,
                IssuingState = documentDataModel.IssuingState,
                SerialNumber = documentDataModel.SerialNumber,
                UnambiguousNumber = documentDataModel.UnambiguousNumber
            };

            int registrationVehicleDataID = Vehicles.AddFromFullModel(ctx, model);

            regDocData.RegistrationVehicleDataID = registrationVehicleDataID;

            ctx.RegistrationDocumentData.Add(regDocData);

            return regDocData.ID;
        }

        public static void Add(int clientID, RegistrationDataModel model)
        {
            using (var ctx = new TOfficeEntities())
            {
                var documentDataModel = model.DocumentData;
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

                RegistrationDocumentData regDocData = new RegistrationDocumentData()
                {
                    CompetentAuthority = documentDataModel.CompetentAuthority,
                    ExpiryDate = documentDataModel.ExpiryDate,
                    IssuingAuthority = documentDataModel.IssuingAuthority,
                    IssuingDate = documentDataModel.IssuingDate,
                    IssuingState = documentDataModel.IssuingState,
                    SerialNumber = documentDataModel.SerialNumber,
                    UnambiguousNumber = documentDataModel.UnambiguousNumber,
                    RegistrationVehicleDataID = regVehicleData.ID
                };

                ctx.RegistrationDocumentData.Add(regDocData);

                ClientRegistrationDocumentData clientRegDocData = new ClientRegistrationDocumentData()
                {
                    ClientID = clientID,
                    RegistrationDocumentDataID = regDocData.ID
                };

                ctx.ClientRegistrationDocumentData.Add(clientRegDocData);

                ctx.SaveChanges();
            }
        }

        public static int Add(TOfficeEntities ctx, int? clientID, int? vehicleID, DocumentDataModel documentDataModel, bool saveChanges)
        {
            if (!clientID.HasValue)
            {
                throw new Exception("Klijent je obavezan podatak prilikom unosa podataka na saobraćajnoj dozvoli.");
            }

            if (!vehicleID.HasValue)
            {
                throw new Exception("Vozilo je obavezan podatak prilikom unosa podataka na saobraćajnoj dozvoli.");
            }

            RegistrationDocumentData regDocData = new RegistrationDocumentData()
            {
                CompetentAuthority = documentDataModel.CompetentAuthority,
                ExpiryDate = documentDataModel.ExpiryDate,
                IssuingAuthority = documentDataModel.IssuingAuthority,
                IssuingDate = documentDataModel.IssuingDate,
                IssuingState = documentDataModel.IssuingState,
                SerialNumber = documentDataModel.SerialNumber,
                UnambiguousNumber = documentDataModel.UnambiguousNumber,
                RegistrationVehicleDataID = (int)vehicleID
            };

            ctx.RegistrationDocumentData.Add(regDocData);

            if (saveChanges)
            {
                ctx.SaveChanges();
            }

            return regDocData.ID;
        }

        public static int AddClientRegDocData(TOfficeEntities ctx, int? clientID, int? regDocDataID, bool saveChanges)
        {
            ClientRegistrationDocumentData clientRegDocData = new ClientRegistrationDocumentData()
            {
                ClientID = (int)clientID,
                RegistrationDocumentDataID = (int)regDocDataID
            };

            ctx.ClientRegistrationDocumentData.Add(clientRegDocData);

            if (saveChanges)
            {
                ctx.SaveChanges();
            }

            return clientRegDocData.ID;
        }
    }
}
