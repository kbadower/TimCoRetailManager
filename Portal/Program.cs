using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portal.Authentication;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace Portal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            builder.Services.AddSingleton<IAPIHelper, APIHelper>();
            builder.Services.AddSingleton<ILoggedInUserModel, LoggedInUserModel>();
            builder.Services.AddTransient<IProductEndpoint, ProductEndpoint>();
            builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
            builder.Services.AddTransient<ISaleEndpoint, SaleEndpoint>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}