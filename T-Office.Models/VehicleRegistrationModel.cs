using System;
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
        public decimal? TotalAmount { get; set; }
        public int? NumberOfInstallments { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? NextRegistrationDate { get; set; }

        public decimal? TotalOutstandingAmount
        {
            get
            {
                decimal sum = 0;

                if (this.Installments != null && this.Installments.Count() > 0)
                {
                    foreach (var installment in this.Installments)
                    {
                        sum += (decimal)installment.Amount;
                        if (installment.PaidAmount.HasValue)
                        {
                            sum -= (decimal)installment.PaidAmount;
                        }
                    }
                }

                return sum;
            }
        }

        public List<InstallmentModel> Installments { get; set; }
    }
}
