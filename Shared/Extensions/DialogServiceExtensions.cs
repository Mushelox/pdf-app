using MudBlazor;
using PdfApp.Shared.Dialogs;

namespace PdfApp.Shared.Extensions;

public static class DialogServiceExtensions
{
    public async static Task ShowInfoDialog(this IDialogService dialogService, string titleText, string contentText)
    {
        var parameters = new DialogParameters<InfoDialog>
        {
            { x => x.ContentText, contentText }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            CloseOnEscapeKey = true
        };

        await dialogService.ShowAsync<InfoDialog>(titleText, parameters, options);
    }
}