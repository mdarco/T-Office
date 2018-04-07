using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Office.BL
{
    public class ITextSharpEvents : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            PdfPTable t = new PdfPTable(1);
            t.DefaultCell.Border = Rectangle.NO_BORDER;
            t.TotalWidth = document.Right - document.Left;

            PdfPCell cell = new PdfPCell(new Phrase(string.Format("Strana {0}", writer.PageNumber), new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            t.AddCell(cell);

            t.WriteSelectedRows(0, -1, document.Left, document.Bottom, writer.DirectContent);
        }
    }
}
