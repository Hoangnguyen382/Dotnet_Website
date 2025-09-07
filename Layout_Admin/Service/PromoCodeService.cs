using System.Net.Http.Json;
using Layout_Admin.Models.DTO;
using Layout_Admin.Models.DTOs;
using Layout_Admin.Service;

namespace Layout_Admin.Service
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

        public async Task<PromoCodeResponseDTO?> GetByIdAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            return await client.GetFromJsonAsync<PromoCodeResponseDTO>($"api/PromoCode/{id}");
        }

        public async Task<bool> CreateAsync(int restaurantId, PromoCodeRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PostAsJsonAsync($"api/PromoCode?restaurantId={restaurantId}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, PromoCodeRequestDTO dto)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.PutAsJsonAsync($"api/PromoCode/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _factory.CreateClientAsync();
            var response = await client.DeleteAsync($"api/PromoCode/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
