using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class ClientModel
    {
        public int? ID { get; set; }
        public string JMBG { get; set; }
        public string PIB { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string RecommendedBy { get; set; }
        public string ClientType { get; set; }

        public List<VehicleDataModel> Vehicles { get; set; }
    }
}
