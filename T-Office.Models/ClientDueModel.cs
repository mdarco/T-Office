using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class ClientDueModel
    {
        public int? ClientID { get; set; }
        public string FullOwnerName { get; set; }
        public string FullUserName { get; set; }
        public string FullVehicleName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? NextRegistrationDate { get; set; }
        public DateTime? InstallmentDate { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public decimal? InstallmentPaidAmount { get; set; }
    }
}
