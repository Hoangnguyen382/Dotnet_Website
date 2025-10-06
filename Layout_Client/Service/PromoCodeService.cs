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
        public async Task<ValidatePromoDTO> ValidatePromoCodeAsync(string code, int restaurantId, decimal totalAmount, int totalQuantity)
        {
            var client = await _factory.CreateClientAsync();
            var url = $"api/PromoCode/validate-promo?code={Uri.EscapeDataString(code)}&restaurantId={restaurantId}&totalAmount={(int)totalAmount}&totalQuantity={totalQuantity}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ValidatePromoDTO>();
                return new ValidatePromoDTO
                {
                    Error = errorResult?.Error ?? "Mã giảm giá không hợp lệ."
                };
            }
            var result = await response.Content.ReadFromJsonAsync<ValidatePromoDTO>();
            if (result == null)
            {
                return new ValidatePromoDTO
                {
                    Error = "Không nhận được dữ liệu từ server."
                };
            }

            return result;
        }



        public async Task<PromoCodeResponseDTO?> GetByIdAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<PromoCodeResponseDTO>($"api/PromoCode/{id}");
        }
        private class PromoResult
        {
            public decimal discount { get; set; }
            public int? promoCodeId { get; set; }
        }
    }
}
