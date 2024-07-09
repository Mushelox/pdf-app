﻿namespace PdfApp.Shared.Models.PdfBuilderElements.ElementSettings.Interfaces;

public interface IAreaSettings
{
    public string BackgroundColorHex { get; set; }

    public int Height { get; set; }
    
    public int Width { get; set; }
}