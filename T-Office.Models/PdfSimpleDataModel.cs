using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class PdfSimpleDataModel
    {
        public string Title { get; set; }
        public List<string> HeaderData { get; set; }
        public List<List<string>> ColumnData { get; set; }
    }
}
