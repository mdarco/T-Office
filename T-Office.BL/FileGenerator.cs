using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.BL
{
    public static class FileGenerator
    {
        //string path = ConfigurationManager.AppSettings["toffice:RegLicenseReaderPath"];

        public static string TEMPLATE_FOLDER = ConfigurationManager.AppSettings["toffice:TemplateFolder"];
        public static string TEMP_FOLDER = TEMPLATE_FOLDER + @"\temp";
        public static string TEMPLATE_FOLDER_VIRTUAL_ROOT = ConfigurationManager.AppSettings["toffice:TemplateFolderVirtualRoot"]; // should not start with '/'
        public static string TEMP_FOLDER_VIRTUAL_ROOT = TEMPLATE_FOLDER_VIRTUAL_ROOT + @"/temp"; // should not start with '/'

        public static void ReplaceTagsInFileTemplate(ref Dictionary<string, string> keyValues, FileTemplateModel model)
        {
            keyValues.Add("<<VLASNIK>>", model.Owner);
            keyValues.Add("<<KORISNIK>>", model.User);
            keyValues.Add("<<VOZILO>>", model.Vehicle);
            keyValues.Add("<<IZNOS_RATE>>", model.InstallmentAmount.HasValue ? model.InstallmentAmount.ToString() : string.Empty);
            keyValues.Add("<<DATUM_PLACANJA>>", model.PaymentDate.HasValue ? ((DateTime)model.PaymentDate).ToString("dd.MM.yyyy") : string.Empty);
        }

        public static MemoryStream CreateFileDocumentFromTemplate(string templateName, FileTemplateModel model)
        {
            string templatePath = TEMPLATE_FOLDER + @"\" + templateName;

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            ReplaceTagsInFileTemplate(ref keyValues, model);

            byte[] templateBytes = File.ReadAllBytes(templatePath);

            // create template copy in temp folder
            string fileName = string.Format("{0}-{1}.docx", Path.GetFileNameWithoutExtension(templatePath), DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));
            string filePath = string.Format(@"{0}\{1}", TEMP_FOLDER, fileName);
            File.WriteAllBytes(filePath, templateBytes);

            string documentText = string.Empty;
            byte[] byteArray = File.ReadAllBytes(filePath);

            MemoryStream stream = new MemoryStream();
            stream.Write(byteArray, 0, byteArray.Length);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                foreach (KeyValuePair<string, string> dataItem in keyValues)
                {
                    OpenXmlHelper.SearchAndReplace(wordDoc, dataItem.Key, dataItem.Value, true);
                }

                using (StreamReader reader = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    documentText = reader.ReadToEnd();
                }

                using (StreamWriter writer = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    writer.Write(documentText);
                }
            }

            return stream;

            //return string.Format("{0}/{1}", TEMP_FOLDER_VIRTUAL_ROOT, fileName);
        }
    }
}
