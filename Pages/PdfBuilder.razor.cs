using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace PdfApp.Pages;

public partial class PdfBuilder
{
    [Inject]
    private ILogger<PdfBuilder> Logger { get; init; } = null!;

    private const string AREA_NAME_TEMPLATES = "templates";
    private const string AREA_NAME_BUILDER = "builder";

    private List<DropItem> DefaultTemplateItems => new()
    {
        new DropItem { Index = 1, Name = "Element X", Identifier = "templates" },
        new DropItem { Index = 2, Name = "Element Y", Identifier = "templates" },
        new DropItem { Index = 3, Name = "Element Z", Identifier = "templates" }
    };

    private List<DropItem> _dropzoneItems;

    protected override void OnInitialized()
    {
        _dropzoneItems = DefaultTemplateItems;
    }

    private void ItemUpdated(MudItemDropInfo<DropItem> dropItemInfo)
    {
        var droppedItem = dropItemInfo.Item!;
        // Logger.LogInformation("DropItemInfo.OrderInZone = {index}", dropItemInfo.IndexInZone);

        if (droppedItem.Identifier == AREA_NAME_TEMPLATES)
        {
            droppedItem = new DropItem
            {
                Identifier = dropItemInfo.DropzoneIdentifier,
                Index = dropItemInfo.IndexInZone,
                Name = droppedItem.Name,
                Guid = droppedItem.Guid
            };
            _dropzoneItems.Add(droppedItem);
        }
        else
        {
            droppedItem.Index = dropItemInfo.IndexInZone;
            // foreach (var itemInCollection in ItemsInBuilderArea().Where(x => x.Index >= droppedItem.Index && x != droppedItem))
            // {
            //     if (itemInCollection.Guid.Equals(droppedItem.Guid))
            //     {
            //         Logger.LogInformation("GUIDs the same, skipping");
            //         continue;
            //     }
            //
            //     itemInCollection.Index++;
            // }
        }

        var itemsWhichRequireIndexIncrease = ItemsInBuilderArea(droppedItem.Index, true).ToList();
        itemsWhichRequireIndexIncrease.Remove(droppedItem);
        
        if (itemsWhichRequireIndexIncrease.Count > 0)
            Logger.LogInformation("Items which require index update:{items}",
                                  itemsWhichRequireIndexIncrease.Select(x => $"\n{x.Name}: currentIndex = {x.Index} || GUID = {x.Guid}"));
        
        foreach (var item in itemsWhichRequireIndexIncrease)
            item.Index++;
        

        RestoreDefaultTemplates();

        var orderedItemsInBuilderZone = ItemsInBuilderArea(ordered: true);
        Logger.LogInformation("Item dropped. Items order:\n{itemsInOrder}",
                              string.Join("\n", orderedItemsInBuilderZone.Select(item => $"{item.Index}: {item.Name}")));
    }

    private void RestoreDefaultTemplates()
    {
        _dropzoneItems.RemoveAll(x => x.Identifier == AREA_NAME_TEMPLATES);
        _dropzoneItems.AddRange(DefaultTemplateItems);

        // Logger.LogInformation("Items in templates area:\n{info}", 
        //                       _dropzoneItems.Where(x => x.Identifier == AREA_NAME_TEMPLATES)
        //                                     .Select(x => $"{x.Index}: {x.Name}"));
    }
    
    private void OnTemplateBlockClick(string cardType)
    {
        //TODO update param from string to object which holds info about building block
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

    public class DropItem
    {
        public int Index { get; set; }

        public string Name { get; init; }

        public string Identifier { get; set; }

        public Guid Guid = Guid.NewGuid();
    }
}