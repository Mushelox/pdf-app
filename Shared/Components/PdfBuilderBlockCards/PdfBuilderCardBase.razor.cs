using Microsoft.AspNetCore.Components;

namespace PdfApp.Shared.Components.PdfBuilderBlockCards;

public abstract partial class PdfBuilderCardBase
{
    protected abstract RenderFragment BuildCardContent();
    
    protected abstract void OnRestoreClick();
    
}