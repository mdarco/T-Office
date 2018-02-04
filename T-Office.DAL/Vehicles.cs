using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Office.DAL.DBModel;
using T_Office.Models;

namespace T_Office.DAL
{
    public static class Vehicles
    {
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
    }
}
