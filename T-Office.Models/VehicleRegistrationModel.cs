﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.Models
{
    public class VehicleRegistrationModel
    {
        public int? ID { get; set; }
        public int? ClientRegistrationDocumentDataID { get; set; }
        public decimal TotalAmount { get; set; }
        public int? NumberOfInstallments { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public List<InstallmentModel> Installments { get; set; }
    }
}