using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.BL
{
    public static class PDFGenerator
    {
        public static MemoryStream CreatePDF<T>(IEnumerable<T> tableData, List<string> headerData) where T : class
        {
            MemoryStream stream = new MemoryStream();

            if (tableData.Count() > 0)
            {
                var T_Properties = typeof(T).GetProperties();
                int columnsCount = T_Properties.Count();

                Document pdfDoc = new Document(PageSize.A4);

                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                pdfWriter.PageEvent = new ITextSharpEvents();

                pdfDoc.Open();

                // logo
                string imgPath = HttpContext.Current.Server.MapPath("~\\img") + "\\logo.png";
                Image imgLogo = Image.GetInstance(imgPath);
                imgLogo.ScalePercent(50);
                pdfDoc.Add(imgLogo);

                // data table
                PdfPTable table = new PdfPTable(columnsCount);
                table.WidthPercentage = 100;
                table.SpacingBefore = 30f;

                PdfPCell cellHeader = new PdfPCell(new Phrase(headerData[0], new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)));
                cellHeader.Colspan = columnsCount;
                cellHeader.HorizontalAlignment = 1; // 0 = left, 1 = center, 2 = right
                table.AddCell(cellHeader);

                // table column header row
                for (int i = 1; i <= columnsCount; i++)
                {
                    PdfPCell cellColHeader = new PdfPCell(new Phrase(headerData[i], new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)));
                    table.AddCell(cellColHeader);
                }

                // table data rows
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
                Font tableDataFont = new Font(bf, 8, Font.NORMAL);

                foreach (T tableRow in tableData)
                {
                    foreach (var prop in T_Properties)
                    {
                        PdfPCell cellData = new PdfPCell(new Phrase(prop.GetValue(tableRow).ToString(), tableDataFont));
                        table.AddCell(cellData);
                    }
                }

                pdfDoc.Add(table);

                pdfDoc.Close();
            }

            return stream;
        }
    }
}
