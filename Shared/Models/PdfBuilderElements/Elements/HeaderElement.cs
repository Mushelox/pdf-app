using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilderElements.Elements;

public class HeaderElement : PdfElementBase
{
    public override void Draw(IContainer container)
    {
        container.Column(column =>
        {
            column.Item()
                  .Text(text =>
                  {
                      text.DefaultTextStyle(x => x.SemiBold().FontSize(32));
                      text.Span("This is TEST text");
                      text.AlignCenter();
                  });
            column.Item()
                  .PaddingVertical(5)
                  .LineHorizontal(1);
        });
    }
}