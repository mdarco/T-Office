using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_Office.Models;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/pdf-helper")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        [Route("generate-simple-pdf")]
        [HttpPost]
        public IActionResult GenerateSimplePdf(PdfSimpleDataModel model)
        {
            MemoryStream stream = BL.PDFGenerator.CreateSimplePDF(model);
            byte[] byteArray = stream.ToArray();

            string fileName = "print.pdf";
            string fileContentType = "application/pdf";

            return File(byteArray, fileContentType, fileName);
        }
    }
}
