using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilderElements.Elements;

public abstract class PdfElementBase
{
    public abstract void Draw(IContainer container);
}