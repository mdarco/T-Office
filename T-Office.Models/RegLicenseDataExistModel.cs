using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class RegLicenseDataExistModel
    {
        public bool IsExistingOwner { get; set; }
        public bool IsExistingUser { get; set; }
        public bool IsExistingVehicle { get; set; }
    }
}
