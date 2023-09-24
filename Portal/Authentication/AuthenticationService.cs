using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Portal.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TRMDesktopUILibrary.Models;

namespace Portal.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;
        private readonly string _authTokenStorageKey;

        public AuthenticationService(HttpClient httpClient,
                                     AuthenticationStateProvider authenticationStateProvider,
                                     ILocalStorageService localStorage, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _configuration = configuration;
            _authTokenStorageKey = _configuration["authTokenStorageKey"];
        }

        public async Task<AuthenticatedUserModel> LogIn(AuthenticationUserModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userForAuthentication.Email),
                new KeyValuePair<string, string>("password", userForAuthentication.Password)
            });

            var authTokenUri = _configuration["api"] + _configuration["authTokenEndpoint"];
            var authResult = await _httpClient.PostAsync(authTokenUri, data);
            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode == false)
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                authContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await _localStorage.SetItemAsync(_authTokenStorageKey, result.Access_Token);

            ((AuthStateProvider)_authenticationStateProvider).NotifyUserAuthentication(result.Access_Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

            return result;
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(_authTokenStorageKey);
            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
