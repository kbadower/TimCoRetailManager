﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using TRMDesktopUILibrary.Api;

namespace Portal.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration configuration, IAPIHelper apiHelper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _configuration = configuration;
            _apiHelper = apiHelper;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        } 

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authTokenStorageKey = _configuration["authTokenStorageKey"];
            var token = await _localStorage.GetItemAsync<string>(authTokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            bool isAuthenticated = await NotifyUserAuthentication(token);

            if (isAuthenticated == false)
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
                    "jwtAuthType")));
        }

        public async Task<bool> NotifyUserAuthentication(string token)
        {
            bool isAuthenticatedOutput = false;
            Task<AuthenticationState> authState;

            try
            {
                await _apiHelper.GetLoggedInUserInfo(token);
                var authenticatedUser = new ClaimsPrincipal(
        new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
        "jwtAuthType"));
                authState = Task.FromResult(new AuthenticationState(authenticatedUser));
                NotifyAuthenticationStateChanged(authState);
                isAuthenticatedOutput = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NotifyUserLogout();
                isAuthenticatedOutput = false;
            }

            return isAuthenticatedOutput;
        }

        public async Task NotifyUserLogout()
        {
            var authTokenStorageKey = _configuration["authTokenStorageKey"];
            await _localStorage.RemoveItemAsync(authTokenStorageKey);
            var authState = Task.FromResult(_anonymous);
            _apiHelper.LogOffUser();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
