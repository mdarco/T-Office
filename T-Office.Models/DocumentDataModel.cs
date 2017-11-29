using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class DocumentDataModel
    {
        public int? ID { get; set; }
        public string IssuingState { get; set; }
        public string CompetentAuthority { get; set; }
        public string IssuingAuthority { get; set; }
        public string UnambiguousNumber { get; set; }
        public Nullable<System.DateTime> IssuingDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string SerialNumber { get; set; }
    }
}
