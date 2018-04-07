using System;
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
    [RoutePrefix("pdf-helper")]
    public class PDFController : ApiController
    {
        [Route("generate-simple-pdf")]
        [HttpPost]
        public HttpResponseMessage GenerateSimplePdf(PdfSimpleDataModel model)
        {
            MemoryStream stream = BL.PDFGenerator.CreateSimplePDF(model);

            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(stream.GetBuffer());
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "print.pdf";
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return result;
        }
    }
}
