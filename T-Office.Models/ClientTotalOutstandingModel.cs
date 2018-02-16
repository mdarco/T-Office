using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class ClientTotalOutstandingModel
    {
        public int? ClientID { get; set; }
        public string Owner { get; set; }
        public string User { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
