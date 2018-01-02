using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVehicleRegistrationCOM;

namespace T_Office.Models
{
    public class RegLicenseData
    {
        public _DOCUMENT_DATA DocumentData { get; set; }
        public _VEHICLE_DATA VehicleData { get; set; }
        public _PERSONAL_DATA PersonalData { get; set; }
    }
}
