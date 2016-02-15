using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfMerger
{
    public class PdfPageEvents : IPdfPageEvent
    {
        private BaseFont _baseFont;
        private PdfContentByte _content;
        
        public void OnOpenDocument(PdfWriter writer, Document document)
        {
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _content = writer.DirectContent;
        }

        public void OnStartPage(PdfWriter writer, Document document)
        {
            
        }

        public void OnEndPage(PdfWriter writer, Document document)
        {
            // Write footer text (page numbers)
            string text = "Page " + writer.PageNumber;
            _content.BeginText();
            _content.SetFontAndSize(_baseFont, 8);
            _content.SetTextMatrix(GetCenterTextPosition(text, writer), 10);
            _content.ShowText(text);
            _content.EndText();
        }

        public void OnCloseDocument(PdfWriter writer, Document document)
        {
            
        }

        public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
            
        }

        public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            
        }

        public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
        {
            
        }

        public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            
        }

        public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
        {
            
        }

        public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            
        }

        public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
        {
            
        }
        
        private float GetCenterTextPosition(string text, PdfWriter writer)
        {
            return writer.PageSize.Width / 2 - _baseFont.GetWidthPoint(text, 8) / 2;
        }
    }
}