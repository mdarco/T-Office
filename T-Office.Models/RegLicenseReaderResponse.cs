using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class RegLicenseReaderResponse
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public RegLicenseData Result { get; set; }
    }
}
