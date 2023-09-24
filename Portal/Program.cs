using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portal;
using Portal.Authentication;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddSingleton<IAPIHelper, APIHelper>();
builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();
builder.Services.AddScoped<IProductEndpoint, ProductEndpoint>();
builder.Services.AddScoped<IUserEndpoint, UserEndpoint>();
builder.Services.AddScoped<ISaleEndpoint, SaleEndpoint>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
