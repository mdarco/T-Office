using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class CostsByPeriodModel
    {
        public int? ClientID { get; set; }
        public string VehicleOwner { get; set; }
        public string VehicleUser { get; set; }
        public string VehicleModel { get; set; }
        public string RegistrationNumber { get; set; }
        public int? VehicleID { get; set; }
        public decimal? TotalCreditAmount { get; set; }
        public decimal? TotalDebtAmount { get; set; }
        public decimal? TotalPaidAmount { get; set; }
    }
}
