using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using PdfApp.Shared.Enums;
using PdfApp.Shared.Extensions;
using PdfApp.Shared.Models;
using PdfApp.Shared.Models.PdfBuilderElements;
using PdfApp.Shared.Models.PdfBuilderElements.Elements;
using PdfApp.Shared.Services;

namespace PdfApp.Pages;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class PdfBuilder
{
    [Inject] private ILogger<PdfBuilder> Logger { get; init; } = null!;

    [Inject] private PdfService PdfService { get; init; } = null!;

    [Inject] private IDialogService DialogService { get; init; } = null!;

    private const string AREA_NAME_BUILDER = "builder";

    private readonly List<DropItem> _dropzoneItems = new();
    private MudDropContainer<DropItem> _dropContainerRef = null!;

    private PdfBuilderCardCallbacks _builderItemsCallbacks;


    protected override void OnInitialized()
    {
        _builderItemsCallbacks = new PdfBuilderCardCallbacks(OnDuplicateBuilderItemClick, OnDeleteBuilderItemClick);
    }


    private async Task ItemUpdated(MudItemDropInfo<DropItem> dropInfo)
    {
        var droppedItem = dropInfo.Item!;
        int dropIndex = dropInfo.IndexInZone;

        if (ValidDropIndex(dropIndex))
        {
            droppedItem.DropZoneIdentifier = dropInfo.DropzoneIdentifier;
            droppedItem.Index = dropInfo.IndexInZone;
            _dropzoneItems.UpdateOrder(dropInfo, item => item.Index, 1);
        }
        else
        {
            await DialogService.ShowInfoDialog("Invalid placement", "Header and footer elements must always occupy the first or last position respectively.");
            RefreshDropContainer();
        }
    }

    private async Task OnTemplateBlockClick(BlockElementData data)
    {
        if (!CanAddToDropzone(data.Type))
        {
            await DialogService.ShowInfoDialog("Duplicate element", $"There can only be one {(data.Type == ElementType.Header ? "header" : "footer")} element.");
            return;
        }

        var newItem = new DropItem(_dropzoneItems.Count, data.Title, data.Type, AREA_NAME_BUILDER);

        AddItemToDropzone(newItem);
        RefreshDropContainer();

        Logger.LogInformation("Item dropped. Order in dropzone: {data}", _dropzoneItems
                                                                         .OrderBy(x => x.Index)
                                                                         .Select(x => $"\n{x.Index}: Type = {x.Type} | GUID: {x.Guid}"));
    }

    private bool CanAddToDropzone(ElementType type)
    {
        if (type == ElementType.Regular)
            return true;

        return _dropzoneItems.All(x => x.Type != type);
    }

    private bool ValidDropIndex(int index)
    {
        Logger.LogInformation("Index = {index} | header present = {header} | footer present = {footer}", index, DropzoneContainsHeader, DropzoneContainsFooter);
        if (index == 0 && DropzoneContainsHeader)
            return false;
        if (index == _dropzoneItems.Count - 1 && DropzoneContainsFooter)
            return false;

        return true;
    }

    private void AddItemToDropzone(DropItem item)
    {
        if (item.Type == ElementType.Regular)
        {
            if (DropzoneContainsFooter)
                _dropzoneItems.Insert(_dropzoneItems.Count - 1, item);
            else
                _dropzoneItems.Add(item);
        }
        else if (item.Type == ElementType.Header)
            _dropzoneItems.Insert(0, item);
        else
            _dropzoneItems.Insert(_dropzoneItems.Count, item);
    }

#region UTILITY

    /// <param name="startIndex">Default is 0</param>
    /// <param name="ordered">If true sorts collection in ascending index order.</param>
    /// <returns>Collection of items in builder area starting from <paramref name="startIndex"/></returns>
    private IEnumerable<DropItem> ItemsInBuilderArea(int startIndex = 0, bool ordered = false)
    {
        var items = _dropzoneItems.Where(x => x.DropZoneIdentifier == AREA_NAME_BUILDER && x.Index >= startIndex);
        if (ordered)
            items = items.OrderBy(x => x.Index);

        return items;
    }

    private bool DropzoneContainsHeader => _dropzoneItems[0].Type == ElementType.Header;

    private bool DropzoneContainsFooter => _dropzoneItems[^1].Type == ElementType.Footer;

    private void RefreshDropContainer()
    {
        StateHasChanged();
        _dropContainerRef.Refresh();
    }

#endregion

    private async void OnGenerateClick()
    {
        var elements = new List<PdfElementBase>
        {
            new HeaderElement(),
            new TextFieldElement()
        };

        await PdfService.GenerateAndDisplayPdf(elements);
    }

    private void OnDeleteBuilderItemClick(DropItem item)
    {
        _dropzoneItems.Remove(item);
        RefreshDropContainer();
    }

    private void OnDuplicateBuilderItemClick(DropItem item)
    {
        //TODO in future the duplicate interaction should duplicate all current block card values set by user
        var copy = item.CreateCopy();
        AddItemToDropzone(copy);
        RefreshDropContainer();
    }
}