using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Validation69530.Client.Validation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddValidation();
builder.Services.AddScoped<UsernameAvailabilityService>();
await builder.Build().RunAsync();
