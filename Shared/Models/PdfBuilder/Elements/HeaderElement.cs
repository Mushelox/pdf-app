using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilder.Elements;

public class HeaderElement : PdfElement
{
    public override void Draw(IContainer container)
    {
        container.Text("This is TEST text");
    }
}