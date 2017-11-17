using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class RegistrationDataModel
    {
        public DocumentDataModel DocumentData { get; set; }
        public VehicleDataModel VehicleData { get; set; }
        public PersonalDataModel PersonalData { get; set; }
    }
}
