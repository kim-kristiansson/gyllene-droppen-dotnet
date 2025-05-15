using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiBaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5088/");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = apiBaseAddress });

await builder.Build().RunAsync();