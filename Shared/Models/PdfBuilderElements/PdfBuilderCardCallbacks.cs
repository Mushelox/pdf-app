namespace PdfApp.Shared.Models.PdfBuilderElements;

public record PdfBuilderCardCallbacks(Action<DropItem> Duplicate, Action<DropItem> Delete);