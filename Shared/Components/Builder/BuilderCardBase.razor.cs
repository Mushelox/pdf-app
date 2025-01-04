using Microsoft.AspNetCore.Components;
using PdfApp.Shared.Enums;
using PdfApp.Shared.Models;
using PdfApp.Shared.Models.PdfBuilderElements;

namespace PdfApp.Shared.Components.Builder;

/// <summary>
/// All PDF builder blocks are represented by a <see cref="MudBlazor.MudCard"/>
/// </summary>
public abstract partial class BuilderCardBase
{
    [Parameter] [EditorRequired] public DropItem ItemData { get; set; } = null!;

    [Parameter] [EditorRequired] public PdfBuilderCardCallbacks Callbacks { get; set; } = null!;
    
    /// <summary>
    /// Override this method to insert content into the card
    /// </summary>
    /// <returns>RenderFragment which will be inserted into the card's MudCardContent tag</returns>
    protected abstract RenderFragment BuildCardContent();
    
    protected abstract void OnRestoreClick();

    private bool DuplicateEnabled => ItemData.Type is not ElementType.Header and not ElementType.Footer;
}