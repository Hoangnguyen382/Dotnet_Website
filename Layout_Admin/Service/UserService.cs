using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Layout_Admin.Model.DTO;
using Microsoft.AspNetCore.Components.Authorization;
namespace Layout_Admin.Service
{
    public class UserService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthHttpClientFactory _factory;
        private readonly AuthenticationStateProvider _authProvider;
        public UserService(HttpClient http, ILocalStorageService localStorage, AuthHttpClientFactory factory, AuthenticationStateProvider authProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _factory = factory;
            _authProvider = authProvider;
        }   

        public async Task<bool> LoginAsync(LoginDTO login)
        {
            var response = await _http.PostAsJsonAsync("login", login);
            if (!response.IsSuccessStatusCode) return false;

            var token = await response.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsync("authToken", token);
            if (_authProvider is CustomAuthenticationStateProvider customProvider)
            {
                customProvider.MarkUserAsAuthenticated(token);
            }
            return true;
        }
        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            if (_authProvider is CustomAuthenticationStateProvider customProvider)
            {
                customProvider.MarkUserAsLoggedOut();
            }
        }

        public async Task<bool> RegisterAsync(UserDTO user)
        {
            var response = await _http.PostAsJsonAsync("register", user);
            return response.IsSuccessStatusCode;
        }
        public async Task<UserResponseDTO?> GetCurrentUserAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
            var restaurantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "RestaurantID")?.Value;
            if (email == null || id == null)
                return null;
            return new UserResponseDTO
            {
                UserID = int.Parse(id),
                Email = email,
                Role = role,
                RestaurantID = int.TryParse(restaurantId, out var rid) ? rid : 0
            };
        }
        public async Task<List<UserResponseDTO>?> GetAllUsersAsync(int restaurantId)
        {
            var response = await _http.GetAsync($"restaurant/{restaurantId}/staffs");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<List<UserResponseDTO>>();
        }
        public async Task<bool> UpdateRoleUserAsync(int userId, string role, UserDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PutAsJsonAsync($"update-role?UserID={userId}&Role={role}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<UserResponseDTO?> CreateStaffAsync(UserDTO staffDto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PostAsJsonAsync("create-staff", staffDto);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<UserResponseDTO>();
        }
    }
}