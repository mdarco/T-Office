using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class PersonalDataModel
    {
        public int? ID { get; set; }
        public string OwnerPersonalNo { get; set; }
        public string OwnerPIB { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurnameOrBusinessName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string UserPersonalNo { get; set; }
        public string UserPIB { get; set; }
        public string UserName { get; set; }
        public string UserSurnameOrBusinessName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string RecommendedBy { get; set; }
    }
}
