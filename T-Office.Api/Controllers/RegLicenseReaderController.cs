using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
            string path = ConfigurationManager.AppSettings["toffice:RegLicenseReaderPath"];
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Nije zadata putanja do programa za čitanje podataka sa saobraćajne dozvole.");
            }

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            string result = string.Empty;

            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                result += line;
            }

            return result;
        }
    }
}
