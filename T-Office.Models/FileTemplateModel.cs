using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class FileTemplateModel
    {
        public string TemplateName { get; set; }

        public string Owner { get; set; }
        public string User { get; set; }
        public string Vehicle { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
