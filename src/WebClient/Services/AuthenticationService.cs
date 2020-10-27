using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebClient.Model.Dto;
using WebClient.Providers;

namespace WebClient.Services
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
        Task Logout();
        Task<string> RefreshToken();
    }

	public class AuthenticationService : IAuthenticationService
    {
        private const string _applicationJson = "application/json";
		private readonly HttpClient _client;
		private readonly AuthenticationStateProvider _authStateProvider;
		private readonly ILocalStorageService _localStorage;

		public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider,
			ILocalStorageService localStorage)
		{
			_client = client;
			_authStateProvider = authStateProvider;
			_localStorage = localStorage;
		}

		public async Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration)
		{
			var content = JsonSerializer.Serialize(userForRegistration);
			var bodyContent = new StringContent(content, Encoding.UTF8, _applicationJson);

			HttpResponseMessage registrationResult = await _client.PostAsync("api/user/registration", bodyContent);
			string registrationContent = await registrationResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			
            if (!registrationResult.IsSuccessStatusCode)
                return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

			return new RegistrationResponseDto { IsSuccessfulRegistration = true };
		}

		public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
		{
			var content = JsonSerializer.Serialize(userForAuthentication);
			var bodyContent = new StringContent(content, Encoding.UTF8, _applicationJson);

			HttpResponseMessage authResult = await _client.PostAsync("api/user/login", bodyContent);
			string authContent = await authResult.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (!authResult.IsSuccessStatusCode)
				return result;

			await _localStorage.SetItemAsync("authToken", result.Token);
			await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
			((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

			return new AuthResponseDto { IsAuthSuccessful = true };
		}

		public async Task<string> RefreshToken()
		{
			var token = await _localStorage.GetItemAsync<string>("authToken");
			var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

			var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
			var bodyContent = new StringContent(tokenDto, Encoding.UTF8, _applicationJson);

			var refreshResult = await _client.PostAsync("api/token/refresh", bodyContent);
			var refreshContent = await refreshResult.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (!refreshResult.IsSuccessStatusCode)
				throw new ApplicationException("Something went wrong during the refresh token action");

			await _localStorage.SetItemAsync("authToken", result.Token);
			await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

			return result.Token;
		}

		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			await _localStorage.RemoveItemAsync("refreshToken");
			((AuthStateProvider)_authStateProvider).NotifyUserLogout();
			_client.DefaultRequestHeaders.Authorization = null;
		}
	}
}
