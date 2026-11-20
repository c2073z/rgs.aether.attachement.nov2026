// SORTLE WEBSITE  -  Program.cs   (built by the AI agent)
// Starts Blazor and runs App.razor inside index.html (the <div id="app">).

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SortleWeb;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
await builder.Build().RunAsync();
