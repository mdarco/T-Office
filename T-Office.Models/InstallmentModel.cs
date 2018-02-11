using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class InstallmentModel
    {
        public int? ID { get; set; }
        public int? VehicleRegistrationID { get; set; }
        public DateTime InstallmentDate { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsAdminBan { get; set; }
        public string Note { get; set; }
    }
}
