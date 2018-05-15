﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using T_Office.Models;

namespace T_Office.Api.Controllers
{
    [RoutePrefix("file-generator")]
    public class FileGeneratorController : ApiController
    {
        [Route("generate-file-from-template")]
        [HttpPost]
        public HttpResponseMessage GenerateFileFromTemplate(FileTemplateModel model)
        {
            string filePath = BL.FileGenerator.CreateFileDocumentFromTemplate(model.TemplateName, model);

            MemoryStream stream = new MemoryStream();
            byte[] byteArray = File.ReadAllBytes(filePath);
            stream.Write(byteArray, 0, byteArray.Length);

            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(stream.GetBuffer());
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "potvrda-placanja.docx";
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            return result;
        }
    }
}