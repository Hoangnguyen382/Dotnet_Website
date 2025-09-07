    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using Blazored.LocalStorage;
    using Layout_Client.Model.DTO;
    namespace Layout_Client.Service
    {
        public class UserService
        {
            private readonly HttpClient _http;
            private readonly ILocalStorageService _localStorage;

            public UserService(HttpClient http, ILocalStorageService localStorage)
            {
                _http = http;
                _localStorage = localStorage;
            }

            public async Task<bool> LoginAsync(LoginDTO login)
            {
                var response = await _http.PostAsJsonAsync("login", login);
                if (!response.IsSuccessStatusCode) return false;

                var token = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsync("authToken", token);
                return true;
            }

            public async Task LogoutAsync()
            {
                await _localStorage.RemoveItemAsync("authToken");
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
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    return null;
                }
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
                var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

                if (email == null || id == null)
                    return null;

                return new UserResponseDTO
                {
                    UserID = int.Parse(id),
                    Email = email,
                    Role = role
                };
            }
            public async Task<bool> ForgotPasswordAsync(string email)
            {
                var response = await _http.PostAsJsonAsync("forgot-password", new { Email = email });
                return response.IsSuccessStatusCode;
            }
            public async Task<bool> ResetPasswordAsync(ResetPassDTO dto)
            {
                var response = await _http.PostAsJsonAsync("reset-password", dto);
                return response.IsSuccessStatusCode;
            }
        }
    }