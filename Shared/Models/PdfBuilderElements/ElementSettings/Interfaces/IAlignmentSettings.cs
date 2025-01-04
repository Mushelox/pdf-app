using QuestPDF.Infrastructure;

namespace PdfApp.Shared.Models.PdfBuilderElements.ElementSettings.Interfaces;

public interface IAlignmentSettings
{
    public HorizontalAlignment HorizontalAlignment { get; set; }
    
    public VerticalAlignment VerticalAlignment { get; set; }
}