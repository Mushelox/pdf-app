using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace PdfApp.Shared.Models.PdfBuilderElements.Elements;

public class TextFieldElement : PdfElementBase
{
    public override void Draw(IContainer container)
    {
        container.Column(column =>
        {
            column.Item()
                  .Text("Field title")
                  .SemiBold()
                  .FontSize(16);

            column.Item()
                  .Border(1)
                  .Background(Colors.Grey.Lighten3)
                  .Height(100);
        });
    }
}