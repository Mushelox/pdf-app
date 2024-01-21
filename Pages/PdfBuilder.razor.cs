using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PdfApp.Pages;

public partial class PdfBuilder
{
    [Inject]
    private ILogger<PdfBuilder> Logger { get; init; } = null!;

    private const string AREA_NAME_TEMPLATES = "templates";
    private const string AREA_NAME_BUILDER = "builder";

    private List<DropItem> _dropzoneItems = new()
    {
        new DropItem
            { Order = 1, Name = "Element X", Identifier = "templates" },
        new DropItem
            { Order = 2, Name = "Element Y", Identifier = "templates" },
        new DropItem
            { Order = 3, Name = "Element Z", Identifier = "templates" }
    };


    private void ItemUpdated(MudItemDropInfo<DropItem> dropItemInfo)
    {
        var item = dropItemInfo.Item;
        if (item == null)
        {
            Logger.LogError("Dropped item is NULL");
            return;
        }

        item.Identifier = dropItemInfo.DropzoneIdentifier;
        item.Order = dropItemInfo.IndexInZone;

        foreach (var dropItem in GetItemsInBuilderArea())
        {
            if (dropItem == item) continue;
            dropItem.Order++;
        }

        var orderedItemsInBuilderZone = GetItemsInBuilderArea(true);
        Logger.LogInformation("Item dropped. Order of items in in the builder area:\n{itemsInOrder}",
                              string.Join("\n", orderedItemsInBuilderZone.Select(item => $"{item.Order}: {item.Name}")));
    }

#region UTILITY

    private IEnumerable<DropItem> GetItemsInBuilderArea(bool ordered = false)
    {
        var items = _dropzoneItems.Where(x => x.Identifier == AREA_NAME_BUILDER);
        if (ordered)
            items = items.OrderBy(x => x.Order);

        return items;
    }

#endregion

    public class DropItem
    {
        public int Order { get; set; }

        public string Name { get; init; }

        public string Identifier { get; set; }
    }
}