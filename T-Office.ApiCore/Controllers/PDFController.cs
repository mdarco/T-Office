using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_Office.Models;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/pdf-helper")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        private IWebHostEnvironment _env;

        public PDFController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Route("generate-simple-pdf")]
        [HttpPost]
        public IActionResult GenerateSimplePdf(PdfSimpleDataModel model)
        {
            string contentRootPath = _env.ContentRootPath;

            MemoryStream stream = BL.PDFGenerator.CreateSimplePDF(model, contentRootPath);
            byte[] byteArray = stream.ToArray();

            string fileName = "print.pdf";
            string fileContentType = "application/pdf";

            return File(byteArray, fileContentType, fileName);
        }
    }
}
