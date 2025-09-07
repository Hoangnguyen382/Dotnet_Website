using System.Net.Http.Json;
using Layout_Client.Models.DTO;
using Layout_Client.Models.DTOs;
using Layout_Client.Model.DTO;
using Layout_Client.Service;

namespace Layout_Client.Service
{
    public class PromoCodeService
    {
        private readonly AuthHttpClientFactory _factory;

        public PromoCodeService(AuthHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<PromoCodeResponseDTO>> GetAllAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<PromoCodeResponseDTO>>($"api/PromoCode?restaurantId={restaurantId}") ?? new();
        }
        public async Task<List<PromoCodeResponseDTO>> GetAllPromoCodeAsync(int restaurantId)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<List<PromoCodeResponseDTO>>($"api/PromoCode/public?restaurantId={restaurantId}") ?? new();
        }
        public async Task<PromoValidateResponse?> ValidatePromoAsync(string code, int restaurantId, decimal totalAmount, int totalQuantity)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.GetAsync(
                $"api/PromoCode/validate-promo?code={code}&restaurantId={restaurantId}&totalAmount={totalAmount}&totalQuantity={totalQuantity}"
            );
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<PromoValidateResponse>();
        }
        public async Task<PromoCodeResponseDTO?> GetByIdAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<PromoCodeResponseDTO>($"api/PromoCode/{id}");
        }
    }
}
