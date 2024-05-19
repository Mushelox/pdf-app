using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilderElements.Elements;

public class HeaderElement : PdfElementBase
{
    public override void Draw(IContainer container)
    {
        container.Text("This is TEST text");
    }
}