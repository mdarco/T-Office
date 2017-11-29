using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class VehicleDataModel
    {
        public int? ID { get; set; }
        public int? RegistrationDocumentDataID { get; set; }
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
    }
}
