//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T_Office.DAL.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegistrationVehicleData
    {
        public int ID { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<System.DateTime> FirstRegistrationDate { get; set; }
        public string ProductionYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string EnginePowerKW { get; set; }
        public string EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public string PowerWeightRatio { get; set; }
        public string Mass { get; set; }
        public string MaxPermissibleLadenMass { get; set; }
        public string TypeApprovalNumber { get; set; }
        public string NumberOfSeats { get; set; }
        public string NumberOfStandingPlaces { get; set; }
        public string EngineIDNumber { get; set; }
        public string VehicleIDNumber { get; set; }
        public string NumberOfAxles { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string RestrictionToChangeOwner { get; set; }
        public string Load { get; set; }
        public int RegistrationDocumentDataID { get; set; }
    
        public virtual RegistrationDocumentData RegistrationDocumentData { get; set; }
    }
}
