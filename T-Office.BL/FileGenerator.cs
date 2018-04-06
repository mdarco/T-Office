using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.BL
{
    public static class FileGenerator
    {
        public const string TEMPLATE_FOLDER = "";
        public static string TEMP_FOLDER = TEMPLATE_FOLDER + @"\Temp";
        public const string TEMPLATE_FOLDER_VIRTUAL_ROOT = ""; // should not start with '/'
        public static string TEMP_FOLDER_VIRTUAL_ROOT = TEMPLATE_FOLDER_VIRTUAL_ROOT + @"/Temp"; // should not start with '/'

        public static void ReplaceTagsInFileTemplate(ref Dictionary<string, string> keyValues, FileTemplateModel model)
        {
            //keyValues.Add("<<BROJ_PREDMETA>>", model.ReferenceNumber);
            //keyValues.Add("<<VRSTA_PREDMETA>>", model.FileColor);
            //keyValues.Add("<<PODNOSILAC_ZAHTEVA>>", model.CurrentClient);
            //keyValues.Add("<<TIP_PREDMETA>>", model.Workflow);
            //keyValues.Add("<<DATUM_OTVARANJA>>", model.CreationDate.HasValue ? ((DateTime)model.CreationDate).ToString("dd.MM.yyyy") : string.Empty);
            //keyValues.Add("<<ORG_JEDINICA>>", model.CurrentOrgUnit);
            //keyValues.Add("<<OBRADJIVAC>>", model.CurrentProcessingUserName);
            //keyValues.Add("<<ROK_RESAVANJA_PO_ZUP>>", model.DeadlineByZUP);
            //keyValues.Add("<<STATUS_PREDMETA>>", (!model.IsActive.HasValue || !(bool)model.IsActive) ? "Pasivan" : "Aktivan");
            //keyValues.Add("<<DATUM_RESAVANJA>>", model.SolutionDate.HasValue ? ((DateTime)model.SolutionDate).ToString("dd.MM.yyyy") : string.Empty);
            //keyValues.Add("<<ROK_CUVANJA>>", model.PreservationInterval);
            //keyValues.Add("<<UPUTSTVO_PISARNICI>>", model.RegistryOfficeNote);
            //keyValues.Add("<<OSTALE_NAPOMENE>>", model.OtherNotes);
            //keyValues.Add("<<PREDMET_KREIRAO>>", model.CreatedByUserFullName);
        }

        public static string CreateFileDocumentFromTemplate(string templatePath, FileTemplateModel model)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            ReplaceTagsInFileTemplate(ref keyValues, model);

            byte[] templateBytes = File.ReadAllBytes(templatePath);

            // create template copy in temp folder
            string fileName = string.Format("{0}-{1}.docx", Path.GetFileNameWithoutExtension(templatePath), DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));
            string filePath = string.Format(@"{0}\{1}", TEMP_FOLDER, fileName);
            File.WriteAllBytes(filePath, templateBytes);

            string documentText = string.Empty;
            byte[] byteArray = File.ReadAllBytes(filePath);

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(templatePath, true))
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
            }

            return string.Format("{0}/{1}", TEMP_FOLDER_VIRTUAL_ROOT, fileName);
        }
    }
}
