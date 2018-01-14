using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class ApiTableResponseModel<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
    }
}
