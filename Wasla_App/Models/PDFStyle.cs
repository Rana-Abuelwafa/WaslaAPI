using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Wasla_App.Models
{
    public class PDFStyle
    {
        public static IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }
        public static IContainer MiniCellStyle(IContainer container)
        {
            return container.Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }
        public static IContainer HeaderCell(IContainer container, string bgColor)
        {
            return container.Background(bgColor)
                .Padding(5)
                .DefaultTextStyle(t => t.FontColor(Colors.White));
        }
    }
}
