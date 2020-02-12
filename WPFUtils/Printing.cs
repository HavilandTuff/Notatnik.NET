using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPFUtils
{
    public static class Printing
    {
        private static FlowDocument CreateFlowDocument(string[] lines, Font font, double pageWidth)
        {
            FlowDocument fd = new FlowDocument();
            //set colors to black and white
            fd.Background = Brushes.White;
            fd.Foreground = Brushes.Black;

            //set font
            fd.FontFamily = font.Family;
            fd.FontStyle = font.Style;
            fd.FontWeight = font.Weight;
            fd.FontSize = font.Size;

            //set single column
            fd.ColumnGap = 0;
            fd.ColumnWidth = pageWidth;

            foreach(string line in lines)
            {
                Paragraph paragraph = new Paragraph(new Run(line));
                paragraph.Margin = new Thickness(0); //No paragraph separation.
                fd.Blocks.Add(paragraph);
            }
            return fd;
        }

        public static void PrintText(string[] lines, Font font)
        {
            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {
                FlowDocument fd = CreateFlowDocument(
                    lines, font, printDialog.PrintableAreaWidth);
                printDialog.PrintDocument(
                    (fd as IDocumentPaginatorSource).DocumentPaginator, fd.Name);
            }
        }
        public static void PrintText(string text, Font font)
        {
            string[] lines = text.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', ' ');
            }
            PrintText(lines, font);
        }
    }
}
