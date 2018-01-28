using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace T_Office.Api.Controllers
{
    [RoutePrefix("reg-license-reader")]
    public class RegLicenseReaderController : ApiController
    {
        [Route("read")]
        [HttpGet]
        public string ReadLicenseData()
        {
            string result = string.Empty;

            string path = ConfigurationManager.AppSettings["toffice:RegLicenseReaderPath"];
            if (string.IsNullOrEmpty(path))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ReasonPhrase = "Nije zadata putanja do programa za citanje podataka sa saobracajne dozvole."
                    });

                // TODO: log error
            }

            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.GetDirectoryName(path)
                    }
                };

                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    result += line;
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ReasonPhrase = string.Format("{0} | {1}", "Doslo je do greske prilikom citanja podataka sa saobracajne dozvole", "[GRESKA] --> " + ex.Message)
                    });

                // TODO: log error
            }

            return result;
        }
    }
}
