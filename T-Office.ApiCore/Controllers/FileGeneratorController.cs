using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using T_Office.Models;

namespace T_Office.ApiCore.Controllers
{
    [Route("api/file-generator")]
    [ApiController]
    public class FileGeneratorController : ControllerBase
    {
        [Route("generate-file-from-template")]
        [HttpPost]
        public IActionResult GenerateFileFromTemplate(FileTemplateModel model)
        {
            string filePath = BL.FileGenerator.CreateFileDocumentFromTemplate(model.TemplateName, model);

            byte[] byteArray = System.IO.File.ReadAllBytes(filePath);

            string fileName = "potvrda-placanja.docx";
            string fileContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            return File(byteArray, fileContentType, fileName);
        }
    }
}
