using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using PdfApp.Shared.Models.PdfBuilder.Elements;
using PdfApp.Shared.Services;

namespace PdfApp.Pages;

public partial class PdfBuilder
{
    [Inject] private ILogger<PdfBuilder> Logger { get; init; } = null!;

    [Inject] private PdfService PdfService { get; init; } = null!;

    private const string AREA_NAME_TEMPLATES = "templates";
    private const string AREA_NAME_BUILDER = "builder";

    private List<DropItem> _dropzoneItems = new();
    private MudDropContainer<DropItem> dropContainerRef;

    private void ItemUpdated(MudItemDropInfo<DropItem> dropInfo)
    {
        var droppedItem = dropInfo.Item!;
        droppedItem.Identifier = dropInfo.DropzoneIdentifier;
        droppedItem.Index = dropInfo.IndexInZone;

        _dropzoneItems.UpdateOrder(dropInfo, item => item.Index, 1);

        var sortedDropItems = _dropzoneItems.OrderBy(x => x.Index);
        Logger.LogInformation("Item dropped. Order in dropzone: {data}", sortedDropItems.Select(x => $"\n{x.Index}: {x.Guid}"));
    }

    private void OnTemplateBlockClick(string cardType)
    {
        var newItem = new DropItem
        {
            Name = cardType,
            Identifier = AREA_NAME_BUILDER,
            Index = _dropzoneItems.Count
        };

        _dropzoneItems.Add(newItem);

        StateHasChanged();
        dropContainerRef.Refresh();
        Logger.LogInformation("Item dropped. Order in dropzone: {data}", _dropzoneItems
                                                                         .OrderBy(x => x.Index)
                                                                         .Select(x => $"\n{x.Index}: {x.Guid}"));
    }

#region UTILITY

    /// <param name="startIndex">Default is 0</param>
    /// <param name="ordered">If true sorts collection in ascending index order.</param>
    /// <returns>Collection of items in builder area starting from <paramref name="startIndex"/></returns>
    private IEnumerable<DropItem> ItemsInBuilderArea(int startIndex = 0, bool ordered = false)
    {
        var items = _dropzoneItems.Where(x => x.Identifier == AREA_NAME_BUILDER && x.Index >= startIndex);
        if (ordered)
            items = items.OrderBy(x => x.Index);

        return items;
    }

#endregion

    private async void OnGenerateClick()
    {
        var elements = new List<PdfElement>()
        {
            new HeaderElement()
        };
        
        await PdfService.GenerateAndDisplayPdf(elements);
    }

    public class DropItem
    {
        public int Index { get; set; }

        public string Name { get; init; }

        public string Identifier { get; set; }

        public Guid Guid = Guid.NewGuid();
    }
}