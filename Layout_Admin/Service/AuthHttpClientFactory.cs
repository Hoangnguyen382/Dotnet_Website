using System.Net.Http.Headers;
using Blazored.LocalStorage;
namespace Layout_Admin.Service
{
    public class AuthHttpClientFactory
    {
        private readonly ILocalStorageService _localStorage;

        public AuthHttpClientFactory(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5176") };
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
    }
}
