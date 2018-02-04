using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Office.DAL.DBModel;
using T_Office.Models;

namespace T_Office.DAL
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
    }
}
