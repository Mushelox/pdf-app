using PdfApp.Shared.Enums;
using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilderElements.ElementSettings.Interfaces;

public interface IFontSettings
{
    public int Size { get; set; }

    public FontWeight Weight { get; set; }

    public TextAlignment Alignment { get; set; }

    public bool Italic { get; set; }

    public bool Strikethrough { get; set; }

    public bool Underline { get; set; }
}