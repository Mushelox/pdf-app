using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilder.Elements;

public abstract class PdfElement
{
    public abstract void Draw(IContainer container);
}