using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.FromLogContext()
             .CreateLogger();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();