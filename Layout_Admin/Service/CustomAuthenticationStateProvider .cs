using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Layout_Admin.Service
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            ClaimsPrincipal user;
            if (string.IsNullOrEmpty(token))
            {
                user = new ClaimsPrincipal(new ClaimsIdentity());
            }
            else
            {
                try
                {
                    var claims = ParseClaimsFromJwt(token);
                    var expClaim = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                    if (expClaim != null && long.TryParse(expClaim, out var exp))
                    {
                        var expDate = DateTimeOffset.FromUnixTimeSeconds(exp);
                        if (expDate < DateTimeOffset.UtcNow)
                        {
                            // Token hết hạn
                            await _localStorage.RemoveItemAsync("authToken");
                            user = new ClaimsPrincipal(new ClaimsIdentity());
                            return new AuthenticationState(user);
                        }
                    }
                    var identity = new ClaimsIdentity(claims, "jwt");
                    user = new ClaimsPrincipal(identity);
                }
                catch
                {
                    user = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}