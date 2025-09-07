using System.Net.Http.Json;
using Layout_Client.Model.DTO;

public class PaymentService
{
    private readonly HttpClient _http;
    public PaymentService(HttpClient http) => _http = http;

    public async Task<MomoPaymentResponseDTO?> CreateMomoPaymentAsync(OrderInfoDTO order)
    {
        var response = await _http.PostAsJsonAsync("api/Payment/momo", order);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<MomoPaymentResponseDTO>();
    }
}
