﻿using Microsoft.JSInterop;
using MudBlazor;
using PdfApp.Shared.Models.PdfBuilder;
using PdfApp.Shared.Models.PdfBuilder.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace PdfApp.Shared.Services;

public class PdfService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<PdfService> _logger;

    public PdfService(IJSRuntime jsRuntime, ILogger<PdfService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task GenerateAndDisplayPdf(IEnumerable<PdfElement> pdfElements)
    {
        byte[] pdfBytes = GeneratePdf(pdfElements);
        string pdfBase64 = Convert.ToBase64String(pdfBytes);

        var pdfUtilityJsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/pdfServiceUtility.js");
        await pdfUtilityJsModule.InvokeVoidAsync("openPdfInNewTab", pdfBase64);

    }

    public byte[] GeneratePdf(IEnumerable<PdfElement> pdfElements)
    {
        _logger.LogInformation("PreparePdfDocument started");
        IContainer? contentContainer = default;
        var document = Document.Create(handler =>
        {
            handler.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(40);
                page.MarginVertical(60);

                page.Header()
                    .Height(60)
                    .Background(Colors.Grey.Lighten1)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(text =>
                    {
                        text.Span("Header")
                            .FontSize(40)
                            .Superscript();
                    });
                    
                page.Footer().Text("Footer");

                contentContainer = page.Content();
                foreach (var element in pdfElements)
                {
                    element.Draw(contentContainer);
                }
            });
        });
        
        _logger.LogInformation("PreparePdfDocument finished");
        return document.GeneratePdf();
    }
}