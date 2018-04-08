using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using T_Office.Models;

namespace T_Office.Test
{
    [TestClass]
    public class FileGenerator
    {
        [TestMethod]
        public void CreatePaidInstallmentReceipt()
        {
            string templateName = "potvrda-placanja.docx";

            var model = new FileTemplateModel()
            {
                Owner = "Unicredit Banka AD",
                User = "Panalex AD, Omladinskih brigada BB, Beograd",
                Vehicle = "Nissan Qashqai DCI Accenta+SAFETY [BG854-IM]",
                InstallmentAmount = 7500,
                PaymentDate = DateTime.Now.Date
            };

            string result = BL.FileGenerator.CreateFileDocumentFromTemplate(templateName, model);
        }
    }
}
