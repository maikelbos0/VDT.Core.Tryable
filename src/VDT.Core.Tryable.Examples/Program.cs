using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VDT.Core.Tryable.Examples;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

await builder.Build().RunAsync();
