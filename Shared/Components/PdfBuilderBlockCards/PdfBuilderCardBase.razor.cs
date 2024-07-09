using Microsoft.AspNetCore.Components;
using PdfApp.Shared.Enums;
using PdfApp.Shared.Models;
using PdfApp.Shared.Models.PdfBuilderElements;

namespace PdfApp.Shared.Components.PdfBuilderBlockCards;

public abstract partial class PdfBuilderCardBase
{
    [Parameter] [EditorRequired] public DropItem ItemData { get; set; } = null!;

    [Parameter] [EditorRequired] public PdfBuilderCardCallbacks Callbacks { get; set; } = null!;
    
    
    protected abstract RenderFragment BuildCardContent();
    
    protected abstract void OnRestoreClick();

    private bool DuplicateEnabled => ItemData.Type is not ElementType.Header and not ElementType.Footer;
}