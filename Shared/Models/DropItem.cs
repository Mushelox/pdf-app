using PdfApp.Shared.Enums;

namespace PdfApp.Shared.Models;

public class DropItem
{
    public Guid Guid { get; init; }
    
    public int Index { get; set; }

    public string Name { get; init; }

    public string DropZoneIdentifier { get; set; }

    public ElementType Type { get; init; }

    public DropItem(int index, string name, ElementType type, string dropZoneIdentifier)
    {
        Guid = Guid.NewGuid();
        Index = index;
        Name = name;
        Type = type;
        DropZoneIdentifier = dropZoneIdentifier;
    }
}