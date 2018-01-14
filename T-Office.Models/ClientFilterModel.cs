using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class ClientFilterModel
    {
        public string JMBG { get; set; }
        public string PIB { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string VehicleRegNo { get; set; }

        // paging and sorting support
        public int? PageNo { get; set; }
        public int? RecordsPerPage { get; set; }
        public string OrderByClause { get; set; }
    }
}
