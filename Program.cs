using Blazored.LocalStorage;
using Markdig;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Website;
using MudBlazor.Services;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
    }).AddSingleton(_ => new MarkdownPipelineBuilder()
        .UseTaskLists()
        .UseEmphasisExtras()
        .Build())
    .AddMudServices()
    .AddBlazoredLocalStorageAsSingleton();

await builder.Build().RunAsync();